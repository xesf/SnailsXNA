using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Effects
{
    class TrembleEffect : TransformEffectBase
    {
        const float MIN_ANGLE = -1f;
        const float MAX_ANGLE = 1f;

        private int _direction;

        public TrembleEffect() 
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this.Rotation += 0.05f * this._direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (this.Rotation < MIN_ANGLE)
            {
                this.Rotation = MIN_ANGLE;
                this._direction *= -1;
            }
            else
            if (this.Rotation > MAX_ANGLE)
            {
                this.Rotation = MAX_ANGLE;
                this._direction *= -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this._direction = 1;
            this.Rotation = 0;
        }
    }
}
