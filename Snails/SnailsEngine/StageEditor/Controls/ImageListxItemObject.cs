using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
  public partial class ImageListxItemObject : ImageListxItem
  {
    IObjectToolboxItem ObjToolboxItem { get { return (IObjectToolboxItem)this.ToolboxItem; } }

    StageObject _Object;
    public StageObject Object 
    {
      get { return this._Object; }
      set
      {

          this._Object = value;
          if (this.ObjToolboxItem == null)
          {
              if (this._Object != value)
              {
                  this.Width = this.FrameRect.Width + (ImageListxItem.LEFT_MARGIN * 2);
                  this.Height = this.FrameRect.Height + (ImageListxItem.TOP_MARGIN * 2);
              }
          }
        
      }
    }

    public ImageListxItemObject()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public ImageListxItemObject(IObjectToolboxItem objToolboxItem)
    {
        this.ToolboxItem = objToolboxItem;
        this.Object = this.ObjToolboxItem.StageObject;
    }
      
    /// <summary>
    /// 
    /// </summary>
    public ImageListxItemObject(StageObject obj)
    {
      this.Object = obj;
    }
  }
}
