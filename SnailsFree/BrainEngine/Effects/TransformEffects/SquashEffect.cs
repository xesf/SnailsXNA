using System;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class SquashEffect : TransformEffectBase
    {
        float _initialSquashLimit;
        float _squashLimit;
        float _decay; // decay makes the effect disappear over time. It goes from 0.0f to 1.0f
                      // The creator of the effect passes the inverted value: 1.0f full decay, 0.0f no decay
                      // Internally 0.0f means full decay, 1.0f no decay
        float _speed;
        float _direction;
        //float _power;

        public Vector2 TargetScale { get; set;}
        Color _blendColor;

        public Color BlendColor 
        {
            get { return this._blendColor; }
            set
            {
                this._blendColor = value;
                this.ColorVector = this._blendColor.ToVector4();
            }
        }

        public SquashEffect(float squashLimit, float speed, float decay) :
             this(squashLimit, speed, decay, Color.White, new Vector2(1.0f, 1.0f))
        {
        }

        public SquashEffect(float squashLimit, float speed, float decay, Color color, Vector2 targetScale)
        {
            this._squashLimit = this._initialSquashLimit = squashLimit;
            this._decay = decay;
            this._speed = speed;
            this.TargetScale = targetScale;
            this.BlendColor = color;
            this.Reset();
           
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            float delta = ((float)gameTime.ElapsedGameTime.TotalMilliseconds * this._speed / 1000) * this._direction;
            float x = this.Scale.X + delta;
            float y = this.Scale.Y - delta;

            if (x < this._squashLimit)
            {
                this._direction *= -1.0f;
                this._squashLimit += this._decay;
                x = this._squashLimit;
                y = (this.TargetScale.Y - this._squashLimit) + this.TargetScale.Y;
            }
            else
            if (y < this._squashLimit)
            {
                this._direction *= -1.0f;
                this._squashLimit += this._decay;
                y = this._squashLimit;
                x = (this.TargetScale.X - this._squashLimit) + this.TargetScale.X;
            }
        
           
            this.Scale = new Vector2(x, y);

            if (this._squashLimit >= this.TargetScale.X)
            {
                this.Ended = true;
                this.Scale = this.TargetScale;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this._squashLimit = this._initialSquashLimit;
            this._direction = -1.0f;
            this.ColorVector = this.BlendColor.ToVector4();
            this.Scale = this.TargetScale;
        }
    }
}
