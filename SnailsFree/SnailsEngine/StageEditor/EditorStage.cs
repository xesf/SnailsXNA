using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;
using LevelEditor.Controls;
using TwoBrainsGames.Snails;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageEditor;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.BrainEngine.Collision;
using System.IO;
using LevelEditor.Forms;

namespace LevelEditor
{
    public class EditorStage
    {
        #region Events
        public event EventHandler SizeChanged;
        #endregion

        #region Const
        public const string DEF_NAME = "noname";
        private const string CATEGORY_MISC = "Misc";
        private const string CATEGORY_MEDALS = "Medals";
        private const string CATEGORY_LIGHT = "Lighting";
        #endregion

        #region variables
        string _Name;
        public static EditorStage _currentStage;
        #endregion

        #region Properties
        // Current stage that is being edited
        public static EditorStage CurrentStage
        {
            get
            {
                return EditorStage._currentStage;
            }
            set
            {
                EditorStage._currentStage = value;
            }        }

        [Browsable(false)]
        public LevelStage LevelStage 
        {
            get { return this.Stage.LevelStage; }
        }

        [Browsable(false)]
        public Stage Stage { get; set; }

        [Browsable(false)]
        public Board Board
        {
            get { return this.Stage.Board; }
            set { this.Stage.Board = value; }
        }

        [Browsable(false)]
        public List<StageObject> Objects
        {
            get { return this.Stage.Objects; }
        }

        [Browsable(false)]
        public Solution ParentSolution { get; private set; }

        [Browsable(false)]
        public bool Changed { get; set; }

        [Browsable(false)]
        public string _desc { get { return this.Stage.Description; } set { this.Stage.Description = value; } }

        [Browsable(false)]
        public List<SnailsBackgroundLayer> Layers { get { return this.Stage.Layers; } }

        [Browsable(false)]
        public Size TileSize
        {
            get { return new Size(Stage.TILE_WIDTH, Stage.TILE_HEIGHT); }
        }

        [Browsable(false)]
        public int Index
        {
            get
            {
                for (int i = 0; i < this.ParentSolution.Stages.Count; i++)
                {
                    if (this.ParentSolution.Stages[i] == this)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        [Browsable(false)]
        public Size SizeInPixels
        {
            get
            {
                return new Size(this.Size.Width * this.TileSize.Width,
                                this.Size.Height * this.TileSize.Height);
            }
        }
        #endregion

        #region Browsable props
        [Browsable(true),
         Category(CATEGORY_MISC)]
        public string Name
        {
            get { return this._Name; }
            set
            {
                if (this._Name != value)
                {
                    this._Name = value;
                    this.Changed = true;
                }
            }
        }

        // Size in tiles
        [Browsable(true),
         Category(CATEGORY_MISC)]
        public Size Size
        {
            get { return new Size(this.Stage.Board.Columns, this.Stage.Board.Rows); }
            set
            {
                this.SetBoardSize(value);
                this.Changed = true;
                this.OnSizeChanged();
            }
        }

        [Browsable(true),
         Category(CATEGORY_MISC)]
        public bool  WithShadows
        {
            get
            {
                if (this.Stage == null)
                    return false;
                return this.Stage._withShadows;
            }
            set { this.Stage._withShadows = value; }
        }

        [Browsable(true),
         Category(CATEGORY_MISC)]
        public GoalType MissionType
        {
            get 
            {
                if (this.Stage == null)
                    return GoalType.SnailDelivery;
                return this.Stage.LevelStage._goal; 
            }
            set { this.Stage.LevelStage._goal = value; }
        }

        [Browsable(true),
         Category(CATEGORY_MISC)]
        public TimeSpan TargetTime
        {
            get
            {
                if (this.Stage == null)
                    return TimeSpan.MinValue;
                return this.Stage.LevelStage._targetTime;
            }
            set 
            { 
                this.Stage.LevelStage._targetTime = value; 
            }
        }

        [Browsable(true),
         Category(CATEGORY_MISC)]
        public int SnailsToSave
        {
            get
            {
                if (this.Stage == null)
                    return 0;
                return this.Stage.LevelStage._snailsToSave;
            }
            set { this.Stage.LevelStage._snailsToSave = value; }
        }

        [Browsable(true),
         Category(CATEGORY_MISC)] // Total snails to be released
        public int TotalSnails
        {
            get
            {
                if (this.Stage == null)
                    return 0;
                return this.Stage.GetTotalSnailsToRelease();
            }
        }

        [Browsable(true),
         Category(CATEGORY_MISC)]
        public string StartupTutorialTopics
        {
            get
            {
                if (this.Stage == null)
                    return null;
                return this.Stage.StartupTopicsString;
            }
            set { this.Stage.StartupTopicsString = value; }
        }

        [Browsable(true),
         Category(CATEGORY_MISC)]
        public Microsoft.Xna.Framework.Vector2 StartupCameraPosition
        {
            get
            {
                if (this.Stage == null)
                    return Microsoft.Xna.Framework.Vector2.Zero;
                return this.Stage.StartupCenter;
            }
            set { this.Stage.StartupCenter = value; }
        }


        [Browsable(true),
         Category(CATEGORY_MISC)]
        public Microsoft.Xna.Framework.Vector2 BackgroundOffset
        {
            get
            {
                if (this.Stage == null)
                    return Microsoft.Xna.Framework.Vector2.Zero;
                return this.Stage._backgroundLayersOffset;
            }
            set { this.Stage._backgroundLayersOffset = value; }

        }

        [Browsable(true),
        Category(CATEGORY_MISC)]
        public bool AvailableInDemo
        {
            get
            {
                if (this.Stage == null)
                    return true;
                return this.Stage.LevelStage.AvailableInDemo;
            }
            set
            {
                this.Stage.LevelStage.AvailableInDemo = value; 
            }

        }
        [Browsable(true),
         Category(CATEGORY_MEDALS),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MedalScoreCriteria A_GoldMedal
        {
            get
            {
                return this.Stage.GoldMedalScoreCriteria;
            }
            set { this.Stage.GoldMedalScoreCriteria = value; }
        }

        [Browsable(true),
        Category(CATEGORY_MEDALS),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MedalScoreCriteria B_SilverMedal
        {
            get
            {
                return this.Stage.SilverMedalScoreCriteria;
            }
            set { this.Stage.SilverMedalScoreCriteria = value; }
        }
        
        [Browsable(true),
        Category(CATEGORY_LIGHT)]
        public bool LightEnabled 
        {
            get
            {
                if (this.Stage == null)
                    return false;
                return this.Stage.LightManager.LightEnabled;
            }
            set
            {
                this.Stage.LightManager.LightEnabled = value;
            }
        }

        [Browsable(true),
        Category(CATEGORY_LIGHT)]
        public Microsoft.Xna.Framework.Color LightColor
        {
            get
            {
                if (this.Stage == null)
                    return Microsoft.Xna.Framework.Color.Black;
                return this.Stage.LightManager.LightColor;
            }
            set
            {
                this.Stage.LightManager.LightColor = value;
            }
        }

        public List<StageObject> CurrentEditedObjects
        {
            get
            {
                if (MainForm._instance.EditingMode == MainForm.StageEditingMode.Hints)
                {
                    if (this.Stage.HintManager.CurrentHint != null)
                    {
                        return this.Stage.HintManager.CurrentHint.StageObjectItems;
                    }
                    return null;
                }
                
                return this.Stage.Objects;
            }
        }
        #endregion

        #region Constructs

        /// <summary>
        /// 
        /// </summary>
        public EditorStage(Solution parentSolution)
        {
            this.Initialize(parentSolution, new LevelStage());
        }

        /// <summary>
        /// 
        /// </summary>
        public EditorStage(Solution parentSolution, LevelStage levelStage)
        {
            this.Initialize(parentSolution, levelStage);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Initialize(Solution parentSolution, LevelStage levelStage)
        {
            this.Name = levelStage.StageId;
            this.ParentSolution = parentSolution;
            this.Stage = new Stage();
            this.Stage.LevelStage = levelStage;
            this.Stage.Id = levelStage.StageId;
            this.InitializeBoard(5, 5);
            this.Changed = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void InitializeBoard(int cols, int rows)
        {
            this.Board = new Board(cols, rows);
        }
        #endregion

        #region Board methods
        /// <summary>
        /// 
        /// </summary>
        public Tile GetTileAt(int col, int row)
        {
            if (this.InBoardBounds(row, col) == false)
                return null;

            if (this.Board.Tiles[row, col] == null)
                return null;

            return this.Board.Tiles[row, col].Tile;
        }
        /*
        /// <summary>
        /// 
        /// </summary>
        public void SetTileAt(int col, int row)
        {
          if (this.InBoardBounds(col, row) == false)
          {
            return;
          }

          this.Board.Tiles[row, col] = Settings.StageData.Tiles["TILE_001"];
        }
        */
        /// <summary>
        /// 
        /// </summary>
        public bool InBoardBounds(int col, int row)
        {
            if (this.Board == null)
                return false;
            if (this.Board.Tiles.GetLength(0) <= row)
                return false;
            if (this.Board.Tiles.GetLength(1) <= col)
                return false;

            return true;
        }
        #endregion

        #region Tile methods

        /// <summary>
        /// 
        /// </summary>
        public StageObject GetObjectAt(int px, int py)
        { 
            foreach (StageObject obj in this.CurrentEditedObjects)
            {
                if (obj.Position.X == px &&
                    obj.Position.Y == py)
                {
                    return obj;
                }

            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        public TileCell SetTileAt(int styleGroupId, int x, int y)
        {
            TileCell newTc;
            if (this.Stage == null)
                return null;
            if (MainForm._instance.EditingMode == MainForm.StageEditingMode.Hints)
            {
                Tile newTile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId,WalkFlags.All);
                newTc = new TileCell(newTile, x, y);
                this.Stage.HintManager.CurrentHint.AddTile(newTc);
            }
            else
            {
                newTc = this.Stage.Board.SetTileAt(styleGroupId, x, y);
            }
            this.Changed = true;

            return newTc;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveTileAt(int x, int y)
        {
            if (this.Stage == null)
                return;

            if (MainForm._instance.EditingMode == MainForm.StageEditingMode.Hints)
            {
                this.Stage.HintManager.CurrentHint.RemoveTileAt(x,y);
            }
            else
            {
                this.Stage.Board.RemoveCellAt(x, y);
            }
            this.Changed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveObject(StageObject obj)
        {
            if (this.Stage == null)
                return;
            if (MainForm._instance.EditingMode == MainForm.StageEditingMode.Stage)
            {
                this.Stage.RemoveObject(obj);
                this.Stage.ProcessObjectsToRemove();
            }
            else
            {
                this.Stage.HintManager.CurrentHint.RemoveObject(obj);
            }
            this.Changed = true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void RotateObjects(List<StageObject> objList)
        {
            foreach (StageObject obj in objList)
            {
                this.RotateObject(obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RotateObject(StageObject obj)
        {
            if (obj.EditorBehaviour.AllowRotation == false)
            {
                return;
            }

            obj.Rotation += 90;
            if (obj.Rotation > 270)
            {
                obj.Rotation = 0;
            }
            obj.Position = obj.GetPositionInBoardFromPoint(obj.Position + obj.Sprite.Offset);
            this.Changed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendObjectToBack(StageObject obj)
        {
            this.Stage.Objects.Remove(obj);
            this.Stage.Objects.Insert(0, obj);
            this.Changed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void MoveObjectToFront(StageObject obj)
        {
            this.Stage.Objects.Remove(obj);
            this.Stage.Objects.Insert(this.Stage.Objects.Count, obj);
            this.Changed = true;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public virtual void Save(bool performValidation)
        {
            if (string.IsNullOrEmpty(this.Stage.Key))
            {
                this.Stage.LevelStage.StageKey = Guid.NewGuid().ToString();
            }

            if (performValidation)
            {
                try
                {
                    this.Validate();
                }
                catch (StageValidationException ex)
                {
                    throw new StageValidationException("Stage cannot be saved.\n" + ex.Message);
                }
            }

            // Update LevelStage
            this.Stage.LevelStage._snailsToRelease = this.Stage.GetTotalSnailsToRelease();
            this.Stage.LevelStage._goldMedalTime = this.Stage.GoldMedalScoreCriteria.TimeNeeded;
            this.Stage.LevelStage._goldMedalScore = this.Stage.GoldMedalScoreCriteria.Score;
            // Make the stage enabled, disabled stages will not save all data in ToDataFileRecord
            // This is a safety measure for unavailable release stages
            // Disabled stages will not be fully exported in compile time	
            this.Stage.IncrementBuildNr();
           
            DataFile dataFile = new DataFile();
            dataFile.RootRecord = this.Stage.ToDataFileRecord(ToDataFileRecordContext.StageSave);
            XmlDataFileWriter writer = new XmlDataFileWriter();
            string folder = System.IO.Path.Combine(GameSettings.StagesOutputFolder, @"stages\" + this.LevelStage.ThemeId.ToString());

            string filename = this.Stage.Id + ".xdf";
            writer.Write(System.IO.Path.Combine(folder, filename), dataFile);
            this.Changed = false;

            // Update Levels file, this synchronizes stage goals (they are doubled in Levels and in each stage)
            folder = System.IO.Path.Combine(GameSettings.StagesOutputFolder, "stages");
            filename = "levels.xdf";
            int idx = this.ParentSolution.Levels.FindStageIndexById(this.Stage.LevelStage.StageId);
            this.ParentSolution.Levels.Stages[idx] = this.Stage.LevelStage;
            this.ParentSolution.Levels.Save(System.IO.Path.Combine(folder, filename));

        }

        /// <summary>
        /// Check if the stage has invalid parameters
        /// </summary>
        public void Validate()
        {
            // Check gold medal criteria
            if (this.Stage.CountPickableObjects(PickableObject.PickableType.GoldCoin) <
                this.Stage.GoldMedalScoreCriteria.GoldCoinsNeeded)
            {
                throw new StageValidationException("Stage has less gold coins then those needed to get the gold medal.");
            }

            if (this.Stage.CountPickableObjects(PickableObject.PickableType.SilverCoin) <
                this.Stage.GoldMedalScoreCriteria.SilverCoinsNeeded)
            {
                throw new StageValidationException("Stage has less silver coins then those needed to get the gold medal.");
            }

            if (this.Stage.CountPickableObjects(PickableObject.PickableType.CopperCoin) <
                this.Stage.GoldMedalScoreCriteria.BronzeCoinsNeeded)
            {
                throw new StageValidationException("Stage has less bronze coins then those needed to get the gold medal.");
            }

            if (this.TotalSnails < this.Stage.GoldMedalScoreCriteria.SnailsNeeded)
            {
                throw new StageValidationException("Stage has less snails then those needed to get the gold medal.");
            }


            // Check silver medal criteria
            if (this.Stage.CountPickableObjects(PickableObject.PickableType.GoldCoin) <
                this.Stage.SilverMedalScoreCriteria.GoldCoinsNeeded)
            {
                throw new StageValidationException("Stage has less gold coins then those needed to get the silver medal.");
            }

            if (this.Stage.CountPickableObjects(PickableObject.PickableType.SilverCoin) <
                this.Stage.SilverMedalScoreCriteria.SilverCoinsNeeded)
            {
                throw new StageValidationException("Stage has less silver coins then those needed to get the silver medal.");
            }

            if (this.Stage.CountPickableObjects(PickableObject.PickableType.CopperCoin) <
                this.Stage.SilverMedalScoreCriteria.BronzeCoinsNeeded)
            {
                throw new StageValidationException("Stage has less bronze coins then those needed to get the silver medal.");
            }

            if (this.TotalSnails < this.Stage.SilverMedalScoreCriteria.SnailsNeeded)
            {
                throw new StageValidationException("Stage has less snails then those needed to get the gold medal.");
            }

            // For gold medals is mandatory:
            // -Save/kill all snails
            // -Pick up all medals
            // Check gold medal criteria
            if (this.Stage.CountPickableObjects(PickableObject.PickableType.GoldCoin) !=
                this.Stage.GoldMedalScoreCriteria.GoldCoinsNeeded)
            {
                throw new StageValidationException("All gold coins are mandatory to get a gold medal. Check gold medal requirements.");
            }
            if (this.Stage.CountPickableObjects(PickableObject.PickableType.SilverCoin) !=
                this.Stage.GoldMedalScoreCriteria.SilverCoinsNeeded)
            {
                throw new StageValidationException("All silver coins are mandatory to get a gold medal. Check gold medal requirements.");
            }
            if (this.Stage.CountPickableObjects(PickableObject.PickableType.CopperCoin) !=
                this.Stage.GoldMedalScoreCriteria.BronzeCoinsNeeded)
            {
                throw new StageValidationException("All bronze coins are mandatory to get a gold medal. Check gold medal requirements.");
            }

            if (this.Stage.LevelStage._goal == GoalType.SnailKiller && 
                (this.TotalSnails != this.Stage.GoldMedalScoreCriteria.SnailsNeeded ||
                 this.TotalSnails != this.Stage.SilverMedalScoreCriteria.SnailsNeeded))
            {
                throw new StageValidationException("Killing all snails is mandatory in Snail Killer. Check medals requirements (snails needed).");
            }

            if (this.Stage.LevelStage._targetTime.TotalSeconds < this.Stage.GoldMedalScoreCriteria.TimeNeeded.TotalSeconds)
            {
                throw new StageValidationException("Target time can't be smaller then gold medal time.");
            }

            if (this.Stage.LevelStage._targetTime.TotalSeconds < this.Stage.SilverMedalScoreCriteria.TimeNeeded.TotalSeconds)
            {
                throw new StageValidationException("Target time can't be smaller then silver medal time.");
            }

        }

        #region Events
        /// <summary>
        /// 
        /// </summary>
        void OnSizeChanged()
        {
            if (this.SizeChanged != null)
            {
                this.SizeChanged(this, new EventArgs());
            }
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public List<StageObject> GetObjectsAt(int px, int py)
        {
            List<StageObject> list = new List<StageObject>();
            Microsoft.Xna.Framework.Point pt = new Microsoft.Xna.Framework.Point(px, py);
            foreach (StageObject obj in this.Stage.Objects)
            {
                Microsoft.Xna.Framework.Rectangle rc = obj.SelectionRectXna;
                if (rc.Contains(pt))
                {
                    list.Add(obj);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<T> GetObjectsOfType<T>()
        {
            List<T> list = new List<T>();
            foreach (StageObject obj in this.Stage.Objects)
            {
                //   if (obj is T)
                //       list.Add(obj as T);
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SnailCounter> GetSnailCounters()
        {
            List<SnailCounter> list = new List<SnailCounter>();
            foreach (StageObject obj in this.Stage.Objects)
            {
                if (obj is SnailCounter)
                {
                    list.Add(obj as SnailCounter);
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<StageObject> GetObjectsByType(Type type)
        {
            List<StageObject> list = new List<StageObject>();
            foreach (StageObject obj in this.Stage.Objects)
            {
                if (obj.GetType() == type)
                {
                    list.Add(obj);
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<StageObject> GetObjectsByType(StageObjectType type)
        {
            List<StageObject> list = new List<StageObject>();
            foreach (StageObject obj in this.Stage.Objects)
            {
                if (obj.Type == type)
                {
                    list.Add(obj);
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<StageObject> GetObjectsByType(Type type, List<StageObject> currentList)
        {
            foreach (StageObject obj in this.Stage.Objects)
            {
                if (obj.GetType() == type)
                {
                    currentList.Add(obj);
                }
            }

            return currentList;
        }

        /// <summary>
        /// 
        /// </summary>
        public StageObject FindObjectById(string uniqueId)
        {
            foreach (StageObject obj in this.Stage.Objects)
            {
                if (string.Compare(obj.UniqueId, uniqueId, true) == 0)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ObjectIdIsAvailable(string uniqueId, StageObject objToIgnore)
        {
            foreach (StageObject obj in this.Stage.Objects)
            {
                if (string.Compare(obj.UniqueId, uniqueId, true) == 0 && obj != objToIgnore)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetAvailableObjectUniqueId(Type objType)
        {
            int n = 1;
            int i = 0;
            string objId;
            do
            {
                objId = string.Format("{0}_{1:00}", objType.Name, n);
                if (i < this.Stage.Objects.Count)
                {
                    if (string.Compare(this.Stage.Objects[i].UniqueId, objId, true) == 0)
                    {
                        n++;
                        i = 0;
                        continue;
                    }
                }
                i++;
            }
            while (i < this.Stage.Objects.Count);

            return objId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(StageObject obj)
        {
            if (MainForm._instance.EditingMode == MainForm.StageEditingMode.Stage)
            {
                this.Stage.AddObjectFromStageEditor(obj);
            }
            else
            {
                this.Stage.HintManager.CurrentHint.AddObject(obj);
            }
            this.Changed = true;
        }

        /// <summary>
        /// This msgbox shouldn't be here, this should be made in the main form when it catches the size changed event
        /// But the event handler has to be changed, and I don't feel like doing it...
        /// </summary>
        private void SetBoardSize(Size newSize)
        {
            if (this.Stage.IsCropAreaEmpty(newSize.Width, newSize.Height) == false)
            {
                if (MessageBox.Show(Globals.MainForm, "Area to be cropped is not empty. Proceed anyway?", Settings.AppName,
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            this.Stage.ResizeBoard(newSize.Width, newSize.Height);

        }

        /// <summary>
        /// Checks if any of the objects on the specified list is out of bounds in the 
        /// The offset is added to the object position
        /// This is used when a drag drop ends to check if any of the objects are out of the bounds
        /// in their new positions
        /// </summary>
        public bool CheckAnyObjectOutOfBoard(List<Object> objList, float offsetx, float offsety)
        {
            foreach (Object obj in objList)
            {
                if (this.CheckObjectOutOfBoard(obj, offsetx, offsety))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        public bool CheckAnyObjectOrTileOutOfBoard(float offsetx, float offsety)
        {
            foreach (StageObject obj in this.Objects)
            {
                if (this.CheckObjectOutOfBoard(obj, offsetx, offsety))
                {
                    return true;
                }
            }

            for (int i = 0; i < this.Board.Columns; i++)
            {
                for(int j = 0; j < this.Board.Rows; j++)
                {
                    if (this.Board.Tiles[j, i] != null &&
                        this.Board.Tiles[j, i].Tile != null)
                    {
                        if (this.CheckObjectOutOfBoard(this.Board.Tiles[j, i], offsetx, offsety))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckObjectOutOfBoard(Object obj, float offsetx, float offsety)
        {
            if (obj as StageObject != null)
            {
                StageObject stageObj = (StageObject)obj;
                
                Microsoft.Xna.Framework.Rectangle rc = stageObj.SelectionRectXna;
                BoundingSquare bs = new BoundingSquare(new Microsoft.Xna.Framework.Vector2(rc.Left + offsetx, rc.Top + offsety),
                                                rc.Width, rc.Height);
                if (stageObj.EditorBehaviour.AllowOutOfTheBoard == false)
                {
                    if (!this.Stage.Board.BoundingBox.Contains(bs))
                    {
                        return true;
                    }
                }
                else // Permitir objectos fora do board (desde que não esteja totalmente fora)
                {
                    if (!this.Stage.Board.BoundingBox.Contains(bs) && !this.Stage.Board.BoundingBox.Intersects(bs))
                    {
                        return true;
                    }
                }
            }
            if (obj as TileCell != null)
            {
                TileCell cell = (TileCell)obj;
                float rowx = (offsetx / this.TileSize.Width);
                float rowy = (offsety / this.TileSize.Height);
                BoundingSquare bs = new BoundingSquare(new Microsoft.Xna.Framework.Vector2((rowx + cell.BoardX) * this.TileSize.Width,
                                                    (rowy + cell.BoardY) * this.TileSize.Height),
                                                    this.TileSize.Width,
                                                    this.TileSize.Height);
                if (!this.Stage.Board.BoundingBox.Contains(bs))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Object> MoveObjectsAndTiles(List<object> objectsAndTilesList, int offsetX, int offsetY, int offsetColRowX, int offsetColRowY)
        {
            List<TileCell> tmpCells = new List<TileCell>(); // This temporary list is used to re-select removed and readded tiles
            List<Object> objectsAndTilesMoved = new List<object>(); // This list holds the objects that are removed because they go out of the board
            foreach (Object obj in objectsAndTilesList)
            {
                // Ignore objects that are out of bounds
                if (obj is StageObject)
                {
                    if (this.CheckObjectOutOfBoard(obj, offsetX, offsetY) == true)
                    {
                        this.RemoveObject((StageObject)obj);
                        continue;
                    }
                }
                else
                {
                    if (obj is TileCell)
                    {
                        if (this.CheckObjectOutOfBoard(obj, offsetColRowX * this.TileSize.Width,
                                                            offsetColRowY * this.TileSize.Height) == true)
                        {
                            TileCell cell = (TileCell)obj;
                            this.RemoveTileAt(cell.BoardX, cell.BoardY);
                            continue;
                        }
                    }
                }
                if (obj is StageObject)
                {
                    StageObject stageObj = (StageObject)obj;
                    stageObj.Position += new Microsoft.Xna.Framework.Vector2(offsetX, offsetY); // new Microsoft.Xna.Framework.Vector2(x, y);
                    stageObj.Position = stageObj.GetPositionInBoardFromPoint(stageObj.Position);
                    stageObj.UpdateBoundingBox();
                    objectsAndTilesMoved.Add(stageObj);
                }

                if (obj is TileCell)
                {
                    TileCell cell = (TileCell)obj;
                    tmpCells.Add(cell.Clone());
                }
            }

            // First step, remove old tiles
            foreach (TileCell cell in tmpCells)
            {
                this.RemoveTileAt(cell.BoardX, cell.BoardY);
            }

            // Second step, add new tiles
            foreach (TileCell cell in tmpCells)
            {
                int rx = cell.BoardX + offsetColRowX;
                int ry = cell.BoardY + offsetColRowY;

                if (cell.Tile != null)
                {
                    TileCell newTC = this.SetTileAt(cell.Tile.StyleGroupId, rx, ry);
                    objectsAndTilesMoved.Add(newTC);
                }
            }

            this.Changed = true;

            return objectsAndTilesMoved;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Object> GetAllObjectsAndTiles()
        {
            List<Object> allObjects = new List<object>();

            if (MainForm._instance.EditingMode == MainForm.StageEditingMode.Stage)
            {
                foreach (StageObject obj in this.Objects)
                {
                    allObjects.Add(obj);
                }

                for (int i = 0; i < this.Board.Columns; i++)
                {
                    for (int j = 0; j < this.Board.Rows; j++)
                    {
                        if (this.Board.Tiles[j, i] != null &&
                            this.Board.Tiles[j, i].Tile != null)
                        {
                            allObjects.Add(this.Board.Tiles[j, i]);
                        }
                    }
                }
            }
            else
            {
                foreach (StageObject obj in this.Stage.HintManager.CurrentHint.StageObjectItems)
                {
                    allObjects.Add(obj);
                }
                foreach (TileCell tile in this.Stage.HintManager.CurrentHint.TileItems)
                {
                    allObjects.Add(tile);
                }

            }
            return allObjects;
        }

        /// <summary>
        /// Runs the tile matching algorithm on all tiles on the stage
        /// This will fix any wrong tile
        /// </summary>
        public void RunTileMatchingAlgorithm()
        {
            for (int i = 0; i < this.Board.Columns; i++)
            {
                for (int j = 0; j < this.Board.Rows; j++)
                {
                    if (this.Board.Tiles[j, i] != null &&
                        this.Board.Tiles[j, i].Tile != null)
                    {
                        Tile currentTile = this.Board.Tiles[j, i].Tile;
                        this.Board.SetTileAt(currentTile.StyleGroupId, i, j);
                    }
                }
            }

            this.Changed = true;
        }
    }
}
