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
        public const char DIR_SEPERATOR_CHAR = '>';

        private static DiscardNotifyIcon _icon;
        private static FSWatcher _watcher;

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
            //yield return "C:\\users\\kryxzael\\desktop\\discard2";
            return Properties.Settings.Default.DiscardDirs.Split('>').Where(Directory.Exists);
#else
            return Properties.Settings.Default.DiscardDirs.Split('>').Where(Directory.Exists);
#endif

        }

        /// <summary>
        /// Sets the directories the discard program will enumerate over
        /// </summary>
        /// <param name="directories"></param>
        public static void SetDiscardDirectories(IEnumerable<string> directories)
        {
            Properties.Settings.Default.DiscardDirs = string.Join(">", directories);
            Properties.Settings.Default.Save();

            //Update FSWatcher
            _watcher?.Dispose();
            _watcher = new FSWatcher();
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
                    _icon.UpdateIcon();
                    RunOnAwake();
                }
            };

            _icon = new DiscardNotifyIcon();
            _icon.Show();

            _watcher = new FSWatcher();

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

            _icon.UpdateIcon();
        }

        /// <summary>
        /// Has the application been run today?
        /// </summary>
        /// <returns></returns>
        private static bool HasBeenRunToday()
        {
#pragma warning disable CS0162
            if (BYPASS_DAY_CHECK)
            {
                return false;
            }

            return DateTime.Now.Date == Properties.Settings.Default.LastDiscardCycle.Date;
#pragma warning restore CS0162
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
