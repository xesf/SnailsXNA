namespace TwoBrainsGames.Snails.Debuging
{
  partial class TileObjectInfo
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this._txtLeft = new System.Windows.Forms.TextBox();
      this._TxtTop = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(4, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(25, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Left";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(4, 30);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(26, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Top";
      // 
      // _txtLeft
      // 
      this._txtLeft.Location = new System.Drawing.Point(46, 4);
      this._txtLeft.Name = "_txtLeft";
      this._txtLeft.Size = new System.Drawing.Size(51, 20);
      this._txtLeft.TabIndex = 2;
      // 
      // _TxtTop
      // 
      this._TxtTop.Location = new System.Drawing.Point(46, 27);
      this._TxtTop.Name = "_TxtTop";
      this._TxtTop.Size = new System.Drawing.Size(51, 20);
      this._TxtTop.TabIndex = 3;
      // 
      // TileObjectInfo
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._TxtTop);
      this.Controls.Add(this._txtLeft);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "TileObjectInfo";
      this.Size = new System.Drawing.Size(104, 53);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox _txtLeft;
    private System.Windows.Forms.TextBox _TxtTop;
  }
}
