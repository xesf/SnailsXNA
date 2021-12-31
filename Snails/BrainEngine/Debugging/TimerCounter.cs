using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace TwoBrainsGames.BrainEngine.Debugging
{
    public class TimerCounter : BrainPerformanceCounter
    {
#if !WP7 && !WIN8
        Stopwatch StopWatch { get; set; }
#endif
        /// <summary>
        /// 
        /// </summary>
        public TimerCounter(Game game) :
            base(game)
        {
#if !WP7 && !WIN8
            this.StopWatch = new Stopwatch();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void End()
        {
#if !WP7 && !WIN8
          this.StopWatch.Stop();
          this.Counter = this.StopWatch.Elapsed.TotalMilliseconds;
#endif
        }

#if !WP7 && !WIN8
        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this.StopWatch.Reset();
            this.StopWatch.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
           this.Counter = this.StopWatch.Elapsed.TotalMilliseconds;
           base.Update(gameTime);
        }
#endif
    }
}
