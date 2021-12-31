using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdRotatorXNA;

namespace TwoBrainsGames.BrainEngine.Advertising
{
    public class AdRotatorAd : BrainAd
    {
        public const int AD_WIDTH = 480;
        public const int AD_HEIGHT = 80;

        BrainAdAlignment _alignment;

        public string PubCenterAppId 
        {
            get { return AdRotatorXNAComponent.Current.PubCenterAppId; }
            set { AdRotatorXNAComponent.Current.PubCenterAppId = value; }
        }

        public string PubCenterAdUnitId 
        {
            get { return AdRotatorXNAComponent.Current.PubCenterAdUnitId; }
            set { AdRotatorXNAComponent.Current.PubCenterAdUnitId = value; }
        }

        public string AdDuplexAppId 
        {
            get { return AdRotatorXNAComponent.Current.AdDuplexAppId; }
            set { AdRotatorXNAComponent.Current.AdDuplexAppId = value; }
        }
        
        public string InneractiveAppId 
        {
            get { return AdRotatorXNAComponent.Current.InneractiveAppId; }
            set { AdRotatorXNAComponent.Current.InneractiveAppId = value; }
        }
        
        public string MobFoxAppId 
        {
            get { return AdRotatorXNAComponent.Current.MobFoxAppId; }
            set { AdRotatorXNAComponent.Current.MobFoxAppId = value; }
        }

        public bool MobFoxIsTest
        {
            get { return AdRotatorXNAComponent.Current.MobFoxIsTest; }
            set { AdRotatorXNAComponent.Current.MobFoxIsTest = value; }
        }
        
        public override bool Visible 
        {
            get { return AdRotatorXNAComponent.Current.Visible; }
            set 
            { 
                AdRotatorXNAComponent.Current.Visible = value; 
            } 
        }

        public BrainAdAlignment Alignment
        {
            get { return this._alignment; }
            set
            {
                this._alignment = value;
                // This depends on orientation , for now assume landscape...
                switch (this._alignment)
                {
                    case BrainAdAlignment.BottomRight:
                        AdRotatorXNAComponent.Current.AdPosition = 
                            new Microsoft.Xna.Framework.Vector2( 
                            BrainGame.Graphics.DisplayMode.Height - AD_WIDTH, 
                            BrainGame.Graphics.DisplayMode.Width - AD_HEIGHT);
                        break;
                }

            }
        }

    }
}
