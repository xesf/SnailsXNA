namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class SnailSacrificeSwitchPropsForm
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
            this._numSnailsNeeded = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._cmbAction = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numSnailsNeeded)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._cmbAction);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._numSnailsNeeded);
            this._gbProps.Controls.Add(this.label10);
            this._gbProps.Size = new System.Drawing.Size(247, 79);
            this._gbProps.Text = "Switch Properties";
            // 
            // _numSnailsNeeded
            // 
            this._numSnailsNeeded.Location = new System.Drawing.Point(153, 19);
            this._numSnailsNeeded.Name = "_numSnailsNeeded";
            this._numSnailsNeeded.Size = new System.Drawing.Size(66, 20);
            this._numSnailsNeeded.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Snails needed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Connected Objects Action";
            // 
            // _cmbAction
            // 
            this._cmbAction.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.SwithOnAction;
            this._cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbAction.FormattingEnabled = true;
            this._cmbAction.Location = new System.Drawing.Point(153, 44);
            this._cmbAction.Name = "_cmbAction";
            this._cmbAction.Size = new System.Drawing.Size(83, 21);
            this._cmbAction.TabIndex = 5;
            // 
            // SnailSacrificeSwitchPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 349);
            this.Name = "SnailSacrificeSwitchPropsForm";
            this.Text = "Snails Switch Properties";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numSnailsNeeded)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown _numSnailsNeeded;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private Controls.BaseComboBox _cmbAction;

    }
}