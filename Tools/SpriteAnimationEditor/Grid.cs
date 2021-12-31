using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpriteAnimationEditor
{
  public class Grid : IViewState, ICloneable
  {
    #region Variables
    int _Width;
    int _Height;
    int _OffsetX;
    int _OffsetY;
    bool _Visible;
    Color _ForeColor;
    #endregion

    #region Properties
    [BrowsableAttribute(true)]
    public int Width
    {
      get { return this._Width; }
      set { this._Width = value; }
    }
    [BrowsableAttribute(true)]
    public Color ForeColor
    {
      get { return this._ForeColor; }
      set { this._ForeColor = value; }
    }
    [BrowsableAttribute(true)]
    public int Height
    {
      get { return this._Height; }
      set { this._Height = value; }
    }

    [BrowsableAttribute(true)]
    public int OffsetX
    {
      get { return this._OffsetX; }
      set { this._OffsetX = value; }
    }

    [BrowsableAttribute(true)]
    public int OffsetY
    {
      get { return this._OffsetY; }
      set { this._OffsetY = value; }
    }

    [BrowsableAttribute(true)]
    public bool Visible
    {
      get { return this._Visible; }
      set { this._Visible = value; }
    }
    #endregion

    #region Construtors
    /// <summary>
    /// 
    /// </summary>
    public Grid()
    {
      this.Visible = true;
      this.Width = Settings.GRID_DEF_WIDTH;
      this.Height = Settings.GRID_DEF_HEIGHT;
      this.ForeColor = Color.Black;
    }

    /// <summary>
    /// 
    /// </summary>
    public static Grid FromXml(XmlElement elem)
    {
      Grid grid = new Grid();
      grid.InitFromXml((XmlElement) elem.ParentNode);
      return grid;
    }

    /// <summary>
    /// 
    /// </summary>
    public override string ToString()
    {
      return string.Format("{0},{1}", this.Width, this.Height);
    }
    #endregion

    #region Other methods
    /// <summary>
    /// 
    /// </summary>
    public void Paint(Graphics graphics, ZoomFactor zoom, Rectangle rect)
    {
      // Grid
      if (this.Visible)
      {
        // Pixel grid
        Pen pen = new Pen(this.ForeColor);
        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        if (zoom >= ZoomFactor.x8)
        {
          for (int i = 0; i < rect.Width * (int)zoom; i++)
          {
            graphics.DrawLine(pen, i * (int)zoom, 0, i * (int)zoom, rect.Height * (int)zoom);
          }

          for (int i = 0; i < rect.Height * (int)zoom; i++)
          {
            graphics.DrawLine(pen, 0, i * (int)zoom, rect.Width * (int)zoom, i * (int)zoom);
          }
        }

        // cells grids
        pen = new Pen(this.ForeColor);
        pen.DashStyle = DashStyle.Dash;
        if (zoom >= ZoomFactor.x8)
        {
          pen.DashStyle = DashStyle.Solid;
        }
        // Vertical lines
        for (int i = 0; i < rect.Width * (int)zoom; i += this.Width * (int)zoom)
        {
          graphics.DrawLine(pen, i + this.OffsetX, 0,
                                  i + this.OffsetX, rect.Height * (int)zoom);
        }

        for (int i = 0; i < rect.Height * (int)zoom; i += this.Height * (int)zoom)
        {
          graphics.DrawLine(pen, 0, i + this.OffsetY, rect.Width * (int)zoom, i + this.OffsetY);
        }
      }
    }
    #endregion

    #region IViewState Members

    /// <summary>
    /// 
    /// </summary>
    public void InitFromXml(System.Xml.XmlElement elemParent)
    {
      XmlElement elemGrid = (XmlElement)elemParent.SelectSingleNode("Grid");
      if (elemGrid == null)
      {
        return;
      }
      this.Width = XmlHelper.GetAttribute(elemGrid, "Width", Settings.GRID_DEF_WIDTH);
      this.Height = XmlHelper.GetAttribute(elemGrid, "Height", Settings.GRID_DEF_HEIGHT);
      this.OffsetX = XmlHelper.GetAttribute(elemGrid, "OffsetX", 0);
      this.OffsetY = XmlHelper.GetAttribute(elemGrid, "OffsetY", 0);
      this.Visible = XmlHelper.GetAttribute(elemGrid, "Visible", false);
    }

    /// <summary>
    /// 
    /// </summary>
    public System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      XmlElement elem = xmlDoc.CreateElement("Grid");
      elem.SetAttribute("Width", this.Width.ToString());
      elem.SetAttribute("Height", this.Height.ToString());
      elem.SetAttribute("OffsetX", this.OffsetX.ToString());
      elem.SetAttribute("OffsetY", this.OffsetY.ToString());
      elem.SetAttribute("Visible", this.Visible.ToString());
      return elem;
    }
    #endregion

    #region ICloneable Members
    /// <summary>
    /// 
    /// </summary>
    public object Clone()
    {
      Grid grid = new Grid();

      grid.Visible = this.Visible;
      grid.Width = this.Width;
      grid.Height = this.Height;
      grid.OffsetX = this.OffsetX;
      grid.OffsetY = this.OffsetY;

      return grid;
    }
    #endregion
  }
}
