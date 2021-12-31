using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{
  internal partial class InGameFilesExportForm : Form
  {
    #region Variables
    List<Project> _Projects;
    #endregion

    #region Properties
    public List<Project> Projects
    {
      get { return this._Projects; }
      set 
      { 
        this._Projects = value;
        this.Refresh();
      }
    }
    public List<Project> SelectedProjects
    {
      get
      {
        List<Project> selectedProjects = new List<Project>();
        foreach(Project project in this._ProjectList.CheckedItems)
        {
          selectedProjects.Add(project);
        }
        return selectedProjects;
      }
    }

    public string TextureFileFolderOverride
    {
      get { return this._TxtTextureFiles.Text; }
      set { this._TxtTextureFiles.Text = value; }
    }

    public string AnimationDataFolderOverride
    {
      get { return this._TxtDataFiles.Text; }
      set { this._TxtDataFiles.Text = value; }
    }

    public bool OverrideFolders
    {
      get { return this._ChkOverride.Enabled; }
      set { this._ChkOverride.Enabled = value; }
    }
    #endregion

    public InGameFilesExportForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
      base.Refresh();
      this._ProjectList.Items.Clear();
      foreach(Project project in this.Projects)
      {
        this._ProjectList.Items.Add(project, false);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CheckProject(Project projectToCheck)
    {
      for(int i = 0; i < this._ProjectList.Items.Count; i++)
      {
        if (this._ProjectList.Items[i] == projectToCheck)
        {
          this._ProjectList.SetSelected(i, true);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CheckAll()
    {
      for (int i = 0; i < this._ProjectList.Items.Count; i++)
      {
        this._ProjectList.SetItemChecked(i, true);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void InGameFilesExportForm_Load(object sender, EventArgs e)
    {
      try
      {
        this._ChkOverride.Checked = Settings.Instance.ExportOverrideFolders;
        this._TxtTextureFiles.Text = Settings.Instance.TextureFolderOverride;
        this._TxtDataFiles.Text = Settings.Instance.AnimationDataFolderOverride;
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void EnableControls()
    {
      this._PanelOverrides.Enabled = this._ChkOverride.Checked;
      this._BtnOk.Enabled = (this.SelectedProjects.Count > 0);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _ChkOverride_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.EnableControls();

        if (this._ChkOverride.Checked)
          this._TxtTextureFiles.Focus();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _ProjectList_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _BtnTexturesFolder_Click(object sender, EventArgs e)
    {
      try
      {
        this._DlgSelectFolder.SelectedPath = this._TxtTextureFiles.Text;
        if (this._DlgSelectFolder.ShowDialog(this) == DialogResult.Cancel)
          return;
        this._TxtTextureFiles.Text = this._DlgSelectFolder.SelectedPath;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _BtnDataFolder_Click(object sender, EventArgs e)
    {
      try
      {
        this._DlgSelectFolder.SelectedPath = this._TxtDataFiles.Text;
        if (this._DlgSelectFolder.ShowDialog(this) == DialogResult.Cancel)
          return;
        this._TxtDataFiles.Text = this._DlgSelectFolder.SelectedPath;
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
        Settings.Instance.ExportOverrideFolders = this._ChkOverride.Checked;
        Settings.Instance.TextureFolderOverride = this._TxtTextureFiles.Text;
        Settings.Instance.AnimationDataFolderOverride = this._TxtDataFiles.Text;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
  }
}
