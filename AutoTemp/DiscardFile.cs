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
        /// Gets the name of the discard file without the preceding numbering
        /// </summary>
        public string RealName
        {
            get
            {
                DeconstructFileName(Source.Name, out int _, out bool _, out string name, out bool _);
                return name;
            }
        }

        public bool Untracked
        {
            get
            {
                DeconstructFileName(Source.Name, out int _, out bool _, out string _, out bool untracked);
                return untracked;
            }
        }

        /// <summary>
        /// Days before deletion
        /// </summary>
        public int DaysLeft
        {
            get
            {
                DeconstructFileName(Source.Name, out int days, out bool _, out string _, out bool _);
                return days;
            }
            set
            {
                if (Source is DirectoryInfo d)
                {
                    d.MoveTo(d.Parent.FullName + "\\" + ConstructFileName(value, NoWarning, RealName));
                }
                else if (Source is FileInfo f)
                {
                    f.MoveTo(f.Directory.FullName + "\\" + ConstructFileName(value, NoWarning, RealName));
                }
            }
        }

        public bool NoWarning
        {
            get
            {
                DeconstructFileName(Source.Name, out int _, out bool nowarn, out string _, out bool _);
                return nowarn;
            }
            set
            {
                if (Source is DirectoryInfo d)
                {
                    d.MoveTo(d.Parent.FullName + "\\" + ConstructFileName(DaysLeft, value, RealName));
                }
                else if (Source is FileInfo f)
                {
                    f.MoveTo(f.Directory.FullName + "\\" + ConstructFileName(DaysLeft, value, RealName));
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

        public static void DeconstructFileName(string input, out int days, out bool noWarn, out string name, out bool untracked)
        {
            string prefix = input.Split(' ').First();
            untracked = false;

            /* 
             * Set the name out variable
             */

            //There is no name (Only prefix)
            if (input.Split(' ').Count() == 1)
            {
                name = "";
            }

            //There is a name (Here a prefix is assumed to exist. If there isn't, it's taken care of later)
            else
            {
                name = string.Join(" ", input.Split(' ').Skip(1));
            }

            /*
             * Parse the prefix
             */

            const string REGEX_NUMBER_D = @"-?\d+d";

            //Format is '7d foo'
            if (Regex.IsMatch(prefix, $"^{REGEX_NUMBER_D}$"))
            {
                days = int.Parse(Regex.Match(prefix, @"-?\d+").Value);
                noWarn = false;
            }

            //Format is '7d! foo' or '!7d foo'
            else if (Regex.IsMatch(prefix, $"^{REGEX_NUMBER_D}!$") || Regex.IsMatch(prefix, $"^!{REGEX_NUMBER_D}$"))
            {
                days = int.Parse(Regex.Match(prefix, @"-?\d+").Value);
                noWarn = true;
            }

            //Format is '! foo' or '!foo'
            else if (input.StartsWith("!"))
            {
                name = input.TrimStart(' ', '!');
                days = 1;
                noWarn = true;
                untracked = true;
            }

            //No valid prefix
            else
            {
                name = input;
                days = Properties.Settings.Default.DefaultDays;
                noWarn = false;
                untracked = true;
            }
        }

        public static string ConstructFileName(int days, bool noWarn, string name)
        {
            return string.Format("{0}{1}d {2}",
                noWarn ? "!" : "",
                days.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")),
                name
            );
        }
    }
}
