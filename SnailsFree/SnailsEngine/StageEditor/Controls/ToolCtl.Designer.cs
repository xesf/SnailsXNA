namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    partial class ToolCtl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._pnlTool = new System.Windows.Forms.Panel();
            this._udQuantity = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this._udQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // _pnlTool
            // 
            this._pnlTool.Location = new System.Drawing.Point(3, 3);
            this._pnlTool.Name = "_pnlTool";
            this._pnlTool.Size = new System.Drawing.Size(113, 100);
            this._pnlTool.TabIndex = 0;
            this._pnlTool.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlTool_Paint);
            // 
            // _udQuantity
            // 
            this._udQuantity.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._udQuantity.Location = new System.Drawing.Point(0, 113);
            this._udQuantity.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this._udQuantity.Name = "_udQuantity";
            this._udQuantity.Size = new System.Drawing.Size(120, 20);
            this._udQuantity.TabIndex = 1;
            this._udQuantity.ValueChanged += new System.EventHandler(this._udQuantity_ValueChanged);
            // 
            // ToolCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this._udQuantity);
            this.Controls.Add(this._pnlTool);
            this.Name = "ToolCtl";
            this.Size = new System.Drawing.Size(120, 133);
            ((System.ComponentModel.ISupportInitialize)(this._udQuantity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _pnlTool;
        private System.Windows.Forms.NumericUpDown _udQuantity;
    }
}
