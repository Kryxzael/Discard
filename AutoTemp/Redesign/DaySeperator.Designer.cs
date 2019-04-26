namespace Discard.Redesign
{
    partial class DaySeperator
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbTens = new System.Windows.Forms.PictureBox();
            this.pbUnits = new System.Windows.Forms.PictureBox();
            this.pbDays = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbTens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUnits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDays)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTens
            // 
            this.pbTens.Location = new System.Drawing.Point(0, 89);
            this.pbTens.Name = "pbTens";
            this.pbTens.Size = new System.Drawing.Size(25, 25);
            this.pbTens.TabIndex = 1;
            this.pbTens.TabStop = false;
            // 
            // pbUnits
            // 
            this.pbUnits.Location = new System.Drawing.Point(0, 65);
            this.pbUnits.Name = "pbUnits";
            this.pbUnits.Size = new System.Drawing.Size(25, 25);
            this.pbUnits.TabIndex = 2;
            this.pbUnits.TabStop = false;
            // 
            // pbDays
            // 
            this.pbDays.Location = new System.Drawing.Point(0, 0);
            this.pbDays.Name = "pbDays";
            this.pbDays.Size = new System.Drawing.Size(25, 59);
            this.pbDays.TabIndex = 3;
            this.pbDays.TabStop = false;
            // 
            // DaySeperator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbDays);
            this.Controls.Add(this.pbUnits);
            this.Controls.Add(this.pbTens);
            this.Name = "DaySeperator";
            this.Size = new System.Drawing.Size(25, 114);
            ((System.ComponentModel.ISupportInitialize)(this.pbTens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUnits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDays)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTens;
        private System.Windows.Forms.PictureBox pbUnits;
        private System.Windows.Forms.PictureBox pbDays;
    }
}
