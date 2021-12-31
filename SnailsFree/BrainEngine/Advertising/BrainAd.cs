using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#if ADVERTISING_SUPPORT
#endif
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.BrainEngine.Advertising
{
    public class BrainAd
    {
        public delegate void AdEventHandler();
        public event AdEventHandler OnAccept;

        public virtual bool Visible { get; set; }
        internal virtual bool Render { get; set; }
        public BoundingSquare ClickableArea { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public BrainAd()
        {
            this.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update(BrainGameTime time) 
        { 
            if (this.OnAccept != null)
            {
                if (BrainGame.ScreenNavigator.InputController.ActionAccept &&
                    this.ClickableArea.Contains(BrainGame.ScreenNavigator.InputController.MotionPosition))
                {
                    this.OnAccept();
                    BrainGame.ScreenNavigator.InputController.Reset();
                }
            }
        }

        public virtual void Draw(Vector2 bannerPosition) { }
    }
}
