namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class LiquidPipePropsForm
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
            this._pipeString = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._cmbPumpAttachment = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._cmbTerminator = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this._cmbTerminator);
            this._gbProps.Controls.Add(this._cmbPumpAttachment);
            this._gbProps.Controls.Add(this.label5);
            this._gbProps.Controls.Add(this.label4);
            this._gbProps.Controls.Add(this.label3);
            this._gbProps.Controls.Add(this._pipeString);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Size = new System.Drawing.Size(273, 118);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Pipe string:";
            // 
            // _pipeString
            // 
            this._pipeString.Location = new System.Drawing.Point(79, 16);
            this._pipeString.Name = "_pipeString";
            this._pipeString.Size = new System.Drawing.Size(178, 20);
            this._pipeString.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(76, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "String format: L3;R4;U4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Pump attachment is at the:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Terminator points to the:";
            // 
            // _cmbPumpAttachment
            // 
            this._cmbPumpAttachment.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.PumpAttachment;
            this._cmbPumpAttachment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbPumpAttachment.FormattingEnabled = true;
            this._cmbPumpAttachment.Location = new System.Drawing.Point(146, 62);
            this._cmbPumpAttachment.Name = "_cmbPumpAttachment";
            this._cmbPumpAttachment.Size = new System.Drawing.Size(90, 21);
            this._cmbPumpAttachment.TabIndex = 5;
            // 
            // _cmbTerminator
            // 
            this._cmbTerminator.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.PipeTerminatorOrientation;
            this._cmbTerminator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTerminator.FormattingEnabled = true;
            this._cmbTerminator.Location = new System.Drawing.Point(146, 89);
            this._cmbTerminator.Name = "_cmbTerminator";
            this._cmbTerminator.Size = new System.Drawing.Size(90, 21);
            this._cmbTerminator.TabIndex = 6;
            // 
            // LiquidPipePropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 388);
            this.Name = "LiquidPipePropsForm";
            this.Text = "LiquidPipePropsForm";
            this.Load += new System.EventHandler(this.LiquidPipePropsForm_Load);
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _pipeString;
        private System.Windows.Forms.Label label3;
        private Controls.BaseComboBox _cmbTerminator;
        private Controls.BaseComboBox _cmbPumpAttachment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}