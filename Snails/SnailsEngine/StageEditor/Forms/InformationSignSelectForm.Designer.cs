namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class InformationSignSelectForm
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
            this._pnlBody = new System.Windows.Forms.Panel();
            this._pnlImage = new System.Windows.Forms.Panel();
            this._lstSigns = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this._btnOk = new System.Windows.Forms.Button();
            this._pnlBody.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlBody
            // 
            this._pnlBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._pnlBody.Controls.Add(this._pnlImage);
            this._pnlBody.Controls.Add(this._lstSigns);
            this._pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlBody.Location = new System.Drawing.Point(3, 3);
            this._pnlBody.Name = "_pnlBody";
            this._pnlBody.Padding = new System.Windows.Forms.Padding(3);
            this._pnlBody.Size = new System.Drawing.Size(437, 319);
            this._pnlBody.TabIndex = 5;
            // 
            // _pnlImage
            // 
            this._pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlImage.Location = new System.Drawing.Point(123, 3);
            this._pnlImage.Name = "_pnlImage";
            this._pnlImage.Size = new System.Drawing.Size(309, 311);
            this._pnlImage.TabIndex = 1;
            this._pnlImage.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlImage_Paint);
            // 
            // _lstSigns
            // 
            this._lstSigns.Dock = System.Windows.Forms.DockStyle.Left;
            this._lstSigns.FormattingEnabled = true;
            this._lstSigns.Location = new System.Drawing.Point(3, 3);
            this._lstSigns.Name = "_lstSigns";
            this._lstSigns.Size = new System.Drawing.Size(120, 311);
            this._lstSigns.TabIndex = 0;
            this._lstSigns.SelectedIndexChanged += new System.EventHandler(this._lstSigns_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this._btnOk);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 322);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(437, 32);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(359, 3);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 2;
            this._btnOk.Text = "&Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // InformationSignSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 357);
            this.Controls.Add(this._pnlBody);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "InformationSignSelectForm";
            this.Text = "InformationSign Selection";
            this.Load += new System.EventHandler(this.InformationSignSelectForm_Load);
            this._pnlBody.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _pnlBody;
        private System.Windows.Forms.Panel _pnlImage;
        private System.Windows.Forms.ListBox _lstSigns;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button _btnOk;
    }
}