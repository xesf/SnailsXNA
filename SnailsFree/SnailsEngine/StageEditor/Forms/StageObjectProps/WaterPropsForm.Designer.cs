namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class WaterPropsForm
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
            this.lbSize = new System.Windows.Forms.Label();
            this._txtSizeX = new System.Windows.Forms.NumericUpDown();
            this.lbX = new System.Windows.Forms.Label();
            this.lbY = new System.Windows.Forms.Label();
            this._txtSizeY = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this._numLevel = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtSizeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSizeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this.label3);
            this._gbProps.Controls.Add(this._numLevel);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._txtSizeY);
            this._gbProps.Controls.Add(this.lbY);
            this._gbProps.Controls.Add(this.lbX);
            this._gbProps.Controls.Add(this._txtSizeX);
            this._gbProps.Controls.Add(this.lbSize);
            this._gbProps.Size = new System.Drawing.Size(273, 92);
            // 
            // lbSize
            // 
            this.lbSize.AutoSize = true;
            this.lbSize.Location = new System.Drawing.Point(12, 21);
            this.lbSize.Name = "lbSize";
            this.lbSize.Size = new System.Drawing.Size(27, 13);
            this.lbSize.TabIndex = 0;
            this.lbSize.Text = "Size";
            // 
            // _txtSizeX
            // 
            this._txtSizeX.Location = new System.Drawing.Point(69, 19);
            this._txtSizeX.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this._txtSizeX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._txtSizeX.Name = "_txtSizeX";
            this._txtSizeX.Size = new System.Drawing.Size(43, 20);
            this._txtSizeX.TabIndex = 1;
            this._txtSizeX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbX
            // 
            this.lbX.AutoSize = true;
            this.lbX.Location = new System.Drawing.Point(49, 21);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(14, 13);
            this.lbX.TabIndex = 2;
            this.lbX.Text = "X";
            // 
            // lbY
            // 
            this.lbY.AutoSize = true;
            this.lbY.Location = new System.Drawing.Point(127, 21);
            this.lbY.Name = "lbY";
            this.lbY.Size = new System.Drawing.Size(14, 13);
            this.lbY.TabIndex = 3;
            this.lbY.Text = "Y";
            // 
            // _txtSizeY
            // 
            this._txtSizeY.Location = new System.Drawing.Point(147, 19);
            this._txtSizeY.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this._txtSizeY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._txtSizeY.Name = "_txtSizeY";
            this._txtSizeY.Size = new System.Drawing.Size(43, 20);
            this._txtSizeY.TabIndex = 4;
            this._txtSizeY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Liquid level";
            // 
            // _numLevel
            // 
            this._numLevel.DecimalPlaces = 1;
            this._numLevel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this._numLevel.Location = new System.Drawing.Point(80, 58);
            this._numLevel.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numLevel.Name = "_numLevel";
            this._numLevel.Size = new System.Drawing.Size(61, 20);
            this._numLevel.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(147, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "(0=empty, 1=full)";
            // 
            // WaterPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 362);
            this.Name = "WaterPropsForm";
            this.Text = "WaterPropsForm";
            this.Load += new System.EventHandler(this.WaterPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtSizeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSizeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numLevel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbSize;
        private System.Windows.Forms.NumericUpDown _txtSizeY;
        private System.Windows.Forms.Label lbY;
        private System.Windows.Forms.Label lbX;
        private System.Windows.Forms.NumericUpDown _txtSizeX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown _numLevel;
        private System.Windows.Forms.Label label2;
    }
}