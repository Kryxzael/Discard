﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discard
{
    public class FSWatcher : IDisposable
    {
        private List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();

        public FSWatcher()
        {
            foreach (string i in Program.GetDiscardDirectories())
            {
                FileSystemWatcher w = new FileSystemWatcher(i);

                w.Deleted += EvFileDeleted;
                w.Renamed += EvFileRenamed;

                w.EnableRaisingEvents = true;

                _watchers.Add(w);
            }
        }

        private void EvFileRenamed(object sender, RenamedEventArgs e)
        {
            if (Path.GetExtension(e.OldFullPath) == ".discard")
            {
                return;
            }

            //Rename tracker file
            try
            {
                DiscardFile oldDiscardFile = new DiscardFile(new FileInfo(e.OldFullPath));
                DiscardFile newDiscardFile = new DiscardFile(new FileInfo(e.FullPath));

                if (oldDiscardFile.HasExternalCounter)
                {
                    (oldDiscardFile.CounterFile as FileInfo).MoveTo(
                        Path.Combine(Path.GetDirectoryName(e.FullPath), DiscardFile.ConstructFileName(
                            days: oldDiscardFile.DaysLeft, 
                            noWarn: oldDiscardFile.NoWarning, 
                            name: DiscardFile.GetRealName(newDiscardFile.Source.Name)
                         ) + ".discard"));
                }
            }
            catch (Exception ex)
            {
                //Really hacky way to fix edge case scenario where the warning would trigger when merging files
                //This may actually break stuff idk.
                if (ex.HResult == -2147024894)
                    return;

                System.Windows.Forms.MessageBox.Show("Unable to rename the trailing counter file for former '" + e.OldName + "'. You need to rename it manually");
            }
        }

        private void EvFileDeleted(object sender, FileSystemEventArgs e)
        {
            if (Path.GetExtension(e.FullPath) == ".discard")
            {
                return;
            }

            //Delete tracker file
            try
            {
                DiscardFile discardFile = new DiscardFile(new FileInfo(e.FullPath));

                if (discardFile.HasExternalCounter)
                {
                    discardFile.CounterFile.Delete();
                }


            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Unable to delete the trailing counter file for '" + e.Name + "'. You need to delete it manually");
            }

        }

        public void Dispose()
        {
            foreach (FileSystemWatcher i in _watchers)
            {
                i.Dispose();
            }
        }

        ~FSWatcher()
        {
            Dispose();
        }
    }
}
