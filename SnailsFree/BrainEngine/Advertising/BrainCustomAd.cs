using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.BrainEngine.Advertising
{
    public class BrainCustomAd : BrainAd
    {

        public SpriteAnimation Animation { get; set; }
        public Vector2 Position { get; set; }
        private string SpriteResourceId { get; set; }
        public bool ConnectedWithPubCenterAd { get; set; }

        public BrainCustomAd(string spriteResourceId)
        {
            this.SpriteResourceId = spriteResourceId;
            this.ConnectedWithPubCenterAd = false;
            this.Render = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this.Animation = new SpriteAnimation(this.SpriteResourceId, ResourceManager.RES_MANAGER_ID_STATIC); 
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateClickableArea(Vector2 bannerPos)
        {
            this.ClickableArea = new BoundingSquare(this.Position + bannerPos, this.Animation.Sprite.Frames[0].Width, this.Animation.Sprite.Frames[0].Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime time)
        {
            if (!this.Visible)
            {
                return;
            }

            if (this.Animation != null)
            {
                this.Animation.Update(time, true);
            }
            base.Update(time);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(Vector2 bannerPosition)
        {
            if (this.Animation != null)
            {
                this.Animation.Draw(bannerPosition + this.Position, BrainGame.SpriteBatch);
            }
        }
    }
}
