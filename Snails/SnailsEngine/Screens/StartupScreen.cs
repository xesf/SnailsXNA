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
        public override void OnUpdate(BrainGameTime gameTime)
        {
#if !WIN8
            this.NavigateTo(ScreenType.BrainsLogo.ToString(), null, null);
#else
            BrainGame.ClearColor = Color.White;
            // o logo já está no SplashScreen da App Metro por isso vamos logo directamente para o menu
            this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), ScreenTransitions.FadeOutWhite, ScreenTransitions.FadeInWhite);
#endif
        }

    }
}
