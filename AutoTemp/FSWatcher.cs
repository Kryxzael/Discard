using System;
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

                w.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.DirectoryName | NotifyFilters.FileName;

                w.Changed += EvFileChanged;
                w.Created += EvFileCreated;
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
                new FileInfo(GetTrackerFileName(e.OldFullPath)).MoveTo(GetTrackerFileName(e.FullPath));
            }
            catch (Exception)
            {
                EvFileCreated(sender, e);
                Console.WriteLine("Unable to rename tracker file of former file '" + e.OldName + "'");
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
                new FileInfo(GetTrackerFileName(e.FullPath)).Delete();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to delete trailing tracker file for " + e.Name);
            }
            
        }

        private void EvFileCreated(object sender, FileSystemEventArgs e)
        {
            if (Path.GetExtension(e.FullPath) == ".discard")
            {
                return;
            }

            //Create tracker file
            //DRY RUN FIRST PLEASE!!!!!!
            //FOR THE LOVE OF JESUS CHRIST!!!


            new TrackingFile(new FileInfo(GetTrackerFileName(e.FullPath))).NoWarning = false; //The "no-warning" setting is only used to actually write a file
        }

        private void EvFileChanged(object sender, FileSystemEventArgs e)
        {
            if (Path.GetExtension(e.FullPath) == ".discard")
            {
                return;
            }

            FileInfo info = new FileInfo(e.FullPath);
            if (info.Attributes.HasFlag(FileAttributes.Hidden))
            {
                //File is made hidden, delete tracker
                EvFileDeleted(sender, e);
            }
            else
            {
                //File is made visible, 
                EvFileCreated(sender, e);
            }
        }

        /// <summary>
        /// Gets the tracker file's name for the given file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static string GetTrackerFileName(string file)
        {
            return file + ".discard";
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
