using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Debugging
{
    public class FPSCounter : BrainPerformanceCounter
    {
        private float ElapsedTime { get; set; }
        private float totalFrames;

        public FPSCounter(Game game)
            : base(game)
        {
        }

      
        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            totalFrames++;

            if (this.ElapsedTime >= 1000)
            {
                this.Counter = totalFrames;
                totalFrames = 0;
                this.ElapsedTime -= 1000;
            }
        }

    }
}
