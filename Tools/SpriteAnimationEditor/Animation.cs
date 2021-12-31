using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace SpriteAnimationEditor
{
  [DefaultPropertyAttribute("Name")]
  class Animation
  {
    #region Consts
    const string DEF_CATEGORY = "Animation";
    const string CATEGORY_SHADOW = "Shadow";
    const string DEF_NAME = "unnamed animation";

    const bool WITH_SHADOW_DEF_VAL = false;
    static SizeF SHADOW_DEPTH_DEF_VAL = new SizeF(2.0f, 2.0f);
    const double SHADOW_LAYER_DEPTH_DEF_VAL = 0.2d;

    const int DEF_FPS = 12;
    const bool RENDER_FRAME_NRS_DEF = false;
    #endregion

    #region Events
    public delegate void FrameAddedHandler(object sender, AnimationFrame frame);
    public delegate void FrameAttributeChangedHandler(object sender, AnimationFrame frame);

    public event EventHandler FramesPerSecondChanged;
    public event FrameAddedHandler FrameAdded;
    public event EventHandler RenderFrameNrsChanged;
    public event EventHandler AnimationChanged;
    public event EventHandler NameChanged;
    public event EventHandler AttributeChanged;
    public event FrameAttributeChangedHandler FrameAttributeChanged;
    public event EventHandler ParentAnimationChanged;
    
    #endregion

    #region Variables
    Project _ParentProject;
    string _Name;
    List<AnimationFrame> _Frames;
    int _FramesPerSecond;
    bool _RenderFrameNrs;
    Point _Offset;
    bool _Changed;
    List<ColisionAreaBase> _ColisionZones;
    Grid _Grid;
    Animation _parentAnimation;
    string _parentAnimationId;
    #endregion

    #region Browsable properties
    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public string ParentAnimationId
    {
        get { return this._parentAnimationId; }
        set 
        {
          if (this._parentAnimationId != value)
          {
            this._parentAnimationId = value;
            if (this._parentAnimationId != null)
            {
              this._parentAnimation = this._ParentProject.ParentSolution.FindAnimation(this._parentAnimationId);
              if (this._parentAnimation == null)
              {
                MessageBox.Show("Animation not found");
              }
            }
            else
            {
              this._parentAnimation = null;
            }
            this.OnAttributeChanged();
            this.OnParentAnimationChanged();
          }
        }
    }


    [BrowsableAttribute(true)]
    [DefaultValueAttribute(DEF_NAME)]
    [CategoryAttribute(DEF_CATEGORY)]
    public string Name
    {
      get { return this._Name; }
      set
      {
        if (this._Name != value)
        {
          this._Name = value;
          this.OnAttributeChanged();
          this.OnNameChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public int FrameCount
    {
      get { return this._Frames.Count; }
    }

    [BrowsableAttribute(true)]
    [DefaultValueAttribute(DEF_FPS)]
    [CategoryAttribute(DEF_CATEGORY)]
    public int FramesPerSecond
    {
      get { return this._FramesPerSecond; }
      set
      {
        if (this._FramesPerSecond != value)
        {
          this._FramesPerSecond = value;
          this.OnAttributeChanged();
          this.OnFramesPerSecondChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [DefaultValueAttribute(RENDER_FRAME_NRS_DEF)]
    [CategoryAttribute(DEF_CATEGORY)]
    public bool RenderFrameNrs
    {
      get
      {
        return this._RenderFrameNrs;
      }
      set
      {
        if (this._RenderFrameNrs != value)
        {
          this._RenderFrameNrs = value;
          this.OnAttributeChanged();
          this.OnRenderFrameNrsChanged();
        }
      }
    }
    [BrowsableAttribute(true)]
    [DefaultValueAttribute(DEF_NAME)]
    [CategoryAttribute(DEF_CATEGORY)]
    public Point Offset
    {
      get { return this._Offset; }
      set
      {
        if (this._Offset != value)
        {
          this._Offset = value;
          this.OnAttributeChanged();
        }
      }
    }
    #endregion

    #region Non browsable properties
    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public Animation ParentAnimation
    {
      get 
      {
        if (this._parentAnimation == null &&
            this._parentAnimationId != null)
        {
           this._parentAnimation = this._ParentProject.ParentSolution.FindAnimation(this._parentAnimationId);
        }

        return this._parentAnimation;
      }
    }

    [BrowsableAttribute(false)]
    public List<ColisionAreaBase> ColisionZones
    {
        get { return this._ColisionZones; }
        set { this._ColisionZones = value; }
    }

    [BrowsableAttribute(false)]
    public bool Changed
    {
      get 
      {
        if (this._Changed)
          return true;

        foreach (AnimationFrame frame in this.Frames)
        {
          if (frame.Changed)
            return true;
        }
        return false;
      }
      set 
      { 
        this._Changed = value;
        foreach (AnimationFrame frame in this.Frames)
        {
          frame.Changed = false;
        }
      }
    }

    [BrowsableAttribute(false)]
    public Project ParentProject
    {
      get { return this._ParentProject; }
      set 
      {
        if (this._ParentProject != value)
        {
          this._ParentProject = value;
          this.Changed = true;
        }
      }
    }

    [BrowsableAttribute(false)]
    public Grid Grid
    {
      get { return this._Grid; }
      set
      {
        if (this._Grid != value)
        {
          this._Grid = value;
          this.Changed = true;
        }
      }
    }
    [BrowsableAttribute(false)]
    public List<AnimationFrame> Frames
    {
      get { return this._Frames; }
      private set 
      {
        if (this._Frames != value)
        {
          this._Frames = value;
          this.Changed = true;
        }
      }
    }

    [BrowsableAttribute(false)]
    public bool HasFrames
    {
      get
      {
        return (this._Frames.Count > 0);
      }
    }

    [BrowsableAttribute(false)]
    public AnimationFrame LastFrame
    {
      get
      {
        if (this._Frames.Count == 0)
          return null;

        return (this._Frames[this._Frames.Count - 1]);
      }
    }


    #endregion

    #region Class constructors and overloads
    /// <summary>
    /// 
    /// </summary>
    public Animation()
    {
      this.Frames = new List<AnimationFrame>();
      this.FramesPerSecond = DEF_FPS;
      this.Name = DEF_NAME;
      this.RenderFrameNrs = RENDER_FRAME_NRS_DEF;
      this.Offset = new Point(0, 0);
      this.Changed = false;
      this.ColisionZones = new List<ColisionAreaBase>();
      this.Grid = new Grid();
    }

    public override string ToString()
    {
      return this.Name;
    }
    #endregion

    #region Frame management methods
    /// <summary>
    /// 
    /// </summary>
    public AnimationFrame AddFrame(Rectangle rect)
    {
      AnimationFrame animFrame = new AnimationFrame(rect, this.Frames.Count + 1, this);
      this.AddFrame(animFrame);
      return animFrame;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddFrame(AnimationFrame animFrame)
    {
      animFrame.FrameAttributeChanged += new EventHandler(AnimationFrame_FrameAttributeChanged);
      animFrame.ParentAnimation = this;
      this.Frames.Add(animFrame);
      this.RenumberFrames();
      if (this.FrameAdded != null)
      {
        this.FrameAdded(this, animFrame);
      }
      this.Changed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemoveFrame(AnimationFrame frame)
    {
      this.Frames.Remove(frame);
      this.RenumberFrames();
      this.Changed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void RenumberFrames()
    {
      int frNr = 0;
      foreach (AnimationFrame frame in this.Frames)
      {
        frNr++;
        frame.SetNumber(frNr);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void MoveFrameUp(AnimationFrame frame)
    {
      if (frame.FrameNr - 2 < 0)
        return;

      this._Frames.Remove(frame);
      this._Frames.Insert(frame.FrameNr - 2, frame);
      this.RenumberFrames();
      this.Changed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveFrameDown(AnimationFrame frame)
    {
      this._Frames.Remove(frame);
      this._Frames.Insert(frame.FrameNr, frame);
      this.RenumberFrames();
      this.Changed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool ContainsFrame(AnimationFrame frame)
    {
      foreach (AnimationFrame frm in this.Frames)
      {
        if (frame == frm)
          return true;
      }
      return false;
    }

   

    /// <summary>
    /// 
    /// </summary>
    public AnimationFrame GetFrame(int nr)
    {
      if (nr > this.Frames.Count || nr <= 0)
      {
        return null;
      }

      return this.Frames[nr - 1];
    }

    /// <summary>
    /// 
    /// </summary>
    public int GetFrameNr(AnimationFrame frame)
    {
      if (frame == null)
        return -1;

      for (int i = 0; i < this.Frames.Count; i++)
      {
        if (this.Frames[i] == frame)
          return i + 1;
      }

      return -1;
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemoveAllFrames()
    {
      foreach (AnimationFrame frame in this.Frames)
      {
        this.RemoveFrame(frame);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void AnimationFrame_FrameAttributeChanged(object sender, EventArgs e)
    {
      this.OnFrameAttributeChanged((AnimationFrame)sender);
    }
    #endregion 

    #region Event launching
    /// <summary>
    /// 
    /// </summary>
    private void OnFramesPerSecondChanged()
    {
      if (this.FramesPerSecondChanged != null)
      {
        this.FramesPerSecondChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnRenderFrameNrsChanged()
    {
      if (this.RenderFrameNrsChanged != null)
      {
        this.RenderFrameNrsChanged(this, new EventArgs());
      }
    }

         
    /// <summary>
    /// 
    /// </summary>
    private void OnAttributeChanged()
    {
      this.Changed = true;
      if (this.AttributeChanged != null)
      {
        this.AttributeChanged(this, new EventArgs());
      }
    }

     /// <summary>
    /// 
    /// </summary>
    private void OnParentAnimationChanged()
    {
      this.Changed = true;
      if (this.ParentAnimationChanged != null)
      {
        this.ParentAnimationChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnFrameAttributeChanged(AnimationFrame animationFrame)
    {
      if (this.FrameAttributeChanged != null)
      {
        this.FrameAttributeChanged(this, animationFrame);
      }
    }
    

    
    /// <summary>
    /// 
    /// </summary>
    private void OnAnimationChanged()
    {
      if (this.AnimationChanged != null)
      {
        this.AnimationChanged(this, new EventArgs());
      }
    }
    #endregion

    #region Save/Load methods
    /// <summary>
    /// 
    /// </summary>
    public XmlElement CreateXmlElement(XmlDocument xmlDoc, bool framesByIndex)
    {
      XmlElement elemAnim = xmlDoc.CreateElement("Sprite");
      elemAnim.SetAttribute("Id", this.Name);
      elemAnim.SetAttribute("Fps", this.FramesPerSecond.ToString());
      elemAnim.SetAttribute("OffsetX", this.Offset.X.ToString());
      elemAnim.SetAttribute("OffsetY", this.Offset.Y.ToString());
      if (this.ParentProject.OutputSprite != null)
      {
        elemAnim.SetAttribute("Width", this.ParentProject.OutputSprite.Width.ToString());
        elemAnim.SetAttribute("Height", this.ParentProject.OutputSprite.Height.ToString());
      }
      if (this.Frames.Count > 0)
      {
          elemAnim.SetAttribute("Width", this.Frames[0].Rectangle.Width.ToString());
          elemAnim.SetAttribute("Height", this.Frames[0].Rectangle.Height.ToString());
      }

      elemAnim.SetAttribute("ParentAnimationId", this._parentAnimationId);

      XmlElement elemFrames = xmlDoc.CreateElement("Frames");
      elemAnim.AppendChild(elemFrames);

      foreach (AnimationFrame animationFrame in this.Frames)
      {
        elemFrames.AppendChild(animationFrame.ToXml(xmlDoc));
      }

      XmlElement elemColAreas = xmlDoc.CreateElement("ColisionZones");
      elemAnim.AppendChild(elemColAreas);
      foreach (ColisionAreaBase colArea in this.ColisionZones)
      {
        elemColAreas.AppendChild(colArea.ToXml(xmlDoc));
      }

      elemAnim.AppendChild(this.Grid.ToXml(xmlDoc));
      this.Changed = false;

      return elemAnim;
    }

    /// <summary>
    /// 
    /// </summary>
    public static Animation CreateFromXml(XmlElement elemAnim, Project parentProject)
    {
      Animation animation = new Animation();
      animation.ParentProject = parentProject;
      animation.Name = elemAnim.Attributes["Id"].Value;
      animation.FramesPerSecond = XmlHelper.GetAttribute(elemAnim, "Fps", 12);
      int offsetX = XmlHelper.GetAttribute(elemAnim, "OffsetX", 0);
      int offsetY = XmlHelper.GetAttribute(elemAnim, "OffsetY", 0);
      animation.Offset = new Point(offsetX, offsetY);
      animation._parentAnimationId = XmlHelper.GetAttribute(elemAnim, "ParentAnimationId", null);

      animation.Frames = new List<AnimationFrame>();
      XmlNodeList frameNodes = elemAnim.SelectNodes("Frames/Frame");
      foreach (XmlElement elemFrame in frameNodes)
      {
       animation.Frames.Add(AnimationFrame.FromXml(elemFrame, animation));
      }
      animation.RenumberFrames();

      animation.ColisionZones = new List<ColisionAreaBase>();
      XmlNodeList elemColAreas = elemAnim.SelectNodes("ColisionZones/BoundingBox");
      foreach (XmlElement elemCol in elemColAreas)
      {
        animation.ColisionZones.Add(BoundingBox.FromXml(elemCol));
      }

      elemColAreas = elemAnim.SelectNodes("ColisionZones/BoundingSphere");
      foreach (XmlElement elemCol in elemColAreas)
      {
        animation.ColisionZones.Add(BoundingSphere.FromXml(elemCol));
      }

      animation.Grid.InitFromXml(elemAnim);
      animation.Changed = false;
      return animation;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public Animation Clone()
    {
      Animation animClone = new Animation();
      animClone.FramesPerSecond = this.FramesPerSecond;
      animClone.Name = this.Name;
      animClone.Offset = new Point(this.Offset.X, this.Offset.Y);
      animClone.ParentProject = this.ParentProject;
      animClone.RenderFrameNrs = this.RenderFrameNrs;
      foreach (AnimationFrame frame in this.Frames)
      {
        animClone.Frames.Add(frame.Clone());
      }

      foreach (ColisionAreaBase col in this.ColisionZones)
      {
        animClone.ColisionZones.Add((ColisionAreaBase)col.Clone());
      }
      return animClone;
    }
    #region Event lauching
    /// <summary>
    /// 
    /// </summary>
    void OnNameChanged()
    {
      if (this.NameChanged != null)
      {
        this.NameChanged(this, new EventArgs());
      }
    }
    #endregion
  }
}
