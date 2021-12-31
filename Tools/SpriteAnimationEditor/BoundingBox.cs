using System;
using System.Drawing;
using System.Xml;

namespace SpriteAnimationEditor
{
  class BoundingBox : ColisionAreaBase
  {
    #region Variables
    Rectangle _Rectangle;
    #endregion

    #region Properties
    public Rectangle Rectangle
    {
      get { return this._Rectangle; }
      set { this._Rectangle = value; }
    }
    #endregion

    #region Constructors and overloads
    /// <summary>
    /// 
    /// </summary>
    public BoundingBox()
    {
      this.Rectangle = new Rectangle(0, 0, 0, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    public BoundingBox(Rectangle rect)
    {
      this.Rectangle = rect;
    }

    /// <summary>
    /// 
    /// </summary>
    public override ColisionAreaBase Scale(double factorX, double factorY)
    {
      int x = (int)Math.Round(this.Rectangle.X * factorX, 0);
      int y = (int)Math.Round(this.Rectangle.Y * factorY, 0);
      int w = (int)Math.Round(this.Rectangle.Width * factorX, 0);
      int h = (int)Math.Round(this.Rectangle.Height * factorY, 0);

      return new BoundingBox(new Rectangle(x, y, w, h));
    }

    /// <summary>
    /// 
    /// </summary>
    public static BoundingBox FromXml(XmlElement elem)
    {
      BoundingBox bb = new BoundingBox();
      bb.InitFromXml(elem);
      return bb;
    }

    /// <summary>
    /// 
    /// </summary>
    public override object Clone()
    {
      BoundingBox bb = new BoundingBox();
      bb.Rectangle = new Rectangle(this.Rectangle.Left, this.Rectangle.Top, this.Rectangle.Width, this.Rectangle.Height);
      bb.Description = this.Description;
      return bb;
    }

    /// <summary>
    /// 
    /// </summary>
    public override string ToString()
    {
      return string.Format("[BB:{0},{1},{2},{3}] {4}", 
        this.Rectangle.Left, this.Rectangle.Top, this.Rectangle.Width, this.Rectangle.Height, this.Description);
    }

    /// <summary>
    /// 
    /// </summary>
    public override bool IsZero()
    {

      return (this.Rectangle.Width == 0 || this.Rectangle.Height == 0);

    }

    /// <summary>
    /// 
    /// </summary>
    public override void Draw(Point pt, Graphics e, Color color)
    {
      Pen pen = new Pen(color);
      e.DrawRectangle(pen, this.Rectangle.Left + pt.X, 
                                this.Rectangle.Top + pt.Y, 
                                this.Rectangle.Width, 
                                this.Rectangle.Height);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void InitFromXml(System.Xml.XmlElement elem)
    {
      int x = XmlHelper.GetAttribute(elem, "Left", 0);
      int y = XmlHelper.GetAttribute(elem, "Top", 0);
      int width = XmlHelper.GetAttribute(elem, "Width", 0);
      int height = XmlHelper.GetAttribute(elem, "Height", 0);
      this.Description = XmlHelper.GetAttribute(elem, "Description", null);
      this.Rectangle = new Rectangle(x, y, width, height);

    }

    /// <summary>
    /// 
    /// </summary>
    public override System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      System.Xml.XmlElement elem = xmlDoc.CreateElement("BoundingBox");
      elem.SetAttribute("Left", this.Rectangle.Left.ToString());
      elem.SetAttribute("Top", this.Rectangle.Top.ToString());
      elem.SetAttribute("Width", this.Rectangle.Width.ToString());
      elem.SetAttribute("Height", this.Rectangle.Height.ToString());
      elem.SetAttribute("Description", this.Description);
      return elem;
    }

    #endregion
  }
}
