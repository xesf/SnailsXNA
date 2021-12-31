using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects.ParticlesEffects;

namespace TwoBrainsGames.Snails.StageObjects
{
    class Crystal : SingleLightEmitter
    {
        #region Const
        // Bounding box indexes
        const int BB_IDX_LIGHT_SOURCE = 0;
        const int BB_IDX_SHINE_FIRST = 1;
        const int BB_IDX_SHINE_LAST = 5;
        const int BB_IDX_EXPLOSION_COLLISION = 6;

        
        // Scale to be used in the light source mask
        const float BIG_CRYSTAL_LIGHT_SIZE = 6f;
        const float MEDIUM_CRYSTAL_LIGHT_SIZE = 5f;
        const float SMALL_CRYSTAL_LIGHT_SIZE = 3f;
        #endregion

        #region Types
        public enum CrystalColorType
        {
            Red,
            Green,
            Blue,
            Yellow,
            Orange
        }

        public enum CrystalSizeType
        {
            Big,
            Medium,
            Small
        }
        #endregion

        #region Vars
        private float _maxLightScale;
        private CrystalSizeType _cristalSize;
        private CrystalColorType _cristalColor;
        private SpriteAnimation _shineAnimation;
        private double _shineShowTimer;
        private Sprite _bigCrystalSprite;
        private Sprite _mediumCrystalSprite;
        private Sprite _smallCrystalSprite;
        private Sprite _fragmentsSprite;
        private BoundingSquare _quadtreeCollisionBB;
        List<Explosion> _explosionList; // Holds the explosions that have hit the crystal
                                        // The only way I could get to avoid double collisions...
        #endregion

        #region Properties
        public CrystalColorType CristalColor 
        {
            get { return this._cristalColor; }
            set
            {
                this._cristalColor = value;
                // Just hardcode the colors...
                switch (this._cristalColor)
                {
                    case CrystalColorType.Blue:
                        this.BlendColor = Color.Blue;
                        break;
                    case CrystalColorType.Green:
                        this.BlendColor = Color.Green;
                        break;
                    case CrystalColorType.Orange:
                        this.BlendColor = Color.Orange;
                        break;
                    case CrystalColorType.Red:
                        this.BlendColor = Color.Red;
                        break;
                    case CrystalColorType.Yellow:
                        this.BlendColor = Color.Yellow;
                        break;
                }         
            }
        }

        public CrystalSizeType CristalSize 
        {
            get { return this._cristalSize; }
            set
            {
                this._cristalSize = value;
                this.Refresh();
      
            }
        }

        private bool IsShineVisible { get { return this._shineAnimation.Visible; } }
       
        public override BoundingSquare QuadtreeCollisionBB
        {
            get
            {
                return this._quadtreeCollisionBB;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Crystal()
            : base(StageObjectType.Crystal)
        {
            this.SpriteAnimationActive = false;
            this._explosionList = new List<Explosion>();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            Crystal from = (Crystal)other;
            this.CristalColor = from.CristalColor;
            this.CristalSize = from.CristalSize;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._shineAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/Shine"));
            this._shineAnimation.OnLastFrame += new SpriteAnimation.LastFrameHandler(_shineAnimation_OnLastFrame);
            this._shineAnimation.Autohide = true;

            this._bigCrystalSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/CrystalBig");
            this._mediumCrystalSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/CrystalMedium");
            this._smallCrystalSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/CrystalSmall");
            this._fragmentsSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/Fragments");

        }

        /// <summary>
        /// 
        /// </summary>
        void _shineAnimation_OnLastFrame()
        {
            this.RandomizeShineShowTimer();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.LightSource.EffectsBlender.Add(new ColorEffect(this.BlendColor, new Color(this.BlendColor.R, this.BlendColor.G, this.BlendColor.B, 0.2f), 0.01f, true));
            this._shineAnimation.Visible = false;
            this.RandomizeShineShowTimer();
            this.Refresh();
            this.LightSource.Position = this.GetBoundingBoxTransformed(BB_IDX_LIGHT_SOURCE).ToBoundingSquare().UpperLeft; 
        }

        /// <summary>
        /// 
        /// </summary>
        public override void KillByExplosion(Explosion exp)
        {
            if (!this._explosionList.Contains(exp))
            {
                this._explosionList.Add(exp);
                switch (this._cristalSize)
                {
                    case CrystalSizeType.Big:
                        this.CristalSize = CrystalSizeType.Medium;
                        break;
                    case CrystalSizeType.Medium:
                        this.CristalSize = CrystalSizeType.Small;
                        break;
                    case CrystalSizeType.Small:
                        this.DisposeFromStage();
                        break;
                }

                ExplosionEffect effect = new ExplosionEffect(this.Position.X, this.Position.Y, this._fragmentsSprite, 1f, SnailsGame.GameSettings.Gravity);
                effect.MinSpeed = 30;
                effect.MaxSpeed = 70;
                effect.MinAngle = 30;
                effect.MaxAngle = 120;

                effect.Color = this.BlendColor;
                effect.ComputeParticles(SnailsGame.GameSettings.Gravity);
                Stage.CurrentStage.Particles.Add(effect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            // The idea here is to adjust the size of the light according with the alpha channel of the light tint
            float scaleFactor = this._maxLightScale - ((255 - this.LightSource.Color.A) / 10f) / 256f;
            this.LightSource.Scale = new Vector2(scaleFactor, scaleFactor);

            if (!this.IsShineVisible)
            {
                this._shineShowTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this._shineShowTimer <= 0)
                {
                    this._shineAnimation.Visible = true;
                    this._shineAnimation.Reset();
                    // Randomize the position of the shine (bboxes in the sprite define the possible positions)
                    int bbIdx = BB_IDX_SHINE_FIRST + BrainGame.Rand.Next(BB_IDX_SHINE_LAST - BB_IDX_SHINE_FIRST);
                    this._shineAnimation.Position = this.GetBoundingBoxTransformed(bbIdx).ToBoundingSquare().UpperLeft;
                }
            }
            else
            {
                this._shineAnimation.Update(gameTime);
            }

            // Remove any disposed explosions from the explosion list
            for (int i = 0; i < this._explosionList.Count; i++)
            {
                if (this._explosionList[i].IsDisposed)
                {
                    this._explosionList.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            this.Sprite.Draw(this.Position, 1, this._rotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
            if (this.IsShineVisible)
            {
                this._shineAnimation.Draw(Stage.CurrentStage.SpriteBatch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Refresh()
        {
            switch (this._cristalSize)
            {
                case CrystalSizeType.Big:
                    this.LightSource.Scale = new Vector2(BIG_CRYSTAL_LIGHT_SIZE, BIG_CRYSTAL_LIGHT_SIZE);
                    if (this._bigCrystalSprite != null)
                    {
                        this.Sprite = this._bigCrystalSprite;
                    }
                    break;

                case CrystalSizeType.Medium:
                    this.LightSource.Scale = new Vector2(MEDIUM_CRYSTAL_LIGHT_SIZE, MEDIUM_CRYSTAL_LIGHT_SIZE);
                    if (this._bigCrystalSprite != null)
                    {
                        this.Sprite = this._mediumCrystalSprite;
                    }
                    break;

                case CrystalSizeType.Small:
                    this.LightSource.Scale = new Vector2(SMALL_CRYSTAL_LIGHT_SIZE, SMALL_CRYSTAL_LIGHT_SIZE);
                    if (this._smallCrystalSprite != null)
                    {
                        this.Sprite = this._smallCrystalSprite;
                    }
                    break;
            }
            this._maxLightScale = this.LightSource.Scale.X;

            if (this.Sprite != null)
            {
                this._quadtreeCollisionBB = this.GetBoundingBoxTransformed(BB_IDX_EXPLOSION_COLLISION).ToBoundingSquare();
            }
        }

     
        /// <summary>
        /// 
        /// </summary>
        private void RandomizeShineShowTimer()
        {
            this._shineShowTimer = BrainGame.Rand.Next(1000);
        }

        #region IDataFileSerializable Members
        /// <summary>
        ///
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.CristalColor = (CrystalColorType)Enum.Parse(typeof(CrystalColorType), record.GetFieldValue<string>("colorType", this.CristalColor.ToString()), true);
            this.CristalSize = (CrystalSizeType)Enum.Parse(typeof(CrystalSizeType), record.GetFieldValue<string>("cristalSize", this.CristalSize.ToString()), true);
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
            record.AddField("colorType", this.CristalColor.ToString());
            record.AddField("cristalSize", this.CristalSize.ToString());
            return record;
        }
        #endregion
 
    }
}
