namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class PickableSelectForm
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
            this._pnlImage = new System.Windows.Forms.Panel();
            this._btnOk = new System.Windows.Forms.Button();
            this._lstPickTypes = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlImage
            // 
            this._pnlImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlImage.Location = new System.Drawing.Point(120, 0);
            this._pnlImage.Name = "_pnlImage";
            this._pnlImage.Size = new System.Drawing.Size(250, 306);
            this._pnlImage.TabIndex = 6;
            this._pnlImage.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlImage_Paint);
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(292, 3);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 5;
            this._btnOk.Text = "&Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _lstPickTypes
            // 
            this._lstPickTypes.Dock = System.Windows.Forms.DockStyle.Left;
            this._lstPickTypes.FormattingEnabled = true;
            this._lstPickTypes.Location = new System.Drawing.Point(0, 0);
            this._lstPickTypes.Name = "_lstPickTypes";
            this._lstPickTypes.Size = new System.Drawing.Size(120, 306);
            this._lstPickTypes.TabIndex = 4;
            this._lstPickTypes.SelectedIndexChanged += new System.EventHandler(this._lstPickTypes_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this._btnOk);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 306);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(370, 34);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // PickableSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 340);
            this.Controls.Add(this._pnlImage);
            this.Controls.Add(this._lstPickTypes);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PickableSelectForm";
            this.Text = "PickableObject Selection";
            this.Load += new System.EventHandler(this.PickableSelectForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _pnlImage;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.ListBox _lstPickTypes;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}