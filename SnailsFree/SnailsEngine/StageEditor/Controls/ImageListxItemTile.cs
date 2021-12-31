using System.Windows.Forms;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
  public partial class ImageListxItemTile : ImageListxItem
  {

    public ITileToolboxItem TileToolboxItem { get { return (ITileToolboxItem)this.ToolboxItem; } }

    /// <summary>
    /// 
    /// </summary>
    public ImageListxItemTile()
    {
      InitializeComponent();
      this.BorderStyle = BorderStyle.FixedSingle;
    }

    /// <summary>
    /// 
    /// </summary>
    public ImageListxItemTile(ITileToolboxItem tileToolboxItem)
    {
      InitializeComponent();
      this.ToolboxItem = tileToolboxItem;
    }
  }
}
