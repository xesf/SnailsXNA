using System;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Controls
{
  class ZoomCombo : ComboBox
  {
    public ZoomFactor Zoom
    {
      get 
      {
        if (this.SelectedItem == null)
          return ZoomFactor.Normal;

        return (ZoomFactor)this.SelectedItem; 
      }
      set { this.SelectedItem = value; }
    }

    public ZoomCombo()
    {
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      // 
      // ZoomCombo
      // 
      this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.Items.AddRange(new object[] {
            SpriteAnimationEditor.ZoomFactor.Normal,
            SpriteAnimationEditor.ZoomFactor.x2,
            SpriteAnimationEditor.ZoomFactor.x4,
            SpriteAnimationEditor.ZoomFactor.x8,
            SpriteAnimationEditor.ZoomFactor.x16});
      this.SelectedIndex = 0;
      this.ResumeLayout(false);

    }
  }
}
