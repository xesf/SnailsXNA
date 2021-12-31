namespace SpriteAnimationEditor.Controls
{
  partial class OutputSprite
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
      this._Title = new SpriteAnimationEditor.Controls.Group();
      this._PanelContainer = new System.Windows.Forms.Panel();
      this._lblNoSpriteMsg = new System.Windows.Forms.Label();
      this._PanelImage = new SpriteAnimationEditor.Controls.DoubleBufferedPanel();
      this._Title.BodyPanel.SuspendLayout();
      this._Title.SuspendLayout();
      this._PanelContainer.SuspendLayout();
      this.SuspendLayout();
      // 
      // _Title
      // 
      // 
      // _Title._PanelBody
      // 
      this._Title.BodyPanel.BackColor = System.Drawing.Color.Transparent;
      this._Title.BodyPanel.Controls.Add(this._PanelContainer);
      this._Title.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Title.BodyPanel.Location = new System.Drawing.Point(0, 18);
      this._Title.BodyPanel.Name = "_PanelBody";
      this._Title.BodyPanel.Size = new System.Drawing.Size(481, 305);
      this._Title.BodyPanel.TabIndex = 2;
      this._Title.CaptionBackColor = System.Drawing.Color.DarkGray;
      this._Title.CaptionForeColor = System.Drawing.Color.Black;
      this._Title.CaptionVisible = true;
      this._Title.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Title.Location = new System.Drawing.Point(0, 0);
      this._Title.Name = "_Title";
      this._Title.Size = new System.Drawing.Size(481, 323);
      this._Title.TabIndex = 1;
      this._Title.Text = "Output Sprite";
      // 
      // _PanelContainer
      // 
      this._PanelContainer.AutoScroll = true;
      this._PanelContainer.Controls.Add(this._lblNoSpriteMsg);
      this._PanelContainer.Controls.Add(this._PanelImage);
      this._PanelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this._PanelContainer.Location = new System.Drawing.Point(0, 0);
      this._PanelContainer.Name = "_PanelContainer";
      this._PanelContainer.Size = new System.Drawing.Size(481, 305);
      this._PanelContainer.TabIndex = 3;
      // 
      // _lblNoSpriteMsg
      // 
      this._lblNoSpriteMsg.Location = new System.Drawing.Point(22, 29);
      this._lblNoSpriteMsg.Name = "_lblNoSpriteMsg";
      this._lblNoSpriteMsg.Size = new System.Drawing.Size(267, 178);
      this._lblNoSpriteMsg.TabIndex = 0;
      this._lblNoSpriteMsg.Text = "Project has no image specified. \r\nSelect Image from Project menu.";
      this._lblNoSpriteMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _PanelImage
      // 
      this._PanelImage.BackColor = System.Drawing.SystemColors.Control;
      this._PanelImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._PanelImage.Location = new System.Drawing.Point(225, 3);
      this._PanelImage.Name = "_PanelImage";
      this._PanelImage.Size = new System.Drawing.Size(233, 165);
      this._PanelImage.TabIndex = 2;
      this._PanelImage.Paint += new System.Windows.Forms.PaintEventHandler(this._PanelImage_Paint);
      this._PanelImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this._PanelImage_MouseMove);
      this._PanelImage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._PanelImage_MouseDoubleClick);
      this._PanelImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this._PanelImage_MouseClick);
      this._PanelImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this._PanelImage_MouseDown);
      this._PanelImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this._PanelImage_MouseUp);
      // 
      // OutputSprite
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._Title);
      this.Name = "OutputSprite";
      this.Size = new System.Drawing.Size(481, 323);
      this._Title.BodyPanel.ResumeLayout(false);
      this._Title.ResumeLayout(false);
      this._PanelContainer.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private SpriteAnimationEditor.Controls.Group _Title;
    private Controls.DoubleBufferedPanel _PanelImage;
    private System.Windows.Forms.Label _lblNoSpriteMsg;
    private System.Windows.Forms.Panel _PanelContainer;
  }
}
