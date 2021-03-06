﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        /// Gets the file that contains this discard file's counter
        /// </summary>
        public FileSystemInfo CounterFile
        {
            get
            {
                if (Source.Extension == ".discard")
                {
                    return Source;
                }

                foreach (DiscardFile i in new DirectoryInfo(Path.GetDirectoryName(Source.FullName)).GetFiles().Select(i => new DiscardFile(i)))
                {
                    if (GetRealName(i.Source.Name).ToLower() == Source.Name.ToLower() + ".discard")
                    {
                        return i.Source;
                    }                    
                }

                return Source;
            }
        }

        /// <summary>
        /// Has this file not been run through the system yet?
        /// </summary>
        public bool Untracked
        {
            get
            {
                DeconstructFileName(CounterFile.Name, out int _, out bool _, out string _, out bool untracked);
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
                DeconstructFileName(CounterFile.Name, out int days, out bool _, out string _, out bool _);
                return days;
            }
            set
            {
                if (CounterFile is DirectoryInfo d)
                {
                    d.MoveTo(d.Parent.FullName + "\\" + ConstructFileName(value, NoWarning, GetRealName(CounterFile.Name)));
                }
                else if (CounterFile is FileInfo f)
                {
                    f.MoveTo(f.Directory.FullName + "\\" + ConstructFileName(value, NoWarning, GetRealName(CounterFile.Name)));
                }
            }
        }

        /// <summary>
        /// Is this file a no-warn file
        /// </summary>
        public bool NoWarning
        {
            get
            {
                DeconstructFileName(CounterFile.Name, out int _, out bool nowarn, out string _, out bool _);
                return nowarn;
            }
            set
            {
                if (CounterFile is DirectoryInfo d)
                {
                    d.MoveTo(d.Parent.FullName + "\\" + ConstructFileName(DaysLeft, value, GetRealName(d.Name)));
                }
                else if (CounterFile is FileInfo f)
                {
                    f.MoveTo(f.Directory.FullName + "\\" + ConstructFileName(DaysLeft, value, GetRealName(f.Name)));
                }
            }
        }

        /// <summary>
        /// Does this file use an external counter system?
        /// </summary>
        public bool HasExternalCounter
        {
            get => CounterFile != Source;
            set => throw new NotImplementedException();
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

        /// <summary>
        /// Creates a counter file of this discard file and transfers all the information from this file to that counter
        /// </summary>
        public void CreateCounterFile()
        {
            if (HasExternalCounter)
            {
                return;
            }

            try
            {
                File.Create(Path.Combine(Path.GetDirectoryName(Source.FullName), ConstructFileName(DaysLeft, NoWarning, Source.Name) + ".discard")).Close();

                (Source as FileInfo)?.MoveTo(
                        Path.Combine(Path.GetDirectoryName(Source.FullName), GetRealName(Source.Name))
                 );

                (Source as DirectoryInfo)?.MoveTo(
                    Path.Combine((Source as DirectoryInfo).Parent.FullName, GetRealName(Source.Name))
                );
            }
            catch (Exception ex)
            {
                //This is so stupid.
                //So apparently, Windows throws an IOException if the target and dest. names are identical
                //This also seems to ONLY happen with folders. Wat
                if (ex.HResult == -2146232800) 
                    return;

                MessageBox.Show("Unable to fully create counter file. This can mean that:\n\n* The counter file was created but the counter of the original file could not be removed\n* The counter file could not be created\n\nYou need to manually fix this", "Creation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void MergeCounterFile()
        {
            if (!HasExternalCounter)
            {
                return;
            }

            try
            {
                FileInfo oldCounterFile = CounterFile as FileInfo;
                (Source as FileInfo)?.MoveTo(Path.Combine(Path.GetDirectoryName(Source.FullName), ConstructFileName(DaysLeft, NoWarning, GetRealName(Source.Name))));
                (Source as DirectoryInfo)?.MoveTo(Path.Combine(Path.GetDirectoryName(Source.FullName), ConstructFileName(DaysLeft, NoWarning, GetRealName(Source.Name))));

                
                oldCounterFile.Delete();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to fully merge counter file. This can mean that:\n\n* The counter file still remains even though it's not in use\n* No action could take place\n\nYou need to manualy fix this", "Merging error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        /// <summary>
        /// Gets the name portion of a discard file
        /// </summary>
        public static string GetRealName(string fileName)
        {
            DeconstructFileName(fileName, out int _, out bool _, out string name, out bool _);
            return name;
        }
    }
}
