namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class LaserCannonBasePropsForm
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
            this._rbOff = new System.Windows.Forms.RadioButton();
            this._rbOn = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._numBlinkOn = new System.Windows.Forms.NumericUpDown();
            this._chkWithBlink = new System.Windows.Forms.CheckBox();
            this._numBlinkOff = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._gbBlink = new System.Windows.Forms.GroupBox();
            this._cmbColor = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numBlinkOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numBlinkOff)).BeginInit();
            this._gbBlink.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this.label5);
            this._gbProps.Controls.Add(this._cmbColor);
            this._gbProps.Controls.Add(this._gbBlink);
            this._gbProps.Controls.Add(this._rbOff);
            this._gbProps.Controls.Add(this._rbOn);
            this._gbProps.Size = new System.Drawing.Size(273, 162);
            // 
            // _rbOff
            // 
            this._rbOff.AutoSize = true;
            this._rbOff.Location = new System.Drawing.Point(79, 19);
            this._rbOff.Name = "_rbOff";
            this._rbOff.Size = new System.Drawing.Size(39, 17);
            this._rbOff.TabIndex = 5;
            this._rbOff.TabStop = true;
            this._rbOff.Text = "Off";
            this._rbOff.UseVisualStyleBackColor = true;
            // 
            // _rbOn
            // 
            this._rbOn.AutoSize = true;
            this._rbOn.Location = new System.Drawing.Point(21, 19);
            this._rbOn.Name = "_rbOn";
            this._rbOn.Size = new System.Drawing.Size(39, 17);
            this._rbOn.TabIndex = 4;
            this._rbOn.TabStop = true;
            this._rbOn.Text = "On";
            this._rbOn.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Time On";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "(msecs)";
            // 
            // _numBlinkOn
            // 
            this._numBlinkOn.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numBlinkOn.Location = new System.Drawing.Point(86, 34);
            this._numBlinkOn.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this._numBlinkOn.Name = "_numBlinkOn";
            this._numBlinkOn.Size = new System.Drawing.Size(64, 20);
            this._numBlinkOn.TabIndex = 7;
            // 
            // _chkWithBlink
            // 
            this._chkWithBlink.AutoSize = true;
            this._chkWithBlink.Location = new System.Drawing.Point(6, 0);
            this._chkWithBlink.Name = "_chkWithBlink";
            this._chkWithBlink.Size = new System.Drawing.Size(74, 17);
            this._chkWithBlink.TabIndex = 9;
            this._chkWithBlink.Text = "With Blink";
            this._chkWithBlink.UseVisualStyleBackColor = true;
            // 
            // _numBlinkOff
            // 
            this._numBlinkOff.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numBlinkOff.Location = new System.Drawing.Point(86, 58);
            this._numBlinkOff.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this._numBlinkOff.Name = "_numBlinkOff";
            this._numBlinkOff.Size = new System.Drawing.Size(64, 20);
            this._numBlinkOff.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(156, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "(msecs)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Time Off";
            // 
            // _gbBlink
            // 
            this._gbBlink.Controls.Add(this._chkWithBlink);
            this._gbBlink.Controls.Add(this._numBlinkOff);
            this._gbBlink.Controls.Add(this.label3);
            this._gbBlink.Controls.Add(this.label6);
            this._gbBlink.Controls.Add(this.label4);
            this._gbBlink.Controls.Add(this.label7);
            this._gbBlink.Controls.Add(this._numBlinkOn);
            this._gbBlink.Location = new System.Drawing.Point(21, 63);
            this._gbBlink.Name = "_gbBlink";
            this._gbBlink.Size = new System.Drawing.Size(229, 89);
            this._gbBlink.TabIndex = 13;
            this._gbBlink.TabStop = false;
            // 
            // _cmbColor
            // 
            this._cmbColor.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.LaserBeamColors;
            this._cmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbColor.FormattingEnabled = true;
            this._cmbColor.Location = new System.Drawing.Point(98, 36);
            this._cmbColor.Name = "_cmbColor";
            this._cmbColor.Size = new System.Drawing.Size(121, 21);
            this._cmbColor.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Laser Color";
            // 
            // LaserCannonBasePropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 432);
            this.Name = "LaserCannonBasePropsForm";
            this.Text = "LaserCannonBase Properties";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numBlinkOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numBlinkOff)).EndInit();
            this._gbBlink.ResumeLayout(false);
            this._gbBlink.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton _rbOff;
        private System.Windows.Forms.RadioButton _rbOn;
        private System.Windows.Forms.NumericUpDown _numBlinkOn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox _gbBlink;
        private System.Windows.Forms.CheckBox _chkWithBlink;
        private System.Windows.Forms.NumericUpDown _numBlinkOff;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private Controls.BaseComboBox _cmbColor;

    }
}