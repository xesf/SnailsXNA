namespace TwoBrainsGames.Snails.Winforms
{
    partial class SaveGameplayForm
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
            this._lbFilename = new System.Windows.Forms.Label();
            this._btnSaveDialog = new System.Windows.Forms.Button();
            this._txtPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._cmbDescription = new System.Windows.Forms.ComboBox();
            this._txtSufix = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._lblPoints = new System.Windows.Forms.Label();
            this._lblMedal = new System.Windows.Forms.Label();
            this._lblStage = new System.Windows.Forms.Label();
            this._lblTheme = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnSave = new System.Windows.Forms.Button();
            this._dlgSave = new System.Windows.Forms.SaveFileDialog();
            this._dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this._lblBuildNr = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._lblBuildNr);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this._lbFilename);
            this.groupBox1.Controls.Add(this._btnSaveDialog);
            this.groupBox1.Controls.Add(this._txtPath);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this._cmbDescription);
            this.groupBox1.Controls.Add(this._txtSufix);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this._lblPoints);
            this.groupBox1.Controls.Add(this._lblMedal);
            this.groupBox1.Controls.Add(this._lblStage);
            this.groupBox1.Controls.Add(this._lblTheme);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 235);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // _lbFilename
            // 
            this._lbFilename.AutoSize = true;
            this._lbFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbFilename.Location = new System.Drawing.Point(82, 176);
            this._lbFilename.Name = "_lbFilename";
            this._lbFilename.Size = new System.Drawing.Size(62, 13);
            this._lbFilename.TabIndex = 17;
            this._lbFilename.Text = "[filename]";
            // 
            // _btnSaveDialog
            // 
            this._btnSaveDialog.Location = new System.Drawing.Point(387, 150);
            this._btnSaveDialog.Name = "_btnSaveDialog";
            this._btnSaveDialog.Size = new System.Drawing.Size(32, 20);
            this._btnSaveDialog.TabIndex = 16;
            this._btnSaveDialog.Text = "...";
            this._btnSaveDialog.UseVisualStyleBackColor = true;
            this._btnSaveDialog.Click += new System.EventHandler(this._btnSaveDialog_Click_1);
            // 
            // _txtPath
            // 
            this._txtPath.Location = new System.Drawing.Point(84, 153);
            this._txtPath.Name = "_txtPath";
            this._txtPath.Size = new System.Drawing.Size(296, 20);
            this._txtPath.TabIndex = 15;
            this._txtPath.TextChanged += new System.EventHandler(this._txtPath_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Path";
            // 
            // _cmbDescription
            // 
            this._cmbDescription.FormattingEnabled = true;
            this._cmbDescription.Items.AddRange(new object[] {
            "Bronze medal",
            "Silver medal ",
            "Gold medal",
            "Gold medal - alternative 1",
            "Gold medal - for Snails masters"});
            this._cmbDescription.Location = new System.Drawing.Point(85, 128);
            this._cmbDescription.Name = "_cmbDescription";
            this._cmbDescription.Size = new System.Drawing.Size(296, 21);
            this._cmbDescription.TabIndex = 13;
            // 
            // _txtSufix
            // 
            this._txtSufix.Location = new System.Drawing.Point(85, 192);
            this._txtSufix.Name = "_txtSufix";
            this._txtSufix.Size = new System.Drawing.Size(189, 20);
            this._txtSufix.TabIndex = 11;
            this._txtSufix.Text = "SUFIX";
            this._txtSufix.TextChanged += new System.EventHandler(this._txtFilename_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Filename";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 131);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Description";
            // 
            // _lblPoints
            // 
            this._lblPoints.AutoSize = true;
            this._lblPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPoints.Location = new System.Drawing.Point(82, 110);
            this._lblPoints.Name = "_lblPoints";
            this._lblPoints.Size = new System.Drawing.Size(49, 13);
            this._lblPoints.TabIndex = 7;
            this._lblPoints.Text = "[points]";
            // 
            // _lblMedal
            // 
            this._lblMedal.AutoSize = true;
            this._lblMedal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblMedal.Location = new System.Drawing.Point(81, 89);
            this._lblMedal.Name = "_lblMedal";
            this._lblMedal.Size = new System.Drawing.Size(48, 13);
            this._lblMedal.TabIndex = 6;
            this._lblMedal.Text = "[medal]";
            // 
            // _lblStage
            // 
            this._lblStage.AutoSize = true;
            this._lblStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblStage.Location = new System.Drawing.Point(81, 46);
            this._lblStage.Name = "_lblStage";
            this._lblStage.Size = new System.Drawing.Size(46, 13);
            this._lblStage.TabIndex = 5;
            this._lblStage.Text = "[stage]";
            // 
            // _lblTheme
            // 
            this._lblTheme.AutoSize = true;
            this._lblTheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTheme.Location = new System.Drawing.Point(81, 26);
            this._lblTheme.Name = "_lblTheme";
            this._lblTheme.Size = new System.Drawing.Size(49, 13);
            this._lblTheme.TabIndex = 4;
            this._lblTheme.Text = "[theme]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Points:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Medal:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Stage:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Theme:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._btnCancel);
            this.panel1.Controls.Add(this._btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 238);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 36);
            this.panel1.TabIndex = 1;
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(217, 6);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 1;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _btnSave
            // 
            this._btnSave.Location = new System.Drawing.Point(136, 6);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(75, 23);
            this._btnSave.TabIndex = 0;
            this._btnSave.Text = "&Save";
            this._btnSave.UseVisualStyleBackColor = true;
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            // 
            // _dlgSave
            // 
            this._dlgSave.Filter = "Snails Gameplay Data | *.sgd";
            // 
            // _lblBuildNr
            // 
            this._lblBuildNr.AutoSize = true;
            this._lblBuildNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblBuildNr.Location = new System.Drawing.Point(81, 68);
            this._lblBuildNr.Name = "_lblBuildNr";
            this._lblBuildNr.Size = new System.Drawing.Size(55, 13);
            this._lblBuildNr.TabIndex = 19;
            this._lblBuildNr.Text = "[buildNr]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "BuildNr:";
            // 
            // SaveGameplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 277);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveGameplayForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Gameplay";
            this.Load += new System.EventHandler(this.SaveGameplayForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label _lblPoints;
        private System.Windows.Forms.Label _lblMedal;
        private System.Windows.Forms.Label _lblStage;
        private System.Windows.Forms.Label _lblTheme;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.TextBox _txtSufix;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SaveFileDialog _dlgSave;
        private System.Windows.Forms.ComboBox _cmbDescription;
        private System.Windows.Forms.TextBox _txtPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button _btnSaveDialog;
        private System.Windows.Forms.FolderBrowserDialog _dlgFolder;
        private System.Windows.Forms.Label _lbFilename;
        private System.Windows.Forms.Label _lblBuildNr;
        private System.Windows.Forms.Label label7;
    }
}