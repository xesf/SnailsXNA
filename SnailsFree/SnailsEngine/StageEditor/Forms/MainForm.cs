using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using LevelEditor.Controls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails;
using TwoBrainsGames.Snails.StageEditor;
using TwoBrainsGames.Snails.StageEditor.Forms;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;
using TwoBrainsGames.Snails.Stages.Hints;

namespace LevelEditor.Forms
{
    public partial class MainForm : BaseForm, IFormStateSave
    {
        public enum StageEditingMode
        {
            Stage,
            Hints
        }
        public static MainForm _instance;
        ToolType _SelectedToolType;
        EditorStage _currentStage;

        #region Properties

        public SelectionType SelectionMode
        {
            get
            {
                return this._BoardCtl.SelectionMode;
            }
            set
            {
                this._BoardCtl.SelectionMode = value;
                this._btnSelectToolTiles.Checked = (this._BoardCtl.SelectionMode == SelectionType.Tiles);
                this._btnSelectToolObjects.Checked = (this._BoardCtl.SelectionMode == SelectionType.Objects);
                this._btnSelectToolAll.Checked = (this._BoardCtl.SelectionMode == SelectionType.TilesAndObjects);

            }
        }

        ToolType SelectedToolType
        {
            get { return this._SelectedToolType; }
            set
            {
                if (this._SelectedToolType != value)
                {
                    this._BoardCtl.SelectedToolType = value;

                    // Clear the boarctl cursor object
                    this._BoardCtl.CursorObject = null;

                    this._SelectedToolType = value;

                    this.EnableButtons();


                }
            }
        }

        Solution CurrentSolution
        {
            get { return this._SolutionCtl.Solution; }
            set
            {
                this._SolutionCtl.Solution = value;
                this._pnlSolution.Visible = (this._SolutionCtl.Solution != null);
            }
        }
        
        EditorStage CurrentStage
        {
            get 
            { 
                return this._currentStage; 
            }
            set 
            {
                EditorStage.CurrentStage = value; 
                EditorStage prevStage = this._currentStage;
                if (this._currentStage != value)
                {
                    this._currentStage = value;
                    if (this._currentStage != null)
                    {
                        Stage.CurrentStage = this._currentStage.Stage;
                        this._BoardCtl.Stage = this._currentStage;
                        this._BoardCtl.Refresh();
                        this._propBrowser.SelectedObject = this._currentStage;
                        Levels.CurrentTheme = this._currentStage.Stage.LevelStage.ThemeId;
                        Levels.CurrentStageNr = this._currentStage.Stage.LevelStage.StageNr;
                        Levels.CurrentCustomStageFilename = null;
                        this.ClearSelection();
                        if (this._currentStage is EditorCustomStage)
                        {
                            ((EditorCustomStage)this._currentStage).ThemeChanged += new EventHandler(MainForm_CurrentStageThemeChanged);
                            Levels.CurrentCustomStageFilename = ((EditorCustomStage)this._currentStage).Filename;
                        }
                        this._SolutionCtl.SelectStage(this._currentStage.Stage.LevelStage);
                    }
                    if (prevStage != null)
                    {
                        if (prevStage.Stage.LevelStage.ThemeId != this._currentStage.Stage.LevelStage.ThemeId)
                        {
                            this.FillTileSelector();
                            this.FillObjectSelector();
                        }
                    }

                }
            }
        }


        public static EditorStage CurrentStageEdited
        {
            get 
            {
                if (MainForm._instance == null)
                {
                    return null;
                }
                return MainForm._instance.CurrentStage; 
            }
        }

        public LevelStage CurrentLevelStage 
        {
            get
            {
                if (this.CurrentStage == null ||
                    this.CurrentStage.Stage == null)
                {
                    return null;
                }
                return this.CurrentStage.Stage.LevelStage;
            }
        }

        public StageEditingMode EditingMode { get; set; }
        public bool ShowOnlyHints { get; set; }
        #endregion

        #region Constructors
        public MainForm()
        {
            MainForm._instance = this;
            InitializeComponent();
        }
        #endregion

        #region Solution methods
        /// <summary>
        /// 
        /// </summary>
        private void CreateNewSolution()
        {
            if (this.CurrentSolution == null)
            {
                this.CurrentSolution = new Solution();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CloseSolution()
        {
            this.CurrentSolution = null;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public DialogResult ShowDialog(IWin32Window parent, Levels levels, ref LevelStage currentStage)
        {
            this.CurrentSolution = new Solution(levels);
            this._SolutionCtl.SelectStage(currentStage);
            DialogResult dr = this.ShowDialog(parent);

            currentStage = this.CurrentLevelStage;
            return dr;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Show(Levels levels)
        {
            this.CurrentSolution = new Solution(levels);

            this.Show();
        }

        #region Stage methods
       

        /// <summary>
        /// 
        /// </summary>
        private void _SolutionCtl_BeforeCurrentStageChanged(out bool cancel)
        {
            cancel = false;
            try
            {
                this.SaveStage(true);
            }
            catch (StageValidationException ex1)
            {
                MessageBox.Show(this, ex1.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancel = true;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _propBrowser_StagePropertyChanged(EditorStage stage)
        {
            try
            {
                this._BoardCtl.Refresh();
                this.CurrentStage.Changed = true;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveStage(bool performValidations)
        {
            if (this.CurrentStage == null)
                return;
            if (this.CurrentStage.Changed == false)
                return;

            this.CurrentStage.Save(performValidations);
        }

        #endregion

        #region File menu
        /// <summary>
        /// 
        /// </summary>
        private void _optExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.CloseApp();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.CreateNewCustomStage();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.CloseSolution();
                this.EnableButtons();
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
        private void _optGridVisible_Click(object sender, EventArgs e)
        {
            try
            {
                UserSettings.GridVisible = ! UserSettings.GridVisible;
                this._BoardCtl.Refresh();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optGridOnTop_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserSettings.GridOnTop)
                {
                    UserSettings.GridOnTop = false;
                }
                else
                {
                    UserSettings.GridOnTop = true;
                }
                this._BoardCtl.Refresh();
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
                this._optGridVisible.Checked = UserSettings.GridVisible;
                this._optGridOnTop.Checked = UserSettings.GridOnTop;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
        #endregion

        #region IFormStateSave Members

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord CreateSaveSate()
        {
            DataFileRecord record = base.CreateSaveSate();
            record.AddField("SolutionHeigth", this._SolutionCtl.Height);
            record.AddField("SolutionWidth", this._pnlSolutionTree.Width);
            record.AddField("TileListHeight", this._TileSelectorCtl.Height);
            record.AddField("ToolboxWidth", this._pnlTools.Width);
            return record;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void RestoreSaveSate(DataFileRecord record)
        {
            if (record == null)
            {
                return;
            }
            base.RestoreSaveSate(record);
            this._SolutionCtl.Height = record.GetFieldValue<int>("SolutionHeigth", this._SolutionCtl.Height);
            this._pnlSolutionTree.Width = record.GetFieldValue<int>("SolutionWidth", this._pnlSolutionTree.Width);
            this._TileSelectorCtl.Height = record.GetFieldValue<int>("TileListHeight", this._TileSelectorCtl.Height);
            this._pnlTools.Width = record.GetFieldValue<int>("ToolboxWidth", this._pnlTools.Width);
        }
        #endregion

        #region Form events
        /// <summary>
        /// 
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
               // Settings.Load();
                UserSettings.Load();
                this.FillTileSelector();
                this.FillObjectSelector();

                this._TileSelectorCtl.Height = UserSettings.TilesToobarHeight;
                this._ObjectSelectorCtl.Height = UserSettings.ObjectsToolbarHeight;
                UserSettings.LoadFormsStates();
                UserSettings.ShowImages = true;
                this.EnableButtons();
                this.SelectedToolType = ToolType.Select;
                this.SelectionMode = SelectionType.TilesAndObjects;
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
                if (this.EditingMode == StageEditingMode.Hints)
                {
                    this.EndHintEditingMode();
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
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                UserSettings.ObjectsToolbarHeight = this._ObjectSelectorCtl.Height;
                UserSettings.TilesToobarHeight = this._TileSelectorCtl.Height;
                UserSettings.Save();
                this.SaveStage(true);
            }
            catch (StageValidationException ex1)
            {
                MessageBox.Show(this, ex1.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }

        }
        #endregion

        #region Board events
        /// <summary>
        /// 
        /// </summary>
        private void _BoardCtl_MouseDownOnObject(StageObject obj, MouseEventArgs mouseArgs)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void _BoardCtl_CellClicked(int x, int y, int rowx, int rowy, MouseEventArgs mouseArgs)
        {
            try
            {
                switch (this.SelectedToolType)
                {
                    case ToolType.Tile:
                        this.ToolTileAction(rowx, rowy, mouseArgs);
                        break;

                    case ToolType.GameObject:
                        this.ToolGameObjectAction(x, y, rowx, rowy, mouseArgs);
                        break;

                }

                this.EnableButtons();

            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void _BoardCtl_SelectionEnded(System.Drawing.Rectangle selectionArea)
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

        #region Other methods
        /// <summary>
        /// 
        /// </summary>
        private void CloseApp()
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            this._mnuStage.Enabled = (this.CurrentStage != null && this.EditingMode != StageEditingMode.Hints );

            this._optGridVisible.Enabled = (this.CurrentStage != null);
            this._optGridOnTop.Enabled = (this.CurrentStage != null);

            this._BoardCtl.Visible = (this.CurrentStage != null);

            if (this.SelectedToolType != ToolType.GameObject)
                this._ObjectSelectorCtl.SelectedObject = null;

            if (this.SelectedToolType != ToolType.Tile)
                this._TileSelectorCtl.SelectedItem = null;

            this._btnSelectToolObjects.Checked = (this.SelectedToolType == ToolType.Select &&
                                                  this.SelectionMode == SelectionType.Objects);
            this._btnSelectToolTiles.Checked = (this.SelectedToolType == ToolType.Select &&
                                                  this.SelectionMode == SelectionType.Tiles);
            this._btnSelectToolAll.Checked = (this.SelectedToolType == ToolType.Select &&
                                                  this.SelectionMode == SelectionType.TilesAndObjects);


            //Selection menu
            this._optDeselect.Enabled = (this._BoardCtl.SelectedItems.Count > 0);

            // Edit menu
            this._optDelete.Enabled = (this._BoardCtl.SelectedItems.Count > 0);

            this._optRotate.Enabled = this._BoardCtl.SelectedObjectsAllowRotation;
         
            this._optRunTileMatching.Enabled = (this.CurrentStage != null);

            this._optGridVisible.Checked = (UserSettings.GridVisible);
            this._optShowTiles.Checked = (UserSettings.ShowTiles);
            this._optShowCameraFrame.Checked = (UserSettings.ShowCameraFrame);
            
            this._optEndHintEdit.Visible = (this.EditingMode == StageEditingMode.Hints);
            this._pnlSolutionTree.Visible = (this.EditingMode != StageEditingMode.Hints);
            this._mnuFile.Enabled = (this.EditingMode != StageEditingMode.Hints);
            this._optShowOnlyHints.Checked = (this.ShowOnlyHints);
            this._optShowOnlyHints.Enabled = (this.EditingMode == StageEditingMode.Hints);
        }
        
       
        /// <summary>
        /// 
        /// </summary>
        private void FillTileSelector()
        {
            this._TileSelectorCtl.InitializeFromSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillObjectSelector()
        {
      
            this._ObjectSelectorCtl.InitializeFromSettings();
        }

        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        private void _TileSelectorCtl_SelectedTileChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._TileSelectorCtl.SelectedItem != null)
                {
                    this.SelectedToolType = ToolType.Tile;
                    this._BoardCtl.Cursor.Set(this._TileSelectorCtl.SelectedItem);
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
        private void _ObjectSelectorCtl_SelectedObjectChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._ObjectSelectorCtl.SelectedObject != null)
                {
                    this.SelectedToolType = ToolType.GameObject;
                    this._BoardCtl.CursorObject = this._ObjectSelectorCtl.SelectedObject;

                    if (this._ObjectSelectorCtl.SelectedObject.EditorBehaviour.ItemSelectForm != null)
                    {
                        System.Runtime.Remoting.ObjectHandle handle = Activator.CreateInstance(System.Reflection.Assembly.GetExecutingAssembly().FullName, 
                                                    this._ObjectSelectorCtl.SelectedObject.EditorBehaviour.ItemSelectForm.FormTypeName);
                        ObjectSelectedBaseForm form = (ObjectSelectedBaseForm)handle.Unwrap();
                        form.ShowDialog(this, _ObjectSelectorCtl.SelectedObject);
                        this._BoardCtl.CursorObject = this._ObjectSelectorCtl.SelectedObject;

                    }
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        #region Tool actions

       

        /// <summary>
        /// 
        /// </summary>
        private void ToolTileAction(int x, int y, MouseEventArgs mouseArgs)
        {
            if (this.CurrentStage == null)
                return;

            if (this._TileSelectorCtl.SelectedItem == null)
                return;

            if (mouseArgs.Button == MouseButtons.Left)
            {
                this._TileSelectorCtl.SelectedItem.OnBoardPlacement(x, y);
                this.CurrentStage.SetTileAt(this._TileSelectorCtl.SelectedItem.Tile.StyleGroupId, x, y);
            }
            else
            if (mouseArgs.Button == MouseButtons.Right)
            {
                this.RemoveTileAt(x, y);
            }
            this._BoardCtl.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ToolGameObjectAction(int x, int y, int rowx, int rowy, MouseEventArgs mouseArgs)
        {
            if (this.CurrentStage == null)
                return;

            if (this._ObjectSelectorCtl.SelectedObject == null)
                return;

            StageObject obj = this._ObjectSelectorCtl.SelectedObject.Clone();
            obj.UniqueId = this.CurrentStage.GetAvailableObjectUniqueId(obj.GetType());
            System.Drawing.Point pt = obj.GetPositionInBoardFromPoint(new System.Drawing.Point(x, y));

            obj.Position = new Vector2(pt.X, pt.Y);
            obj.UpdateBoundingBox();

            StageObject objToFind = this.CurrentStage.GetObjectAt((int)obj.Position.X, (int)obj.Position.Y);
            if (objToFind != null)
            {
                if (objToFind.GetType() == obj.GetType())
                {
                    throw new ApplicationException("Cannot put two objects of the same type at the same location.");
                }
            }

            this.CurrentStage.AddObject(obj);
            if (this.EditingMode != StageEditingMode.Hints)
            {
                if (obj.EditorBehaviour.OpenPropertiesWhenAdded)
                {
                    this.ShowObjectProperties(obj);
                }
            }

            this._BoardCtl.Refresh();
        }


        #endregion

        #region Tool strip events
        /// <summary>
        /// 
        /// </summary>
        private void _btnSelectToolTiles_Click(object sender, EventArgs e)
        {
            try
            {
                 this.SelectedToolType = ToolType.Select;
                 this.SelectionMode = SelectionType.Tiles;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnSelectToolObjects_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedToolType = ToolType.Select;
                this.SelectionMode = SelectionType.Objects;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnSelectToolAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedToolType = ToolType.Select;
                this.SelectionMode = SelectionType.TilesAndObjects;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
        #endregion

        #region Selections
        /// <summary>
        /// 
        /// </summary>
        private void ClearSelection()
        {
            this._BoardCtl.ClearSelection();
        }
        #endregion

        #region Select menu
        /// <summary>
        /// 
        /// </summary>
        private void _optDeselect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearSelection();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
        #endregion

        #region Edit menu
        private void _optDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.DeleteSelectedObjects();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        void DeleteObject(StageObject obj)
        {
            if (this.CurrentStage == null)
                return;
            this.CurrentStage.RemoveObject(obj);
            this._BoardCtl.SelectedItems.Remove(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        void DeleteSelectedObjects()
        {
          
            if (this._BoardCtl.SelectedItems.Count > 1)
            {
                if (MessageBox.Show(this, "Remove selected tiles/objects?", Settings.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                     == DialogResult.No)
                    return;
            }

            for (int i = 0; i < this._BoardCtl.SelectedItems.Count; i++)
            {
                if (this._BoardCtl.SelectedItems[i] as StageObject != null)
                {
                    this.CurrentStage.RemoveObject((StageObject)this._BoardCtl.SelectedItems[i]);
                    this._BoardCtl.SelectedItems.RemoveAt(i);
                    i--;
                }
                else
                if (this._BoardCtl.SelectedItems[i] as TileCell != null)
                {
                    TileCell cell = (TileCell)this._BoardCtl.SelectedItems[i];
                    this.RemoveTileAt(cell.BoardX, cell.BoardY);
                    this._BoardCtl.SelectedItems.RemoveAt(i);
                    i--;
                }
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        void ShowObjectProperties(StageObject obj)
        {
            if (obj == null)
                return;

            if (this.CurrentStage == null)
                return;
#if STAGE_EDITOR
            obj.EditProperties(this, this.CurrentStage);
#endif
//            ObjectPropsBaseForm.Show(this, this.CurrentStage, obj);
            this._BoardCtl.Refresh();
            this._propBrowser.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 27)
                    this.Close();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optEditToolsMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CurrentStage == null)
                    return;

                EditToolsMenuForm form = new EditToolsMenuForm();
                form.ShowDialog(this, this.CurrentStage);
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optStageDataEditor_Click(object sender, EventArgs e)
        {
            try
            {
                StageDataEditorForm form = new StageDataEditorForm();
                form.ShowDialog(this);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void _BoardCtl_DeleteItemClicked(object sender, EventArgs e)
        {
            try
            {
                this.DeleteSelectedObjects();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _BoardCtl_StageObjectPropertiesClicked(StageObject obj)
        {
            try
            {
                this.ShowObjectProperties(obj);
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>

        private void _mnuEdit_DropDownOpening(object sender, EventArgs e)
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

        /// <summary>
        /// 
        /// </summary>
        private void _mnuSelect_DropDownOpening(object sender, EventArgs e)
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

        /// <summary>
        /// 
        /// </summary>
        private void _optSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                this._BoardCtl.SelectAll();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optProperties_Click(object sender, EventArgs e)
        {
            try
            {
                StagePropertiesForm form = new StagePropertiesForm();
                form.ShowDialog(this.CurrentStage);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optRotate_Click(object sender, EventArgs e)
        {
            try
            {
                this.RotateObjects();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RotateObjects()
        {
            if (this.CurrentStage == null)
            {
                return;
            }

            foreach (StageObject obj in this._BoardCtl.SelectedObjects)
            {
                this.CurrentStage.RotateObject(obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _BoardCtl_MoveToFrontClicked(StageObject obj)
        {
            try
            {
                this.MoveObjectToFront(obj);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void _BoardCtl_SendToBackClicked(StageObject obj)
        {
            try
            {
                this.SendObjectToBack(obj);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void MoveObjectToFront(StageObject obj)
        {
            this.CurrentStage.MoveObjectToFront(obj);
            this.Refresh();
        }

        private void SendObjectToBack(StageObject obj)
        {
            this.CurrentStage.SendObjectToBack(obj);
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveTileAt(int col, int row)
        {
            ITileToolboxItem tileToolboxItem = this._TileSelectorCtl.GetTileToolboxItemByTile(StageEditor.CurrentStageEdited.Stage.Board.GetTileAt(row, col));
            if (tileToolboxItem != null)
            {
                tileToolboxItem.OnBoardRemove(col, row);
            }
            this.CurrentStage.RemoveTileAt(col, row);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _SolutionCtl_SolutionNodeSelected(SolutionCtl.NodeType nodeType)
        {
            try
            {
                switch (nodeType)
                {
                    case SolutionCtl.NodeType.CustomStage:
                        this.LoadStage(true, this._SolutionCtl.SelectedStageResourceId);
                        break;

                    case SolutionCtl.NodeType.Stage:
                        this.LoadStage(false, this._SolutionCtl.SelectedStageResourceId);
                        break;
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
        private void LoadStage(bool customStage, string resourceId)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this._SolutionCtl.SelectedStageResourceId == null)
                {
                    return;
                }

                if (customStage)
                {
                    this.CurrentStage = this.CurrentSolution.LoadCustomStage(this._SolutionCtl.SelectedStageResourceId);
                }
                else
                {
                    this.CurrentStage = this.CurrentSolution.LoadStage(this._SolutionCtl.SelectedStageResourceId);
                }

            }
            finally
            {
                this.Cursor = this.DefaultCursor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void MainForm_CurrentStageThemeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CurrentStage == null ||
                    this.CurrentStage.Stage == null ||
                    this.CurrentStage.LevelStage == null)
                {
                    return;
                }

                Levels.CurrentTheme = this.CurrentStage.Stage.LevelStage.ThemeId;
                Levels._instance.ReloadStageData();
                this._currentStage.Stage.RefreshTiles();
                this._BoardCtl.Refresh();
                this.FillTileSelector();
                this.FillObjectSelector();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateNewCustomStage()
        {
            NewCustomStageForm form = new NewCustomStageForm();
            if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                EditorCustomStage stage = EditorCustomStage.CreateNew(form.LevelStage, this.CurrentSolution);
                this._SolutionCtl.AddCustomStage(stage.LevelStage);
                this.CurrentStage = stage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optDeleteStage_Click(object sender, EventArgs e)
        {
            try
            {
                this.DeleteSelectedStage();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DeleteSelectedStage()
        {
            if (this.CurrentStage == null)
            {
                return;
            }
            if (this.CurrentStage.Stage.IsCustomStage == false)
            {
                MessageBox.Show(this, "Cannot delete this stage.\nOnly custom stages can be deleted.", Settings.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!File.Exists(this.CurrentStage.LevelStage.CustomStageFilename))
            {
                return;
            }
            if (MessageBox.Show(this, "Delete selected stage?\nOperation can't be undone.", Settings.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 
                            System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            File.Delete(this.CurrentStage.LevelStage.CustomStageFilename);
            this._SolutionCtl.RemoveStage(this.CurrentStage.LevelStage);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optPrefs_Click(object sender, EventArgs e)
        {
            try
            {
                PreferencesForm form = new PreferencesForm();
                form.ShowDialog(this);
                if (form.CustomStagesFolderChanged)
                {
                    this._SolutionCtl.Refresh();
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
        private void _optSelAll_Click(object sender, EventArgs e)
        {
            try
            {
                this._BoardCtl.SelectAll();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void _pnlTools_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void _optMoveTilesAndObjects_Click(object sender, EventArgs e)
        {
            try
            {
                MoveTilesAndObjectsForm form = new MoveTilesAndObjectsForm();
                if (form.ShowDialog(this, this.CurrentStage) == System.Windows.Forms.DialogResult.OK)
                {
                    this._BoardCtl.Refresh();
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
        private void _optRunTileMatching_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "This option runs the tile matching algorithm in all tiles\non the stage.\n" +
                                          "This is done by re-adding all the tiles.", 
                                          "Tile Matching Algorithm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                    DialogResult.OK)
                {
                    this.CurrentStage.RunTileMatchingAlgorithm();
                    this._BoardCtl.Refresh();
                }

            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void _optShowTiles_Click(object sender, EventArgs e)
        {
            try
            {
                UserSettings.ShowTiles = !UserSettings.ShowTiles;
                this._BoardCtl.Refresh();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void _optShowCameraFrame_Click(object sender, EventArgs e)
        {
            try
            {
                UserSettings.ShowCameraFrame = !UserSettings.ShowCameraFrame;
                this._BoardCtl.Refresh();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optHints_Click(object sender, EventArgs e)
        {
            try
            {
                this.OpenHintEditForm();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optEndHintEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.EditingMode == StageEditingMode.Hints)
                {
                    this._BoardCtl.Refresh();
                    this.OpenHintEditForm();
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
        private void OpenHintEditForm()
        {
            this.EditingMode = StageEditingMode.Hints;
            this.EnableButtons();
            HintsForm form = new HintsForm();
            if (form.ShowDialog(this.CurrentStage) == System.Windows.Forms.DialogResult.OK)
            {
                switch (form.DialogAction)
                {
                    case HintsForm.HintsDialogResult.Add:
                    case HintsForm.HintsDialogResult.Edit:
                        this.CurrentStage.Stage.HintManager.CurrentHint = form.SelectedHint;
                        this.EnableButtons();
                        return;

                }
            }
            this.EndHintEditingMode();
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndHintEditingMode()
        {
            this.EditingMode = StageEditingMode.Stage;
            this.ShowOnlyHints = false;
            this._BoardCtl.Refresh();
            this.EnableButtons();
        }

        private void _optShowOnlyHints_Click(object sender, EventArgs e)
        {
            try
            {
                this.ShowOnlyHints = !this.ShowOnlyHints;
                this._BoardCtl.Refresh();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        public void RefreshBoard()
        {
            this._BoardCtl.Refresh();
        }
     }
}
