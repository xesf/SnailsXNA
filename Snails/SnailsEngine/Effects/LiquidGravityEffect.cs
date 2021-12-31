using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Effects
{
    public class LiquidGravityEffect : TransformEffectBase
    {
        private float _gravity;

        public LiquidGravityEffect(float gravity)
        {
            this._gravity = gravity;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            /*
            float t = ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 100f);
            float y = this._currentSpeed * t;
            float resistance = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.99f / 1000f;
            //this._currentSpeed -= this._liquidDensity * t;
            this._currentSpeed *= (resistance * 10f);
            if (this._currentSpeed < this._minSpeed)
            {
                this._currentSpeed = this._minSpeed;
            }*/
            float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100f * this._gravity;
            this.Position = new Vector3(0f, speed, 0f);
        }

    }
}
