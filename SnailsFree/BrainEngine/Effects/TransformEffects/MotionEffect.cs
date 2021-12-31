using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class MotionEffect : TransformEffectBase, ITransformEffect
    {
        #region Members
        float _gravity;
        float _t;
        Vector2 _initialSpeed;
        Vector2 _prev;
        #endregion

        public float Gravity
        {
            get { return this._gravity; }
            set { this._gravity = value; }
        }
        public Vector2 InitialSpeed
        {
            get { return this._initialSpeed; }
            set { this._initialSpeed = value; }
        }
        public Vector2 CurrentSpeed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MotionEffect(float gravity, Vector2 initialSpeed)
        {
            this._gravity = gravity;
            this.Reset(initialSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this._t += ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 120f);
            
            // newypos = startingypos + ((yspeed * t) - 0.5 * acc * (t * t));
            float y = ((this._initialSpeed.Y * this._t) + 0.5f * this._gravity * (this._t * this._t));

            float x = this._initialSpeed.X * this._t;
            this.Position = new Vector3((x - this._prev.X), (y - this._prev.Y), 0.0f);
            this._prev = new Vector2(x, y);

            this.CurrentSpeed = new Vector2(this._initialSpeed.X,
                                            (this._initialSpeed.Y + (this._gravity * this._t)));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset(Vector2 initialSpeed)
        {
            base.Reset();
            this._initialSpeed = initialSpeed;
            this._t = 0f;
            this._prev = Vector2.Zero;
        }
    }
}
