using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// This is the controller wheel for the laser cannon and laser mirrors
    /// </summary>
    class RotationController: ICursorInteractable
    {
        // base sprite
        #region Consts
        private const int CONTROLLER_SPRITE = 0;
        private const int CONTROLLER_HOTSPOT_IDX = 1;
        private const int CONTROLLER_HOTSPOT_IDX_TOUCH = 2; // For touch devices

        private const float SENSIVITY_HD = 0.04f;
        private const float SENSIVITY_LD = 0.06f;

        private const float MAX_OFFSET = 10f; // This could be changed to a property if needed
        #endregion

        #region Vars
        private SpriteAnimation _arrowsAnimation;
        private Sprite _arrowsSprite;
        private Sprite _controllerSprite;
        private Vector2 _controllerPosition;
        private Vector2 _leftArrowPosition;
        private int _controllerCurFrame;
        private IRotationControllable _parentObject;
        private BoundingSquare _bbControllerHotSpot;
        private bool _cursorDown;
        private Vector2 _interactingInitialPos;
        private Vector2 _position;
        private ColorEffect _arrowFadeEffect;
        private float _sensivity;
        private int _controllerBBIdx;
        private Sample _tickSound;
        #endregion

        public RotationController(IRotationControllable parent)
        {
            this._parentObject = parent;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            this._position = position;
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this._arrowsSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "ControllerArrows");
            this._controllerSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "LaserController");
            this._tickSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.ROTAION_CONTROLLER_TICK, (Object2D)this._parentObject);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this._arrowsAnimation = new SpriteAnimation(this._arrowsSprite);
            this._controllerCurFrame = 0;
            this._arrowFadeEffect = new ColorEffect(Color.White, new Color(0, 0, 0, 0), 0.05f, false);
            this._arrowFadeEffect.Active = false;
            this._sensivity = (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.HD ? SENSIVITY_HD : SENSIVITY_LD);
            this._controllerBBIdx = (SnailsGame.GameSettings.UseTouch ? CONTROLLER_HOTSPOT_IDX_TOUCH : CONTROLLER_HOTSPOT_IDX);
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            this._arrowsAnimation.Update(gameTime);
            if (this._arrowFadeEffect.Active)
            {
                this._arrowFadeEffect.Update(gameTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            this._controllerSprite.Draw(this._controllerPosition, this._controllerCurFrame, this._parentObject.ControlledObject.Rotation, _parentObject.ControlledObject.SpriteEffect, Stage.CurrentStage.SpriteBatch);
         }

        /// <summary>
        /// 
        /// </summary>
        public void DrawForeground()
        {
            this._arrowsAnimation.Draw(this._leftArrowPosition, _parentObject.ControlledObject.Rotation, this._arrowFadeEffect.Color,  Stage.CurrentStage.SpriteBatch);
     #if DEBUG
            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                this._bbControllerHotSpot.Draw(Color.Orange, Stage.CurrentStage.Camera.Position);
            }
     #endif   
        }

        /// <summary>
        /// This refreshes the arrows and controller position, and the controller BB
        /// This all a big mess. This calculus have to take into account the parent StageObject transformations,
        /// like rotation, position and horizontal flip
        /// </summary>
        public void Refresh()
        {
            // Transform the controller BB to controller object coordinates
            // this._position holds the controller position inside the StageObject (object coordinates)
            BoundingSquare bs = this._controllerSprite.BoundingBox.Transform(this._position);
            // Tramsform the bs to StageObject world coordinates
            OOBoundingBox oobb = this._parentObject.ControlledObject.TransformBoundingBox(bs);
            // Set the controller position
            this._controllerPosition = (this._parentObject.ControlledObject.IsHorizontallyFlipped ? oobb.P1 : oobb.P0);
            this._leftArrowPosition = oobb.GetCenter();// +a1;
           // Set the bb for the controller

            bs = this._controllerSprite.BoundingBoxes[this._controllerBBIdx].Transform(this._position);
            // Tramsform the bs to StageObject world coordinates
            oobb = this._parentObject.ControlledObject.TransformBoundingBox(bs);
            this._bbControllerHotSpot = oobb.ToBoundingSquare();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(Vector2 pt)
        {
            return this._bbControllerHotSpot.Contains(pt);
        }

        #region ICursorInteractable Members
        /// <summary>
        /// 
        /// </summary>
        public StageCursor.CursorType QueryCursor()
        {
            return StageCursor.CursorType.Select;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool QueryInterating()
        {
            if (Stage.CurrentStage.Cursor.ScreenPosition == Vector2.Zero)
                return false;
            // Cursor is grabbing the controller, don't check if the cursor is out of the grabbing zone
            if (this._cursorDown)
            {
                return true;
            }
            return this.Contains(Stage.CurrentStage.Cursor.Position);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionPressed(Vector2 cursorPos)
        {
            if (!this._cursorDown)
            {
                this._interactingInitialPos = cursorPos;
                this._cursorDown = true;
                this._arrowFadeEffect.Active = true;
                this._arrowFadeEffect.StartColor = this._arrowFadeEffect.CurrentColor;
                this._arrowFadeEffect.EndColor = new Color(0,0,0,0);
                return;
            }
            this._cursorDown = true;
            if (cursorPos == this._interactingInitialPos)
            {
                return;
            }

            int dir = 0;
            switch ((int)this._parentObject.ControlledObject.Rotation)
            {
                case 0:
                    dir = (cursorPos.X > this._interactingInitialPos.X ? 1 : -1);
                    break;
                case 90:
                    dir = (cursorPos.Y > this._interactingInitialPos.Y ? 1 : -1);
                    break;
                case 180:
                    dir = (cursorPos.X > this._interactingInitialPos.X ? -1 : 1);
                    break;
                case 270:
                    dir = (cursorPos.Y > this._interactingInitialPos.Y ? -1 : 1);
                    break;
            }
            float lengthMoved = (this._interactingInitialPos - cursorPos).LengthSquared();
            float offset = -(lengthMoved * this._sensivity * dir);
            if (offset > MAX_OFFSET)
            {
                offset = MAX_OFFSET;
            }
            if (this._parentObject.ControllerValueChanged(offset))
            {
               this._controllerCurFrame++;
                if (this._controllerCurFrame >= this._controllerSprite.FrameCount)
                {
                    this._controllerCurFrame = 0;
                }
                this._tickSound.Play();
            }
            this._interactingInitialPos = cursorPos;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionReleased()
        {
            this._cursorDown = false;
            this._arrowFadeEffect.StartColor = this._arrowFadeEffect.CurrentColor;
            this._arrowFadeEffect.EndColor = Color.White;
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
                return (Stage.CurrentStage._state == Stage.StageState.Playing);
            }
        }
        #endregion
    }
}
