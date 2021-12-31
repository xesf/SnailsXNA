using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel;

namespace LevelEditor.Controls
{
  internal class OpenImageTypeEditor : UITypeEditor
  {
    private OpenFileDialog ofd = new OpenFileDialog();

    public override UITypeEditorEditStyle GetEditStyle(
     ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    public override object EditValue(
     ITypeDescriptorContext context,
     IServiceProvider provider,
     object value)
    {
      ofd.FileName = (value == null? null : value.ToString());
      ofd.Filter = "Image files|*.png";
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        return ofd.FileName;
      }
      return base.EditValue(context, provider, value);
    }
  }
}
