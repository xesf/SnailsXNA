using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    public class UISnailsThemeIcon : UIImage
    {
        ThemeType _theme;

        public ThemeType Theme
        {
            get
            {
                return this._theme;
            }
            set
            {
                this._theme = value;
                this.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/" + this._theme.ToString() + "ThemeIcon");
              
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UISnailsThemeIcon(UIScreen screenOwner) :
            base(screenOwner)
        {
        }
    }
}
