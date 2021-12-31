using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    class ControllableLaserCannon : LaserCannonBase, ICursorInteractable, IRotationControllable
    {
    
        #region Consts
        public const float MAX_ROTATION = 40;
        public const float MIN_ROTATION = -40;

        // base sprite
        private const int CANNON_POSITION_BS_IDX = 0;
        private const int ON_OFF_BUTTON_IDX = 1;
        private const int CONTROLLER_IDX = 2;

        // Cannon sprite
        private const int BEAM_ORIGIN_BS_IDX = 0;
        private const int GLOW_BS_IDX = 1;

        const double TURNING_ON_TIME = 1000;
        #endregion

        #region vars
        private float _cannonRotation;
        private Vector2 _cannonPosition;
        private Vector2 _onOffButtonPosition;
        private Vector2 _glowPosition;
        private Sprite _cannonSprite;
        private Sprite _onOffButtonSprite;
        private Sprite _sphereSprite;

        private SpriteAnimation _laserStartAnimation;
        private SpriteAnimation _laserGlowAnimation;
        private RotationController _controller;

        private Sample _laserStartUpSound;

        #endregion

        #region Properties
        public float CannonRotation
        {
            get { return this._cannonRotation; }
            set
            {
                this._cannonRotation = value;

            }

        }
        #endregion

        public ControllableLaserCannon()
            : base(StageObjectType.ControllableLaserCannon)
        {
            this._controller = new RotationController(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._cannonSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "LaserBeamCannon");
            this._onOffButtonSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "CannonState");
            this._sphereSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "CannonSphere");
            this._laserStartAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "LaserBeamStart"));
            this._laserGlowAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "CannonGlow"));
            this._controller.LoadContent();

            this._laserStartUpSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.LASER_POWER_UP, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._laserStartAnimation.BlendColor = this._laserXnaColor;
            this._laserGlowAnimation.BlendColor = this._laserXnaColor;
            this.UpdateCannon();
            this._controller.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCannon()
        {
            if (this._cannonRotation > MAX_ROTATION)
            {
                this._cannonRotation = MAX_ROTATION;
            }
            if (this._cannonRotation < MIN_ROTATION)
            {
                this._cannonRotation = MIN_ROTATION;
            }

            int flip = (this.IsHorizontallyFlipped? -1: 1); // Used to flip the BBoxes

            this._onOffButtonPosition = Mathematics.TransformVector(new Vector2(this.Sprite.BoundingBoxes[ON_OFF_BUTTON_IDX].Center.X * flip,
                                                                                this.Sprite.BoundingBoxes[ON_OFF_BUTTON_IDX].Center.Y), this.Rotation, this.Position);
            this._cannonPosition = this.TransformSpriteFrameBB(CANNON_POSITION_BS_IDX).GetCenter(); //Mathematics.TransformVector(this.Sprite.BoundingBoxes[CANNON_POSITION_BS_IDX].Center, this.Rotation, this.Position);
            
            // Laser beam setup
            Vector2 pos = new Vector2(this._cannonSprite.BoundingBoxes[BEAM_ORIGIN_BS_IDX].Center.X * flip,
                                      this._cannonSprite.BoundingBoxes[BEAM_ORIGIN_BS_IDX].Center.Y);
            pos = Mathematics.TransformVector(pos, this.CannonRotation, new Vector2(this.Sprite.BoundingBoxes[CANNON_POSITION_BS_IDX].Center.X * flip,
                                                                                    this.Sprite.BoundingBoxes[CANNON_POSITION_BS_IDX].Center.Y));
            this._laserBeam.Position = Mathematics.TransformVector(pos, this.Rotation, this.Position);
            this._laserBeam.SpriteEffect = this.SpriteEffect;
            this._laserBeam.BeamRotation = this.CannonRotation + this.Rotation + 90f + (this.IsHorizontallyFlipped? 180f: 0f);
            
            // Glow position
            pos = new Vector2(this._cannonSprite.BoundingBoxes[GLOW_BS_IDX].Center.X * flip,
                              this._cannonSprite.BoundingBoxes[GLOW_BS_IDX].Center.Y);
            pos = Mathematics.TransformVector(pos, this.CannonRotation, new Vector2(this.Sprite.BoundingBoxes[CANNON_POSITION_BS_IDX].Center.X * flip,
                                                                                    this.Sprite.BoundingBoxes[CANNON_POSITION_BS_IDX].Center.Y));
            this._glowPosition = Mathematics.TransformVector(pos, this.Rotation, this.Position);

            this._controller.SetPosition(this.Sprite.BoundingBoxes[CONTROLLER_IDX].UpperLeft);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StageStartupPhaseEnded()
        {
            base.StageStartupPhaseEnded();
            if (this._state == LaserBeamSourceState.TurningOn)
            {
                this._laserStartUpSound.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            switch (this._state)
            {
                case LaserBeamSourceState.TurningOn:
                    this._laserBeam.Visible = false;
                    this._laserStartAnimation.Visible = true;
                    this._laserStartAnimation.Update(gameTime);
                    this._turningOnTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (this._turningOnTime <= 0)
                    {
                        this._state = LaserBeamSourceState.On;
                        this._laserBeam.Visible = true;
                     //   this._laserSound.Play(true);
                    }
                    break;
            }

            // Check if the player cursor is inside the controller area
            if (this._controller.Contains(Stage.CurrentStage.Cursor.Position) && 
                Stage.CurrentStage.Cursor.IsInteractingWithObject == false)
            {
                Stage.CurrentStage.Cursor.SetInteractingObject(this);
            }

            this._controller.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);

            this._onOffButtonSprite.Draw(this._onOffButtonPosition, (int)this._state, this.Rotation, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
            if (this._state == LaserBeamSourceState.TurningOn)
            {
                this._laserStartAnimation.Draw(this._laserBeam.Position, this.CannonRotation + this.Rotation, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
            }
            if (!shadow)
            {
                this._sphereSprite.Draw(this._glowPosition, 0, this.CannonRotation + this.Rotation, this.SpriteEffect, this._laserXnaColor, 1f, Stage.CurrentStage.SpriteBatch);
                this._cannonSprite.Draw(this._cannonPosition, 0, this.CannonRotation + this.Rotation, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
            }
            else
            {
                this._sphereSprite.Draw(this._glowPosition + GenericConsts.ShadowDepth, 0, this.CannonRotation + this.Rotation, this.SpriteEffect, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
                this._cannonSprite.Draw(this._cannonPosition + GenericConsts.ShadowDepth, 0, this.CannonRotation + this.Rotation, this.SpriteEffect, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
            }
            this._controller.Draw();

            if (this.TurnedOn)
            {
                this._laserGlowAnimation.Draw(this._glowPosition, 0f, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void ForegroundDraw()
        {
            base.ForegroundDraw();
            this._controller.DrawForeground();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void SetTurningOnState(bool playSound)
        {
            this.TurnedOn = true;
            this._laserStartAnimation.Visible = false;
            this._turningOnTime = TURNING_ON_TIME;
            this._blinkEffect.Active = false;
            this._state = LaserBeamSourceState.TurningOn;
            if (playSound)
            {
                this._laserStartUpSound.Play();
            }
        }
   
        #region ICursorInteractable Members
        /// <summary>
        /// 
        /// </summary>
        public StageCursor.CursorType QueryCursor()
        {
            return this._controller.QueryCursor();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool QueryInterating()
        {
            return this._controller.QueryInterating();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionPressed(Vector2 cursorPos)
        {
            this._controller.CursorActionPressed(cursorPos);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionReleased()
        {
            this._controller.CursorActionReleased();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionSelected()
        {
        }

        #endregion

        #region IRotationControllable Members

        public StageObject ControlledObject { get { return this; } }

        public bool ControllerValueChanged(float value)
        {
            float prevRotation = this._cannonRotation;
            this._cannonRotation -= value;
            this.UpdateCannon();

            return (prevRotation != this._cannonRotation);
        }


        #endregion

        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);

            record.AddField("cannonRotation", this.CannonRotation);
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.CannonRotation = record.GetFieldValue<float>("cannonRotation", this.CannonRotation);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        #endregion

    }
}
