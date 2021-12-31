using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Debugging
{
    class MemoryPeakCounter : BrainPerformanceCounter
    {

        public MemoryPeakCounter(Game game)
            : base(game)
        {
        }

      
        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
#if WP7
            this.Counter = (double)(long)(Microsoft.Phone.Info.DeviceStatus.ApplicationPeakMemoryUsage / 1024);
#endif
        }
    }
}
