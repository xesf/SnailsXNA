namespace SpriteAnimationEditor.Forms
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      SpriteAnimationEditor.Grid grid2 = new SpriteAnimationEditor.Grid();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this._MenuFile = new System.Windows.Forms.ToolStripMenuItem();
      this._OptNewSolution = new System.Windows.Forms.ToolStripMenuItem();
      this._OptOpenSolution = new System.Windows.Forms.ToolStripMenuItem();
      this._OptCloseSolution = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this._OptSaveAll = new System.Windows.Forms.ToolStripMenuItem();
      this._OptSave = new System.Windows.Forms.ToolStripMenuItem();
      this._OptSaveAs = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this._OptPreferences = new System.Windows.Forms.ToolStripMenuItem();
      this._OptRecentFilesSep1 = new System.Windows.Forms.ToolStripSeparator();
      this._OptRecentFiles = new System.Windows.Forms.ToolStripMenuItem();
      this._OptRecentFilesSep2 = new System.Windows.Forms.ToolStripSeparator();
      this._OptExit = new System.Windows.Forms.ToolStripMenuItem();
      this._mnuView = new System.Windows.Forms.ToolStripMenuItem();
      this._mnuGrid = new System.Windows.Forms.ToolStripMenuItem();
      this._OptGridVisible = new System.Windows.Forms.ToolStripMenuItem();
      this._OptGridSize = new System.Windows.Forms.ToolStripMenuItem();
      this._MenuSolution = new System.Windows.Forms.ToolStripMenuItem();
      this._OptAddNewProject = new System.Windows.Forms.ToolStripMenuItem();
      this._OptAddExistingProject = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
      this._OptRemoveProject = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this._OptTileTester = new System.Windows.Forms.ToolStripMenuItem();
      this._MenuProj = new System.Windows.Forms.ToolStripMenuItem();
      this._OptAddAnimation = new System.Windows.Forms.ToolStripMenuItem();
      this._OptRemoveAnimation = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this._OptSelectSprite = new System.Windows.Forms.ToolStripMenuItem();
      this._OptAddFrames = new System.Windows.Forms.ToolStripMenuItem();
      this._OptRefreshImage = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this._optResolutionMapping = new System.Windows.Forms.ToolStripMenuItem();
      this._MenuAnimation = new System.Windows.Forms.ToolStripMenuItem();
      this._OptAddNewFrame = new System.Windows.Forms.ToolStripMenuItem();
      this._OptEditFrame = new System.Windows.Forms.ToolStripMenuItem();
      this._OptRemoveAnimFrames = new System.Windows.Forms.ToolStripMenuItem();
      this._OptSelAllAnimationFrames = new System.Windows.Forms.ToolStripMenuItem();
      this._OptArrangeAnimFrames = new System.Windows.Forms.ToolStripMenuItem();
      this._OptMoveAnimFrameUp = new System.Windows.Forms.ToolStripMenuItem();
      this._OptMoveAnimFrameDown = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this._OptEditColisionZones = new System.Windows.Forms.ToolStripMenuItem();
      this._DlgOpen = new System.Windows.Forms.OpenFileDialog();
      this._DlgSave = new System.Windows.Forms.SaveFileDialog();
      this._ColorPicker = new System.Windows.Forms.ColorDialog();
      this._DlgOpenAnimation = new System.Windows.Forms.OpenFileDialog();
      this._PanelSolution = new System.Windows.Forms.Panel();
      this._PanelProject = new System.Windows.Forms.Panel();
      this._OutputSprite = new SpriteAnimationEditor.Controls.OutputSprite();
      this._SplitterOutputSprite = new System.Windows.Forms.Splitter();
      this._Playback = new SpriteAnimationEditor.Controls.AnimationPlayback();
      this._SplitterFrameList = new System.Windows.Forms.Splitter();
      this._FrameList = new SpriteAnimationEditor.Controls.FrameList();
      this._SplitterSolution = new System.Windows.Forms.Splitter();
      this._SolutionManager = new SpriteAnimationEditor.Controls.SolutionManager();
      this._StatusBar = new System.Windows.Forms.StatusStrip();
      this._progressBar = new System.Windows.Forms.ToolStripProgressBar();
      this._StatusText = new System.Windows.Forms.ToolStripStatusLabel();
      this._LblSolutionFile = new System.Windows.Forms.ToolStripStatusLabel();
      this._optMoveFrames = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1.SuspendLayout();
      this._PanelSolution.SuspendLayout();
      this._PanelProject.SuspendLayout();
      this._StatusBar.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuFile,
            this._mnuView,
            this._MenuSolution,
            this._MenuProj,
            this._MenuAnimation});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(916, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // _MenuFile
      // 
      this._MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptNewSolution,
            this._OptOpenSolution,
            this._OptCloseSolution,
            this.toolStripSeparator3,
            this._OptSaveAll,
            this._OptSave,
            this._OptSaveAs,
            this.toolStripSeparator1,
            this._OptPreferences,
            this._OptRecentFilesSep1,
            this._OptRecentFiles,
            this._OptRecentFilesSep2,
            this._OptExit});
      this._MenuFile.Name = "_MenuFile";
      this._MenuFile.Size = new System.Drawing.Size(37, 20);
      this._MenuFile.Text = "&File";
      this._MenuFile.DropDownOpening += new System.EventHandler(this._MenuFile_DropDownOpening);
      // 
      // _OptNewSolution
      // 
      this._OptNewSolution.Name = "_OptNewSolution";
      this._OptNewSolution.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
      this._OptNewSolution.Size = new System.Drawing.Size(202, 22);
      this._OptNewSolution.Text = "&New Solution...";
      this._OptNewSolution.Click += new System.EventHandler(this._OptNewSolution_Click);
      // 
      // _OptOpenSolution
      // 
      this._OptOpenSolution.Image = global::SpriteAnimationEditor.Properties.Resources.open;
      this._OptOpenSolution.Name = "_OptOpenSolution";
      this._OptOpenSolution.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
      this._OptOpenSolution.Size = new System.Drawing.Size(202, 22);
      this._OptOpenSolution.Text = "&Open Solution...";
      this._OptOpenSolution.Click += new System.EventHandler(this._OptOpenSolution_Click);
      // 
      // _OptCloseSolution
      // 
      this._OptCloseSolution.Image = global::SpriteAnimationEditor.Properties.Resources.close_solution;
      this._OptCloseSolution.Name = "_OptCloseSolution";
      this._OptCloseSolution.Size = new System.Drawing.Size(202, 22);
      this._OptCloseSolution.Text = "Close Solution";
      this._OptCloseSolution.Click += new System.EventHandler(this._OptCloseSolution_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(199, 6);
      // 
      // _OptSaveAll
      // 
      this._OptSaveAll.Image = global::SpriteAnimationEditor.Properties.Resources.saveall;
      this._OptSaveAll.Name = "_OptSaveAll";
      this._OptSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
      this._OptSaveAll.Size = new System.Drawing.Size(202, 22);
      this._OptSaveAll.Text = "Save All";
      this._OptSaveAll.Click += new System.EventHandler(this._OptSaveAll_Click);
      // 
      // _OptSave
      // 
      this._OptSave.Image = global::SpriteAnimationEditor.Properties.Resources.save;
      this._OptSave.Name = "_OptSave";
      this._OptSave.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                  | System.Windows.Forms.Keys.S)));
      this._OptSave.Size = new System.Drawing.Size(202, 22);
      this._OptSave.Text = "&Save";
      this._OptSave.Click += new System.EventHandler(this._OptSave_Click);
      // 
      // _OptSaveAs
      // 
      this._OptSaveAs.Name = "_OptSaveAs";
      this._OptSaveAs.Size = new System.Drawing.Size(202, 22);
      this._OptSaveAs.Text = "Save &As....";
      this._OptSaveAs.Click += new System.EventHandler(this._OptSaveAs_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
      // 
      // _OptPreferences
      // 
      this._OptPreferences.Name = "_OptPreferences";
      this._OptPreferences.Size = new System.Drawing.Size(202, 22);
      this._OptPreferences.Text = "Preferences...";
      this._OptPreferences.Click += new System.EventHandler(this._OptPreferences_Click);
      // 
      // _OptRecentFilesSep1
      // 
      this._OptRecentFilesSep1.Name = "_OptRecentFilesSep1";
      this._OptRecentFilesSep1.Size = new System.Drawing.Size(199, 6);
      // 
      // _OptRecentFiles
      // 
      this._OptRecentFiles.Name = "_OptRecentFiles";
      this._OptRecentFiles.Size = new System.Drawing.Size(202, 22);
      this._OptRecentFiles.Text = "Recent Files";
      // 
      // _OptRecentFilesSep2
      // 
      this._OptRecentFilesSep2.Name = "_OptRecentFilesSep2";
      this._OptRecentFilesSep2.Size = new System.Drawing.Size(199, 6);
      // 
      // _OptExit
      // 
      this._OptExit.Name = "_OptExit";
      this._OptExit.Size = new System.Drawing.Size(202, 22);
      this._OptExit.Text = "&Exit";
      this._OptExit.Click += new System.EventHandler(this._OptExit_Click);
      // 
      // _mnuView
      // 
      this._mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuGrid});
      this._mnuView.Name = "_mnuView";
      this._mnuView.Size = new System.Drawing.Size(44, 20);
      this._mnuView.Text = "View";
      this._mnuView.DropDownOpening += new System.EventHandler(this._mnuView_DropDownOpening);
      // 
      // _mnuGrid
      // 
      this._mnuGrid.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptGridVisible,
            this._OptGridSize});
      this._mnuGrid.Name = "_mnuGrid";
      this._mnuGrid.Size = new System.Drawing.Size(96, 22);
      this._mnuGrid.Text = "Grid";
      // 
      // _OptGridVisible
      // 
      this._OptGridVisible.CheckOnClick = true;
      this._OptGridVisible.Name = "_OptGridVisible";
      this._OptGridVisible.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
      this._OptGridVisible.Size = new System.Drawing.Size(150, 22);
      this._OptGridVisible.Text = "Visible";
      this._OptGridVisible.Click += new System.EventHandler(this._OptGridVisible_Click);
      // 
      // _OptGridSize
      // 
      this._OptGridSize.Name = "_OptGridSize";
      this._OptGridSize.Size = new System.Drawing.Size(150, 22);
      this._OptGridSize.Text = "Attributes...";
      this._OptGridSize.Click += new System.EventHandler(this._OptGridSize_Click);
      // 
      // _MenuSolution
      // 
      this._MenuSolution.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptAddNewProject,
            this._OptAddExistingProject,
            this.toolStripSeparator8,
            this._OptRemoveProject,
            this.toolStripSeparator4,
            this._OptTileTester});
      this._MenuSolution.Name = "_MenuSolution";
      this._MenuSolution.Size = new System.Drawing.Size(63, 20);
      this._MenuSolution.Text = "Solution";
      this._MenuSolution.DropDownOpening += new System.EventHandler(this._MenuSolution_DropDownOpening);
      // 
      // _OptAddNewProject
      // 
      this._OptAddNewProject.Image = global::SpriteAnimationEditor.Properties.Resources.new_project;
      this._OptAddNewProject.Name = "_OptAddNewProject";
      this._OptAddNewProject.Size = new System.Drawing.Size(200, 22);
      this._OptAddNewProject.Text = "Add New Sprite Set...";
      this._OptAddNewProject.Click += new System.EventHandler(this._OptAddNewProject_Click);
      // 
      // _OptAddExistingProject
      // 
      this._OptAddExistingProject.Name = "_OptAddExistingProject";
      this._OptAddExistingProject.Size = new System.Drawing.Size(200, 22);
      this._OptAddExistingProject.Text = "Add Existing Sprite Set...";
      this._OptAddExistingProject.Click += new System.EventHandler(this._OptAddExistingProject_Click);
      // 
      // toolStripSeparator8
      // 
      this.toolStripSeparator8.Name = "toolStripSeparator8";
      this.toolStripSeparator8.Size = new System.Drawing.Size(197, 6);
      // 
      // _OptRemoveProject
      // 
      this._OptRemoveProject.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._OptRemoveProject.Name = "_OptRemoveProject";
      this._OptRemoveProject.Size = new System.Drawing.Size(200, 22);
      this._OptRemoveProject.Text = "Remove Sprite Set";
      this._OptRemoveProject.Click += new System.EventHandler(this._OptRemoveProject_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(197, 6);
      // 
      // _OptTileTester
      // 
      this._OptTileTester.Name = "_OptTileTester";
      this._OptTileTester.Size = new System.Drawing.Size(200, 22);
      this._OptTileTester.Text = "Tile Tester...";
      this._OptTileTester.Click += new System.EventHandler(this._OptTileTester_Click);
      // 
      // _MenuProj
      // 
      this._MenuProj.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptAddAnimation,
            this._OptRemoveAnimation,
            this.toolStripSeparator6,
            this._OptSelectSprite,
            this._OptAddFrames,
            this._OptRefreshImage,
            this.toolStripSeparator5,
            this._optResolutionMapping});
      this._MenuProj.Name = "_MenuProj";
      this._MenuProj.Size = new System.Drawing.Size(68, 20);
      this._MenuProj.Text = "Sprite Set";
      this._MenuProj.DropDownOpening += new System.EventHandler(this._MenuProj_DropDownOpening);
      // 
      // _OptAddAnimation
      // 
      this._OptAddAnimation.Image = global::SpriteAnimationEditor.Properties.Resources.new_animattion;
      this._OptAddAnimation.Name = "_OptAddAnimation";
      this._OptAddAnimation.Size = new System.Drawing.Size(223, 22);
      this._OptAddAnimation.Text = "Add New Animation";
      this._OptAddAnimation.Click += new System.EventHandler(this._OptAddAnimation_Click);
      // 
      // _OptRemoveAnimation
      // 
      this._OptRemoveAnimation.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._OptRemoveAnimation.Name = "_OptRemoveAnimation";
      this._OptRemoveAnimation.Size = new System.Drawing.Size(223, 22);
      this._OptRemoveAnimation.Text = "Remove Animation";
      this._OptRemoveAnimation.Click += new System.EventHandler(this._OptRemoveAnimation_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(220, 6);
      // 
      // _OptSelectSprite
      // 
      this._OptSelectSprite.Name = "_OptSelectSprite";
      this._OptSelectSprite.Size = new System.Drawing.Size(223, 22);
      this._OptSelectSprite.Text = "Select Image...";
      this._OptSelectSprite.Click += new System.EventHandler(this._OptSelectSprite_Click);
      // 
      // _OptAddFrames
      // 
      this._OptAddFrames.Name = "_OptAddFrames";
      this._OptAddFrames.Size = new System.Drawing.Size(223, 22);
      this._OptAddFrames.Text = "Generate Image From Files...";
      this._OptAddFrames.Click += new System.EventHandler(this._OptAddFrames_Click);
      // 
      // _OptRefreshImage
      // 
      this._OptRefreshImage.Name = "_OptRefreshImage";
      this._OptRefreshImage.ShortcutKeys = System.Windows.Forms.Keys.F5;
      this._OptRefreshImage.Size = new System.Drawing.Size(223, 22);
      this._OptRefreshImage.Text = "Refresh Image";
      this._OptRefreshImage.Click += new System.EventHandler(this._OptRefreshImage_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(220, 6);
      // 
      // _optResolutionMapping
      // 
      this._optResolutionMapping.Name = "_optResolutionMapping";
      this._optResolutionMapping.Size = new System.Drawing.Size(223, 22);
      this._optResolutionMapping.Text = "Resolution Mapping...";
      this._optResolutionMapping.Click += new System.EventHandler(this._optResolutionMapping_Click);
      // 
      // _MenuAnimation
      // 
      this._MenuAnimation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptAddNewFrame,
            this._OptEditFrame,
            this._OptRemoveAnimFrames,
            this._OptSelAllAnimationFrames,
            this._OptArrangeAnimFrames,
            this._optMoveFrames,
            this.toolStripSeparator2,
            this._OptEditColisionZones});
      this._MenuAnimation.Name = "_MenuAnimation";
      this._MenuAnimation.Size = new System.Drawing.Size(75, 20);
      this._MenuAnimation.Text = "&Animation";
      this._MenuAnimation.DropDownOpening += new System.EventHandler(this._MenuAnimation_DropDownOpening);
      // 
      // _OptAddNewFrame
      // 
      this._OptAddNewFrame.Name = "_OptAddNewFrame";
      this._OptAddNewFrame.Size = new System.Drawing.Size(246, 22);
      this._OptAddNewFrame.Text = "Add New Frames...";
      this._OptAddNewFrame.Click += new System.EventHandler(this._OptAddNewFrame_Click);
      // 
      // _OptEditFrame
      // 
      this._OptEditFrame.Name = "_OptEditFrame";
      this._OptEditFrame.Size = new System.Drawing.Size(246, 22);
      this._OptEditFrame.Text = "Edit Frame...";
      this._OptEditFrame.Click += new System.EventHandler(this._OptEditFrame_Click);
      // 
      // _OptRemoveAnimFrames
      // 
      this._OptRemoveAnimFrames.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._OptRemoveAnimFrames.Name = "_OptRemoveAnimFrames";
      this._OptRemoveAnimFrames.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
      this._OptRemoveAnimFrames.Size = new System.Drawing.Size(246, 22);
      this._OptRemoveAnimFrames.Text = "Remove Selected Frames";
      this._OptRemoveAnimFrames.Click += new System.EventHandler(this._OptRemoveAnimFrames_Click);
      // 
      // _OptSelAllAnimationFrames
      // 
      this._OptSelAllAnimationFrames.Name = "_OptSelAllAnimationFrames";
      this._OptSelAllAnimationFrames.Size = new System.Drawing.Size(246, 22);
      this._OptSelAllAnimationFrames.Text = "Select All Frames";
      this._OptSelAllAnimationFrames.Click += new System.EventHandler(this._OptSelAllAnimationFrames_Click);
      // 
      // _OptArrangeAnimFrames
      // 
      this._OptArrangeAnimFrames.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptMoveAnimFrameUp,
            this._OptMoveAnimFrameDown});
      this._OptArrangeAnimFrames.Name = "_OptArrangeAnimFrames";
      this._OptArrangeAnimFrames.Size = new System.Drawing.Size(246, 22);
      this._OptArrangeAnimFrames.Text = "Arrange Frames";
      // 
      // _OptMoveAnimFrameUp
      // 
      this._OptMoveAnimFrameUp.Name = "_OptMoveAnimFrameUp";
      this._OptMoveAnimFrameUp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
      this._OptMoveAnimFrameUp.Size = new System.Drawing.Size(203, 22);
      this._OptMoveAnimFrameUp.Text = "Move Up";
      this._OptMoveAnimFrameUp.Click += new System.EventHandler(this._OptMoveAnimFrameUp_Click);
      // 
      // _OptMoveAnimFrameDown
      // 
      this._OptMoveAnimFrameDown.Name = "_OptMoveAnimFrameDown";
      this._OptMoveAnimFrameDown.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
      this._OptMoveAnimFrameDown.Size = new System.Drawing.Size(203, 22);
      this._OptMoveAnimFrameDown.Text = "Move Down";
      this._OptMoveAnimFrameDown.Click += new System.EventHandler(this._OptMoveAnimFrameDown_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(243, 6);
      // 
      // _OptEditColisionZones
      // 
      this._OptEditColisionZones.Name = "_OptEditColisionZones";
      this._OptEditColisionZones.Size = new System.Drawing.Size(246, 22);
      this._OptEditColisionZones.Text = "Edit Colision Zones...";
      this._OptEditColisionZones.Click += new System.EventHandler(this._OptEditColisionZones_Click);
      // 
      // _DlgOpen
      // 
      this._DlgOpen.DefaultExt = "png";
      this._DlgOpen.Filter = "PNG Files|*.png";
      this._DlgOpen.InitialDirectory = "c:\\temp\\a\\";
      this._DlgOpen.Multiselect = true;
      // 
      // _DlgSave
      // 
      this._DlgSave.DefaultExt = "sae";
      this._DlgSave.Filter = "Sprite Animation Editor File|*.sae";
      // 
      // _DlgOpenAnimation
      // 
      this._DlgOpenAnimation.Filter = "Sprite Animation Editor Files |*.sae";
      // 
      // _PanelSolution
      // 
      this._PanelSolution.Controls.Add(this._PanelProject);
      this._PanelSolution.Controls.Add(this._SplitterSolution);
      this._PanelSolution.Controls.Add(this._SolutionManager);
      this._PanelSolution.Controls.Add(this._StatusBar);
      this._PanelSolution.Dock = System.Windows.Forms.DockStyle.Fill;
      this._PanelSolution.Location = new System.Drawing.Point(0, 24);
      this._PanelSolution.Name = "_PanelSolution";
      this._PanelSolution.Size = new System.Drawing.Size(916, 531);
      this._PanelSolution.TabIndex = 14;
      // 
      // _PanelProject
      // 
      this._PanelProject.Controls.Add(this._OutputSprite);
      this._PanelProject.Controls.Add(this._SplitterOutputSprite);
      this._PanelProject.Controls.Add(this._Playback);
      this._PanelProject.Controls.Add(this._SplitterFrameList);
      this._PanelProject.Controls.Add(this._FrameList);
      this._PanelProject.Dock = System.Windows.Forms.DockStyle.Fill;
      this._PanelProject.Location = new System.Drawing.Point(0, 0);
      this._PanelProject.Name = "_PanelProject";
      this._PanelProject.Padding = new System.Windows.Forms.Padding(2);
      this._PanelProject.Size = new System.Drawing.Size(713, 509);
      this._PanelProject.TabIndex = 15;
      this._PanelProject.Visible = false;
      // 
      // _OutputSprite
      // 
      this._OutputSprite.AllowFrameClick = true;
      this._OutputSprite.Animation = null;
      this._OutputSprite.BackColor = System.Drawing.SystemColors.Control;
      this._OutputSprite.Dock = System.Windows.Forms.DockStyle.Fill;
      this._OutputSprite.Frame = null;
      this._OutputSprite.FrameColor = System.Drawing.Color.White;
      grid2.ForeColor = System.Drawing.Color.Black;
      grid2.Height = 32;
      grid2.OffsetX = 0;
      grid2.OffsetY = 0;
      grid2.Visible = true;
      grid2.Width = 32;
      this._OutputSprite.Grid = grid2;
      this._OutputSprite.GridColor = System.Drawing.Color.Black;
      this._OutputSprite.GridVisible = true;
      this._OutputSprite.Location = new System.Drawing.Point(2, 195);
      this._OutputSprite.Name = "_OutputSprite";
      this._OutputSprite.Project = null;
      this._OutputSprite.ShowImages = true;
      this._OutputSprite.Size = new System.Drawing.Size(373, 312);
      this._OutputSprite.SpriteBackColor = System.Drawing.Color.Gray;
      this._OutputSprite.TabIndex = 10;
      this._OutputSprite.Zoom = SpriteAnimationEditor.ZoomFactor.x8;
      this._OutputSprite.SelectedFrameChanged += new System.EventHandler(this._OutputSprite_SelectedFrameChanged);
      this._OutputSprite.GridChanged += new System.EventHandler(this._OutputSprite_GridChanged);
      this._OutputSprite.FrameClicked += new SpriteAnimationEditor.Controls.OutputSprite.FrameClickedHandler(this._OutputSprite_AnimationFrameClicked);
      // 
      // _SplitterOutputSprite
      // 
      this._SplitterOutputSprite.BackColor = System.Drawing.SystemColors.Control;
      this._SplitterOutputSprite.Dock = System.Windows.Forms.DockStyle.Right;
      this._SplitterOutputSprite.Location = new System.Drawing.Point(375, 195);
      this._SplitterOutputSprite.Name = "_SplitterOutputSprite";
      this._SplitterOutputSprite.Size = new System.Drawing.Size(3, 312);
      this._SplitterOutputSprite.TabIndex = 12;
      this._SplitterOutputSprite.TabStop = false;
      // 
      // _Playback
      // 
      this._Playback.Animation = null;
      this._Playback.BackColor = System.Drawing.SystemColors.Control;
      this._Playback.CurrentFrame = null;
      this._Playback.CurrentFrameNr = 0;
      this._Playback.Dock = System.Windows.Forms.DockStyle.Right;
      this._Playback.FrameBackColor = System.Drawing.Color.Gray;
      this._Playback.FramesPerSecond = 1;
      this._Playback.Location = new System.Drawing.Point(378, 195);
      this._Playback.Name = "_Playback";
      this._Playback.ShowImages = false;
      this._Playback.Size = new System.Drawing.Size(333, 312);
      this._Playback.TabIndex = 3;
      this._Playback.CurrentFrameChanged += new System.EventHandler(this._Playback_CurrentFrameChanged);
      this._Playback.FramesPerSecondChanged += new System.EventHandler(this._Playback_FramesPerSecondChanged);
      // 
      // _SplitterFrameList
      // 
      this._SplitterFrameList.BackColor = System.Drawing.SystemColors.Control;
      this._SplitterFrameList.Dock = System.Windows.Forms.DockStyle.Top;
      this._SplitterFrameList.Location = new System.Drawing.Point(2, 192);
      this._SplitterFrameList.Name = "_SplitterFrameList";
      this._SplitterFrameList.Size = new System.Drawing.Size(709, 3);
      this._SplitterFrameList.TabIndex = 11;
      this._SplitterFrameList.TabStop = false;
      // 
      // _FrameList
      // 
      this._FrameList.Animation = null;
      this._FrameList.BackColor = System.Drawing.SystemColors.Control;
      this._FrameList.Dock = System.Windows.Forms.DockStyle.Top;
      this._FrameList.Location = new System.Drawing.Point(2, 2);
      this._FrameList.Name = "_FrameList";
      this._FrameList.Project = null;
      this._FrameList.Size = new System.Drawing.Size(709, 190);
      this._FrameList.TabIndex = 13;
      this._FrameList.AddFramesFromFilesClicked += new System.EventHandler(this._FrameList_AddFramesFromFilesClicked);
      this._FrameList.DoubleClick += new System.EventHandler(this._FrameList_DoubleClick);
      this._FrameList.AddFramesToAnimationClicked += new System.EventHandler(this._FrameList_AddFramesToAnimationClicked);
      this._FrameList.SelectedAnimationFrameChanged += new System.EventHandler(this._FrameList_SelectedAnimationFrameChanged);
      this._FrameList.DeleteAnimationFramesClicked += new System.EventHandler(this._FrameList_DeleteAnimationFramesClicked);
      this._FrameList.FrameDoubleClicked += new SpriteAnimationEditor.Controls.FrameList.FrameDoubleClickedHandler(this._FrameList_FrameDoubleClicked);
      // 
      // _SplitterSolution
      // 
      this._SplitterSolution.BackColor = System.Drawing.SystemColors.Control;
      this._SplitterSolution.Dock = System.Windows.Forms.DockStyle.Right;
      this._SplitterSolution.Location = new System.Drawing.Point(713, 0);
      this._SplitterSolution.Name = "_SplitterSolution";
      this._SplitterSolution.Size = new System.Drawing.Size(3, 509);
      this._SplitterSolution.TabIndex = 16;
      this._SplitterSolution.TabStop = false;
      // 
      // _SolutionManager
      // 
      this._SolutionManager.BackColor = System.Drawing.SystemColors.Control;
      this._SolutionManager.Dock = System.Windows.Forms.DockStyle.Right;
      this._SolutionManager.Location = new System.Drawing.Point(716, 0);
      this._SolutionManager.Name = "_SolutionManager";
      this._SolutionManager.SelectedFrame = null;
      this._SolutionManager.Size = new System.Drawing.Size(200, 509);
      this._SolutionManager.SolutionExplorerHeight = 111;
      this._SolutionManager.SolutionHeight = 111;
      this._SolutionManager.TabIndex = 15;
      this._SolutionManager.ProjectFrameRectangleColorChanged += new System.EventHandler(this._SolutionManager_ProjectFrameRectangleColorChanged);
      this._SolutionManager.SolutionNameChanged += new System.EventHandler(this._SolutionManager_SolutionNameChanged);
      this._SolutionManager.ProjectFrameNrsColorChanged += new System.EventHandler(this._SolutionManager_ProjectFrameNrsColorChanged);
      this._SolutionManager.FrameAttributeChanged += new SpriteAnimationEditor.Controls.SolutionManager.FrameAttributeChangedHandler(this._SolutionManager_FrameAttributeChanged);
      this._SolutionManager.SelectedProjectChanged += new System.EventHandler(this._SolutionManager_SelectedProjectChanged);
      this._SolutionManager.ProjectSpriteBackColorChanged += new System.EventHandler(this._SolutionManager_ProjectSpriteBackColorChanged);
      this._SolutionManager.SolutionFilenameChanged += new System.EventHandler(this._SolutionManager_SolutionFilenameChanged);
      this._SolutionManager.CtxMenuRemoveProjecClicked += new System.EventHandler(this._SolutionManager_CtxMenuRemoveProjecClicked);
      this._SolutionManager.ProjectOutputSpriteChanged += new System.EventHandler(this._SolutionManager_ProjectOutputSpriteChanged);
      this._SolutionManager.CtxMenuCloseSolutionClicked += new System.EventHandler(this._SolutionManager_CtxMenuCloseSolutionClicked);
      this._SolutionManager.ProjectAttributeChanged += new System.EventHandler(this._SolutionManager_ProjectAttributeChanged);
      this._SolutionManager.FrameMovedDown += new SpriteAnimationEditor.Controls.SolutionManager.FrameHandler(this._SolutionManager_FrameMovedDown);
      this._SolutionManager.CtxMenuAddExistingProjectClicked += new System.EventHandler(this._SolutionManager_CtxMenuAddExistingProjectClicked);
      this._SolutionManager.ProjectLoaded += new SpriteAnimationEditor.Controls.SolutionManager.ProjectHandler(this._SolutionManager_ProjectLoaded);
      this._SolutionManager.FrameAddedToAnimation += new SpriteAnimationEditor.Controls.SolutionManager.AnimationFrameHandler(this._SolutionManager_FrameAddedToAnimation);
      this._SolutionManager.CtxMenuAddNewAnimClicked += new System.EventHandler(this._SolutionManager_CtxMenuAddNewAnimClicked);
      this._SolutionManager.SolutionDataLoaded += new SpriteAnimationEditor.Controls.SolutionManager.SolutionHandler(this._SolutionManager_SolutionDataLoaded);
      this._SolutionManager.AnimationFramesPerSecondChanged += new SpriteAnimationEditor.Controls.SolutionManager.AnimationHandler(this._SolutionManager_AnimationFramesPerSecondChanged);
      this._SolutionManager.EditFrameCollisionZonesClicked += new SpriteAnimationEditor.Controls.SolutionManager.FrameHandler(this._SolutionManager_EditFrameCollisionZonesClicked);
      this._SolutionManager.FrameMovedUp += new SpriteAnimationEditor.Controls.SolutionManager.FrameHandler(this._SolutionManager_FrameMovedUp);
      this._SolutionManager.SelectedFrameChanged += new System.EventHandler(this._SolutionManager_SelectedFrameChanged);
      this._SolutionManager.FramesRemoved += new SpriteAnimationEditor.Controls.SolutionManager.FrameListHandlerHandler(this._SolutionManager_FramesRemoved);
      this._SolutionManager.CtxMenuAddFramesToProjectClicked += new System.EventHandler(this._SolutionManager_CtxMenuAddFramesToProjectClicked);
      this._SolutionManager.ProjectLoadStarted += new SpriteAnimationEditor.Controls.SolutionManager.ProjectHandler(this._SolutionManager_ProjectLoadStarted);
      this._SolutionManager.CtxMenuAddNewProjectClicked += new System.EventHandler(this._SolutionManager_CtxMenuAddNewProjectClicked);
      this._SolutionManager.SelectedAnimationChanged += new System.EventHandler(this._SolutionManager_SelectedAnimationChanged);
      this._SolutionManager.CtxMenuDelAnimationClicked += new System.EventHandler(this._SolutionManager_CtxMenuDelAnimationClicked);
      this._SolutionManager.AnimationParentAnimationChanged += new SpriteAnimationEditor.Controls.SolutionManager.AnimationHandler(this._SolutionManager_AnimationParentAnimationChanged);
      // 
      // _StatusBar
      // 
      this._StatusBar.BackColor = System.Drawing.SystemColors.Control;
      this._StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._progressBar,
            this._StatusText,
            this._LblSolutionFile});
      this._StatusBar.Location = new System.Drawing.Point(0, 509);
      this._StatusBar.Name = "_StatusBar";
      this._StatusBar.Size = new System.Drawing.Size(916, 22);
      this._StatusBar.TabIndex = 17;
      // 
      // _progressBar
      // 
      this._progressBar.Name = "_progressBar";
      this._progressBar.Size = new System.Drawing.Size(100, 16);
      this._progressBar.Visible = false;
      // 
      // _StatusText
      // 
      this._StatusText.BackColor = System.Drawing.SystemColors.Control;
      this._StatusText.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                  | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                  | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this._StatusText.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this._StatusText.Name = "_StatusText";
      this._StatusText.Size = new System.Drawing.Size(450, 17);
      this._StatusText.Spring = true;
      this._StatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // _LblSolutionFile
      // 
      this._LblSolutionFile.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                  | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                  | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this._LblSolutionFile.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this._LblSolutionFile.Name = "_LblSolutionFile";
      this._LblSolutionFile.Size = new System.Drawing.Size(450, 17);
      this._LblSolutionFile.Spring = true;
      this._LblSolutionFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // _optMoveFrames
      // 
      this._optMoveFrames.Name = "_optMoveFrames";
      this._optMoveFrames.Size = new System.Drawing.Size(246, 22);
      this._optMoveFrames.Text = "Move frames...";
      this._optMoveFrames.Click += new System.EventHandler(this._optMoveFrames_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.AppWorkspace;
      this.ClientSize = new System.Drawing.Size(916, 555);
      this.Controls.Add(this._PanelSolution);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "MainForm";
      this.Text = "Sprite Animation Studio";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this._PanelSolution.ResumeLayout(false);
      this._PanelSolution.PerformLayout();
      this._PanelProject.ResumeLayout(false);
      this._StatusBar.ResumeLayout(false);
      this._StatusBar.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem _MenuFile;
    private System.Windows.Forms.ToolStripMenuItem _OptPreferences;
    private System.Windows.Forms.OpenFileDialog _DlgOpen;
    private System.Windows.Forms.ToolStripMenuItem _OptOpenSolution;
    private System.Windows.Forms.ToolStripMenuItem _OptSave;
    private System.Windows.Forms.ToolStripMenuItem _OptSaveAs;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator _OptRecentFilesSep1;
    private System.Windows.Forms.ToolStripMenuItem _OptExit;
    private System.Windows.Forms.ToolStripMenuItem _MenuAnimation;
    private Controls.AnimationPlayback _Playback;
    private System.Windows.Forms.SaveFileDialog _DlgSave;
    private System.Windows.Forms.ColorDialog _ColorPicker;
    private System.Windows.Forms.ToolStripMenuItem _OptNewSolution;
    private System.Windows.Forms.OpenFileDialog _DlgOpenAnimation;
    private System.Windows.Forms.ToolStripMenuItem _OptRecentFiles;
    private System.Windows.Forms.ToolStripSeparator _OptRecentFilesSep2;
    private Controls.OutputSprite _OutputSprite;
    private System.Windows.Forms.Panel _PanelSolution;
    private System.Windows.Forms.Panel _PanelProject;
    private System.Windows.Forms.Splitter _SplitterFrameList;
    private System.Windows.Forms.Splitter _SplitterOutputSprite;
    private System.Windows.Forms.ToolStripMenuItem _MenuProj;
    private SpriteAnimationEditor.Controls.FrameList _FrameList;
    private SpriteAnimationEditor.Controls.SolutionManager _SolutionManager;
    private System.Windows.Forms.Splitter _SplitterSolution;
    private System.Windows.Forms.ToolStripMenuItem _OptRemoveAnimFrames;
    private System.Windows.Forms.ToolStripMenuItem _OptAddFrames;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem _OptArrangeAnimFrames;
    private System.Windows.Forms.ToolStripMenuItem _OptMoveAnimFrameUp;
    private System.Windows.Forms.ToolStripMenuItem _OptMoveAnimFrameDown;
    private System.Windows.Forms.ToolStripMenuItem _MenuSolution;
    private System.Windows.Forms.ToolStripMenuItem _OptAddNewProject;
    private System.Windows.Forms.ToolStripMenuItem _OptRemoveProject;
    private System.Windows.Forms.ToolStripMenuItem _OptAddAnimation;
    private System.Windows.Forms.ToolStripMenuItem _OptRemoveAnimation;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem _OptSaveAll;
    private System.Windows.Forms.StatusStrip _StatusBar;
    private System.Windows.Forms.ToolStripStatusLabel _StatusText;
    private System.Windows.Forms.ToolStripMenuItem _OptCloseSolution;
    private System.Windows.Forms.ToolStripStatusLabel _LblSolutionFile;
    private System.Windows.Forms.ToolStripMenuItem _OptSelAllAnimationFrames;
    private System.Windows.Forms.ToolStripMenuItem _OptAddExistingProject;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    private System.Windows.Forms.ToolStripMenuItem _OptSelectSprite;
    private System.Windows.Forms.ToolStripMenuItem _OptAddNewFrame;
    private System.Windows.Forms.ToolStripMenuItem _mnuView;
    private System.Windows.Forms.ToolStripMenuItem _OptGridVisible;
    private System.Windows.Forms.ToolStripMenuItem _OptGridSize;
    private System.Windows.Forms.ToolStripMenuItem _OptRefreshImage;
    private System.Windows.Forms.ToolStripMenuItem _OptEditFrame;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem _OptEditColisionZones;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem _OptTileTester;
    private System.Windows.Forms.ToolStripMenuItem _mnuGrid;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem _optResolutionMapping;
    private System.Windows.Forms.ToolStripProgressBar _progressBar;
    private System.Windows.Forms.ToolStripMenuItem _optMoveFrames;
  }
}

