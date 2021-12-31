namespace FontEdit
{
    partial class PreferencesForm
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
          this.groupBox1 = new System.Windows.Forms.GroupBox();
          this._RdWhiteBox = new System.Windows.Forms.RadioButton();
          this._TxtDefCharHeight = new System.Windows.Forms.TextBox();
          this._RdBlackBox = new System.Windows.Forms.RadioButton();
          this.label2 = new System.Windows.Forms.Label();
          this._TxtDefCharWidth = new System.Windows.Forms.TextBox();
          this.label1 = new System.Windows.Forms.Label();
          this._BtnCancel = new System.Windows.Forms.Button();
          this._BtnOk = new System.Windows.Forms.Button();
          this.groupBox1.SuspendLayout();
          this.SuspendLayout();
          // 
          // groupBox1
          // 
          this.groupBox1.Controls.Add(this._RdWhiteBox);
          this.groupBox1.Controls.Add(this._TxtDefCharHeight);
          this.groupBox1.Controls.Add(this._RdBlackBox);
          this.groupBox1.Controls.Add(this.label2);
          this.groupBox1.Controls.Add(this._TxtDefCharWidth);
          this.groupBox1.Controls.Add(this.label1);
          this.groupBox1.Location = new System.Drawing.Point(12, 12);
          this.groupBox1.Name = "groupBox1";
          this.groupBox1.Size = new System.Drawing.Size(163, 151);
          this.groupBox1.TabIndex = 5;
          this.groupBox1.TabStop = false;
          this.groupBox1.Text = "Font defaults";
          // 
          // _RdWhiteBox
          // 
          this._RdWhiteBox.AutoSize = true;
          this._RdWhiteBox.Location = new System.Drawing.Point(24, 116);
          this._RdWhiteBox.Name = "_RdWhiteBox";
          this._RdWhiteBox.Size = new System.Drawing.Size(74, 17);
          this._RdWhiteBox.TabIndex = 7;
          this._RdWhiteBox.TabStop = true;
          this._RdWhiteBox.Text = "White Box";
          this._RdWhiteBox.UseVisualStyleBackColor = true;
          // 
          // _TxtDefCharHeight
          // 
          this._TxtDefCharHeight.Location = new System.Drawing.Point(79, 57);
          this._TxtDefCharHeight.Name = "_TxtDefCharHeight";
          this._TxtDefCharHeight.Size = new System.Drawing.Size(52, 20);
          this._TxtDefCharHeight.TabIndex = 3;
          // 
          // _RdBlackBox
          // 
          this._RdBlackBox.AutoSize = true;
          this._RdBlackBox.Location = new System.Drawing.Point(24, 93);
          this._RdBlackBox.Name = "_RdBlackBox";
          this._RdBlackBox.Size = new System.Drawing.Size(73, 17);
          this._RdBlackBox.TabIndex = 6;
          this._RdBlackBox.TabStop = true;
          this._RdBlackBox.Text = "Black Box";
          this._RdBlackBox.UseVisualStyleBackColor = true;
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(21, 60);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(38, 13);
          this.label2.TabIndex = 2;
          this.label2.Text = "Height";
          // 
          // _TxtDefCharWidth
          // 
          this._TxtDefCharWidth.Location = new System.Drawing.Point(79, 31);
          this._TxtDefCharWidth.Name = "_TxtDefCharWidth";
          this._TxtDefCharWidth.Size = new System.Drawing.Size(52, 20);
          this._TxtDefCharWidth.TabIndex = 1;
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(21, 34);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(35, 13);
          this.label1.TabIndex = 0;
          this.label1.Text = "Width";
          // 
          // _BtnCancel
          // 
          this._BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this._BtnCancel.Location = new System.Drawing.Point(95, 183);
          this._BtnCancel.Name = "_BtnCancel";
          this._BtnCancel.Size = new System.Drawing.Size(75, 23);
          this._BtnCancel.TabIndex = 4;
          this._BtnCancel.Text = "&Cancel";
          this._BtnCancel.UseVisualStyleBackColor = true;
          // 
          // _BtnOk
          // 
          this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
          this._BtnOk.Location = new System.Drawing.Point(14, 183);
          this._BtnOk.Name = "_BtnOk";
          this._BtnOk.Size = new System.Drawing.Size(75, 23);
          this._BtnOk.TabIndex = 3;
          this._BtnOk.Text = "&Ok";
          this._BtnOk.UseVisualStyleBackColor = true;
          this._BtnOk.Click += new System.EventHandler(this._BtnOk_Click);
          // 
          // PreferencesForm
          // 
          this.AcceptButton = this._BtnOk;
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.CancelButton = this._BtnCancel;
          this.ClientSize = new System.Drawing.Size(190, 216);
          this.Controls.Add(this.groupBox1);
          this.Controls.Add(this._BtnCancel);
          this.Controls.Add(this._BtnOk);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "PreferencesForm";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
          this.Text = "Preferences";
          this.Load += new System.EventHandler(this.PreferencesForm_Load);
          this.groupBox1.ResumeLayout(false);
          this.groupBox1.PerformLayout();
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _TxtDefCharHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _TxtDefCharWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _BtnCancel;
        private System.Windows.Forms.Button _BtnOk;
        private System.Windows.Forms.RadioButton _RdWhiteBox;
        private System.Windows.Forms.RadioButton _RdBlackBox;
    }
}