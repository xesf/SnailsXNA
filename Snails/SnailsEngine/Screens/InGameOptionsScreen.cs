using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.Snails.Screens.Transitions;

namespace TwoBrainsGames.Snails.Screens
{
    class InGameOptionsScreen : SnailsScreen
	{
        enum ScreenState
        {
            Idle,
            ReturnToGame,
            BackToMenu,
            Options,
            BackToStageSelConfirmation,
            RestartStageConfirmation,
            SoundToggle,
            BackMainMenuConfirmation
        }

        ScreenState _state;
        UISnailsBoard _board;
        UISnailsTitle _gameTitle;
        UISnailsMenu _mnuMain;
        UISnailsMenu _mnuConfirm;
        UISnailsMenu _mnuOptions;
        UISoundMenu _mnuSoundSettings;
        UISnailsMenuItem _itmResumeGame;
        UISnailsMenuItem _itmToggleFullscreen;
        UIGoldMedalInfoPanel _goldMedalPanel;
        UICloseButton _btnClose;
        UIPanel _pnlMenu;
        bool _musicActiveWhenOpened;

        public InGameOptionsScreen(ScreenNavigator owner) :
            base(owner, ScreenType.InGameOptions)
        { }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.BackgroundImage = null;
            this.Name = "";

            // Board
            this._board = new UISnailsBoard(this, UISnailsBoard.BoardType.LeafsMedium);
            this._board.Name = "_board";
			this._board.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._board.OnShow += new UIControl.UIEvent(_board_OnShow);
            this._board.OnHide += new UIControl.UIEvent(_board_OnHide);
            this._board.OnHideBegin += new UIControl.UIEvent(_board_OnHideBegin);
            //this._board.ImagePosition = this.PixelsToScreenUnits (new Vector2(0, 60));
            this._board.Position = this.PixelsToScreenUnits(new Vector2(0, 60));
            this.Controls.Add(this._board);

            // Main menu panel (used to center the menu), 
            this._pnlMenu = new UIPanel(this);
            this._pnlMenu.Name = "_pnlMenu";
			this._pnlMenu.Size = this.PixelsToScreenUnits(new BrainEngine.UI.Size(400.0f, 350.0f));
			this._pnlMenu.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
			this._pnlMenu.Position = this.PixelsToScreenUnits(new Vector2(0.0f, 120.0f));
			this._board.Controls.Add(this._pnlMenu);

            // Main menu
            this._mnuMain = new UISnailsMenu(this);
			this._mnuMain.Size = this.PixelsToScreenUnits(new BrainEngine.UI.Size(300.0f, 380.0f));
            this._mnuMain.TextResourceId = "MNU_GAME_PAUSED";
            this._mnuMain.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._mnuMain.OnMenuShown += new UIControl.UIEvent(_mnuMain_OnMenuShown);
            this._mnuMain.OnItemSelectedBegin += new UIControl.UIEvent(_mnuMain_OnItemSelectedBegin);
			this._pnlMenu.Controls.Add(this._mnuMain);

            // Main menu items
            if (SnailsGame.GameSettings.ShowContinueOption)
            {
                this._itmResumeGame = this._mnuMain.AddMenuItem("MNU_ITEM_RESUME_GAME", this.Menu_OnReturnToGame, InputBase.InputActions.Back, false);
            }

			this._mnuMain.AddMenuItem("MNU_ITEM_AWARDS", this.Menu_OnAchievements, 0, false, true);
			this._mnuMain.AddMenuItem("MNU_ITEM_OPTIONS", this.Menu_OnOptions, 0);
            this._mnuMain.AddMenuItem("MNU_ITEM_QUIT_STAGE", this.Menu_OnQuitStage, 0, SnailsGame.GameSettings.ShowConfirmationMenus);
 
            // Options menu
            this._mnuOptions = new UISnailsMenu(this);
            this._mnuOptions.Name = "_mnuOptions";
            this._mnuOptions.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
			this._mnuOptions.Size = this.PixelsToScreenUnits(new BrainEngine.UI.Size(300.0f, 380.0f));
            this._mnuOptions.TextResourceId = "MNU_ITEM_OPTIONS";
            this._mnuOptions.DefaultItemIndex = 0;
            this._mnuOptions.ItemSize = UISnailsMenu.MenuItemSize.Medium;
            this._mnuOptions.OnMenuShown += new UIControl.UIEvent(_mnuOptions_OnMenuShown);
            this._mnuOptions.OnItemSelectedBegin += new UIControl.UIEvent(_mnuOptions_OnItemSelectedBegin);
            this._mnuOptions.OnBackPressed += new UIControl.UIEvent(OptionsMenu_OnBackToMain);
            this._mnuOptions.WithBackButton = true;
            this._pnlMenu.Controls.Add(this._mnuOptions);

            // Menu items
            this._mnuOptions.AddMenuItem("MNU_ITEM_SOUND_SETTINGS", this.OptionsMenu_OnSoundSettings, 0);
            if (SnailsGame.GameSettings.AllowToggleFullScreen)
            {
                this._itmToggleFullscreen = this._mnuOptions.AddMenuItem("", this.OptionsMenu_OnToggleFullscreen, 0, false);
            }
            if (SnailsGame.GameSettings.UseGamepad)
            {
                this._mnuOptions.AddMenuItem("MNU_ITEM_CONTROLS", this.OptionsMenu_OnControlsHelp, 0, false);
            }
            this._mnuOptions.AddMenuItem("MNU_ITEM_HOW_TO_PLAY", this.OptionsMenu_OnHowToPlay, 0, false);

            // Game title
            this._gameTitle = new UISnailsTitle(this);
			this._gameTitle.Position = this.PixelsToScreenUnits(new Vector2(0, -60.0f));
            this._gameTitle.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._gameTitle.Mode = UISnailsTitle.TitleMode.Log;
			this._board.Controls.Add(this._gameTitle);
           
            // Sound menu
            this._mnuSoundSettings = new UISoundMenu(this);
            this._mnuSoundSettings.Name = "_mnuSoundSettings";
            this._mnuSoundSettings.OnHide += new UIControl.UIEvent(_mnuSoundSettings_OnHide);
            this._mnuSoundSettings.OnShow += new UIControl.UIEvent(_mnuSoundSettings_OnShow);
            this._mnuSoundSettings.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._mnuSoundSettings.StopPlayMusic = true;
            this._pnlMenu.Controls.Add(this._mnuSoundSettings);

            // Stage gold medal info
            this._goldMedalPanel = new UIGoldMedalInfoPanel(this);
            this._goldMedalPanel.Name = "_goldMedalPanel";
            this._goldMedalPanel.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly | BrainEngine.UI.AlignModes.Bottom;
			this._goldMedalPanel.Margins.Bottom = this.PixelsToScreenUnitsY (-40);
			this._board.Controls.Add(this._goldMedalPanel);
            
            // Close button
            this._btnClose = new UICloseButton(this);
            this._btnClose.ParentAlignment = BrainEngine.UI.AlignModes.Right | BrainEngine.UI.AlignModes.Top;
            this._btnClose.OnPress += new UIControl.UIEvent(_btnClose_OnPress);
			this._btnClose.Margins.Top = this.PixelsToScreenUnitsY(-30);
			this._btnClose.Margins.Right = this.PixelsToScreenUnitsX(0);
            this._btnClose.FaceType = UICloseButton.ButtonFaceType.Light;
            this._board.Controls.Add(this._btnClose); // This button is ugly if the title is displayed in the menu

            this.OnPopupClosed += new UIControl.UIEvent(InGameOptionsScreen_OnPopupClosed);
            this.OnBlurEffectFadeEnded += new EventHandler(InGameOptionsScreen_OnBlurEffectFadeEnded);
            this.OnBlurEffectEnded += new EventHandler(InGameOptionsScreen_OnBlurEffectEnded);
            this.WithBlurEffect = true;
        }

        #region Screen events
        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            SnailsGame.ProfilesManager.Save();
            this.DisableInput();
            this._gameTitle.Visible = false;
            this._board.Visible = false;
            this._mnuMain.Visible = false;
            this._mnuOptions.Visible = false;
            this._state = ScreenState.Idle;
            this.SetToggleScreenModeMenuItemName();
            BrainGame.GameCursor.SetCursor(GameCursors.Default);
            this._mnuSoundSettings.Visible = false;
            this._goldMedalPanel.Visible = false;
            this._gameTitle.WithShake = false;
            this._musicActiveWhenOpened = BrainGame.MusicManager.IsMusicActive;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
            base.OnUpdate(gameTime);
          
            switch (this._state)
            {
                case ScreenState.SoundToggle:
                    if (SnailsGame.GameSettings.UseGamepad)
                    {
                        if (this.InputController.ActionBack ||
                            this.InputController.ActionAccept)
                        {
                            this.DisableInput();
                        }
                    }
                    else
                    if (this.InputController.ActionBack)
                    {
                        this.DisableInput();
                    }           
                    break;
            }
          
        }

        /// <summary>
        /// 
        /// </summary>
        void InGameOptionsScreen_OnPopupClosed(IUIControl sender)
        {
            this.EnableInput();
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        void InGameOptionsScreen_OnBlurEffectFadeEnded(object sender, EventArgs e)
        {
            this.Close();
            Stage.CurrentStage.ResumeGame();
        }

        /// <summary>
        /// 
        /// </summary>

        void InGameOptionsScreen_OnBlurEffectEnded(object sender, EventArgs e)
        {
            this._gameTitle.Show();
            this._board.Show();
        }

        #region Board events
        /// <summary>
        /// 
        /// </summary>
        void _board_OnShow(IUIControl sender)
        {
            this._state = ScreenState.Idle;
            this._mnuMain.Show();
            this._goldMedalPanel.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _board_OnHide(IUIControl sender)
        {
            this.FadeBlurOut();
        }

        /// <summary>
        /// 
        /// </summary>
        void _board_OnHideBegin(IUIControl sender)
        {
            this._goldMedalPanel.Hide();
        }


        #endregion

        #region Main menu events
        /// <summary>
        /// 
        /// </summary>
        void _mnuMain_OnMenuShown(IUIControl sender)
        {
            this.EnableInput();
            if (this.CursorMode == BrainEngine.UI.CursorModes.SnapToControl)
            {
                this._itmResumeGame.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _mnuMain_OnItemSelectedBegin(IUIControl sender)
        {
            this.DisableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Menu_OnReturnToGame(IUIControl sender)
        {
            this.ReturnToGame();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Menu_OnOptions(IUIControl sender)
        {
            this.DisableInput();
            this._mnuOptions.Show();
        }

		/// <summary>
		/// 
		/// </summary>
		private void Menu_OnAchievements(IUIControl sender)
		{
#if GAMESERVICE
            Guide.ShowAchievements ();
#endif
			this.EnableInput();
		}

        /// <summary>
        /// 
        /// </summary>
        private void Menu_OnMainMenu(IUIControl sender)
        {
            this.DisableInput();

            this.QuitToMainMenu();

        }

        /// <summary>
        /// 
        /// </summary>
        private void Menu_OnQuitStage(IUIControl sender)
        {
            this.DisableInput();
			this.ReturnToStageSelection();

        }

        /// <summary>
        /// 
        /// </summary>
        private void Menu_OnRestartStage(IUIControl sender)
        {
		     this.RestartStage();
        }
#endregion

#region Options menu
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
        private void _mnuOptions_OnMenuShown(IUIControl sender)
        {
            this.EnableInput();
            this._state = ScreenState.Idle;

            if (this.CursorMode == BrainEngine.UI.CursorModes.SnapToControl)
            {
                this._mnuOptions.SetFocus(0);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnSoundSettings(IUIControl sender)
        {
            this.DisableInput();
            this._state = ScreenState.SoundToggle;
            this._mnuSoundSettings.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnControlsHelp(IUIControl sender)
        {
            this.Navigator.PopUp(ScreenType.XBoxControllerHelp.ToString());
            this.EnableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnHowToPlay(IUIControl sender)
        {
            HowToPlayScreen.PopUp(SnailsGame.Tutorial.Topics);
            this.EnableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OptionsMenu_OnBackToMain(IUIControl sender)
        {

            this.DisableInput();
            this._mnuMain.Show();
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
        private void SetToggleScreenModeMenuItemName()
        {
            if (this._itmToggleFullscreen != null)
            {
                this._itmToggleFullscreen.TextResourceId = (BrainGame.GraphicsManager.IsFullScreen ?
                            "MNU_ITEM_WINDOWED" :
                            "MNU_ITEM_FULLSCREEN");
            }
        }

#endregion

#region Confirm menu events

        /// <summary>
        /// 
        /// </summary>
        private void QuitToMainMenu()
        {
            BrainGame.ResourceManager.Unload(ResourceManagerIds.TUTORIAL_RESOURCES);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleAndMenuVisible);
            this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReturnToStageSelection()
        {
            Stage.CurrentStage.QuitStage();
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, true);
            this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RestartStage()
        {
            // This flag disbles the stage briefing board. Nothing will be displayed, but
            // it's required because the stage load is there
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, false);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, false);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.InGameOptions);
            this.NavigateTo(ScreenType.StageStart.ToString(), ScreenTransitions.LeafsClosing, null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuConfirm_OnYes(IUIControl sender)
        {
            switch (this._state)
            {
                case ScreenState.BackMainMenuConfirmation:
                    this.QuitToMainMenu();
                    break;

                case ScreenState.BackToStageSelConfirmation:
                    this.ReturnToStageSelection();
                    break;

                case ScreenState.RestartStageConfirmation:
                    this.RestartStage();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuConfirm_OnNo(IUIControl sender)
        {
            this.DisableInput();
            this._mnuMain.Show();
        }
#endregion

        /// <summary>
        /// 
        /// </summary>
        void _mnuSoundSettings_OnHide(IUIControl sender)
        {
            this._mnuOptions.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _mnuSoundSettings_OnShow(IUIControl sender)
        {
            this.EnableInput();
        }

        void _btnClose_OnPress(IUIControl sender)
        {
            this.ReturnToGame();
        }


        /// <summary>
        /// 
        /// </summary>
        private void ReturnToGame()
        {
            if (this._musicActiveWhenOpened == false)
            {
                BrainGame.MusicManager.StopMusic();
            }
            else
            {
                BrainGame.MusicManager.ResumeMusic();
            }
            this.DisableInput();
            this._board.Hide();
            this._gameTitle.Hide();
        }
        
    }
}
