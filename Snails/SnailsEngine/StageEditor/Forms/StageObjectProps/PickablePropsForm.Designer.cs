namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class PickablePropsForm
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
            this._numQuantity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._numQuantity);
            this._gbProps.Controls.Add(this.label1);
            this._gbProps.Size = new System.Drawing.Size(249, 47);
            // 
            // _numQuantity
            // 
            this._numQuantity.Location = new System.Drawing.Point(79, 19);
            this._numQuantity.Maximum = new decimal(new int[] {
            35000,
            0,
            0,
            0});
            this._numQuantity.Name = "_numQuantity";
            this._numQuantity.Size = new System.Drawing.Size(66, 20);
            this._numQuantity.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Quantity";
            // 
            // PickablePropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 317);
            this.Name = "PickablePropsForm";
            this.Text = "PickablePropsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PickablePropsForm_FormClosed);
            this.Load += new System.EventHandler(this.PickablePropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numQuantity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown _numQuantity;
    }
}