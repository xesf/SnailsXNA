using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Controls
{
  public partial class DoubleBufferedPanel : Panel
  {
  
    public DoubleBufferedPanel()
    {
      InitializeComponent();

      this.SetStyle(
      ControlStyles.UserPaint |
      ControlStyles.AllPaintingInWmPaint |
      ControlStyles.DoubleBuffer, true);
    }


    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
      if (DesignMode)
        base.OnPaintBackground(pevent);
    }
  }
}
