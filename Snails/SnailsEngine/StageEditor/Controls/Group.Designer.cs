namespace TwoBrains.Common.Controls
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
            this._lblCollapse = new System.Windows.Forms.Label();
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
            this._Caption.Size = new System.Drawing.Size(145, 14);
            this._Caption.TabIndex = 0;
            this._Caption.Text = "label1";
            // 
            // _PanelCaption
            // 
            this._PanelCaption.BackColor = System.Drawing.Color.Transparent;
            this._PanelCaption.Controls.Add(this._lblCollapse);
            this._PanelCaption.Controls.Add(this._Caption);
            this._PanelCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this._PanelCaption.Location = new System.Drawing.Point(5, 5);
            this._PanelCaption.Name = "_PanelCaption";
            this._PanelCaption.Padding = new System.Windows.Forms.Padding(2);
            this._PanelCaption.Size = new System.Drawing.Size(149, 18);
            this._PanelCaption.TabIndex = 1;
            // 
            // _lblCollapse
            // 
            this._lblCollapse.Dock = System.Windows.Forms.DockStyle.Right;
            this._lblCollapse.Location = new System.Drawing.Point(130, 2);
            this._lblCollapse.Name = "_lblCollapse";
            this._lblCollapse.Size = new System.Drawing.Size(17, 14);
            this._lblCollapse.TabIndex = 1;
            this._lblCollapse.Text = "-";
            this._lblCollapse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._lblCollapse.Visible = false;
            this._lblCollapse.Click += new System.EventHandler(this._lblCollapse_Click);
            // 
            // _PanelBody
            // 
            this._PanelBody.BackColor = System.Drawing.SystemColors.Control;
            this._PanelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this._PanelBody.Location = new System.Drawing.Point(5, 23);
            this._PanelBody.Name = "_PanelBody";
            this._PanelBody.Size = new System.Drawing.Size(149, 133);
            this._PanelBody.TabIndex = 2;
            // 
            // Group
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._PanelBody);
            this.Controls.Add(this._PanelCaption);
            this.Name = "Group";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(159, 161);
            this._PanelCaption.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label _Caption;
    private System.Windows.Forms.Panel _PanelCaption;
    private System.Windows.Forms.Panel _PanelBody;
    private System.Windows.Forms.Label _lblCollapse;
  }
}
