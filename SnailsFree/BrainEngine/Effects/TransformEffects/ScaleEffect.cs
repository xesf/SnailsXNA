using System;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class ScaleEffect : TransformEffectBase
    {
        Vector2 _initialScale;
        float _speed;
        Vector2 _finalScale;
        bool _loop;
        float _directionX;
        float _directionY;
        float _minX;
        float _maxX;
        float _minY;
        float _maxY;
        float _decayFactor;
        float _decayThreshold;
        
        /// <summary>
        /// 
        /// </summary>
        public ScaleEffect() :
            this(new Vector2(1f, 1f), 0.01f, new Vector2(2f, 2f), false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ScaleEffect(Vector2 initialScale, float speed, Vector2 finalScale, bool loop)
        {
            this.Reset(initialScale, speed, finalScale, loop, 0f, 0f);
        }

        /// <summary>
        /// 
        /// </summary>
        public ScaleEffect(Vector2 initialScale, float speed, Vector2 finalScale, bool loop, float decayFactor, float decayThreshold)
        {
            this.Reset(initialScale, speed, finalScale, loop, decayFactor, decayThreshold);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset(Vector2 initialScale, float speed, Vector2 finalScale, bool loop, float decayFactor, float decayThreshold)
        {
            this._initialScale = initialScale;
            this._speed = speed;
            this._finalScale = finalScale;
            this._loop = loop;
            this._decayFactor = decayFactor;
            this._decayThreshold = decayThreshold;

            _minX = this._initialScale.X;
            _maxX = this._finalScale.X;
            if (_minX > _maxX)
            {
                _minX = this._finalScale.X;
                _maxX = this._initialScale.X;
            }

            _minY = this._initialScale.Y;
            _maxY = this._finalScale.Y;
            if (_minY > _maxY)
            {
                _minY = this._finalScale.Y;
                _maxY = this._initialScale.Y;
            }
            this.Reset(); 
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this._directionX = (this._initialScale.X > this._finalScale.X ? -1.0f : 1.0f);
            this._directionY = (this._initialScale.Y > this._finalScale.Y ? -1.0f : 1.0f);
            this.Scale = this._initialScale;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            float increment = this._speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            float scaleX = this.Scale.X + (increment * this._directionX);
            float scaleY = this.Scale.Y + (increment * this._directionY);



            if (this._decayFactor > 0f)
            {
                if (this._directionX > 0)
                {
                    if (this.Scale.X > this._decayThreshold ||
                        this.Scale.Y > this._decayThreshold)
                    {
                        this._speed *= Math.Abs(0.95f - (this._decayFactor * (float)gameTime.ElapsedGameTime.TotalSeconds));
                    }
                }
                else
                {
                    if (this.Scale.X < this._decayThreshold ||
                        this.Scale.Y < this._decayThreshold)
                    {
                        this._speed *= Math.Abs(0.95f - (this._decayFactor * (float)gameTime.ElapsedGameTime.TotalSeconds));
                    }
                }

                if (Math.Abs(this._speed) <= 0.05f)
                {
                    if (this._directionX > 0)
                    {
                        scaleX = this._maxX;
                    }
                    else
                    {
                        scaleX = this._minX;
                    }
               
                    if (this._directionY > 0)
                    {
                        scaleY = this._maxY;
                    }
                    else
                    {
                        scaleY = this._minY;
                    }
                }
            }

            // X
            if (scaleX > this._maxX)
            {
                scaleX = this._maxX;
                if (this._loop)
                {
                    this._directionX = -1.0f;
                }
            }

            if (scaleX < this._minX)
            {
                scaleX = this._minX;
                if (this._loop)
                {
                    this._directionX = 1.0f;
                }
            }

            // Y
            if (scaleY > this._maxY)
            {
                scaleY = this._maxY;
                if (this._loop)
                {
                    this._directionY = -1.0f;
                }
            }

            if (scaleY < this._minX)
            {
                scaleY = this._minX;
                if (this._loop)
                {
                    this._directionY = 1.0f;
                }
            }

            this.Scale = new Vector2(scaleX, scaleY);

            if (scaleX == this._finalScale.X &&
                scaleY == this._finalScale.Y &&
                this._loop == false)
            {
                this.Ended = true;
            }

        }
    }
}
