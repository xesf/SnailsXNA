using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Effects;
using System;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class SnailShell : MovingObject
    {
        enum ShellStatus
        {
            None,
            Hidding,
            Floating,
            Hoovering
        };

        #region Constants
        public const string ID = "SNAIL_SHELL";
        private const int HOOVER_UNDWATER = 5;
        private const float ROTATION_LIMIT = 180f;
        #endregion

        #region Members
        FloatingEffect _floatEffect;
        RotationEffect _rotationEffect;
        ShellStatus _status;
        Sprite _hiddingInShellSprite;
        Sprite _shellSprite;
        float _previousPositionY;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public SnailShell()
            : this(StageObjectType.SnailShell)
        { }

        /// <summary>
        /// 
        /// </summary>
        protected SnailShell(StageObjectType type)
            : base(type)
        { }

        /// <summary>
        /// 
        /// </summary>
        public SnailShell(StageObject other)
            : base(other)
        {
            this.Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            //SnailShell shell = (SnailShell)other;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._shellSprite = this.Sprite;
            this._hiddingInShellSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/anim-snails", Snail.SPRITE_SNAIL_HIDDING_IN_SHELL);
        }

        public void FloatDeath(Liquid inWaterObj, float speed, bool withHideInShellAnimation)
        {
            _inLiquidRef = inWaterObj;

            _floatEffect = new FloatingEffect(speed, this.Position, inWaterObj, true, HOOVER_UNDWATER);
            _floatEffect.Amplitude = 1;
            _floatEffect.Interval = 2;
            _floatEffect.Active = false;
            this.EffectsBlender.Add(_floatEffect);

            // get roation direction
            int dir = 1;
            if (this.Rotation == 0)
            {
            /*    if (BrainGame.Rand.Next(1, 3) == 2)
                {
                    dir = -1;
                }*/
            }
            else if (this.Rotation - ROTATION_LIMIT > 0)
            {
                dir = -1;
            }
            
            _rotationEffect = new RotationEffect(6 * dir);
            this.EffectsBlender.Add(_rotationEffect, StageObject.TRANSF_EFFECT_ROTATION);
            this._rotationEffect.Active = false;

            if (withHideInShellAnimation)
            {
                this.SetHidingInShellStatus();
            }
            else
            {
                this.SetFloatingShellStatus();
            }

            if (inWaterObj == null)
            {
                this.KillWithProjection();
                return;
            }
            
        }

        private void SetHidingInShellStatus()
        {
            _status = ShellStatus.Hidding;
            this.Sprite = this._hiddingInShellSprite;
            this.CurrentFrame = 0;
        }

        private void SetFloatingShellStatus()
        {
            _status = ShellStatus.Floating;
            this.Sprite = this._shellSprite;
            this.CurrentFrame = 0;
            _floatEffect.Active = true;
            _rotationEffect.Active = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            base.OnLastFrame();
            switch (this._status)
            {
                case ShellStatus.Hidding:
                    this.SetFloatingShellStatus();
                    break;
            }
        }

        public override void Update(BrainGameTime gameTime)
        {
            if (this.EffectsBlender.Count > 0 &&
                this._inLiquidRef.LiquidLevel == Liquid.LIQUID_LEVEL_EMPTY)
            {
                this.EffectsBlender.DeleteEffects(StageObject.TRANSF_EFFECT_ROTATION);
                this.EffectsBlender.DeleteEffects(StageObject.TRANSF_EFFECT_HOOVER);
            }

            base.Update(gameTime);

            if (this._status == ShellStatus.Hidding)
            {
                return;
            }

            if (_status == ShellStatus.Floating && _floatEffect.Ended ||
                !this.EffectsBlender.Contains(StageObject.TRANSF_EFFECT_HOOVER) && this._inLiquidRef.LiquidLevel != Liquid.LIQUID_LEVEL_EMPTY) // add hoover effect
            {
                _status = ShellStatus.Hoovering;
                this.EffectsBlender.Add(new HooverEffect(0.06f, 0.3f, 0f), StageObject.TRANSF_EFFECT_HOOVER);
                _previousPositionY = _inLiquidRef.QuadtreeCollisionBB.Top;
            }
            
            // stop rotation when reach the limit
            if (_rotationEffect.Speed < 0 && this.Rotation < ROTATION_LIMIT ||
                _rotationEffect.Speed > 0 && this.Rotation > ROTATION_LIMIT)
            {
                _rotationEffect.Ended = true;
                this.Rotation = ROTATION_LIMIT;
                this.EffectsBlender.DeleteEffects(StageObject.TRANSF_EFFECT_ROTATION);
            }

            if (_status == ShellStatus.Hoovering)
            {
                if (_previousPositionY != _inLiquidRef.QuadtreeCollisionBB.Top)
                {
                    this.Y += _inLiquidRef.QuadtreeCollisionBB.Top - _previousPositionY;
                }
            }
            _previousPositionY = _inLiquidRef.QuadtreeCollisionBB.Top;
        }
    }
}
