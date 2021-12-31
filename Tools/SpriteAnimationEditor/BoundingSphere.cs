using System;
using System.Drawing;
using System.Xml;

namespace SpriteAnimationEditor
{
  class BoundingSphere : ColisionAreaBase
  {
    #region Variables
    int _Radius;
    int _x, _y;
    #endregion

    #region Properties
    public int Radius
    {
      get { return this._Radius; }
      set { this._Radius = value; }
    }

    public int X
    {
      get { return this._x; }
      set { this._x = value; }
    }

    public int Y
    {
      get { return this._y; }
      set { this._y = value; }

    }
    #endregion

    #region Constructors and overloads
    /// <summary>
    /// 
    /// </summary>
    public BoundingSphere()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public BoundingSphere(int x, int y, int radius)
    {
      this.X = x;
      this.Y = y;
      this.Radius = radius;
    }
    /// <summary>
    /// 
    /// </summary>
    public override ColisionAreaBase Scale(double factorX, double factorY)
    {
      int radius = (int)Math.Round(this.Radius * ((factorX + factorY) / 2) , 0);
      int x = (int)Math.Round(this.X * factorX, 0);
      int y = (int)Math.Round(this.Y * factorY, 0);

      return new BoundingSphere(x, y, radius);
    }

    /// <summary>
    /// 
    /// </summary>
    public static BoundingSphere FromXml(XmlElement elem)
    {
      BoundingSphere bs = new BoundingSphere();
      bs.InitFromXml(elem);
      return bs;
    }


    /// <summary>
    /// 
    /// </summary>
    public override object Clone()
    {
      BoundingSphere bs = new BoundingSphere(this.X, this.Y, this.Radius);
      bs.Description = this.Description;
      return bs;
    }

    /// <summary>
    /// 
    /// </summary>
    public override string ToString()
    {
      return string.Format("[BS :{0},{1} Radius:{2}] {3}", this.X, this.Y, this.Radius, this.Description);
    }

    /// <summary>
    /// 
    /// </summary>
    public override bool IsZero()
    {
      return (this.Radius == 0);

    }

    /// <summary>
    /// 
    /// </summary>
    public override void Draw(Point pt, Graphics e, Color color)
    {
      Pen pen = new Pen(color);
      e.DrawArc(pen, this.X + pt.X - this.Radius, this.Y + pt.Y - this.Radius, this.Radius * 2, this.Radius * 2, 0, 360);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void InitFromXml(System.Xml.XmlElement elem)
    {
      this.X = XmlHelper.GetAttribute(elem, "X", 0);
      this.Y = XmlHelper.GetAttribute(elem, "Y", 0);
      this.Radius = XmlHelper.GetAttribute(elem, "Radius", 0);
      this.Description = XmlHelper.GetAttribute(elem, "Description", null);
    }

    /// <summary>
    /// 
    /// </summary>
    public override System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      System.Xml.XmlElement elem = xmlDoc.CreateElement("BoundingSphere");
      elem.SetAttribute("X", this.X.ToString());
      elem.SetAttribute("Y", this.Y.ToString());
      elem.SetAttribute("Radius", this.Radius.ToString());
      elem.SetAttribute("Description", this.Description);

      return elem;
    }

    #endregion
  }
}
