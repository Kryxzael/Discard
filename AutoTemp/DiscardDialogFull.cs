using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;

namespace Discard
{
    public partial class DiscardDialogFull : Form
    {
        public IEnumerable<DiscardFile> Files { get; }

        public DiscardDialogFull()
        {
            InitializeComponent();
        }

        public DiscardDialogFull(IEnumerable<DiscardFile> files)
        {
            InitializeComponent();

            Files = files;
            CreateImageListOfFiles();
            PopulateListView();
        }

        private void PopulateListView()
        {
            int index = 0;
            foreach (DiscardFile i in Files)
            {
                ListViewItem listItem = new ListViewItem(i.Name)
                {
                    Tag = i,
                    ImageIndex = index++,
                };

                listItem.SubItems.Add(GetSizeStr(i.Source, out long size));
                listItem.SubItems.Add(GetLastUsedStr(i.Source, out int days));

                if (size == 0)
                {
                    listItem.ForeColor = Color.Green;
                }
                else if (days == 2)
                {
                    listItem.ForeColor = Color.Goldenrod;
                }
                else if (days == 1)
                {
                    listItem.ForeColor = Color.Orange;
                }
                else if (days == 0)
                {
                    listItem.ForeColor = Color.Red;
                }

                lstvwDelete.Items.Add(listItem);
            }
        }

        /// <summary>
        /// Updates the imagelist to contain the icons of the files
        /// </summary>
        private void CreateImageListOfFiles()
        {
            imgList = new ImageList()
            {
                ImageSize = new Size(32,32),
                ColorDepth = ColorDepth.Depth32Bit,
                //TransparentColor = Color.Black,
            };

            foreach (DiscardFile i in Files)
            {
                imgList.Images.Add(ThumbnailGenerator.WindowsThumbnailProvider.GetThumbnail(i.Source.FullName, 32, 32, ThumbnailGenerator.ThumbnailOptions.None));
            }

            lstvwDelete.SmallImageList = lstvwPostpone.SmallImageList = imgList;
        }

        /// <summary>
        /// Extracts the icon of a filesystementry object
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Obsolete("Use ThumbailProvider")]
        public static Bitmap GetIconFromFileOrFolder(FileSystemInfo file)
        {
            try
            {
                if (file is FileInfo f)
                {
                    using (ShellFile shFile = ShellFile.FromFilePath(f.FullName))
                        return shFile.Thumbnail.SmallBitmap;
                }
                else if (file is DirectoryInfo d)
                {
                    using (ShellFileSystemFolder shFile = ShellFileSystemFolder.FromFolderPath(d.FullName))
                        return shFile.Thumbnail.SmallBitmap;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Could not open file for icon read");
                throw;
            }
            
        }

        /// <summary>
        /// Gets all the discard files in the delete section
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiscardFile> GetFilesForDeletion()
        {
            return lstvwDelete.Items.Cast<ListViewItem>()
                .Select(i => i.Tag)
                .Cast<DiscardFile>();
        }

        /// <summary>
        /// Gets all the discard files in the postpone section
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiscardFile> GetFilesForPostponement()
        {
            return lstvwPostpone.Items.Cast<ListViewItem>()
                .Select(i => i.Tag)
                .Cast<DiscardFile>();
        }

        private void Delete_ItemSelected(object sender, EventArgs e)
        {
            btnSendToPostpone.Enabled = lstvwDelete.SelectedItems.Count != 0;
            btnSendToArchive.Enabled = lstvwDelete.SelectedItems.Count == 1;
        }

        private void Postpone_ItemSelected(object sender, EventArgs e)
        {
            btnTakeFromPostpone.Enabled = lstvwPostpone.SelectedItems.Count != 0;
        }

        private void BtnSendToPostpone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in lstvwDelete.SelectedItems)
            {
                lstvwDelete.Items.Remove(i);
                lstvwPostpone.Items.Add(i);
            }
        }

        private void BtnTakeFromPostpone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in lstvwPostpone.SelectedItems)
            {
                lstvwPostpone.Items.Remove(i);
                lstvwDelete.Items.Add(i);
            }
        }

        private void BtnSendToArchive_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in lstvwDelete.SelectedItems)
            {
                DiscardFile discardFile = i.Tag as DiscardFile;

                //Shows the save file dialog
                SaveFileDialog dia = new SaveFileDialog()
                {
                    Title = "Archive " + discardFile.Name
                };

                //Add extension filters
                if (discardFile.Source is FileInfo f)
                {
                    dia.Filter = $"Current extension (*{f.Extension})|*{f.Extension}|Any extension (*.*)|*";
                    dia.FileName = discardFile.Name;
                }
                else if (discardFile.Source is DirectoryInfo d)
                {
                    dia.Filter = "File Directory|*";
                    dia.FileName = discardFile.Name;
                }

                //Shows the dialog
                if (dia.ShowDialog() == DialogResult.Cancel)
                {
                    //If the user hit cancel, continue to the next file in the selection
                    continue;
                }

                try
                {
                    discardFile.Archive(dia.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not archive file, Please try again, or in another location", "Archive failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                lstvwDelete.Items.Remove(i);
                
            }
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in lstvwDelete.SelectedItems)
            {
                Process.Start((i.Tag as DiscardFile).Source.FullName);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        string GetSizeStr(FileSystemInfo info, out long realCount)
        {
            if (info is DirectoryInfo d)
            {
                realCount = d.GetFileSystemInfos().Length;
                return realCount == 0 ? "none" : realCount + " F";
            }
            else if (info is FileInfo f)
            {
                realCount = f.Length;

                if (realCount >= 1E9)
                {
                    return (realCount / 1E9).ToString("0.##") + " GB";
                }
                else if (realCount >= 1E7)
                {
                    return (realCount / 1E7).ToString("0.#") + " MB";
                }
                else if (realCount >= 1E3)
                {
                    return (realCount / 1E3).ToString("0") + " kB";
                }
                else if (realCount > 0)
                {
                    return realCount + " B";
                }
                else
                {
                    return "empty";
                }
            }
            else
            {
                realCount = 0;
                return "N/A";
            }
        }

        string GetLastUsedStr(FileSystemInfo info, out int days)
        {
            days = (DateTime.Now - info.LastWriteTime).Days;
            return  days + "d ago";
        }

        private void List_OpenFile(object sender, EventArgs e)
        {
            if (sender is ListView l)
            {
                if (l.SelectedItems.Count == 1)
                {
                    Process.Start((l.SelectedItems[0].Tag as DiscardFile).Source.FullName);
                }
            }
        }
    }
}
