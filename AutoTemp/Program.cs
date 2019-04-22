using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discard
{
    public static class Program
    {
        public static string FLAG_FILE => Path.Combine(GetDiscardDirectories().First(), ".FLAG");

#if DEBUG
        public const int MAX_DAYS = 2;
        public const bool BYPASS_DAY_CHECK = true;
#else
        public const int MAX_DAYS = 7;
        public const bool BYPASS_DAY_CHECK = false;
#endif

        /// <summary>
        /// Gets the directories that the discard program will enumerate over
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetDiscardDirectories()
        {
#if DEBUG
            yield return "C:\\users\\kryxzael\\desktop\\discard2";
#else
            yield return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\discard";
            yield return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\downloads";
#endif

        }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Microsoft.Win32.SystemEvents.SessionSwitch += (s, e) =>
            {
                if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock)
                {
                    RunOnAwake();
                }
            };

            RunOnAwake();
            Application.Run();
        }

        /// <summary>
        /// Runs a cycle if it hasn't been run today
        /// </summary>
        private static void RunOnAwake()
        {
            if (HasBeenRunToday())
            {
                //App has allready been run and will not be run again
                return;
            }

            //Runs a discard cycle;
            DiscardCycle.RunNow(GetDiscardDirectories().Select(i => new DirectoryInfo(i)));

            //Writes the flag file
            WriteFlagFile();
        }

        /// <summary>
        /// Has the application been run today?
        /// </summary>
        /// <returns></returns>
        private static bool HasBeenRunToday()
        {
            if (BYPASS_DAY_CHECK)
            {
                return false;
            }

            try
            {
                //Finds file
                FileInfo flagFile = new FileInfo(FLAG_FILE);

                if (!flagFile.Exists)
                {
                    //Flag file does not exits, we can assume the app has not been run
                    return false;
                }
                else
                {
                    //True if the last write time is greater that the current date at 00:00:00
                    return flagFile.LastWriteTime.Date >= DateTime.Now.Date;
                }
            }
            catch (Exception)
            {
                //Could not read file. Assume to has been read
                Console.WriteLine("[ERROR] Exception thrown when reading flag file. Is the file locked?");
                return true;
            }
        }

        /// <summary>
        /// Updates the flag file
        /// </summary>
        /// <returns></returns>
        private static void WriteFlagFile()
        {
            try
            {
                File.Delete(FLAG_FILE); //Deletes existing file
                File.WriteAllBytes(FLAG_FILE, new byte[0]); //Writes a new empty file
                File.SetAttributes(FLAG_FILE, File.GetAttributes(FLAG_FILE) | FileAttributes.Hidden); //Makes the file hidden
            }
            catch (Exception)
            {
                //File could not be written to
                Console.WriteLine("[ERROR] Flag file could not be updated, Is the file locked?");
            }
        }
    }
}
