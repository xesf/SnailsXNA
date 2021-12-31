using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.Snails.Player;
//using TwoBrainsGames.BrainEngine.RemoteServices;

namespace TwoBrainsGames.Snails.Screens
{
    class MainMenuScreen : SnailsScreen
    {
        protected enum State
        {
            PressAnyKey,
            MainMenu,
            QuitMenu,
            IntroPicCursorIdle,
            HiddingMenuCursorIdle,
            AssyncronousLoad,
            HowToPlay,
            ShowMainMenu,
            StageSelection,
            ContinueGame
        }

        public enum StartupType
        {
            IntroPicture,
            AllHidden,
            TitleVisibleMenuHidden,
            TitleAndMenuVisible
        }

        #region Consts
        private const double TIME_IDLE_SHOW_INTRO_PIC = 30000;
        private const double TIME_IDLE_SHOW_MENU = 30000;

        private const double BLINK_TIME = 500;

        // Bounding boxes indexes in the menu background sprite
        private const int BB_IDX_PRESS_ANY_KEY = 0;
        private const int BB_IDX_MENU = 1;
        #endregion

        #region Members
        protected StartupType _startType;
        protected State _state;

        protected UIIntroPicture _introPicture;
        protected UICaption _capPressAnyKey;

        protected UIMainMenuBodyPanel _pnlBody;
        protected UISnailsMenu _mnuMain;
        protected UITimer _timerShowMenu;
        protected UISnailsMenuItem _itmNewGame;
        protected UISnailsMenuItem _itmStageSelection;
        protected UISnailsButton _btnPurchase;

#if DEBUG
        protected UISnailsButton _btnStats;
#endif
        #endregion

        #region Properties
        bool ShowHowToPlayOption { get; set; } // If true the menu shows How To Play option
        bool ShowCreditsOption { get; set; } // If true the menu shows the Credits option
        bool ShowPurchaseButton
        {
            get
            {
                return (SnailsGame.IsTrial &&
                        SnailsGame.GameSettings.WithAppStore);
            }
        }
        #endregion

        public MainMenuScreen(ScreenNavigator owner) :
            base(owner, ScreenType.MainMenu)
        {
        }

        ~MainMenuScreen()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();

            // Load theme music
            SnailsGame.ThemeMusic = BrainGame.ResourceManager.GetMusicTemporary(AudioTags.MAIN_MENU_THEME);

            this.Name = "MainMenu";
            this.ShowCreditsOption = false;
            this.ShowHowToPlayOption = true;

            // Screen setup
            this.BackgroundImageBlendColor = Colors.MainMenuScrBkColor;

            // Intro image
            this._introPicture = new UIIntroPicture(this);
            this._introPicture.Name = "_introPicture";
            this._introPicture.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._introPicture.HideEffect = new ColorEffect(new Color(255, 255, 255, 255), new Color(0, 0, 0, 0), 0.05f, false);
            this._introPicture.ShowEffect = new ColorEffect(new Color(0, 0, 0, 0), new Color(255, 255, 255, 255), 0.025f, false);
            //this._introPicture.OnHide += new UIControl.UIEvent(_introPicture_OnHide);
            this._introPicture.ScaleChilds(new Vector2(SnailsGame.GameSettings.RatioNativeResolutionWidth,
                                                        SnailsGame.GameSettings.RatioNativeResolutionHeight));
            this.Controls.Add(this._introPicture);

            // Press any key
            this._capPressAnyKey = new UICaption(this, "", Color.White, UICaption.CaptionStyle.IntroPressAnyKey);
            this._capPressAnyKey.Name = "_capPressAnyKey";
            this._capPressAnyKey.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._capPressAnyKey.Position = new Vector2(0, 8000);
            this._capPressAnyKey.TextResourceId = "LBL_PRESS_ANY_BUTTON";
            this.Controls.Add(this._capPressAnyKey);
            this._introPicture.SendToBack();

            // This one depends on this._capPressAnyKeyBlendColor
            this._capPressAnyKey.Effect = new BlinkEffect(800, 300, this._capPressAnyKey, BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TEXT_BLINK), this._capPressAnyKey.BlendColor);

            // Body - see UIMainMenuBodyPanel for remarks
            this._pnlBody = new UIMainMenuBodyPanel(this);
            this.Controls.Add(this._pnlBody);

            // Main menu
            this._mnuMain = new UISnailsMenu(this);
            this._mnuMain.Name = "_mnuMain";
            this._mnuMain.Size = new BrainEngine.UI.Size(3000.0f, 3800.0f);
            this._mnuMain.Visible = false;
            this._mnuMain.TextResourceId = "MNU_MAIN_MENU";
            this._mnuMain.DefaultItemIndex = 0;
            this._mnuMain.OnMenuShownBegin += new UIControl.UIEvent(MainMenu_OnMenuShownBegin);
            this._mnuMain.OnMenuShown += new UIControl.UIEvent(MainMenu_OnMenuShown);
            this._mnuMain.OnMenuHideBegin += new UIControl.UIEvent(MainMenu_OnMenuHideBegin);
            //this._mnuMain.OnItemSelectedBegin += new UIControl.UIEvent(_mnuMain_OnItemSelectedBegin);
            this._mnuMain.ParentAlignment = AlignModes.HorizontalyVertically;
            this._pnlBody.Controls.Add(this._mnuMain);

            // New game item
            this._itmNewGame = this._mnuMain.AddMenuItem("MNU_ITEM_PLAY", this.MainMenu_OnNewGame, 0, false);
            // Stage selection
            this._itmStageSelection = this._mnuMain.AddMenuItem("MNU_ITEM_STAGE_SELECTION", this.MainMenu_OnStageSelection, 0, false);

            // Credits/ how to play
            this._mnuMain.AddMenuItem("MNU_ITEM_CREDITS", this.MainMenu_OnCredits, 0, false, this.ShowCreditsOption);
            this._mnuMain.AddMenuItem("MNU_ITEM_AWARDS", this.OptionsMenu_OnAchievements, 0, false, true);

            // Options
            this._mnuMain.AddMenuItem("MNU_ITEM_OPTIONS", this.MainMenu_OnOptions, 0);
            if (SnailsGame.GameSettings.ShowQuitOptions)
            {
                this._mnuMain.AddMenuItem("MNU_ITEM_QUIT", this.MainMenu_OnQuit, InputBase.InputActions.Back, true);
            }
#if !WIN8
            else
            {
                this.OnBack += this.MenuConfirm_OnYes;
            }
#endif


            // Purchase button
            this._btnPurchase = new UISnailsButton(this, "BTN_PURCHASE", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnPurchase_OnClick, true);
            this._btnPurchase.Name = "_btnPurchase";
            this._btnPurchase.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
            this.Controls.Add(this._btnPurchase);

#if DEBUG
            // Player stats
            this._btnStats = new UISnailsButton(this, "BTN_PLAYER_STATS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnStats_OnClick, true);
            this._btnStats.ParentAlignment = AlignModes.Bottom | AlignModes.Right;
            this.Controls.Add(this._btnStats);
#endif 
            // Timer used to show the menu
            this._timerShowMenu = new UITimer(this, 750, false);
            this._timerShowMenu.OnTimer += this.TimerShowMenu_OnTimer;
            this.Controls.Add(this._timerShowMenu);

            this.OnOpenTransitionEnded += new UIControl.UIEvent(MainMenuScreen_OnOpenTransitionEnded);
            this.OnPopupClosed += new UIControl.UIEvent(MainMenuScreen_OnPopupClosed);
            this.OnGameplayModeChanged += new UIControl.UIEvent(MainMenuScreen_OnGameplayModeChanged);
            this.ShowTrialTag = true;
            this.ShowRateTag = true;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            if (SnailsGame.ThemeMusic != null)
            {
                SnailsGame.ThemeMusic.Play(true);
            }
            
            this._startType = this.Navigator.GlobalCache.Get<StartupType>(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, StartupType.IntroPicture);

            switch (this._startType)
            {
                case StartupType.AllHidden:
                    // Menu
                    this._mnuMain.Visible = false;
                    this._timerShowMenu.Reset();
                    this._timerShowMenu.Enabled = true;

                    // Intro picture
                    this._introPicture.ResetBackgroundAlpha();
                    this._capPressAnyKey.Visible = false;

                    this._state = State.MainMenu;
                    this._btnPurchase.Visible = false;
#if DEBUG
                    this._btnStats.Visible = false;
#endif
                    break;

                case StartupType.TitleVisibleMenuHidden:
                    this._btnPurchase.Visible = false;

                    if (this.ShowPurchaseButton)
                    {
                        this._btnPurchase.Show();
                    }
#if DEBUG
                    this._btnStats.Show();
#endif
                    this._mnuMain.Show();
                    this._capPressAnyKey.Visible = false;
                    this._state = State.MainMenu;
                    break;

                case StartupType.TitleAndMenuVisible:
                    this._mnuMain.ShowWithoutEffects();
                    this._introPicture.SetBackgroundAlpha();
                    this._capPressAnyKey.Visible = false;
                    this._state = State.MainMenu;
                    this._btnPurchase.Visible = this.ShowPurchaseButton;
#if DEBUG
                    this._btnStats.Visible = true;
#endif
                    this._mnuMain.AcceptControllerInput = true;
                    break;

                // Screen starts with the intro picture
                // Hide the menu, the game title and the instruction bar
                // Show the press any key message and the snails lettering
                case StartupType.IntroPicture:
                    this._introPicture.ResetBackgroundAlpha();
                    this._mnuMain.Visible = false;
                    this._btnPurchase.Visible = false;
#if DEBUG
                    this._btnStats.Visible = false;
#endif
                    this._state = State.PressAnyKey;
                    this._timerShowMenu.Enabled = false;
                    this.InstructionBar.Visible = false;
                    break;
            }

            this._introPicture.Initialize();
            this._introPicture.SetSaveState(this.Navigator.GlobalCache.Get<TwoBrainsGames.Snails.Screens.CommonControls.UIIntroPicture.IntroPictureSaveState>(GlobalCacheKeys.INTRO_PICTURE_STATE));

            this.InputController.ResetTimeIdle();

            // Configure main menu
            if (SnailsGame.ProfilesManager.CurrentProfile != null)
            {
                this._itmNewGame.TextResourceId = (SnailsGame.ProfilesManager.CurrentProfile.HasStartedNewGame ? "MNU_ITEM_CONTINUE" : "MNU_ITEM_PLAY");
                this._itmStageSelection.Enabled = SnailsGame.ProfilesManager.CurrentProfile.HasStartedNewGame;
            }

            this.EnableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // This quits the game when pressing back in WP7 when in the intro picture
            if (SnailsGame.GameSettings.BackQuitsGameOnIntroPicture &&
                this.InputController.ActionBack && this._introPicture.Visible)
            {
                this.QuitGame();
                return;
            }

            switch (this._state)
            {
                case State.PressAnyKey:
                    if (!SnailsGame.IsTrial)
                    {
                        SnailsGame.AchievementsManager.Notify((int)AchievementsType.PurchaseTheGame);
                    }

                    if (this.InputController.CheckActionStartPressed())
                    {
#if XBOX
                        // signed in player
                        SnailsGame.ProfilesManager.SignedInPlayer();
                        SnailsGame.ProfilesManager.Load(); // this is assyncronous
#endif
                        this._state = State.AssyncronousLoad;
                    }
                    break;
                case State.AssyncronousLoad:
                    if (SnailsGame.GameSettings.AsyncProfileLoading == false)
                    {
                        this._state = State.ShowMainMenu;
                    }
                    else
                    {
                        // if load is complete, than continue to show the Main Menu
                        if (SnailsGame.ProfilesManager.IsCompleted)
                        {
                            this._itmNewGame.TextResourceId = (SnailsGame.ProfilesManager.CurrentProfile.HasStartedNewGame ? "MNU_ITEM_CONTINUE" : "MNU_ITEM_PLAY");
                            this._itmStageSelection.Enabled = SnailsGame.ProfilesManager.CurrentProfile.HasStartedNewGame;

                            if (!SnailsGame.ProfilesManager.CurrentProfile.OverscanSet &&
                                SnailsGame.GameSettings.AllowOverscanAdjustment)
                            {
                                this.Navigator.GlobalCache.Set(GlobalCacheKeys.OVERSCAN_CALLER_SCREEN, ScreenType.InGameOptions);
                                this.NavigateTo(ScreenType.Overscan.ToString(), ScreenTransitions.FadeOut, ScreenTransitions.FadeIn);
                            }
                            else
                            {
                                this._state = State.ShowMainMenu;
                            }
                        }
                    }
                    break;

                case State.ShowMainMenu:
                    this._introPicture.FadeInBackground();
                    this._capPressAnyKey.Visible = false;
                    this.InputController.ResetTimeIdle(); 
                    this._state = State.MainMenu;
                   // this._timerShowMenu.Reset();
                    //this._timerShowMenu.Enabled = true;
                    this._mnuMain.Show();
                    break;

                case State.MainMenu:
                    if (this.InputController.TimeIdleMsecs > TIME_IDLE_SHOW_INTRO_PIC)
                    {
                        this._introPicture.FadeOutBackground();
                        this._mnuMain.Hide();
                        this.InputController.ResetTimeIdle();
                        this._state = State.IntroPicCursorIdle;
                    }
                    break;

                case State.IntroPicCursorIdle:
                    if (this.InputController.TimeIdleMsecs > TIME_IDLE_SHOW_MENU ||
                        this.InputController.CheckActionStartPressed())
                    {
                        this._state = State.ShowMainMenu;
                    }
                    if (!this._capPressAnyKey.Visible)
                    {
                        this._capPressAnyKey.Show();
                    }
                    break;

                case State.ContinueGame:
                    this.ContinueGame();
                    break;
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        void MainMenuScreen_OnGameplayModeChanged(IUIControl sender)
        {
            if (!SnailsGame.IsTrial)
            {
                this._btnPurchase.Visible = false;
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.PurchaseTheGame);
            }
        }

        /// <summary>
        /// This is needed because if the screen starts with the menu already open,
        /// we have to set EnableInput when the screen transition open
        /// </summary>
        void MainMenuScreen_OnOpenTransitionEnded(IUIControl sender)
        {
            if (this._startType == StartupType.TitleAndMenuVisible)
            {
                this.EnableInput();
                this._mnuMain.SetFocusOnLastSelectedItem();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void MainMenuScreen_OnPopupClosed(IUIControl sender)
        {
            this.EnableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        void MainMenu_OnMenuShownBegin(IUIControl sender)
        {
            if (this.ShowPurchaseButton)
            {
                this._btnPurchase.Show();
            }
    #if DEBUG
            this._btnStats.Show();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainMenu_OnMenuShown(IUIControl sender)
        {
            this.EnableInput();
            this._mnuMain.SetFocusOnLastSelectedItem();
  
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainMenu_OnMenuHideBegin(IUIControl sender)
        {
            this._btnPurchase.Hide();
#if DEBUG
            this._btnStats.Hide();
#endif
        }


        /// <summary>
        /// 
        /// </summary>
        private void ContinueGame()
        {
            if (SnailsGame.ProfilesManager.CurrentProfile.HasStartedNewGame)
            {
                if (SnailsGame.ThemeMusic != null && SnailsGame.ThemeMusic.IsPlaying)
                {
                    BrainGame.MusicManager.FadeMusic(0, AudioTags.MUSIC_FADE_MSECONDS);
                }

                this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, true);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.SELECTED_STAGE_INFO, Levels.CurrentLevel.GetCurrentStageInfo());
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, false);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.MainMenu);
                this.NavigateTo(ScreenGroupType.InGame.ToString(), ScreenType.StageStart.ToString(), ScreenTransitions.LeafsClosing, null); // Don't use open transition 
            }
            else
            {
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, false);
                this.NavigateTo(ScreenType.ThemeSelection.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainMenu_OnNewGame(IUIControl sender)
        {
            if (SnailsGame.GameSettings.UseLeaderboards)
            {
                if (SnailsGame.ProfilesManager.CurrentProfile.WithRemoteServicesUser)
                {
                    this.ShowLogin();
                    return;
                }
            }

            this.ContinueGame();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainMenu_OnStageSelection(IUIControl sender)
        {
            if (SnailsGame.GameSettings.UseLeaderboards)
            {
                if (!SnailsGame.ProfilesManager.CurrentProfile.WithRemoteServicesUser)
                {
                    this.ShowLogin();
                    return;
                }
            }

            this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, false);
            this.NavigateTo(ScreenType.ThemeSelection.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainMenu_OnOptions(IUIControl sender)
        {
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.INTRO_PICTURE_STATE, this._introPicture.GetSaveState());
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.OPTIONS_STARTUP_MODE, OptionsScreen.StartupType.MenuHidden);
            this.NavigateTo(ScreenType.Options.ToString(), null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void MainMenu_OnCredits(IUIControl sender)
        {
            this.Navigator.GlobalCache.Set("CREDITS_SCREEN_CALLER", ScreenType.MainMenu);
            this.NavigateTo(ScreenType.Credits.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainMenu_OnQuit(IUIControl sender)
        {
            this.QuitGame();
        }

        /// <summary>
        /// 
        /// </summary>
        private void TimerShowMenu_OnTimer(IUIControl sender)
        {
            this._mnuMain.Show();
        }


        /// <summary>
        /// 
        /// </summary>
        private void QuitGame()
        {
            this.NavigateTo(ScreenType.Quit.ToString(), ScreenTransitions.FadeOut, null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuConfirm_OnYes(IUIControl sender)
        {
            this.QuitGame();
        }


        /// <summary>
        /// 
        /// </summary>
        void btnPurchase_OnClick(IUIControl sender)
        {
            SnailsGame.Instance.PurchaseGame();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnAchievements(IUIControl sender)
        {
            this.NavigateTo(ScreenType.Awards.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }



        /// <summary>
        /// 
        /// </summary>
        private void ShowLogin()
        {
            SnailsGame.ScreenNavigator.PopUp(ScreenType.Login.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /*private void LoginCallBack(RemoteAPICallResult result)
        {
            if (result.WithError == false)
            {
                this._state = State.ContinueGame;
            }
        }*/

#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        void btnStats_OnClick(IUIControl sender)
        {
            this.NavigateTo("MainMenu", ScreenType.PlayerStats.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }
#endif
    }
}
