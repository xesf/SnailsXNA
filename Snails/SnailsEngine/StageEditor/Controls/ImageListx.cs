using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
  public partial class ImageListx : UserControl
  {
    public event EventHandler SelectedItemChanged;

    ImageListxItem _SelectedItem;
    public ImageListxItem SelectedItem
    {
      get
      {
        return this._SelectedItem;
      }
      set
      {
        if (this._SelectedItem != value)
        {
          this._SelectedItem = value;
          this.OnSelectedItemChanged();
        }
      }
    }
    public ImageListx()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
      this.Controls.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Add(ImageListxItem item)
    {
      int y = this.Padding.Top;
      if (this.Controls.Count > 0)
      {
        y += this.Controls[this.Controls.Count - 1].Top +
             this.Controls[this.Controls.Count - 1].Height;
      }
      item.Top = y;
      item.Left = this.Padding.Left;
      this.Controls.Add(item);
      item.ItemClicked += new EventHandler(item_ItemClicked);
      this.CenterControl(item);
    }

    /// <summary>
    /// 
    /// </summary>
    void item_ItemClicked(object sender, EventArgs e)
    {
      if (sender is ImageListxItem == false)
        return;

      ImageListxItem item = (ImageListxItem)sender;

      if (this.SelectedItem != null)
      {
        this.SelectedItem.Selected = false;
      }

      item.Selected = true;
      this.SelectedItem = item;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnSelectedItemChanged()
    {
      if (this.SelectedItemChanged != null)
      {
        this.SelectedItemChanged(this, new EventArgs());
      }
    }
      /// <summary>
      /// 
      /// </summary>
      private void CenterControl(Control ctl)
      {
          if (ctl == null)
              return;

          ctl.Left = (this.Width / 2) - (ctl.Width / 2);
      }
      

      /// <summary>
      /// 
      /// </summary>
    private void ImageListx_Resize(object sender, EventArgs e)
    {
        try
        {
            foreach (Control ctl in this.Controls)
            {
                this.CenterControl(ctl);
            }
        }
        catch(System.Exception ex)
        {
            Diag.ShowException(this.ParentForm, ex);
        }
    }

    private void ImageListx_Scroll(object sender, ScrollEventArgs e)
    {
        this.Refresh();
    }

  }

}
