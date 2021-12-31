namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class SimpleSwitchPropsForm
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
            this._rbOn = new System.Windows.Forms.RadioButton();
            this._rbOff = new System.Windows.Forms.RadioButton();
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._rbOff);
            this._gbProps.Controls.Add(this._rbOn);
            this._gbProps.Size = new System.Drawing.Size(273, 47);
            // 
            // _rbOn
            // 
            this._rbOn.AutoSize = true;
            this._rbOn.Location = new System.Drawing.Point(21, 19);
            this._rbOn.Name = "_rbOn";
            this._rbOn.Size = new System.Drawing.Size(39, 17);
            this._rbOn.TabIndex = 0;
            this._rbOn.TabStop = true;
            this._rbOn.Text = "On";
            this._rbOn.UseVisualStyleBackColor = true;
            // 
            // _rbOff
            // 
            this._rbOff.AutoSize = true;
            this._rbOff.Location = new System.Drawing.Point(79, 19);
            this._rbOff.Name = "_rbOff";
            this._rbOff.Size = new System.Drawing.Size(39, 17);
            this._rbOff.TabIndex = 1;
            this._rbOff.TabStop = true;
            this._rbOff.Text = "Off";
            this._rbOff.UseVisualStyleBackColor = true;
            // 
            // SimpleSwitchPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 317);
            this.Name = "SimpleSwitchPropsForm";
            this.Text = "SimpleSwitchPropsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SimpleSwitchPropsForm_FormClosed);
            this.Load += new System.EventHandler(this.SimpleSwitchPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton _rbOff;
        private System.Windows.Forms.RadioButton _rbOn;
    }
}