namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class ObjectPropsBaseForm
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
            this._pnlFooter = new System.Windows.Forms.FlowLayoutPanel();
            this._btOk = new System.Windows.Forms.Button();
            this._gbProps = new System.Windows.Forms.GroupBox();
            this._lblId = new System.Windows.Forms.Label();
            this._txtUniqueId = new System.Windows.Forms.TextBox();
            this._gbCommon = new System.Windows.Forms.GroupBox();
            this._pnlLinks = new System.Windows.Forms.Panel();
            this._objLinks = new TwoBrainsGames.Snails.StageEditor.Controls.EditableObjectList();
            this._pnlRotation = new System.Windows.Forms.Panel();
            this._numRotation = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this._pnlSpriteEffect = new System.Windows.Forms.Panel();
            this._cbSpriteEffect = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._lblSpriteEffect = new System.Windows.Forms.Label();
            this._pnlId = new System.Windows.Forms.Panel();
            this._pnlFooter.SuspendLayout();
            this._gbCommon.SuspendLayout();
            this._pnlLinks.SuspendLayout();
            this._pnlRotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numRotation)).BeginInit();
            this._pnlSpriteEffect.SuspendLayout();
            this._pnlId.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlFooter
            // 
            this._pnlFooter.Controls.Add(this._btOk);
            this._pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlFooter.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this._pnlFooter.Location = new System.Drawing.Point(5, 337);
            this._pnlFooter.Name = "_pnlFooter";
            this._pnlFooter.Size = new System.Drawing.Size(273, 31);
            this._pnlFooter.TabIndex = 0;
            // 
            // _btOk
            // 
            this._btOk.Location = new System.Drawing.Point(195, 3);
            this._btOk.Name = "_btOk";
            this._btOk.Size = new System.Drawing.Size(75, 23);
            this._btOk.TabIndex = 0;
            this._btOk.Text = "&Ok";
            this._btOk.UseVisualStyleBackColor = true;
            this._btOk.Click += new System.EventHandler(this._btOk_Click);
            // 
            // _gbProps
            // 
            this._gbProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbProps.Location = new System.Drawing.Point(5, 234);
            this._gbProps.Name = "_gbProps";
            this._gbProps.Size = new System.Drawing.Size(273, 103);
            this._gbProps.TabIndex = 1;
            this._gbProps.TabStop = false;
            // 
            // _lblId
            // 
            this._lblId.Location = new System.Drawing.Point(35, 4);
            this._lblId.Name = "_lblId";
            this._lblId.Size = new System.Drawing.Size(35, 17);
            this._lblId.TabIndex = 16;
            this._lblId.Text = "Id";
            this._lblId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _txtUniqueId
            // 
            this._txtUniqueId.Location = new System.Drawing.Point(76, 3);
            this._txtUniqueId.Name = "_txtUniqueId";
            this._txtUniqueId.Size = new System.Drawing.Size(121, 20);
            this._txtUniqueId.TabIndex = 11;
            // 
            // _gbCommon
            // 
            this._gbCommon.Controls.Add(this._pnlLinks);
            this._gbCommon.Controls.Add(this._pnlRotation);
            this._gbCommon.Controls.Add(this._pnlSpriteEffect);
            this._gbCommon.Controls.Add(this._pnlId);
            this._gbCommon.Dock = System.Windows.Forms.DockStyle.Top;
            this._gbCommon.Location = new System.Drawing.Point(5, 5);
            this._gbCommon.Name = "_gbCommon";
            this._gbCommon.Size = new System.Drawing.Size(273, 229);
            this._gbCommon.TabIndex = 2;
            this._gbCommon.TabStop = false;
            this._gbCommon.Text = "StageObject Properties";
            // 
            // _pnlLinks
            // 
            this._pnlLinks.Controls.Add(this._objLinks);
            this._pnlLinks.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlLinks.Location = new System.Drawing.Point(3, 97);
            this._pnlLinks.Name = "_pnlLinks";
            this._pnlLinks.Size = new System.Drawing.Size(267, 128);
            this._pnlLinks.TabIndex = 23;
            // 
            // _objLinks
            // 
            this._objLinks.Location = new System.Drawing.Point(8, 6);
            this._objLinks.Name = "_objLinks";
            this._objLinks.ObjectTypeFilter = null;
            this._objLinks.Size = new System.Drawing.Size(225, 114);
            this._objLinks.TabIndex = 20;
            this._objLinks.Text = "Object links";
            // 
            // _pnlRotation
            // 
            this._pnlRotation.Controls.Add(this._numRotation);
            this._pnlRotation.Controls.Add(this.label1);
            this._pnlRotation.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlRotation.Location = new System.Drawing.Point(3, 70);
            this._pnlRotation.Name = "_pnlRotation";
            this._pnlRotation.Size = new System.Drawing.Size(267, 27);
            this._pnlRotation.TabIndex = 24;
            // 
            // _numRotation
            // 
            this._numRotation.Location = new System.Drawing.Point(76, 4);
            this._numRotation.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this._numRotation.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this._numRotation.Name = "_numRotation";
            this._numRotation.Size = new System.Drawing.Size(46, 20);
            this._numRotation.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Rotation";
            // 
            // _pnlSpriteEffect
            // 
            this._pnlSpriteEffect.Controls.Add(this._cbSpriteEffect);
            this._pnlSpriteEffect.Controls.Add(this._lblSpriteEffect);
            this._pnlSpriteEffect.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlSpriteEffect.Location = new System.Drawing.Point(3, 43);
            this._pnlSpriteEffect.Name = "_pnlSpriteEffect";
            this._pnlSpriteEffect.Size = new System.Drawing.Size(267, 27);
            this._pnlSpriteEffect.TabIndex = 22;
            // 
            // _cbSpriteEffect
            // 
            this._cbSpriteEffect.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.SpriteEffect;
            this._cbSpriteEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbSpriteEffect.FormattingEnabled = true;
            this._cbSpriteEffect.Location = new System.Drawing.Point(76, 3);
            this._cbSpriteEffect.Name = "_cbSpriteEffect";
            this._cbSpriteEffect.Size = new System.Drawing.Size(121, 21);
            this._cbSpriteEffect.TabIndex = 19;
            // 
            // _lblSpriteEffect
            // 
            this._lblSpriteEffect.AutoSize = true;
            this._lblSpriteEffect.Location = new System.Drawing.Point(5, 6);
            this._lblSpriteEffect.Name = "_lblSpriteEffect";
            this._lblSpriteEffect.Size = new System.Drawing.Size(65, 13);
            this._lblSpriteEffect.TabIndex = 18;
            this._lblSpriteEffect.Text = "Sprite Effect";
            // 
            // _pnlId
            // 
            this._pnlId.Controls.Add(this._txtUniqueId);
            this._pnlId.Controls.Add(this._lblId);
            this._pnlId.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlId.Location = new System.Drawing.Point(3, 16);
            this._pnlId.Name = "_pnlId";
            this._pnlId.Size = new System.Drawing.Size(267, 27);
            this._pnlId.TabIndex = 21;
            // 
            // ObjectPropsBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 373);
            this.Controls.Add(this._gbProps);
            this.Controls.Add(this._gbCommon);
            this.Controls.Add(this._pnlFooter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "ObjectPropsBaseForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "ObjectPropsBaseForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ObjectPropsBaseForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ObjectPropsBaseForm_FormClosed);
            this.Load += new System.EventHandler(this.ObjectPropsBaseForm_Load);
            this.Shown += new System.EventHandler(this.ObjectPropsBaseForm_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ObjectPropsBaseForm_KeyPress);
            this._pnlFooter.ResumeLayout(false);
            this._gbCommon.ResumeLayout(false);
            this._pnlLinks.ResumeLayout(false);
            this._pnlRotation.ResumeLayout(false);
            this._pnlRotation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numRotation)).EndInit();
            this._pnlSpriteEffect.ResumeLayout(false);
            this._pnlSpriteEffect.PerformLayout();
            this._pnlId.ResumeLayout(false);
            this._pnlId.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel _pnlFooter;
        private System.Windows.Forms.Button _btOk;
        protected System.Windows.Forms.GroupBox _gbProps;
        private System.Windows.Forms.GroupBox _gbCommon;
        private System.Windows.Forms.Panel _pnlLinks;
        private Controls.EditableObjectList _objLinks;
        private System.Windows.Forms.Panel _pnlSpriteEffect;
        private System.Windows.Forms.Panel _pnlId;
        private System.Windows.Forms.TextBox _txtUniqueId;
        private System.Windows.Forms.Label _lblId;
        private System.Windows.Forms.Label _lblSpriteEffect;
        private Controls.BaseComboBox _cbSpriteEffect;
        private System.Windows.Forms.Panel _pnlRotation;
        private System.Windows.Forms.NumericUpDown _numRotation;
        private System.Windows.Forms.Label label1;
    }
}