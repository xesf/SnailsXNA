using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace SpriteAnimationEditor.Forms
{
  public partial class MainForm : Form , IViewState
  {

    #region Variables
    PreferencesForm _PreferencesForm = new PreferencesForm();
    Controls.ZoomMenu _mnuZoom;
    static MainForm _Instance;
    #endregion

    #region Properties
    internal static Controls.SolutionManager SolutionManager
    {
      get { return MainForm._Instance._SolutionManager; }
    }
    #endregion

    #region Constructs and overrides
    /// <summary>
    /// 
    /// </summary>
    public MainForm()
    {
      InitializeComponent();
      this._mnuZoom = new SpriteAnimationEditor.Controls.ZoomMenu();
      this._mnuZoom.ZoomSelected += new SpriteAnimationEditor.Controls.ZoomMenu.ZoomSelectedHandler(_mnuZoom_ZoomSelected);
      MainForm._Instance = this;
    }
    #endregion
  
    #region Main form events
    /// <summary>
    /// 
    /// </summary>
    private void MainForm_Load(object sender, EventArgs e)
    {
      try
      {
        Settings.Instance.ShowImagesChanged += new EventHandler(Settings_ShowImagesChanged);
        Settings.Instance.Load();

        this._OutputSprite.Zoom = Settings.Instance.OutputSpriteZoom;
        this._mnuView.DropDownItems.Add(this._mnuZoom);

        if (Settings.Instance.RecentFiles.Count > 0)
        {
          this.OpenSolution(Settings.Instance.RecentFiles[0]);
        }
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
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      try
      {
        e.Cancel = (!this.CheckSave());
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      try
      {
        Settings.Instance.OutputSpriteZoom = this._OutputSprite.Zoom;
        Settings.Instance.Save();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    #endregion

    #region Setttings events
    /// <summary>
    /// 
    /// </summary>
    private void Settings_ShowImagesChanged(object sender, EventArgs e)
    {
      try
      {
        this._Playback.ShowImages = Settings.Instance.ShowImages;
        this._OutputSprite.ShowImages = Settings.Instance.ShowImages;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    #endregion

    #region Open/Save methods
    /// <summary>
    /// 
    /// </summary>
    private bool SaveSolution(bool saveAs, Solution solution)
    {
      string filename = solution.Filename;
      if (saveAs)
      {
        this._DlgSave.FileName = this._SolutionManager.Solution.Filename;
        this._DlgSave.Filter = Settings.SOLUTION_FILE_FILTER;
        if (this._DlgSave.ShowDialog(this) == DialogResult.Cancel)
          return false;
        filename = this._DlgSave.FileName;
      }
      solution.Save(filename);
      this.EnableControls();
      this._StatusText.Text = "Solution saved.";
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    private bool SaveProject(bool saveAs, Project project)
    {
      if (project == null)
        return false;
      string filename = project.Filename;

      if (saveAs)
      {
        this._DlgSave.FileName = this._SolutionManager.Solution.Filename;
        this._DlgSave.Filter = Settings.PROJECT_FILE_FILTER;
        if (this._DlgSave.ShowDialog(this) == DialogResult.Cancel)
          return false;
        filename = this._DlgSave.FileName;
      }

      project.Save(filename);
      this.EnableControls();
      this._StatusText.Text = "Project saved.";
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    private bool Save(bool saveAs)
    {
      switch(this._SolutionManager.SelectedNodeType)
      {
        case SpriteAnimationEditor.Controls.SolutionManager.NodeType.Solution:
          return this.SaveSolution(saveAs, this._SolutionManager.Solution);

        case SpriteAnimationEditor.Controls.SolutionManager.NodeType.Project:
        case SpriteAnimationEditor.Controls.SolutionManager.NodeType.Animation:
          return this.SaveProject(saveAs, this._SolutionManager.SelectedProject);
      }
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void SaveAll(bool forceSave)
    {
      this.SaveSolution(false, this._SolutionManager.Solution);
      foreach (Project project in this._SolutionManager.Solution.ProjectList)
      {
        if (project.Changed || forceSave)
        {
          this.SaveProject(false, project);
        }
      }
      this._StatusText.Text = "Saved all succedded.";
    }

    /// <summary>
    /// 
    /// </summary>
    private void OpenSolution(string filename)
    {
      if (this.CheckSave() == false)
        return;
      if (filename == null)
      {
        this._DlgOpen.InitialDirectory = Settings.Instance.LastSolutionFolder;
        this._DlgOpen.Filter = Settings.SOLUTION_FILE_FILTER;
        if (this._DlgOpen.ShowDialog(this) == DialogResult.Cancel)
          return;

        filename = this._DlgOpen.FileName;
      }
      this.Cursor = Cursors.WaitCursor;
      try
      {
        this._SolutionManager.OpenSolution(filename);
        Settings.Instance.LastSolutionFolder = Path.GetDirectoryName(filename);
        Settings.Instance.LastProjectFolder = Path.GetDirectoryName(filename);
        Settings.Instance.AddRecentFile(filename);
        this._StatusText.Text = "Solution loaded.";
        this._progressBar.Visible = false;
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private bool CheckSolutionSave()
    {
      if (!this._SolutionManager.IsSolutionChanged)
        return true;

      DialogResult dr = MessageBox.Show(this, "Solution " + this._SolutionManager.Solution + " changes not saved. Do you want to save the changes?", "File Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
      if (dr == DialogResult.Cancel)
        return false;

      if (dr == DialogResult.Yes)
      {
        if (this.SaveSolution(false, this._SolutionManager.Solution) == false)
          return false;
      }
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    private bool CheckProjectSave(Project project)
    {
      if (project.Changed == false)
        return true;

      DialogResult dr = MessageBox.Show(this, "Project " + project.Name + " changes not saved. Do you want to save the changes?", "File Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
      if (dr == DialogResult.Cancel)
        return false;

      if (dr == DialogResult.Yes)
      {
        if (this.SaveProject(false, project) == false)
          return false;
      }
      return true;
    }  
    /// <summary>
    /// 
    /// </summary>
    private bool CheckSave()
    {
      if (this.CheckSolutionSave() == false)
        return false;

      foreach (Project project in this._SolutionManager.ChangedProjects)
      {
        if (this.CheckProjectSave(project) == false)
          return false;
      }

      return true;
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    private void SetCurrentFrame(AnimationFrame currentFrame)
    {
      this._OutputSprite.SetSelectedAnimationFrames(currentFrame);
      this._SolutionManager.SelectedFrame = currentFrame;
      this._OutputSprite.Refresh();
    }

    #region Playback events
    /// <summary>
    /// 
    /// </summary>
    private void _Playback_CurrentFrameChanged(object sender, EventArgs e)
    {
      try
      {
        this.SetCurrentFrame(this._Playback.CurrentFrame);
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
    private void _Playback_FramesPerSecondChanged(object sender, EventArgs e)
    {
      try
      {
        if (this._SolutionManager.SelectedAnimation == null)
          return;
        //this._SolutionManager.SelectedAnimation.FramesPerSecond = this._Playback.FramesPerSecond;
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
    #endregion

    #region FrameList events
    /// <summary>
    /// 
    /// </summary>
    private void _FrameList_DoubleClick(object sender, EventArgs e)
    {
      try
      {
        if (this._FrameList.SelectedAnimationFrames.Count <= 0)
          return;

        if (Settings.FormAddNewFramesForm.ShowEditDialog(this, this._FrameList.SelectedAnimationFrames[0])
           == DialogResult.OK)
        {
        }
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
    private void _FrameList_SelectedAnimationFrameChanged(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.SetSelectedAnimationFrames(this._FrameList.SelectedAnimationFrames);
        if (this._FrameList.SelectedAnimationFrames.Count > 0)
        {
          this._SolutionManager.SelectedFrame = this._FrameList.SelectedAnimationFrames[0];
        }
        this._OutputSprite.Refresh();
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
    private void _FrameList_FrameDoubleClicked(object sender, AnimationFrame frame)
    {
      try
      {
        this.EditFrame(frame);
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
    private void _FrameList_AddFramesToAnimationClicked(object sender, EventArgs e)
    {
      try
      {
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
    #endregion

    #region SolutionManager Events

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_CtxMenuAddExistingProjectClicked(object sender, EventArgs e)
    {
      try
      {
        this.AddExistingProject();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_CtxMenuAddNewProjectClicked(object sender, EventArgs e)
    {
      try
      {
        this.AddNewProject();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_AnimationRenderFrameNrsChanged(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_SelectedProjectChanged(object sender, EventArgs e)
    {
      try
      {
        this._FrameList.Project = this._SolutionManager.SelectedProject;
        this._OutputSprite.Project = this._SolutionManager.SelectedProject;
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
    private void _SolutionManager_SelectedAnimationChanged(object sender, EventArgs e)
    {
      try
      {
        this._FrameList.Animation = this._SolutionManager.SelectedAnimation;
        this._OutputSprite.Animation = this._SolutionManager.SelectedAnimation;
        this._Playback.Animation = this._SolutionManager.SelectedAnimation;

        if (this._SolutionManager.SelectedAnimation != null)
        {
          this._OutputSprite.Grid = this._SolutionManager.SelectedAnimation.Grid;
        }
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
    private void _SolutionManager_FrameAddedToAnimation(object sender, AnimationFrame frame)
    {
      try
      {
        this._FrameList.AddAnimationFrame(frame);
        this._OutputSprite.Refresh();
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
    private void _SolutionManager_FrameAttributeChanged(object sender, AnimationFrame frame)
    {
      try
      {
        this._FrameList.RefreshAnimationFrame(frame);
        this._Playback.Refresh();
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
    private void _SolutionManager_ProjectOutputSpriteChanged(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.Refresh();
        this._Playback.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_AnimationFramesPerSecondChanged(object sender, Animation animation)
    {
      try
      {
        this._Playback.FramesPerSecond = animation.FramesPerSecond;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_AnimationParentAnimationChanged(object sender, Animation animation)
    {
      try
      {
         this._Playback.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_SolutionFilenameChanged(object sender, EventArgs e)
    {
      try
      {
        this.RefreshStatusBar();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_SolutionNameChanged(object sender, EventArgs e)
    {
      try
      {
        this.RefreshStatusBar();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_CtxMenuCloseSolutionClicked(object sender, EventArgs e)
    {
      try
      {
        this.CloseSolution();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_SelectedFrameChanged(object sender, EventArgs e)
    {
      try
      {
        this._FrameList.SelectFrame(this._SolutionManager.SelectedFrame, false);
        
        this._Playback.CurrentFrame = this._SolutionManager.SelectedFrame;
        this._Playback.RefreshFrame();
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
    private void _SolutionManager_FrameMovedDown(object sender, AnimationFrame frame)
    {
      try
      {
        this._FrameList.MoveAnimationFrameDown(frame);
        this._StatusText.Text = "";
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
    private void _SolutionManager_FrameMovedUp(object sender, AnimationFrame frame)
    {
      try
      {
        this._FrameList.MoveAnimationFrameUp(frame);
        this._StatusText.Text = "";
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
    private void _SolutionManager_CtxMenuRemoveProjecClicked(object sender, EventArgs e)
    {
      try
      {
        this.RemoveProject();
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
    private void _SolutionManager_FramesRemoved(object sender, List<AnimationFrame> frameList)
    {
      try
      {
        if (frameList.Count == 0)
          return;

        this._FrameList.RemoveAnimationFrames(frameList);
        this._OutputSprite.Refresh();
        this._Playback.FrameRemoved(frameList[0]); 
        this._FrameList.SelectFrame(this._SolutionManager.SelectedFrame, false);
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
    private void _SolutionManager_CtxMenuDelAnimationClicked(object sender, EventArgs e)
    {
      try
      {
        this.RemoveAnimation();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_ProjectFrameNrsColorChanged(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_ProjectAttributeChanged(object sender, EventArgs e)
    {
      try
      {
        this._SolutionManager.RefreshProps();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_ProjectFrameRectangleColorChanged(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_ProjectSpriteBackColorChanged(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.Refresh();
        this._Playback.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    #endregion

    #region OutputSprite events
   
    /// <summary>
    ///  
    /// </summary>
    private void _OutputSprite_AnimationFrameClicked(object sender, AnimationFrame animationFrame)
    {
      try
      {
//        this._FrameList.SelectFrame(animationFrame, (Control.ModifierKeys & Keys.Control) == Keys.Control);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OutputSprite_SelectedFrameChanged(object sender, EventArgs e)
    {
      try
      {
        this._FrameList.SelectFrames(this._OutputSprite.SelectedAnimationFrames);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OutputSprite_GridChanged(object sender, EventArgs e)
    {
      try
      {
        if (this._SolutionManager.SelectedProject == null)
          return;

        this._SolutionManager.SelectedAnimation.Grid = this._OutputSprite.Grid;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
    #endregion

    #region Solution related methods

    /// <summary>
    /// 
    /// </summary>
    private void CloseSolution()
    {
      if (this.CheckSave() == false)
        return;

      this._SolutionManager.CloseSolution();
      this.EnableControls();
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateNewSolution()
    {
      this._DlgSave.Filter = Settings.SOLUTION_FILE_FILTER;
      if (string.IsNullOrEmpty(Settings.Instance.LastSolutionFolder) == false)
        Directory.SetCurrentDirectory(Settings.Instance.LastSolutionFolder);

      this._DlgSave.FileName = Goodies.GetNextFilename(Settings.DEFAULT_SOLUTION_FILENAME);
      this._DlgSave.InitialDirectory = Settings.Instance.LastSolutionFolder;
      if (this._DlgSave.ShowDialog(this) == DialogResult.Cancel)
        return;

      Solution solution = new Solution();
      solution.Name = Path.GetFileNameWithoutExtension(this._DlgSave.FileName);
      solution.Save(this._DlgSave.FileName);

      this._SolutionManager.Clear();
      this._SolutionManager.AddSolution(solution);
      Settings.Instance.LastSolutionFolder = Path.GetDirectoryName(this._DlgSave.FileName);
      Settings.Instance.AddRecentFile(this._DlgSave.FileName);
      this.EnableControls();
      this._StatusText.Text = "Solution created.";
    }

    /// <summary>
    /// 
    /// </summary>
    private void AddNewSolution()
    {
      if (this.CheckSave())
      {
        this._SolutionManager.CloseSolution();
        this.CreateNewSolution();
      }
      this.EnableControls();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_SolutionClosed(object sender, EventArgs e)
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
    private void AddExistingProject()
    {
      this._DlgOpen.Filter = Settings.PROJECT_FILE_FILTER;
      this._DlgOpen.InitialDirectory = Settings.Instance.LastProjectFolder;
      if (this._DlgOpen.ShowDialog(this) == DialogResult.Cancel)
        return;
      Project project = new Project(this._SolutionManager.Solution, this._DlgOpen.FileName);
      project.Reload();
      this._SolutionManager.AddProject(project);
      Settings.Instance.LastProjectFolder = Path.GetDirectoryName(this._DlgOpen.FileName);
      this.EnableControls();
      this._StatusText.Text = "Project " + project.Name + " added to the solution";
    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowTileTester()
    {
      // At this moment edit the first TileTest on the solution. If it does not exits,
      // create a new one. Multiple TileTests on the same solution will be supported
      TileTest tileTest;
      if (this._SolutionManager.Solution.TileTestList.Count == 0)
        tileTest = this._SolutionManager.CreateNewTileTest("NONAME");
      else
        tileTest = this._SolutionManager.Solution.TileTestList[0];
      Settings.TileTesterForm.ShowDialog(this, tileTest);
    }
    #endregion

    #region Project related methods

    /// <summary>
    /// 
    /// </summary>
    private void SelectImage()
    {
      if (this._SolutionManager.SelectedProject == null)
        return;
      /*
      OpenFileDialog dlg = new OpenFileDialog();
      dlg.Filter = Settings.IMAGE_FILER;
      dlg.InitialDirectory = Settings.Instance.LastImportFramesFolder;
      if (dlg.ShowDialog(this) == DialogResult.Cancel)
      {
        return;
      }
      Settings.Instance.LastImportFramesFolder = Path.GetDirectoryName(dlg.FileName);
      this._SolutionManager.SelectedProject.SetSprite(dlg.FileName);*/
      this._SolutionManager.SelectProjectImage(this._SolutionManager.SelectedProject);

    }

    /// <summary>
    /// 
    /// </summary>
    private void AddFramesFromFiles()
    {
      this._Playback.Stop();
      this._DlgOpen.Filter = Settings.FRAMES_FILE_FILTER;
      this._DlgOpen.InitialDirectory = Settings.Instance.LastImportFramesFolder;
      Forms.GenerateImageFromFilesForm importForm = new GenerateImageFromFilesForm();
      if (importForm.ShowDialog(this, this._SolutionManager.SelectedProject) == DialogResult.OK)
      {

        this._SolutionManager.CreateSpriteFromFiles(importForm.FilenameList, 
                  importForm.FramesPerColumn);

        if (importForm.AutoGenerateFrames)
        {
          Animation animation = (importForm.AnimationToAffect == null? this.AddNewAnimation() : importForm.AnimationToAffect);
          int i = 0, j = 0;
          int frameWidth = importForm.FrameWidth;
          int frameHeight = importForm.FrameHeight;
          foreach (string s in importForm.FilenameList)
          {
            this._SolutionManager.AddFrameToAnimation(animation,
                  new Rectangle(i * frameWidth, j * frameHeight, frameWidth, frameHeight));
            i++;
            if (i >= importForm.FramesPerColumn)
            {
              j++;
              i = 0;
            }
          }
        }
      }
  
      this.EnableControls();
    }
  
    /// <summary>
    /// 
    /// </summary>
    private void AddNewProject()
    {
      if (string.IsNullOrEmpty(Settings.Instance.LastProjectFolder) == false)
        Directory.SetCurrentDirectory(Settings.Instance.LastProjectFolder);

      this._DlgSave.Filter = Settings.PROJECT_FILE_FILTER;
      this._DlgSave.InitialDirectory = Settings.Instance.LastProjectFolder;
      this._DlgSave.FileName = Goodies.GetNextFilename(Settings.DEFAULT_PROJECT_FILENAME);
      if (this._DlgSave.ShowDialog(this) == DialogResult.Cancel)
        return;

      Project newProject = new Project();
      newProject.Name = Path.GetFileNameWithoutExtension(this._DlgSave.FileName);
      this._SolutionManager.AddProject(newProject);
      this.AddNewAnimation();
      newProject.Save(this._DlgSave.FileName);
      Settings.Instance.LastProjectFolder = Path.GetDirectoryName(this._DlgSave.FileName);
      this._StatusText.Text = "Project created and one default animation was added.";
      this.EnableControls();
    }

    /// <summary>
    /// 
    /// </summary>
    private void RemoveProject()
    {
      this._SolutionManager.DeleteProject();
      this.EnableControls();
      this._StatusText.Text = "Project removed.";
    }

  

    /// <summary>
    /// 
    /// </summary>
    private void MoveFrameUp()
    {
      if (this._FrameList.HasAnimationFramesSelected == false)
        return;
      if (this._FrameList.HasSingleFrameAnimationSelected == false)
      {
        MessageBox.Show(this, "Can only move one frame at a time.", Settings.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      this._SolutionManager.MoveFrameUp(this._FrameList.SelectedAnimationFrames[0]);
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveFrameDown()
    {

      if (this._FrameList.HasAnimationFramesSelected == false)
        return;
      if (this._FrameList.HasSingleFrameAnimationSelected == false)
      {
        MessageBox.Show(this, "Can only move one frame at a time.", Settings.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      this._SolutionManager.MoveFrameDown(this._FrameList.SelectedAnimationFrames[0]);
    }


    /// <summary>
    /// 
    /// </summary>
    private void _FrameList_AddFramesFromFilesClicked(object sender, EventArgs e)
    {
      try
      {
        this.AddFramesFromFiles();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_CtxMenuAddFramesToProjectClicked(object sender, EventArgs e)
    {
      try
      {
        this.AddFramesFromFiles();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_CtxMenuAddNewAnimClicked(object sender, EventArgs e)
    {
      try
      {
        this.AddNewAnimation();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RefreshImage()
    {
      try
      {
        this._SolutionManager.RefreshProjectImage(this._SolutionManager.SelectedProject);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void ShowResolutionMappers()
    {
      ResolutionMappersForm mappers = new ResolutionMappersForm();
      mappers.ShowDialog(this, this._SolutionManager.SelectedProject);
    }
    #endregion

    #region Animation related methods
    /// <summary>
    /// 
    /// </summary>
    private void AddFrames()
    {
      if (Settings.FormAddNewFramesForm.ShowAddDialog(this, this._SolutionManager.SelectedAnimation) == DialogResult.Cancel)
      {
        return;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private Animation AddNewAnimation()
    {
      Animation animation = new Animation();
      this._SolutionManager.AddAnimation(this._SolutionManager.SelectedProject, animation);
      this.EnableControls();

      return animation;
    }

    /// <summary>
    /// 
    /// </summary>
    private void RemoveAnimation()
    {
      this._SolutionManager.DeleteAnimation();
      this.EnableControls();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _FrameList_DeleteAnimationFramesClicked(object sender, EventArgs e)
    {
      try
      {
        this.RemoveSelectedAnimationFrames();
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
    private void EditColisionZones()
    {
      if (Settings.FormColisionZones.ShowDialog(this, this._SolutionManager.SelectedAnimation) == DialogResult.OK)
      {
        this._SolutionManager.SelectedAnimation.ColisionZones = Settings.FormColisionZones.ColisionZones;
      }
    }

    #endregion

    #region Frames related methods
    /// <summary>
    /// 
    /// </summary>
    private void EditFrame(AnimationFrame frame)
    {
      if (frame == null)
            return;

      if (Settings.FormAddNewFramesForm.ShowEditDialog(this, frame) == DialogResult.OK)
      {
        frame.Rectangle = Settings.FormAddNewFramesForm.FrameRectangle;
        this._FrameList.RefreshAnimationFrame(frame);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RemoveSelectedAnimationFrames()
    {
      this._SolutionManager.RemoveFrames(this._FrameList.SelectedAnimationFrames);
    }

    #endregion

    #region Settings events
    /// <summary>
    /// 
    /// </summary>
    void Instance_ShowImagesChanged(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.ShowImages = Settings.Instance.ShowImages;
        this._Playback.ShowImages = Settings.Instance.ShowImages;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
    #endregion

    #region Miscelaneous methods
    /// <summary>
    /// 
    /// </summary>
    void _OptRecentFilesFile_Click(object sender, EventArgs e)
    {
      try
      {
        if (sender as ToolStripItem == null)
          return;

        this.OpenSolution(((ToolStripItem)sender).Text);
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Settings.Instance.RecentFiles.Remove(((ToolStripItem)sender).Text);
        Diag.ShowException(this, ex);
      }
    }
    /// <summary>
    /// 
    /// </summary>
    void EnableControls()
    {
      this._OptGridVisible.Checked = this._OutputSprite.Grid.Visible;
      this._PanelSolution.Visible = this._SolutionManager.SolutionLoaded;
      this._PanelProject.Visible = (this._SolutionManager.SelectedProject != null);

      // File menu
      this._OptSave.Enabled = (this._SolutionManager.SelectedNodeType != SpriteAnimationEditor.Controls.SolutionManager.NodeType.None);
      this._OptSaveAs.Enabled = (this._SolutionManager.SelectedNodeType != SpriteAnimationEditor.Controls.SolutionManager.NodeType.None);
      this._OptSaveAll.Enabled = (this._SolutionManager.SelectedNodeType != SpriteAnimationEditor.Controls.SolutionManager.NodeType.None);
      this._OptCloseSolution.Enabled = this._SolutionManager.SolutionLoaded;
      this._OptRecentFiles.Enabled = (Settings.Instance.RecentFiles.Count > 0);

      // Solution menu
      this._MenuSolution.Enabled = this._SolutionManager.SolutionLoaded;
      this._OptRemoveProject.Enabled = (this._SolutionManager.SelectedProject != null);

      // Project menu
      this._MenuProj.Enabled = (this._SolutionManager.SelectedProject != null);
      this._OptRemoveAnimation.Enabled = (this._SolutionManager.SelectedAnimation != null);
      this._OptAddAnimation.Enabled = (this._SolutionManager.SelectedProject != null);
      this._OptAddFrames.Enabled = (this._SolutionManager.SelectedProject != null);
      this._OptAddFrames.Enabled = (this._SolutionManager.SelectedProject != null && this._SolutionManager.SelectedProject.WithSprite);

      // Animation menu
      this._MenuAnimation.Enabled = (this._SolutionManager.SelectedAnimation != null);
      this._OptRemoveAnimFrames.Enabled = (this._FrameList.HasAnimationFramesSelected);
      this._OptEditFrame.Enabled = (this._FrameList.HasAnimationFramesSelected);
      this._OptArrangeAnimFrames.Enabled = (this._FrameList.HasAnimationFramesSelected);
      this._OptMoveAnimFrameUp.Enabled = (this._FrameList.IsFirstAnimFrameSelected == false);
      this._OptMoveAnimFrameDown.Enabled = (this._FrameList.IsLastAnimFrameSelected == false);
      this._OptSelAllAnimationFrames.Enabled = (this._FrameList.HasAnimationFrames);
      this._OptAddNewFrame.Enabled = (this._SolutionManager.SelectedAnimation != null && 
                                      this._SolutionManager.SelectedProject != null &&
                                      this._SolutionManager.SelectedProject.WithSprite);

      // View menu
      this._mnuGrid.Enabled = (this._SolutionManager.SelectedProject != null && this._SolutionManager.SelectedProject.WithSprite);
      this._mnuZoom.Enabled = (this._SolutionManager.SelectedProject != null && this._SolutionManager.SelectedProject.WithSprite);
    }

    
    /// <summary>
    /// 
    /// </summary>
    private void RefreshStatusBar()
    {
      if (this._SolutionManager.Solution == null)
      {
        this._LblSolutionFile.Text = "";
        return;
      }
      this._LblSolutionFile.Text = "Solution: " + this._SolutionManager.Solution.Name + "(" + this._SolutionManager.Solution.Filename + ")";
    }
    #endregion

    #region File menu
    /// <summary>
    /// 
    /// </summary>
    private void _MenuFile_DropDownOpening(object sender, EventArgs e)
    {
      try
      {
        switch (this._SolutionManager.SelectedNodeType)
        {
          case SpriteAnimationEditor.Controls.SolutionManager.NodeType.Solution:
            this._OptSave.Text = "Save " + Path.GetFileName(this._SolutionManager.Solution.Filename);
            this._OptSaveAs.Text = "Save " + Path.GetFileName(this._SolutionManager.Solution.Filename) + " As...";
            break;

          case SpriteAnimationEditor.Controls.SolutionManager.NodeType.Project:
          case SpriteAnimationEditor.Controls.SolutionManager.NodeType.Animation:
            this._OptSave.Text = "Save " + Path.GetFileName(this._SolutionManager.SelectedProject.Filename);
            this._OptSaveAs.Text = "Save " + Path.GetFileName(this._SolutionManager.SelectedProject.Filename) + " As...";
            break;

        }

        this._OptRecentFiles.DropDownItems.Clear();
        foreach (string filename in Settings.Instance.RecentFiles)
        {
          ToolStripItem item = this._OptRecentFiles.DropDownItems.Add(filename);
          item.Click += new EventHandler(this._OptRecentFilesFile_Click);
        }

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
    private void _OptNewSolution_Click(object sender, EventArgs e)
    {
      try
      {
        this.AddNewSolution();
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
    private void _OptOpenSolution_Click(object sender, EventArgs e)
    {
      try
      {
        this.OpenSolution(null);
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
    private void _OptCloseSolution_Click(object sender, EventArgs e)
    {
      try
      {
        this.CloseSolution();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private void _OptSaveAll_Click(object sender, EventArgs e)
    {
      try
      {
        this.SaveAll(false);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private void _OptSave_Click(object sender, EventArgs e)
    {
      try
      {
        this.Save(false);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private void _OptSaveAs_Click(object sender, EventArgs e)
    {
      try
      {
        this.Save(true);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptPreferences_Click(object sender, EventArgs e)
    {
      try
      {
        this._PreferencesForm.Show();
        this._PreferencesForm.Focus();

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
    private void _OptExit_Click(object sender, EventArgs e)
    {
      try
      {
        this._Playback.Stop();
        this.Close();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
    #endregion

    #region View menu
    /// <summary>
    /// 
    /// </summary>
    private void SetOutputSpriteZoom(ZoomFactor zoom)
    {
      this._OutputSprite.Zoom = zoom;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ZoomOutputSprite(bool zoomIn)
    {
      if (zoomIn)
      {
        this._OutputSprite.ZoomIn();
      }
      else
      {
        this._OutputSprite.ZoomOut();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptZoomIn_Click(object sender, EventArgs e)
    {
      try
      {
        this.ZoomOutputSprite(true);
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
    private void _OptZoomOut_Click(object sender, EventArgs e)
    {
      try
      {
        this.ZoomOutputSprite(false);
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
    private void _mnuZoom_ZoomSelected(object sender, ZoomFactor zoom)
    {
      try
      {
        this.SetOutputSpriteZoom(zoom);
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
    private void _OptGridVisible_Click(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.GridVisible = this._OptGridVisible.Checked;
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
    private void _OptGridSize_Click(object sender, EventArgs e)
    {
      try
      {
        this._OutputSprite.EditGridSize();
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
    private void _mnuView_DropDownOpening(object sender, EventArgs e)
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
    #endregion
    
    #region Solution menu
    /// <summary>
    /// 
    /// </summary>
    private void _MenuSolution_DropDownOpening(object sender, EventArgs e)
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
    private void _OptAddNewProject_Click(object sender, EventArgs e)
    {
      try
      {
        this.AddNewProject();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptRemoveProject_Click(object sender, EventArgs e)
    {
      try
      {
        this.RemoveProject();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private void _OptAddExistingProject_Click(object sender, EventArgs e)
    {
      try
      {
        this.AddExistingProject();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptTileTester_Click(object sender, EventArgs e)
    {
      try
      {
        this.ShowTileTester();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }

    }

    #endregion

    #region Project menu
    /// <summary>
    /// 
    /// </summary>
    private void _MenuProj_DropDownOpening(object sender, EventArgs e)
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
    private void _OptAddAnimation_Click(object sender, EventArgs e)
    {
      try
      {
        this.AddNewAnimation();
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
    private void _OptRemoveAnimation_Click(object sender, EventArgs e)
    {
      try
      {
        this.RemoveAnimation();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptSelectSprite_Click(object sender, EventArgs e)
    {
      try
      {
        this.SelectImage();
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
    private void _OptAddFrames_Click(object sender, EventArgs e)
    {
      try
      {
        this.AddFramesFromFiles();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptRefreshImage_Click(object sender, EventArgs e)
    {
      try
      {
        this.RefreshImage();
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
    private void _optResolutionMapping_Click(object sender, EventArgs e)
    {
      try
      {
        this.ShowResolutionMappers();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }

    }

    #endregion

    #region Animation menu
    /// <summary>
    /// 
    /// </summary>
    private void _MenuAnimation_DropDownOpening(object sender, EventArgs e)
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
    private void _OptAddNewFrame_Click(object sender, EventArgs e)
    {
      try
      {
        this.AddFrames();
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
    private void _OptRemoveAnimFrames_Click(object sender, EventArgs e)
    {
      try
      {
        this.RemoveSelectedAnimationFrames();
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
    private void _OptSelAllAnimationFrames_Click(object sender, EventArgs e)
    {
      try
      {
        this._FrameList.SelectAllAnimationFrames();
        this._FrameList.AnimationFocus();
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
    private void _OptMoveAnimFrameUp_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveFrameUp();
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
    private void _OptMoveAnimFrameDown_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveFrameDown();
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
    private void _OptEditFrame_Click(object sender, EventArgs e)
    {
      try
      {
        this.EditFrame(this._SolutionManager.SelectedFrame);
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
    private void _OptEditColisionZones_Click(object sender, EventArgs e)
    {
      try
      {
        this.EditColisionZones();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }

    }

    #endregion

    #region IViewState Members
    /// <summary>
    /// 
    /// </summary>
    public void InitFromXml(System.Xml.XmlElement elemParent)
    {
      XmlElement elem = (XmlElement)elemParent.SelectSingleNode(this.Name);
      if (elem == null)
        return;

      this.Left = XmlHelper.GetAttribute(elem, "Left", 100);
      this.Top = XmlHelper.GetAttribute(elem, "Top", 100);
      this.Width = XmlHelper.GetAttribute(elem, "Width", 300);
      this.Height = XmlHelper.GetAttribute(elem, "Height", 300);

      if (this.Width < 300)
        this.Width = 300;
      if (this.Height < 300)
        this.Height = 300;

      if (this.Left < 0)
          this.Left = 100;
      if (this.Top < 0)
          this.Top = 100;

      this._OutputSprite.Zoom = XmlHelper.GetAttribute(elem, "Zoom", ZoomFactor.Normal);

      this._SolutionManager.Width = XmlHelper.GetAttribute(elem, "SolutionManagerWidth", 200);
      this._SolutionManager.SolutionHeight = XmlHelper.GetAttribute(elem, "SolutionExplorerHeight", 200);
      this._FrameList.Height = XmlHelper.GetAttribute(elem, "FrameListHeight", 170);
      this._OutputSprite.Height = XmlHelper.GetAttribute(elem, "OuputSpriteHeight", 170);

      this.WindowState = (FormWindowState)(XmlHelper.GetAttribute(elem, "State", (int)FormWindowState.Normal));

      this._OutputSprite.Grid.InitFromXml(elem);
    }

    /// <summary>
    /// 
    /// </summary>
    public System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      XmlElement xmlElement = xmlDoc.CreateElement(this.Name);
      xmlElement.SetAttribute("Left", this.Left.ToString());
      xmlElement.SetAttribute("Top", this.Top.ToString());
      xmlElement.SetAttribute("Width", this.Width.ToString());
      xmlElement.SetAttribute("Height", this.Height.ToString());
      xmlElement.SetAttribute("Zoom", this._OutputSprite.Zoom.ToString());
      xmlElement.SetAttribute("SolutionManagerWidth", this._SolutionManager.Width.ToString());
      xmlElement.SetAttribute("SolutionExplorerHeight", this._SolutionManager.SolutionHeight.ToString());
      xmlElement.SetAttribute("FrameListHeight", this._FrameList.Height.ToString());
      xmlElement.SetAttribute("OuputSpriteHeight", this._OutputSprite.Height.ToString());
      xmlElement.SetAttribute("State", ((int) this.WindowState).ToString());

      xmlElement.AppendChild(this._OutputSprite.Grid.ToXml(xmlDoc));
      return xmlElement;
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
	private void _SolutionManager_EditFrameCollisionZonesClicked(object sender, AnimationFrame frame)
	{
		try
		{
			this.EditFrameCollisionZones(frame);
		}
		catch (System.Exception ex)
		{
			Diag.ShowException(this, ex);
		}
	}

	  /// <summary>
	  /// 
	  /// </summary>
	private void EditFrameCollisionZones(AnimationFrame frame)
	{
		if (frame == null)
		{
			return;
		}

		if (Settings.FormColisionZones.ShowDialog(this, frame) == DialogResult.OK)
		  {
			  frame.ColisionZones = Settings.FormColisionZones.ColisionZones;
		  }
	}

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_ProjectLoaded(object sender, Project project)
    {
     
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_FramesRemoved(object sender)
    {
     
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionManager_SolutionDataLoaded(object sender, Solution solution)
    {
      try
      {
        this._progressBar.Value = 0;
        this._progressBar.Maximum = solution.ProjectList.Count;
        this._StatusText.Text = "Loading solution.";
        this._progressBar.Visible = true;
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
    private void _SolutionManager_ProjectLoadStarted(object sender, Project project)
    {
      try
      {
        this._StatusText.Text = "Loading " + project.Name + "...";
        this._progressBar.Value++;
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
    private void _optMoveFrames_Click(object sender, EventArgs e)
    {
      try
      {
        if (this._SolutionManager.SelectedAnimation == null)
        {
          return;
        }
        MoveFramesForm form = new MoveFramesForm();
        if (form.ShowDialog(this._SolutionManager.SelectedAnimation, this._FrameList.SelectedAnimationFrames) ==
          DialogResult.OK)
        {
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
    public void RefreshFrames()
    {
      this._OutputSprite.Refresh();
      this._FrameList.RefreshAnimationFrames();
    }
  }
}
