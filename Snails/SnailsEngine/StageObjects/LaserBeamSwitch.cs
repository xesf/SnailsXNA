using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    class LaserBeamSwitch : Switch
    {
        #region Consts
        private const int RECEIVER_POS_BS_IDX = 0;

        private const int POINTER_POS_BB_IDX = 0;
        private const int RED_POS_BB_IDX = 1;
        private const int GREEN_POS_BB_IDX = 2;
        private const int ON_TEXT_POS_BB_IDX = 3;
        
        private const double ACTIVATION_TIME = 5000;
        private const float MAX_POINTER_ANGLE = 310f;
        #endregion

        #region Vars
        private bool _laserBeamCollided;
        private Sprite _lightningSprite;
        private Sprite _lightsSprite;
        private Sprite _pointerSprite;
        private Sprite _sphereSprite;
        private SpriteAnimation _lightningAnimation;
        private BoundingCircle _bsReceiver;
        private BoundingSquare _switchBB;
        private float _charge;
        private Vector2 _pointerPosition;
        private float _pointerAngle;

        private Vector2 _lightPosition;
        private int _lightFrame;
        private ColorEffect _colorEffect;

        protected Sample _electricitySound;
        protected Sample _switchSound;

        #endregion

        public LaserBeam.LaserBeamColor LaserColor { get; set; }

        public override BoundingSquare QuadtreeCollisionBB
        {
            get
            {
                return this._switchBB;
            }
        }

        public LaserBeamSwitch()
            : base(StageObjectType.LaserBeamSwitch)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._lightningSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "SwitchLightning");
            this._lightsSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "SwitchLights");
            this._pointerSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "SwitchPointer");
            this._sphereSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "SwitchSphere");

            this._electricitySound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.ELECTRICITY, this);
            this._switchSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SWITCH_LEVER_ACTIVATING, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._lightningAnimation = new SpriteAnimation(this._lightningSprite);
            this._lightningAnimation.Visible = false;
            this._colorEffect = new ColorEffect(LaserBeam.GetXnaColor(this.LaserColor), new Color(180, 180, 180), 0.01f, false);
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Refresh()
        {
            this._bsReceiver = this.Sprite._boundingSpheres[RECEIVER_POS_BS_IDX].Transform(this.Position);
            this._switchBB = this._bsReceiver.GetContainingSquare();
            this._lightningAnimation.Position = this._bsReceiver._center;
            this._pointerPosition = this.TransformSpriteFrameBB(POINTER_POS_BB_IDX).GetCenter();
            this._lightPosition = this.TransformSpriteFrameBB(RED_POS_BB_IDX).P0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            if (this.IsOn)
            {
                if (this._colorEffect.Ended == false)
                {
                    this._colorEffect.Update(gameTime);
                }
                return;
            }
          
            if (this._lightningAnimation.Visible)
            {
                this._lightningAnimation.Update(gameTime);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AfterUpdate(BrainGameTime gameTime)
        {
            base.AfterUpdate(gameTime);

            if (this.IsOn)
            {
                return;
            }


            if (this._laserBeamCollided)
            {
                this._electricitySound.Play(true);
                this._charge += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 100f / ACTIVATION_TIME);
                if (this._charge > 100)
                {
                    this._charge = 100;
                    this._lightFrame = 1;
                    this._lightPosition = this.TransformSpriteFrameBB(GREEN_POS_BB_IDX).P0;
                    this._pointerAngle = MAX_POINTER_ANGLE;
                    this._lightningAnimation.Visible = false;
                    this.SwitchOn();
                    this._electricitySound.Stop();
                    this._switchSound.Play();
                }
                this._pointerAngle = (this._charge * MAX_POINTER_ANGLE / 100f);
            }
            else
            {
                this._electricitySound.Stop();
                this._charge -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 100f / ACTIVATION_TIME * 3); // 3 times faster             
                if (this._charge < 0)
                {
                    this._charge = 0;
                }
                this._pointerAngle = (this._charge * MAX_POINTER_ANGLE / 100f);
               
                this._lightningAnimation.Visible = false;
            }
            this._laserBeamCollided = false;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            if (shadow)
            {
                this._sphereSprite.Draw(this._bsReceiver._center + GenericConsts.ShadowDepth, 0, this.ShadowColor, Stage.CurrentStage.SpriteBatch);
            }
            else
            {
                this._sphereSprite.Draw(this._bsReceiver._center, 0, this._colorEffect.Color, Stage.CurrentStage.SpriteBatch);
            }

            base.Draw(shadow);
            if (this._lightningAnimation.Visible)
            {
                this._lightningAnimation.Draw(Stage.CurrentStage.SpriteBatch);
            }
            this._lightsSprite.Draw(this._lightPosition, this._lightFrame, Stage.CurrentStage.SpriteBatch);
            this._pointerSprite.Draw(this._pointerPosition, 0, this._pointerAngle, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);

            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                this._bsReceiver.Draw(Color.Red, Stage.CurrentStage.Camera.Position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool OnLaserBeamCollided(LaserBeam beam, out Vector2 collidingPoint)
        {
            if (this.IsOff)
            {
                if (this._bsReceiver.IntersectsLine(beam.BeamOrigin, beam.BeamEndPoint, out collidingPoint))
                {
                    if (beam.LaserColor != this.LaserColor)
                    {
                        return true;
                    }
                    if (!this.IsOn)
                    {
                        this._laserBeamCollided = true;
                        this._lightningAnimation.Visible = true;
                    }
                    return true;
                }
            }
            else
            {
                collidingPoint = Vector2.Zero;
            }
            return false;
        }

        #region IDataFileSerializable Members
        /// <summary>
        ///
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.LaserColor = (LaserBeam.LaserBeamColor)Enum.Parse(typeof(LaserBeam.LaserBeamColor), record.GetFieldValue<string>("colorType", this.LaserColor.ToString()), true);
        }

        /// <summary>
        ///
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }


        /// <summary>
        ///
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            record.AddField("colorType", this.LaserColor.ToString());
             return record;
        }
        #endregion
 
    }
}
