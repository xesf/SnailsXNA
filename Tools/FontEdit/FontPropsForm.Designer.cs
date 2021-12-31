namespace FontEdit
{
    partial class FontPropsForm
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
            this._BtnOk = new System.Windows.Forms.Button();
            this._BtnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._TxtImageFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._txtImageId = new System.Windows.Forms.TextBox();
            this._txtImageId1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _BtnOk
            // 
            this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._BtnOk.Location = new System.Drawing.Point(68, 124);
            this._BtnOk.Name = "_BtnOk";
            this._BtnOk.Size = new System.Drawing.Size(75, 23);
            this._BtnOk.TabIndex = 0;
            this._BtnOk.Text = "&Ok";
            this._BtnOk.UseVisualStyleBackColor = true;
            this._BtnOk.Click += new System.EventHandler(this._BtnOk_Click);
            // 
            // _BtnCancel
            // 
            this._BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._BtnCancel.Location = new System.Drawing.Point(149, 124);
            this._BtnCancel.Name = "_BtnCancel";
            this._BtnCancel.Size = new System.Drawing.Size(75, 23);
            this._BtnCancel.TabIndex = 1;
            this._BtnCancel.Text = "&Cancel";
            this._BtnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._txtImageId);
            this.groupBox1.Controls.Add(this._txtImageId1);
            this.groupBox1.Controls.Add(this._TxtImageFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 106);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // _TxtImageFile
            // 
            this._TxtImageFile.Location = new System.Drawing.Point(79, 33);
            this._TxtImageFile.Name = "_TxtImageFile";
            this._TxtImageFile.Size = new System.Drawing.Size(159, 20);
            this._TxtImageFile.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image file";
            // 
            // _txtImageId
            // 
            this._txtImageId.Location = new System.Drawing.Point(79, 59);
            this._txtImageId.Name = "_txtImageId";
            this._txtImageId.Size = new System.Drawing.Size(159, 20);
            this._txtImageId.TabIndex = 3;
            this._txtImageId.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // _txtImageId1
            // 
            this._txtImageId1.AutoSize = true;
            this._txtImageId1.Location = new System.Drawing.Point(21, 62);
            this._txtImageId1.Name = "_txtImageId1";
            this._txtImageId1.Size = new System.Drawing.Size(48, 13);
            this._txtImageId1.TabIndex = 2;
            this._txtImageId1.Text = "Image Id";
            // 
            // FontPropsForm
            // 
            this.AcceptButton = this._BtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._BtnCancel;
            this.ClientSize = new System.Drawing.Size(293, 168);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._BtnCancel);
            this.Controls.Add(this._BtnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FontPropsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Font Properties";
            this.Load += new System.EventHandler(this.FontPropsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _BtnOk;
        private System.Windows.Forms.Button _BtnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _TxtImageFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _txtImageId;
        private System.Windows.Forms.Label _txtImageId1;
    }
}