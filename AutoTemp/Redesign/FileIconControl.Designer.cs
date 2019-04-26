namespace Discard.Redesign
{
    partial class FileIconControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbThumb = new System.Windows.Forms.PictureBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.pbAlert = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbThumb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAlert)).BeginInit();
            this.SuspendLayout();
            // 
            // pbThumb
            // 
            this.pbThumb.Location = new System.Drawing.Point(3, 3);
            this.pbThumb.Name = "pbThumb";
            this.pbThumb.Size = new System.Drawing.Size(64, 64);
            this.pbThumb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbThumb.TabIndex = 0;
            this.pbThumb.TabStop = false;
            // 
            // lblFileName
            // 
            this.lblFileName.Location = new System.Drawing.Point(3, 70);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(64, 44);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "Uninitialized";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbAlert
            // 
            this.pbAlert.BackColor = System.Drawing.Color.Transparent;
            this.pbAlert.Location = new System.Drawing.Point(0, 0);
            this.pbAlert.Name = "pbAlert";
            this.pbAlert.Size = new System.Drawing.Size(32, 32);
            this.pbAlert.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAlert.TabIndex = 2;
            this.pbAlert.TabStop = false;
            // 
            // FileIconControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbAlert);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.pbThumb);
            this.Name = "FileIconControl";
            this.Size = new System.Drawing.Size(73, 114);
            ((System.ComponentModel.ISupportInitialize)(this.pbThumb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAlert)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbThumb;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.PictureBox pbAlert;
    }
}
