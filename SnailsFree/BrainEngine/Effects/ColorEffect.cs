using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects
{
    public class ColorEffect : TransformEffectBase
    {

        Vector4 _startColor;
        Vector4 _endColor;
        Vector4 _vColor;
        float _speed;
        bool _loop;
        public Color _finalColor; // Color to set when the effect ends, equals to endColor by default

        public Color StartColor
        {
            set { this._startColor = value.ToVector4(); }
            get { return new Color(this._startColor); }
        }

        public Color EndColor
        {
            set { this._endColor = value.ToVector4(); }
            get { return new Color(this._endColor); }
        }

        public Color CurrentColor
        {
            set { this._vColor = value.ToVector4(); }
            get { return new Color(this._vColor); }
        }
        public override bool Ended
        {
            get { return base.Ended; }
            set
            {
                base.Ended = value;
                if (value)
                {
                    this.ColorVector = this._finalColor.ToVector4();
                }
            }
        }

        Color StartColorEx { get; set; }
        Color EndColorEx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ColorEffect(Color startColor, Color endColor, float speed, bool loop) :
            this(startColor, endColor, speed, loop, endColor)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public ColorEffect(Color startColor, Color endColor, float speed, bool loop, Color finalColor) :
            this(startColor, endColor, speed, loop, finalColor, 0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ColorEffect(Color startColor, Color endColor, float speed, bool loop, Color finalColor, double expirationTime)
        {
            this._startColor = startColor.ToVector4();
            this._endColor = endColor.ToVector4();
            this._vColor = this._startColor;
            this.ColorVector = this._startColor;
            this._speed = speed;
            this._loop = loop;
            this._finalColor = finalColor;
            this._expirationTime = expirationTime;
            this.SetScaleOnEnd = false;
            this.StartColorEx = startColor;
            this.EndColorEx = endColor;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this._vColor = this._startColor;
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetEx()
        {
            this._startColor = this.StartColorEx.ToVector4();
            this._endColor = this.EndColorEx.ToVector4();
            this._vColor = this._startColor;
            this.ColorVector = this._startColor;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            float msecs = (float)(UseRealTime ? gameTime.ElapsedRealTime.TotalMilliseconds : gameTime.ElapsedGameTime.TotalMilliseconds);

            float speed = this._speed * msecs / 10.0f;
            this._vColor.X += (this._endColor.X - this._startColor.X) * speed;
            this._vColor.Y += (this._endColor.Y - this._startColor.Y) * speed;
            this._vColor.Z += (this._endColor.Z - this._startColor.Z) * speed;
            this._vColor.W += (this._endColor.W - this._startColor.W) * speed;

            // If increases
            if (this._endColor.X > this._startColor.X && this._vColor.X > this._endColor.X)
            {
                this._vColor.X = this._endColor.X;
            }

            if (this._endColor.Y > this._startColor.Y && this._vColor.Y > this._endColor.Y)
            {
                this._vColor.Y = this._endColor.Y;
            }

            if (this._endColor.Z > this._startColor.Z && this._vColor.Z > this._endColor.Z)
            {
                this._vColor.Z = this._endColor.Z;
            }

            if (this._endColor.W > this._startColor.W && this._vColor.W > this._endColor.W)
            {
                this._vColor.W = this._endColor.W;
            }

            // If decreases
            if (this._endColor.X < this._startColor.X && this._vColor.X < this._endColor.X)
            {
                this._vColor.X = this._endColor.X;
            }

            if (this._endColor.Y < this._startColor.Y && this._vColor.Y < this._endColor.Y)
            {
                this._vColor.Y = this._endColor.Y;
            }

            if (this._endColor.Z < this._startColor.Z && this._vColor.Z < this._endColor.Z)
            {
                this._vColor.Z = this._endColor.Z;
            }

            if (this._endColor.W < this._startColor.W && this._vColor.W < this._endColor.W)
            {
                this._vColor.W = this._endColor.W;
            }

            if (this._endColor == this._vColor)
            {
                if (this._loop)
                {
                    Vector4 saveColor = this._endColor;
                    this._endColor = this._startColor;
                    this._startColor = saveColor;
                    this._vColor = this._startColor;
                }
                else
                {
                    this.Ended = true;
                }
            }

            this.ColorVector = this._vColor;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reverse()
        {
            Vector4 saveColor = this._startColor;
            this._startColor = this._vColor;
            this._endColor = saveColor;
            this.Ended = false;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Reset(Color startColor, Color endColor)
        {
            base.Reset();
            this.StartColor = startColor;
            this.EndColor = endColor;
            this._vColor = this._startColor;
            this.Ended = false;
        }
    }
}