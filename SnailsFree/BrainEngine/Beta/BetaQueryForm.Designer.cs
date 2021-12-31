namespace TwoBrainsGames.BrainEngine.Beta
{
    partial class BetaQueryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BetaQueryForm));
            this._lblTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._chkBePartOfTheBeta = new System.Windows.Forms.CheckBox();
            this._btnProceed = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._txtMail = new System.Windows.Forms.TextBox();
            this._pnlMail = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._chkDontShow = new System.Windows.Forms.CheckBox();
            this._btnExit = new System.Windows.Forms.Button();
            this._pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._pnlInfo = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this._pnlMail.SuspendLayout();
            this._pnlButtons.SuspendLayout();
            this._pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lblTitle
            // 
            this._lblTitle.AutoSize = true;
            this._lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTitle.Location = new System.Drawing.Point(14, 16);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(196, 13);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "Welcome to the %BETA_NAME%.";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 42);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label2.Size = new System.Drawing.Size(450, 236);
            this.label2.TabIndex = 1;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // _chkBePartOfTheBeta
            // 
            this._chkBePartOfTheBeta.AutoSize = true;
            this._chkBePartOfTheBeta.Location = new System.Drawing.Point(17, 281);
            this._chkBePartOfTheBeta.Name = "_chkBePartOfTheBeta";
            this._chkBePartOfTheBeta.Size = new System.Drawing.Size(303, 17);
            this._chkBePartOfTheBeta.TabIndex = 2;
            this._chkBePartOfTheBeta.Text = "I want to become a beta-tester and help develop this game";
            this._chkBePartOfTheBeta.UseVisualStyleBackColor = true;
            this._chkBePartOfTheBeta.CheckedChanged += new System.EventHandler(this._chkBePartOfTheBeta_CheckedChanged);
            // 
            // _btnProceed
            // 
            this._btnProceed.Location = new System.Drawing.Point(284, 3);
            this._btnProceed.Name = "_btnProceed";
            this._btnProceed.Size = new System.Drawing.Size(187, 23);
            this._btnProceed.TabIndex = 3;
            this._btnProceed.Text = "&Proceed";
            this._btnProceed.UseVisualStyleBackColor = true;
            this._btnProceed.Click += new System.EventHandler(this._btnProceed_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "E-Mail";
            // 
            // _txtMail
            // 
            this._txtMail.Location = new System.Drawing.Point(134, 77);
            this._txtMail.Name = "_txtMail";
            this._txtMail.Size = new System.Drawing.Size(218, 20);
            this._txtMail.TabIndex = 5;
            this._txtMail.TextChanged += new System.EventHandler(this._txtMail_TextChanged);
            // 
            // _pnlMail
            // 
            this._pnlMail.Controls.Add(this.label4);
            this._pnlMail.Controls.Add(this.label3);
            this._pnlMail.Controls.Add(this.label1);
            this._pnlMail.Controls.Add(this._txtMail);
            this._pnlMail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlMail.Location = new System.Drawing.Point(0, 327);
            this._pnlMail.Name = "_pnlMail";
            this._pnlMail.Size = new System.Drawing.Size(484, 120);
            this._pnlMail.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(37, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(315, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Please type your e-mail address in the textbox bellow. ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(393, 39);
            this.label3.TabIndex = 6;
            this.label3.Text = "A beta key and instructions will be sent to this e-mail address, please note that" +
                " we \r\nwill never ask for any passwords.\r\nYou can unsubscribe from the beta anyti" +
                "me you want.";
            // 
            // _chkDontShow
            // 
            this._chkDontShow.AutoSize = true;
            this._chkDontShow.Location = new System.Drawing.Point(17, 304);
            this._chkDontShow.Name = "_chkDontShow";
            this._chkDontShow.Size = new System.Drawing.Size(179, 17);
            this._chkDontShow.TabIndex = 8;
            this._chkDontShow.Text = "Don\'t display this message again";
            this._chkDontShow.UseVisualStyleBackColor = true;
            // 
            // _btnExit
            // 
            this._btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnExit.Location = new System.Drawing.Point(203, 3);
            this._btnExit.Name = "_btnExit";
            this._btnExit.Size = new System.Drawing.Size(75, 23);
            this._btnExit.TabIndex = 4;
            this._btnExit.Text = "&Exit";
            this._btnExit.UseVisualStyleBackColor = true;
            // 
            // _pnlButtons
            // 
            this._pnlButtons.Controls.Add(this._btnProceed);
            this._pnlButtons.Controls.Add(this._btnExit);
            this._pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this._pnlButtons.Location = new System.Drawing.Point(0, 447);
            this._pnlButtons.Name = "_pnlButtons";
            this._pnlButtons.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this._pnlButtons.Size = new System.Drawing.Size(484, 34);
            this._pnlButtons.TabIndex = 9;
            // 
            // _pnlInfo
            // 
            this._pnlInfo.Controls.Add(this._lblTitle);
            this._pnlInfo.Controls.Add(this.label2);
            this._pnlInfo.Controls.Add(this._chkBePartOfTheBeta);
            this._pnlInfo.Controls.Add(this._chkDontShow);
            this._pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlInfo.Location = new System.Drawing.Point(0, 0);
            this._pnlInfo.Name = "_pnlInfo";
            this._pnlInfo.Size = new System.Drawing.Size(484, 327);
            this._pnlInfo.TabIndex = 10;
            // 
            // BetaQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 481);
            this.Controls.Add(this._pnlInfo);
            this.Controls.Add(this._pnlMail);
            this.Controls.Add(this._pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BetaQueryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "%BETA_NAME%";
            this.Load += new System.EventHandler(this.BetaQueryForm_Load);
            this._pnlMail.ResumeLayout(false);
            this._pnlMail.PerformLayout();
            this._pnlButtons.ResumeLayout(false);
            this._pnlInfo.ResumeLayout(false);
            this._pnlInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox _chkBePartOfTheBeta;
        private System.Windows.Forms.Button _btnProceed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _txtMail;
        private System.Windows.Forms.Panel _pnlMail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox _chkDontShow;
        private System.Windows.Forms.Button _btnExit;
        private System.Windows.Forms.FlowLayoutPanel _pnlButtons;
        private System.Windows.Forms.Panel _pnlInfo;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
    }
}