using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class LinearMoveEffect : TransformEffectBase, ITransformEffect
    {  
        #region Member vars
        private float _Speed;
        //private float _Angle;
        private float _AngleSin;
        private float _AngleCos;
        private Vector3 _direction;
        #endregion

        #region Contructs and overrides
        /// <summary>
        /// 
        /// </summary>
        public LinearMoveEffect(float speed, float angle)
        {
            this._Speed = speed;
            //this._Angle = angle;
            this._AngleSin = (float)Math.Sin(MathHelper.ToRadians(angle));
            this._AngleCos = (float)Math.Cos(MathHelper.ToRadians(angle));
            this._direction = new Vector3((float)this._AngleCos, -(float)this._AngleSin, 0f);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            float speed = this._Speed * gameTime.ElapsedGameTime.Milliseconds / 10;
            this.Position = this._direction * speed;
        }
        #endregion
    }
}
