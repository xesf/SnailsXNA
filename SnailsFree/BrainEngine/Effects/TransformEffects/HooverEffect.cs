using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class HooverEffect : TransformEffectBase, ITransformEffect
    {

        #region Constants
        #endregion

        #region Members
        float Speed { get; set; }
        float Power { get; set; }
        float Angle { get; set; }
        float HooverAngle { get; set; }
        Vector3 CurrentPosition { get; set; }
        protected Vector3 _previousPosition;
        #endregion

        #region Constructs
        /// <summary>
        /// 
        /// </summary>
        public HooverEffect(float speed, float power, float angle)
        {
            this.Speed = speed;
            this.Power = power;
            this.HooverAngle = angle;
            this.Angle = 0;
        }
        #endregion

        #region ITransformEffect Members

        /// <summary>
        /// 
        /// </summary>
        public override void  Update(BrainGameTime gameTime)
        {
            float y = (float)Math.Sin(MathHelper.ToRadians(this.Angle)) * this.Power * 10.0f;
            this.CurrentPosition = new Vector3(0.0f, y, 0.0f);
            Matrix rotateMat = Matrix.CreateRotationZ(MathHelper.ToRadians(this.HooverAngle));
            this.CurrentPosition = Vector3.Transform(this.CurrentPosition, rotateMat);
            this.Angle += 1.0f * (float)gameTime.ElapsedGameTime.TotalMilliseconds * this.Speed / this.Power;
            if (this.Angle > 360)
            {
                this.Angle = this.Angle - 360;
            }
            this.Position = this.CurrentPosition - this._previousPosition;
            this._previousPosition = this.CurrentPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
          base.Reset();
        }
        #endregion
    }
}
