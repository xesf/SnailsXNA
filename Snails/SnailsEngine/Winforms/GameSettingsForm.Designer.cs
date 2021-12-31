namespace TwoBrainsGames.Snails.Winforms
{
    partial class GameSettingsForm
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
            this._btnOk = new System.Windows.Forms.Button();
            this._chkShowFps = new System.Windows.Forms.CheckBox();
            this._cmbStartup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._cmbSelectedTheme = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this._numSelectedStage = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this._chkFullscreen = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._chkWinAlwaysActive = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this._cmbMode = new System.Windows.Forms.ComboBox();
            this._chkDeleteProfile = new System.Windows.Forms.CheckBox();
            this._chkShowSpriteFrame = new System.Windows.Forms.CheckBox();
            this._chkShowBB = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this._chkUnlockAll = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._numSelectedStage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(147, 339);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 0;
            this._btnOk.Text = "&Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _chkShowFps
            // 
            this._chkShowFps.AutoSize = true;
            this._chkShowFps.Location = new System.Drawing.Point(23, 113);
            this._chkShowFps.Name = "_chkShowFps";
            this._chkShowFps.Size = new System.Drawing.Size(115, 17);
            this._chkShowFps.TabIndex = 2;
            this._chkShowFps.Text = "Show FPS counter";
            this._chkShowFps.UseVisualStyleBackColor = true;
            // 
            // _cmbStartup
            // 
            this._cmbStartup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbStartup.FormattingEnabled = true;
            this._cmbStartup.Location = new System.Drawing.Point(111, 31);
            this._cmbStartup.Name = "_cmbStartup";
            this._cmbStartup.Size = new System.Drawing.Size(121, 21);
            this._cmbStartup.TabIndex = 3;
            this._cmbStartup.SelectedIndexChanged += new System.EventHandler(this._cmbStartup_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Start game in";
            // 
            // _cmbSelectedTheme
            // 
            this._cmbSelectedTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbSelectedTheme.FormattingEnabled = true;
            this._cmbSelectedTheme.Location = new System.Drawing.Point(108, 262);
            this._cmbSelectedTheme.Name = "_cmbSelectedTheme";
            this._cmbSelectedTheme.Size = new System.Drawing.Size(121, 21);
            this._cmbSelectedTheme.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 265);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Selected Theme";
            // 
            // _numSelectedStage
            // 
            this._numSelectedStage.Location = new System.Drawing.Point(108, 289);
            this._numSelectedStage.Name = "_numSelectedStage";
            this._numSelectedStage.Size = new System.Drawing.Size(55, 20);
            this._numSelectedStage.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 291);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Selected Stage";
            // 
            // _chkFullscreen
            // 
            this._chkFullscreen.AutoSize = true;
            this._chkFullscreen.Location = new System.Drawing.Point(23, 89);
            this._chkFullscreen.Name = "_chkFullscreen";
            this._chkFullscreen.Size = new System.Drawing.Size(74, 17);
            this._chkFullscreen.TabIndex = 9;
            this._chkFullscreen.Text = "Fullscreen";
            this._chkFullscreen.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._chkUnlockAll);
            this.groupBox1.Controls.Add(this._chkWinAlwaysActive);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this._cmbMode);
            this.groupBox1.Controls.Add(this._chkDeleteProfile);
            this.groupBox1.Controls.Add(this._chkShowSpriteFrame);
            this.groupBox1.Controls.Add(this._chkShowBB);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this._chkFullscreen);
            this.groupBox1.Controls.Add(this._cmbSelectedTheme);
            this.groupBox1.Controls.Add(this._chkShowFps);
            this.groupBox1.Controls.Add(this._numSelectedStage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this._cmbStartup);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 321);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // _chkWinAlwaysActive
            // 
            this._chkWinAlwaysActive.AutoSize = true;
            this._chkWinAlwaysActive.Checked = true;
            this._chkWinAlwaysActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkWinAlwaysActive.Location = new System.Drawing.Point(23, 205);
            this._chkWinAlwaysActive.Name = "_chkWinAlwaysActive";
            this._chkWinAlwaysActive.Size = new System.Drawing.Size(134, 17);
            this._chkWinAlwaysActive.TabIndex = 16;
            this._chkWinAlwaysActive.Text = "Window Always Active";
            this._chkWinAlwaysActive.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Gameplay mode";
            // 
            // _cmbMode
            // 
            this._cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbMode.FormattingEnabled = true;
            this._cmbMode.Location = new System.Drawing.Point(111, 58);
            this._cmbMode.Name = "_cmbMode";
            this._cmbMode.Size = new System.Drawing.Size(121, 21);
            this._cmbMode.TabIndex = 14;
            // 
            // _chkDeleteProfile
            // 
            this._chkDeleteProfile.AutoSize = true;
            this._chkDeleteProfile.Location = new System.Drawing.Point(23, 182);
            this._chkDeleteProfile.Name = "_chkDeleteProfile";
            this._chkDeleteProfile.Size = new System.Drawing.Size(121, 17);
            this._chkDeleteProfile.TabIndex = 13;
            this._chkDeleteProfile.Text = "Delete Player Profile";
            this._chkDeleteProfile.UseVisualStyleBackColor = true;
            // 
            // _chkShowSpriteFrame
            // 
            this._chkShowSpriteFrame.AutoSize = true;
            this._chkShowSpriteFrame.Location = new System.Drawing.Point(23, 159);
            this._chkShowSpriteFrame.Name = "_chkShowSpriteFrame";
            this._chkShowSpriteFrame.Size = new System.Drawing.Size(152, 17);
            this._chkShowSpriteFrame.TabIndex = 12;
            this._chkShowSpriteFrame.Text = "Show sprite frame (orange)";
            this._chkShowSpriteFrame.UseVisualStyleBackColor = true;
            // 
            // _chkShowBB
            // 
            this._chkShowBB.AutoSize = true;
            this._chkShowBB.Location = new System.Drawing.Point(23, 136);
            this._chkShowBB.Name = "_chkShowBB";
            this._chkShowBB.Size = new System.Drawing.Size(155, 17);
            this._chkShowBB.TabIndex = 11;
            this._chkShowBB.Text = "Show bounding boxes (red)";
            this._chkShowBB.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(310, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "(Only valid when game starts directly from Stage Briefing Screen)";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // _chkUnlockAll
            // 
            this._chkUnlockAll.AutoSize = true;
            this._chkUnlockAll.Location = new System.Drawing.Point(205, 89);
            this._chkUnlockAll.Name = "_chkUnlockAll";
            this._chkUnlockAll.Size = new System.Drawing.Size(103, 17);
            this._chkUnlockAll.TabIndex = 17;
            this._chkUnlockAll.Text = "Unlock all levels";
            this._chkUnlockAll.UseVisualStyleBackColor = true;
            // 
            // GameSettingsForm
            // 
            this.AcceptButton = this._btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 370);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameSettingsForm";
            this.Load += new System.EventHandler(this.GameSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._numSelectedStage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.CheckBox _chkShowFps;
        private System.Windows.Forms.ComboBox _cmbStartup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _cmbSelectedTheme;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _numSelectedStage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox _chkFullscreen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox _chkShowBB;
        private System.Windows.Forms.CheckBox _chkShowSpriteFrame;
        private System.Windows.Forms.CheckBox _chkDeleteProfile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox _cmbMode;
        private System.Windows.Forms.CheckBox _chkWinAlwaysActive;
        private System.Windows.Forms.CheckBox _chkUnlockAll;
    }
}