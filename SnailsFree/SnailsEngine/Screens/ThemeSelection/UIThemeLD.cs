using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Configuration;

namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
    public class UIThemeLD : UITheme
    {
        public UIThemeLD(UIScreen screenOwner, ThemeType themeId) :
            base(screenOwner, themeId)
        {
			         this.FocusEffectEnabled = false;

            // Theme image
			//  this._imgTheme.Position = new Vector2(500f, 800f);

            // Unlocked stages Locker
			//  this._imgSmallLocker.Position = new Vector2(4250f, 950f);
            // Stages unlocked
			//   this._lblStagesUnlocked.Position = this._imgSmallLocker.Position + new Vector2(950f, 400f);
			//	this._imgSmallLocker.Position = this.PixelsToScreenUnits(new Vector2(325, 50));

            // Gold medals earned medal
			//  this._imgMedal.Position = new Vector2(4250f, 2550f);
            // Gold medals earned 
			// this._lblGoldMedalsEarned.Position = this._imgMedal.Position + new Vector2(900f, 500f);

            // Theme locker
			//	this._lockerImage.Position = new Vector2(1050, 1400);
			// this._lblToUnlock.Position = new Vector2(0, 2430);
            // Garden stages needed
			//  this._lblGardenNeeded.Position = this._lblToUnlock.Position + new Vector2(0, 620);
            // Ancient egypt stages needed
			//  this._lblEgyptNeeded.Position = this._lblGardenNeeded.Position + new Vector2(0, 550);
            // Robot factory stages needed
			//  this._lblFactoryNeeded.Position = this._lblEgyptNeeded.Position + new Vector2(0, 550);

	    }

    }
}
