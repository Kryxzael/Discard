namespace Discard
{
    partial class DiscardDialogFull
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiscardDialogFull));
            this.label1 = new System.Windows.Forms.Label();
            this.lstvwDelete = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmLastUsed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.pbArchiveIcon = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSendToArchive = new System.Windows.Forms.Button();
            this.btnSendToPostpone = new System.Windows.Forms.Button();
            this.btnTakeFromPostpone = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lstvwPostpone = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.pbArchiveIcon)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(507, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "The following files are subject to deletion.\r\nYou may choose to postpone them or " +
    "archive them somewhere else on your system.\r\nFiles that are not processed now wi" +
    "ll be deleted";
            // 
            // lstvwDelete
            // 
            this.lstvwDelete.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lstvwDelete.AutoArrange = false;
            this.lstvwDelete.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmSize,
            this.clmLastUsed});
            this.lstvwDelete.FullRowSelect = true;
            this.lstvwDelete.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvwDelete.LabelWrap = false;
            this.lstvwDelete.LargeImageList = this.imgList;
            this.lstvwDelete.Location = new System.Drawing.Point(170, 60);
            this.lstvwDelete.Name = "lstvwDelete";
            this.lstvwDelete.Size = new System.Drawing.Size(251, 323);
            this.lstvwDelete.SmallImageList = this.imgList;
            this.lstvwDelete.TabIndex = 1;
            this.lstvwDelete.UseCompatibleStateImageBehavior = false;
            this.lstvwDelete.View = System.Windows.Forms.View.Details;
            this.lstvwDelete.SelectedIndexChanged += new System.EventHandler(this.Delete_ItemSelected);
            this.lstvwDelete.DoubleClick += new System.EventHandler(this.List_OpenFile);
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 129;
            // 
            // clmSize
            // 
            this.clmSize.Text = "Size";
            this.clmSize.Width = 56;
            // 
            // clmLastUsed
            // 
            this.clmLastUsed.Text = "Used";
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "Color_BatteryDrainingInsides_0@2x.png");
            // 
            // pbArchiveIcon
            // 
            this.pbArchiveIcon.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbArchiveIcon.Image = global::Discard.Properties.Resources.Folder;
            this.pbArchiveIcon.Location = new System.Drawing.Point(0, 0);
            this.pbArchiveIcon.Name = "pbArchiveIcon";
            this.pbArchiveIcon.Size = new System.Drawing.Size(102, 96);
            this.pbArchiveIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbArchiveIcon.TabIndex = 2;
            this.pbArchiveIcon.TabStop = false;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Archive";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pbArchiveIcon);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 148);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(102, 123);
            this.panel1.TabIndex = 4;
            // 
            // btnSendToArchive
            // 
            this.btnSendToArchive.Enabled = false;
            this.btnSendToArchive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendToArchive.Location = new System.Drawing.Point(121, 175);
            this.btnSendToArchive.Name = "btnSendToArchive";
            this.btnSendToArchive.Size = new System.Drawing.Size(40, 40);
            this.btnSendToArchive.TabIndex = 5;
            this.btnSendToArchive.Text = "←";
            this.btnSendToArchive.UseVisualStyleBackColor = true;
            this.btnSendToArchive.Click += new System.EventHandler(this.BtnSendToArchive_Click);
            // 
            // btnSendToPostpone
            // 
            this.btnSendToPostpone.Enabled = false;
            this.btnSendToPostpone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendToPostpone.Location = new System.Drawing.Point(430, 161);
            this.btnSendToPostpone.Name = "btnSendToPostpone";
            this.btnSendToPostpone.Size = new System.Drawing.Size(40, 40);
            this.btnSendToPostpone.TabIndex = 7;
            this.btnSendToPostpone.Text = "→";
            this.btnSendToPostpone.UseVisualStyleBackColor = true;
            this.btnSendToPostpone.Click += new System.EventHandler(this.BtnSendToPostpone_Click);
            // 
            // btnTakeFromPostpone
            // 
            this.btnTakeFromPostpone.Enabled = false;
            this.btnTakeFromPostpone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTakeFromPostpone.Location = new System.Drawing.Point(430, 217);
            this.btnTakeFromPostpone.Name = "btnTakeFromPostpone";
            this.btnTakeFromPostpone.Size = new System.Drawing.Size(40, 40);
            this.btnTakeFromPostpone.TabIndex = 8;
            this.btnTakeFromPostpone.Text = "←";
            this.btnTakeFromPostpone.UseVisualStyleBackColor = true;
            this.btnTakeFromPostpone.Click += new System.EventHandler(this.BtnTakeFromPostpone_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(170, 388);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(251, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = "Delete";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(485, 359);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(253, 22);
            this.label4.TabIndex = 9;
            this.label4.Text = "Postpone";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(582, 384);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(663, 384);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // lstvwPostpone
            // 
            this.lstvwPostpone.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lstvwPostpone.AutoArrange = false;
            this.lstvwPostpone.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstvwPostpone.FullRowSelect = true;
            this.lstvwPostpone.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvwPostpone.LabelWrap = false;
            this.lstvwPostpone.LargeImageList = this.imgList;
            this.lstvwPostpone.Location = new System.Drawing.Point(487, 60);
            this.lstvwPostpone.Name = "lstvwPostpone";
            this.lstvwPostpone.Size = new System.Drawing.Size(251, 296);
            this.lstvwPostpone.SmallImageList = this.imgList;
            this.lstvwPostpone.TabIndex = 13;
            this.lstvwPostpone.UseCompatibleStateImageBehavior = false;
            this.lstvwPostpone.View = System.Windows.Forms.View.Details;
            this.lstvwPostpone.SelectedIndexChanged += new System.EventHandler(this.Postpone_ItemSelected);
            this.lstvwPostpone.DoubleClick += new System.EventHandler(this.List_OpenFile);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 129;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.Width = 56;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Used";
            // 
            // DiscardDialogFull
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(752, 419);
            this.Controls.Add(this.lstvwPostpone);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTakeFromPostpone);
            this.Controls.Add(this.btnSendToPostpone);
            this.Controls.Add(this.btnSendToArchive);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstvwDelete);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DiscardDialogFull";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Discard";
            ((System.ComponentModel.ISupportInitialize)(this.pbArchiveIcon)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lstvwDelete;
        private System.Windows.Forms.PictureBox pbArchiveIcon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSendToArchive;
        private System.Windows.Forms.Button btnSendToPostpone;
        private System.Windows.Forms.Button btnTakeFromPostpone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.ColumnHeader clmLastUsed;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ListView lstvwPostpone;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}