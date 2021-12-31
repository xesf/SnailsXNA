namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class CrystalPropsForm
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
            this._cmbColor = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._cmbSize = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._cmbSize);
            this._gbProps.Controls.Add(this.label1);
            this._gbProps.Controls.Add(this._cmbColor);
            this._gbProps.Size = new System.Drawing.Size(273, 67);
            // 
            // _cmbColor
            // 
            this._cmbColor.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.CrystalColors;
            this._cmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbColor.FormattingEnabled = true;
            this._cmbColor.Location = new System.Drawing.Point(79, 13);
            this._cmbColor.Name = "_cmbColor";
            this._cmbColor.Size = new System.Drawing.Size(121, 21);
            this._cmbColor.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Size";
            // 
            // _cmbSize
            // 
            this._cmbSize.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.CrystalSizes;
            this._cmbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbSize.FormattingEnabled = true;
            this._cmbSize.Location = new System.Drawing.Point(79, 40);
            this._cmbSize.Name = "_cmbSize";
            this._cmbSize.Size = new System.Drawing.Size(121, 21);
            this._cmbSize.TabIndex = 2;
            // 
            // CrystalPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 337);
            this.Name = "CrystalPropsForm";
            this.Text = "Crystal";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CrystalPropsForm_FormClosed);
            this.Load += new System.EventHandler(this.CrystalPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.BaseComboBox _cmbColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.BaseComboBox _cmbSize;
    }
}