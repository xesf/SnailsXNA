using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Screens
{
    class OptionsScreen : SnailsScreen
    {

        enum ScreenState
        {
            None,
            MainMenu,
            SoundFxToggle,
            MusicToggle,
            Language,
            ControlsHelp,
            HowToPlay
        }

        public enum StartupType
        {
            MenuHidden,
            MenuVisible
        }

        UIMainMenuBodyPanel _pnlBody;
        UISnailsMenu _mnuOptions;
        UISoundMenu _mnuSound;
        UIXBoxControls _xboxController;
        UISnailsMenuItem _itmToggleFullscreen;
        UILanguageMenu _mnuLanguage;
        StartupType _startType;
        UISnailsMenuItem _itemScreenSize;
        UITimer _tmrGenerir;
        protected UIIntroPicture _introPicture;

        ScreenState State { get; set; }
        bool ShowHowToPlayOption { get; set; } // If true the menu shows How To Play option
        bool ShowCreditsOption { get; set; } // If true the menu shows the Credits option
        /// <summary>
        /// 
        /// </summary>
        public OptionsScreen(ScreenNavigator owner) :
            base(owner, ScreenType.Options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.Name = "Options";

            this.BackgroundImage = null;

            // Screen setup
            this.ShowCreditsOption = true;
            this.ShowHowToPlayOption = true;

            //this.BackgroundImageBlendColor = Colors.OptionsScrBkColor;

            // Game title
            /*this._gameTitle = new UISnailsTitle(this);
            this._gameTitle.Name = "_gameTitle";
            this.Controls.Add(this._gameTitle);*/

            // Intro image
            this._introPicture = new UIIntroPicture(this);
            this._introPicture.Name = "_introPicture2";
            this._introPicture.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._introPicture.HideEffect = new ColorEffect(new Color(255, 255, 255, 255), new Color(0, 0, 0, 0), 0.05f, false);
            this._introPicture.ShowEffect = new ColorEffect(new Color(0, 0, 0, 0), new Color(255, 255, 255, 255), 0.025f, false);
            this.Controls.Add(this._introPicture);

            // Body - see UIMainMenuBodyPanel for remarks
            this._pnlBody = new UIMainMenuBodyPanel(this);
            this.Controls.Add(this._pnlBody);

            // Main menu
            this._mnuOptions = new UISnailsMenu(this);
            this._mnuOptions.Name = "_mnuOptions";
            this._mnuOptions.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._mnuOptions.Size = new BrainEngine.UI.Size(3000.0f, 3800.0f);
            this._mnuOptions.TextResourceId = "MNU_ITEM_OPTIONS";
            this._mnuOptions.DefaultItemIndex = 0;
            this._mnuOptions.ItemSize = UISnailsMenu.MenuItemSize.Medium;
            this._mnuOptions.OnMenuShown += new UIControl.UIEvent(_mnuOptions_OnMenuShown);
            this._mnuOptions.OnItemSelectedBegin += new UIControl.UIEvent(_mnuOptions_OnItemSelectedBegin);
            this._mnuOptions.OnBackPressed += new UIControl.UIEvent(_mnuOptions_OnBackPressed);
            this._mnuOptions.WithBackButton = true;
            this._pnlBody.Controls.Add(this._mnuOptions);

            // Menu items
            this._mnuOptions.AddMenuItem("MNU_ITEM_SOUND_SETTINGS", this.OptionsMenu_OnSoundSettings, 0);
            this._itmToggleFullscreen = this._mnuOptions.AddMenuItem(null, this.OptionsMenu_OnToggleFullscreen, 0, false, SnailsGame.GameSettings.AllowToggleFullScreen);

            if (!SnailsGame.GameSettings.SingleLanguage)
            {
                this._mnuOptions.AddMenuItem("MNU_ITEM_LANGUAGE", this.OptionsMenu_OnLanguage, 0);
            }
            this._mnuOptions.AddMenuItem("MNU_ITEM_CONTROLS", this.OptionsMenu_OnControlsHelp, 0, true, SnailsGame.GameSettings.UseGamepad);
            this._itemScreenSize = this._mnuOptions.AddMenuItem("MNU_ITEM_SCREEN_SIZE", this.OptionsMenu_OnScreenSize, 0, false, SnailsGame.GameSettings.AllowOverscanAdjustment);
        //    this._mnuOptions.AddMenuItem("MNU_ITEM_HOW_TO_PLAY", this.OptionsMenu_OnHowToPlay, 0, false, this.ShowHowToPlayOption);
            this._mnuOptions.AddMenuItem("MNU_ITEM_CREDITS", this.OptionsMenu_OnCredits, 0, false, this.ShowCreditsOption);
//            this._mnuOptions.AddMenuItem("MNU_ITEM_AWARDS", this.OptionsMenu_OnAwards, 0, false, true);
            //  this._mnuOptions.AddMenuItem("MNU_ITEM_BACK", this.OptionsMenu_OnBackToMain, InputBase.InputActions.Back, true);

            // Language menu
            this._mnuLanguage = new UILanguageMenu(this);
            this._mnuLanguage.OnHide += new UIControl.UIEvent(_mnuLanguage_OnHide);
            this._mnuLanguage.OnShow += new UIControl.UIEvent(_mnuLanguage_OnShow);
            this._mnuLanguage.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._mnuLanguage.OnLanguageSelected += new UIControl.UIEvent(_mnuLanguage_OnLanguageSelected);
            this._pnlBody.Controls.Add(this._mnuLanguage);

            // Music menu
            this._mnuSound = new UISoundMenu(this);
            this._mnuSound.OnHide += new UIControl.UIEvent(_mnuSound_OnHide);
            this._mnuSound.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._mnuSound.OnShow += new UIControl.UIEvent(_mnuSound_OnShow);
            this._pnlBody.Controls.Add(this._mnuSound);

            // A  generic timer, used for diferent things
            this._tmrGenerir = new UITimer(this);
            this.Controls.Add(this._tmrGenerir);


            // XBox controller
            this._xboxController = new UIXBoxControls(this);
            this._xboxController.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._xboxController.OnDismiss += new UIControl.UIEvent(_xboxController_OnDismiss);
            this._xboxController.OnShow += new UIControl.UIEvent(_xboxController_OnShow);
            this._pnlBody.Controls.Add(this._xboxController);


            this.OnOpenTransitionEnded += new UIControl.UIEvent(OptionsScreen_OnOpenTransitionEnded);
            this.OnPopupClosed += new UIControl.UIEvent(OptionsScreen_OnPopupClosed);
            this.ShowRateTag = SnailsGame.GameSettings.AllowRate;
        }


        /// <summary>
        /// This is needed because if the screen starts with the menu already open,
        /// we have to set EnableInput when the screen transition open
        /// </summary>
        void OptionsScreen_OnOpenTransitionEnded(IUIControl sender)
        {
            if (this._startType == StartupType.MenuVisible)
            {
                this.EnableInput();
                this._mnuOptions.SetFocus(this._itemScreenSize);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void OptionsScreen_OnPopupClosed(IUIControl sender)
        {
            this.EnableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            this._introPicture.Initialize();
            this._introPicture.ShowBackgroundPanel();
            this._introPicture.SetBackgroundAlpha();

            this._introPicture.SetSaveState(this.Navigator.GlobalCache.Get<TwoBrainsGames.Snails.Screens.CommonControls.UIIntroPicture.IntroPictureSaveState>(GlobalCacheKeys.INTRO_PICTURE_STATE));

            this.State = ScreenState.None;
            this.FooterMessage.Visible = true;
            this._mnuOptions.IdxLastItemSelected = 0;
            this.SetToggleScreenModeMenuItemName();
            this._mnuLanguage.Visible = false;
            this._mnuSound.Visible = false;
            this._xboxController.Visible = false;
            this.DisableInput();         

            this._startType = this.Navigator.GlobalCache.Get<StartupType>(GlobalCacheKeys.OPTIONS_STARTUP_MODE, StartupType.MenuHidden);

            switch (this._startType)
            {
                case StartupType.MenuHidden:
                    this._mnuOptions.Visible = false;
                    this._mnuOptions.Show();
                    break;

                case StartupType.MenuVisible:
                    this._mnuOptions.ShowWithoutEffects();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _tmrGenerir_OnTimerShowMainMenu(IUIControl sender)
        {
            this._mnuOptions.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _mnuOptions_OnMenuShown(IUIControl sender)
        {
            this.EnableInput();
            this._mnuOptions.SetFocusOnLastSelectedItem();
            this.State = ScreenState.MainMenu;
        }

        /// <summary>
        /// 
        /// </summary>
        void _mnuOptions_OnItemSelectedBegin(IUIControl sender)
        {
            this.DisableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnSoundSettings(IUIControl sender)
        {
            this.DisableInput();
            this.State = ScreenState.MusicToggle;
            this._mnuSound.Show();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnScreenSize(IUIControl sender)
        {
            this.DisableInput();
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.OPTIONS_LAST_SELECTED_ITEM, this._itemScreenSize);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.OVERSCAN_CALLER_SCREEN, ScreenType.Options);
            this.NavigateTo(ScreenType.Overscan.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening); 
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnControlsHelp(IUIControl sender)
        {
            this.DisableInput();
            this.State = ScreenState.ControlsHelp;
            this._xboxController.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnLanguage(IUIControl sender)
        {
            this.DisableInput();
            this.State = ScreenState.Language;
            this._mnuLanguage.Show();
        }
        

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnToggleFullscreen(IUIControl sender)
        {
            BrainGame.ToggleFullScreen();
            this.SetToggleScreenModeMenuItemName();
            SnailsGame.ProfilesManager.CurrentProfile.Fullscreen = SnailsGame.GameSettings.IsFullScreen;
            SnailsGame.ProfilesManager.Save();
            this.EnableInput();
        }

        
        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnHowToPlay(IUIControl sender)
        {
            this.DisableInput();
            HowToPlayScreen.PopUp(SnailsGame.Tutorial.Topics);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnCredits(IUIControl sender)
        {
            this.Navigator.GlobalCache.Set("CREDITS_SCREEN_CALLER", ScreenType.Options);
            this.NavigateTo(ScreenType.Credits.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnAwards(IUIControl sender)
        {
            this.NavigateTo(ScreenType.Awards.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetToggleScreenModeMenuItemName()
        {
            if (this._itmToggleFullscreen != null)
            {
                this._itmToggleFullscreen.TextResourceId = (BrainGame.GraphicsManager.IsFullScreen ?
                            "MNU_ITEM_WINDOWED" :
                            "MNU_ITEM_FULLSCREEN");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnPlayerStats(IUIControl sender)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnBackToMain(IUIControl sender)
        {
            this.NavigateToMain();
        }

        void _mnuOptions_OnBackPressed(IUIControl sender)
        {
            this.NavigateToMain();
        }

        /// <summary>
        /// 
        /// </summary>
        private void NavigateToMain()
        {
            SnailsGame.ProfilesManager.Save();
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.INTRO_PICTURE_STATE, this._introPicture.GetSaveState());
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleVisibleMenuHidden);
            this.NavigateTo(ScreenType.MainMenu.ToString(), null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        void _mnuSound_OnHide(IUIControl sender)
        {
            this._mnuOptions.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _mnuSound_OnShow(IUIControl sender)
        {
            this.EnableInput();
        }

        #region Menu language events
        /// <summary>
        /// 
        /// </summary>
        void _mnuLanguage_OnHide(IUIControl sender)
        {
            this._mnuOptions.Show();
        }
      
        /// <summary>
        /// 
        /// </summary>
        void _mnuLanguage_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this._mnuLanguage.SetFocus((int)SnailsGame.CurrentLanguage);
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        void _xboxController_OnDismiss(IUIControl sender)
        {
            this._mnuOptions.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _xboxController_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this._xboxController.Focus();
        }

    
        /// <summary>
        /// 
        /// </summary>
        void _mnuLanguage_OnLanguageSelected(IUIControl sender)
        {
            this.NavigateToMain();
        }

    }
}
