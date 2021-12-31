using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{
  public partial class GridForm : Form
  {
    public Grid Grid
    {
      get 
      {
        Grid grid = new Grid();
        int v;
        Int32.TryParse(this._txtWidth.Text, out v);
        grid.Width = v;
        Int32.TryParse(this._txtHeight.Text, out v);
        grid.Height = v;
        Int32.TryParse(this._txtOffsetX.Text, out v);
        grid.OffsetX = v;
        Int32.TryParse(this._txtOffsetY.Text, out v);
        grid.OffsetY = v;

        return grid;
      }
      set
      {
        this._txtWidth.Text = value.Width.ToString();
        this._txtHeight.Text = value.Height.ToString();
        this._txtOffsetX.Text = value.OffsetX.ToString();
        this._txtOffsetY.Text = value.OffsetY.ToString();

      }
    }

    public GridForm()
    {
      InitializeComponent();
    }

  }
}
