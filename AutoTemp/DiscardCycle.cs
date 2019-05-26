using System;
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
        /// Is the application currently showing a dialog window
        /// </summary>
        private static bool IsRunning { get; set; }

        private DiscardCycle(IEnumerable<DirectoryInfo> path)
        {
            //don't ask how this works please
            DiscardFiles = path.Select(o => o.GetFileSystemInfos()
                .Where(i => !i.Attributes.HasFlag(FileAttributes.Hidden))
                .Select(i => new DiscardFile(i)))
                .SelectMany(i => i)
            .ToList();

        }

        /// <summary>
        /// Creates a new discard cycle containing the information it would contain were it to run
        /// </summary>
        /// <param name="where"></param>
        public static DiscardCycle DryRun(IEnumerable<DirectoryInfo> where)
        {
            return new DiscardCycle(where);
        }

        /// <summary>
        /// Runs a discard cycle now on the given directory
        /// </summary>
        public static bool RunNow(IEnumerable<DirectoryInfo> where)
        {
            //Do not show dialog if the application is already running
            if (IsRunning)
            {
                Console.WriteLine("Discard was already running a cycle and will not run again");
                return false;
            }

            IsRunning = true;

            DiscardCycle _ = new DiscardCycle(where);

            //Updates file labels
            foreach (DiscardFile i in _.DiscardFiles)
            {
                try
                {
                    i.DaysLeft--;
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not update file label. File might be locked.");
                }
            }

            //Shows the dialog for the files that are to be deleted
            IEnumerable<DiscardFile> filesToPrompt = _.DiscardFiles.Where(i => i.Expired && !i.NoWarning);

            if (filesToPrompt.Any())
            {
                DiscardDialogFull dia = new DiscardDialogFull(filesToPrompt);
                dia.ShowDialog();

                if (dia.DialogResult == DialogResult.Cancel)
                {
                    IsRunning = false;
                    return false;
                }

                //Delete files
                foreach (DiscardFile i in dia.GetFilesForDeletion().Union(_.DiscardFiles.Where(i => i.Expired && i.NoWarning)))
                {
                    try
                    {
                        i.Delete();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("The file/folder " + i.RealName + " could not be deleted. It might be in use\r\n" + ex.Message, "Discard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                //Postpone files
                foreach (DiscardFile i in dia.GetFilesForPostponement())
                {
                    try
                    {
                        i.Postpone();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("The file/folder " + i.RealName + " could not be updated. It might be in use\r\n" + ex.Message, "Discard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }

            IsRunning = false;
            return true;
        }
    }
}
