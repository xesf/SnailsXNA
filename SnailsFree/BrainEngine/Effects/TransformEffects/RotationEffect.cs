using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class RotationEffect : TransformEffectBase, ITransformEffect
    {
        #region Member vars
        public float Speed; // In degrees per second
        public float RotationSum { get; set; }
        #endregion

        #region Constructs and overrides

        /// <summary>
        /// 
        /// </summary>
        public RotationEffect(float speed)
        {
            this.Speed = speed;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            float rot = this.Speed * gameTime.ElapsedGameTime.Milliseconds / 100;
            this.Rotation = rot;
            this.RotationSum += rot;
        }
        #endregion
    }
}
