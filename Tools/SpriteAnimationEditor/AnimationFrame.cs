using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Xml;

namespace SpriteAnimationEditor
{
  [Serializable]
  class AnimationFrame : IViewState
  {
    #region Consts
    const string DEF_CATEGORY = "Frame";
    #endregion

    #region Events
    public event EventHandler FrameAttributeChanged;
    #endregion

    #region Variables
    Rectangle _Rectangle;
    int _FrameNr;
    Animation _ParentAnimation;
    List<ColisionAreaBase> _ColisionZones;
    int _PlayTime;
    bool _Changed;
    float _rotation;
    
    Point _offset;
    Point _pivot;
    #endregion

    #region Browsable attributes
    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public Rectangle Rectangle
    {
      get { return this._Rectangle; }
      set 
      {
        if (this._Rectangle != value)
        {
          this._Rectangle = value;
          this.OnFrameAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    [DefaultValue(0)]
    public int PlayTime
    {
      get { return this._PlayTime; }
      set
      {
        if (this._PlayTime != value)
        {
          this._PlayTime = value;
          this.OnFrameAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public int FrameNr
    {
      get { return this._FrameNr; }
      private set 
      {
        if (this._FrameNr != value)
        {
          this._FrameNr = value;
          this.OnFrameAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public float Rotation
    {
        get { return this._rotation; }
        set
        {
            if (this._rotation != value)
            {
                this._rotation = value;
                this.OnFrameAttributeChanged();
            }
        }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public Point Offset
    {
        get { return this._offset; }
        set
        {
            if (this._offset != value)
            {
                this._offset = value;
                this.OnFrameAttributeChanged();
            }
        }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public Point Pivot
    {
      get { return this._pivot; }
      set
      {
        if (this._pivot != value)
        {
          this._pivot = value;
          this.OnFrameAttributeChanged();
        }
      }
    }
    #endregion

    #region Non-browsable attributes
    [BrowsableAttribute(false)]
    public List<ColisionAreaBase> ColisionZones
    {
      get { return this._ColisionZones; }
      set 
      { 
        if (this._ColisionZones != value) 
        {
          this._ColisionZones = value;
          this.OnFrameAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(false)]
    public bool Changed
    {
      get { return this._Changed; }
      set { this._Changed = value; }
    }

    [BrowsableAttribute(false)]
    public Animation ParentAnimation
    {
      get { return this._ParentAnimation; }
      set 
      {
        if (this._ParentAnimation != value)
        {
          this._ParentAnimation = value;
          this.Changed = true;
        }
      }
    }
    #endregion

    #region Class constructs and overrides
    /// <summary>
    /// 
    /// </summary>
    private AnimationFrame()
    {
      this.ColisionZones = new List<ColisionAreaBase>();
      this.Changed = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public AnimationFrame (Rectangle rectangle, Animation animation)
    {
      this.Initialize(rectangle, 0, animation);
    }

    /// <summary>
    /// 
    /// </summary>
    public AnimationFrame(int x, int y, int width, int height, Animation animation)
    {
      this.Initialize(new Rectangle(x, y, width, height), 0, animation);
    }
   
    /// <summary>
    /// 
    /// </summary>
    public AnimationFrame(Rectangle rect, int frameNr, Animation animation)
    {
      this.Initialize(rect, frameNr, animation);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Initialize(Rectangle rect, int frameNr, Animation animation)
    {
      this.Rectangle = rect;
      this.FrameNr = frameNr;
      this.ParentAnimation = animation;
      this.ColisionZones = new List<ColisionAreaBase>();
      this.Changed = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public static AnimationFrame FromXml(XmlElement elem, Animation animation)
    {
      AnimationFrame frame = new AnimationFrame();

      frame.InitFromXml(elem);
      frame.ParentAnimation = animation;

      return frame;
    }

    #endregion

    #region Event launching
    /// <summary>
    /// 
    /// </summary>
    private void OnFrameAttributeChanged()
    {
      this.Changed = true;
      if (this.FrameAttributeChanged != null)
      {
        this.FrameAttributeChanged(this, new EventArgs());
      }
    }
    #endregion

    #region Other methods
    /// <summary>
    /// 
    /// </summary>
    public void SetNumber(int number)
    {
      this.FrameNr = number;
    }

    /// <summary>
    /// 
    /// </summary>
    public AnimationFrame Clone()
    {
      AnimationFrame cloneFrame = new AnimationFrame(this.Rectangle, this.ParentAnimation);
      cloneFrame.PlayTime = this.PlayTime;
      cloneFrame.ColisionZones = new List<ColisionAreaBase>();
      foreach (ColisionAreaBase col in this.ColisionZones)
      {
        cloneFrame.ColisionZones.Add((ColisionAreaBase)col.Clone());
      }
      cloneFrame.Offset = this.Offset;
      cloneFrame.Rotation = this.Rotation;
      cloneFrame.Pivot = this.Pivot;
      cloneFrame.ParentAnimation = this.ParentAnimation;
      
      return cloneFrame;
    }

    /// <summary>
    /// 
    /// </summary>
    public AnimationFrame Scale(double factorX, double factorY)
    {
      AnimationFrame frame = new AnimationFrame();
      frame.Rectangle = new Rectangle((int)Math.Round(this.Rectangle.X * factorX, 0),
                                      (int)Math.Round(this.Rectangle.Y * factorY, 0),
                                      (int)Math.Round(this.Rectangle.Width * factorX, 0),
                                      (int)Math.Round(this.Rectangle.Height * factorY, 0));
      foreach (ColisionAreaBase col in this.ColisionZones)
      {
        frame.ColisionZones.Add(col.Scale(factorX, factorY));
      }
      return frame;
    }
    #endregion

    #region IViewState Members
    /// <summary>
    /// 
    /// </summary>
    public void InitFromXml(System.Xml.XmlElement elemFrame)
    {
      int x = Convert.ToInt32(elemFrame.Attributes["Left"].Value);
      int y = Convert.ToInt32(elemFrame.Attributes["Top"].Value);
      int width = Convert.ToInt32(elemFrame.Attributes["Width"].Value);
      int height = Convert.ToInt32(elemFrame.Attributes["Height"].Value);
      this.Rectangle = new Rectangle(x, y, width, height);
      this.PlayTime = XmlHelper.GetAttribute(elemFrame, "PlayTime", 0);
      this._offset = new Point(XmlHelper.GetAttribute(elemFrame, "OffsetX", 0), XmlHelper.GetAttribute(elemFrame, "OffsetY", 0));
      this._rotation = XmlHelper.GetAttribute(elemFrame, "Rotation", 0.0f);
      this._pivot = new Point(XmlHelper.GetAttribute(elemFrame, "PivotX", 0), XmlHelper.GetAttribute(elemFrame, "PivotY", 0));

	  this.ColisionZones = new List<ColisionAreaBase>();
	  XmlNodeList elemColAreas = elemFrame.SelectNodes("ColisionZones/BoundingBox");
	  foreach (XmlElement elemCol in elemColAreas)
	  {
		  this.ColisionZones.Add(BoundingBox.FromXml(elemCol));
	  }
    }

    /// <summary>
    /// 
    /// </summary>
    public System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      XmlElement elemFrame;

      elemFrame = xmlDoc.CreateElement("Frame");
      elemFrame.SetAttribute("Left", this.Rectangle.Left.ToString());
      elemFrame.SetAttribute("Top", this.Rectangle.Top.ToString());
      elemFrame.SetAttribute("Width", this.Rectangle.Width.ToString());
      elemFrame.SetAttribute("Height", this.Rectangle.Height.ToString());
      elemFrame.SetAttribute("PlayTime", this.PlayTime.ToString());
      elemFrame.SetAttribute("OffsetX", this.Offset.X.ToString());
      elemFrame.SetAttribute("OffsetY", this.Offset.Y.ToString());
      elemFrame.SetAttribute("Rotation", this._rotation.ToString());
      elemFrame.SetAttribute("PivotX", this.Pivot.X.ToString());
      elemFrame.SetAttribute("PivotY", this.Pivot.Y.ToString());

	  if (this.ColisionZones.Count > 0)
	  {
		  // Collision areas
		  XmlElement elemColAreas = xmlDoc.CreateElement("ColisionZones");
		  foreach (ColisionAreaBase colArea in this.ColisionZones)
		  {
			  elemColAreas.AppendChild(colArea.ToXml(xmlDoc));
		  }
		  elemFrame.AppendChild(elemColAreas);
	  }
      return elemFrame;
    }
    #endregion
  }
}
