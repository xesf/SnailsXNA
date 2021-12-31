using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.Snails.Screens.Transitions;

namespace TwoBrainsGames.Snails.Screens
{
    class NewGameScreen : UIScreen
    {
        #region Consts
        private const string CURSOR_SPRITE = "DefaultCursor"; // This should be global...
        private const string LABEL_TITLE = "Profiles";
        private const string LABEL_INFO = "Choose your player's name: ";
        #endregion

        private TextFont _fontTextBigger;

        private UITextFontLabelInput _textInput;
        private TextFont _textFont;
        //private UIButton _backButton;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public NewGameScreen(ScreenNavigator owner) :
            base(owner)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            
            // fonts
            this._fontTextBigger = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-big", ResourceManager.ResourceManagerCacheType.Static);
            this._textFont = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);

            // Title
            UITextFontLabel title = new UITextFontLabel(this, this._fontTextBigger, LABEL_TITLE);
            title.Position = new Vector2(0, 1500);
            title.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this.Controls.Add(title);

            // info label
            UITextFontLabel info = new UITextFontLabel(this, this._textFont, LABEL_INFO);
            info.Position = new Vector2(950, 3200);
            this.Controls.Add(info);

            // input control
            this._textInput = new UITextFontLabelInput(this, this._textFont);
            this._textInput.Position = new Vector2(5900, 3200);
            this.Controls.Add(this._textInput);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BackButton_OnPress(IUIControl sender)
        {
            BackToScreen();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            
        }

        private void BackToScreen()
        {
            string screen = this.Navigator.GlobalCache.Get<string>(GlobalCacheKeys.NEW_GAME_BACK_SCREEN);
            if (screen == ScreenType.MainMenu.ToString())
            {
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleAndMenuVisible);
            }
            this.NavigateTo(screen, ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
            if (this.InputController.ActionAccept && _textInput.HasText())
            {
                SnailsGame.ProfilesManager.CreateProfile(_textInput.Text);

                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.NewGame);
                this.NavigateTo(ScreenGroupType.InGame.ToString(), ScreenType.StageStart.ToString(), ScreenTransitions.FadeOut, ScreenTransitions.FadeIn);
            }
            else if (this.InputController.ActionBack)
            {
                BackToScreen();
            }
        }
    }
}
