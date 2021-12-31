namespace TwoBrainsGames.Snails.StageEditor.Forms
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
            this._btnSelFolder = new System.Windows.Forms.Button();
            this._txtCustomStagesFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._btnOk = new System.Windows.Forms.Button();
            this._dlgSelFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._btnSelFolder);
            this.groupBox1.Controls.Add(this._txtCustomStagesFolder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 95);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // _btnSelFolder
            // 
            this._btnSelFolder.Location = new System.Drawing.Point(401, 42);
            this._btnSelFolder.Name = "_btnSelFolder";
            this._btnSelFolder.Size = new System.Drawing.Size(30, 23);
            this._btnSelFolder.TabIndex = 2;
            this._btnSelFolder.Text = "...";
            this._btnSelFolder.UseVisualStyleBackColor = true;
            this._btnSelFolder.Click += new System.EventHandler(this._btnSelFolder_Click);
            // 
            // _txtCustomStagesFolder
            // 
            this._txtCustomStagesFolder.Location = new System.Drawing.Point(135, 44);
            this._txtCustomStagesFolder.Name = "_txtCustomStagesFolder";
            this._txtCustomStagesFolder.Size = new System.Drawing.Size(260, 20);
            this._txtCustomStagesFolder.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Custom stages folder";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 95);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 35);
            this.panel1.TabIndex = 3;
            // 
            // _btnOk
            // 
            this._btnOk.Location = new System.Drawing.Point(193, 6);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 6;
            this._btnOk.Text = "&Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 130);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "PreferencesForm";
            this.Text = "Preferences";
            this.Load += new System.EventHandler(this.PreferencesForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.TextBox _txtCustomStagesFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _btnSelFolder;
        private System.Windows.Forms.FolderBrowserDialog _dlgSelFolder;
    }
}