namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class PropPropsForm
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
            this._cbPropType = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this._cbRotation = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._cbPropType);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this.label7);
            this._gbProps.Controls.Add(this._cbRotation);
            this._gbProps.Size = new System.Drawing.Size(251, 75);
            // 
            // _cbPropType
            // 
            this._cbPropType.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.Rotation;
            this._cbPropType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbPropType.FormattingEnabled = true;
            this._cbPropType.Location = new System.Drawing.Point(83, 19);
            this._cbPropType.Name = "_cbPropType";
            this._cbPropType.Size = new System.Drawing.Size(121, 21);
            this._cbPropType.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Tipo";
            // 
            // _cbRotation
            // 
            this._cbRotation.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.Rotation;
            this._cbRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbRotation.FormattingEnabled = true;
            this._cbRotation.Location = new System.Drawing.Point(83, 46);
            this._cbRotation.Name = "_cbRotation";
            this._cbRotation.Size = new System.Drawing.Size(121, 21);
            this._cbRotation.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Rotation";
            // 
            // PropPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 345);
            this.Name = "PropPropsForm";
            this.Text = "Prop Properties";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PropPropsForm_FormClosed);
            this.Load += new System.EventHandler(this.PropPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox _cbRotation;
        private TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox _cbPropType;
        private System.Windows.Forms.Label label7;
    }
}