using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        /// Gets the name of the discard file without the preceding numbering
        /// </summary>
        public string RealName
        {
            get
            {
                string[] split = Source.Name.Split(' ');

                //The name's first part ends with a 'd' and starts with a number if we trim the 'd'
                if (TryParseNumberWithD(split.First(), out int r))
                {
                    return string.Join(" ", split.Skip(1).ToArray());
                }

                //No valid marker found
                return Source.Name;
            }
        }

        /// <summary>
        /// Days before deletion
        /// </summary>
        public int DaysLeft
        {
            get
            {
                string[] split = Source.Name.Split(' ');

                if (TryParseNumberWithD(split.First(), out int r))
                {
                    return r;
                }

                return Program.MAX_DAYS;
            }
            set
            {
                if (Source is DirectoryInfo d)
                {
                    d.MoveTo(d.Parent.FullName + "\\" + CreateNumberWithD(value) + " " + RealName);
                }
                else if (Source is FileInfo f)
                {
                    f.MoveTo(f.Directory.FullName + "\\" + CreateNumberWithD(value) + " " + RealName);
                }
            }
        }

        /// <summary>
        /// Has this file expired
        /// </summary>
        public bool Expired
        {
            get => DaysLeft <= 0;
        }

        public DiscardFile(FileSystemInfo fileOrFolder)
        {
            //Sets the source
            Source = fileOrFolder;
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
                recursiveSetAttr(d.FullName);
                d.Delete(true);
            }


            /* local */ void recursiveSetAttr(string path)
            {
                //Update file flags
                foreach (string i in Directory.GetFiles(path))
                {
                    try
                    {
                        File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("[ERROR] Failed to update attribute flag for " + path);
                    }
                }

                //Recursive search subdirectories
                foreach (string i in Directory.GetDirectories(path))
                {
                    recursiveSetAttr(i);
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

        private bool TryParseNumberWithD(string toParse, out int result)
        {
            if (!toParse.EndsWith("d"))
            {
                result = -1;
                return false;
            }

            return int.TryParse(toParse.TrimEnd('d'), out result);
        }

        private string CreateNumberWithD(int number)
        {
            return number.ToString() + "d";
        }
    }
}
