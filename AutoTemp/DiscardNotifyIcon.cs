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
                Icon = Properties.Resources.DiscardError
            };
            _icon.MouseUp += OnClick;
            _icon.ContextMenuStrip = _context;
            _context.Opening += (s, e) => GenerateContext();
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
            DiscardCycle cycle = DiscardCycle.DryRun(Program.GetDiscardDirectories().Select(i => new DirectoryInfo(i)), 0);

            /*
             * Add tracked files
             */
            IEnumerable<IGrouping<int, DiscardFile>> trackedFiles = cycle.DiscardFiles
                .Where(i => !i.Untracked)
                .GroupBy(i => i.DaysLeft)
                .OrderBy(i => i.Key);

            foreach (IGrouping<int, DiscardFile> group in trackedFiles)
            {
                //Day header
                _context.Items.Add("-");
                ToolStripMenuItem header = new ToolStripMenuItem(group.Key + (Math.Abs(group.Key) == 1 ? " day" : " days"))
                {
                    Font = _boldFont,
                };

                switch (group.Key)
                {
                    case int i when (i <= 0):
                        header.ForeColor = Color.Red;
                        header.BackColor = Color.Pink;
                        break;
                    case 1:
                        header.BackColor = Color.Wheat;
                        break;
                    case int i when (i > Properties.Settings.Default.DefaultDays):
                        header.ForeColor = Color.FromArgb(75, 75, 75);
                        break;
                }

                _context.Items.Add(header);

                foreach (DiscardFile i in group)
                {
                    if (group.Count() >= 8)
                    {
                        header.DropDownItems.Add(CreateContextMenuForDiscardFile(i));
                    }
                    else
                    {
                        _context.Items.Add(CreateContextMenuForDiscardFile(i));
                    }
                }
            }

            /*
            * Add untracked files
            */
            IEnumerable<DiscardFile> untrackedFiles = cycle.DiscardFiles
                .Where(i => i.Untracked);

            if (untrackedFiles.Any())
            {
                _context.Items.Add("-");

                ToolStripMenuItem header = new ToolStripMenuItem("Untracked")
                {
                    Font = _boldFont,
                    ForeColor = Color.DodgerBlue,
                    BackColor = Color.LightSkyBlue
                };

                _context.Items.Add(header);

                foreach (DiscardFile i in untrackedFiles)
                {
                    if (untrackedFiles.Count() >= 8)
                    {
                        _context.Items.Add(CreateContextMenuForDiscardFile(i));
                    }
                    else
                    {
                        _context.Items.Add(CreateContextMenuForDiscardFile(i));
                    }
                }
            }



            /*
             * The other stuff
             */
            _context.Items.Add("-");
            _context.Items.Add("&Settings...", null, (s, e) => new Settings().ShowDialog());
            _context.Items.Add("&Console ...", null, (s, e) => new UserConsoleLib.UserConsole().ShowDialog());
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

            _context.Items.Add("-");
            _context.Items.Add("Exit", null, (s, e) => Application.Exit());

            return _context;
        }

        /// <summary>
        /// Creates a ToolStripMenuItem from a discard file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private ToolStripMenuItem CreateContextMenuForDiscardFile(DiscardFile file)
        {
            //Create the button for the file
            ToolStripMenuItem fileButton = new ToolStripMenuItem()
            {
                Text = DiscardFile.GetRealName(file.Source.Name) + (file.Source is DirectoryInfo ? "\\" : ""),
                Image = ThumbnailGenerator.WindowsThumbnailProvider.GetThumbnail(file.Source.FullName, 16, 16, ThumbnailGenerator.ThumbnailOptions.None),
                Font = Control.DefaultFont
            };

            fileButton.Click += (s, e) => Process.Start(file.Source.FullName);

            /*
             * Colorization
             */

            //Based on extended timers
            if (file.DaysLeft > DiscardFile.GetDefaultDays(file))
            {
                fileButton.ForeColor = Color.FromArgb(75, 75, 75);
                fileButton.ToolTipText = "This " + (file.Source is FileInfo ? "file" : "directory") + " has an extended timer";
            }

            //Based on modification time
            switch ((DateTime.Now - file.Source.LastWriteTime).Days)
            {
                case 0:
                    fileButton.ForeColor = Color.Red;
                    fileButton.ToolTipText = "This " + (file.Source is FileInfo ? "file" : "directory") + " was edited less than 24 hours ago";
                    break;
                case 1:
                    fileButton.ForeColor = Color.Orange;
                    fileButton.ToolTipText = "This " + (file.Source is FileInfo ? "file" : "directory") + " was edited less than 48 hours ago";
                    break;
                case 2:
                    fileButton.ForeColor = Color.Goldenrod;
                    fileButton.ToolTipText = "This " + (file.Source is FileInfo ? "file" : "directory") + " was edited less than 72 hours ago";
                    break;
            }

            //Based on tracking
            if (file.Untracked)
            {
                fileButton.ForeColor = Color.DodgerBlue;
                fileButton.BackColor = Color.LightSkyBlue;
                fileButton.ToolTipText = "This " + (file.Source is FileInfo ? "file" : "directory") + " is untracked";
            }

            //Based on emptyness
            if ((file.Source is FileInfo f && f.Length == 0) || (file.Source is DirectoryInfo d && !d.EnumerateFileSystemInfos().Any()))
            {
                fileButton.ForeColor = Color.Green;
                fileButton.ToolTipText = "This " + (file.Source is FileInfo ? "file" : "directory") + " is empty and can safely be deleted";
            }

            //Backcolor
            if (file.DaysLeft <= 0)
            {
                fileButton.BackColor = Color.Pink;
            }
            else if (file.DaysLeft == 1 && !file.Untracked)
            {
                fileButton.BackColor = Color.Wheat;
            }

            /*
             * Sub-buttons
             */
            fileButton.DropDown.Items.Add("Open", null, (s, e) =>
            {
                Process.Start(file.Source.FullName);
            });

            fileButton.DropDown.Items.Add("Open file location", null, (s, e) =>
            {
                if (file.Source is FileInfo fi)
                {
                    Process.Start(fi.DirectoryName);
                }
                else if (file.Source is DirectoryInfo di)
                {
                    Process.Start(di.Parent.FullName);
                }
            });
            fileButton.DropDown.Items.Add("-");

            if (file.HasExternalCounter)
            {
                fileButton.DropDown.Items.Add("Merge external tracker file", null, (s, e) =>
                {
                    file.MergeCounterFile();
                });
            }
            else
            {
                fileButton.DropDown.Items.Add("Create external tracker file", null, (s, e) =>
                {
                    file.CreateCounterFile();
                });
            }
            

            fileButton.DropDown.Items.Add("Archive...", null, (s, e) =>
            {
                //Copied from the discard dialog

                //Shows the save file dialog
                SaveFileDialog dia = new SaveFileDialog()
                {
                    Title = "Archive " + DiscardFile.GetRealName(file.Source.Name)
                };

                //Add extension filters
                if (file.Source is FileInfo fi)
                {
                    dia.Filter = $"Current extension (*{fi.Extension})|*{fi.Extension}|Any extension (*.*)|*";
                    dia.FileName = DiscardFile.GetRealName(file.Source.Name);
                }
                else if (file.Source is DirectoryInfo)
                {
                    dia.Filter = "File Directory|*";
                    dia.FileName = DiscardFile.GetRealName(file.Source.Name);
                }

                //Shows the dialog
                if (dia.ShowDialog() == DialogResult.Cancel)
                {
                    //If the user hit cancel, continue to the next file in the selection
                    return;
                }

                try
                {
                    file.Archive(dia.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not archive file, Please try again, or in another location", "Archive failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            fileButton.DropDown.Items.Add("Delete now...", null, (s, e) =>
            {
                if (MessageBox.Show("Are you sure you want to permanently delete '" + file.Source.Name + "'?", "Delete now", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    file.Delete();
                }
            });

            fileButton.DropDown.Items.Add("-");
            (fileButton.DropDown.Items.Add("Set days left...") as ToolStripMenuItem).DropDown.Items.AddRange(new ToolStripItem[]{
                        new ToolStripMenuItem("1 day", null, (s, e) => file.DaysLeft = 1),
                        new ToolStripMenuItem("3 days", null, (s, e) => file.DaysLeft = 3),
                        new ToolStripMenuItem("5 days", null, (s, e) => file.DaysLeft = 5),
                        new ToolStripMenuItem("7 days", null, (s, e) => file.DaysLeft = 7),
                        new ToolStripMenuItem("14 days", null, (s, e) => file.DaysLeft = 14),
                        new ToolStripMenuItem("30 days", null, (s, e) => file.DaysLeft = 30),
                        new ToolStripMenuItem("60 days", null, (s, e) => file.DaysLeft = 60),
                        new ToolStripMenuItem("999 days", null, (s, e) => file.DaysLeft = 999),
                    });

            fileButton.DropDown.Items.Add(new ToolStripMenuItem("Warn before deletion", null, (s, e) =>
            {
                file.NoWarning = !file.NoWarning;
            })
            { Checked = !file.NoWarning });

            return fileButton;
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
            switch (DiscardCycle.DryRun(Program.GetDiscardDirectories().Select(i => new DirectoryInfo(i)), 0)
                .DiscardFiles
                    .OrderBy(i => i.DaysLeft)
                    .FirstOrDefault()?
                    .DaysLeft ?? 3
                 )
            {
                case int n when n <= 0:
                    _icon.Icon = Properties.Resources.DiscardError;
                    _icon.Text = "One or more files are overdue for deletion";
                    break;
                case 1:
                    _icon.Icon = Properties.Resources.DiscardWarning;
                    _icon.Text = "One or more files will be deleted tomorrow";
                    break;
                default:
                    _icon.Icon = Properties.Resources.DiscardOK;
#if DEBUG
                    _icon.Text = "Discard (Debug Mode)";
#else
                    _icon.Text = "Discard";
#endif
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
                if (Program.GetDiscardDirectories().Any())
                {
                    Process.Start(Program.GetDiscardDirectories().First());
                }
                else
                {
                    new Settings().ShowDialog();
                }
                
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
