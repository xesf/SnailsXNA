namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class NewCustomStageForm
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
            this._cmbTheme = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._txtFilename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._cmbTheme);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this._txtFilename);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this._txtName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 108);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // _cmbTheme
            // 
            this._cmbTheme.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.Themes;
            this._cmbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTheme.FormattingEnabled = true;
            this._cmbTheme.Location = new System.Drawing.Point(85, 19);
            this._cmbTheme.Name = "_cmbTheme";
            this._cmbTheme.Size = new System.Drawing.Size(121, 21);
            this._cmbTheme.TabIndex = 0;
            this._cmbTheme.SelectedIndexChanged += new System.EventHandler(this._cmbTheme_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Theme";
            // 
            // _txtFilename
            // 
            this._txtFilename.Enabled = false;
            this._txtFilename.Location = new System.Drawing.Point(85, 72);
            this._txtFilename.Name = "_txtFilename";
            this._txtFilename.Size = new System.Drawing.Size(199, 20);
            this._txtFilename.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Filename";
            // 
            // _txtName
            // 
            this._txtName.Location = new System.Drawing.Point(85, 46);
            this._txtName.Name = "_txtName";
            this._txtName.Size = new System.Drawing.Size(119, 20);
            this._txtName.TabIndex = 1;
            this._txtName.TextChanged += new System.EventHandler(this._txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._btnCancel);
            this.panel1.Controls.Add(this._btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 108);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(329, 35);
            this.panel1.TabIndex = 3;
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(166, 6);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 1;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _btnOk
            // 
            this._btnOk.Location = new System.Drawing.Point(85, 7);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 0;
            this._btnOk.Text = "&Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // NewCustomStageForm
            // 
            this.AcceptButton = this._btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 143);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "NewCustomStageForm";
            this.Text = "New Custom Stage";
            this.Load += new System.EventHandler(this.NewCustomStageForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _txtFilename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Label label3;
        private Controls.BaseComboBox _cmbTheme;
    }
}