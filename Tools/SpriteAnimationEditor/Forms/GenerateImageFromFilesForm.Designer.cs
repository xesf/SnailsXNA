namespace SpriteAnimationEditor.Forms
{
  partial class GenerateImageFromFilesForm
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
      this._btnOk = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this._OpenFilesDlg = new System.Windows.Forms.OpenFileDialog();
      this.label1 = new System.Windows.Forms.Label();
      this._TxtFramesPerColumn = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this._LblTotal = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this._LblSpriteSize = new System.Windows.Forms.Label();
      this._LblFrameSize = new System.Windows.Forms.Label();
      this._ChkDeleteExistingFrames = new System.Windows.Forms.CheckBox();
      this._ChkAutoGenFrames = new System.Windows.Forms.CheckBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this._grpFrameGeneration = new System.Windows.Forms.GroupBox();
      this._cmbAnimations = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this._CurFrame)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this._grpFrameGeneration.SuspendLayout();
      this.SuspendLayout();
      // 
      // _ImageList
      // 
      this._ImageList.FormattingEnabled = true;
      this._ImageList.Location = new System.Drawing.Point(6, 19);
      this._ImageList.Name = "_ImageList";
      this._ImageList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this._ImageList.Size = new System.Drawing.Size(212, 264);
      this._ImageList.TabIndex = 0;
      this._ImageList.SelectedIndexChanged += new System.EventHandler(this._ImageList_SelectedIndexChanged);
      // 
      // _CurFrame
      // 
      this._CurFrame.BackColor = System.Drawing.Color.Gray;
      this._CurFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._CurFrame.Location = new System.Drawing.Point(224, 19);
      this._CurFrame.Name = "_CurFrame";
      this._CurFrame.Size = new System.Drawing.Size(273, 206);
      this._CurFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this._CurFrame.TabIndex = 1;
      this._CurFrame.TabStop = false;
      // 
      // _SelFrames
      // 
      this._SelFrames.Location = new System.Drawing.Point(6, 289);
      this._SelFrames.Name = "_SelFrames";
      this._SelFrames.Size = new System.Drawing.Size(212, 23);
      this._SelFrames.TabIndex = 2;
      this._SelFrames.Text = "Select Frames...";
      this._SelFrames.UseVisualStyleBackColor = true;
      this._SelFrames.Click += new System.EventHandler(this._SelFrames_Click);
      // 
      // _btnOk
      // 
      this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._btnOk.Location = new System.Drawing.Point(158, 20);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 3;
      this._btnOk.Text = "&Ok";
      this._btnOk.UseVisualStyleBackColor = true;
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(239, 20);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 4;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._btnOk);
      this.groupBox1.Controls.Add(this._btnCancel);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox1.Location = new System.Drawing.Point(0, 467);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(508, 55);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      // 
      // _OpenFilesDlg
      // 
      this._OpenFilesDlg.DefaultExt = "png";
      this._OpenFilesDlg.Filter = "All supported image files|*.png;*.jpg|Png files|*.png|Bitmap files|*.bmp|JPG file" +
          "s|*.jpg";
      this._OpenFilesDlg.Multiselect = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 61);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(98, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Frames Per Column";
      // 
      // _TxtFramesPerColumn
      // 
      this._TxtFramesPerColumn.Location = new System.Drawing.Point(110, 58);
      this._TxtFramesPerColumn.Name = "_TxtFramesPerColumn";
      this._TxtFramesPerColumn.Size = new System.Drawing.Size(34, 20);
      this._TxtFramesPerColumn.TabIndex = 7;
      this._TxtFramesPerColumn.TextChanged += new System.EventHandler(this._TxtFramesPerColumn_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 16);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(71, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Total Frames:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(173, 61);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(60, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Sprite Size:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 39);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(59, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Frame Size";
      // 
      // _LblTotal
      // 
      this._LblTotal.AutoSize = true;
      this._LblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._LblTotal.Location = new System.Drawing.Point(83, 16);
      this._LblTotal.Name = "_LblTotal";
      this._LblTotal.Size = new System.Drawing.Size(81, 13);
      this._LblTotal.TabIndex = 11;
      this._LblTotal.Text = "[total frames]";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this._LblSpriteSize);
      this.groupBox2.Controls.Add(this._LblFrameSize);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this._LblTotal);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this._TxtFramesPerColumn);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox2.Location = new System.Drawing.Point(0, 261);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(508, 112);
      this.groupBox2.TabIndex = 12;
      this.groupBox2.TabStop = false;
      // 
      // _LblSpriteSize
      // 
      this._LblSpriteSize.AutoSize = true;
      this._LblSpriteSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._LblSpriteSize.Location = new System.Drawing.Point(242, 61);
      this._LblSpriteSize.Name = "_LblSpriteSize";
      this._LblSpriteSize.Size = new System.Drawing.Size(72, 13);
      this._LblSpriteSize.TabIndex = 13;
      this._LblSpriteSize.Text = "[sprite size]";
      // 
      // _LblFrameSize
      // 
      this._LblFrameSize.AutoSize = true;
      this._LblFrameSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._LblFrameSize.Location = new System.Drawing.Point(83, 39);
      this._LblFrameSize.Name = "_LblFrameSize";
      this._LblFrameSize.Size = new System.Drawing.Size(72, 13);
      this._LblFrameSize.TabIndex = 12;
      this._LblFrameSize.Text = "[frame size]";
      // 
      // _ChkDeleteExistingFrames
      // 
      this._ChkDeleteExistingFrames.AutoSize = true;
      this._ChkDeleteExistingFrames.Checked = true;
      this._ChkDeleteExistingFrames.CheckState = System.Windows.Forms.CheckState.Checked;
      this._ChkDeleteExistingFrames.Location = new System.Drawing.Point(15, 59);
      this._ChkDeleteExistingFrames.Name = "_ChkDeleteExistingFrames";
      this._ChkDeleteExistingFrames.Size = new System.Drawing.Size(129, 17);
      this._ChkDeleteExistingFrames.TabIndex = 15;
      this._ChkDeleteExistingFrames.Text = "Delete existing frames";
      this._ChkDeleteExistingFrames.UseVisualStyleBackColor = true;
      // 
      // _ChkAutoGenFrames
      // 
      this._ChkAutoGenFrames.AutoSize = true;
      this._ChkAutoGenFrames.Checked = true;
      this._ChkAutoGenFrames.CheckState = System.Windows.Forms.CheckState.Checked;
      this._ChkAutoGenFrames.Location = new System.Drawing.Point(9, 0);
      this._ChkAutoGenFrames.Name = "_ChkAutoGenFrames";
      this._ChkAutoGenFrames.Size = new System.Drawing.Size(127, 17);
      this._ChkAutoGenFrames.TabIndex = 14;
      this._ChkAutoGenFrames.Text = "Autogenerate frames ";
      this._ChkAutoGenFrames.UseVisualStyleBackColor = true;
      this._ChkAutoGenFrames.CheckedChanged += new System.EventHandler(this._ChkAutoGenFrames_CheckedChanged);
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this._ImageList);
      this.groupBox3.Controls.Add(this._CurFrame);
      this.groupBox3.Controls.Add(this._SelFrames);
      this.groupBox3.Location = new System.Drawing.Point(0, 0);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(508, 321);
      this.groupBox3.TabIndex = 13;
      this.groupBox3.TabStop = false;
      // 
      // _grpFrameGeneration
      // 
      this._grpFrameGeneration.Controls.Add(this._cmbAnimations);
      this._grpFrameGeneration.Controls.Add(this.label5);
      this._grpFrameGeneration.Controls.Add(this._ChkDeleteExistingFrames);
      this._grpFrameGeneration.Controls.Add(this._ChkAutoGenFrames);
      this._grpFrameGeneration.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._grpFrameGeneration.Location = new System.Drawing.Point(0, 373);
      this._grpFrameGeneration.Name = "_grpFrameGeneration";
      this._grpFrameGeneration.Size = new System.Drawing.Size(508, 94);
      this._grpFrameGeneration.TabIndex = 14;
      this._grpFrameGeneration.TabStop = false;
      // 
      // _cmbAnimations
      // 
      this._cmbAnimations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbAnimations.FormattingEnabled = true;
      this._cmbAnimations.Location = new System.Drawing.Point(140, 32);
      this._cmbAnimations.Name = "_cmbAnimations";
      this._cmbAnimations.Size = new System.Drawing.Size(231, 21);
      this._cmbAnimations.TabIndex = 17;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 35);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(122, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Animation to be affected";
      // 
      // ImportFramesForm
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(508, 522);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this._grpFrameGeneration);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ImportFramesForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Generate Image From Files";
      this.Load += new System.EventHandler(this.ImportFramesForm_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImportFramesForm_KeyDown);
      ((System.ComponentModel.ISupportInitialize)(this._CurFrame)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this._grpFrameGeneration.ResumeLayout(false);
      this._grpFrameGeneration.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox _ImageList;
    private System.Windows.Forms.PictureBox _CurFrame;
    private System.Windows.Forms.Button _SelFrames;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.OpenFileDialog _OpenFilesDlg;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _TxtFramesPerColumn;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label _LblTotal;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label _LblSpriteSize;
    private System.Windows.Forms.Label _LblFrameSize;
    private System.Windows.Forms.CheckBox _ChkDeleteExistingFrames;
    private System.Windows.Forms.CheckBox _ChkAutoGenFrames;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.GroupBox _grpFrameGeneration;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.ComboBox _cmbAnimations;
  }
}