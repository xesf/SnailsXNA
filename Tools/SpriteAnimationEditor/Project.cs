using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.Xml;

namespace SpriteAnimationEditor
{
  [DefaultPropertyAttribute("Name")]
  class Project
  {
    #region Consts
    const string DEF_NAME = "unnamed project";
    const string DEF_CATEGORY = "Project";
    const string FILENAME_DEF_NAME = "noname.saProj";
    const string RESOURCE_ID_DEF_NAME = "imageId";
    const int DEF_FRAMES_PER_COLUMN = 6;
    const string INGAME_DEF_NAME = "animation.sa";
    const string SPRITE_DEF_NAME = "animation.png";
    const string DEF_SPRITE_BACK_COLOR = "Gray";
    const string DEF_FRAME_SIZE = "-1,-1";
    const string DEF_FRAME_NR_COLOR = "White";
    const string DEF_FRAME_RECT_COLOR = "White";
    #endregion

    #region Events
    public delegate void AnimationAddedHandler(object sender, Animation frame);

    public event AnimationAddedHandler AnimationAdded;
    public event EventHandler NameChanged;
    public event EventHandler FilenameChanged;
    public event EventHandler SpriteBackColorChanged;
    public event EventHandler FramesPerColumnChanged;
    public event EventHandler OutputSpriteChanged;
    public event EventHandler FrameNrsColorChanged;
    public event EventHandler FrameRectangleColorChanged;
    public event EventHandler AttributeChanged;
    #endregion

    #region Variables
    string _Name;
    string _ImageId;
    List<Animation> _AnimationList;
    Solution _ParentSolution;
    string _Filename;
    int _FramesPerColumn;
    Color _SpriteBackColor;
    bool _Changed;
    bool _Loaded;
    Image _OutputSprite;
    Color _FrameNrsColor;
    Color _FrameRectangleColor;
    string _SpriteFilename;
    #endregion

    #region Non browsable Properties

    [BrowsableAttribute(false)]
    public string ProjectFolder
    {
       
      get 
      { 
        if (string.IsNullOrEmpty(Path.GetDirectoryName(this.Filename)))
          return null;

        return Path.GetDirectoryName(this.Filename); 
      }
    }

    [BrowsableAttribute(false)]
    public List<ResolutionMapper> ResolutionMappers { get; private set;}

    [BrowsableAttribute(false)]
    public bool WithSprite
    { get { return (this._OutputSprite != null); } }

    [BrowsableAttribute(false)]
    public List<Animation> AnimationList
    {
      get { return this._AnimationList; }
      set 
      { 
        this._AnimationList = value;
        this.Changed = true;
      }
    }

    [BrowsableAttribute(false)]
    public Solution ParentSolution
    {
      get { return this._ParentSolution; }
      set 
      { 
        this._ParentSolution = value;
        this.Changed = true;
      }
    }


    [BrowsableAttribute(false)]
    public bool Changed
    {
      get 
      {
        if (this._Changed)
          return true;

        foreach (Animation animation in this.AnimationList)
        {
          if (animation.Changed)
            return true;
        }
        return false; 
      }
      set 
      { 
        this._Changed = value;
        foreach (Animation animation in this.AnimationList)
        {
          animation.Changed = false;
        }
      }
    }

    [BrowsableAttribute(false)]
    public bool Loaded
    {
      get { return this._Loaded; }
      set { this._Loaded = value; }
    }

    [BrowsableAttribute(false)]
    public Image OutputSprite
    {
      get { return this._OutputSprite; }
      set 
      { 
        if (this._OutputSprite != value)
        {
          this._OutputSprite = value;
          this.OnOutputSpriteChanged();
        }
      }
    }
    #endregion

    #region Browsable Properties
   
    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    [DefaultValueAttribute(typeof(Color), DEF_FRAME_RECT_COLOR)]
    public Color FrameRectangleColor
    {
      get
      {
        return this._FrameRectangleColor;
      }
      set
      {
        if (this._FrameRectangleColor != value)
        {
          this._FrameRectangleColor = value;
          this.Changed = true;
          this.OnFrameRectangleColorChanged();
          this.OnAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    [DefaultValueAttribute(typeof(Color), DEF_FRAME_NR_COLOR)]
    public Color FrameNumbersColor
    {
      get { return this._FrameNrsColor; }
      set
      {
        if (this._FrameNrsColor != value)
        {
          this._FrameNrsColor = value;
          this.Changed = true;
          this.OnFrameNrsColorChanged();
          this.OnAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [DefaultValueAttribute(FILENAME_DEF_NAME)]
    [CategoryAttribute(DEF_CATEGORY)]
    public string Filename
    {
      get { return this._Filename; }
      private set
      {
        if (this._Filename != value)
        {
          this._Filename = value;
          this.Changed = true;
          this.OnFilenameChanged();
          this.OnAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public string ImageId
    {
      get { return this._ImageId; }
      set
      {
        if (this._ImageId != value)
        {
          this._ImageId = value;
          this.Changed = true;
          this.OnAttributeChanged();
        }
      }
    }
   
    [BrowsableAttribute(true)]
    [DefaultValueAttribute(DEF_FRAMES_PER_COLUMN)]
    [CategoryAttribute(DEF_CATEGORY)]
    public int FramesPerColumn
    {
      get
      {
        return this._FramesPerColumn;
      }
      set
      {
        if (this._FramesPerColumn != value)
        {
          this._FramesPerColumn = value;
          this.Changed = true;
          this.OnFramesPerColumnChanged();
          this.OnAttributeChanged();
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
          this.Changed = true;
          this.OnNameChanged();
          this.OnAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    [DefaultValueAttribute(typeof(Color), DEF_SPRITE_BACK_COLOR)]
    public Color SpriteBackColor
    {
      get
      {
        return this._SpriteBackColor;
      }
      set
      {
        if (this._SpriteBackColor != value)
        {
          this._SpriteBackColor = value;
          this.Changed = true;
          this.OnSpriteBackColorChanged();
          this.OnAttributeChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    public string SpriteFilename
    {
      get
      {
        return this._SpriteFilename;
      }
      set
      {
        if (this._SpriteFilename != value)
        {
          this._SpriteFilename = value;
          this.Changed = true;
          this.OnAttributeChanged();
        }
      }
    }
    #endregion

    #region Constructors and overrides
    /// <summary>
    /// 
    /// </summary>
    public Project()
    {
      this.Initialize(null, DEF_NAME, FILENAME_DEF_NAME);
    }

    /// <summary>
    /// 
    /// </summary>
    public Project(Solution solution, string name, string filename)
    {
      this.Initialize(solution, name, filename);
    }

    /// <summary>
    /// 
    /// </summary>
    public Project(Solution solution, string filename)
    {
      this.Initialize(solution, DEF_NAME, filename);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Initialize(Solution solution, string name, string filename)
    {
      this.AnimationList = new List<Animation>();
      this.ResolutionMappers = new List<ResolutionMapper>();
      this.ParentSolution = solution;
      this.Name = name;
      this.Filename = filename;
      this.Loaded = false;
      this.Changed = false;
      this.ImageId = RESOURCE_ID_DEF_NAME;
      this.FramesPerColumn = DEF_FRAMES_PER_COLUMN;
      this.SpriteBackColor = Color.FromName(DEF_SPRITE_BACK_COLOR);
      this.FrameNumbersColor = Color.FromName(DEF_FRAME_NR_COLOR);
      this.FrameRectangleColor = Color.FromName(DEF_FRAME_RECT_COLOR);
    }

    /// <summary>
    /// 
    /// </summary>
    public override string ToString()
    {
      return this.Name;
    }
    #endregion

    #region Open/save methods
    /// <summary>
    /// 
    /// </summary>
    public void Save(string filename)
    {
      if (string.IsNullOrEmpty(filename))
        throw new ApplicationException("Project filename cannot be null.");

      this.Filename = filename;
      // New format
      XmlDocument xmlDoc = new XmlDocument();
      XmlElement elemRoot = xmlDoc.CreateElement("SpriteSet");
      xmlDoc.AppendChild(elemRoot);

      elemRoot.SetAttribute("Name", this.Name);
      elemRoot.SetAttribute("ImageId", this.ImageId);
      string f = Goodies.RelativePath(Path.GetDirectoryName(this.Filename), this.SpriteFilename);
      elemRoot.SetAttribute("Image", f);
      elemRoot.SetAttribute("SpriteBackColor", this.SpriteBackColor.ToArgb().ToString());
      elemRoot.SetAttribute("FrameNumbersColor", this.FrameNumbersColor.ToArgb().ToString());
      elemRoot.SetAttribute("FrameRectangleColor", this.FrameRectangleColor.ToArgb().ToString());
      // Animations
      foreach (Animation animation in this.AnimationList)
      {
        XmlElement elemAnim = animation.CreateXmlElement(xmlDoc, true);
        elemRoot.AppendChild(elemAnim);
      }

      XmlElement elemMappers = xmlDoc.CreateElement("ResolutionMapping");
      elemRoot.AppendChild(elemMappers);
      // Resolution mappers
      foreach (ResolutionMapper mapper in this.ResolutionMappers)
      {
        XmlElement elemMapper = mapper.CreateXmlElement(xmlDoc);
        elemMappers.AppendChild(elemMapper);
      }

      xmlDoc.Save(this.Filename);

      this.SaveResolutionMappers();
      this.Changed = false;
    }

    /// <summary>
    /// 
    /// </summary>
    private void SaveResolutionMappers()
    {
      foreach (ResolutionMapper mapper in this.ResolutionMappers)
      {
        Project proj = this.Clone();
        proj.Filename = mapper.OutputFile;
        proj.Name = this.Name;
        proj.OutputSprite = null;
        if (string.IsNullOrEmpty(mapper.ImageFilename) == false)
        {
          proj.OutputSprite = Bitmap.FromFile(mapper.ImageFilename);
        }

        proj.SpriteFilename = mapper.ImageFilename;

        if (proj.WithSprite && this.WithSprite)
        {
          double factorX = (double)proj.OutputSprite.Width / (double)this.OutputSprite.Width;
          double factorY = (double)proj.OutputSprite.Height / (double)this.OutputSprite.Height;
          foreach (Animation anim in proj.AnimationList)
          {
            for (int i = 0; i < anim.ColisionZones.Count; i++)
            {
              anim.ColisionZones[i] = anim.ColisionZones[i].Scale(factorX, factorX);
            }

            for (int i = 0; i < anim.Frames.Count; i++)
            {
              anim.Frames[i] = anim.Frames[i].Scale(factorX, factorX);
            }
          }
          proj.Save(proj.Filename);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reload()
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.Load(this.Filename);
      XmlElement rootElem = (XmlElement)xmlDoc.SelectSingleNode("SpriteSet");
      this.Name = XmlHelper.GetAttribute(rootElem, "Name", "");
      this.ImageId = XmlHelper.GetAttribute(rootElem, "ImageId", "");
      string saveCurDir = Directory.GetCurrentDirectory();
      this.SpriteFilename = XmlHelper.GetAttribute(rootElem, "Image", null);
      if(!string.IsNullOrEmpty(this.SpriteFilename))
      {
        Directory.SetCurrentDirectory(Path.GetDirectoryName(this.Filename));
        this.SpriteFilename = Path.GetFullPath(this.SpriteFilename);
        Directory.SetCurrentDirectory(saveCurDir);
      }

      this.FramesPerColumn = XmlHelper.GetAttribute(rootElem, "FramesPerColumn", DEF_FRAMES_PER_COLUMN);
      this.SpriteBackColor = XmlHelper.GetAttribute(rootElem, "SpriteBackColor", Color.Gray);
      this.FrameNumbersColor = XmlHelper.GetAttribute(rootElem, "FrameNumbersColor", Color.FromName(DEF_FRAME_NR_COLOR));
      this.FrameRectangleColor = XmlHelper.GetAttribute(rootElem, "FrameRectangleColor", Color.FromName(DEF_FRAME_RECT_COLOR));

      // Load animations
      this.AnimationList = new List<Animation>();
      XmlNodeList animationNodes = rootElem.SelectNodes("Sprite");
      foreach (XmlElement elemAnimation in animationNodes)
      {
        Animation animation = Animation.CreateFromXml(elemAnimation, this);
        animation.ParentProject = this;
        this.AnimationList.Add(animation);
      }

      // Load Resolution mappers
      this.ResolutionMappers = new List<ResolutionMapper>();
      XmlNodeList resMappersNodes = rootElem.SelectNodes("ResolutionMapping//Mapper");
      foreach (XmlElement elemResMapper in resMappersNodes)
      {
        ResolutionMapper mapper = ResolutionMapper.CreateFromXml(elemResMapper, this);
        this.ResolutionMappers.Add(mapper);
      }

      this.RefreshOutputSprite();
      this.Changed = false;
    }

    #endregion

    #region Animation methods
    /// <summary>
    /// 
    /// </summary>
    public void AddAnimation(Animation animation)
    {
      this.AnimationList.Add(animation);
      animation.ParentProject = this;
      this.OnAnimationAdded(animation);
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemoveAnimation(Animation animation)
    {
      this.AnimationList.Remove(animation);
      animation.ParentProject = null;
      this.Changed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public AnimationFrame AddFrameToAnimation(Animation animation, Rectangle rect)
    {
      if (this.AnimationList.Contains(animation) == false)
        return null;
      AnimationFrame animFrame = animation.AddFrame(rect);
      this.Changed = true;
      return animFrame;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddFrameToAnimation(Animation animation, AnimationFrame frame)
    {
      if (this.AnimationList.Contains(animation) == false)
        return;
       animation.AddFrame(frame);
      this.Changed = true;
    }
    /// <summary>
    /// 
    /// </summary>
    public Animation GetAnimationByName(string name)
    {
      foreach (Animation anim in this._AnimationList)
      {
        if (anim.Name == name)
        {
          return anim;
        }
      }
      return null;
    }
    #endregion

    #region Mapper methods
    /// <summary>
    /// 
    /// </summary>
    public void AddMapper(ResolutionMapper mapper)
    {
      this.ResolutionMappers.Add(mapper);
      this.Changed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetMapper(int idx, ResolutionMapper mapper)
    {
      this.ResolutionMappers[idx] = mapper;
      this.Changed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemoveMapper(ResolutionMapper mapper)
    {
      this.ResolutionMappers.Remove(mapper);
      this.Changed = true;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public Project Clone()
    {
      Project proj = new Project();
      proj.Filename = this.Filename;
      proj.FrameNumbersColor = this.FrameNumbersColor;
      proj.FrameRectangleColor = this.FrameRectangleColor;
      proj.FramesPerColumn = this.FramesPerColumn;
      proj.ImageId = this.ImageId;
      proj.Name = this.Name;
      proj.SpriteFilename = this.SpriteFilename;
      if (string.IsNullOrEmpty(this.SpriteFilename) == false)
      {
        proj.OutputSprite = Bitmap.FromFile(this.SpriteFilename);
      }
      proj.ParentSolution = this.ParentSolution;
      proj.SpriteBackColor = this.SpriteBackColor;

      foreach (Animation anim in this.AnimationList)
      {
        proj.AnimationList.Add(anim.Clone());
      }

      return proj;
    }
    #region Output sprite methods

    /// <summary>
    /// 
    /// </summary>
    public void SetSprite(string filename)
    {
 /*     // Copia para a folder do projecto
      string path = Path.GetDirectoryName(this.Filename);
      string pathFile = Path.GetDirectoryName(filename);
      if (string.Compare(path, pathFile, true) != 0)
      {
        File.Copy(filename, Path.Combine(path, Path.GetFileName(filename)), true);
      }
      */
      this.OutputSprite = Image.FromFile(filename);
      this.SpriteFilename = filename;
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshOutputSprite()
    {
      if (string.IsNullOrEmpty(this.SpriteFilename))
        return;

      if (File.Exists(this.SpriteFilename) == false)
      {
        throw new ApplicationException("File not found! " + this.SpriteFilename);
      }

      System.IO.FileStream fs = new FileStream(this.SpriteFilename, FileMode.Open);

      this.OutputSprite = System.Drawing.Image.FromStream(fs);

      fs.Close();

//      this.OutputSprite = Image.FromFile(this.SpriteFilename);
    }

    /// <summary>
    /// 
    /// </summary>
    public void CreateSpriteFromFiles(List<string> filesList, int framesPerColumn)
    {
      if (filesList.Count == 0)
        return;
      Goodies.CheckFrameSizes(filesList);
      Image firstFrame = Image.FromFile(filesList[0]);
      Bitmap bmp = new Bitmap(firstFrame.Width * framesPerColumn, 
                              firstFrame.Height * ((filesList.Count / framesPerColumn) + 
                                      (filesList.Count % framesPerColumn == 0? 0 : 1)) ,
                              PixelFormat.Format32bppArgb);
      Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
      BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
      IntPtr dataPtr = bmpData.Scan0;
      byte[] bmpBytes = new byte[bmpData.Stride * bmpData.Height];
      int col = 0, row = 0;

      for (int i = 0; i < filesList.Count; i++)
      {
        Bitmap frameImg = (Bitmap)Bitmap.FromFile(filesList[i]);
        Rectangle frameRect = new Rectangle(0, 0, frameImg.Width, frameImg.Height);
        BitmapData frameData = frameImg.LockBits(frameRect, ImageLockMode.ReadWrite, frameImg.PixelFormat);
        IntPtr frameDataPtr = frameData.Scan0;
        byte[] frameBytes = new byte[frameData.Stride * frameData.Height];
        System.Runtime.InteropServices.Marshal.Copy(frameDataPtr, frameBytes, 0, frameBytes.Length);

        for (int j = 0; j < frameData.Height; j++)
        {
          Buffer.BlockCopy(frameBytes, j * frameData.Stride,
                           bmpBytes, (framesPerColumn * frameData.Stride * row * frameData.Height) + (col * frameData.Stride) + (j * frameData.Stride * framesPerColumn),
                           frameData.Stride);
        }

        frameImg.UnlockBits(frameData);
        col++;
        if (col >= framesPerColumn)
        {
          col = 0;
          row++;
        }
      }

      System.Runtime.InteropServices.Marshal.Copy(bmpBytes, 0, dataPtr, bmpBytes.Length);
      bmp.UnlockBits(bmpData);
      
      this.OutputSprite = bmp;
      string path = Path.GetDirectoryName(this.Filename);
      string filename = Path.Combine(path, this.Name + ".png");
      this.OutputSprite.Save(filename, ImageFormat.Png);
      this.SpriteFilename = filename;
    }

    #endregion

    #region Event lauching
    /// <summary>
    /// 
    /// </summary>
    private void OnFrameNrsColorChanged()
    {
      if (this.FrameNrsColorChanged != null)
      {
        this.FrameNrsColorChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnFrameRectangleColorChanged()
    {
      if (this.FrameRectangleColorChanged != null)
      {
        this.FrameRectangleColorChanged(this, new EventArgs());
      }
    }
  
    
    /// <summary>
    /// 
    /// </summary>
    private void OnAttributeChanged()
    {
      if (this.AttributeChanged != null)
      {
        this.AttributeChanged(this, new EventArgs());
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    void OnAnimationAdded(Animation animation)
    {
      if (this.AnimationAdded != null)
      {
        this.AnimationAdded(this, animation);
      }
    }

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

    /// <summary>
    /// 
    /// </summary>
    private void OnFilenameChanged()
    {
      if (this.FilenameChanged != null)
      {
        this.FilenameChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnSpriteBackColorChanged()
    {
      if (this.SpriteBackColorChanged != null)
      {
        this.SpriteBackColorChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnFramesPerColumnChanged()
    {
      if (this.FramesPerColumnChanged != null)
      {
        this.FramesPerColumnChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnOutputSpriteChanged()
    {
      if (this.OutputSpriteChanged != null)
      {
        this.OutputSpriteChanged(this, new EventArgs());
      }
    }

    #endregion  
  }
}
