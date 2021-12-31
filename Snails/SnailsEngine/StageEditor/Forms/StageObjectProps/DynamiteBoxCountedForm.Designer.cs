namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class DynamiteBoxCountedForm
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
            this.label2 = new System.Windows.Forms.Label();
            this._numSnailsAllowed = new System.Windows.Forms.NumericUpDown();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numSnailsAllowed)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._numSnailsAllowed);
            this._gbProps.Size = new System.Drawing.Size(273, 48);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Snails Allowed to Pass";
            // 
            // _numSnailsAllowed
            // 
            this._numSnailsAllowed.Location = new System.Drawing.Point(140, 19);
            this._numSnailsAllowed.Name = "_numSnailsAllowed";
            this._numSnailsAllowed.Size = new System.Drawing.Size(66, 20);
            this._numSnailsAllowed.TabIndex = 5;
            // 
            // DynamiteBoxCountedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 318);
            this.Name = "DynamiteBoxCountedForm";
            this.Text = "DynamiteBoxCountedForm";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numSnailsAllowed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _numSnailsAllowed;
    }
}