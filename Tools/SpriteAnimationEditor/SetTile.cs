using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;

namespace SpriteAnimationEditor
{
  class SetTile : IViewState
  {
    #region Variables
    Animation _Animation;
    int _FrameNr;
    Point _Location;
    #endregion

    #region Properties
    public Animation Animation
    {
      get { return this._Animation; }
      private set { this._Animation = value; }
    }

    public int FrameNr
    {
      get { return this._FrameNr; }
      private set { this._FrameNr = value; }
    }

    public Point Location
    {
      get { return this._Location; }
      set { this._Location = value; }
    }

    public int Width
    {
      get { return this.Rectangle.Width; }
    }
    public int Height
    {
      get { return this.Rectangle.Height; }
    }

    public Rectangle Rectangle
    {
      get 
      {
        if (this.Animation == null)
          return new Rectangle();
        if (this.FrameNr < 0 || this.FrameNr > this.Animation.FrameCount)
          return new Rectangle();

        return this.Animation.Frames[this.FrameNr].Rectangle;
      }
    }
    #endregion

    #region Class constructs and overrides
    /// <summary>
    /// 
    /// </summary>
    private SetTile()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public SetTile(Animation animation, int frameNr)
    {
      this._Animation = animation;
      this._FrameNr = frameNr;
    }

    /// <summary>
    /// 
    /// </summary>
    public static SetTile FromXml(XmlElement elem, Solution solution)
    {
      SetTile tile = new SetTile();
      tile.InitFromXml(elem, solution);
      return tile;
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool TryFromXml(XmlElement elem, Solution solution)
    {
      SetTile tileSet = new SetTile();
      tileSet.InitFromXml(elem, solution);

      if (tileSet.Animation == null ||
          tileSet.FrameNr <= 0 ||
          tileSet.FrameNr > tileSet.Animation.FrameCount)
        return false;

      return true;
    }
    #endregion

    #region IViewState Members

    /// <summary>
    /// 
    /// </summary>
    public void InitFromXml(XmlElement elem, Solution solution)
    {
      int x = XmlHelper.GetAttribute(elem, "X", 0);
      int y = XmlHelper.GetAttribute(elem, "Y", 0);
      this.Location = new Point(x, y);
      
      string projectName = XmlHelper.GetAttribute(elem, "Project", null);
      string animationName = XmlHelper.GetAttribute(elem, "Animation", null);
      Project project = solution.GetProjectByName(projectName);
      this.Animation = solution.GetAnimationByName(project, animationName);
      this.FrameNr = XmlHelper.GetAttribute(elem, "FrameNr", 0);
    }

    /// <summary>
    /// 
    /// </summary>
    public void InitFromXml(XmlElement elem)
    {
      throw new ApplicationException("Invalid method. Use SetTile.InitFromXml overloading method.");
    }

    /// <summary>
    /// 
    /// </summary>
    public XmlElement ToXml(XmlDocument xmlDoc)
    {
      XmlElement elem = xmlDoc.CreateElement("SetTile");

      elem.SetAttribute("X", this.Location.X.ToString());
      elem.SetAttribute("Y", this.Location.Y.ToString());
      elem.SetAttribute("Project", this.Animation.ParentProject.Name);
      elem.SetAttribute("Animation", this.Animation.Name);
      elem.SetAttribute("FrameNr", this.FrameNr.ToString());

      return elem;
    }

    #endregion
  }
}
