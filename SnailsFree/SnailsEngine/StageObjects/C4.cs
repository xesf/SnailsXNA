using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    class C4 : MovingObject , ISwitchable
    {
        #region Consts
        const int LIGHT_POS_BB_IDX = 1;
        const int RED_LIGHT_FRAME = 1;
        const int GREEN_LIGHT_FRAME = 0;
        #endregion

        enum C4State
        {
            Idle,
            Beeping,
            Exploded
        }


//        private C4State _state;
        private Sprite _c4LightSprite;
        private Vector2 _lightPosition;
        private int _lightFrame;
        private BlinkEffect _lightEffect;
        private Sample _beepSound;

        public C4()
            : base(StageObjectType.C4)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._c4LightSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects", "C4Light");
            this._beepSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.C4_BEEP, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._lightPosition = this.Sprite.BoundingBoxes[LIGHT_POS_BB_IDX].Center;
            this._lightEffect = new BlinkEffect(250, 2000, this._beepSound, 0, 0);
            this._lightFrame = RED_LIGHT_FRAME;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            if (this.IsDead || this.IsDisposed)
            {
                return;
            }

            if (Stage.CurrentStage.Board.GetTileAt(this.BoardY, this.BoardX) == null)
            {
                this.DynamicFlags = StageObjectDynamicFlags.IsVisible;
                this.StaticFlags = StageObjectStaticFlags.CanFall | StageObjectStaticFlags.CanDieWithAnyTypeOfExplosion | StageObjectStaticFlags.CanDieWithExplosions;
                this.DettachFromPath();
            }

            this._lightEffect.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            if (this._lightEffect.Visible)
            {
                this._c4LightSprite.Draw(this.Position + this._lightPosition, this._lightFrame, Stage.CurrentStage.SpriteBatch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void KillByExplosion(Explosion exp)
        {
            this.Explode();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Explode()
        {
             this.Explode(Explosion.ExplosionSize.Small, 
                          Explosion.ExplosionSize.Medium, 
                          Explosion.ExplosionRadiusType.Circle, 
                          Explosion.ObjectTypeAffected.All, 
                          this.Position, 
                          true,
                          null, null, true, false);
        }

        #region ISwitchable Members
        /// <summary>
        /// 
        /// </summary>
        public void SwitchOn()
        {

            this.Explode();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOff()
        {
            
        }
       

        public bool IsOn
        {
            get { return false; } // C4 is always off. When it's on it has exploded
        }

        #endregion
    }
}
