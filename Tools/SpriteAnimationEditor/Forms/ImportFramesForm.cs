using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor
{
  partial class ImportFramesForm : Form
  {
    Animation _Animation;

    public ImportFramesForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public DialogResult ShowDialog(IWin32Window owner, Animation animation)
    {
      this._Animation = animation;
      return this.ShowDialog(owner);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ImportFramesForm_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void _SelFrames_Click(object sender, EventArgs e)
    {
       try
       {
         OpenFileDialog dlg = new OpenFileDialog();
         dlg.Multiselect = true;
         if (dlg.ShowDialog(this) == DialogResult.OK)
         {
           foreach (string file in dlg.FileNames)
           {
             Frame frame = Frame.CreateFromImage(file);
             this._ImageList.Items.Add(frame);
           }

           if (this._ImageList.Items.Count > 0 && this._ImageList.SelectedItem == null)
             this._ImageList.SelectedIndex = 0;
         }
       }
       catch (System.Exception ex)
       {
         Diag.ShowException(this, ex);
       }
    }

    /// <summary>
    /// 
    /// </summary>

    private void _ImageList_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.Multiselect = true;
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
          foreach (string file in dlg.FileNames)
          {
            Frame frame = Frame.CreateFromImage(file);
            this._ImageList.Items.Add(frame);
          }

          if (this._ImageList.Items.Count > 0 && this._ImageList.SelectedItem == null)
            this._ImageList.SelectedIndex = 0;
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
  }
}