using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class ShakeEffect : TransformEffectBase, ITransformEffect
    {
        #region Member vars
        int _ShakeCurrentTime;
        int _ShakeTime;
        int _ShakeStrength;
        Vector2 _PreviousShake;
        #endregion

        #region Contructs and overrides
        /// <summary>
        /// 
        /// </summary>
        public ShakeEffect()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ShakeEffect(int shakeTime, int shakeStrength)
        {
            this.Reset(shakeTime, shakeStrength);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
          
          if (this._ShakeCurrentTime <= 0)
          {
            this.Ended = true;
            this.Active = false;
            return;
          }
            
          int strength = this._ShakeCurrentTime * this._ShakeStrength / this._ShakeTime;
          int shakeX = strength - BrainGame.Rand.Next(strength * 2);
          int shakeY = strength - BrainGame.Rand.Next(strength * 2);
          this._ShakeCurrentTime -= gameTime.ElapsedGameTime.Milliseconds;
          this.VirtualPosition = new Vector3(shakeX - this._PreviousShake.X, shakeY - this._PreviousShake.Y, 0.0f);
          this._PreviousShake = new Vector2(shakeX, shakeY);

        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset(int shakeTime, int shakeStrength)
        {
            this.Reset();
            this._ShakeTime = shakeTime;
            this._ShakeStrength = shakeStrength;
            this._ShakeCurrentTime = shakeTime;
        }
        #endregion

    }
}
