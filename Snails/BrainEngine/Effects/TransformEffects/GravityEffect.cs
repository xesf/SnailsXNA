using System;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class GravityEffect : TransformEffectBase, ITransformEffect
    {
        #region Members
        float _gravity;
        //float _speed;
        double _t;
        float _initialSpeed;
        float prev;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public GravityEffect(float gravity, float initialSpeed)
        {
            this._initialSpeed = initialSpeed;
            //this._speed = initialSpeed;
            this._gravity = gravity;
            this._t = 0f;
            prev = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this._t += (gameTime.ElapsedGameTime.TotalMilliseconds / 100);
            
            // newypos = startingypos + ((yspeed * t) - 0.5 * acc * (t * t));
            float y = (float)((this._initialSpeed * this._t) - 0.5f * this._gravity * (this._t * this._t));
            this.Position = new Vector3(0.0f, -(y -prev), 0.0f);
            prev = y;
        }
    }
}
