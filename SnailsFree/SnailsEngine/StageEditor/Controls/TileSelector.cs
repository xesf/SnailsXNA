using System;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
  public partial class TileSelector : UserControl
  {
    public event EventHandler SelectedTileChanged;

    public ITileToolboxItem SelectedItem
    {
      get
      {
        if (this._TileList.SelectedItem == null)
          return null;
        if (this._TileList.SelectedItem is ImageListxItemTile == false)
          return null;

        return ((ImageListxItemTile)this._TileList.SelectedItem).TileToolboxItem;
      }

      set
      {
        if (this._TileList.SelectedItem != null)
        {
          this._TileList.SelectedItem.Selected = false;
        }
        this._TileList.SelectedItem = null;
      }
    }

    public ThemeType Theme
    {
        get;
        private set;
    }

    public TileSelector()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
      this._TileList.Clear();
    }

      /// <summary>
      /// 
      /// </summary>
    public void InitializeFromSettings()
    {
        this._TileList.Clear();
        Settings.LoadTileToolboxItems();
        if (Settings.TileToolboxItems != null)
        {
            foreach (ITileToolboxItem item in Settings.TileToolboxItems)
            {
                this._TileList.Add(new ImageListxItemTile(item));
            }
        }
    }
     

    /// <summary>
    /// 
    /// </summary>
    private void OnSelectedTileChanged()
    {
      if (this.SelectedTileChanged != null)
      {
        this.SelectedTileChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _TileList_SelectedItemChanged(object sender, EventArgs e)
    {
      try
      {
        this.OnSelectedTileChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    private void group1_Resize(object sender, EventArgs e)
    {
        this.Height = this.group1.Height;
    }

    /// <summary>
    /// 
    /// </summary>
    public ITileToolboxItem GetTileToolboxItemByTile(Tile tile)
    {
        if (tile == null)
        {
            return null;
        }
        foreach (ImageListxItemTile item in this._TileList.Controls)
        {
            if (item.TileToolboxItem.Tile.StyleGroupId == tile.StyleGroupId)
            {
                return item.TileToolboxItem;
            }
        }
        return null;
    }
  }
}
