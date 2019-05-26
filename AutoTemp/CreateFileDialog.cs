using System;
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

namespace Discard
{
    /// <summary>
    /// Dialog for creating new files in a discard directory
    /// </summary>
    public partial class CreateFileDialog : Form
    {
        public CreateFileDialog()
        {
            InitializeComponent();

            //Fills the combobox with all the registered discard directories
            foreach (string i in Program.GetDiscardDirectories())
            {
                cboxCreateIn.Items.Add(i);
            }

            cboxCreateIn.SelectedIndex = 0;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            //Focuses the name box
            txtName.Focus();
        }

        /// <summary>
        /// Called when OK is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOk(object sender, EventArgs e)
        {
            try
            {
                //Create new discard file instance
                string path = Path.Combine((string)cboxCreateIn.SelectedItem, txtName.Text);
                DiscardFile newFile = new DiscardFile(rdoTypeFile.Checked ? (FileSystemInfo)new FileInfo(path) : (FileSystemInfo)new DirectoryInfo(path));

                //File exists. The file cannot be created
                if (newFile.Source.Exists)
                {
                    MessageBox.Show("That file name is already in use. Try a different name", "In use", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Create the file/folder
                if (newFile.Source is FileInfo f)
                {
                    f.Create().Close();
                }
                else if (newFile.Source is DirectoryInfo d)
                {
                    d.Create();
                }

                //Apply days left
                if (rdoTimeDefault.Checked)
                {
                    //Leave untracked
                }
                else if (rdoTimeTomorrow.Checked)
                {
                    newFile.DaysLeft = 1;
                }
                else if (rdoTimeAfter.Checked)
                {
                    newFile.DaysLeft = (int)numTime.Value;
                }
                else if (rdoTimeAt.Checked)
                {
                    newFile.DaysLeft = (dtpTime.Value - DateTime.Now).Days;
                }

                //Apply nowarn
                if (!chkWarn.Checked)
                {
                    newFile.NoWarning = true;
                }

                //Open file is auto open is set
                if (chkAutoOpen.Checked)
                {
                    Process.Start(newFile.Source.FullName);
                }

            }

            //Something went wrong. Throw up an error box
            catch (Exception)
            {
                MessageBox.Show("The file/folder could not be created or some settings could not be applied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            //Closes dialog
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Called when Cancel is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel(object sender, EventArgs e)
        {
            Close();
        }
    }
}
