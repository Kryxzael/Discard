using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discard
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            numDefaultDays.Value = Properties.Settings.Default.DefaultDays;
            lstDiscards.Items.AddRange(Program.GetDiscardDirectories().ToArray());
        }

        private void OnOk(object sender, EventArgs e)
        {
            Program.SetDiscardDirectories(lstDiscards.Items.Cast<string>());
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Close();
        }

        private void OnAdd(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();

            if (browser.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            if (lstDiscards.Items.Cast<string>().Any(i => i == browser.SelectedPath))
            {
                MessageBox.Show("This directory is already registered as a discard directory");
                return;
            }

            lstDiscards.Items.Add(browser.SelectedPath);
            btnOk.Enabled = true;
        }

        private void OnRemove(object sender, EventArgs e)
        {
            lstDiscards.Items.Remove(lstDiscards.SelectedItem);

            if (lstDiscards.Items.Count == 0)
            {
                btnOk.Enabled = false;
            }
        }
    }
}
