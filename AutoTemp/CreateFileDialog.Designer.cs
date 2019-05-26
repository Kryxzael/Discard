namespace Discard
{
    partial class CreateFileDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoTypeDirectory = new System.Windows.Forms.RadioButton();
            this.rdoTypeFile = new System.Windows.Forms.RadioButton();
            this.chkAutoOpen = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkWarn = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numTime = new System.Windows.Forms.NumericUpDown();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.rdoTimeAt = new System.Windows.Forms.RadioButton();
            this.rdoTimeAfter = new System.Windows.Forms.RadioButton();
            this.rdoTimeTomorrow = new System.Windows.Forms.RadioButton();
            this.rdoTimeDefault = new System.Windows.Forms.RadioButton();
            this.cboxCreateIn = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(307, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(219, 150);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnOk);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoTypeDirectory);
            this.groupBox1.Controls.Add(this.rdoTypeFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 72);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Object Type";
            // 
            // rdoTypeDirectory
            // 
            this.rdoTypeDirectory.AutoSize = true;
            this.rdoTypeDirectory.Location = new System.Drawing.Point(6, 42);
            this.rdoTypeDirectory.Name = "rdoTypeDirectory";
            this.rdoTypeDirectory.Size = new System.Drawing.Size(67, 17);
            this.rdoTypeDirectory.TabIndex = 1;
            this.rdoTypeDirectory.Text = "Directory";
            this.rdoTypeDirectory.UseVisualStyleBackColor = true;
            // 
            // rdoTypeFile
            // 
            this.rdoTypeFile.AutoSize = true;
            this.rdoTypeFile.Checked = true;
            this.rdoTypeFile.Location = new System.Drawing.Point(6, 19);
            this.rdoTypeFile.Name = "rdoTypeFile";
            this.rdoTypeFile.Size = new System.Drawing.Size(41, 17);
            this.rdoTypeFile.TabIndex = 0;
            this.rdoTypeFile.TabStop = true;
            this.rdoTypeFile.Text = "File";
            this.rdoTypeFile.UseVisualStyleBackColor = true;
            // 
            // chkAutoOpen
            // 
            this.chkAutoOpen.AutoSize = true;
            this.chkAutoOpen.Checked = true;
            this.chkAutoOpen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoOpen.Location = new System.Drawing.Point(219, 20);
            this.chkAutoOpen.Name = "chkAutoOpen";
            this.chkAutoOpen.Size = new System.Drawing.Size(149, 17);
            this.chkAutoOpen.TabIndex = 3;
            this.chkAutoOpen.Text = "Open object after creation";
            this.chkAutoOpen.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(219, 124);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(170, 20);
            this.txtName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Object name";
            // 
            // chkWarn
            // 
            this.chkWarn.AutoSize = true;
            this.chkWarn.Checked = true;
            this.chkWarn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarn.Location = new System.Drawing.Point(219, 44);
            this.chkWarn.Name = "chkWarn";
            this.chkWarn.Size = new System.Drawing.Size(125, 17);
            this.chkWarn.TabIndex = 7;
            this.chkWarn.Text = "Warn before deletion";
            this.chkWarn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numTime);
            this.groupBox2.Controls.Add(this.dtpTime);
            this.groupBox2.Controls.Add(this.rdoTimeAt);
            this.groupBox2.Controls.Add(this.rdoTimeAfter);
            this.groupBox2.Controls.Add(this.rdoTimeTomorrow);
            this.groupBox2.Controls.Add(this.rdoTimeDefault);
            this.groupBox2.Location = new System.Drawing.Point(12, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(198, 83);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Delete object...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "...days";
            // 
            // numTime
            // 
            this.numTime.Location = new System.Drawing.Point(68, 38);
            this.numTime.Name = "numTime";
            this.numTime.Size = new System.Drawing.Size(70, 20);
            this.numTime.TabIndex = 7;
            // 
            // dtpTime
            // 
            this.dtpTime.Location = new System.Drawing.Point(56, 61);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.Size = new System.Drawing.Size(93, 20);
            this.dtpTime.TabIndex = 6;
            // 
            // rdoTimeAt
            // 
            this.rdoTimeAt.AutoSize = true;
            this.rdoTimeAt.Location = new System.Drawing.Point(6, 64);
            this.rdoTimeAt.Name = "rdoTimeAt";
            this.rdoTimeAt.Size = new System.Drawing.Size(44, 17);
            this.rdoTimeAt.TabIndex = 5;
            this.rdoTimeAt.Text = "At...";
            this.rdoTimeAt.UseVisualStyleBackColor = true;
            // 
            // rdoTimeAfter
            // 
            this.rdoTimeAfter.AutoSize = true;
            this.rdoTimeAfter.Location = new System.Drawing.Point(6, 42);
            this.rdoTimeAfter.Name = "rdoTimeAfter";
            this.rdoTimeAfter.Size = new System.Drawing.Size(56, 17);
            this.rdoTimeAfter.TabIndex = 4;
            this.rdoTimeAfter.Text = "After...";
            this.rdoTimeAfter.UseVisualStyleBackColor = true;
            // 
            // rdoTimeTomorrow
            // 
            this.rdoTimeTomorrow.AutoSize = true;
            this.rdoTimeTomorrow.Location = new System.Drawing.Point(68, 19);
            this.rdoTimeTomorrow.Name = "rdoTimeTomorrow";
            this.rdoTimeTomorrow.Size = new System.Drawing.Size(72, 17);
            this.rdoTimeTomorrow.TabIndex = 3;
            this.rdoTimeTomorrow.Text = "Tomorrow";
            this.rdoTimeTomorrow.UseVisualStyleBackColor = true;
            // 
            // rdoTimeDefault
            // 
            this.rdoTimeDefault.AutoSize = true;
            this.rdoTimeDefault.Checked = true;
            this.rdoTimeDefault.Location = new System.Drawing.Point(6, 19);
            this.rdoTimeDefault.Name = "rdoTimeDefault";
            this.rdoTimeDefault.Size = new System.Drawing.Size(59, 17);
            this.rdoTimeDefault.TabIndex = 2;
            this.rdoTimeDefault.TabStop = true;
            this.rdoTimeDefault.Text = "Default";
            this.rdoTimeDefault.UseVisualStyleBackColor = true;
            // 
            // cboxCreateIn
            // 
            this.cboxCreateIn.FormattingEnabled = true;
            this.cboxCreateIn.Location = new System.Drawing.Point(219, 84);
            this.cboxCreateIn.Name = "cboxCreateIn";
            this.cboxCreateIn.Size = new System.Drawing.Size(170, 21);
            this.cboxCreateIn.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Create in...";
            // 
            // CreateFileDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(394, 183);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboxCreateIn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkWarn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.chkAutoOpen);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "CreateFileDialog";
            this.Text = "Create Discard File or Folder";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoTypeDirectory;
        private System.Windows.Forms.RadioButton rdoTypeFile;
        private System.Windows.Forms.CheckBox chkAutoOpen;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkWarn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numTime;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.RadioButton rdoTimeAt;
        private System.Windows.Forms.RadioButton rdoTimeAfter;
        private System.Windows.Forms.RadioButton rdoTimeTomorrow;
        private System.Windows.Forms.RadioButton rdoTimeDefault;
        private System.Windows.Forms.ComboBox cboxCreateIn;
        private System.Windows.Forms.Label label3;
    }
}