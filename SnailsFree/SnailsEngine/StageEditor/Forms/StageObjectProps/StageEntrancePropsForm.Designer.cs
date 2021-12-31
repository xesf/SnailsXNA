namespace TwoBrainsGames.Snails.StageEditor.Forms
{
  partial class StageEntrancePropsForm
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
            this.label7 = new System.Windows.Forms.Label();
            this._numInitialDelay = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this._chkReleasesKing = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._numSnailsToRelease = new System.Windows.Forms.NumericUpDown();
            this._numReleaseInterval = new System.Windows.Forms.NumericUpDown();
            this._cmbDirection = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._cmbSnailCounters = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this._cmbSnailsType = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._numBeforeKing = new System.Windows.Forms.NumericUpDown();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numInitialDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numSnailsToRelease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numReleaseInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numBeforeKing)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this.label10);
            this._gbProps.Controls.Add(this._numBeforeKing);
            this._gbProps.Controls.Add(this._cmbSnailsType);
            this._gbProps.Controls.Add(this.label6);
            this._gbProps.Controls.Add(this._cmbSnailCounters);
            this._gbProps.Controls.Add(this.label5);
            this._gbProps.Controls.Add(this.label7);
            this._gbProps.Controls.Add(this._numInitialDelay);
            this._gbProps.Controls.Add(this.label8);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._chkReleasesKing);
            this._gbProps.Controls.Add(this.label1);
            this._gbProps.Controls.Add(this.label9);
            this._gbProps.Controls.Add(this.label3);
            this._gbProps.Controls.Add(this._cmbDirection);
            this._gbProps.Controls.Add(this._numReleaseInterval);
            this._gbProps.Controls.Add(this._numSnailsToRelease);
            this._gbProps.Controls.Add(this.label4);
            this._gbProps.Size = new System.Drawing.Size(249, 228);
            this._gbProps.Text = "StageEntrance Properties";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(182, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "(msecs)";
            // 
            // _numInitialDelay
            // 
            this._numInitialDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numInitialDelay.Location = new System.Drawing.Point(111, 104);
            this._numInitialDelay.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this._numInitialDelay.Name = "_numInitialDelay";
            this._numInitialDelay.Size = new System.Drawing.Size(66, 20);
            this._numInitialDelay.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(42, 106);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Initial Delay";
            // 
            // _chkReleasesKing
            // 
            this._chkReleasesKing.AutoSize = true;
            this._chkReleasesKing.Location = new System.Drawing.Point(111, 156);
            this._chkReleasesKing.Name = "_chkReleasesKing";
            this._chkReleasesKing.Size = new System.Drawing.Size(15, 14);
            this._chkReleasesKing.TabIndex = 15;
            this._chkReleasesKing.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Releases the King";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(182, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "(msecs)";
            // 
            // _numSnailsToRelease
            // 
            this._numSnailsToRelease.Location = new System.Drawing.Point(111, 25);
            this._numSnailsToRelease.Name = "_numSnailsToRelease";
            this._numSnailsToRelease.Size = new System.Drawing.Size(66, 20);
            this._numSnailsToRelease.TabIndex = 3;
            // 
            // _numReleaseInterval
            // 
            this._numReleaseInterval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numReleaseInterval.Location = new System.Drawing.Point(110, 78);
            this._numReleaseInterval.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this._numReleaseInterval.Name = "_numReleaseInterval";
            this._numReleaseInterval.Size = new System.Drawing.Size(66, 20);
            this._numReleaseInterval.TabIndex = 4;
            // 
            // _cmbDirection
            // 
            this._cmbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbDirection.FormattingEnabled = true;
            this._cmbDirection.Location = new System.Drawing.Point(110, 130);
            this._cmbDirection.Name = "_cmbDirection";
            this._cmbDirection.Size = new System.Drawing.Size(121, 21);
            this._cmbDirection.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Direction";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Release Interval";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Snails To Release";
            // 
            // _cmbSnailCounters
            // 
            this._cmbSnailCounters.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.StageObjects;
            this._cmbSnailCounters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbSnailCounters.FormattingEnabled = true;
            this._cmbSnailCounters.Location = new System.Drawing.Point(110, 201);
            this._cmbSnailCounters.Name = "_cmbSnailCounters";
            this._cmbSnailCounters.Size = new System.Drawing.Size(121, 21);
            this._cmbSnailCounters.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Snail Counter";
            // 
            // _cmbSnailsType
            // 
            this._cmbSnailsType.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.SnailTypes;
            this._cmbSnailsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbSnailsType.FormattingEnabled = true;
            this._cmbSnailsType.Location = new System.Drawing.Point(110, 51);
            this._cmbSnailsType.Name = "_cmbSnailsType";
            this._cmbSnailsType.Size = new System.Drawing.Size(121, 21);
            this._cmbSnailsType.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Snails Type";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 177);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(169, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Snails To Release Before the King";
            // 
            // _numBeforeKing
            // 
            this._numBeforeKing.Location = new System.Drawing.Point(185, 175);
            this._numBeforeKing.Name = "_numBeforeKing";
            this._numBeforeKing.Size = new System.Drawing.Size(46, 20);
            this._numBeforeKing.TabIndex = 24;
            // 
            // StageEntrancePropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 498);
            this.Name = "StageEntrancePropsForm";
            this.Text = "StageEntrance Properties";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numInitialDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numSnailsToRelease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numReleaseInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numBeforeKing)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown _numSnailsToRelease;
        private System.Windows.Forms.NumericUpDown _numReleaseInterval;
        private System.Windows.Forms.ComboBox _cmbDirection;
        private System.Windows.Forms.CheckBox _chkReleasesKing;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown _numInitialDelay;
        private System.Windows.Forms.Label label8;
        private Controls.BaseComboBox _cmbSnailCounters;
        private System.Windows.Forms.Label label5;
        private Controls.BaseComboBox _cmbSnailsType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown _numBeforeKing;
    }
}