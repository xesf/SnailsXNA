using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{
  partial class GenerateImageFromFilesForm : Form
  {
    private Project _Project;
    private Project Project
    {
      set { this._Project = value; }
      get { return this._Project; }
    }

    public int FramesPerColumn
    {
      get 
      {
        if (String.IsNullOrEmpty(this._TxtFramesPerColumn.Text))
          return 0;

        return System.Convert.ToInt32(this._TxtFramesPerColumn.Text); 
      }
      private set
      {
        this._TxtFramesPerColumn.Text = value.ToString();
        this.Refresh();
      }
    }

    public int FrameWidth
    {
      get
      {
        if (this._ImageList.Items.Count == 0)
          return 0;
        Image image = Image.FromFile(this._ImageList.Items[0].ToString());
        return image.Width;
      }
    }

    private int FrameCount
    {
      get
      {
        return this._ImageList.Items.Count;
      }
    }

    public int FrameHeight
    {
      get
      {
        if (this._ImageList.Items.Count == 0)
          return 0;
        Image image = Image.FromFile(this._ImageList.Items[0].ToString());
        return image.Height;
      }
    }

    private int SpriteWidth
    {
      get
      {
        return this.FrameWidth * this.FramesPerColumn;
      }
    }

    private int SpriteHeight
    {
      get
      {
        if (this.FramesPerColumn == 0)
          return 0;

        return this.FrameHeight * ((this.FrameCount / this.FramesPerColumn) + (this.FrameCount % this.FramesPerColumn == 0? 0 : 1));
      }
    }

    public bool AutoGenerateFrames
    {
      get { return this._ChkAutoGenFrames.Checked; }
    }

    public bool DeleteExistingFrames
    {
      get { return this._ChkDeleteExistingFrames.Checked; }
    }

    public Animation AnimationToAffect
    {
      get
      {
        if (this._cmbAnimations.SelectedIndex == 0)
          return null;

        return (Animation) this._cmbAnimations.SelectedItem;
      }
    }
    public List<string> FilenameList
    {
      get
      {
        List<string> fileList = new List<string>();
        foreach (string s in this._ImageList.Items)
        {
          fileList.Add(s);
        }
        return fileList;
      }
    }
    public GenerateImageFromFilesForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public DialogResult ShowDialog(IWin32Window owner, Project project)
    {
      if (project == null)
        return DialogResult.Cancel;

      this.Project = project;
      return this.ShowDialog(owner);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ImportFramesForm_Load(object sender, EventArgs e)
    {
      try
      {
        this.FramesPerColumn = 0;
        this.Visible = true;
        this.Refresh();

        this._cmbAnimations.Items.Add("[Create new animation]");
        foreach (Animation anim in this.Project.AnimationList)
        {
          this._cmbAnimations.Items.Add(anim);
        }
        this._cmbAnimations.SelectedIndex = 0;
        this.ImportFrames();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
      finally
      {
        this.EnableButtons();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ImportFrames()
    {
      this._OpenFilesDlg.InitialDirectory = Settings.Instance.LastImportFramesFolder;
      if (this._OpenFilesDlg.ShowDialog(this) == DialogResult.OK)
      {
        // Check frame sizes. they must all have the same size
        if (this._OpenFilesDlg.FileNames.Length == 0)
          return;

        Goodies.CheckFrameSizes((this._ImageList.Items.Count > 0? this._ImageList.Items[0].ToString(): null),
                    this._OpenFilesDlg.FileNames);
        foreach (string file in this._OpenFilesDlg.FileNames)
        {
          this._ImageList.Items.Add(file);
        }

        if (this._ImageList.Items.Count > 0 && this._ImageList.SelectedItem == null)
        {
          this._ImageList.SelectedIndex = 0;
          Settings.Instance.LastImportFramesFolder = Path.GetDirectoryName(this._OpenFilesDlg.FileNames[0]);
        }
      }
      this.Refresh();

    }

    /// <summary>
    /// 
    /// </summary>
    private void _SelFrames_Click(object sender, EventArgs e)
    {
      try
      {
        this.ImportFrames();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
      finally
      {
        this.EnableButtons();
      }
    }

    /// <summary>
    /// 
    /// </summary>

    private void _ImageList_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (this._ImageList.SelectedItem == null)
          return;
        this._CurFrame.Image = Image.FromFile(this._ImageList.SelectedItem.ToString());
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
      base.Refresh();
      this._LblTotal.Text = this.FrameCount.ToString();
      this._LblFrameSize.Text = string.Format("{0} X {1}", this.FrameWidth, this.FrameHeight);
      this._LblSpriteSize.Text = string.Format("{0} X {1}", this.SpriteWidth, this.SpriteHeight);
      this.EnableButtons();
      this._CurFrame.Visible = Settings.Instance.ShowImages;
    }

    /// <summary>
    /// 
    /// </summary>
    private void EnableButtons()
    {
      this._btnOk.Enabled = (this.FrameCount > 0 && this.FramesPerColumn > 0);
      this._cmbAnimations.Enabled = this._ChkAutoGenFrames.Checked;
      this._ChkDeleteExistingFrames.Enabled = this._ChkAutoGenFrames.Checked;
    }

    /// <summary>
    /// 
    /// </summary>
    private void _TxtFramesPerColumn_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ImportFramesForm_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        if (this._ImageList.SelectedItem == null)
          return;

        if (e.KeyCode == Keys.Delete)
        {
          if (this._ImageList.SelectedItems.Count == 1)
          {
            int iSelIdx = this._ImageList.SelectedIndex;
            this._ImageList.Items.Remove(this._ImageList.SelectedItem);
            if (this._ImageList.Items.Count > 0)
            {
              iSelIdx--;
              if (iSelIdx < 0)
                iSelIdx = 0;

              this._ImageList.SelectedIndex = iSelIdx;
            }
          }
          else
          {
            for (; this._ImageList.SelectedItems.Count > 0; )
            {
              this._ImageList.Items.Remove(this._ImageList.SelectedItems[0]);
            }
          }
          this.Refresh();
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
    private void _ChkAutoGenFrames_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.EnableButtons();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
  }
}