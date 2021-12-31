using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{
  public partial class PreferencesForm : Form
  {
    public PreferencesForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    private void PreferencesForm_Load(object sender, EventArgs e)
    {
      try
      {
        this._PrefsGrid.SelectedObject = Settings.Instance;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _BtnOk_Click(object sender, EventArgs e)
    {
      try
      {
        this.Hide();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }

    }
  }
}