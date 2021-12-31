namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class LaserBeamMirrorPropsForm
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
            this._numMirrorAngle = new System.Windows.Forms.NumericUpDown();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numMirrorAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._numMirrorAngle);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Size = new System.Drawing.Size(273, 49);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mirror angle";
            // 
            // _numMirrorAngle
            // 
            this._numMirrorAngle.Location = new System.Drawing.Point(79, 18);
            this._numMirrorAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this._numMirrorAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this._numMirrorAngle.Name = "_numMirrorAngle";
            this._numMirrorAngle.Size = new System.Drawing.Size(50, 20);
            this._numMirrorAngle.TabIndex = 1;
            // 
            // LaserBeamMirrorPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 319);
            this.Name = "LaserBeamMirrorPropsForm";
            this.Text = "LaserBeamMirror Properties";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numMirrorAngle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _numMirrorAngle;
    }
}