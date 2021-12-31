using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Controls
{
  internal partial class TileSelector : UserControl
  {
    #region Consts
    Color SEL_COLOR = Color.FromArgb(196, 225, 255);
    Color SEL_BORDER_COLOR = Color.FromArgb(51, 153, 255);
    #endregion

    #region Variables
    public Solution _Solution;
    Tile _SelectedTile;
    #endregion

    #region Properties
    public Solution Solution
    {
      get { return this._Solution; }
      set 
      {
        if (this._Solution != value)
        {
          this._Solution = value;
          this.RefreshCombos();
        }
      }
    }

    public Tile SelectedTile
    {
      get { return this._SelectedTile; }
      private set { this._SelectedTile = value; }
    }

    Animation CurrentAnimation
    {
      get 
      {
        if (this._cmbAnimation.Items.Count == 0)
          return null;
        if (this._cmbAnimation.SelectedItem == null)
          return null;

        return (Animation)this._cmbAnimation.SelectedItem;
      }
    }

    #endregion

    #region Class constructs and overrides
    /// <summary>
    /// 
    /// </summary>
    public TileSelector()
    {
      InitializeComponent();
    }
    #endregion

    #region Other methods
    /// <summary>
    /// 
    /// </summary>
    private void RefreshCombos()
    {
      this._cmbAnimation.Items.Clear();
      this._cmbProject.Items.Clear();
      if (this.Solution == null)
        return;

      foreach (Project project in this.Solution.ProjectList)
      {
        this._cmbProject.Items.Add(project);
      }

      if (this.Solution.ProjectList.Count == 0)
        return;

      this._cmbProject.SelectedIndex = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    private void RepositionTiles()
    {
      int x = 10, y = 10, maxHeight = 0;
      for (int i = 0; i < this._pnlTiles.Controls.Count; i++)
      {
        Tile tile = (Tile)this._pnlTiles.Controls[i];

        tile.Left = x;
        tile.Top = y;
        if (tile.Height > maxHeight)
          maxHeight = tile.Height;

        x += tile.Width + 10;
        if (i + 1 < this._pnlTiles.Controls.Count)
        {
           Tile nextTile = (Tile)this._pnlTiles.Controls[i + 1];
           if (x + nextTile.Width + 10 > this._pnlTiles.Width)
           {
             x = 10;
             y += maxHeight + 10;
           }
        }
      }
    }
    #endregion

    #region control events
    /// <summary>
    /// 
    /// </summary>
    private void _cmbProject_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this._cmbProject.Items.Count == 0)
        return;
      this.SelectedTile = null;
      this._cmbAnimation.Items.Clear();
      this._pnlTiles.Controls.Clear();
      foreach (Animation animation in ((Project)this._cmbProject.SelectedItem).AnimationList)
      {
        this._cmbAnimation.Items.Add(animation);
      }

      if (this._cmbAnimation.Items.Count > 0)
        this._cmbAnimation.SelectedIndex = 0;

    }

    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
      this._pnlTiles.Controls.Clear();
      this.SelectedTile = null;
      foreach (AnimationFrame frame in this.CurrentAnimation.Frames)
      {
        Tile tile = new Tile(this.CurrentAnimation, frame.FrameNr);
        tile.Click += new EventHandler(tile_Click);
        this._pnlTiles.Controls.Add(tile);
      }
      this.RepositionTiles();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _cmbAnimation_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _pnlTiles_Resize(object sender, EventArgs e)
    {
      try
      {
        this.SelectedTile = (Tile)sender;
      }
      catch (System.Exception)
      {
      }
    }

    /// <summary>
    /// 
    /// </summary
    private void tile_Click(object sender, EventArgs e)
    {
      if (this.SelectedTile != null)
      {
        this.SelectedTile.BackColor = this._pnlTiles.BackColor;
      }
      this.SelectedTile = (Tile)sender;
      this.SelectedTile.BackColor = SEL_COLOR;
      this._pnlTiles.Refresh();
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    private void _pnlTiles_Paint(object sender, PaintEventArgs e)
    {
      try
      {
        if (this.SelectedTile != null)
        {
          Tile tile = this.SelectedTile;
          SolidBrush brush = new SolidBrush(SEL_COLOR);

          e.Graphics.FillRectangle(brush, tile.Left - 5, tile.Top - 5, tile.Width + 10, tile.Height + 7);
          e.Graphics.DrawRectangle(new Pen(SEL_BORDER_COLOR), tile.Left - 5, tile.Top - 5, tile.Width + 10, tile.Height + 7);
        }
      }
      catch (System.Exception)
      {
      }
    }

  }
}
