using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discard
{
    [Obsolete]
    public class ProgramOld
    {
        /// <summary>
        /// Gets the path of the autotemp directory
        /// </summary>
        public static readonly string TEMP_FOLDER_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Discard";
        public static string FLAG_FILE => Path.Combine(TEMP_FOLDER_PATH, ".FLAG");
        public static readonly CultureInfo C = CultureInfo.GetCultureInfo("en-US");

#if DEBUG
        public const int MAX_DAYS = 1;
        public const bool BYPASS_DAY_CHECK = true;
#else
        public const int MAX_DAYS = 7;
        public const bool BYPASS_DAY_CHECK = false;
#endif

        public const bool AWAIT_INPUT = false;

        public static IEnumerable<string> DiscardDirectories
        {
            get
            {

#if DEBUG
                //yield return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                yield return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Discard2";
#else
                yield return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Discard";
                yield return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\downloads";
#endif
            }
        }

        private static NotifyIcon _balloon = new NotifyIcon();

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            Application.Run(new DiscardDialogFull());
            return;

            _balloon = new NotifyIcon()
            {
                Icon = Properties.Resources.Discard,
            };

            Microsoft.Win32.SystemEvents.PowerModeChanged += (s, e) =>
            {
                if (e.Mode == Microsoft.Win32.PowerModes.Resume)
                {
                    Run();
                }
            };
            Run();
            Application.Run();

        }

        public static void Run()
        {
            Console.WriteLine("Running...");

            if (AppHasBeenRun())
            {
                Console.WriteLine("[WARNING] App has already been run");
                return;
            }

            UpdateTempFiles();
            WriteFlagFile();

            Console.WriteLine("Complete!");
        }

        /// <summary>
        /// Checks if the application has allready been run today by checking last write time of the flag file
        /// </summary>
        /// <returns></returns>
        public static bool AppHasBeenRun()
        {
            if (BYPASS_DAY_CHECK)
            {
                return false;
            }

            try
            {
                FileInfo flagFile = new FileInfo(FLAG_FILE);

                if (!flagFile.Exists)
                {
                    Console.WriteLine("[WARNING] Flag file did not exist, assuming app has not been run");
                    return false;
                }
                else
                {
                    return flagFile.LastWriteTime.Date >= DateTime.Now.Date;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Exception thrown when reading flag file. Is the file locked?");
                return false;
            }
        }

        /// <summary>
        /// Updates the flag file
        /// </summary>
        /// <returns></returns>
        public static void WriteFlagFile()
        {
            try
            {
                File.Delete(FLAG_FILE);
                File.WriteAllBytes(FLAG_FILE, new byte[0]);
                File.SetAttributes(FLAG_FILE, File.GetAttributes(FLAG_FILE) | FileAttributes.Hidden);
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Flag file could not be updated, Is the file locked?");
            }
        }

        public static void UpdateTempFiles()
        {
            try
            {
                string lastFileWarning = null;
                bool multipleWarnings = false;

                foreach (string o in DiscardDirectories)
                {
                    DirectoryInfo dinfo = new DirectoryInfo(o);

                    if (!dinfo.Exists)
                    {
                        Console.WriteLine("[ERROR] A discard directory was not found");
                        continue;
                    }

                    foreach (FileSystemInfo i in dinfo.GetFileSystemInfos().Where(i => !(i.Attributes.HasFlag(FileAttributes.Hidden) || i.Attributes.HasFlag(FileAttributes.System))))
                    {
                        try
                        {
                            CountdownFile countdownFile = new CountdownFile(i);

                            if (countdownFile.DaysLeft != 9999)
                            {
                                countdownFile.DaysLeft--;
                            }
                            
                            countdownFile.UpdateFile();

                            if (countdownFile.DaysLeft <= 0)
                            {
                                countdownFile.Delete();
                            }
                            else if (countdownFile.DaysLeft == 1)
                            {
                                multipleWarnings = lastFileWarning != null;
                                lastFileWarning = countdownFile.RealName;
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("[ERROR] Could not update file: " + i.Name);
                        }

                    }
                }
                

                if (multipleWarnings)
                {
                    _balloon.Visible = true;
                    _balloon.ShowBalloonTip(10000, "Discard", "Multiple files, including " + lastFileWarning + ", will be deleted tomorrow!", ToolTipIcon.Warning);
                }
                else if (lastFileWarning != null)
                {
                    _balloon.Visible = true;
                    _balloon.ShowBalloonTip(10000, "Discard", "The file " + lastFileWarning + " will be deleted tomorrow!", ToolTipIcon.Warning);
                }

            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Could not update files");
            }

        }


        public class CountdownFile
        {
            public int DaysLeft { get; set; }
            public string RealName { get; set; }
            public FileSystemInfo RealPath { get; }

            public CountdownFile(FileSystemInfo file)
            {
                RealPath = file;

                try
                {
                    //The file is persitant
                    if (file.Name.StartsWith("!"))
                    {
                        DaysLeft = 9999;
                        RealName = new string(file.Name.Skip(1).ToArray()).TrimStart();
                    }
                    else
                    {

                        string[] split = file.Name.Split(' ');

                        if (split[0].StartsWith("∞"))
                        {
                            DaysLeft = 9999;
                        }
                        else
                        {
                            DaysLeft = int.Parse(split[0].TrimEnd('d'));
                        }

                        
                        RealName = string.Join(" ", split.Skip(1));
                    }


                }
                catch (Exception)
                {
                    Console.WriteLine("[WARNING] Invalid countdown file. Could be a new file");

                    DaysLeft = MAX_DAYS;
                    RealName = file.Name;
                }
            }

            public void UpdateFile()
            {
                if (RealPath is FileInfo)
                {
                    (RealPath as FileInfo).MoveTo(Path.Combine((RealPath as FileInfo).DirectoryName, CreateName()));
                }
                else if (RealPath is DirectoryInfo)
                {
                    (RealPath as DirectoryInfo).MoveTo(Path.Combine((RealPath as DirectoryInfo).Parent.FullName, CreateName()));
                }
            }

            private string CreateName()
            {
                if (DaysLeft == 9999)
                {
                    return "∞d " + RealName;
                }
                else
                {
                    return DaysLeft.ToString(C) + "d " + RealName;
                }
                
            }

            public void Delete()
            {
                try
                {
                    string parent = null;

                    if (RealPath is FileInfo)
                    {
                        parent = (RealPath as FileInfo).Directory.Name;
                    }
                    else if (RealPath is DirectoryInfo)
                    {
                        parent = (RealPath as DirectoryInfo).Parent.Name;
                    }

                    DiscardDialogue.DiscardDialogResult diaResult = DiscardDialogue.ShowDialog(this, out string diaPath);

                    //BM here
                    switch (diaResult)
                    {
                        case DiscardDialogue.DiscardDialogResult.Delete:
                            break;

                        case DiscardDialogue.DiscardDialogResult.PermaDelete:
                            if (RealPath is FileInfo)
                            {
                                (RealPath as FileInfo).Delete();
                            }
                            else if (RealPath is DirectoryInfo)
                            {
                                (RealPath as DirectoryInfo).Delete(true);
                            }
                            break;

                        case DiscardDialogue.DiscardDialogResult.Postpone:
                            DaysLeft++;
                            UpdateFile();
                            return;

                        case DiscardDialogue.DiscardDialogResult.Archive:
                            if (RealPath is FileInfo)
                            {
                                (RealPath as FileInfo).MoveTo(diaPath);
                            }
                            else if (RealPath is DirectoryInfo)
                            {
                                (RealPath as DirectoryInfo).MoveTo(diaPath);
                            }
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File could not be deleted", "Discard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
