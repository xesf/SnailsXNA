using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using System.Collections.Generic;
using TwoBrainsGames.Snails.Input;

namespace TwoBrainsGames.Snails.ToolObjects
{
    public enum ToolObjectType
    {
        None = -1,
        Box = 0,
        Salt,
        Vitamin,
        Trampoline,
        Copper,
        Dynamite,
        Apple,
        DynamiteBox,
        DynamiteBoxTriggered,
        DirectionalBoxCW,
        DirectionalBoxCCW,
        EndMission
    }

    public class ToolObject : Object2D, IBrainComponent, ISnailsDataFileSerializable
    {
        #region Constants
        const float ICON_LAYER_DEPTH = 0.82F;
        const float SELECTED_ICON_LAYER_DEPTH = 0.8F;
        const float COUNTER_LAYER_DEPTH = 0.92F;

        public const string RES_PLAYER_CURSOR_SELECTION = "spriteset/tools-menu-selected";
        #endregion

        string _quantityString;
        int _quantity;
        Vector2 _iconPosition;
        Vector2 _quantityPosition; 
        protected int _shortcutFrameIdx;      
        protected Input.GameplayInput.GamePlayButtons _shortcutKey;
        Vector2 _shortcutKeyPosition;
        string _onUseSoundRes;

        #region Properties
        public ToolObjectType Type;
        public TextFont Font;
        private bool _selected;
        protected bool _allowOnPaths;
        Sample _toolSelection;
        Sample _toolUseSound;

        public Color CursorDrawBlendColor;

        public int Quantity 
        {
            get { return this._quantity; }
            set 
            { 
                this._quantity = value;
                this._quantityString = this._quantity.ToString();
                if (this._quantity <= 0 && this._withQuantity)
                {
                    this.Sprite = this._spriteOutOfStock;
                }
                else
                {
                    this.Sprite = (this._selected? this._spriteWhenSelected : this._spriteWhenUnselected);
                }

            }
        }

        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                if (this._quantity == 0 && this._withQuantity)
                {
                    this.Sprite = this._spriteOutOfStock;
                }
                else
                {
                    this.Sprite = (this._selected ? this._spriteWhenSelected : this._spriteWhenUnselected);
                }
            }
        }

        public bool WithEnoughQuantity
        {
            get { return ((this._quantity > 0 && this._withQuantity) || !this._withQuantity); }
        }

        public int _toolboxIndex; // Index in the toolbox list (used to set the shotcut key frame)
        public bool SnapIt;
        public string ObjectId;
        protected Sprite _spriteWhenSelected; // Sprite when the tool is selected
        protected Sprite _spriteWhenUnselected; // Sprite when the tool has quantity > 0 and is unselected
        protected Sprite _spriteOutOfStock; // Sprite when the tool has quantity = 0 
        protected Sprite _spriteOutOfStockCross;
        protected Sprite _spriteShortcurKeys;
        public Sprite _iconSprite; // The icon that is used in the tools menu
        public Sprite ObjectSprite; // The object that is displayed along with the cursor

        protected Vector2 CounterPos;
        public BoundingSquare SelectionFrame;

        public float Width { get { return this.Sprite.Frames[0].Width; } }
        public float Height { get { return this.Sprite.Frames[0].Height ; } }

        protected bool _withQuantity; // If false, the tool is not limited by quantity

        public bool IsSelectable
        {
            get { return (this._quantity > 0 || (this._quantity == 0 && this._withQuantity == false)); }
        }

        

        #endregion

      
        /// <summary>
        /// 
        /// </summary>
        public ToolObject()
        {
            this.LayerDepth = ToolObject.ICON_LAYER_DEPTH;
            this.SnapIt = false;
            this._withQuantity = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public ToolObject(ToolObjectType type)
        {
            this.Type = type;
            this.CounterPos = new Vector2(5, 30);
            this.SnapIt = false;
            this._withQuantity = true;
            this._shortcutFrameIdx = (int)type;
            this._shortcutKey = Input.GameplayInput.GamePlayButtons.None;
        }

        public ToolObject(ToolObjectType type, int quantity)
            : this(type)
        {
            this.Quantity = quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Copy(ToolObject other)
        {
            base.Copy(other);

            this._spriteOutOfStock = other._spriteOutOfStock;
            this._spriteOutOfStockCross = other._spriteOutOfStockCross;
            this._spriteWhenSelected = other._spriteWhenSelected;
            this._spriteWhenUnselected = other._spriteWhenUnselected;
            this._iconSprite = other._iconSprite;
            this.Sprite = other.Sprite;
            this.Type = other.Type;
            this.ObjectId = other.ObjectId;
            this.Quantity = other.Quantity;
            this.Font = other.Font;
            this.SnapIt = other.SnapIt;
            this.ObjectSprite = other.ObjectSprite;
            this._spriteShortcurKeys = other._spriteShortcurKeys;
            this._toolSelection = other._toolSelection;
            this._allowOnPaths = other._allowOnPaths;
            this._shortcutKey = other._shortcutKey;
            this.BlendColor = other.BlendColor;
            this._onUseSoundRes = other._onUseSoundRes;
            this._toolUseSound = other._toolUseSound;
       }

        /// <summary>
        /// 
        /// </summary>
        public static ToolObject Create(ToolObjectType type)
        {
            ToolObject tool;

            switch (type)
            {
                case ToolObjectType.Box: tool = new ToolBox(); break;
                case ToolObjectType.Copper: tool = new ToolCopper(); break;
                case ToolObjectType.Dynamite: tool = new ToolDynamite(); break;
                case ToolObjectType.Vitamin: tool = new ToolVitamin(); break;
                case ToolObjectType.Apple: tool = new ToolApple(); break;
                case ToolObjectType.Salt: tool = new ToolSalt(); break;
                case ToolObjectType.DynamiteBox: tool = new ToolDynamiteBox(); break;
                case ToolObjectType.DynamiteBoxTriggered: tool = new ToolDynamiteBoxTriggered(); break;
                case ToolObjectType.Trampoline: tool = new ToolTrampoline(); break;
                case ToolObjectType.DirectionalBoxCW: tool = new ToolDirectionalBox(ToolObjectType.DirectionalBoxCW); break;
                case ToolObjectType.DirectionalBoxCCW: tool = new ToolDirectionalBox(ToolObjectType.DirectionalBoxCCW); break;
                case ToolObjectType.None:
                default:
                    throw new BrainException("Invalid ToolObjectType [" + type .ToString() + "]");
            }

            return tool;
        }

        /// <summary>
        /// 
        /// </summary>
        public ToolObject Clone()
        {
            ToolObject tool;

            tool = Create(this.Type);
            tool.Copy(this);

            return tool;
        }

        /// <summary>
        /// Useful to show the tutorial when a tool is selected
        /// </summary>
        public virtual void OnSelect()
        {
            if (_toolSelection != null && !_toolSelection.IsPlaying)
            {
                _toolSelection.Play();
            }
         
        }
        #region IBrainComponent Members

     

        public SpriteBatch SpriteBatch
        {
            get { return Levels.CurrentLevel.SpriteBatch; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadContent()
        {
            this._iconSprite = BrainGame.ResourceManager.GetSpriteTemporary(this);
            StageObject obj = Levels.CurrentLevel.StageData.GetObject(this.ObjectId);
            this.BlendColor = obj.BlendColor;
            this.ObjectSprite = BrainGame.ResourceManager.GetSpriteTemporary(obj);
            this._spriteWhenUnselected = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ToolIcon");
            this._spriteOutOfStock = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ToolIconOutOfStock");
            this._spriteOutOfStockCross = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ToolIconOutOfStockCross");
            this._spriteWhenSelected = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ToolIconSelected");
            this._spriteShortcurKeys = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ShortcutKeys");
            this.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/notebook-medium", ResourceManager.ResourceManagerCacheType.Static);

            _toolSelection = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TOOL_SELECTION);
            if (this._onUseSoundRes != null)
            {
                this._toolUseSound = BrainGame.ResourceManager.GetSampleTemporary(this._onUseSoundRes);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw()
        {
            // The tool background icon (the notebook page)
            this.Sprite.Draw(this.Position, SpriteBatch);
            // Tool icon
            this._iconSprite.Draw(this._iconPosition, SpriteBatch);

            // Quantity
            this.Font.DrawString(SpriteBatch, this._quantityString, this._quantityPosition, new Vector2(1f, 1f), Color.Blue);

            // Red cross when tool is out of stock
            if (this._quantity <= 0)
            {
                this._spriteOutOfStockCross.Draw(this.Position, SpriteBatch);
            }

            else
            {
                if (SnailsGame.GameSettings.WithToolSelectionShortcutKeys)
                {
                    this._spriteShortcurKeys.Draw(this._shortcutKeyPosition, this._shortcutFrameIdx, SpriteBatch);
                }
            }
#if DEBUG
            if (BrainGame.Settings.ShowBoundingBoxes)
            {
                this.SelectionFrame.Draw(Color.Red, Vector2.Zero);
                this.AABoundingBox.Draw(Color.Red, Vector2.Zero);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnloadContent()
        { }

        #endregion

        /// <summary>
        /// Set position should be used to set the tool position
        /// This away the positions are pre-computed
        /// </summary>
        public virtual void SetPosition(Vector2 pos)
        {
            this.Position = pos;
            this._iconPosition = pos + new Vector2(45.0f, 9.0f); // This could be replaced by BB. Not doing now because of DB&P
            this._quantityPosition = pos + new Vector2(20.0f, 15.0f);
            if (SnailsGame.GameSettings.WithToolSelectionShortcutKeys)
            {
                this._shortcutKeyPosition = pos + new Vector2(0f, this._spriteWhenSelected.Height - this._spriteShortcurKeys.Height);
            }
            this.SelectionFrame = new BoundingSquare(this.Position, this.Width, this.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        protected BoundingSquare TransformBB(Vector2 position)
        {
            return this.ObjectSprite.BoundingBox.Transform(position);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsValidAtPosition(Vector2 position)
        {
            if (_quantity <= 0 && _withQuantity)
                return false;

            if (position.X < 0 || position.Y < 0)
                return false;

            if (position.X >= Stage.CurrentStage.Board.Width)
                return false;

            if (position.Y >= Stage.CurrentStage.Board.Height)
                return false;

            if (this.SnapIt)
            {
                return true;
            }


            // Checks if a tool is valid at a specified position
            // A tool is valid if there's no tile at the object's BoundingBox
            // Override IsValidAtPosition() to change this behaviour

            // Check if the object is not out of the board
            BoundingSquare bb = this.TransformBB(position);
            if (!Stage.CurrentStage.Board.BoundingBox.Contains(bb))
            {
                return false;
            }

            // Check if there's no tile in all of the bb corners
            Board board = Stage.CurrentStage.Board;
            if (board.GetTileAt(bb.UpperLeft) == null &&
                board.GetTileAt(bb.UpperRight) == null &&
                board.GetTileAt(bb.LowerLeft) == null &&
                board.GetTileAt(bb.LowerRight) == null)
            {
                return true; // No tile in any of the corners, BB does not colide
            }

            // If the tool is valid if the player is trying to place the tool on a path
            // that is, if the BB collides with a path
            if (this._allowOnPaths)
            {
                // Get all intersection paths
                List<IQuadtreeContainable> paths = Stage.CurrentStage.Board.Quadtree.GetCollidingObjects(bb, Stage.QUADTREE_PATH_LIST_IDX);
                if (paths.Count > 0) // Intersects? Ok, allow if (collision resolution will solve the collision later)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Action(Vector2 position)
        {
            if (this._toolUseSound != null)
            {
                this._toolUseSound.Play();
            }

            this.Quantity--;
            if (this.Quantity <= 0)
            {
                this.Quantity = 0;
                this.Selected = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void DrawCursor(Vector2 position, bool enabled)
        {
            this.ObjectSprite.Draw(position, 0, Color.White, Levels.CurrentLevel.SpriteBatch);

#if DEBUG
            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                BoundingSquare bb = this.TransformBB(position);
                bb.Draw(Color.Red, Vector2.Zero);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void SetCursorOnBoard(bool enabled)
        {
            if (enabled)
            {
                SnailsGame.GameCursor.SetCursor(GameCursors.Default);
            }
            else
            {
                if (SnailsGame.GameSettings.ShowToolWithCursor)
                {
                    SnailsGame.GameCursor.SetCursor(GameCursors.Forbidden);
                }
            } 
        }

        /// <summary>
        /// Returns the ID in the stage data for the specified ToolObjectType
        /// </summary>
        public static string ToolTypeToString(ToolObjectType type)
        {
            switch (type)
            {
                case ToolObjectType.Apple:
                    return ToolApple.ID;
                case ToolObjectType.Box:
                    return ToolBox.ID;
                case ToolObjectType.Copper:
                    return ToolCopper.ID;
                case ToolObjectType.Dynamite:
                    return ToolDynamite.ID;
                case ToolObjectType.Vitamin:
                    return ToolVitamin.ID;
                case ToolObjectType.Salt:
                    return ToolSalt.ID;
                case ToolObjectType.DynamiteBox:
                    return ToolDynamiteBox.ID;
                case ToolObjectType.DynamiteBoxTriggered:
                    return ToolDynamiteBoxTriggered.ID;
                case ToolObjectType.Trampoline:
                    return ToolTrampoline.ID;
                case ToolObjectType.DirectionalBoxCW:
                    return ToolDirectionalBox.ID_CW;
                case ToolObjectType.DirectionalBoxCCW:
                    return ToolDirectionalBox.ID_CCW;
            }
            throw new SnailsException("Unexpected ToolObjectType [" + type .ToString() + "]"); 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsToolShortcutPressed()
        {

            if (this._shortcutKey == Input.GameplayInput.GamePlayButtons.None)
            {
                return false;
            }
            return Stage.CurrentStage.Input.QueryActionDown(this._shortcutKey);
        }
        

        /// <summary>
        /// 
        /// </summary>
        protected BoardPathNode GetNearestClickedPath(BoundingSquare bs, Vector2 clickPosition)
        {
            List<IQuadtreeContainable> paths = Stage.CurrentStage.Board.Quadtree.GetCollidingObjects(bs, Stage.QUADTREE_PATH_LIST_IDX);

            if (paths.Count == 0)
            {
                return null;
            }

            BoardPathNode node = (BoardPathNode)paths[0];
            if (paths.Count <= 1)
            {
                return node;
            }

            // get the closest path if there are more than 1
            float distance = 0;
            Vector2 actionVector = clickPosition;
            foreach (BoardPathNode n in paths)
            {
                // get the farthest point
                float dist = 0;
                float p0 = Vector2.DistanceSquared(actionVector, n.Value.P0);
                float p1 = Vector2.DistanceSquared(actionVector, n.Value.P1);

                // get the farthest distance
                dist = (p0 < p1) ? p1 : p0;
                        
                // now we can check for the closest path
                if (distance == 0 || distance > dist)
                {
                    distance = dist;
                    node = n;
                }
            }

            return node;
        }


        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);

            this.Type = (ToolObjectType)Enum.Parse(typeof(ToolObjectType), record.GetFieldValue<string>("type", Type.ToString()), true);
            this.ObjectId = record.GetFieldValue<string>("objectId", ObjectId);
            this.Quantity = record.GetFieldValue<int>("quantity", Quantity);
            this.SnapIt = record.GetFieldValue<bool>("snapIt", SnapIt);
            this._allowOnPaths = record.GetFieldValue<bool>("allowOnPaths", this._allowOnPaths);
            this._shortcutKey = (GameplayInput.GamePlayButtons)Enum.Parse(typeof(GameplayInput.GamePlayButtons), record.GetFieldValue<string>("gamePlayButtonShortkey", this._shortcutKey.ToString()), true);
            this._onUseSoundRes = record.GetFieldValue<string>("onUseSoundRes", null);
        }

        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        /// <summary>
        /// When the context is StageSave, not all properties are exported and some are removed
        /// This is because this properties are global for all objects of the same type and come from StageData
        /// Only the object specific properties will matter in this case
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord();
            record.Name = "Tool";
            switch (context)
            {
                case ToDataFileRecordContext.StageDataSave:
                    record.AddField("type", this.Type.ToString());
                    record.AddField("objectId", this.ObjectId);
                    record.AddField("snapIt", this.SnapIt);
                    record.AddField("allowOnPaths", this._allowOnPaths);
                    record.AddField("onUseSoundRes", this._onUseSoundRes);
                    break;

                case ToDataFileRecordContext.StageSave:
                    record.RemoveField("res"); // This are added in the base, so we have to remove them
                    record.RemoveField("sprite");
                    break;
            }
            record.AddField("quantity", this.Quantity);
            record.AddField("gamePlayButtonShortkey", this._shortcutKey.ToString());
            return record;
        }

        #endregion
    }
}
