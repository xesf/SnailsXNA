namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class LaserSwitchPropsForm
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
            this._cmbColor = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._cmbAction = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._cmbAction);
            this._gbProps.Controls.Add(this.label3);
            this._gbProps.Controls.Add(this._cmbColor);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Size = new System.Drawing.Size(273, 82);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Laser Color";
            // 
            // _cmbColor
            // 
            this._cmbColor.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.LaserBeamColors;
            this._cmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbColor.FormattingEnabled = true;
            this._cmbColor.Location = new System.Drawing.Point(86, 19);
            this._cmbColor.Name = "_cmbColor";
            this._cmbColor.Size = new System.Drawing.Size(121, 21);
            this._cmbColor.TabIndex = 1;
            // 
            // _cmbAction
            // 
            this._cmbAction.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.SwithOnAction;
            this._cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbAction.FormattingEnabled = true;
            this._cmbAction.Location = new System.Drawing.Point(153, 46);
            this._cmbAction.Name = "_cmbAction";
            this._cmbAction.Size = new System.Drawing.Size(83, 21);
            this._cmbAction.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Connected Objects Action";
            // 
            // LaserSwitchPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 352);
            this.Name = "LaserSwitchPropsForm";
            this.Text = "LaserSwitchPropsForm";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private Controls.BaseComboBox _cmbColor;
        private Controls.BaseComboBox _cmbAction;
        private System.Windows.Forms.Label label3;
    }
}