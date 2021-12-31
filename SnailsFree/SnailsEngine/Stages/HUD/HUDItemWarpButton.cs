using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    class HUDItemWarpButton : HUDItem, ICursorInteractable
    {
        const int BUTTON_FRAME_IDX = 0;
        const int FAST_FWD_FRAME_IDX = 1;
        const int NORMAL_SPEED_FRAME_IDX = 2;
        const int KEYBOARD_HELP = 3;

        private Sprite _spriteWarp;
        private Vector2 _signPosition;
        private Vector2 _helpPosition;
        private int _signSpriteFrameIdx;
        private BoundingSquare _bsButton;
        private bool _buttonReleased; // This makes sure the user releases the mouse button before
                                      // allowing new toggle (or else there would be a flickering effect)
        private bool _isInteracting;
        private int _keyHelpFrameIdx;

        private bool _showKeyHelp;
        
        public HUDItemWarpButton()
        {
            this._buttonReleased = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            this.Position = new Vector2(-10f + SnailsGame.ScreenRectangle.X, SnailsGame.ScreenHeight - 70f);
            this._signPosition = this.Position + new Vector2(15f, 20f);
            if (SnailsGame.GameSettings.UseKeyboard)
            {
                this._helpPosition = this.Position + new Vector2(18f, -7f);
                this._keyHelpFrameIdx = StageHUD.WIN_HELP_TIME_WARP_FRAME_NR;
                this._showKeyHelp = true;
            }
            else
            if (SnailsGame.GameSettings.UseGamepad)
            {
                this._helpPosition = this.Position + new Vector2(72f, -20f);
                this._keyHelpFrameIdx = StageHUD.XBOX_HELP_TIME_WARP_FRAME_NR;
                this._showKeyHelp = true;
            }
            else
            if (SnailsGame.GameSettings.UseTouch)
            {
                this._helpPosition = Vector2.Zero;
                this._keyHelpFrameIdx = 0;
                this._showKeyHelp = false;
            }

            this._signSpriteFrameIdx = FAST_FWD_FRAME_IDX;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            // Timer
            this._spriteWarp = new Sprite(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "TimeWarpButton"));
            this._bsButton = this._spriteWarp.BoundingBox.Transform(this.Position);
            this.Size = new Vector2(this._spriteWarp.Frames[0].Rect.X, this._spriteWarp.Frames[0].Rect.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (SnailsGame.GameSettings.UseTouch)
            {
                // Check if the player cursor is inside the controller area
                if (this._isInteracting == false && this.QueryCursorInsideInteractingZone())
                {
                    Stage.CurrentStage.Cursor.SetInteractingObject(this);
                    this._buttonReleased = true;
                    this._isInteracting = true;
                }
                else if (this._buttonReleased)
                {
                    this._isInteracting = false;
                }
            }
            else
            {
                // Check if the player cursor is inside the controller area
                if (this.QueryCursorInsideInteractingZone())
                {
                    if (this._isInteracting == false)
                    {
                        Stage.CurrentStage.Cursor.SetInteractingObject(this);
                        this._buttonReleased = true;
                        this._isInteracting = true;
                    }
                }
                else
                {
                    this._isInteracting = false;
                }     
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this._spriteWarp.Draw(this.Position, BUTTON_FRAME_IDX, spriteBatch);
            this._spriteWarp.Draw(this._signPosition, this._signSpriteFrameIdx, spriteBatch);

            if (this._showKeyHelp)
            {
                StageHUD.SpriteXBoxHelp.Draw(this._helpPosition, this._keyHelpFrameIdx, spriteBatch);
            }

#if DEBUG
            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                this._bsButton.Draw(Color.Red, Vector2.Zero);
            }
#endif

        }

        /// <summary>
        /// 
        /// </summary>
        private bool QueryCursorInsideInteractingZone()
        {
            // return this._bsController.Collides(Stage.CurrentStage.Cursor._hotSpot);
            return this._bsButton.Contains(Stage.CurrentStage.Cursor.ScreenPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void TimeWarpChanged()
        {
            if (Stage.CurrentStage.InTimeWarp)
            {
                this._signSpriteFrameIdx = NORMAL_SPEED_FRAME_IDX;
            }
            else
            {
                this._signSpriteFrameIdx = FAST_FWD_FRAME_IDX;
             }
        }

        #region ICursorInteractable Members

        public StageCursor.CursorType QueryCursor()
        {
            return StageCursor.CursorType.Select;
        }

        public bool QueryInterating()
        {
            return this.QueryCursorInsideInteractingZone();
        }

        public void CursorActionPressed(Vector2 cursorPos)
        {
            if (this._buttonReleased)
            {
                Stage.CurrentStage.ToggleTimeWarp();
                this._buttonReleased = false;
            }
        }

        public void CursorActionReleased()
        {
            this._buttonReleased = true;      
        }
        /// <summary>
        /// 
        /// </summary>
        public void CursorActionSelected()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanAcceptCursorInteraction
        {
            get
            {
                return (Stage.CurrentStage.State == Stage.StageState.Playing);
            }
        }

        #endregion
    }
}
