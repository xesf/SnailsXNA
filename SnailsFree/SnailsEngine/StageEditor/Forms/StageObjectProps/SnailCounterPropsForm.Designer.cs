namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class SnailCounterPropsForm
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
            this._cmbLinkTo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._cmbLinkTo);
            this._gbProps.Controls.Add(this.label7);
            this._gbProps.Size = new System.Drawing.Size(226, 54);
            // 
            // _cbSpriteEffect
            // 
            this._cbSpriteEffect.Items.AddRange(new object[] {
            Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
            Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally,
            Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically});
            // 
            // _cmbLinkTo
            // 
            this._cmbLinkTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbLinkTo.FormattingEnabled = true;
            this._cmbLinkTo.Location = new System.Drawing.Point(83, 19);
            this._cmbLinkTo.Name = "_cmbLinkTo";
            this._cmbLinkTo.Size = new System.Drawing.Size(121, 21);
            this._cmbLinkTo.Sorted = true;
            this._cmbLinkTo.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Link With";
            // 
            // SnailCounterPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 159);
            this.Name = "SnailCounterPropsForm";
            this.Text = "SnailCounter Properties";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SnailCounterPropsForm_FormClosed);
            this.Load += new System.EventHandler(this.SnailCounterPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox _cmbLinkTo;
        private System.Windows.Forms.Label label7;
    }
}