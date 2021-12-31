using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.BrainEngine.Effects
{
    public class FlickEffect : TransformEffectBase
    {
        Vector2 _speed;
        Vector2 _initialSpeed;
        public const float _friction = 0.99f;
        public Vector2 Speed 
        { 
            get { return this._speed; } 
            set { this._speed = value; } 
        }

        public FlickEffect(Vector2 speed)
        {
            this.Reset(speed);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this.PositionV2 = this._speed * (float)gameTime.ElapsedRealTime.TotalSeconds;
            // apply friction
            this._speed *= Math.Abs(0.95f - (_friction * (float)gameTime.ElapsedGameTime.TotalSeconds));

            if ((Math.Abs(this._speed.X) <= 25f && Math.Abs(this._speed.Y) <= 25f) || // threshold to stop
                 this._speed == Vector2.Zero) 
            {
                this._speed = Vector2.Zero;
                this.Active = false;
            }
        }

        /// <summary>
        /// Reset should put the effect in the same state when it was created
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            _speed = _initialSpeed;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset(Vector2 speed)
        {
            base.Reset();
            _speed = _initialSpeed = speed;// (speed * -1);
        }

    }
}
