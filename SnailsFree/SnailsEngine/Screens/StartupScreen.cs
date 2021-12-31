using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Screens.Transitions;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Screens
{
    class StartupScreen : UIScreen
	{
        public StartupScreen(ScreenNavigator owner) :
            base(owner)
        {
            this.BackgroundColor = Color.White;
        }
		/// <summary>
		/// 
		/// </summary>
		public override void OnStart()
		{
			BrainGame.DisplayHDDAccessIcon = true;
			BrainGame.ClearColor = Color.White;
		}

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
			this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), ScreenTransitions.FadeOutWhite, ScreenTransitions.FadeInWhite);
		}

    }
}
