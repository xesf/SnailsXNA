namespace SpriteAnimationEditor
{
  partial class ImportFramesForm
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
      this._ImageList = new System.Windows.Forms.ListBox();
      this._CurFrame = new System.Windows.Forms.PictureBox();
      this._SelFrames = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this._OpenFilesDlg = new System.Windows.Forms.OpenFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this._CurFrame)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // _ImageList
      // 
      this._ImageList.FormattingEnabled = true;
      this._ImageList.Location = new System.Drawing.Point(12, 12);
      this._ImageList.Name = "_ImageList";
      this._ImageList.Size = new System.Drawing.Size(212, 407);
      this._ImageList.TabIndex = 0;
      this._ImageList.SelectedIndexChanged += new System.EventHandler(this._ImageList_SelectedIndexChanged);
      // 
      // _CurFrame
      // 
      this._CurFrame.BackColor = System.Drawing.Color.Gray;
      this._CurFrame.Location = new System.Drawing.Point(230, 12);
      this._CurFrame.Name = "_CurFrame";
      this._CurFrame.Size = new System.Drawing.Size(273, 407);
      this._CurFrame.TabIndex = 1;
      this._CurFrame.TabStop = false;
      // 
      // _SelFrames
      // 
      this._SelFrames.Location = new System.Drawing.Point(12, 425);
      this._SelFrames.Name = "_SelFrames";
      this._SelFrames.Size = new System.Drawing.Size(212, 23);
      this._SelFrames.TabIndex = 2;
      this._SelFrames.Text = "Select Frames...";
      this._SelFrames.UseVisualStyleBackColor = true;
      this._SelFrames.Click += new System.EventHandler(this._SelFrames_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(158, 20);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "&Ok";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(239, 20);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(75, 23);
      this.button3.TabIndex = 4;
      this.button3.Text = "&Cancel";
      this.button3.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.button2);
      this.groupBox1.Controls.Add(this.button3);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox1.Location = new System.Drawing.Point(0, 454);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(508, 55);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      // 
      // _OpenFilesDlg
      // 
      this._OpenFilesDlg.DefaultExt = "png";
      this._OpenFilesDlg.Filter = "Png Files|*.png|Bitmap|*.bmp";
      this._OpenFilesDlg.Multiselect = true;
      // 
      // ImportFramesForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(508, 509);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this._SelFrames);
      this.Controls.Add(this._CurFrame);
      this.Controls.Add(this._ImageList);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ImportFramesForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Frames Import";
      this.Load += new System.EventHandler(this.ImportFramesForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this._CurFrame)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox _ImageList;
    private System.Windows.Forms.PictureBox _CurFrame;
    private System.Windows.Forms.Button _SelFrames;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.OpenFileDialog _OpenFilesDlg;
  }
}