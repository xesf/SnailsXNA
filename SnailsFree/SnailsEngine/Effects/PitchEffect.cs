using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine;
using System;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.Effects
{
    /// <summary>
    /// This effect is used when a pitch effect ends to make a smooth Scale ending
    /// just like when the camera is flicked
    /// </summary>
    class PinchEffect : TransformEffectBase
    {
        float _startScale;
        float _speed;
        //InGameCamera _camera;

        public PinchEffect(InGameCamera camera)
        {
            //this._camera = camera;
        }

        /// <summary>
        /// 
        /// </summary>
        public PinchEffect(float startScale, float lastScaleDelta)
        {
            this.Reset(startScale, lastScaleDelta);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this._speed *= Math.Abs((0.99f - (0.99f * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f)));
            if (Math.Abs(this._speed) <= 0.01f)
            {
                this.Active = false;
                return;
            }
            
            this.Scale += new Vector2(this._speed,  this._speed);

        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset(float startScale, float lastScaleDelta)
        {
            base.Reset();
            this._startScale = startScale;
            this.Scale = new Vector2(this._startScale, this._startScale);
            this._speed = lastScaleDelta;
        }
    }
}
