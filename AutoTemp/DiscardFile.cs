using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Discard
{
    /// <summary>
    /// Represents a file in a discard folder
    /// </summary>
    public class DiscardFile
    {
        /// <summary>
        /// Gets the filesysteminfo object that represents this file/folder
        /// </summary>
        public FileSystemInfo Source { get; }

        /// <summary>
        /// Gets the tracker file for this discard file
        /// </summary>
        public TrackingFile Tracker { get; }

        /// <summary>
        /// Gets the name of the discard file without the preceding numbering
        /// </summary>
        public string Name => Source.Name;

        /// <summary>
        /// Does this discard file not have a tracker?
        /// </summary>
        public bool Untracked => !Tracker.Source.Exists;

        /// <summary>
        /// Has this file expired
        /// </summary>
        public bool Expired => Tracker.Expired;

        /// <summary>
        /// Days before deletion
        /// </summary>
        public int DaysLeft
        {
            get => Tracker.DaysLeft;
            set => Tracker.DaysLeft = value;
        }

        /// <summary>
        /// Will this file be deleted without warning
        /// </summary>
        public bool NoWarning
        {
            get => Tracker.NoWarning;
            set => Tracker.NoWarning = value;
        }

        public DiscardFile(FileSystemInfo fileOrFolder)
        {
            //Sets the sources
            Source = fileOrFolder;
            Tracker = new TrackingFile(new FileInfo(Source.FullName + ".discard"));
        }

        /// <summary>
        /// Deletes the file or directory
        /// </summary>
        /// <returns></returns>
        public void Delete()
        {
            if (Source is FileInfo f)
            {
                File.SetAttributes(f.FullName, File.GetAttributes(f.FullName) & ~FileAttributes.ReadOnly);
                f.Delete();
            }
            else if (Source is DirectoryInfo d)
            {
                recursiveSetAttr(d);
                d.Delete(true);
            }


            /* local */ void recursiveSetAttr(DirectoryInfo dir)
            {
                try
                {
                    dir.Attributes = FileAttributes.Normal;
                }
                catch (Exception)
                {
                    Console.Error.WriteLine("recursiveSetAttr: Could not update flag of directory " + dir.Name);
                }
                

                foreach (FileSystemInfo i in dir.GetFileSystemInfos())
                {
                    if (i is DirectoryInfo d)
                    {
                        recursiveSetAttr(d);
                    }
                    else if (i is FileInfo fi)
                    {
                        try
                        {
                            fi.Attributes = FileAttributes.Normal;
                        }
                        catch (Exception)
                        {
                            Console.Error.WriteLine("recursiveSetAttr: Could not update flag of file " + i.Name);
                        }
                        
                    }
                }
            }
        }

        /// <summary>
        /// Archives the file or folder
        /// </summary>
        /// <param name="target">Location to archive</param>
        public void Archive(string target)
        {
            if (Source is FileInfo f)
            {
                f.MoveTo(target);
            }
            else if (Source is DirectoryInfo d)
            {
                d.MoveTo(target);
            }
        }

        /// <summary>
        /// Postpones the file by one day if it is set to expire
        /// </summary>
        public void Postpone()
        {
            if (Expired)
            {
                DaysLeft = 1;
            }
        }
    }
}
