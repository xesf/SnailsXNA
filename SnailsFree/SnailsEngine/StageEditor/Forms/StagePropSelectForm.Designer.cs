namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class StagePropSelectForm
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
            this._pnlBody = new System.Windows.Forms.Panel();
            this._pnlImage = new System.Windows.Forms.Panel();
            this._lstStageProps = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this._pnlBody.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(345, 3);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 2;
            this._btnOk.Text = "&Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _pnlBody
            // 
            this._pnlBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._pnlBody.Controls.Add(this._pnlImage);
            this._pnlBody.Controls.Add(this._lstStageProps);
            this._pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlBody.Location = new System.Drawing.Point(3, 3);
            this._pnlBody.Name = "_pnlBody";
            this._pnlBody.Padding = new System.Windows.Forms.Padding(3);
            this._pnlBody.Size = new System.Drawing.Size(423, 302);
            this._pnlBody.TabIndex = 3;
            this._pnlBody.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlImage_Paint);
            // 
            // _pnlImage
            // 
            this._pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlImage.Location = new System.Drawing.Point(123, 3);
            this._pnlImage.Name = "_pnlImage";
            this._pnlImage.Size = new System.Drawing.Size(295, 294);
            this._pnlImage.TabIndex = 1;
            this._pnlImage.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlImage_Paint);
            // 
            // _lstStageProps
            // 
            this._lstStageProps.Dock = System.Windows.Forms.DockStyle.Left;
            this._lstStageProps.FormattingEnabled = true;
            this._lstStageProps.Location = new System.Drawing.Point(3, 3);
            this._lstStageProps.Name = "_lstStageProps";
            this._lstStageProps.Size = new System.Drawing.Size(120, 294);
            this._lstStageProps.TabIndex = 0;
            this._lstStageProps.SelectedIndexChanged += new System.EventHandler(this._lstStageProps_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this._btnOk);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 305);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(423, 32);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // StagePropSelectForm
            // 
            this.AcceptButton = this._btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 340);
            this.Controls.Add(this._pnlBody);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "StagePropSelectForm";
            this.Text = "StageProp Selection";
            this.Load += new System.EventHandler(this.PropSelectForm_Load);
            this._pnlBody.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Panel _pnlBody;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel _pnlImage;
        private System.Windows.Forms.ListBox _lstStageProps;
    }
}