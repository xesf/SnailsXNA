using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{
  public partial class ExportInGameFilesForm : Form
  {
    public string PngFile
    {
      get { return this._PngFile.Text; }
      set { this._PngFile.Text = value; }
    }

    public string AnimationDataFile
    {
      get { return this._AnimationDataFile.Text; }
      set { this._AnimationDataFile.Text = value; }
    }

    public ExportInGameFilesForm()
    {
      InitializeComponent();
    }
  }
}