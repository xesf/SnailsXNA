using System;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Effects
{
    public class PopOutEffect : TransformEffectBase
    {
        bool _increasing;
        Vector2 _maxScale;
        float _speed;
        float _initialSpeed;
        Color _blendColor;
        Vector2 _initialScale;

        public PopOutEffect(Vector2 maxScale, float speed) :
            this(maxScale, speed, Color.White, new Vector2(1.0f, 1.0f))
        {
        }

        public PopOutEffect(Vector2 maxScale, float speed, Color color, Vector2 initialScale)
        {
            this._maxScale = maxScale;
            this._speed = speed;
            this._initialSpeed = speed;
            this._initialScale = initialScale;
            this._blendColor = color; 
            this.Reset();
        }
       
        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            float increment = this._speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            float scaleX = 0.0f;
            float scaleY = 0.0f;
            
            if (this._increasing)
            {
                scaleX = this.Scale.X + increment;
                scaleY = this.Scale.Y + increment;

                if (scaleX > this._maxScale.X)
                {
                    scaleX = this._maxScale.X;
                }
                if (scaleY > this._maxScale.Y)
                {
                    scaleY = this._maxScale.Y;
                }
                if (scaleX == this._maxScale.X &&
                    scaleY == this._maxScale.Y)
                {
                    this._increasing = false;
                }
            }
            else
            {
                scaleX = this.Scale.X - increment;
                scaleY = this.Scale.Y - increment;

                if (scaleX < 0.0f)
                {
                    scaleX = 0.0f;
                }
                if (scaleY < 0.0f)
                {
                    scaleY = 0.0f;
                }
                if (scaleX == 0.0f && scaleY == 0.0f)
                {
                    this.Ended = true;
                }
            }

            this.Scale = new Vector2(scaleX, scaleY);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this._increasing = true;
            this._speed = this._initialSpeed;
            this.ColorVector = this._blendColor.ToVector4();
            this.Scale = this._initialScale;
        }
    }
}
