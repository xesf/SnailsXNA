namespace SpriteAnimationEditor.Forms
{
  partial class GridForm
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
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this._BtnOk = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this._txtHeight = new System.Windows.Forms.TextBox();
      this._txtWidth = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this._txtOffsetY = new System.Windows.Forms.TextBox();
      this._txtOffsetX = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this._BtnOk);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox2.Location = new System.Drawing.Point(0, 157);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(276, 45);
      this.groupBox2.TabIndex = 4;
      this.groupBox2.TabStop = false;
      // 
      // _BtnOk
      // 
      this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._BtnOk.Location = new System.Drawing.Point(106, 16);
      this._BtnOk.Name = "_BtnOk";
      this._BtnOk.Size = new System.Drawing.Size(75, 23);
      this._BtnOk.TabIndex = 0;
      this._BtnOk.Text = "&Ok";
      this._BtnOk.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this._txtOffsetY);
      this.groupBox1.Controls.Add(this._txtOffsetX);
      this.groupBox1.Controls.Add(this.label6);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this._txtHeight);
      this.groupBox1.Controls.Add(this._txtWidth);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(276, 157);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(83, 48);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(38, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Height";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(83, 25);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(35, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Width";
      // 
      // _txtHeight
      // 
      this._txtHeight.Location = new System.Drawing.Point(128, 48);
      this._txtHeight.Name = "_txtHeight";
      this._txtHeight.Size = new System.Drawing.Size(53, 20);
      this._txtHeight.TabIndex = 2;
      // 
      // _txtWidth
      // 
      this._txtWidth.Location = new System.Drawing.Point(128, 22);
      this._txtWidth.Name = "_txtWidth";
      this._txtWidth.Size = new System.Drawing.Size(53, 20);
      this._txtWidth.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(24, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(30, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Size:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(83, 112);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(14, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Y";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(83, 89);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(14, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "X";
      // 
      // _txtOffsetY
      // 
      this._txtOffsetY.Location = new System.Drawing.Point(128, 112);
      this._txtOffsetY.Name = "_txtOffsetY";
      this._txtOffsetY.Size = new System.Drawing.Size(53, 20);
      this._txtOffsetY.TabIndex = 7;
      // 
      // _txtOffsetX
      // 
      this._txtOffsetX.Location = new System.Drawing.Point(128, 86);
      this._txtOffsetX.Name = "_txtOffsetX";
      this._txtOffsetX.Size = new System.Drawing.Size(53, 20);
      this._txtOffsetX.TabIndex = 6;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(24, 89);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(38, 13);
      this.label6.TabIndex = 5;
      this.label6.Text = "Offset:";
      // 
      // GridForm
      // 
      this.AcceptButton = this._BtnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(276, 202);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.groupBox2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "GridForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Grid Settings";
      this.groupBox2.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button _BtnOk;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox _txtWidth;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _txtHeight;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox _txtOffsetY;
    private System.Windows.Forms.TextBox _txtOffsetX;
    private System.Windows.Forms.Label label6;
  }
}