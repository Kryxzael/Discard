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
    public class DiscardNotifyIcon : IDisposable
    {
        private readonly NotifyIcon _icon;
        private readonly ContextMenuStrip _context = new ContextMenuStrip();



        public DiscardNotifyIcon()
        {
            _icon = new NotifyIcon
            {
                Icon = Properties.Resources.DiscardWarning
            };
            _icon.MouseUp += OnClick;
        }

        private ContextMenuStrip GenerateContext()
        {
            _context.Items.Clear();
            DiscardCycle cycle = DiscardCycle.DryRun(Program.GetDiscardDirectories().Select(i => new System.IO.DirectoryInfo(i)));

            //Files
            IEnumerable<IGrouping<int, DiscardFile>> files = cycle.DiscardFiles.GroupBy(i => i.DaysLeft);
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


            //Other
            _context.Items.Add("-");
            _context.Items.Add("&Settings...", null, (s, e) => MessageBox.Show("NYI"));
            _context.Items.Add("&Create...", null, (s, e) => MessageBox.Show("NYI"));
            _context.Items.Add("&Open...", null);
            {
                ToolStripMenuItem last = (ToolStripMenuItem)_context.Items[_context.Items.Count - 1];
                foreach (string i in Program.GetDiscardDirectories())
                {
                    last.DropDownItems.Add(i, null, (s, e) => Process.Start(i));
                }
            }


            return _context;
        }

        public void Show()
        {
            _icon.Visible = true;
        }

        public void Hide()
        {
            _icon.Visible = false;
        }

        public void Click()
        {
            OnClick(_icon, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }

        public void SetIcon(Icon icon)
        {
            _icon.Icon = icon;
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Process.Start(Program.GetDiscardDirectories().First());
            }
            else if (e.Button == MouseButtons.Right)
            {
                GenerateContext().Show(Control.MousePosition);
            }
            
        }

        public void Dispose()
        {
            _icon.Dispose();
            _context.Dispose();
        }
    }
}
