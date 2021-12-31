using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Controls
{
  internal partial class Tile : UserControl
  {
    #region Variables
    Animation _Animation;
    int _FrameNr;
    bool _DrawBorder;
    #endregion

    #region Properties
    public Animation Animation
    {
      get { return this._Animation; }
      set { this._Animation = value; }
    }

    public int FrameNr
    {
      get { return this._FrameNr; }
      set { this._FrameNr = value; }
    }
    public bool DrawBorder
    {
      get { return this._DrawBorder; }
      set 
      {
        if (this._DrawBorder != value)
        {
          this._DrawBorder = value;
          this.Refresh();
        }
      }
    }
    #endregion

    #region Class constructs and overrides
    /// <summary>
    /// 
    /// </summary>
    public Tile()
    {
      InitializeComponent();
      this.FrameNr = -1;
    }

    /// <summary>
    /// 
    /// </summary>
    public Tile(Animation animation, int frameNr)
    {
      InitializeComponent();
      if (frameNr < 1 || frameNr > animation.Frames.Count)
        throw new ApplicationException("Invalid Frame number. Out of animation frame list.");
      this.Animation = animation;
      this.FrameNr = frameNr;
      this.Width = this.Animation.Frames[frameNr - 1].Rectangle.Width;
      this.Height = this.Animation.Frames[frameNr - 1].Rectangle.Height;
    }
    #endregion

    #region Control events
    /// <summary>
    /// 
    /// </summary>
    private void Tile_Paint(object sender, PaintEventArgs e)
    {
      try
      {
        if (this.Animation == null || this.FrameNr > this.Animation.FrameCount ||
            this.FrameNr < 0)
          return;
       
        Rectangle rect = this.Animation.Frames[this.FrameNr].Rectangle;
         e.Graphics.DrawImage(this.Animation.ParentProject.OutputSprite,
          new Rectangle(0, 0, rect.Width, rect.Height),
          rect, GraphicsUnit.Pixel);
        
        if (this.DrawBorder)
        {
          e.Graphics.DrawRectangle(Pens.LightGray, 0, 0, rect.Width - 1, rect.Height - 1);
        }
      }
      catch (System.Exception)
      {
      }
    }
    #endregion

  }
}
