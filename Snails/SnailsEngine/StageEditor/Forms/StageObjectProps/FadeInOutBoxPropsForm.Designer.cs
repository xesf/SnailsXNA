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
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._rbFadedOut);
            this._gbProps.Controls.Add(this._rbFadedIn);
            this._gbProps.Size = new System.Drawing.Size(273, 45);
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
            // FadeInOutBoxPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 315);
            this.Name = "FadeInOutBoxPropsForm";
            this.Text = "FadeInOutPropsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FadeInOutBoxPropsForm_FormClosed);
            this.Load += new System.EventHandler(this.FadeInOutBoxPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton _rbFadedOut;
        private System.Windows.Forms.RadioButton _rbFadedIn;

    }
}