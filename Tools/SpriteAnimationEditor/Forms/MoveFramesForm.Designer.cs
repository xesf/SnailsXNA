namespace SpriteAnimationEditor.Forms
{
  partial class MoveFramesForm
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
      this.panel1 = new System.Windows.Forms.Panel();
      this._btnCancel = new System.Windows.Forms.Button();
      this._btnOk = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this._txtOffsetY = new System.Windows.Forms.TextBox();
      this._txtOffsetX = new System.Windows.Forms.TextBox();
      this._lbl = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this._chkSelectedOnly = new System.Windows.Forms.CheckBox();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this._btnCancel);
      this.panel1.Controls.Add(this._btnOk);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 140);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(235, 34);
      this.panel1.TabIndex = 0;
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(126, 6);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 1;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // _btnOk
      // 
      this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._btnOk.Location = new System.Drawing.Point(45, 6);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 0;
      this._btnOk.Text = "&Ok";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._chkSelectedOnly);
      this.groupBox1.Controls.Add(this._txtOffsetY);
      this.groupBox1.Controls.Add(this._txtOffsetX);
      this.groupBox1.Controls.Add(this._lbl);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(235, 140);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      // 
      // _txtOffsetY
      // 
      this._txtOffsetY.Location = new System.Drawing.Point(98, 59);
      this._txtOffsetY.Name = "_txtOffsetY";
      this._txtOffsetY.Size = new System.Drawing.Size(48, 20);
      this._txtOffsetY.TabIndex = 3;
      this._txtOffsetY.Text = "0";
      this._txtOffsetY.TextChanged += new System.EventHandler(this._txtOffsetY_TextChanged);
      // 
      // _txtOffsetX
      // 
      this._txtOffsetX.Location = new System.Drawing.Point(98, 31);
      this._txtOffsetX.Name = "_txtOffsetX";
      this._txtOffsetX.Size = new System.Drawing.Size(48, 20);
      this._txtOffsetX.TabIndex = 2;
      this._txtOffsetX.Text = "0";
      this._txtOffsetX.TextChanged += new System.EventHandler(this._txtOffsetX_TextChanged);
      // 
      // _lbl
      // 
      this._lbl.AutoSize = true;
      this._lbl.Location = new System.Drawing.Point(43, 62);
      this._lbl.Name = "_lbl";
      this._lbl.Size = new System.Drawing.Size(45, 13);
      this._lbl.TabIndex = 1;
      this._lbl.Text = "Y Offset";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(43, 34);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(45, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "X Offset";
      // 
      // _chkSelectedOnly
      // 
      this._chkSelectedOnly.AutoSize = true;
      this._chkSelectedOnly.Location = new System.Drawing.Point(45, 96);
      this._chkSelectedOnly.Name = "_chkSelectedOnly";
      this._chkSelectedOnly.Size = new System.Drawing.Size(163, 17);
      this._chkSelectedOnly.TabIndex = 4;
      this._chkSelectedOnly.Text = "Apply to selected frames only";
      this._chkSelectedOnly.UseVisualStyleBackColor = true;
      // 
      // MoveFramesForm
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(235, 174);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "MoveFramesForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Move Frames";
      this.Load += new System.EventHandler(this.MoveFramesForm_Load);
      this.panel1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox _txtOffsetY;
    private System.Windows.Forms.TextBox _txtOffsetX;
    private System.Windows.Forms.Label _lbl;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox _chkSelectedOnly;
  }
}