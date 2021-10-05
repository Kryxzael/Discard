using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discard
{
    public partial class Settings : Form
    {
        private const int EXT_COLUMN = 0;
        private const int DAYS_COLUMN = 1;

        //TODO: This, simply, is not working. It won't save on exit
        private NameValueCollection DefaultDaysPerExt
        {
            get
            {
                return Properties.Settings.Default.DefaultDaysPerExtension;
            }
        }

        public Settings()
        {
            InitializeComponent();

            numDefaultDays.Value = Properties.Settings.Default.DefaultDays;
            lstDiscards.Items.AddRange(Program.GetDiscardDirectories().ToArray());
            dataDefaultDays.Rows.AddRange(
                DefaultDaysPerExt
                .Cast<string>()
                .Select(key => 
                    {
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.Cells.AddRange(new[] { new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell() });

                        newRow.Cells[EXT_COLUMN].Value = key;
                        newRow.Cells[DAYS_COLUMN].Value = DefaultDaysPerExt.GetValues(key).Single().ToString(CultureInfo.InvariantCulture);

                        return newRow;
                    }
                )
                .ToArray()
            );
        }

        private void OnOk(object sender, EventArgs e)
        {
            Program.SetDiscardDirectories(lstDiscards.Items.Cast<string>());

            DefaultDaysPerExt.Clear();
            foreach (DataGridViewRow i in dataDefaultDays.Rows)
            {
                if (i.IsNewRow)
                    continue;

                DefaultDaysPerExt.Add(i.Cells[EXT_COLUMN].Value.ToString(), i.Cells[DAYS_COLUMN].Value.ToString());
            }

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

        private void OnDefaultDaysCellValidation(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == EXT_COLUMN)
            {
                foreach (DataGridViewRow i in dataDefaultDays.Rows)
                {
                    if (i.Index == e.RowIndex)
                        continue;

                    if (e.FormattedValue.Equals(i.Cells[EXT_COLUMN].Value)) // == operator doesn't work here for some God forsaken reason
                    {
                        btnOk.Enabled = false;
                        e.Cancel = true;
                        return;
                    }
                }
            }
            else if (e.ColumnIndex == DAYS_COLUMN)
            {
                if (!int.TryParse(e.FormattedValue.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int _))
                {
                    btnOk.Enabled = false;
                    e.Cancel = true;
                    return;
                }
            }

            btnOk.Enabled = true;
        }
    }
}
