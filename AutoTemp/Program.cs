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
        public const bool BYPASS_DAY_CHECK = true;
#else
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
            return Properties.Settings.Default.DiscardDirs?.Cast<string>() ?? new string[0] { };
#endif

        }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!GetDiscardDirectories().Any())
            {
                MessageBox.Show("No discard directories configured. Manual entries to Properties.Settings required. This will be fixed in future releases", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Microsoft.Win32.SystemEvents.SessionSwitch += (s, e) =>
            {
                if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock)
                {
                    RunOnAwake();
                }
            };


            new DiscardNotifyIcon().Show();
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

            return DateTime.Now.Date == Properties.Settings.Default.LastDiscardCycle.Date;
        }

        /// <summary>
        /// Updates the flag file
        /// </summary>
        /// <returns></returns>
        private static void WriteFlagFile()
        {
            Properties.Settings.Default.LastDiscardCycle = DateTime.Now.Date;
            Properties.Settings.Default.Save();
        }
    }
}
