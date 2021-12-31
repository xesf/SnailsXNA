namespace SpriteAnimationEditor.Forms
{
  partial class ExportInGameFilesForm
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this._PngAnimation = new System.Windows.Forms.Button();
      this._BtnPng = new System.Windows.Forms.Button();
      this._AnimationDataFile = new System.Windows.Forms.TextBox();
      this._PngFile = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this._BtnOk = new System.Windows.Forms.Button();
      this._BtnCancel = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this._PngAnimation);
      this.groupBox1.Controls.Add(this._BtnPng);
      this.groupBox1.Controls.Add(this._AnimationDataFile);
      this.groupBox1.Controls.Add(this._PngFile);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(575, 150);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(23, 114);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(334, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "-Changes in this settings will not be reflected on the Project Properties";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(23, 101);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(227, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "-Relative paths are relative to the project folder";
      // 
      // _PngAnimation
      // 
      this._PngAnimation.Location = new System.Drawing.Point(529, 54);
      this._PngAnimation.Name = "_PngAnimation";
      this._PngAnimation.Size = new System.Drawing.Size(33, 23);
      this._PngAnimation.TabIndex = 5;
      this._PngAnimation.Text = "...";
      this._PngAnimation.UseVisualStyleBackColor = true;
      // 
      // _BtnPng
      // 
      this._BtnPng.Location = new System.Drawing.Point(529, 26);
      this._BtnPng.Name = "_BtnPng";
      this._BtnPng.Size = new System.Drawing.Size(33, 23);
      this._BtnPng.TabIndex = 4;
      this._BtnPng.Text = "...";
      this._BtnPng.UseVisualStyleBackColor = true;
      // 
      // _AnimationDataFile
      // 
      this._AnimationDataFile.Location = new System.Drawing.Point(118, 55);
      this._AnimationDataFile.Name = "_AnimationDataFile";
      this._AnimationDataFile.Size = new System.Drawing.Size(405, 20);
      this._AnimationDataFile.TabIndex = 3;
      // 
      // _PngFile
      // 
      this._PngFile.Location = new System.Drawing.Point(118, 28);
      this._PngFile.Name = "_PngFile";
      this._PngFile.Size = new System.Drawing.Size(405, 20);
      this._PngFile.TabIndex = 2;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 59);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(101, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Animation Data File:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(60, 31);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "PNG File:";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this._BtnOk);
      this.groupBox2.Controls.Add(this._BtnCancel);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox2.Location = new System.Drawing.Point(0, 150);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(575, 55);
      this.groupBox2.TabIndex = 6;
      this.groupBox2.TabStop = false;
      // 
      // _BtnOk
      // 
      this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._BtnOk.Location = new System.Drawing.Point(209, 20);
      this._BtnOk.Name = "_BtnOk";
      this._BtnOk.Size = new System.Drawing.Size(75, 23);
      this._BtnOk.TabIndex = 3;
      this._BtnOk.Text = "&Ok";
      this._BtnOk.UseVisualStyleBackColor = true;
      // 
      // _BtnCancel
      // 
      this._BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._BtnCancel.Location = new System.Drawing.Point(290, 20);
      this._BtnCancel.Name = "_BtnCancel";
      this._BtnCancel.Size = new System.Drawing.Size(75, 23);
      this._BtnCancel.TabIndex = 4;
      this._BtnCancel.Text = "&Cancel";
      this._BtnCancel.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(11, 88);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(38, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Notes:";
      // 
      // ExportInGameFilesForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(575, 205);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.groupBox2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ExportInGameFilesForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Export In Game Files";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button _BtnPng;
    private System.Windows.Forms.TextBox _AnimationDataFile;
    private System.Windows.Forms.TextBox _PngFile;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button _BtnOk;
    private System.Windows.Forms.Button _BtnCancel;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button _PngAnimation;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
  }
}