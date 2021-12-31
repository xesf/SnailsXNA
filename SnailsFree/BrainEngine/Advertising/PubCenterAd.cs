using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#if ADVERTISING_SUPPORT
using Microsoft.Advertising.Mobile.Xna;
#endif
namespace TwoBrainsGames.BrainEngine.Advertising
{
    public class PubCenterAd : BrainAd
    {
        private const int AD_WIDTH = 480;
        private const int AD_HEIGHT = 80;

        public event AdEventHandler OnError;
        public event AdEventHandler OnRefreshed;

        BrainAdAlignment _alignment;

        public string Id { get; private set; }
        string AdBlockId { get; set; }
#if ADVERTISING_SUPPORT
        DrawableAd BannerAd { get; set; }
#endif
        BrainAdAlignment Alignment
        {
            get { return this._alignment; }
            set
            {
                this._alignment = value;
#if ADVERTISING_SUPPORT
                // This depends on orientation , for now assume landscape...
                switch (this._alignment)
                {
                    case BrainAdAlignment.Bottom:
                        this.BannerAd.DisplayRectangle = new Rectangle(0, BrainGame.Graphics.DisplayMode.Width - AD_HEIGHT,
                                                                        BrainGame.Graphics.DisplayMode.Height, AD_HEIGHT);
                        break;
                    case BrainAdAlignment.Top:
                        this.BannerAd.DisplayRectangle = new Rectangle(0, 0, BrainGame.Graphics.DisplayMode.Height, AD_HEIGHT);
                        break;
                    case BrainAdAlignment.BottomRight:
                        this.BannerAd.DisplayRectangle = new Rectangle(BrainGame.Graphics.DisplayMode.Height - AD_WIDTH, BrainGame.Graphics.DisplayMode.Width - AD_HEIGHT,
                                                                        AD_WIDTH, AD_HEIGHT);
                        break;
                }
#endif
            }
        }

        internal override bool Render
        {
            set 
            { 
#if ADVERTISING_SUPPORT
                this.BannerAd.Visible = value; 
#endif
            }
        }

        #if ADVERTISING_SUPPORT
        public override bool Visible
        {
            set
            {
                if (value == true)
                {
                    if (BrainGame.AdvertisingManager.Banner.Visible)
                    {
                        this.BannerAd.Visible = value;
                    }
                }
                else
                {
                    this.BannerAd.Visible = value;
                }
                base.Visible = value;
            }
        }
        #endif

        public PubCenterAd(string adBlockId, BrainAdAlignment alignment)
        {
            this.AdBlockId = adBlockId;
#if DEBUG
            this.AdBlockId = "Image480_80";
#endif
#if ADVERTISING_SUPPORT

            this.BannerAd = AdGameComponent.Current.CreateAd(this.AdBlockId, new Rectangle(0, 0, AD_WIDTH, AD_HEIGHT), true);
            this.BannerAd.Visible = false;
            this.BannerAd.VisibleChanged += new EventHandler(BannerAd_VisibleChanged);
            this.BannerAd.AdRefreshed += new EventHandler(BannerAd_AdRefreshed);
            this.BannerAd.ErrorOccurred += new EventHandler<Microsoft.Advertising.AdErrorEventArgs>(BannerAd_ErrorOccurred);
#endif
            this.Alignment = alignment;
        }

        /// <summary>
        /// 
        /// </summary>
        void BannerAd_AdRefreshed(object sender, EventArgs e)
        {
            if (this.OnRefreshed != null)
            {
                this.OnRefreshed();
            }
        }
#if ADVERTISING_SUPPORT
        /// <summary>
        /// 
        /// </summary>
        void BannerAd_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            if (this.OnError != null)
            {
                this.OnError();
            }
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        void BannerAd_VisibleChanged(object sender, EventArgs e)
        {
            foreach (BrainAd ad in BrainGame.AdvertisingManager.Banner.Ads)
            {
                if (ad is BrainCustomAd)
                {
                    BrainCustomAd brainCustomAd = (BrainCustomAd)ad;
                    #if ADVERTISING_SUPPORT
                    if (brainCustomAd.ConnectedWithPubCenterAd)
                    {
                        ad.Render = this.BannerAd.Visible;
                    }
                    else
                    {
                        ad.Render = !this.BannerAd.Visible;
                    }
                    #endif
                }
            }
        }
    }
}
