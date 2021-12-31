using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SpriteAnimationEditor.Forms
{
  internal partial class ResolutionMappingForm : Form
  {
    public ResolutionMapper Mapper{ get; set; }

    Animation SelectedAnimation
    {
      get
      {
        if (this._lstAnimations.SelectedItem == null)
          return null;

        return (Animation)this._lstAnimations.SelectedItem;
      }
    }

    public ResolutionMappingForm()
    {
      InitializeComponent();
      
    }

    /// <summary>
    /// 
    /// </summary>
    public DialogResult Add(Form parentForm, Project project)
    {
      this.Mapper = new ResolutionMapper(project);
      this.Mapper.Name = project.Name;
      this.Mapper.OutputFile = Path.GetFileName(project.Filename);
      DialogResult dr = this.ShowDialog(ParentForm);
      return dr;
    }

    /// <summary>
    /// 
    /// </summary>
    public DialogResult Edit(Form parentForm, ResolutionMapper mapper)
    {
      this.Mapper = mapper.Clone();
      DialogResult dr = this.ShowDialog(ParentForm);
      return dr;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
      base.Refresh();
      if (this.Mapper == null)
        return;
      this._lstAnimations.Items.Clear();

      this._lblProject.Text = this.Mapper.ParentProject.Name;
      this._txtName.Text = this.Mapper.Name;
      this._txtSSOutput.Text = this.Mapper.OutputFile;
      this.ttSSFile.SetToolTip(this._txtSSOutput, this._txtSSOutput.Text);
      this._chkActive.Checked = this.Mapper.Active;

      if (this.Mapper.ParentProject != null)
      {
        foreach (Animation anim in this.Mapper.ParentProject.AnimationList)
        {
          this._lstAnimations.Items.Add(anim);
        }
      }

      this._chkActive.Checked = this.Mapper.Active;
      this.RefreshImage();
    }

    /// <summary>
    /// 
    /// </summary>
    void RefreshImage()
    {
      this._imgImage.Visible = Settings.Instance.ShowImages;
      this._lblImage.Text = this.Mapper.ImageFilename;
      this.ttPngFile.SetToolTip(this._lblImage, this._lblImage.Text);
      if (this.Mapper == null || this.Mapper.ImageFilename == null)
        return;

      this._imgImage.Image = Bitmap.FromFile(this.Mapper.ImageFilename);
      this._imgImage.SizeMode = PictureBoxSizeMode.AutoSize;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResolutionMappingForm_Load(object sender, EventArgs e)
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
    private void _optSelImage_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.Mapper == null)
          return;

        this._DlgOpen.InitialDirectory = Settings.Instance.LastResMapImageFolder;
        if (this._DlgOpen.ShowDialog(this) != DialogResult.OK)
          return;

        Settings.Instance.LastResMapImageFolder = Path.GetDirectoryName(this._DlgOpen.FileName);
        this.Mapper.ImageFilename = this._DlgOpen.FileName;
        this.RefreshImage();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _btnOk_Click(object sender, EventArgs e)
    {
      try
      {
       
        this.Mapper.Name = this._txtName.Text;
        this.Mapper.OutputFile = this._txtSSOutput.Text;
        this.Mapper.Active = this._chkActive.Checked;
        Settings.Instance.LastResMapSSFolder = Path.GetDirectoryName(this.Mapper.OutputFile);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResolutionMappingForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      try
      {
        if (string.Compare(this.Mapper.OutputFile, this.Mapper.ParentProject.Filename, true) == 0)
        {
          e.Cancel = true;
          throw new ApplicationException("Output SS filename cannot be equal to project filename.");
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
    private void _btnSelFolder_Click(object sender, EventArgs e)
    {
      try
      {
        this._dlgFolder.SelectedPath = Settings.Instance.LastResMapSSFolder;
        if (this._dlgFolder.ShowDialog(this) == DialogResult.Cancel)
          return;
        this._txtSSOutput.Text = Path.Combine(this._dlgFolder.SelectedPath, Path.GetFileName(this._txtSSOutput.Text));
        Settings.Instance.LastResMapSSFolder = this._dlgFolder.SelectedPath;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _lstAnimations_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this._imgImage.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _imgImage_Paint(object sender, PaintEventArgs e)
    {
      if (this.SelectedAnimation == null)
        return;

      if (this._imgImage.Image == null)
        return;

      foreach (AnimationFrame frame in this.SelectedAnimation.Frames)
      {
        Pen pen = new Pen(this.Mapper.ParentProject.FrameRectangleColor);
        e.Graphics.DrawRectangle(pen, frame.Rectangle);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _btnOk_Validating(object sender, CancelEventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(this._txtName.Text))
        {
          MessageBox.Show(this, "Name must be filled.", Settings.AppName);
          e.Cancel = true;
          return;
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
    private void _txtName_TextChanged(object sender, EventArgs e)
    {
      try
      {
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
  }
}
