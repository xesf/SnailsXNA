using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Drawing.Imaging;

namespace SpriteAnimationEditor
{
  class Frame
  {
    #region Variables
    string _Filename;
    bool _HorizontalFlip;
    Image _Image;
    Image _InvertedImage;
    Project _ParentProject;
    Rectangle _Rectangle;
    #endregion

    #region Properties
    public string Filename
    {
      get { return this._Filename; }
      private set { this._Filename = value; }
    }

    public Project ParentProject
    {
      get { return this._ParentProject; }
      set { this._ParentProject = value; }
    }

    public Image Image
    {
      get 
      {
        if (this._HorizontalFlip == true)
          return this._InvertedImage;

        return this._Image; 
      }
      private set 
      { 
        this._Image = value;
        this.GenerateImage();
      }
    }

    public int Width
    {
      get { return this._Image.Width; }
    }

    public int Height
    {
      get { return this._Image.Height; }
    }

    public Rectangle Rectangle
    {
      get { return this._Rectangle; }
      set { this._Rectangle = value; }
    }

    public bool HorizontalFlip
    {
      get { return this._HorizontalFlip; }
      set 
      { 
        this._HorizontalFlip = value;
        this.GenerateImage();
      }
    }
    #endregion

    #region Class constructors and overloads
    /// <summary>
    /// 
    /// </summary>
    private Frame()
    {
      this._HorizontalFlip = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public Frame(Rectangle rectangle, Project project)
    {
      this._Rectangle = rectangle;
      this._ParentProject = project;
    }

    /// <summary>
    /// 
    /// </summary>
    public override string ToString()
    {
      return Path.GetFileName(this.Filename);
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public static Frame CreateFromImage(string filename)
    {
      Frame frame = new Frame();
      frame.Image = Image.FromFile(filename);
      frame._Filename = filename;
      return frame;
    }

    /// <summary>
    /// 
    /// </summary>
    public static Frame CreateFromXml(XmlElement elem, Project parentProject)
    {
      // Only relative paths to the project are stored in the file
      // Set current directory to the directory project
      Directory.SetCurrentDirectory(Path.GetDirectoryName(parentProject.Filename));
      Frame frame = new Frame();
      string filename = elem.Attributes["Image"].Value;
      // Set current directory using the relative project path
      if (!string.IsNullOrEmpty(filename))
        Directory.SetCurrentDirectory(Path.GetDirectoryName(filename));
      // Make absolute frame path
      frame.Filename = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(filename));
      frame.Image = Image.FromFile(frame.Filename);
      frame.HorizontalFlip = Convert.ToBoolean(elem.Attributes["HorizontalFlip"].Value);
      frame.Rectangle = new Rectangle(Convert.ToInt32(elem.Attributes["Left"].Value),
        Convert.ToInt32(elem.Attributes["Top"].Value),
        Convert.ToInt32(elem.Attributes["Width"].Value),
        Convert.ToInt32(elem.Attributes["Height"].Value));
      frame.ParentProject = parentProject;
      return frame;
    }

    /// <summary>
    /// 
    /// </summary>
    public XmlElement CreateXmlElement(XmlDocument xmlDoc)
    {
      XmlElement elemFrame = xmlDoc.CreateElement("Frame");
      // Store relative project path
      string filename = Goodies.RelativePath(Path.GetDirectoryName(this.ParentProject.Filename), this.Filename);
      elemFrame.SetAttribute("Image", filename);
      elemFrame.SetAttribute("HorizontalFlip", this.HorizontalFlip.ToString());
      elemFrame.SetAttribute("Left", this.Rectangle.Left.ToString());
      elemFrame.SetAttribute("Top", this.Rectangle.Top.ToString());
      elemFrame.SetAttribute("Width", this.Rectangle.Width.ToString());
      elemFrame.SetAttribute("Height", this.Rectangle.Height.ToString());
      return elemFrame;
    }

    /// <summary>
    /// 
    /// </summary>
    void GenerateImage()
    {
      if (this.HorizontalFlip == false)
      {
        this._InvertedImage = this._Image;
        return;
      }

      Bitmap frameImg = (Bitmap)this._Image;
      Rectangle frameRect = new Rectangle(0, 0, frameImg.Width, frameImg.Height);
      BitmapData frameData = frameImg.LockBits(frameRect, ImageLockMode.ReadWrite, frameImg.PixelFormat);
      IntPtr frameDataPtr = frameData.Scan0;
      byte[] frameBytes = new byte[frameData.Stride * frameData.Height];
      System.Runtime.InteropServices.Marshal.Copy(frameDataPtr, frameBytes, 0, frameBytes.Length);

      Bitmap bmp = new Bitmap(this._Image.Width, this._Image.Height, frameImg.PixelFormat);
      Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
      BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, frameImg.PixelFormat);
      byte[] bmpBytes = new byte[bmpData.Stride * bmpData.Height];
      
      for (int j = 0; j < frameData.Height; j++)
      {
        for (int k = 0; k < frameData.Stride; k += 4)
        {
          Buffer.BlockCopy(frameBytes, (j * frameData.Stride + (frameData.Stride - k - 4)),
                           bmpBytes,   (j * frameData.Stride) + k,
                           4);
        }
      }

      System.Runtime.InteropServices.Marshal.Copy(bmpBytes, 0, bmpData.Scan0, bmpBytes.Length);
      bmp.UnlockBits(bmpData);
      frameImg.UnlockBits(bmpData);
      this._InvertedImage = (Image)bmp;
    }
  }
}
