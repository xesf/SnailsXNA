namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class LiquidPumpPropsForm
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
            this._rbPumpingIn = new System.Windows.Forms.RadioButton();
            this._rbPumpingOut = new System.Windows.Forms.RadioButton();
            this._numPumpSpeed = new System.Windows.Forms.NumericUpDown();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numPumpSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._numPumpSpeed);
            this._gbProps.Controls.Add(this._rbPumpingOut);
            this._gbProps.Controls.Add(this._rbPumpingIn);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Size = new System.Drawing.Size(273, 78);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Pump speed:";
            // 
            // _rbPumpingIn
            // 
            this._rbPumpingIn.AutoSize = true;
            this._rbPumpingIn.Location = new System.Drawing.Point(27, 19);
            this._rbPumpingIn.Name = "_rbPumpingIn";
            this._rbPumpingIn.Size = new System.Drawing.Size(78, 17);
            this._rbPumpingIn.TabIndex = 2;
            this._rbPumpingIn.TabStop = true;
            this._rbPumpingIn.Text = "Pumping In";
            this._rbPumpingIn.UseVisualStyleBackColor = true;
            // 
            // _rbPumpingOut
            // 
            this._rbPumpingOut.AutoSize = true;
            this._rbPumpingOut.Location = new System.Drawing.Point(115, 19);
            this._rbPumpingOut.Name = "_rbPumpingOut";
            this._rbPumpingOut.Size = new System.Drawing.Size(86, 17);
            this._rbPumpingOut.TabIndex = 3;
            this._rbPumpingOut.TabStop = true;
            this._rbPumpingOut.Text = "Pumping Out";
            this._rbPumpingOut.UseVisualStyleBackColor = true;
            // 
            // _numPumpSpeed
            // 
            this._numPumpSpeed.DecimalPlaces = 2;
            this._numPumpSpeed.Location = new System.Drawing.Point(99, 48);
            this._numPumpSpeed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numPumpSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numPumpSpeed.Name = "_numPumpSpeed";
            this._numPumpSpeed.Size = new System.Drawing.Size(54, 20);
            this._numPumpSpeed.TabIndex = 4;
            this._numPumpSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LiquidPumpPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 348);
            this.Name = "LiquidPumpPropsForm";
            this.Text = "LiquidPumpPropsForm";
            this.Load += new System.EventHandler(this.LiquidPumpPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numPumpSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton _rbPumpingOut;
        private System.Windows.Forms.RadioButton _rbPumpingIn;
        private System.Windows.Forms.NumericUpDown _numPumpSpeed;
    }
}