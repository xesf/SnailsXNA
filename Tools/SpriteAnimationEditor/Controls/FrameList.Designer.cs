namespace SpriteAnimationEditor.Controls
{
  partial class FrameList
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
      this.components = new System.ComponentModel.Container();
      this.group1 = new SpriteAnimationEditor.Controls.Group();
      this._AnimationListPanel = new System.Windows.Forms.Panel();
      this._ListAnimation = new System.Windows.Forms.ListView();
      this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this._MenuAnimFrames = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._OptSelAllAnimFrames = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this._OptRemAnimFrames = new System.Windows.Forms.ToolStripMenuItem();
      this.group1.BodyPanel.SuspendLayout();
      this.group1.SuspendLayout();
      this._AnimationListPanel.SuspendLayout();
      this._MenuAnimFrames.SuspendLayout();
      this.SuspendLayout();
      // 
      // group1
      // 
      // 
      // group1._PanelBody
      // 
      this.group1.BodyPanel.BackColor = System.Drawing.Color.Transparent;
      this.group1.BodyPanel.Controls.Add(this._AnimationListPanel);
      this.group1.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.group1.BodyPanel.Location = new System.Drawing.Point(0, 18);
      this.group1.BodyPanel.Name = "_PanelBody";
      this.group1.BodyPanel.Size = new System.Drawing.Size(694, 255);
      this.group1.BodyPanel.TabIndex = 2;
      this.group1.CaptionBackColor = System.Drawing.Color.DarkGray;
      this.group1.CaptionForeColor = System.Drawing.Color.Black;
      this.group1.CaptionVisible = true;
      this.group1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.group1.Location = new System.Drawing.Point(0, 0);
      this.group1.Name = "group1";
      this.group1.Size = new System.Drawing.Size(694, 273);
      this.group1.TabIndex = 1;
      this.group1.Text = "Frame List";
      // 
      // _AnimationListPanel
      // 
      this._AnimationListPanel.Controls.Add(this._ListAnimation);
      this._AnimationListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this._AnimationListPanel.Location = new System.Drawing.Point(0, 0);
      this._AnimationListPanel.Name = "_AnimationListPanel";
      this._AnimationListPanel.Size = new System.Drawing.Size(694, 255);
      this._AnimationListPanel.TabIndex = 5;
      // 
      // _ListAnimation
      // 
      this._ListAnimation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader7,
            this.columnHeader1});
      this._ListAnimation.ContextMenuStrip = this._MenuAnimFrames;
      this._ListAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
      this._ListAnimation.FullRowSelect = true;
      this._ListAnimation.GridLines = true;
      this._ListAnimation.HideSelection = false;
      this._ListAnimation.Location = new System.Drawing.Point(0, 0);
      this._ListAnimation.Name = "_ListAnimation";
      this._ListAnimation.Size = new System.Drawing.Size(694, 255);
      this._ListAnimation.TabIndex = 1;
      this._ListAnimation.UseCompatibleStateImageBehavior = false;
      this._ListAnimation.View = System.Windows.Forms.View.Details;
      this._ListAnimation.SelectedIndexChanged += new System.EventHandler(this._ListAnimation_SelectedIndexChanged);
      this._ListAnimation.DoubleClick += new System.EventHandler(this._ListAnimation_DoubleClick);
      this._ListAnimation.Click += new System.EventHandler(this._ListAnimation_Click);
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "#";
      this.columnHeader5.Width = 31;
      // 
      // columnHeader7
      // 
      this.columnHeader7.Text = "Region";
      this.columnHeader7.Width = 269;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Colision Detection";
      this.columnHeader1.Width = 338;
      // 
      // _MenuAnimFrames
      // 
      this._MenuAnimFrames.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptSelAllAnimFrames,
            this.toolStripSeparator3,
            this._OptRemAnimFrames});
      this._MenuAnimFrames.Name = "_MenuAnimFrames";
      this._MenuAnimFrames.Size = new System.Drawing.Size(207, 54);
      this._MenuAnimFrames.Opening += new System.ComponentModel.CancelEventHandler(this._MenuAnimFrames_Opening);
      // 
      // _OptSelAllAnimFrames
      // 
      this._OptSelAllAnimFrames.Name = "_OptSelAllAnimFrames";
      this._OptSelAllAnimFrames.Size = new System.Drawing.Size(206, 22);
      this._OptSelAllAnimFrames.Text = "Select All Frames";
      this._OptSelAllAnimFrames.Click += new System.EventHandler(this._OptSelAllAnimFrames_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(203, 6);
      // 
      // _OptRemAnimFrames
      // 
      this._OptRemAnimFrames.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._OptRemAnimFrames.Name = "_OptRemAnimFrames";
      this._OptRemAnimFrames.Size = new System.Drawing.Size(206, 22);
      this._OptRemAnimFrames.Text = "Remove Selected Frames";
      this._OptRemAnimFrames.Click += new System.EventHandler(this._OptRemAnimFrames_Click);
      // 
      // FrameList
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.group1);
      this.Name = "FrameList";
      this.Size = new System.Drawing.Size(694, 273);
      this.group1.BodyPanel.ResumeLayout(false);
      this.group1.ResumeLayout(false);
      this._AnimationListPanel.ResumeLayout(false);
      this._MenuAnimFrames.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private SpriteAnimationEditor.Controls.Group group1;
    private System.Windows.Forms.ListView _ListAnimation;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.Panel _AnimationListPanel;
    private System.Windows.Forms.ContextMenuStrip _MenuAnimFrames;
    private System.Windows.Forms.ToolStripMenuItem _OptSelAllAnimFrames;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem _OptRemAnimFrames;
    private System.Windows.Forms.ColumnHeader columnHeader1;
  }
}
