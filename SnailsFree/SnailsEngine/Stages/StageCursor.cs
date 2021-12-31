
using System;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.ToolObjects;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.Snails.Screens;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework.Input;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Stages
{
    public delegate void StageCursorActionEvent(); // delegate declaration

    public enum StageCursorActionStatus
    {
        None, 
        Action,
        DroppingObject
    }

    public partial class StageCursor : Object2D
    {
        #region Consts
        public const int PLAYER_CURSOR_BLICK_INTERVAL_START = 600;
        public const int PLAYER_CURSOR_BLICK_INTERVAL_END = 1200;
       
        #endregion


        public enum CursorType
        {
            NoAction,
            Select,
            Dropping,
            OutOfStock,
            ToolCursor,
            PanningMap
        }
        
        enum CursorState
        {
            Normal,
            PanningMap,
            InteractingWithObject,
            TutorialOpened,
            PichingMap
        }

        #region Members
        private CursorState _state;
        private CursorType _cursorType;
        private bool _toolActionIsValid;
        private bool IsOutOfStock;
        private StageCursorActionStatus ActionStatus;
        private Sprite _spriteDropping;
        private Sprite _spriteDefault;
        //private Sprite _spriteOutOfStock;
        //private Sprite _spriteSelect;
        private Sprite _spritePan;  // Sprite when camera is panning
        private Sprite _spriteBeforePan; // Sprite before pan started - used to reset the cursor when the pan ends
        private SpriteAnimation _toolInvalidAnim;
        private ColorEffect _invalidToolEffect;
        private double Timer;
        private Vector2 _panLastPosition;
        private Vector2 _panLastOffset;
        private float _panDistance;
        private double _panEllapsedTime;
       // private FlickEffect _flickEffect;
        private Sample _toolInvalidSample;

        public Vector2 ScreenPosition
        {
            get { return BrainGame.GameCursor.Position; }
            set { BrainGame.GameCursor.Position = value; }
        }
        private Color TOOL_OPACITY = new Color(180, 180, 180, 150);
        ICursorInteractable _interactingObject; // If set, means that the cursor is interacting with an object
                                                // The trampoline is an example of this
        private ToolObject Tool;// { get { return Stage.CurrentStage.Tool; } }

        public bool IsInteractingWithObject { get { return this._state == CursorState.InteractingWithObject; } }

        public bool CanUseTools
        {
            get
            {
                return (Stage.CurrentStage.State == Stage.StageState.Playing);
            }
        }

        public bool CanInteractWithObjects
        {
            get
            {
                return (Stage.CurrentStage.State == Stage.StageState.Playing);
            }
        }

        public bool EndPanEaseOutEnabled { get; set; }
        #endregion

        private CursorType _cursorBeforePanning;
        private bool _showInvalidActionSign;
        //private float _lastPinchScale;
        private float _startPinchScale;
        private double _pinchTime;

        /// <summary>
        /// 
        /// </summary>
        public StageCursor()
        { }

        /// <summary>
        /// 
        /// </summary>
        public StageCursor(StageCursor other)
            : base(other)
        {
            Copy(other);
            this.EndPanEaseOutEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(Object2D other)
        {
            base.Copy(other);
        }

        public virtual void Initialize()
        {
            this._toolActionIsValid = false;
            if (SnailsGame.GameSettings.UseGamepad)
            {
                SetPositionInScreenMiddle();
            }
            this.SetCursor(CursorType.Select);
            this._showInvalidActionSign = (SnailsGame.GameSettings.UseTouch);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadContent()
        {
            // get default sprite on stagedata
            this.Sprite = BrainGame.ResourceManager.GetSpriteStatic(SpriteResources.PLAYER_CURSOR_DEFAULT);
            this._spriteDefault = BrainGame.ResourceManager.GetSpriteStatic(SpriteResources.PLAYER_CURSOR_DEFAULT);
            this._spriteDropping = BrainGame.ResourceManager.GetSpriteStatic(SpriteResources.PLAYER_CURSOR_DROP);
            this._spritePan = BrainGame.ResourceManager.GetSpriteStatic(SpriteResources.PLAYER_CURSOR_CAM_PAN);
            this._toolInvalidSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TOOL_INVALID);
            this._toolInvalidAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary(SpriteResources.TOOL_INVALID_FEEDBACK));
            this._toolInvalidAnim.Autohide = true;

            this._invalidToolEffect = new ColorEffect(Color.White, new Color(0, 0, 0, 0), 0.03f, false);
            this._invalidToolEffect.UseRealTime = true;
        }


        /// <summary>
        /// Deals with controller input
        /// </summary>
        public void ControllerEvents(BrainGameTime gameTime)
        {
            Vector2 pos = Vector2.Zero;
            if (SnailsGame.GameSettings.UseMouse ||
                SnailsGame.GameSettings.UseTouch)
            {
                // cursor screen position
                ScreenPosition = Stage.CurrentStage.Input.MotionPosition;
                pos = new Vector2(ScreenPosition.X, ScreenPosition.Y);
            }
            else
            if (SnailsGame.GameSettings.UseGamepad)
            {
               pos = new Vector2(ScreenPosition.X, ScreenPosition.Y);

                if (Stage.CurrentStage.Input.CursorMotion)
                {
                    pos.X += Stage.CurrentStage.Input.MotionPosition.X * 7;
                    pos.Y += Stage.CurrentStage.Input.MotionPosition.Y * -7; // invert Y axis
                }
                else
                {
                    if (Stage.CurrentStage.Input.IsCursorUpPressed)
                        pos.Y -= Stage.CurrentStage.Board.TileHeight;
                    if (Stage.CurrentStage.Input.IsCursorDownPressed)
                        pos.Y += Stage.CurrentStage.Board.TileHeight;
                    if (Stage.CurrentStage.Input.IsCursorLeftPressed)
                        pos.X -= Stage.CurrentStage.Board.TileWidth;
                    if (Stage.CurrentStage.Input.IsCursorRightPressed)
                        pos.X += Stage.CurrentStage.Board.TileWidth;
                }
            }

            // correct if out of bounds - yup this needed in windows also. Because in fullscreen the viewport might not
            // match the screen
            Vector2 camMove = Vector2.Zero; // Used to autoscroll the map if the cursor moves out of the screen bounds
            if (pos.X < 0)
            {
                camMove = new Vector2(pos.X, 0f);
                pos.X = 0;
            }
            else
            if (pos.X > SnailsGame.ScreenWidth)
            {
                camMove = new Vector2(pos.X - SnailsGame.ScreenWidth, 0f);
                pos.X = SnailsGame.ScreenWidth;
            }

            if (pos.Y < 0)
            {
                camMove = new Vector2(camMove.X, pos.Y);
                pos.Y = 0;
            }
            else
            if (pos.Y > SnailsGame.ScreenHeight)
            {
                camMove = new Vector2(camMove.X, pos.Y - SnailsGame.ScreenHeight);
                pos.Y = SnailsGame.ScreenHeight;
            }

            if (camMove != Vector2.Zero && 
                SnailsGame.GameSettings.UseGamepad)
            {
                Stage.CurrentStage.Camera.MoveByOffset(camMove);
            }

            ScreenPosition = pos;
            Position = ((pos - Stage.CurrentStage.Camera.Origin) / Stage.CurrentStage.Camera.Scale) + Stage.CurrentStage.Camera.Position;

            this.UpdateBoundingBox();

            switch (this._state)
            {
                case CursorState.Normal:

                    if (Stage.CurrentStage.Input.MapPinchStarted)
                    { 
                        // Only allow piching if the cursor is inside the stage area
                        if (Stage.CurrentStage.StageHUD._stageArea.Contains(this.ScreenPosition))//Stage.CurrentStage.Input.MotionPosition))
                        {
                            this._state = CursorState.PichingMap;
                            this._pinchTime = 0;
                            this._startPinchScale = Stage.CurrentStage.Camera.Scale.X;
                            //this._lastPinchScale = Stage.CurrentStage.Camera.Scale.X; // X or Y doens't matter, they are always equal
                        }
                    }
                    else if (Stage.CurrentStage.Input.MapPanStarted)
                    {
                       // Only allow panning if the cursor is inside the stage area
                        if (Stage.CurrentStage.StageHUD._stageArea.Contains(this.ScreenPosition))//Stage.CurrentStage.Input.MotionPosition))
                        {
                            this._state = CursorState.PanningMap;
                            this._panEllapsedTime = 0;
                            this._panDistance = 0f;
                            this._panLastPosition = this.ScreenPosition;// Stage.CurrentStage.Input.MotionPosition;
                            this._spriteBeforePan = this.Sprite;
                            this.Sprite = this._spritePan;
                            this._cursorBeforePanning = this._cursorType;
                            this.SetCursor(CursorType.PanningMap);
                        }
                        break; // Break here. Ignore everything if pan has started
                    }

                    // If game is paused and the user clicks the board, notify the stage so it can start the game 
                    if (Stage.CurrentStage.Input.IsStageStartSelected &&
                        Stage.CurrentStage.StageHUD._stageArea.Contains(this.ScreenPosition))
                    {
                        if (Stage.CurrentStage.State == Stage.StageState.Startup)
                        {
                            Stage.CurrentStage.StartupEnded();
                        }
                        else
                        if (Stage.CurrentStage.State == Stage.StageState.InspectingHints)
                        {
                            Stage.CurrentStage.EndInspectingHints();
                        }
                    }

                    if (Stage.CurrentStage.Input.IsToolUpClicked && Stage.CurrentStage.StageHUD._toolsMenu.HasToolsToSelect)
                    {
                        Stage.CurrentStage.StageHUD._toolsMenu.DecrementSelection();
                    }
                    if (Stage.CurrentStage.Input.IsToolDownClicked && Stage.CurrentStage.StageHUD._toolsMenu.HasToolsToSelect)
                    {
                        Stage.CurrentStage.StageHUD._toolsMenu.IncrementSelection();
                    }

                    // Only allow cursor actions if the stage has not ended
                    if (Stage.CurrentStage.Input.IsActionPressed)  // check when its pressed
                    {
                        if (Stage.CurrentStage.StageHUD._toolsMenu.IsOver(ScreenPosition))
                        {
                            SnailsGame.GameCursor.SetCursor(GameCursors.Default);
                            Stage.CurrentStage.StageHUD._toolsMenu.ClickOverTool(ScreenPosition);
                        }
                    }
                       
                    if (!Stage.CurrentStage.StageHUD._toolsMenu.IsOver(ScreenPosition))
                    {
                        this.CheckToolAction();
                    }
                    break;

                case CursorState.InteractingWithObject:
                    this.ProcessInteractingObjectInput();
                    break;


                case CursorState.PanningMap:
                 /*   if (Stage.CurrentStage.Input.MapFlick)
                    {
                        // Only allow piching if the cursor is inside the stage area
                        if (Stage.CurrentStage.StageHUD._stageArea.Contains(this.ScreenPosition))//Stage.CurrentStage.Input.MotionPosition))
                        {
                            this._state = CursorState.FlickingMap;
                        }
                        break;
                    }*/
                    this._panEllapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
                    // Check if the cursor is out of the stage area
                    // The cursor should not go out of the stage area while panning
                    if (this.ScreenPosition.X < Stage.CurrentStage.StageHUD._stageArea.Left)
                    {
                        Stage.CurrentStage.Input.SetMotionPosition(new Vector2(Stage.CurrentStage.StageHUD._stageArea.Left, this.ScreenPosition.Y));
                    }
                    else
                        if (this.ScreenPosition.X >= Stage.CurrentStage.StageHUD._stageArea.Left + Stage.CurrentStage.StageHUD._stageArea.Width)
                    {
                        Stage.CurrentStage.Input.SetMotionPosition(new Vector2(Stage.CurrentStage.StageHUD._stageArea.Left + Stage.CurrentStage.StageHUD._stageArea.Width, this.ScreenPosition.Y));
                    }

                    if (this.ScreenPosition.Y < Stage.CurrentStage.StageHUD._stageArea.Top)
                    {
                        Stage.CurrentStage.Input.SetMotionPosition(new Vector2(this.ScreenPosition.X, Stage.CurrentStage.StageHUD._stageArea.Top));
                    }
                    else
                        if (this.ScreenPosition.Y >= Stage.CurrentStage.StageHUD._stageArea.Top + Stage.CurrentStage.StageHUD._stageArea.Height)
                    {
                        Stage.CurrentStage.Input.SetMotionPosition(new Vector2(this.ScreenPosition.X, Stage.CurrentStage.StageHUD._stageArea.Top + Stage.CurrentStage.StageHUD._stageArea.Height));
                    }

                    if (Stage.CurrentStage.Input.MapPanEnded)
                    {
                        this.EndPan();
                    }
                    break;

                case CursorState.TutorialOpened:
                    break;

                case CursorState.PichingMap:
                    Stage.CurrentStage.Camera.Zoom(Stage.CurrentStage.Input.PinchScale);
                    if (Stage.CurrentStage.Input.MapPinchEnded)
                    {
                        Stage.CurrentStage.Camera.EndPinch(this._startPinchScale, this._pinchTime);
                        this._state = CursorState.Normal;
                    }
                    //this._lastPinchScale = Stage.CurrentStage.Input.PinchScale;
                    this._pinchTime += gameTime.ElapsedRealTime.TotalMilliseconds;
                    break;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessInteractingObjectInput()
        {
            // Only allow cursor actions if the stage has not ended
            if (this._interactingObject.CanAcceptCursorInteraction)
            {
                if (Stage.CurrentStage.Input.IsActionDown)
                {
                    this._interactingObject.CursorActionPressed(this.Position);
                }
                if (Stage.CurrentStage.Input.IsActionReleased)
                {
                    this._interactingObject.CursorActionReleased();
                }
                if (//Stage.CurrentStage.Input.IsActionPressed ||
                    Stage.CurrentStage.Input.IsActionClicked)
                {
                    this._interactingObject.CursorActionSelected();
                }
            }
        }

        /// <summary>
        /// Checks the usage of a tool
        /// </summary>
        private void CheckToolAction()
        {
            if (Stage.CurrentStage.Input.IsActionClicked) // only check when its release (player can always cancel its move before releasing)
            {
                if (this.ActionStatus == StageCursorActionStatus.None &&
                    this.Tool != null && Tool.WithEnoughQuantity &&
                    !Stage.CurrentStage.StageHUD._toolsMenu.IsOver(ScreenPosition))
                {
                    if (this.CheckCurrentToolIsValid())
                    {
                        this.ActionStatus = StageCursorActionStatus.Action;
                    }
                    else
                    {
                        if (this._showInvalidActionSign)
                        {
                            this._toolInvalidAnim.Visible = true;
                            this._toolInvalidAnim.Position = this.ScreenPosition;
                            this._toolInvalidAnim.Reset();
                            this._toolInvalidAnim.EffectsBlender.Clear();
                            this._invalidToolEffect.Reset();
                            this._toolInvalidAnim.EffectsBlender.Add(this._invalidToolEffect);
                        }

                        this._toolInvalidSample.Play();
                      
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool CheckCurrentToolIsValid()
        {
            return Tool.IsValidAtPosition(this.Position);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
         
            switch (this._state)
            {
                // Normal cursor behaviour
                case CursorState.Normal:
                    if (!Stage.CurrentStage.StageHUD._toolsMenu.IsOver(ScreenPosition))
                    {
                        switch (this.ActionStatus)
                        {
                            case StageCursorActionStatus.None:
                                if (SnailsGame.GameSettings.ShowCursor &&
						    		Tool != null && !IsInteractingWithObject)
                                {
                                    //bool prevEnabled = this._toolActionIsValid;
                                    this._toolActionIsValid = this.CheckCurrentToolIsValid();
                                    this.IsOutOfStock = false;
                                    this.SetCursor(CursorType.ToolCursor);
                                }
                              
                                break;

                            case StageCursorActionStatus.Action:
                                if (Tool.WithEnoughQuantity)
                                {
                                    Tool.Action(this.Position);
                                    this.ActionStatus = StageCursorActionStatus.DroppingObject;
                                    this.SetCursor(CursorType.Dropping);
                                    this.Timer = 0;

                                    if (Tool.Quantity <= 0)
                                    {
                                        Stage.CurrentStage.StageHUD._toolsMenu.RemoveTool(Tool);
                                        this.SetCursor(CursorType.OutOfStock);
                                        this.ActionStatus = StageCursorActionStatus.None;
                                        this.IsOutOfStock = true;
                                    }
                                }
                                break;

                            case StageCursorActionStatus.DroppingObject:
                                this.Timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (this.Timer > 250)
                                {
                                    this.Timer = 0;
                                    this.ActionStatus = StageCursorActionStatus.None;
                                    this.SetCursor(CursorType.NoAction);
                                }
                                break;
                        }
                    }
                    else
                    {
                        // if its out of stock and its inside Tools Menu area, 
                        // than it should swap to the default cursor
                        if (this.IsOutOfStock)
                        {
                            this.IsOutOfStock = false;
                            this.SetCursor(CursorType.NoAction);
                        }
                        else
                        {
                            this.SetCursor(CursorType.Select);
                        }
                    }
                    break;

                // Interacting with an object
                case CursorState.InteractingWithObject:
                     // Query if the object is still interacting
                    if (this._interactingObject.QueryInterating() == true)
                    {
                        this.SetCursor(this._interactingObject.QueryCursor());
                        return; // The regular cursor loop should not run. The object controls the cursor
                    }
                    else
                    {
                        this._interactingObject = null;
                        this._state = CursorState.Normal;
                    }
                    break;

                // Panning the map
                // The map is panned while the right mouse is pressed and dragged
                case CursorState.PanningMap:
                    this._panLastOffset = this._panLastPosition - this.ScreenPosition;
                    if (this._panLastOffset != Vector2.Zero)
                    {
                        Stage.CurrentStage.Camera.MoveByOffset(this._panLastOffset);
                    }
                    // Add to the pan distance: We will use the pan distance to do the flick at the end of the pan
                    this._panDistance += this._panLastOffset.Length();
                    this._panLastPosition = this.ScreenPosition;
                    this.ScreenPosition = this._panLastPosition;
                    break;

                case CursorState.TutorialOpened:
                    break;
            }

            if (this.Tool != null)
            {
                this.Tool.Update(gameTime);
            }

            if (this._toolInvalidAnim.Visible)
            {
                this._toolInvalidAnim.Update(gameTime, true);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw()
        {
            
            if (SnailsGame.Tutorial.TopicVisible)
            {
                return;
            }

            if (SnailsGame.GameSettings.ShowToolWithCursor)
            {
                if (this.IsCursorToolActive())
                {
                    // The selected tool
                    if (!Stage.CurrentStage.StageHUD._toolsMenu.IsOver(ScreenPosition) && this._state != CursorState.PanningMap)
                    {
                        if (this.Tool != null && this.Tool.SnapIt && this.ActionStatus == StageCursorActionStatus.None)
                        {
                            this.DrawSelectedToolObjectsSnapped();
                        }
                        else if (this.Tool != null && this.Tool.SnapIt == false && this.ActionStatus == StageCursorActionStatus.None)
                        {
                            this.DrawSelectedToolObjects();
                        }
                    }
                }
            }

            if (this._toolInvalidAnim.Visible)
            {
                this._toolInvalidAnim.Draw(Stage.CurrentStage.SpriteBatch);
            }

        }

        /// <summary>
        /// The cursor when a tool is selected may not be active if the cursor is interacting with an object (trampoline for instance)
        /// or is hoovring over some stage hud icon (like the time warp button)
        /// In this cases the tool on the cursor is not displayed and the player can't use a tool
        /// </summary>
        private bool IsCursorToolActive()
        {
            return (!this.IsInteractingWithObject && !Stage.CurrentStage.StageHUD.IsInteractingWithCursor);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void DrawSelectedToolObjects()
        {
            Vector2 pos = this.Position - Stage.CurrentStage.Camera.UpperLeftScreenCorner;
            this.Tool.DrawCursor(pos, this._toolActionIsValid);
        }

        internal void DrawSelectedToolObjectsSnapped()
        {
            int areaX = (BoardX * Stage.CurrentStage.Board.TileWidth) - (int)(Stage.CurrentStage.Camera.UpperLeftScreenCorner.X);
            int areaY = (BoardY * Stage.CurrentStage.Board.TileHeight) - (int)(Stage.CurrentStage.Camera.UpperLeftScreenCorner.Y);
            this.Tool.ObjectSprite.Draw(new Vector2(areaX, areaY), 0, this.Tool.CursorDrawBlendColor, Levels.CurrentLevel.SpriteBatch);
        }

        internal void DrawTileCellArea()
        {
            int areaX = (BoardX * Stage.CurrentStage.Board.TileWidth) - (int)(Stage.CurrentStage.Camera.UpperLeftScreenCorner.X);
            int areaY = (BoardY * Stage.CurrentStage.Board.TileHeight) - (int)(Stage.CurrentStage.Camera.UpperLeftScreenCorner.Y);
            Rectangle rect = new Rectangle(areaX, areaY, Stage.CurrentStage.Board.TileWidth, Stage.CurrentStage.Board.TileHeight);
            BrainGame.DrawRectangleFilled(Levels.CurrentLevel.SpriteBatch, rect, new Color(0, 0, 255, 50));
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndPan()
        {
            this._state = CursorState.Normal;
            if (this._spriteBeforePan != null)
            {
                this.Sprite = this._spriteBeforePan;
            }
            this.SetCursor(this._cursorBeforePanning);

            Vector2 panVector = this._panLastOffset;
            if (this._panLastOffset != Vector2.Zero && this.EndPanEaseOutEnabled)
            {
                float speed = this._panDistance / (float)this._panEllapsedTime;
                panVector.Normalize();
                panVector *= speed;
                Stage.CurrentStage.Camera.DoFlick(panVector);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GameLostFocus()
        {
            if (this._state == CursorState.PanningMap)
            {
                this.EndPan();
            }
             this._state = CursorState.Normal;
       }

        /// <summary>
        /// 
        /// </summary>
        public void TutorialTopicOpened()
        {
            if (this._state == CursorState.PanningMap)
            {
                this.EndPan();
            }
            this.SetCursor(CursorType.Select);
            this._state = CursorState.TutorialOpened;
        }

        /// <summary>
        /// 
        /// </summary>
        public void TutorialTopicClosed()
        {
            this.SetCursor(CursorType.Select);
            this._state = CursorState.Normal;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void SetPositionInScreenMiddle()
        {
            float sx = (SnailsGame.ScreenWidth / 2) / Stage.CurrentStage.Board.TileWidth;
            float sy = (SnailsGame.ScreenWidth / 2) / Stage.CurrentStage.Board.TileHeight;
            ScreenPosition = new Vector2(sx, sy);
            Position = new Vector2(sx, sy);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetInteractingObject(ICursorInteractable obj)
        {
            if (this._state != CursorState.PanningMap)
            {
                this._state = CursorState.InteractingWithObject;
                this._interactingObject = obj;
                this.SetCursor(obj.QueryCursor());
                this.ProcessInteractingObjectInput();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            this.ScreenPosition = position;
            this.Position = position;
            this.Position += Stage.CurrentStage.Camera.Position;
            // Check this, should be in 
            Stage.CurrentStage.Input.SetMotionPosition(position);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetCursor(CursorType cursor)
        {
            this._cursorType = cursor;

            switch (this._cursorType)
            {
                case CursorType.Select:
                    SnailsGame.GameCursor.SetCursor(GameCursors.Select);
                    break;
                case CursorType.NoAction:
                    if (SnailsGame.GameSettings.ShowToolWithCursor)
                    {
                        SnailsGame.GameCursor.SetCursor(GameCursors.Forbidden);
                    }
                    break;
                case CursorType.Dropping:
                    this.Sprite = this._spriteDropping;
                    break;
                case CursorType.OutOfStock:
                    // This cannot be set to a special cursor because some objects may be selected on the stage, so we have
                    // to leave the default cursor
                    SnailsGame.GameCursor.SetCursor(GameCursors.Select);
                    break;
                case CursorType.ToolCursor:
                    if (this.Tool != null)
                    {
                        this.Tool.SetCursorOnBoard(this._toolActionIsValid);
                    }
                    break;
                case CursorType.PanningMap:
                    SnailsGame.GameCursor.SetCursor(GameCursors.MapPanCursor);
                    break;
                default:
                    this.Sprite = this._spriteDefault;
                    break;
            }
        }

        public void SetSelectedTool(ToolObject tool)
        {
            this.Tool = tool;
            if (tool != null)
            {
                Vector4 colorv = this.Tool.BlendColor.ToVector4();
                colorv *= (float)TOOL_OPACITY.A / 255f;
                this.Tool.CursorDrawBlendColor = new Color(colorv);
            }
        }


        #region IBoardCoordinates Members

        public int BoardX
        {
            get { return (int)(X / Stage.CurrentStage.Board.TileWidth); }
            set { X = value * Stage.CurrentStage.Board.TileWidth; }
        }

        public int BoardY
        {
            get { return (int)(Y / Stage.CurrentStage.Board.TileHeight); }
            set { Y = value * Stage.CurrentStage.Board.TileHeight; }
        }

        #endregion
    }
}
