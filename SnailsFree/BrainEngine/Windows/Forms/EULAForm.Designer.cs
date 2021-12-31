namespace TwoBrainsGames.BrainEngine.Windows.Forms
{
    partial class EULAForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EULAForm));
            this._lblTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._btnDisagree = new System.Windows.Forms.Button();
            this._btnAgree = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lblTitle
            // 
            this._lblTitle.AutoSize = true;
            this._lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTitle.Location = new System.Drawing.Point(12, 9);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(119, 13);
            this._lblTitle.TabIndex = 10;
            this._lblTitle.Text = "Software Disclaimer";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(438, 233);
            this.label2.TabIndex = 11;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // _btnDisagree
            // 
            this._btnDisagree.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnDisagree.Location = new System.Drawing.Point(234, 8);
            this._btnDisagree.Name = "_btnDisagree";
            this._btnDisagree.Size = new System.Drawing.Size(75, 23);
            this._btnDisagree.TabIndex = 3;
            this._btnDisagree.Text = "&I Disagree";
            this._btnDisagree.UseVisualStyleBackColor = true;
            // 
            // _btnAgree
            // 
            this._btnAgree.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnAgree.Location = new System.Drawing.Point(153, 8);
            this._btnAgree.Name = "_btnAgree";
            this._btnAgree.Size = new System.Drawing.Size(75, 23);
            this._btnAgree.TabIndex = 4;
            this._btnAgree.Text = "&I Agree";
            this._btnAgree.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._btnDisagree);
            this.panel1.Controls.Add(this._btnAgree);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 307);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 41);
            this.panel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(438, 31);
            this.label1.TabIndex = 13;
            this.label1.Text = "Please read the followind disclaimer. You have to agree with this terms in order " +
                "to use this software.";
            // 
            // EULAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 348);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._lblTitle);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EULAForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Snails Disclaimer";
            this.Load += new System.EventHandler(this.EULAForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _btnDisagree;
        private System.Windows.Forms.Button _btnAgree;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}