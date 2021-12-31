namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class ControllableLaserCannonForm
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
            this._numCannonAngle = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this._cmbSwitchOffAction = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._gbProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numCannonAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._cmbSwitchOffAction);
            this._gbProps.Controls.Add(this.label8);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Controls.Add(this._numCannonAngle);
            this._gbProps.Size = new System.Drawing.Size(273, 212);
            this._gbProps.Controls.SetChildIndex(this._numCannonAngle, 0);
            this._gbProps.Controls.SetChildIndex(this.label2, 0);
            this._gbProps.Controls.SetChildIndex(this.label8, 0);
            this._gbProps.Controls.SetChildIndex(this._cmbSwitchOffAction, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Cannon Rotation";
            // 
            // _numCannonAngle
            // 
            this._numCannonAngle.Location = new System.Drawing.Point(117, 160);
            this._numCannonAngle.Name = "_numCannonAngle";
            this._numCannonAngle.Size = new System.Drawing.Size(54, 20);
            this._numCannonAngle.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(24, 189);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Switch Off Action";
            // 
            // _cmbSwitchOffAction
            // 
            this._cmbSwitchOffAction.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.LasrSwitchOffAction;
            this._cmbSwitchOffAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbSwitchOffAction.FormattingEnabled = true;
            this._cmbSwitchOffAction.Location = new System.Drawing.Point(117, 186);
            this._cmbSwitchOffAction.Name = "_cmbSwitchOffAction";
            this._cmbSwitchOffAction.Size = new System.Drawing.Size(121, 21);
            this._cmbSwitchOffAction.TabIndex = 19;
            // 
            // ControllableLaserCannonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 482);
            this.Name = "ControllableLaserCannonForm";
            this.Text = "ControllableLaserCannon";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numCannonAngle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _numCannonAngle;
        private System.Windows.Forms.Label label8;
        private Controls.BaseComboBox _cmbSwitchOffAction;
    }
}