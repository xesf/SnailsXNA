namespace TwoBrainsGames.Snails.Winforms
{
    partial class LoadGameplayForm
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
            this.label4 = new System.Windows.Forms.Label();
            this._lblBuildNr = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._lstFiles = new System.Windows.Forms.ListBox();
            this._btnSaveDialog = new System.Windows.Forms.Button();
            this._txtPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._lblStage = new System.Windows.Forms.Label();
            this._lblTheme = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnLoad = new System.Windows.Forms.Button();
            this._dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._lblDateTime = new System.Windows.Forms.Label();
            this._lblTimeTaken = new System.Windows.Forms.Label();
            this._lblMedal = new System.Windows.Forms.Label();
            this._lblScore = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this._lblBuildNr);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this._lstFiles);
            this.groupBox1.Controls.Add(this._btnSaveDialog);
            this.groupBox1.Controls.Add(this._txtPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this._lblStage);
            this.groupBox1.Controls.Add(this._lblTheme);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(524, 257);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Saves";
            // 
            // _lblBuildNr
            // 
            this._lblBuildNr.AutoSize = true;
            this._lblBuildNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblBuildNr.Location = new System.Drawing.Point(62, 68);
            this._lblBuildNr.Name = "_lblBuildNr";
            this._lblBuildNr.Size = new System.Drawing.Size(55, 13);
            this._lblBuildNr.TabIndex = 16;
            this._lblBuildNr.Text = "[buildNr]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "BuildNr:";
            // 
            // _lstFiles
            // 
            this._lstFiles.FormattingEnabled = true;
            this._lstFiles.Location = new System.Drawing.Point(21, 135);
            this._lstFiles.Name = "_lstFiles";
            this._lstFiles.Size = new System.Drawing.Size(310, 108);
            this._lstFiles.TabIndex = 14;
            this._lstFiles.SelectedIndexChanged += new System.EventHandler(this._lstFiles_SelectedIndexChanged);
            // 
            // _btnSaveDialog
            // 
            this._btnSaveDialog.Location = new System.Drawing.Point(303, 87);
            this._btnSaveDialog.Name = "_btnSaveDialog";
            this._btnSaveDialog.Size = new System.Drawing.Size(32, 20);
            this._btnSaveDialog.TabIndex = 13;
            this._btnSaveDialog.Text = "...";
            this._btnSaveDialog.UseVisualStyleBackColor = true;
            this._btnSaveDialog.Click += new System.EventHandler(this._btnSaveDialog_Click);
            // 
            // _txtPath
            // 
            this._txtPath.Location = new System.Drawing.Point(65, 87);
            this._txtPath.Name = "_txtPath";
            this._txtPath.Size = new System.Drawing.Size(232, 20);
            this._txtPath.TabIndex = 8;
            this._txtPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this._txtPath_KeyDown);
            this._txtPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._txtPath_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Path:";
            // 
            // _lblStage
            // 
            this._lblStage.AutoSize = true;
            this._lblStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblStage.Location = new System.Drawing.Point(62, 47);
            this._lblStage.Name = "_lblStage";
            this._lblStage.Size = new System.Drawing.Size(46, 13);
            this._lblStage.TabIndex = 5;
            this._lblStage.Text = "[stage]";
            // 
            // _lblTheme
            // 
            this._lblTheme.AutoSize = true;
            this._lblTheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTheme.Location = new System.Drawing.Point(62, 26);
            this._lblTheme.Name = "_lblTheme";
            this._lblTheme.Size = new System.Drawing.Size(49, 13);
            this._lblTheme.TabIndex = 4;
            this._lblTheme.Text = "[theme]";
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
            this.panel1.Controls.Add(this._btnLoad);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 260);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(524, 36);
            this.panel1.TabIndex = 3;
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(265, 6);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 1;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _btnLoad
            // 
            this._btnLoad.Enabled = false;
            this._btnLoad.Location = new System.Drawing.Point(184, 6);
            this._btnLoad.Name = "_btnLoad";
            this._btnLoad.Size = new System.Drawing.Size(75, 23);
            this._btnLoad.TabIndex = 0;
            this._btnLoad.Text = "&Load";
            this._btnLoad.UseVisualStyleBackColor = true;
            this._btnLoad.Click += new System.EventHandler(this._btnLoad_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._lblScore);
            this.groupBox2.Controls.Add(this._lblMedal);
            this.groupBox2.Controls.Add(this._lblTimeTaken);
            this.groupBox2.Controls.Add(this._lblDateTime);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(338, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 108);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Details";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Time Taken:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Medal:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Score:";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Date/Time:";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // _lblDateTime
            // 
            this._lblDateTime.AutoSize = true;
            this._lblDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblDateTime.Location = new System.Drawing.Point(73, 27);
            this._lblDateTime.Name = "_lblDateTime";
            this._lblDateTime.Size = new System.Drawing.Size(83, 13);
            this._lblDateTime.TabIndex = 5;
            this._lblDateTime.Text = "[aaaa-mm-dd]";
            // 
            // _lblTimeTaken
            // 
            this._lblTimeTaken.AutoSize = true;
            this._lblTimeTaken.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTimeTaken.Location = new System.Drawing.Point(73, 44);
            this._lblTimeTaken.Name = "_lblTimeTaken";
            this._lblTimeTaken.Size = new System.Drawing.Size(71, 13);
            this._lblTimeTaken.TabIndex = 6;
            this._lblTimeTaken.Text = "[mm:mm:ss]";
            // 
            // _lblMedal
            // 
            this._lblMedal.AutoSize = true;
            this._lblMedal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblMedal.Location = new System.Drawing.Point(73, 61);
            this._lblMedal.Name = "_lblMedal";
            this._lblMedal.Size = new System.Drawing.Size(48, 13);
            this._lblMedal.TabIndex = 7;
            this._lblMedal.Text = "[medal]";
            // 
            // _lblScore
            // 
            this._lblScore.AutoSize = true;
            this._lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblScore.Location = new System.Drawing.Point(73, 78);
            this._lblScore.Name = "_lblScore";
            this._lblScore.Size = new System.Drawing.Size(46, 13);
            this._lblScore.TabIndex = 8;
            this._lblScore.Text = "[score]";
            // 
            // LoadGameplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 299);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadGameplayForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Gameplay";
            this.Load += new System.EventHandler(this.LoadGameplayForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label _lblStage;
        private System.Windows.Forms.Label _lblTheme;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Button _btnLoad;
        private System.Windows.Forms.TextBox _txtPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button _btnSaveDialog;
        private System.Windows.Forms.ListBox _lstFiles;
        private System.Windows.Forms.FolderBrowserDialog _dlgFolder;
        private System.Windows.Forms.Label _lblBuildNr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label _lblScore;
        private System.Windows.Forms.Label _lblMedal;
        private System.Windows.Forms.Label _lblTimeTaken;
        private System.Windows.Forms.Label _lblDateTime;
    }
}