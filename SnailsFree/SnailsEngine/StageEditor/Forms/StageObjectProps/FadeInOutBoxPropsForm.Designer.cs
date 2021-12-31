namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class FadeInOutBoxPropsForm
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
            this._rbFadedOut = new System.Windows.Forms.RadioButton();
            this._rbFadedIn = new System.Windows.Forms.RadioButton();
            this._gbAutofade = new System.Windows.Forms.GroupBox();
            this._numFadeOutTime = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this._numFadeInTime = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this._numInitialDelay = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._chkAutofade = new System.Windows.Forms.CheckBox();
            this._gbProps.SuspendLayout();
            this._gbAutofade.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numFadeOutTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numFadeInTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numInitialDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._gbAutofade);
            this._gbProps.Controls.Add(this._rbFadedOut);
            this._gbProps.Controls.Add(this._rbFadedIn);
            this._gbProps.Size = new System.Drawing.Size(273, 156);
            // 
            // _rbFadedOut
            // 
            this._rbFadedOut.AutoSize = true;
            this._rbFadedOut.Location = new System.Drawing.Point(94, 19);
            this._rbFadedOut.Name = "_rbFadedOut";
            this._rbFadedOut.Size = new System.Drawing.Size(75, 17);
            this._rbFadedOut.TabIndex = 5;
            this._rbFadedOut.TabStop = true;
            this._rbFadedOut.Text = "Faded Out";
            this._rbFadedOut.UseVisualStyleBackColor = true;
            // 
            // _rbFadedIn
            // 
            this._rbFadedIn.AutoSize = true;
            this._rbFadedIn.Location = new System.Drawing.Point(21, 19);
            this._rbFadedIn.Name = "_rbFadedIn";
            this._rbFadedIn.Size = new System.Drawing.Size(67, 17);
            this._rbFadedIn.TabIndex = 4;
            this._rbFadedIn.TabStop = true;
            this._rbFadedIn.Text = "Faded In";
            this._rbFadedIn.UseVisualStyleBackColor = true;
            // 
            // _gbAutofade
            // 
            this._gbAutofade.Controls.Add(this._numFadeOutTime);
            this._gbAutofade.Controls.Add(this.label7);
            this._gbAutofade.Controls.Add(this._numFadeInTime);
            this._gbAutofade.Controls.Add(this.label6);
            this._gbAutofade.Controls.Add(this._numInitialDelay);
            this._gbAutofade.Controls.Add(this.label5);
            this._gbAutofade.Controls.Add(this.label4);
            this._gbAutofade.Controls.Add(this.label3);
            this._gbAutofade.Controls.Add(this.label2);
            this._gbAutofade.Controls.Add(this._chkAutofade);
            this._gbAutofade.Location = new System.Drawing.Point(21, 42);
            this._gbAutofade.Name = "_gbAutofade";
            this._gbAutofade.Size = new System.Drawing.Size(233, 108);
            this._gbAutofade.TabIndex = 6;
            this._gbAutofade.TabStop = false;
            // 
            // _numFadeOutTime
            // 
            this._numFadeOutTime.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numFadeOutTime.Location = new System.Drawing.Point(105, 79);
            this._numFadeOutTime.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this._numFadeOutTime.Name = "_numFadeOutTime";
            this._numFadeOutTime.Size = new System.Drawing.Size(59, 20);
            this._numFadeOutTime.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(170, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "(msecs)";
            // 
            // _numFadeInTime
            // 
            this._numFadeInTime.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numFadeInTime.Location = new System.Drawing.Point(105, 53);
            this._numFadeInTime.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this._numFadeInTime.Name = "_numFadeInTime";
            this._numFadeInTime.Size = new System.Drawing.Size(59, 20);
            this._numFadeInTime.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(170, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "(msecs)";
            // 
            // _numInitialDelay
            // 
            this._numInitialDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._numInitialDelay.Location = new System.Drawing.Point(105, 26);
            this._numInitialDelay.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this._numInitialDelay.Name = "_numInitialDelay";
            this._numInitialDelay.Size = new System.Drawing.Size(59, 20);
            this._numInitialDelay.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(170, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "(msecs)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fade Out Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fade In Time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Initial Delay";
            // 
            // _chkAutofade
            // 
            this._chkAutofade.AutoSize = true;
            this._chkAutofade.Location = new System.Drawing.Point(6, 0);
            this._chkAutofade.Name = "_chkAutofade";
            this._chkAutofade.Size = new System.Drawing.Size(69, 17);
            this._chkAutofade.TabIndex = 0;
            this._chkAutofade.Text = "Autofade";
            this._chkAutofade.UseVisualStyleBackColor = true;
            this._chkAutofade.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // FadeInOutBoxPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 426);
            this.Name = "FadeInOutBoxPropsForm";
            this.Text = "FadeInOutPropsForm";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this._gbAutofade.ResumeLayout(false);
            this._gbAutofade.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numFadeOutTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numFadeInTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numInitialDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton _rbFadedOut;
        private System.Windows.Forms.RadioButton _rbFadedIn;
        private System.Windows.Forms.GroupBox _gbAutofade;
        private System.Windows.Forms.CheckBox _chkAutofade;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _numInitialDelay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown _numFadeOutTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown _numFadeInTime;
        private System.Windows.Forms.Label label6;

    }
}