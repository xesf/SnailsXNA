using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SpriteAnimationEditor.Forms
{
  internal partial class TileTesterForm : Form, IViewState
  {
    #region Variables
    SetTile _SelectedTile;
    TileTest _TileTest;
    #endregion

    #region Properties
   
    SetTile SelectedTile
    {
      get { return this._SelectedTile; }
      set { this._SelectedTile = value; }
    }
  
    TileTest TileTest
    {
      get { return this._TileTest; }
      set { this._TileTest = value; }
    }
    #endregion

    #region Class constructs and overrides
    /// <summary>
    /// 
    /// </summary>
    public TileTesterForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public DialogResult ShowDialog(IWin32Window owner, TileTest tileTest)
    {
      this.TileTest = tileTest;
      this._TileSelector.Solution = this.TileTest.ParentSolution;
      this.Text = "Tile Tester - " + this.TileTest.Name;
      this._pnlSet.BackColor = this.TileTest.BackColor;
      return this.ShowDialog(owner);
    }

    /// <summary>
    /// 
    /// </summary>
    private void TileTesterForm_Load(object sender, EventArgs e)
    {
      try
      {
        if (this.TileTest == null)
        {
          this.Close();
          return;
        }
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
    #endregion

    #region Other Methods

    /// <summary>
    /// 
    /// </summary>
    private void EnableControls()
    {
      this._optGridVisible.Checked = this.TileTest.Grid.Visible;
      this._OptGridOnTop.Checked = this.TileTest.GridOnTop;
      this._optSnapToGrid.Checked = this.TileTest.SnapToGrid;
      this._optDeleteTile.Enabled = (this.SelectedTile != null);
    }

    /// <summary>
    /// 
    /// </summary>
    private SetTile GetTileAt(Point pt)
    {
      foreach (SetTile tile in this.TileTest.Tiles)
      {
        Rectangle rect = new Rectangle(tile.Location.X, tile.Location.Y,
                          tile.Rectangle.Width, tile.Rectangle.Height);

        if (rect.Contains(pt))
          return tile;
      }

      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    private void DeleteSelectedTile()
    {
      if (this.SelectedTile == null)
        return;

      this.TileTest.Tiles.Remove(this.SelectedTile);
      this.SelectedTile = null;
      this._pnlSet.Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    private void ClearCanvas()
    {
      if (MessageBox.Show(this, "Canvas will be cleared. Continue?", Settings.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
         == DialogResult.No)
        return;

      this.TileTest.Clear();
      this.SelectedTile = null;
      this._pnlSet.Refresh();
    }
    #endregion

    protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
    {
      if (this.SelectedTile == null)
        return false;

      int v = 1;
      if ((Control.ModifierKeys & Keys.Control) != 0)
        v = 10;

      int x = this.SelectedTile.Location.X, 
          y = this.SelectedTile.Location.Y;

       
      switch (m.WParam.ToInt32())
      {
        case 37: // Left
          x -= v;
          break;

        case 38: // Up
          y -= v;
          break;

        case 39: // Right
           x += v;
         break;

        case 40: // Down
           y += v;
         break;

        default:
          return false;
      }

      if (x < 0) x = 0;
      if (y < 0) y = 0;
      if (x > this._pnlSet.Width) x = this._pnlSet.Width;
      if (y > this._pnlSet.Height) y = this._pnlSet.Height;
      this.SelectedTile.Location = new Point(x, y);
      this._pnlSet.Refresh();
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void _pnlSet_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void _pnlSet_MouseClick(object sender, MouseEventArgs e)
    {
      try
      {

        this.SelectedTile = this.GetTileAt(e.Location);
        if (this.SelectedTile != null)
        {
          this._pnlSet.Refresh();
          if (e.Button == MouseButtons.Right)
          {
            this._ctxmnuTile.Show(this._pnlSet.PointToScreen(e.Location));
          }
          return;
        }

        if (e.Button == MouseButtons.Left)
        {
          SetTile setTile = new SetTile(this._TileSelector.SelectedTile.Animation, this._TileSelector.SelectedTile.FrameNr);

          if (this.TileTest.SnapToGrid == false)
          {
            setTile.Location = new Point(e.Location.X - (setTile.Width / 2),
                                     e.Location.Y - (setTile.Height / 2));
          }
          else
          {
            int x = ((int)(e.Location.X / this.TileTest.Grid.Width)) * this.TileTest.Grid.Width;
            int y = ((int)(e.Location.Y / this.TileTest.Grid.Height)) * this.TileTest.Grid.Height;
            setTile.Location = new Point(x, y);
          }
          this.TileTest.Tiles.Add(setTile);
          this._pnlSet.Refresh();
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
      finally
      {
        this.EnableControls();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _pnlSet_Paint(object sender, PaintEventArgs e)
    {
      try
      {
       
        e.Graphics.FillRectangle(new SolidBrush(this._pnlSet.BackColor),
                                                0, 0, this._pnlSet.Width, this._pnlSet.Height);

        if (this.TileTest.GridOnTop == false)
          this.TileTest.Grid.Paint(e.Graphics, ZoomFactor.Normal, new Rectangle(0, 0, this._pnlSet.Width, this._pnlSet.Height));
        
        foreach (SetTile tile in this.TileTest.Tiles)
        {
          Rectangle rect = tile.Animation.Frames[tile.FrameNr].Rectangle;

          e.Graphics.DrawImage(tile.Animation.ParentProject.OutputSprite,
           new Rectangle(tile.Location.X, tile.Location.Y, rect.Width, rect.Height),
           rect, GraphicsUnit.Pixel);
        }

        if (this.SelectedTile != null)
        {
          Rectangle rect = this.SelectedTile.Animation.Frames[this.SelectedTile.FrameNr].Rectangle;
          Pen pen = new Pen(Color.Black);
          pen.Width = 2;
          pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
          e.Graphics.DrawRectangle(pen, this.SelectedTile.Location.X,
                                        this.SelectedTile.Location.Y, 
                                        this.SelectedTile.Width,
                                        this.SelectedTile.Height);
        }

        if (this.TileTest.GridOnTop == true)
          this.TileTest.Grid.Paint(e.Graphics, ZoomFactor.Normal, new Rectangle(0, 0, this._pnlSet.Width, this._pnlSet.Height));

      }
      catch (System.Exception)
      {
      }
    }

    #region Menu events
    /// <summary>
    /// 
    /// </summary>
    private void _optVisible_Click(object sender, EventArgs e)
    {
      try
      {
        this.TileTest.Grid.Visible = (!this._optGridVisible.Checked);
        this._pnlSet.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptGridOnTop_Click(object sender, EventArgs e)
    {
      try
      {
        this.TileTest.GridOnTop = (!this._OptGridOnTop.Checked);
        this._pnlSet.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optSnapToGrid_Click(object sender, EventArgs e)
    {
      try
      {
        this.TileTest.SnapToGrid = (!this._optSnapToGrid.Checked);
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optGridSize_Click(object sender, EventArgs e)
    {
      try
      {
        GridForm form = new GridForm();
        form.Grid = this.TileTest.Grid;
        if (form.ShowDialog(this) == DialogResult.Cancel)
          return;

        this.TileTest.Grid = form.Grid;
        this._pnlSet.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optBakgroundColor_Click(object sender, EventArgs e)
    {
      try
      {
        this._dlgColor.Color = this.TileTest.BackColor;
        if (this._dlgColor.ShowDialog(this) == DialogResult.Cancel)
          return;

        this._pnlSet.BackColor = this._dlgColor.Color;
        this.TileTest.BackColor = this._dlgColor.Color;
        this._pnlSet.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optDeleteTile_Click(object sender, EventArgs e)
    {
      try
      {
        this.DeleteSelectedTile();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optClearCanvas_Click(object sender, EventArgs e)
    {
      try
      {
        this.ClearCanvas();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptRefreshProjImages_Click(object sender, EventArgs e)
    {
      try
      {
        foreach (Project project in this.TileTest.ParentSolution.ProjectList)
        {
          project.RefreshOutputSprite();
        }
        this._pnlSet.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    #endregion

    private void _TileSelector_Load(object sender, EventArgs e)
    {

    }


    #region IViewState Members
    /// <summary>
    /// 
    /// </summary>
    public void InitFromXml(System.Xml.XmlElement elemParent)
    {
      XmlElement elem = (XmlElement)elemParent.SelectSingleNode(this.Name);
      if (elem == null)
        return;

      this.Left = XmlHelper.GetAttribute(elem, "Left", this.Left);
      this.Top = XmlHelper.GetAttribute(elem, "Top", this.Top);
      this.Width = XmlHelper.GetAttribute(elem, "Width", this.Width);
      this.Height = XmlHelper.GetAttribute(elem, "Height", this.Height);

    }

    /// <summary>
    /// 
    /// </summary>
    public System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      XmlElement elem = xmlDoc.CreateElement(this.Name);

      elem.SetAttribute("Left", this.Left.ToString());
      elem.SetAttribute("Top", this.Top.ToString());
      elem.SetAttribute("Width", this.Width.ToString());
      elem.SetAttribute("Height", this.Height.ToString());

      return elem;
    }

    #endregion
  }
}
