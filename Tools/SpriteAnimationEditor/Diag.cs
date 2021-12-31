using System;
using System.Windows.Forms;

namespace SpriteAnimationEditor
{
  class Diag
  {
    public static void ShowException(Form owner, System.Exception ex)
    {
      MessageBox.Show(owner, ex.Message + "\n" +ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }
}
