using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discard
{
    /// <summary>
    /// Handles the task bar icon
    /// </summary>
    public class DiscardNotifyIcon : IDisposable
    {
        /// <summary>
        /// The underlaying task bar icon
        /// </summary>
        private readonly NotifyIcon _icon;

        /// <summary>
        /// The menu that is shown when the icon is right clicked
        /// </summary>
        private readonly ContextMenuStrip _context = new ContextMenuStrip();

        /// <summary>
        /// Creates a new taskbar icon
        /// </summary>
        public DiscardNotifyIcon()
        {
            _icon = new NotifyIcon
            {
                Icon = Properties.Resources.DiscardWarning
            };
            _icon.MouseUp += OnClick;
        }

        /// <summary>
        /// Regenerates the menu
        /// </summary>
        /// <returns></returns>
        private ContextMenuStrip GenerateContext()
        {
            //Clear existing items
            _context.Items.Clear();

            /*
             * FILES
             */

            //Fetches discard files
            DiscardCycle cycle = DiscardCycle.DryRun(Program.GetDiscardDirectories().Select(i => new System.IO.DirectoryInfo(i)));
            IEnumerable<IGrouping<int, DiscardFile>> files = cycle.DiscardFiles.GroupBy(i => i.DaysLeft);

            //Add files to context menu along with headers
            foreach (IGrouping<int, DiscardFile> group in files)
            {
                _context.Items.Add("-");
                _context.Items.Add(group.Key + (Math.Abs(group.Key) == 1 ? " day" : " days")).Font = new System.Drawing.Font(Control.DefaultFont, System.Drawing.FontStyle.Bold);
                foreach (DiscardFile i in group)
                {
                    ToolStripMenuItem last = (ToolStripMenuItem)_context.Items.Add(
                        text: i.RealName,
                        image: null /* TODO: Fetch icon */,
                        onClick: (s, e) => Process.Start(i.Source.FullName)
                    );
                }
            }


            /*
             * The other stuff
             */
            _context.Items.Add("-");
            _context.Items.Add("&Settings...", null, (s, e) => new Settings().ShowDialog());
            _context.Items.Add("&Create...", null, (s, e) => new CreateFileDialog().ShowDialog()).Enabled = Program.GetDiscardDirectories().Any();

            //Open button
            _context.Items.Add("&Open...", null).Enabled = Program.GetDiscardDirectories().Any();
            {
                ToolStripMenuItem last = (ToolStripMenuItem)_context.Items[_context.Items.Count - 1];
                foreach (string i in Program.GetDiscardDirectories())
                {
                    last.DropDownItems.Add(i, null, (s, e) => Process.Start(i));
                }
            }


            return _context;
        }

        /// <summary>
        /// Shows the taskbar icon
        /// </summary>
        public void Show()
        {
            _icon.Visible = true;
        }

        /// <summary>
        /// Hides the discard icon
        /// </summary>
        public void Hide()
        {
            _icon.Visible = false;
        }

        /// <summary>
        /// Simulates a click on the button
        /// </summary>
        public void Click()
        {
            OnClick(_icon, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }

        /// <summary>
        /// Sets the icon of the taskbar button
        /// </summary>
        /// <param name="icon"></param>
        public void SetIcon(Icon icon)
        {
            _icon.Icon = icon;
        }

        /// <summary>
        /// Handles clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick(object sender, MouseEventArgs e)
        {
            //Left click -> start main discard directory
            if (e.Button == MouseButtons.Left)
            {
                Process.Start(Program.GetDiscardDirectories().First());
            }

            //Right click -> show menu
            else if (e.Button == MouseButtons.Right)
            {
                GenerateContext().Show(Control.MousePosition);
            }
            
        }

        /// <summary>
        /// Disposes the IDisposables of this class
        /// </summary>
        public void Dispose()
        {
            _icon.Dispose();
            _context.Dispose();
        }
    }
}
