namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class SpikePropsForm
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
            this._numReleaseDelay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._numActivationTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this._rbOff = new System.Windows.Forms.RadioButton();
            this._rbOn = new System.Windows.Forms.RadioButton();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numReleaseDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numActivationTime)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._rbOff);
            this._gbProps.Controls.Add(this._rbOn);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._numReleaseDelay);
            this._gbProps.Controls.Add(this.label3);
            this._gbProps.Controls.Add(this.label4);
            this._gbProps.Controls.Add(this._numActivationTime);
            this._gbProps.Controls.Add(this.label1);
            this._gbProps.Size = new System.Drawing.Size(246, 98);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "(msecs)";
            // 
            // _numReleaseDelay
            // 
            this._numReleaseDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numReleaseDelay.Location = new System.Drawing.Point(107, 40);
            this._numReleaseDelay.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this._numReleaseDelay.Name = "_numReleaseDelay";
            this._numReleaseDelay.Size = new System.Drawing.Size(66, 20);
            this._numReleaseDelay.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Startup Delay";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(179, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "(msecs)";
            // 
            // _numActivationTime
            // 
            this._numActivationTime.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numActivationTime.Location = new System.Drawing.Point(107, 14);
            this._numActivationTime.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this._numActivationTime.Name = "_numActivationTime";
            this._numActivationTime.Size = new System.Drawing.Size(66, 20);
            this._numActivationTime.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Activation Time";
            // 
            // _rbOff
            // 
            this._rbOff.AutoSize = true;
            this._rbOff.Location = new System.Drawing.Point(79, 66);
            this._rbOff.Name = "_rbOff";
            this._rbOff.Size = new System.Drawing.Size(39, 17);
            this._rbOff.TabIndex = 18;
            this._rbOff.TabStop = true;
            this._rbOff.Text = "Off";
            this._rbOff.UseVisualStyleBackColor = true;
            // 
            // _rbOn
            // 
            this._rbOn.AutoSize = true;
            this._rbOn.Location = new System.Drawing.Point(21, 66);
            this._rbOn.Name = "_rbOn";
            this._rbOn.Size = new System.Drawing.Size(39, 17);
            this._rbOn.TabIndex = 17;
            this._rbOn.TabStop = true;
            this._rbOn.Text = "On";
            this._rbOn.UseVisualStyleBackColor = true;
            // 
            // SpikePropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 368);
            this.Name = "SpikePropsForm";
            this.Text = "Spike Properties";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numReleaseDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numActivationTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown _numActivationTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _numReleaseDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton _rbOff;
        private System.Windows.Forms.RadioButton _rbOn;
    }
}