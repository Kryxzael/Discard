using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        /// The font that is used for bold text in the menu
        /// </summary>
        private static readonly Font _boldFont = new Font(Control.DefaultFont, FontStyle.Bold);

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
                //Day header
                {
                    _context.Items.Add("-");
                    ToolStripMenuItem header = new ToolStripMenuItem(group.Key + (Math.Abs(group.Key) == 1 ? " day" : " days"))
                    {
                        Font = _boldFont,
                    };

                    switch (group.Key)
                    {
                        case int i when (i <= 0):
                            header.ForeColor = Color.Red;
                            break;
                        case 1:
                            header.ForeColor = Color.Orange;
                            break;
                        case 2:
                            header.ForeColor = Color.Goldenrod;
                            break;
                    }

                    _context.Items.Add(header);
                }
                
                //Add files in the current group
                foreach (DiscardFile i in group)
                {
                    //Add the file button
                    ToolStripMenuItem fileButton = (ToolStripMenuItem)_context.Items.Add(
                        text: i.RealName,
                        image: DiscardDialogFull.GetIconFromFileOrFolder(i.Source),
                        onClick: (s, e) => Process.Start(i.Source.FullName)
                    );
    
                    //Add the subbuttons
                    fileButton.DropDown.Items.Add("Open", null, (s, e) =>
                    {
                        Process.Start(i.Source.FullName);
                    });
                    fileButton.DropDown.Items.Add("Open file location", null, (s, e) => 
                    {
                        if (i.Source is FileInfo f)
                        {
                            Process.Start(f.DirectoryName);
                        }
                        else if (i.Source is DirectoryInfo d)
                        {
                            Process.Start(d.Parent.FullName);
                        }
                    });
                    fileButton.DropDown.Items.Add("Archive...", null, (s, e) => 
                    {
                        //Shows the save file dialog
                        SaveFileDialog dia = new SaveFileDialog()
                        {
                            Title = "Archive " + i.RealName
                        };

                        //Add extension filters
                        if (i.Source is FileInfo f)
                        {
                            dia.Filter = $"Current extension (*{f.Extension})|*{f.Extension}|Any extension (*.*)|*";
                            dia.FileName = i.RealName;
                        }
                        else if (i.Source is DirectoryInfo d)
                        {
                            dia.Filter = "File Directory|*";
                            dia.FileName = i.RealName;
                        }

                        //Shows the dialog
                        if (dia.ShowDialog() == DialogResult.Cancel)
                        {
                            //If the user hit cancel, continue to the next file in the selection
                            return;
                        }

                        try
                        {
                            i.Archive(dia.FileName);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Could not archive file, Please try again, or in another location", "Archive failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                    fileButton.DropDown.Items.Add("Delete now...", null, (s, e) => 
                    {
                        if (MessageBox.Show("Are you sure you want to permanently delete this file?", "Delete now", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            i.Delete();
                        }
                    });
                    fileButton.DropDown.Items.Add("-");
                    (fileButton.DropDown.Items.Add("Set days left...") as ToolStripMenuItem).DropDown.Items.AddRange(new ToolStripItem[]{
                        new ToolStripMenuItem("1 day", null, (s, e) => i.DaysLeft = 1),
                        new ToolStripMenuItem("3 days", null, (s, e) => i.DaysLeft = 3),
                        new ToolStripMenuItem("5 days", null, (s, e) => i.DaysLeft = 5),
                        new ToolStripMenuItem("7 days", null, (s, e) => i.DaysLeft = 7),
                        new ToolStripMenuItem("14 days", null, (s, e) => i.DaysLeft = 14),
                        new ToolStripMenuItem("30 days", null, (s, e) => i.DaysLeft = 30),
                        new ToolStripMenuItem("60 days", null, (s, e) => i.DaysLeft = 60),
                        new ToolStripMenuItem("999 days", null, (s, e) => i.DaysLeft = 999),
                    });

                    fileButton.DropDown.Items.Add(new ToolStripMenuItem("Warn before deletion", null, (s, e) => i.NoWarning = !i.NoWarning) { Checked = !i.NoWarning });
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
        /// Updates the icon of the taskbar button
        /// </summary>
        public void UpdateIcon()
        {
            switch (DiscardCycle.DryRun(Program.GetDiscardDirectories().Select(i => new DirectoryInfo(i)))
                .DiscardFiles
                    .OrderBy(i => i.DaysLeft)
                    .FirstOrDefault()?
                    .DaysLeft ?? 3
                 )
            {
                case int n when n <= 0:
                    _icon.Icon = Properties.Resources.DiscardError;
                    break;
                case 1:
                    _icon.Icon = Properties.Resources.DiscardWarning;
                    break;
                default:
                    _icon.Icon = Properties.Resources.DiscardOK;
                    break;
            }
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
