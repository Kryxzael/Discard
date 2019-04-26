namespace Discard
{
    partial class DiscardManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flw = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flw.Location = new System.Drawing.Point(0, 0);
            this.flw.Name = "flowLayoutPanel1";
            this.flw.Size = new System.Drawing.Size(768, 139);
            this.flw.TabIndex = 0;
            // 
            // DiscardManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 139);
            this.Controls.Add(this.flw);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DiscardManager";
            this.Text = "DiscardManager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flw;
    }
}