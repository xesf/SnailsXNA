using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public partial class Trampoline : MovingObject, ICursorInteractable
    {
        enum TrampolineState
        {
            Idle,
            Shooting
        }

        enum TrajectoryDotsFadeState
        {
            Idle,
            Computing,
            StartFading,
            Fading
        }

        #region Consts
        public const string ID = "TRAMPOLINE";

        public const string SPRITE_BASE = "Base";
        public const string SPRITE_POWER = "Power";
        private const string SPRITE_SHOOT = "TriggerShoot";
        private const string SPRITE_HOT_SPOT = "HotSpot";
        private const string SPRITE_DOTS = "Dots";
        private const string SPRITE_TRAJECT_DOTS = "TrajectoryDot";

        private const float MAX_SPEED = 90.0f;
        private const float MAX_ANGLE = 75.0f;
        private const float MIN_ANGLE = -75.0f;
        private const float MIN_SPEED_PERCENT = 30.0f;
        private const int CRATE_TOOL_VALID_BB_IDX = 1; // Index for the collision box in the sprite for the crate tool valid test

        const float HOT_SPOT_OFFSET_LENGTH = 0f; // This will offset the hotspot in wp

        const int NUM_TRAJECTORY_DOTS = 20;
        const int TRAJECTORY_DOTS_START_FADE_TIME = 1000;

        // Index in the sprite for the controller BS
        const int BS_CONTROLLER_IDX = 0;
        const int BS_CONTROLLER_WP_IDX = 1;
        #endregion

        #region Members
        float _angle;
        float _jumpSpeed;
        float _jumpSpeedPercent;
        Sprite _powerSprite;
        Sprite _dotsSprite;
        Sprite _baseSprite;
        Sprite _shootSprite;
        Sprite _idleSprite;
        Sprite _trajDotsSprite;
        TrampolineState _state;
        BoundingCircle _bsControllerTransf;
        bool _cursorDown;
        float _powerSpriteScale;
        Vector2 _powerBaseVector;
        float _powerBaseVectorLen;
        Sprite _spriteHotSpot;
        float _hotSpotRotation;
        Sample _jumpSample;
        Sample _throwSample;
        float _hotSpotOffsetLength;
        BoundingCircle _bsController;
        Vector2 HotSpotPosition { get { return this._bsControllerTransf._center; } }
        Vector2 _dotsPosition;
        Vector2[] _preCalcTrajDots = new Vector2[NUM_TRAJECTORY_DOTS];
        int _trajDotFrameIdx = 0;
        float _trajDotFrameElapsed;
        int _lastDot;
        TrajectoryDotsFadeState _trajectDotsFadeState = TrajectoryDotsFadeState.Idle;
        Color _trajDotColor = Color.White;
        float _trajDotFadeElapsed;

        public float Angle { get { return this._angle; } }
        public float JumpSpeed { get { return this._jumpSpeed; } }
        public bool DrawHotSpot { get; set; }

        #endregion

        public Trampoline()
            : base(StageObjectType.Trampoline)
        {
            this.DrawInForeground = true;
            this._SpritePlaybackMode = AnimtionPlaybackModes.PlayOnce;
            this._jumpSpeedPercent = 50.0f;
            this.DrawHotSpot = true;
        }

        public Trampoline(Trampoline other)
            : base(other)
        {
            Copy(other);
        }

        public Trampoline(string resourceId)
            : this()
        {
            this.ResourceId = resourceId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            Trampoline tramp = other as Trampoline;
            _angle = tramp._angle;
            _jumpSpeed = tramp._jumpSpeed;
            _jumpSpeedPercent = tramp._jumpSpeedPercent;
            _baseSprite = tramp._baseSprite;
            _powerSprite = tramp._powerSprite;
            _dotsSprite = tramp._dotsSprite;
            _shootSprite = tramp._shootSprite;
            _spriteHotSpot = tramp._spriteHotSpot;
            _trajDotsSprite = tramp._trajDotsSprite;
            _bsController = tramp._bsController;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._powerSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, SPRITE_POWER);
            this._baseSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, SPRITE_BASE);
            this._shootSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, SPRITE_SHOOT);
            this._spriteHotSpot = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, SPRITE_HOT_SPOT);
            this._dotsSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, SPRITE_DOTS);
            this._trajDotsSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, SPRITE_TRAJECT_DOTS);
            this._idleSprite = this.Sprite;
            this._jumpSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TRAMPOLINE_JUMP, this);
            this._throwSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_THROW, this);
        }

        /// <summary>
        /// m
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._hotSpotOffsetLength = 0;
            this._bsController = this._powerSprite._boundingSpheres[BS_CONTROLLER_IDX];
            if (SnailsGame.GameSettings.UseTouch)
            {
                _hotSpotOffsetLength = HOT_SPOT_OFFSET_LENGTH;
                this._bsController = this._powerSprite._boundingSpheres[BS_CONTROLLER_WP_IDX];
            }

            // This is the power vector. Starts in the origin of the object and ends in the center of the bs
            // It's the vector when speed is 100 and the angle is 0
            this._powerBaseVector = this._bsController._center;
            this._powerBaseVectorLen = this._powerBaseVector.Length();
            this._powerBaseVector.Y *= -1;

            this.ComputeJumpSpeed();
            this.UpdateControllerBB();

            this._hotSpotRotation = 0;
            this._jumpSpeedPercent = 50;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void HintInitialize()
        {
            this.DrawHotSpot = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateBoundingBox()
        {
            base.UpdateBoundingBox();
            this.UpdateControllerBB();
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateControllerBB()
        {
            // The general idea here, is to place the center of the BS in the tip of the power vector
            // The length of the power vector depends on the speed

            // First, we compute the length of the power vector depending on the current speed
            float len = this._jumpSpeedPercent * this._powerBaseVectorLen / 100.0f;

            // Then, we calculate a vector equal to the base power vector but with the new speed in size
            Vector2 v = this._powerBaseVector;
            v.Normalize();
            v *= (len + _hotSpotOffsetLength);

            // Rotate the vector to the current angle of the controller
            v = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(this._angle)));

            // Compute the new BS (we add the current position because previous calculus were made
            // using vectors in the origin
            if (this._bsController != null) // This was crashing in the stage Editor. It may no be needed if corrected in the editor
            {
                this._bsControllerTransf = new BoundingCircle(this.Position - v, this._bsController._radius);
            }
            v.Normalize();
            this._dotsPosition = this.Position - (v * len);
         }

        /// <summary>
        /// Speed is controller has a percent. Here he compute the real speed which is in a diferent scale
        /// and we calculate the amout of scaling the controller sprite will need
        /// </summary>
        private void ComputeJumpSpeed()
        {
            this._jumpSpeed = (this._jumpSpeedPercent * Trampoline.MAX_SPEED / 100);
            this._powerSpriteScale = (this._jumpSpeedPercent / 100.0f);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {

            base.Update(gameTime);

            if (this._state != TrampolineState.Shooting)
            {
                this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            }

            // Check if the player cursor is inside the controller area
            if (this.QueryCursorInsideInteractingZone() && Stage.CurrentStage.Cursor.IsInteractingWithObject == false)
            {
                Stage.CurrentStage.Cursor.SetInteractingObject(this);
            }

            this._hotSpotRotation += 0.03f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _trajDotFrameElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_trajDotFrameElapsed > 25)
            {
                _trajDotFrameElapsed = 0;
                _trajDotFrameIdx++;
                if (_trajDotFrameIdx >= this._lastDot)
                {
                    _trajDotFrameIdx = 0;
                }
            }

            if (QueryInterating() || 
                _trajectDotsFadeState == TrajectoryDotsFadeState.StartFading)
            {
                // since the stage Tile can be changed we need to 
                // always precalculate when we are interacting
                this.ComputeTrajectoryDots();
            }
            else
            {
                if (_trajectDotsFadeState == TrajectoryDotsFadeState.Computing)
                {
                    _trajectDotsFadeState = TrajectoryDotsFadeState.StartFading;
                }
                if (_trajectDotsFadeState == TrajectoryDotsFadeState.StartFading)
                {
                    _trajDotFadeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (_trajDotFadeElapsed > TRAJECTORY_DOTS_START_FADE_TIME)
                    {
                        _trajDotFadeElapsed = 0;
                        _trajectDotsFadeState = TrajectoryDotsFadeState.Fading;
                    }
                }
            }

            if (_trajectDotsFadeState == TrajectoryDotsFadeState.Fading)
            {
                Vector4 colorV = _trajDotColor.ToVector4();
                colorV.W -= 0.025f;
                colorV.X -= 0.025f;
                colorV.Y -= 0.025f;
                colorV.Z -= 0.025f;
                if (colorV.W < 0)
                {
                    colorV.W = 0;
                    _trajectDotsFadeState = TrajectoryDotsFadeState.Idle;
                }
                _trajDotColor = new Color(colorV);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool QueryCursorInsideInteractingZone()
        {
            if (Stage.CurrentStage.Cursor.ScreenPosition == Vector2.Zero)
                return false;
            return this._bsControllerTransf.Contains(Stage.CurrentStage.Cursor.Position);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            if (this._state == TrampolineState.Shooting)
            {
                this._state = TrampolineState.Idle;
                this.Sprite = this._idleSprite;
                this.CurrentFrame = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
#if DEBUG_ASSERTIONS
            if (obj as Snail == null || listIdx != Stage.QUADTREE_SNAIL_LIST_IDX)
            {
                throw new BrainException("Expected snail object!");
            }
#endif
            if (this._state == TrampolineState.Shooting)
            {
                return;
            }
            Snail snail = obj as Snail;
            if (snail.CanJumpTranpoline == false)
            {
                return;
            }           
            BoundingSquare bb = this.TransformSpriteFrameBB(1).ToBoundingSquare();

            //if (bb.Collides(snail.AABoundingBox))
            if (snail.CheckCollisionWithTrampolim(bb))
            {
                this._jumpSample.Play();
                this._throwSample.Play();                
                snail.JumpOnTrampoline(this);
                this._state = TrampolineState.Shooting;
                this.Sprite = this._shootSprite;
                this.CurrentFrame = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            Vector2 pos = Vector2.Zero;
            Color color = this.BlendColor;
            if (shadow)
            {
                pos += GenericConsts.ShadowDepth;
                color = this.ShadowColor;
            }
            this._powerSprite.Draw(this.Position + pos, 0, this._angle, this.Sprite.Offset, 1.0f, this._powerSpriteScale, color, Stage.CurrentStage.SpriteBatch);
            this._baseSprite.Draw(this.Position + pos, 0, 0, SpriteEffects.None, 1.0f, color, 1f, Stage.CurrentStage.SpriteBatch);
            base.Draw(shadow);

            if (this.DrawHotSpot)
            {
                this._spriteHotSpot.Draw(this.HotSpotPosition, 0, this._hotSpotRotation, SpriteEffects.None, Levels.CurrentLevel.SpriteBatch);
                if (this._hotSpotOffsetLength != 0)
                {
                    this._dotsSprite.Draw(this._dotsPosition, 0, this._angle, SpriteEffects.None, Levels.CurrentLevel.SpriteBatch);
                }
            }


#if DEBUG
            if (BrainGame.Settings.ShowBoundingBoxes)
            {
                this._bsControllerTransf.Draw(Color.Red, Stage.CurrentStage.Camera.UpperLeftScreenCorner);
                this.AABoundingBox.Draw(Color.Yellow, Stage.CurrentStage.Camera.UpperLeftScreenCorner);
            }
#endif
        }

        private void DrawTrajectoryDots()
        {
            int idx = 0;
            int frame = 0;
            for (int i = 0; i <= this._lastDot; i++)
            {
                Vector2 dotPos = _preCalcTrajDots[i];
                frame = (idx == _trajDotFrameIdx) ? 1 : 0;
                this._trajDotsSprite.Draw(dotPos, frame, 0f, SpriteEffects.None, _trajDotColor, 1f, Levels.CurrentLevel.SpriteBatch);
                idx++;
                if (BrainGame.Settings.ShowBoundingBoxes)
                {
                    BoundingSquare bs = this._trajDotsSprite.BoundingBox;
                    bs.TransformInPlace(dotPos);
                    bs.Draw(Color.Red, Stage.CurrentStage.Camera.Position);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ForegroundDraw()
        {
            base.ForegroundDraw();

            if (QueryInterating() || 
                _trajectDotsFadeState == TrajectoryDotsFadeState.StartFading ||
                _trajectDotsFadeState == TrajectoryDotsFadeState.Computing)
            {
                _trajDotColor = Color.White;
                DrawTrajectoryDots();
            }
            else
            {
                if (_trajectDotsFadeState == TrajectoryDotsFadeState.Fading)
                {
                    DrawTrajectoryDots();
                }
            }
        }

        private void ComputeTrajectoryDots()
        {
            _trajectDotsFadeState = TrajectoryDotsFadeState.Computing;

            // trampolim angle and speed
            float angleSin = (float)Math.Sin(MathHelper.ToRadians(90.0f - _angle));
            float angleCos = (float)Math.Cos(MathHelper.ToRadians(90.0f - _angle));
            Vector2 initialSpeed = new Vector2(angleCos, -angleSin) * _jumpSpeed;

            // reset previous trajectories
            double t = 1;

            this._lastDot = NUM_TRAJECTORY_DOTS - 1; // replace by a const, or use .length if the list is changed to an array
            for (int i = 0; i < NUM_TRAJECTORY_DOTS; i++)
            {
                // gravity formula
                t += 0.75f;
                float x = initialSpeed.X * (float)t;
                float y = (float)((initialSpeed.Y * t) + 0.5f * SnailsGame.GameSettings.Gravity * (t * t));
                // dot positions
                Vector2 dotPos = new Vector2(this.X + x, this.Y + y);
                Vector2 prevDotPos = (i > 0) ? _preCalcTrajDots[i - 1] : Vector2.Zero;

                if (i == 0) // always add the first point
                {
                    _preCalcTrajDots[i] = dotPos;
                    continue;
                }

                BoardPathNode bpn = Stage.CurrentStage.Board.PathCollidesWithVector(prevDotPos, dotPos);
                if (bpn == null)
                {
                    _preCalcTrajDots[i] = dotPos;
                    continue;
                }

                this._lastDot = i - 1;
                break; // when the first dot collide, we don't need to compute the others.
            }
        }

        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._angle = record.GetFieldValue<float>("angle", this._angle);
        }

        public override DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = base.ToDataFileRecord();
            record.AddField("angle", (int)this._angle);
            return record;
        }
        #endregion

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
            // Cursor is grabbing the controller, don't check if the cursor is out of the grabbing zone
            if (this._cursorDown)
            {
                return true;
            }
            return this.QueryCursorInsideInteractingZone();
        }

        /// <summary>
        /// The idea here, is to put the center of the bs in the place where the cursor is
        /// It was hard to rearch the final result, but in the end I wanted to:
        ///   - The center of the bs controller is placed in the cursor position
        ///   - After the update, the mouse position is placed in the new bs center
        ///     This is needed to stop the mouse from getting out of the BS
        /// </summary>
        public void CursorActionPressed(Vector2 cursorPos)
        {
            if (this._cursorDown == false)
            {
                Stage.CurrentStage.Cursor.SetPosition(this._bsControllerTransf._center - Stage.CurrentStage.Camera.UpperLeftScreenCorner);
                this._cursorDown = true;
                return;
            }
            Vector2 v4 = this._bsControllerTransf._center - cursorPos;
            if (v4.Length() < 1.5f) // We will not process this if the cursor didn't moved at lease 2 pixels
            {                       // This if is here, because the cursor position is placed in the center of
                return;             // the bs, if the bs didn't moved at least 1 pixel, the cursor would be static in the bs center
            }

            //Vector2 saveCenter = this._bsControllerTransf._center;

            Vector2 v1 = this.Position - cursorPos;
            float len = v1.Length() - this._hotSpotOffsetLength;
            if (len > this._powerBaseVectorLen)
                len = this._powerBaseVectorLen;
            this._jumpSpeedPercent = len * 100.0f / this._powerBaseVectorLen;

            if (this._jumpSpeedPercent > 100.0f)
                this._jumpSpeedPercent = 100.0f;
            if (this._jumpSpeedPercent < MIN_SPEED_PERCENT)
                this._jumpSpeedPercent = MIN_SPEED_PERCENT;

            v1.Normalize();
            Vector2 v2 = this._powerBaseVector;
            v2.Normalize();

            this._angle = Mathematics.AngleBetweenNormalizedVectors(v1, v2);
            if (this._angle < Trampoline.MIN_ANGLE)
                this._angle = Trampoline.MIN_ANGLE;

            if (this._angle > Trampoline.MAX_ANGLE)
                this._angle = Trampoline.MAX_ANGLE;

            this.ComputeJumpSpeed();
            this.UpdateControllerBB();
            // ?
            if (SnailsGame.GameSettings.Platform != BrainSettings.PlaformType.WP7Emulation)
            {
                Stage.CurrentStage.Cursor.SetPosition(this._bsControllerTransf._center - Stage.CurrentStage.Camera.UpperLeftScreenCorner);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionReleased()
        {
            this._cursorDown = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionSelected()
        {
        }
        #endregion
    }
}
