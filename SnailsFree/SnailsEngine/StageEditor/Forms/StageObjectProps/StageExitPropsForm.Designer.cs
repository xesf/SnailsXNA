namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class StageExitPropsForm
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
            this._cmbSnailCounters = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._cmbSnailCounters);
            this._gbProps.Controls.Add(this.label7);
            this._gbProps.Size = new System.Drawing.Size(251, 50);
            this._gbProps.Text = "StageExit Properties";
            // 
            // _cmbSnailCounters
            // 
            this._cmbSnailCounters.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.StageObjects;
            this._cmbSnailCounters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbSnailCounters.FormattingEnabled = true;
            this._cmbSnailCounters.Location = new System.Drawing.Point(83, 19);
            this._cmbSnailCounters.Name = "_cmbSnailCounters";
            this._cmbSnailCounters.Size = new System.Drawing.Size(121, 21);
            this._cmbSnailCounters.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Snail Counter";
            // 
            // StageExitPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 320);
            this.Name = "StageExitPropsForm";
            this.Text = "StageExit Properties";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StageExitPropsForm_FormClosed);
            this.Load += new System.EventHandler(this.StageExitPropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private Controls.BaseComboBox _cmbSnailCounters;
    }
}