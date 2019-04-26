using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discard.Redesign
{
    public partial class FileIconControl : UserControl
    {
        public DiscardFile TargetFile { get; }

        public FileIconControl()
        {
            InitializeComponent();
            pbAlert.Parent = pbThumb;

            //Make control clickable
            ApplyToAllSubControls(this, i => i.Click += Clicked);
        }



        public FileIconControl(DiscardFile file) : this()
        {
            TargetFile = file;
            UpdateFileInfo();
        }

        private void UpdateFileInfo()
        {
            //No target, do not attempt (re)initialization
            if (TargetFile == null)
            {
                return;
            }
            
            lblFileName.Text = TargetFile.RealName;
            pbThumb.Image?.Dispose();
            pbThumb.Image = DiscardDialogFull.GetIconFromFileOrFolder(TargetFile.Source);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            BackColor = SystemColors.Highlight;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            BackColor = SystemColors.Control;
        }

        private void Clicked(object sender, EventArgs e)
        {
            Focus();
        }

        private void ApplyToAllSubControls(Control control, Action<Control> action)
        {
            action(control);

            foreach (Control i in control.Controls)
            {
                ApplyToAllSubControls(i, action);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

            //Custom code
            pbThumb.Image.Dispose();
        }

    }
}
