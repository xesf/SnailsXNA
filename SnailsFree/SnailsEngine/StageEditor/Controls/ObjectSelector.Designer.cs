namespace TwoBrainsGames.Snails.StageEditor.Controls
{
  partial class ObjectSelector
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
            this.group1 = new TwoBrains.Common.Controls.Group();
            this._ObjectList = new TwoBrainsGames.Snails.StageEditor.Controls.ImageListx();
            this.group1.BodyPanel.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // group1
            // 
            this.group1.AllowCollapse = true;
            // 
            // group1._PanelBody
            // 
            this.group1.BodyPanel.BackColor = System.Drawing.SystemColors.Control;
            this.group1.BodyPanel.Controls.Add(this._ObjectList);
            this.group1.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group1.BodyPanel.Location = new System.Drawing.Point(5, 23);
            this.group1.BodyPanel.Name = "_PanelBody";
            this.group1.BodyPanel.Size = new System.Drawing.Size(140, 122);
            this.group1.BodyPanel.TabIndex = 2;
            this.group1.CaptionBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.group1.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.group1.CaptionVisible = true;
            this.group1.Collapsed = false;
            this.group1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group1.Location = new System.Drawing.Point(0, 0);
            this.group1.Name = "group1";
            this.group1.Padding = new System.Windows.Forms.Padding(5);
            this.group1.Size = new System.Drawing.Size(150, 150);
            this.group1.TabIndex = 0;
            this.group1.Text = "Objects";
            this.group1.Resize += new System.EventHandler(this.group1_Resize);
            // 
            // _ObjectList
            // 
            this._ObjectList.AutoScroll = true;
            this._ObjectList.BackColor = System.Drawing.Color.White;
            this._ObjectList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._ObjectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ObjectList.Location = new System.Drawing.Point(0, 0);
            this._ObjectList.Name = "_ObjectList";
            this._ObjectList.Padding = new System.Windows.Forms.Padding(5);
            this._ObjectList.SelectedItem = null;
            this._ObjectList.Size = new System.Drawing.Size(140, 122);
            this._ObjectList.TabIndex = 0;
            this._ObjectList.SelectedItemChanged += new System.EventHandler(this._ObjectList_SelectedItemChanged);
            // 
            // ObjectSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.group1);
            this.Name = "ObjectSelector";
            this.group1.BodyPanel.ResumeLayout(false);
            this.group1.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private TwoBrains.Common.Controls.Group group1;
    private ImageListx _ObjectList;
  }
}
