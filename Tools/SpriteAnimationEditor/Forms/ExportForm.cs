using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{
  public partial class ExportForm : Form
  {
    public int Cols
    {
      get { return System.Convert.ToInt32(this._Cols.Text); } 
    }
    public string Filename
    {
      get { return this._Filename.Text; }
    }


    public ExportForm()
    {
      InitializeComponent();
    }

    private void _ExportForm_Load(object sender, EventArgs e)
    {

    }

    
  }
}
