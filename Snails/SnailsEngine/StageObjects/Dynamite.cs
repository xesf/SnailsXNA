using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Graphics;

namespace TwoBrainsGames.Snails.StageObjects
{
    public enum DynamiteStatus
     {
        Counting,
        Exploding,
        Done
    }

    public class Dynamite : MovingObject
    {
        #region Constants
        public const string ID = "DYNAMITE";
        public const int EXPLOSION_TIME = 1300;
        #endregion

        #region Members
        enum DynamiteState
        {
            Burning,
            Extinguished
        }
        protected Sample _fuseSample;
        protected Sample _beepSample;
        private Sprite _spriteExplosionRadius;
        private Sprite _fuseSprite;
        private Sprite _fuseFlameSprite;
        //private Sprite _dynamiteBodySprite;

        private SpriteAnimation _fuseAnimation;
        private SpriteAnimation _fuseFlameAnimation;

        // Used to make the explosion radius blink
        double _blinkTime;
        double _blinkCurrentTime;
        bool _radiusVisible;
        DynamiteState _state;
        #endregion

        public Dynamite()
            : base(StageObjectType.Dynamite)
        {
            this.FramesPerTime = Dynamite.EXPLOSION_TIME;
        }

        public Dynamite(StageObjectType type)
            : base(type)
        {
            this.FramesPerTime = Dynamite.EXPLOSION_TIME;
        }

        public Dynamite(Dynamite other)
            : base(other)
        {
            Copy(other);
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
            _fuseSample = (other as Dynamite)._fuseSample;
            _beepSample = (other as Dynamite)._beepSample;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            // Sounds
            this._fuseSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.FUSE, this);
            this._beepSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.DYNAMITE_BEEP, this);
            
            // Sprites
            this._spriteExplosionRadius = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite", "DynamiteRadius");
            this._fuseSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite", "Fuse");
            this._fuseFlameSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite", "Flame");
            //this._dynamiteBodySprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite", "DynamiteBody");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._blinkTime = 300;
            this._blinkCurrentTime = 0;
            this._radiusVisible = true;

            this._fuseAnimation = new SpriteAnimation(this._fuseSprite);
            this._fuseAnimation.OnLastFrame += new SpriteAnimation.LastFrameHandler(_fuseAnimation_OnLastFrame);
            this._fuseFlameAnimation = new SpriteAnimation(this._fuseFlameSprite);
        }

        /// <summary>
        /// 
        /// </summary>
        void _fuseAnimation_OnLastFrame()
        {
            this.Explode();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnAddedToStage()
        {
            _fuseSample.Play(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Explode()
        {
            _fuseSample.Stop();
            this.Explode(Explosion.ExplosionSize.Medium, 
                         Explosion.ExplosionSize.Medium, 
                         Explosion.ExplosionRadiusType.Circle,
                         Explosion.ObjectTypeAffected.All,
                         this.Position,
                         false,
                         null, 
                         null, true, false);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void KillByFire()
        {
            this.Explode();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StopSamples()
        {
            _fuseSample.Stop();
            base.StopSamples();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            if (this._state == DynamiteState.Burning)
            {
                this._fuseAnimation.Update(gameTime);
                this._fuseFlameAnimation.Update(gameTime);

                this._blinkCurrentTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this._blinkCurrentTime > this._blinkTime)
                {
                    this._radiusVisible = !this._radiusVisible;
                    this._blinkCurrentTime = this._blinkCurrentTime - this._blinkTime;
                    this._blinkTime *= 0.92f;
                    if (this._radiusVisible)
                    {
                        this._beepSample.Play();
                    }
                    if (this._blinkTime < 20)
                    {
                        this._blinkTime = 20;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            this._fuseAnimation.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
            if (this._state == DynamiteState.Burning)
            {
                this._fuseFlameAnimation.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
            }
        }

        /// <summary>
        /// Method called on objects that need to draw on top of tiles
        /// </summary>
        public override void ForegroundDraw()
        {
            base.ForegroundDraw();
            if (this._radiusVisible)
            {
                this._spriteExplosionRadius.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
            }
        }

        public override void OnEnterLiquid(Liquid liquid)
        {
            if (liquid is Water)
            {
                base.OnEnterLiquid(liquid);

                this._fuseSample.Stop();
                this.SpriteAnimationActive = false;

                this.Extinguish();
            }
            else if (liquid is Acid)
            {
                Explode();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Extinguish()
        {
            this._radiusVisible = false;
            this._blinkCurrentTime = 0;
            this._fuseFlameAnimation.Visible = false;
            this._fuseAnimation.PausePlay();
            this._state = DynamiteState.Extinguished;
        }
    }
}
