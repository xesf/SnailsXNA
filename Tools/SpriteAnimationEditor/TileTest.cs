using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;

namespace SpriteAnimationEditor
{
  class TileTest : IViewState
  {
    #region Variables
    string _Name;
    List<SetTile> _Tiles;
    Solution _ParentSolution;
    Grid _Grid;
    bool _GridOnTop;
    bool _SnapToGrid;
    Color _BackColor;
    bool _Changed;
    #endregion

    #region Properties
    public Solution ParentSolution
    {
      get { return this._ParentSolution; }
      private set { this._ParentSolution = value; }
    }

    bool Changed
    {
      get { return this._Changed; }
      set { this._Changed = value; }
    }

    public Color BackColor
    {
      get { return this._BackColor; }
      set 
      {
        if (this._BackColor != value)
        {
          this._BackColor = value;
          this.Changed = true;
        }
      }
    }

    public List<SetTile> Tiles
    {
      get { return this._Tiles; }
      private set { this._Tiles = value; }
    }

    public bool GridOnTop
    {
      get { return this._GridOnTop; }
      set 
      { 
        this._GridOnTop = value; 
      }
    }

    public bool SnapToGrid
    {
      get { return this._SnapToGrid; }
      set { this._SnapToGrid = value; }
    }

    public Grid Grid
    {
      get { return this._Grid; }
      set { this._Grid = value; }
    }

    public string Name
    {
      get { return this._Name; }
      set { this._Name = value; }
    }
    #endregion

    #region Class constructs and overrides

    /// <summary>
    /// 
    /// </summary>
    private TileTest(Solution parentSolution)
    {
      this.ParentSolution = parentSolution;
      this.Tiles = new List<SetTile>();
      this.Grid = new Grid();
    }

    /// <summary>
    /// 
    /// </summary>
    public TileTest(Solution parentSolution, string name)
    {
      this.ParentSolution = parentSolution;
      this.Tiles = new List<SetTile>();
      this.Grid = new Grid();
      this.Name = name;
    }

    /// <summary>
    /// 
    /// </summary>
    public static TileTest FromXml(XmlElement elem, Solution solution)
    {
      TileTest test = new TileTest(solution);
      test.InitFromXml(elem);
      return test;
    }
    #endregion

    #region Other methods
    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
      this.Tiles = new List<SetTile>();
    }
    #endregion

    #region IViewState Members
    /// <summary>
    /// 
    /// </summary>
    public void InitFromXml(System.Xml.XmlElement elem)
    {
      this.Name = XmlHelper.GetAttribute(elem, "Name", "");
      this.SnapToGrid = XmlHelper.GetAttribute(elem, "SnapToGrid", false);
      this.GridOnTop = XmlHelper.GetAttribute(elem, "GridOnTop", false);
      this.BackColor = XmlHelper.GetAttribute(elem, "BackColor", Color.Gray);
      XmlElement elemGrid = (XmlElement)elem.SelectSingleNode("Grid");
      this.Grid = SpriteAnimationEditor.Grid.FromXml(elemGrid);

      this.Tiles = new List<SetTile>();
      XmlNodeList nodesTiles = elem.SelectNodes("Tiles/SetTile");
      foreach (XmlElement elemTile in nodesTiles)
      {
        if (SetTile.TryFromXml(elemTile, this.ParentSolution))
        {
          Tiles.Add(SetTile.FromXml(elemTile, this.ParentSolution));
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      XmlElement elem = xmlDoc.CreateElement("TileTest");

      elem.SetAttribute("Name", this.Name);
      elem.SetAttribute("SnapToGrid", this.SnapToGrid.ToString());
      elem.SetAttribute("GridOnTop", this.GridOnTop.ToString());
      elem.SetAttribute("BackColor", this.BackColor.ToArgb().ToString());
      elem.AppendChild(this.Grid.ToXml(xmlDoc));

      XmlElement elemTiles = xmlDoc.CreateElement("Tiles");
      foreach (SetTile tile in this.Tiles)
      {
        elemTiles.AppendChild(tile.ToXml(xmlDoc));
      }
      elem.AppendChild(elemTiles);

      return elem;
    }

    #endregion
  }
}
