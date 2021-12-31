using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.StageObjects;

namespace TwoBrainsGames.Snails.Effects
{
    public class FloatingEffect : TransformEffectBase, ITransformEffect
    {
        #region Members
        float _speed;
        float _angle;
        int _amplitude;
        int _interval;
        int _signal;
        //Vector2 _objectPosition;
        Vector2 _auxObjectPosition;
        Liquid _liquidObj;
        bool _hasHoover = false;
        int _hooverValue = 0;
        #endregion

        #region Properties
        public float Angle { get { return _angle; } set { _angle = value; } }
        public int Amplitude { get { return _amplitude; } set { _amplitude = value; } }
        public int Interval { get { return _interval; } set { _interval = value; } }
        public int Signal { get { return _signal; } set { _signal = value; } }
        public Rectangle FloatArea 
        { 
            get 
            { 
                Rectangle rect = _liquidObj.QuadtreeCollisionBB.ToRect();
                if (_hasHoover)
                {
                    rect.Y += _hooverValue;
                    rect.Height -= _hooverValue;
                }
                return rect;
            } 
        }
        #endregion

        public FloatingEffect(float speed, Vector2 objPos, Liquid liquidObj) 
            : this(speed, objPos, liquidObj, false, 0)
        {            
        }

        public FloatingEffect(float speed, Vector2 objPos, Liquid liquidObj, bool hasHoover, int hooverValue)
        {
            _speed = speed;
            //_objectPosition = objPos;
            _auxObjectPosition = objPos;
            _liquidObj = liquidObj;
            _hasHoover = hasHoover;
            _hooverValue = hooverValue;

            // Randomize
            _amplitude = BrainGame.Rand.Next(1, 3);
            _interval = BrainGame.Rand.Next(1, 3);
            _signal = BrainGame.Rand.Next(1, 2);
            if (_signal == 2)
                _signal = -1;
            _angle = BrainGame.Rand.Next(0, 360);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            
            float x = this._signal * this._amplitude * (float)Math.Sin(this._interval * MathHelper.ToRadians(this._angle));
            this._angle += 1.0f * (float)gameTime.ElapsedGameTime.TotalMilliseconds * this._speed;
            if (this._angle > 360)
                this._angle = 1;

            float newX = this.Position.X + (x * (float)gameTime.ElapsedGameTime.TotalMilliseconds * this._speed);
            float newY = this.Position.Y - (1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds * this._speed);

            Vector3 direction = new Vector3(newX, newY, 0f);
            this.Position = direction * this._speed;

            _auxObjectPosition += this.PositionV2;

            Vector2 tmpPos = _auxObjectPosition + this.PositionV2;
            if (!this.FloatArea.Contains((int)tmpPos.X, (int)tmpPos.Y))
            {
                this.Ended = true;
            }
        }
    }
}
