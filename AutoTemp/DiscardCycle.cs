﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discard
{
    /// <summary>
    /// Represents a cleanup operation done daily by discard
    /// </summary>
    public class DiscardCycle
    {
        /// <summary>
        /// The files that the discard cycle will run through
        /// </summary>
        public List<DiscardFile> DiscardFiles { get; }

        /// <summary>
        /// Gets the amount of cycles (days since last cycle) this DiscardCycle will run
        /// </summary>
        public int Cycles { get; }

        /// <summary>
        /// Is the application currently showing a dialog window
        /// </summary>
        private static bool IsRunning { get; set; }

        private DiscardCycle(IEnumerable<DirectoryInfo> path, int cycles)
        {
            //don't ask how this works please
            DiscardFiles = path.Select(o => o.GetFileSystemInfos()
                .Where(i => !i.Attributes.HasFlag(FileAttributes.Hidden) && i.Extension != ".discard")
                .Select(i => new DiscardFile(i)))
                .SelectMany(i => i)
            .ToList();

            Cycles = cycles;
        }

        /// <summary>
        /// Creates a new discard cycle containing the information it would contain were it to run
        /// </summary>
        /// <param name="where"></param>
        public static DiscardCycle DryRun(IEnumerable<DirectoryInfo> where, int cycles)
        {
            return new DiscardCycle(where, cycles);
        }

        /// <summary>
        /// Runs a discard cycle now on the given directory
        /// </summary>
        public static bool RunNow(IEnumerable<DirectoryInfo> where, int cycles)
        {
            //Do not show dialog if the application is already running
            if (IsRunning)
            {
                Console.WriteLine("Discard was already running a cycle and will not run again");
                return false;
            }

            IsRunning = true;

            DiscardCycle _ = new DiscardCycle(where, cycles);

            //Updates file labels
            foreach (DiscardFile i in _.DiscardFiles)
            {
                try
                {
                    i.DaysLeft -= cycles;
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not update file label. File might be locked.");
                }
            }

            //Shows the dialog for the files that are to be deleted
            IEnumerable<DiscardFile> filesToPrompt = _.DiscardFiles.Where(i => i.Expired && !i.NoWarning);
            List<DiscardFile> filesForDeletion = _.DiscardFiles.Where(i => i.NoWarning && i.Expired).ToList();

            if (filesToPrompt.Any())
            {
                DiscardDialogFull dia = new DiscardDialogFull(filesToPrompt);
                dia.ShowDialog();

                if (dia.DialogResult == DialogResult.Cancel)
                {
                    IsRunning = false;
                    return false;
                }

                filesForDeletion.AddRange(dia.GetFilesForDeletion());

                //Postpone files
                foreach (DiscardFile i in dia.GetFilesForPostponement())
                {
                    try
                    {
                        /* I have decided against actually updating the file labels. Instead just let it go negative */
                        //i.Postpone();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("The file/folder " + DiscardFile.GetRealName(i.Source.Name) + " could not be updated. It might be in use\r\n" + ex.Message, "Discard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }

            //Delete files
            foreach (DiscardFile i in filesForDeletion)
            {
                try
                {
                    i.Delete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The file/folder " + DiscardFile.GetRealName(i.Source.Name) + " could not be deleted. It might be in use\r\n" + ex.Message, "Discard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            IsRunning = false;
            return true;
        }
    }
}
