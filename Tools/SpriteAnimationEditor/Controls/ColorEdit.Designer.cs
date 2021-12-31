namespace SpriteAnimationEditor.Controls
{
  partial class ColorEdit
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
      this.panel1 = new System.Windows.Forms.Panel();
      this.colorDialog1 = new System.Windows.Forms.ColorDialog();
      this._BtnPick = new System.Windows.Forms.Button();
      this.panel2 = new System.Windows.Forms.Panel();
      this.panel3 = new System.Windows.Forms.Panel();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      // 
      // panel1
      // 
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(54, 23);
      this.panel1.TabIndex = 1;
      // 
      // _BtnPick
      // 
      this._BtnPick.Location = new System.Drawing.Point(60, 0);
      this._BtnPick.Name = "_BtnPick";
      this._BtnPick.Size = new System.Drawing.Size(23, 23);
      this._BtnPick.TabIndex = 2;
      this._BtnPick.Text = "...";
      this._BtnPick.UseVisualStyleBackColor = true;
      this._BtnPick.Click += new System.EventHandler(this._BtnPick_Click);
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.label1);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(200, 42);
      this.panel2.TabIndex = 3;
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this.panel1);
      this.panel3.Controls.Add(this._BtnPick);
      this.panel3.Location = new System.Drawing.Point(206, 3);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(200, 31);
      this.panel3.TabIndex = 4;
      // 
      // ColorEdit
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel3);
      this.Controls.Add(this.panel2);
      this.Name = "ColorEdit";
      this.Size = new System.Drawing.Size(364, 42);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ColorDialog colorDialog1;
    private System.Windows.Forms.Button _BtnPick;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel panel3;
  }
}
