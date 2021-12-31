using LevelEditor.Controls;
using TwoBrainsGames.Snails.StageObjects;
namespace LevelEditor.Forms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._menu = new System.Windows.Forms.MenuStrip();
            this._mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this._optNew = new System.Windows.Forms.ToolStripMenuItem();
            this._optDeleteStage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._optClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._optExit = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this._optSelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this._optRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._optDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._optPrefs = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this._optGridVisible = new System.Windows.Forms.ToolStripMenuItem();
            this._optGridOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this._optShowTiles = new System.Windows.Forms.ToolStripMenuItem();
            this._optShowCameraFrame = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this._optSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this._optDeselect = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuStage = new System.Windows.Forms.ToolStripMenuItem();
            this._optProperties = new System.Windows.Forms.ToolStripMenuItem();
            this._optEditToolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._optStageDataEditor = new System.Windows.Forms.ToolStripMenuItem();
            this._optRunTileMatching = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this._optMoveTilesAndObjects = new System.Windows.Forms.ToolStripMenuItem();
            this._pnlSolution = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this._BoardCtl = new LevelEditor.Controls.BoardCtl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this._pnlTools = new System.Windows.Forms.Panel();
            this._ObjectSelectorCtl = new TwoBrainsGames.Snails.StageEditor.Controls.ObjectSelector();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this._TileSelectorCtl = new TwoBrainsGames.Snails.StageEditor.Controls.TileSelector();
            this._splitBoard = new System.Windows.Forms.Splitter();
            this._pnlSolutionTree = new System.Windows.Forms.Panel();
            this._propBrowser = new LevelEditor.Controls.PropertiesCtl();
            this._splitSolution = new System.Windows.Forms.Splitter();
            this._SolutionCtl = new LevelEditor.Controls.SolutionCtl();
            this._ToolStrip = new System.Windows.Forms.ToolStrip();
            this._btnSelectToolAll = new System.Windows.Forms.ToolStripButton();
            this._btnSelectToolTiles = new System.Windows.Forms.ToolStripButton();
            this._btnSelectToolObjects = new System.Windows.Forms.ToolStripButton();
            this.testeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._imgList = new System.Windows.Forms.ImageList(this.components);
            this._menu.SuspendLayout();
            this._pnlSolution.SuspendLayout();
            this.panel1.SuspendLayout();
            this._pnlTools.SuspendLayout();
            this._pnlSolutionTree.SuspendLayout();
            this._ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menu
            // 
            this._menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuFile,
            this._mnuEdit,
            this._mnuView,
            this._mnuSelect,
            this._mnuStage});
            this._menu.Location = new System.Drawing.Point(0, 0);
            this._menu.Name = "_menu";
            this._menu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this._menu.Size = new System.Drawing.Size(927, 28);
            this._menu.TabIndex = 0;
            this._menu.Text = "menuStrip1";
            // 
            // _mnuFile
            // 
            this._mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optNew,
            this._optDeleteStage,
            this.toolStripSeparator3,
            this._optClose,
            this.toolStripSeparator1,
            this._optExit});
            this._mnuFile.Name = "_mnuFile";
            this._mnuFile.Size = new System.Drawing.Size(44, 24);
            this._mnuFile.Text = "&File";
            // 
            // _optNew
            // 
            this._optNew.Name = "_optNew";
            this._optNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this._optNew.Size = new System.Drawing.Size(262, 24);
            this._optNew.Text = "&New custom stage...";
            this._optNew.Click += new System.EventHandler(this._optNew_Click);
            // 
            // _optDeleteStage
            // 
            this._optDeleteStage.Name = "_optDeleteStage";
            this._optDeleteStage.Size = new System.Drawing.Size(262, 24);
            this._optDeleteStage.Text = "Delete stage";
            this._optDeleteStage.Click += new System.EventHandler(this._optDeleteStage_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(259, 6);
            // 
            // _optClose
            // 
            this._optClose.Name = "_optClose";
            this._optClose.Size = new System.Drawing.Size(262, 24);
            this._optClose.Text = "Close";
            this._optClose.Click += new System.EventHandler(this._optClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(259, 6);
            // 
            // _optExit
            // 
            this._optExit.Name = "_optExit";
            this._optExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this._optExit.Size = new System.Drawing.Size(262, 24);
            this._optExit.Text = "&Exit";
            this._optExit.Click += new System.EventHandler(this._optExit_Click);
            // 
            // _mnuEdit
            // 
            this._mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optSelAll,
            this.toolStripSeparator5,
            this._optRotate,
            this.toolStripSeparator2,
            this._optDelete,
            this.toolStripSeparator4,
            this._optPrefs});
            this._mnuEdit.Name = "_mnuEdit";
            this._mnuEdit.Size = new System.Drawing.Size(47, 24);
            this._mnuEdit.Text = "&Edit";
            this._mnuEdit.DropDownOpening += new System.EventHandler(this._mnuEdit_DropDownOpening);
            // 
            // _optSelAll
            // 
            this._optSelAll.Name = "_optSelAll";
            this._optSelAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this._optSelAll.Size = new System.Drawing.Size(192, 24);
            this._optSelAll.Text = "Select All";
            this._optSelAll.Click += new System.EventHandler(this._optSelAll_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(189, 6);
            // 
            // _optRotate
            // 
            this._optRotate.Name = "_optRotate";
            this._optRotate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this._optRotate.Size = new System.Drawing.Size(192, 24);
            this._optRotate.Text = "Rotate";
            this._optRotate.Click += new System.EventHandler(this._optRotate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(189, 6);
            // 
            // _optDelete
            // 
            this._optDelete.Image = ((System.Drawing.Image)(resources.GetObject("_optDelete.Image")));
            this._optDelete.Name = "_optDelete";
            this._optDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this._optDelete.Size = new System.Drawing.Size(192, 24);
            this._optDelete.Text = "Delete";
            this._optDelete.Click += new System.EventHandler(this._optDelete_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(189, 6);
            // 
            // _optPrefs
            // 
            this._optPrefs.Name = "_optPrefs";
            this._optPrefs.Size = new System.Drawing.Size(192, 24);
            this._optPrefs.Text = "Preferences...";
            this._optPrefs.Click += new System.EventHandler(this._optPrefs_Click);
            // 
            // _mnuView
            // 
            this._mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optGridVisible,
            this._optGridOnTop,
            this._optShowTiles,
            this._optShowCameraFrame});
            this._mnuView.Name = "_mnuView";
            this._mnuView.Size = new System.Drawing.Size(53, 24);
            this._mnuView.Text = "View";
            this._mnuView.DropDownOpening += new System.EventHandler(this._mnuView_DropDownOpening);
            // 
            // _optGridVisible
            // 
            this._optGridVisible.Name = "_optGridVisible";
            this._optGridVisible.Size = new System.Drawing.Size(214, 24);
            this._optGridVisible.Text = "Grid Visible";
            this._optGridVisible.Click += new System.EventHandler(this._optGridVisible_Click);
            // 
            // _optGridOnTop
            // 
            this._optGridOnTop.Name = "_optGridOnTop";
            this._optGridOnTop.Size = new System.Drawing.Size(214, 24);
            this._optGridOnTop.Text = "Grid On Top";
            this._optGridOnTop.Click += new System.EventHandler(this._optGridOnTop_Click);
            // 
            // _optShowTiles
            // 
            this._optShowTiles.Name = "_optShowTiles";
            this._optShowTiles.Size = new System.Drawing.Size(214, 24);
            this._optShowTiles.Text = "Tiles Visible";
            this._optShowTiles.Click += new System.EventHandler(this._optShowTiles_Click);
            // 
            // _optShowCameraFrame
            // 
            this._optShowCameraFrame.Name = "_optShowCameraFrame";
            this._optShowCameraFrame.Size = new System.Drawing.Size(214, 24);
            this._optShowCameraFrame.Text = "Show Camera Frame";
            this._optShowCameraFrame.Click += new System.EventHandler(this._optShowCameraFrame_Click);
            // 
            // _mnuSelect
            // 
            this._mnuSelect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optSelectAll,
            this._optDeselect});
            this._mnuSelect.Name = "_mnuSelect";
            this._mnuSelect.Size = new System.Drawing.Size(61, 24);
            this._mnuSelect.Text = "Select";
            this._mnuSelect.DropDownOpening += new System.EventHandler(this._mnuSelect_DropDownOpening);
            // 
            // _optSelectAll
            // 
            this._optSelectAll.Name = "_optSelectAll";
            this._optSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this._optSelectAll.Size = new System.Drawing.Size(230, 24);
            this._optSelectAll.Text = "Select All";
            this._optSelectAll.Click += new System.EventHandler(this._optSelectAll_Click);
            // 
            // _optDeselect
            // 
            this._optDeselect.Name = "_optDeselect";
            this._optDeselect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this._optDeselect.Size = new System.Drawing.Size(230, 24);
            this._optDeselect.Text = "Clear Selection";
            this._optDeselect.Click += new System.EventHandler(this._optDeselect_Click);
            // 
            // _mnuStage
            // 
            this._mnuStage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optProperties,
            this._optEditToolsMenu,
            this._optStageDataEditor,
            this._optRunTileMatching,
            this.toolStripSeparator6,
            this._optMoveTilesAndObjects});
            this._mnuStage.Name = "_mnuStage";
            this._mnuStage.ShowShortcutKeys = false;
            this._mnuStage.Size = new System.Drawing.Size(59, 24);
            this._mnuStage.Text = "Stage";
            // 
            // _optProperties
            // 
            this._optProperties.Name = "_optProperties";
            this._optProperties.Size = new System.Drawing.Size(280, 24);
            this._optProperties.Text = "Properties...";
            this._optProperties.Click += new System.EventHandler(this._optProperties_Click);
            // 
            // _optEditToolsMenu
            // 
            this._optEditToolsMenu.Name = "_optEditToolsMenu";
            this._optEditToolsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this._optEditToolsMenu.Size = new System.Drawing.Size(280, 24);
            this._optEditToolsMenu.Text = "Edit Tools Menu...";
            this._optEditToolsMenu.Click += new System.EventHandler(this._optEditToolsMenu_Click);
            // 
            // _optStageDataEditor
            // 
            this._optStageDataEditor.Name = "_optStageDataEditor";
            this._optStageDataEditor.Size = new System.Drawing.Size(280, 24);
            this._optStageDataEditor.Text = "StageData Editor...";
            this._optStageDataEditor.Click += new System.EventHandler(this._optStageDataEditor_Click);
            // 
            // _optRunTileMatching
            // 
            this._optRunTileMatching.Name = "_optRunTileMatching";
            this._optRunTileMatching.Size = new System.Drawing.Size(280, 24);
            this._optRunTileMatching.Text = "Run Tile Matching Algorythm...";
            this._optRunTileMatching.Click += new System.EventHandler(this._optRunTileMatching_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(277, 6);
            // 
            // _optMoveTilesAndObjects
            // 
            this._optMoveTilesAndObjects.Name = "_optMoveTilesAndObjects";
            this._optMoveTilesAndObjects.Size = new System.Drawing.Size(280, 24);
            this._optMoveTilesAndObjects.Text = "Move Tiles and Objects...";
            this._optMoveTilesAndObjects.Click += new System.EventHandler(this._optMoveTilesAndObjects_Click);
            // 
            // _pnlSolution
            // 
            this._pnlSolution.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this._pnlSolution.Controls.Add(this.panel1);
            this._pnlSolution.Controls.Add(this.splitter1);
            this._pnlSolution.Controls.Add(this._pnlTools);
            this._pnlSolution.Controls.Add(this._splitBoard);
            this._pnlSolution.Controls.Add(this._pnlSolutionTree);
            this._pnlSolution.Controls.Add(this._ToolStrip);
            this._pnlSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSolution.Location = new System.Drawing.Point(0, 28);
            this._pnlSolution.Margin = new System.Windows.Forms.Padding(4);
            this._pnlSolution.Name = "_pnlSolution";
            this._pnlSolution.Size = new System.Drawing.Size(927, 483);
            this._pnlSolution.TabIndex = 1;
            this._pnlSolution.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this._BoardCtl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(152, 39);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(466, 444);
            this.panel1.TabIndex = 15;
            // 
            // _BoardCtl
            // 
            this._BoardCtl.CursorObject = null;
            this._BoardCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._BoardCtl.Location = new System.Drawing.Point(0, 0);
            this._BoardCtl.Margin = new System.Windows.Forms.Padding(5);
            this._BoardCtl.Name = "_BoardCtl";
            this._BoardCtl.SelectedItems = ((System.Collections.Generic.List<object>)(resources.GetObject("_BoardCtl.SelectedItems")));
            this._BoardCtl.SelectedToolType = TwoBrainsGames.Snails.StageEditor.ToolType.None;
            this._BoardCtl.SelectionMode = TwoBrainsGames.Snails.StageEditor.SelectionType.Tiles;
            this._BoardCtl.Size = new System.Drawing.Size(452, 433);
            this._BoardCtl.Stage = null;
            this._BoardCtl.TabIndex = 10;
            this._BoardCtl.CellClicked += new LevelEditor.Controls.BoardCtl.CellClickedHandler(this._BoardCtl_CellClicked);
            this._BoardCtl.StageObjectPropertiesClicked += new LevelEditor.Controls.BoardCtl.StageObjectHandler(this._BoardCtl_StageObjectPropertiesClicked);
            this._BoardCtl.DeleteItemsClicked += new System.EventHandler(this._BoardCtl_DeleteItemClicked);
            this._BoardCtl.MoveToFrontClicked += new LevelEditor.Controls.BoardCtl.StageObjectHandler(this._BoardCtl_MoveToFrontClicked);
            this._BoardCtl.SendToBackClicked += new LevelEditor.Controls.BoardCtl.StageObjectHandler(this._BoardCtl_SendToBackClicked);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(148, 39);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 444);
            this.splitter1.TabIndex = 13;
            this.splitter1.TabStop = false;
            // 
            // _pnlTools
            // 
            this._pnlTools.BackColor = System.Drawing.SystemColors.Control;
            this._pnlTools.Controls.Add(this._ObjectSelectorCtl);
            this._pnlTools.Controls.Add(this.splitter2);
            this._pnlTools.Controls.Add(this._TileSelectorCtl);
            this._pnlTools.Dock = System.Windows.Forms.DockStyle.Left;
            this._pnlTools.Location = new System.Drawing.Point(0, 39);
            this._pnlTools.Margin = new System.Windows.Forms.Padding(4);
            this._pnlTools.Name = "_pnlTools";
            this._pnlTools.Size = new System.Drawing.Size(148, 444);
            this._pnlTools.TabIndex = 12;
            this._pnlTools.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlTools_Paint);
            // 
            // _ObjectSelectorCtl
            // 
            this._ObjectSelectorCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ObjectSelectorCtl.Location = new System.Drawing.Point(0, 173);
            this._ObjectSelectorCtl.Margin = new System.Windows.Forms.Padding(5);
            this._ObjectSelectorCtl.Name = "_ObjectSelectorCtl";
            this._ObjectSelectorCtl.SelectedObject = null;
            this._ObjectSelectorCtl.Size = new System.Drawing.Size(148, 271);
            this._ObjectSelectorCtl.TabIndex = 1;
            this._ObjectSelectorCtl.SelectedObjectChanged += new System.EventHandler(this._ObjectSelectorCtl_SelectedObjectChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 169);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(148, 4);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // _TileSelectorCtl
            // 
            this._TileSelectorCtl.Dock = System.Windows.Forms.DockStyle.Top;
            this._TileSelectorCtl.Location = new System.Drawing.Point(0, 0);
            this._TileSelectorCtl.Margin = new System.Windows.Forms.Padding(5);
            this._TileSelectorCtl.Name = "_TileSelectorCtl";
            this._TileSelectorCtl.SelectedItem = null;
            this._TileSelectorCtl.Size = new System.Drawing.Size(148, 169);
            this._TileSelectorCtl.TabIndex = 0;
            this._TileSelectorCtl.SelectedTileChanged += new System.EventHandler(this._TileSelectorCtl_SelectedTileChanged);
            // 
            // _splitBoard
            // 
            this._splitBoard.BackColor = System.Drawing.SystemColors.Control;
            this._splitBoard.Dock = System.Windows.Forms.DockStyle.Right;
            this._splitBoard.Location = new System.Drawing.Point(618, 39);
            this._splitBoard.Margin = new System.Windows.Forms.Padding(4);
            this._splitBoard.Name = "_splitBoard";
            this._splitBoard.Size = new System.Drawing.Size(4, 444);
            this._splitBoard.TabIndex = 11;
            this._splitBoard.TabStop = false;
            // 
            // _pnlSolutionTree
            // 
            this._pnlSolutionTree.Controls.Add(this._propBrowser);
            this._pnlSolutionTree.Controls.Add(this._splitSolution);
            this._pnlSolutionTree.Controls.Add(this._SolutionCtl);
            this._pnlSolutionTree.Dock = System.Windows.Forms.DockStyle.Right;
            this._pnlSolutionTree.Location = new System.Drawing.Point(622, 39);
            this._pnlSolutionTree.Margin = new System.Windows.Forms.Padding(4);
            this._pnlSolutionTree.Name = "_pnlSolutionTree";
            this._pnlSolutionTree.Size = new System.Drawing.Size(305, 444);
            this._pnlSolutionTree.TabIndex = 9;
            // 
            // _propBrowser
            // 
            this._propBrowser.BackColor = System.Drawing.SystemColors.Control;
            this._propBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propBrowser.Location = new System.Drawing.Point(0, 212);
            this._propBrowser.Margin = new System.Windows.Forms.Padding(5);
            this._propBrowser.Name = "_propBrowser";
            this._propBrowser.SelectedObject = null;
            this._propBrowser.Size = new System.Drawing.Size(305, 232);
            this._propBrowser.TabIndex = 6;
            this._propBrowser.StagePropertyChanged += new LevelEditor.Controls.PropertiesCtl.StageHandler(this._propBrowser_StagePropertyChanged);
            // 
            // _splitSolution
            // 
            this._splitSolution.BackColor = System.Drawing.SystemColors.Control;
            this._splitSolution.Dock = System.Windows.Forms.DockStyle.Top;
            this._splitSolution.Location = new System.Drawing.Point(0, 208);
            this._splitSolution.Margin = new System.Windows.Forms.Padding(4);
            this._splitSolution.Name = "_splitSolution";
            this._splitSolution.Size = new System.Drawing.Size(305, 4);
            this._splitSolution.TabIndex = 5;
            this._splitSolution.TabStop = false;
            // 
            // _SolutionCtl
            // 
            this._SolutionCtl.BackColor = System.Drawing.SystemColors.Control;
            this._SolutionCtl.Dock = System.Windows.Forms.DockStyle.Top;
            this._SolutionCtl.Location = new System.Drawing.Point(0, 0);
            this._SolutionCtl.Margin = new System.Windows.Forms.Padding(5);
            this._SolutionCtl.Name = "_SolutionCtl";
            this._SolutionCtl.PropGrid = this._propBrowser;
            this._SolutionCtl.Size = new System.Drawing.Size(305, 208);
            this._SolutionCtl.Solution = null;
            this._SolutionCtl.TabIndex = 4;
            this._SolutionCtl.BeforeCurrentStageChanged += new LevelEditor.Controls.SolutionCtl.CancelEventHandler(this._SolutionCtl_BeforeCurrentStageChanged);
            this._SolutionCtl.SolutionNodeSelected += new LevelEditor.Controls.SolutionCtl.SolutionNodeSelectedHandler(this._SolutionCtl_SolutionNodeSelected);
            // 
            // _ToolStrip
            // 
            this._ToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this._ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnSelectToolAll,
            this._btnSelectToolTiles,
            this._btnSelectToolObjects});
            this._ToolStrip.Location = new System.Drawing.Point(0, 0);
            this._ToolStrip.Name = "_ToolStrip";
            this._ToolStrip.Size = new System.Drawing.Size(927, 39);
            this._ToolStrip.TabIndex = 14;
            this._ToolStrip.Text = "toolStrip1";
            // 
            // _btnSelectToolAll
            // 
            this._btnSelectToolAll.CheckOnClick = true;
            this._btnSelectToolAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSelectToolAll.Image = ((System.Drawing.Image)(resources.GetObject("_btnSelectToolAll.Image")));
            this._btnSelectToolAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnSelectToolAll.Name = "_btnSelectToolAll";
            this._btnSelectToolAll.Size = new System.Drawing.Size(36, 36);
            this._btnSelectToolAll.Text = "toolStripButton2";
            this._btnSelectToolAll.ToolTipText = "Selection Tool (Tiles+Objects)";
            this._btnSelectToolAll.Click += new System.EventHandler(this._btnSelectToolAll_Click);
            // 
            // _btnSelectToolTiles
            // 
            this._btnSelectToolTiles.CheckOnClick = true;
            this._btnSelectToolTiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSelectToolTiles.Image = ((System.Drawing.Image)(resources.GetObject("_btnSelectToolTiles.Image")));
            this._btnSelectToolTiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnSelectToolTiles.Name = "_btnSelectToolTiles";
            this._btnSelectToolTiles.Size = new System.Drawing.Size(36, 36);
            this._btnSelectToolTiles.Text = "toolStripButton1";
            this._btnSelectToolTiles.ToolTipText = "Selection Tool (Tiles)";
            this._btnSelectToolTiles.Click += new System.EventHandler(this._btnSelectToolTiles_Click);
            // 
            // _btnSelectToolObjects
            // 
            this._btnSelectToolObjects.CheckOnClick = true;
            this._btnSelectToolObjects.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSelectToolObjects.Image = ((System.Drawing.Image)(resources.GetObject("_btnSelectToolObjects.Image")));
            this._btnSelectToolObjects.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnSelectToolObjects.Name = "_btnSelectToolObjects";
            this._btnSelectToolObjects.Size = new System.Drawing.Size(36, 36);
            this._btnSelectToolObjects.Text = "toolStripButton1";
            this._btnSelectToolObjects.ToolTipText = "Selection Tool (Objects)";
            this._btnSelectToolObjects.Click += new System.EventHandler(this._btnSelectToolObjects_Click);
            // 
            // testeToolStripMenuItem
            // 
            this.testeToolStripMenuItem.Name = "testeToolStripMenuItem";
            this.testeToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.testeToolStripMenuItem.Text = "teste";
            // 
            // _imgList
            // 
            this._imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imgList.ImageStream")));
            this._imgList.TransparentColor = System.Drawing.Color.Transparent;
            this._imgList.Images.SetKeyName(0, "delete.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(927, 511);
            this.Controls.Add(this._pnlSolution);
            this.Controls.Add(this._menu);
            this.KeyPreview = true;
            this.MainMenuStrip = this._menu;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "MainForm";
            this.ShowInTaskbar = true;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this._menu.ResumeLayout(false);
            this._menu.PerformLayout();
            this._pnlSolution.ResumeLayout(false);
            this._pnlSolution.PerformLayout();
            this.panel1.ResumeLayout(false);
            this._pnlTools.ResumeLayout(false);
            this._pnlSolutionTree.ResumeLayout(false);
            this._ToolStrip.ResumeLayout(false);
            this._ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip _menu;
    private System.Windows.Forms.ToolStripMenuItem _mnuFile;
    private System.Windows.Forms.ToolStripMenuItem _optExit;
    private System.Windows.Forms.ToolStripMenuItem _optNew;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.Panel _pnlSolution;
    private System.Windows.Forms.Panel _pnlSolutionTree;
    private LevelEditor.Controls.PropertiesCtl _propBrowser;
    private System.Windows.Forms.Splitter _splitSolution;
    private LevelEditor.Controls.SolutionCtl _SolutionCtl;
    private LevelEditor.Controls.BoardCtl _BoardCtl;
    private System.Windows.Forms.ToolStripMenuItem _mnuStage;
    private System.Windows.Forms.Splitter _splitBoard;
    private System.Windows.Forms.ToolStripMenuItem _optClose;
    private System.Windows.Forms.ToolStripMenuItem _optEditToolsMenu;
    private System.Windows.Forms.ToolStripMenuItem _mnuView;
    private System.Windows.Forms.ToolStripMenuItem _optGridVisible;
    private System.Windows.Forms.ToolStripMenuItem _optGridOnTop;
    private System.Windows.Forms.Splitter splitter1;
    private System.Windows.Forms.Panel _pnlTools;
    private TwoBrainsGames.Snails.StageEditor.Controls.TileSelector _TileSelectorCtl;
    private TwoBrainsGames.Snails.StageEditor.Controls.ObjectSelector _ObjectSelectorCtl;
    private System.Windows.Forms.ToolStrip _ToolStrip;
    private System.Windows.Forms.ToolStripButton _btnSelectToolTiles;
    private System.Windows.Forms.ToolStripButton _btnSelectToolObjects;
    private System.Windows.Forms.ToolStripButton _btnSelectToolAll;
    private System.Windows.Forms.ToolStripMenuItem _mnuSelect;
    private System.Windows.Forms.ToolStripMenuItem _optDeselect;
    private System.Windows.Forms.ToolStripMenuItem testeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _mnuEdit;
    private System.Windows.Forms.ToolStripMenuItem _optDelete;
    private System.Windows.Forms.ImageList _imgList;
	private System.Windows.Forms.ToolStripMenuItem _optStageDataEditor;
    private System.Windows.Forms.ToolStripMenuItem _optSelectAll;
    private System.Windows.Forms.ToolStripMenuItem _optProperties;
    private System.Windows.Forms.ToolStripMenuItem _optRotate;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem _optDeleteStage;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem _optPrefs;
    private System.Windows.Forms.ToolStripMenuItem _optSelAll;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.Splitter splitter2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem _optMoveTilesAndObjects;
    private System.Windows.Forms.ToolStripMenuItem _optRunTileMatching;
    private System.Windows.Forms.ToolStripMenuItem _optShowTiles;
    private System.Windows.Forms.ToolStripMenuItem _optShowCameraFrame;
  }
}