using System;
using TwoBrainsGames.BrainEngine.UI.Screens;
///
/// The Quit screen does nothing but quiting the game
/// It's useful to use a FadeOut effect transition when quiting the game
namespace TwoBrainsGames.Snails.Screens
{
    class QuitGameScreen : UIScreen
    {
        
        public QuitGameScreen(ScreenNavigator owner) :
            base(owner)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainEngine.BrainGameTime gameTime)
        {
            SnailsGame.ProfilesManager.Save();
            // Just quit the game
            SnailsGame.QuitGame();
        }

    }
}
