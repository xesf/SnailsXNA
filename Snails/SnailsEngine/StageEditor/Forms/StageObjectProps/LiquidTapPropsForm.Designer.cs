namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class LiquidTapPropsForm
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
            this._numPumpSpeed = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this._cmbOpendDirection = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this._cmbSignToShow = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numPumpSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this.label4);
            this._gbProps.Controls.Add(this._cmbSignToShow);
            this._gbProps.Controls.Add(this.label3);
            this._gbProps.Controls.Add(this._numPumpSpeed);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._cmbOpendDirection);
            this._gbProps.Size = new System.Drawing.Size(273, 105);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pump speed:";
            // 
            // _numPumpSpeed
            // 
            this._numPumpSpeed.DecimalPlaces = 2;
            this._numPumpSpeed.Location = new System.Drawing.Point(126, 23);
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
            this._numPumpSpeed.Size = new System.Drawing.Size(64, 20);
            this._numPumpSpeed.TabIndex = 5;
            this._numPumpSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Opening direction:";
            // 
            // _cmbOpendDirection
            // 
            this._cmbOpendDirection.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.TapOpeningDirection;
            this._cmbOpendDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbOpendDirection.FormattingEnabled = true;
            this._cmbOpendDirection.Location = new System.Drawing.Point(126, 49);
            this._cmbOpendDirection.Name = "_cmbOpendDirection";
            this._cmbOpendDirection.Size = new System.Drawing.Size(121, 21);
            this._cmbOpendDirection.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Sign to Show:";
            // 
            // _cmbSignToShow
            // 
            this._cmbSignToShow.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.TabSignToShow;
            this._cmbSignToShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbSignToShow.FormattingEnabled = true;
            this._cmbSignToShow.Location = new System.Drawing.Point(126, 76);
            this._cmbSignToShow.Name = "_cmbSignToShow";
            this._cmbSignToShow.Size = new System.Drawing.Size(121, 21);
            this._cmbSignToShow.TabIndex = 7;
            // 
            // LiquidTapPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 375);
            this.Name = "LiquidTapPropsForm";
            this.Text = "LiquidTapPropsForm";
            this.Load += new System.EventHandler(this.LiquidTapPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numPumpSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _numPumpSpeed;
        private System.Windows.Forms.Label label3;
        private Controls.BaseComboBox _cmbOpendDirection;
        private System.Windows.Forms.Label label4;
        private Controls.BaseComboBox _cmbSignToShow;

    }
}