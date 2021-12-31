namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class ControllableLaserCannonForm
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
            this._numCannonAngle = new System.Windows.Forms.NumericUpDown();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numCannonAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._numCannonAngle);
            this._gbProps.Size = new System.Drawing.Size(273, 186);
            this._gbProps.Controls.SetChildIndex(this._numCannonAngle, 0);
            this._gbProps.Controls.SetChildIndex(this.label2, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Cannon Rotation";
            // 
            // _numCannonAngle
            // 
            this._numCannonAngle.Location = new System.Drawing.Point(117, 160);
            this._numCannonAngle.Name = "_numCannonAngle";
            this._numCannonAngle.Size = new System.Drawing.Size(54, 20);
            this._numCannonAngle.TabIndex = 17;
            // 
            // ControllableLaserCannonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 456);
            this.Name = "ControllableLaserCannonForm";
            this.Text = "ControllableLaserCannon";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numCannonAngle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _numCannonAngle;
    }
}