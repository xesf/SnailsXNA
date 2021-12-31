using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.BrainEngine.Advertising
{
    public class AdvertisingBanner
    {
        private const int BANNER_HEIGHT = 80;

        AdvertisingBannerDock _dock;
        Rectangle Rect { get; set; }
        public List<BrainAd> Ads { get; private set; }
        public Vector2 Position { get; set; }
        bool _visible;

        public AdvertisingBannerDock Dock 
        {
            get { return this._dock; }
            set 
            { 
                this._dock = value;
                this.RefreshRect();
            }
        }
        public bool Visible 
        {
            get { return this._visible; }
            set
            {
                this._visible = value;
                foreach (BrainAd ad in this.Ads)
                {
                    if (ad is PubCenterAd)
                    {
                        PubCenterAd pubCenterAd = (PubCenterAd)ad;
                        if (value)
                        {
                            if (pubCenterAd.Visible)
                            {
                                pubCenterAd.Render = true;
                            }
                        }
                        else
                        {
                            pubCenterAd.Render = false;
                        }
                    }
                    else
                    {
                        ad.Visible = value;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal AdvertisingBanner()
        {
            this.Ads = new List<BrainAd>();
            this.Dock = AdvertisingBannerDock.Top;
            this.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime time)
        {

            foreach (BrainAd ad in this.Ads)
            {
                ad.Update(time);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            BrainGame.SpriteBatch.Begin();
            BrainGame.SpriteBatch.Draw(UIScreen.ClearTexture, this.Rect, Color.Black);
            foreach (BrainAd ad in this.Ads)
            {
                if (ad.Visible && ad.Render)
                {
                    ad.Draw(this.Position);
                }
            }
            BrainGame.SpriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshRect()
        {
            switch (this.Dock)
            {
                case AdvertisingBannerDock.Top:
                    this.Rect = new Rectangle(0, 0, BrainGame.Graphics.DisplayMode.Height, BANNER_HEIGHT);
                    break;

                case AdvertisingBannerDock.Bottom:
                    this.Rect = new Rectangle(0, BrainGame.Graphics.DisplayMode.Width - BANNER_HEIGHT, BrainGame.Graphics.DisplayMode.Height, BANNER_HEIGHT);
                    break;
            }
            this.Position = new Vector2(this.Rect.X, this.Rect.Y);
        }
    }
}
