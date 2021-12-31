namespace SpriteAnimationEditor.Forms
{
  partial class ExportForm
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
      this._Cols = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this._Filename = new System.Windows.Forms.TextBox();
      this._Ok = new System.Windows.Forms.Button();
      this._Cancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _Cols
      // 
      this._Cols.Location = new System.Drawing.Point(72, 26);
      this._Cols.Name = "_Cols";
      this._Cols.Size = new System.Drawing.Size(52, 20);
      this._Cols.TabIndex = 0;
      this._Cols.Text = "4";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(31, 29);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(27, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Cols";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(31, 62);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(23, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "File";
      // 
      // _Filename
      // 
      this._Filename.Location = new System.Drawing.Point(72, 59);
      this._Filename.Name = "_Filename";
      this._Filename.Size = new System.Drawing.Size(217, 20);
      this._Filename.TabIndex = 5;
      this._Filename.Text = "c:\\temp\\a\\anim.png";
      // 
      // _Ok
      // 
      this._Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._Ok.Location = new System.Drawing.Point(102, 102);
      this._Ok.Name = "_Ok";
      this._Ok.Size = new System.Drawing.Size(75, 23);
      this._Ok.TabIndex = 6;
      this._Ok.Text = "&Ok";
      this._Ok.UseVisualStyleBackColor = true;
      // 
      // _Cancel
      // 
      this._Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._Cancel.Location = new System.Drawing.Point(183, 102);
      this._Cancel.Name = "_Cancel";
      this._Cancel.Size = new System.Drawing.Size(75, 23);
      this._Cancel.TabIndex = 7;
      this._Cancel.Text = "&Cancel";
      this._Cancel.UseVisualStyleBackColor = true;
      // 
      // ExportForm
      // 
      this.AcceptButton = this._Ok;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._Cancel;
      this.ClientSize = new System.Drawing.Size(361, 139);
      this.Controls.Add(this._Cancel);
      this.Controls.Add(this._Ok);
      this.Controls.Add(this._Filename);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this._Cols);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ExportForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Export";
      this.Load += new System.EventHandler(this._ExportForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox _Cols;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox _Filename;
    private System.Windows.Forms.Button _Ok;
    private System.Windows.Forms.Button _Cancel;
  }
}