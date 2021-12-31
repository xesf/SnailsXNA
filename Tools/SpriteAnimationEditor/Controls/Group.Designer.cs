namespace SpriteAnimationEditor.Controls
{
  partial class Group
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
      this._Caption = new System.Windows.Forms.Label();
      this._PanelCaption = new System.Windows.Forms.Panel();
      this._PanelBody = new System.Windows.Forms.Panel();
      this._PanelCaption.SuspendLayout();
      this.SuspendLayout();
      // 
      // _Caption
      // 
      this._Caption.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this._Caption.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Caption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this._Caption.Location = new System.Drawing.Point(2, 2);
      this._Caption.Name = "_Caption";
      this._Caption.Size = new System.Drawing.Size(155, 14);
      this._Caption.TabIndex = 0;
      this._Caption.Text = "label1";
      // 
      // _PanelCaption
      // 
      this._PanelCaption.BackColor = System.Drawing.Color.Transparent;
      this._PanelCaption.Controls.Add(this._Caption);
      this._PanelCaption.Dock = System.Windows.Forms.DockStyle.Top;
      this._PanelCaption.Location = new System.Drawing.Point(0, 0);
      this._PanelCaption.Name = "_PanelCaption";
      this._PanelCaption.Padding = new System.Windows.Forms.Padding(2);
      this._PanelCaption.Size = new System.Drawing.Size(159, 18);
      this._PanelCaption.TabIndex = 1;
      // 
      // _PanelBody
      // 
      this._PanelBody.BackColor = System.Drawing.SystemColors.Control;
      this._PanelBody.Dock = System.Windows.Forms.DockStyle.Fill;
      this._PanelBody.Location = new System.Drawing.Point(0, 18);
      this._PanelBody.Name = "_PanelBody";
      this._PanelBody.Size = new System.Drawing.Size(159, 143);
      this._PanelBody.TabIndex = 2;
      // 
      // Group
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._PanelBody);
      this.Controls.Add(this._PanelCaption);
      this.Name = "Group";
      this.Size = new System.Drawing.Size(159, 161);
      this._PanelCaption.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label _Caption;
    private System.Windows.Forms.Panel _PanelCaption;
    private System.Windows.Forms.Panel _PanelBody;
  }
}
