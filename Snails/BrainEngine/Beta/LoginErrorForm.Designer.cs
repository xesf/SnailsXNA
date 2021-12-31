namespace TwoBrainsGames.BrainEngine.Beta
{
    partial class LoginErrorForm
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
            this._chkPlayNormalyNowOn = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._btnOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._bntRetry = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._chkPlayNormalyNowOn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 145);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // _chkPlayNormalyNowOn
            // 
            this._chkPlayNormalyNowOn.AutoSize = true;
            this._chkPlayNormalyNowOn.Location = new System.Drawing.Point(40, 107);
            this._chkPlayNormalyNowOn.Name = "_chkPlayNormalyNowOn";
            this._chkPlayNormalyNowOn.Size = new System.Drawing.Size(213, 17);
            this._chkPlayNormalyNowOn.TabIndex = 2;
            this._chkPlayNormalyNowOn.Text = "I don\'t want to be a beta-tester anymore";
            this._chkPlayNormalyNowOn.UseVisualStyleBackColor = true;
            this._chkPlayNormalyNowOn.CheckedChanged += new System.EventHandler(this._chkPlayNormalyNowOn_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(393, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "There was an error while trying to login, or the login was cancelled. ";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(37, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(485, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "Do you want to play the game as a regular player? \r\n(Gameplay data will not be se" +
                "nt to our servers, so you will not be playing as a beta-tester).";
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(331, 3);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(106, 23);
            this._btnOk.TabIndex = 1;
            this._btnOk.Text = "&Play anyway!";
            this._btnOk.UseVisualStyleBackColor = true;
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this._btnCancel.Location = new System.Drawing.Point(107, 3);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(106, 23);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "&Quit";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _bntRetry
            // 
            this._bntRetry.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this._bntRetry.Location = new System.Drawing.Point(219, 3);
            this._bntRetry.Name = "_bntRetry";
            this._bntRetry.Size = new System.Drawing.Size(106, 23);
            this._bntRetry.TabIndex = 3;
            this._bntRetry.Text = "&Retry";
            this._bntRetry.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._btnCancel);
            this.panel1.Controls.Add(this._bntRetry);
            this.panel1.Controls.Add(this._btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(10, 161);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 26);
            this.panel1.TabIndex = 4;
            // 
            // LoginErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 197);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginErrorForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login error";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Button _bntRetry;
        private System.Windows.Forms.CheckBox _chkPlayNormalyNowOn;
        private System.Windows.Forms.Panel panel1;
    }
}