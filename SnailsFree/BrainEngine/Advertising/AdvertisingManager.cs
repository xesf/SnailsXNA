using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#if ADVERTISING_SUPPORT
using Microsoft.Advertising.Mobile.Xna;
using AdRotatorXNA;
#endif

namespace TwoBrainsGames.BrainEngine.Advertising
{
    public class AdvertisingManager
    {
        public string ApplicationId { get; set; }
        public AdvertisingBanner Banner { get; private set; }
        Rectangle AdvertisingArea { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AdvertisingManager()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this.Banner = new AdvertisingBanner();

#if DEBUG
            this.ApplicationId = "test_client";
#endif
#if ADVERTISING_SUPPORT
            if (!BrainGame.Settings.UseAdRotator)
            {
                AdGameComponent.Initialize(BrainGame.Instance, this.ApplicationId);
                BrainGame.Instance.Components.Add(AdGameComponent.Current);
            }
            else
            {
                AdRotatorXNAComponent.Initialize(BrainGame.Instance);
#if DEBUG
        /*        AdRotatorXNAComponent.Current.PubCenterAppId = "test_client";
                AdRotatorXNAComponent.Current.PubCenterAdUnitId = "Image480_80";

                AdRotatorXNAComponent.Current.AdDuplexAppId = "0";

                AdRotatorXNAComponent.Current.InneractiveAppId = "DavideCleopadre_ClockAlarmNightLight_WP7";

                AdRotatorXNAComponent.Current.MobFoxAppId = "474b65a3575dcbc261090efb2b996301";
                AdRotatorXNAComponent.Current.MobFoxIsTest = true;*/
#endif


                AdRotatorXNAComponent.Current.SlidingAdDirection = SlideDirection.None;
                AdRotatorXNAComponent.Current.DefaultSettingsFileUri = "defaultAdSettings.xml";
            //    AdRotatorXNAComponent.Current.SettingsUrl = "http://www.snailsvideogame.net/tmp/defaultAdSettings.xml";

                BrainGame.Instance.Components.Add(AdRotatorXNAComponent.Current);
            }
#endif
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime time)
        {
            if (this.Banner.Visible)
            {
                this.Banner.Update(time);
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            if (this.Banner.Visible)
            {
                this.Banner.Draw();
            } 
        }
    }
}
