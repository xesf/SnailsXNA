using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Screens
{
	class StageStartScreen : SnailsScreen
	{
        enum ScreenState
        {
            None,
            GoBack,
            StartGame,
            MainMenu
        }

        #region Vars
        protected UISnailsMenuTitle _title;
        protected UISnailsBoard _board;
        protected UISnailsStageGoalIcon _goalIcon;
        protected UISnailsThemeIcon _themeIcon;
        protected UIValuedCaption _capStageNr;
        protected UIValuedCaption _capMode;
        protected UIValuedCaption _capTotalSnails;
        protected UIValuedCaption _capTime;
        protected UIValuedCaption _capToDeliver;
        protected UISnailsButton _btnStageSelection;
        protected UISnailsButton _btnStart;
        protected UISnailsButton _btnMainMenu;
        protected UISnailsButton _btnLoadSolution;
        protected UISnailsButton _btnStartAndSaveSolution;
        UIXBoxControls _xboxController;
        UILoadingTip _loadHintMessage { get; set; }

        // Use this to store label 4 & 5 positions
        // This is needed because depending on the stage mode, some labels might not be visible
        Vector2 _label4Pos;
        Vector2 _label5Pos;
        private bool _loadStarted;
        private bool _canStartLoading;
        UIImage _imgFreeSnailsAd { get; set; }
        #endregion

        #region Properties
        ScreenState State { get; set; }
        LevelStage LevelStageInfo { get; set; }
        private float LineSpacing { get; set; }
        private bool ShowStageBriefing
        {
            get
            {
                return this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false);
            }

        }
        private bool ShowXBoxHelp
        {
            get
            {
                return this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, false);
            }

        }

        bool TipMessageVisible
        {
            get
            {
                return (this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.SHOW_TIP_ON_LOADING, false) &&
                        this._loadHintMessage.AvailableForCurrentLocale);
            }
        }


        private bool ShouldShowRateDialog
        {
            get
            {
                if (SnailsGame.GameSettings.AllowRate == false)
                {
                    return false;
                }
                if (SnailsGame.ProfilesManager.CurrentProfile.GameWasRated)
                {
                    return false;
                }

                if (Levels.CurrentTheme == ThemeType.ThemeA &&
                    Levels.CurrentStageNr < 5)
                {
                    return false;
                }

                ScreenType screenCaller = this.Navigator.GlobalCache.Get<ScreenType>(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.None);
                if (screenCaller != ScreenType.MainMenu &&
                    screenCaller != ScreenType.ThemeSelection)
                {
                    // Show 1 out of 3 times
                    if (BrainGame.Rand.Next(3) == 1)
                    {
                        return true;
                    }
                    return false;
                }

                // Only if network is available
                /*if (TwoBrainsGames.BrainEngine.RemoteServices.Network.IsInternetAvailable == false)
                {
                    return false;
                }*/

                return true;
            }
        }
        #endregion

        public StageStartScreen(ScreenNavigator owner) :
            base(owner, ScreenType.StageStart)
        { 
        
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.BackgroundImage = null;
            this.Name = "StageStart";
            this.OnInitializeFromContent += new UIControl.UIEvent(StageStartScreen_OnInitializeFromContent);
            this.OnAfterInitializeFromContent += new UIControl.UIEvent(StageStartScreen_OnAfterInitializeFromContent);
            this.BackgroundType = ScreenBackgroundType.Leafs;
           
            // Board
            this._board = new UISnailsBoard(this, UISnailsBoard.BoardType.LightWoodMediumLong);
            this._board.Name = "_board";
            this._board.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._board.Position = new Vector2(0.0f, 3000.0f);
            this._board.OnHide += new BrainEngine.UI.Controls.UIControl.UIEvent(_board_OnHide);
            this._board.Size = new Size(this._board.Size.Width, this._board.Size.Height + 1500f); // The buttons have to be inside or else Action will not work when we're in control snap cursor mode
            this.Controls.Add(this._board);

            // Title - Stage briefing
            this._title = new UISnailsMenuTitle(this);
            this._title.Name = "_title";
            this._title.TextResourceId = "TITLE_STAGE_BRIEFING";
            this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
            this._board.Controls.Add(this._title);

            // Theme icon
            this._themeIcon = new UISnailsThemeIcon(this);
            this._themeIcon.Name = "_themeIcon";
            this._board.Controls.Add(this._themeIcon);

            // Stage Nr
            this._capStageNr = new UIValuedCaption(this, "LBL_BRIEFING_STAGE", 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0, false);
            this._capStageNr.Name = "_capStageNr";
            this._capStageNr.Mode = UIValuedCaption.CaptionMode.Simple;
            this._capStageNr.AnimateValue = false;
            this._board.Controls.Add(this._capStageNr);

            // Mode
            this._capMode = new UIValuedCaption(this, "LBL_BRIEFING_MODE", 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0, false);
            this._capMode.Mode = UIValuedCaption.CaptionMode.Simple;
            this._capMode.AnimateValue = false;
            this._board.Controls.Add(this._capMode);

            // Goal icon
            this._goalIcon = new UISnailsStageGoalIcon(this);
            this._goalIcon.IconSize = UISnailsStageGoalIcon.GoalIconSize.Small;
            this._board.Controls.Add(this._goalIcon);

            // Total snails
            this._capTotalSnails = new UIValuedCaption(this, "LBL_BRIEFING_TOTAL_SNAILS", 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0, false);
            this._capTotalSnails.Mode = UIValuedCaption.CaptionMode.Simple;
            this._capTotalSnails.AnimateValue = false;
            this._board.Controls.Add(this._capTotalSnails);

            // To deliver
            this._capToDeliver = new UIValuedCaption(this, "LBL_BRIEFING_TO_DELIVER", 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0, false);
            this._capToDeliver.Mode = UIValuedCaption.CaptionMode.Simple;
            this._capToDeliver.AnimateValue = false;
            this._board.Controls.Add(this._capToDeliver);

            // Time
            this._capTime = new UIValuedCaption(this, "LBL_BRIEFING_REF_TIME", 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0, false);
            this._capTime.Mode = UIValuedCaption.CaptionMode.Simple;
            this._capTime.AnimateValue = false;
            this._board.Controls.Add(this._capTime);


            // Screen events
//            this.OnBeforeControlsDraw += new UIControl.UIEvent(StageStartScreen_OnBeforeControlsDraw);
            this._asyncLoad.OnAsyncOperationEnded += new UIControl.UIEvent(_asyncLoad_OnAsyncOperationEnded);

            // Button start
            this._btnStart = new UISnailsButton(this, "BNT_START", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnStart_OnClick, false);
            this._btnStart.Name = "_btnStart";
            this._btnStart.ParentAlignment = AlignModes.Bottom;
            this._btnStart.OnShow += new UIControl.UIEvent(btnStart_OnShow);
            this._btnStart.ButtonAction = UISnailsButton.ButtonActionType.Start;
            this._board.Controls.Add(this._btnStart);

            // Button stage selection
            this._btnStageSelection = new UISnailsButton(this, "BTN_STAGE_SELECTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, this.btnStageSelection_OnBack, false);
            this._btnStageSelection.Name = "_btnStageSelection";
            this._btnStageSelection.ParentAlignment = AlignModes.Bottom;
            this._btnStageSelection.ButtonAction = UISnailsButton.ButtonActionType.Back;
            this._board.Controls.Add(this._btnStageSelection);

            // Button Main menu
            this._btnMainMenu = new UISnailsButton(this, "BTN_MAIN_MENU", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnMainMenu_OnClick, false);
            this._btnMainMenu.Name = "_btnMainMenu";
            this._btnMainMenu.ParentAlignment = AlignModes.Bottom;
            this._btnMainMenu.ButtonAction = UISnailsButton.ButtonActionType.MainMenu;
            this._board.Controls.Add(this._btnMainMenu);
           
            // Button "Load solution"
            this._btnLoadSolution = new UISnailsButton(this, "BTN_LOAD_SOLUTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnLoadSolution_OnClick, false);
            this._btnLoadSolution.Name = "_btnLoadSolution";
            this._btnLoadSolution.ParentAlignment = AlignModes.Bottom;
#if GAMEPLAY_RECORDING_SUPPORT
            this.Controls.Add(this._btnLoadSolution);
#endif

            // Button "Start & Record solution"
            this._btnStartAndSaveSolution = new UISnailsButton(this, "BTN_START_AND_SAVE_SOLUTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnStartAndSaveSolution_OnClick, false);
            this._btnStartAndSaveSolution.Name = "_btnStartAndSaveSolution";
            this._btnStartAndSaveSolution.ParentAlignment = AlignModes.Bottom;
#if GAMEPLAY_RECORDING_SUPPORT
            this.Controls.Add(this._btnStartAndSaveSolution);
#endif

            //
            this._loadHintMessage = new UILoadingTip(this);
            this.Controls.Add(this._loadHintMessage);


            // Controller help
            if (SnailsGame.Settings.Platform == BrainSettings.PlaformType.XBox)
            {
                this._xboxController = new UIXBoxControls(this);
                this._xboxController.Position = new Vector2(0f, 3200f);
                this._xboxController.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
                this._xboxController.OnDismiss += new UIControl.UIEvent(_xboxController_OnDismiss);
                this._xboxController.OnDismissButtonShown += new UIControl.UIEvent(_xboxController_OnDismissButtonShown);
                this._xboxController.ButtonCaptionType = UIXBoxControls.ButtonType.Start;
                this.Controls.Add(this._xboxController);
            }

            this._imgFreeSnailsAd = new UIImage(this);
            this._imgFreeSnailsAd.Position = new Vector2(100f, 100f);
            this._imgFreeSnailsAd.OnAccept += new UIControl.UIEvent(_imgFreeSnailsAd_OnAccept);
            this._imgFreeSnailsAd.AcceptControllerInput = true;
            this.Controls.Add(this._imgFreeSnailsAd);

            this.OnPopupClosed += new UIControl.UIEvent(StageStartScreen_OnPopupClosed);
        }

        /// <summary>
        /// 
        /// </summary>
        void StageStartScreen_OnPopupClosed(IUIControl sender)
        {
            this._canStartLoading = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void StageStartScreen_OnInitializeFromContent(IUIControl sender)
        {
            this.LineSpacing = this.GetContentPropertyValue<float>("lineSpacing", this.LineSpacing);

        }

        /// <summary>
        /// 
        /// </summary>
        void StageStartScreen_OnAfterInitializeFromContent(IUIControl sender)
        {
            this._capStageNr.UpdateLayout();
            this._capMode.Position = this._capStageNr.Position + new Vector2(0.0f, this.LineSpacing);
            this._capTotalSnails.Position = this._capMode.Position + new Vector2(0.0f, this.LineSpacing);
            this._capToDeliver.Position = this._capTotalSnails.Position + new Vector2(0.0f, this.LineSpacing);
            this._capTime.Position = this._capToDeliver.Position + new Vector2(0.0f, this.LineSpacing);
            this._label4Pos = this._capToDeliver.Position;
            this._label5Pos = this._capTime.Position;

            this._capMode.Width = this._capStageNr.Width;
            this._capTotalSnails.Width = this._capStageNr.Width;
            this._capToDeliver.Width = this._capStageNr.Width;
            this._capTime.Width = this._capStageNr.Width;

            this._capMode.UpdateLayout();
            this._goalIcon.Position = new Vector2(this._capMode.Position.X + this._capMode.Width + 100f, this._capMode.ValuePosition.Y - 100f);
        }


        /// <summary>
        /// 
        /// </summary>
        void btnStart_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this.InstructionBar.HideAllLabels();
            this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Start);
            this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Back);
            this._btnStart.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            // No internet? Check it again. This will allow the rate board to show if the internet is available
            /*if (!TwoBrainsGames.BrainEngine.RemoteServices.Network.IsInternetAvailable)
            {
                TwoBrainsGames.BrainEngine.RemoteServices.Network.TestInternetAsync();
            }*/

            this.DisableInput();
            ThemeType theme = Levels.CurrentTheme;
            int stageNr = Levels.CurrentStageNr;
            this.LevelStageInfo = this.Navigator.GlobalCache.Get<LevelStage>(GlobalCacheKeys.SELECTED_STAGE_INFO, null);
            if (this.LevelStageInfo == null) // This is to allow a direct start in this screen
            {
                theme = SnailsGame.GameSettings.StartupTheme;
                stageNr = SnailsGame.GameSettings.StartupStageNr;
                if (Levels.CurrentLevel == null)
                {
                    throw new SnailsException("Levels not loaded! It should be loaded GameplayScreen.LoadContent()");
                }
                this.LevelStageInfo = Levels.CurrentLevel.GetLevelStage(theme, stageNr);

                Levels.CurrentStageNr = stageNr;
                Levels.CurrentTheme = theme;
                Levels.CurrentCustomStageFilename = null;
            }
            else
            {
                Levels.CurrentStageNr = this.LevelStageInfo.StageNr;
                Levels.CurrentTheme = this.LevelStageInfo.ThemeId;
                Levels.CurrentCustomStageFilename = this.LevelStageInfo.CustomStageFilename;
            }

#if STAGE_EDITOR
            if (SnailsGame.GameSettings.EntryPoint == GameSettings.GameEntryPoint.StageEditor)
            {
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, false);
                this.State = ScreenState.StartGame;
            }
#endif
            this._loadHintMessage.Visible = false;
            this._asyncLoad.MinimumTime = 0;
            
            if (this._xboxController != null)
            {
                this._xboxController.Visible = false;
            }

            this._board.Visible = false;

            if (this.ShowXBoxHelp && this._xboxController != null)
            {
                this._xboxController.DismissButtonVisible = false; // Only activate the dismiss button when the load ends
                this._xboxController.Show();
            }
            else
            if (this.ShowStageBriefing == true)
            {
                this.InitializeLabels();
                this._board.Show();
            }


            // Set label colors according to the theme
            // Stage not loaded at this time, use global Levels var
            Color color = Colors.ThemeCaptions[(int)Levels.CurrentTheme];
            Color colorVal = Colors.ThemeValues[(int)Levels.CurrentTheme];

            this._capStageNr.CaptionColor = color;
            this._capMode.CaptionColor = color;
            this._capTotalSnails.CaptionColor = color;
            this._capTime.CaptionColor = color;
            this._capToDeliver.CaptionColor = color;

            this._capStageNr.ValueColor = colorVal;
            this._capMode.ValueColor = colorVal;
            this._capTotalSnails.ValueColor = colorVal;
            this._capTime.ValueColor = colorVal;
            this._capToDeliver.ValueColor = colorVal;

            this._btnStart.Visible = false;
            this._btnStageSelection.Visible = false;
            this._btnMainMenu.Visible = false;
            this._btnLoadSolution.Visible = false;
            this._btnStartAndSaveSolution.Visible = false;

            this._loadStarted = false;
            this.EnableInput();

            if (this.ShouldShowRateDialog)
            {
                GameplayScreen.Instance.PopUp(ScreenType.Rate.ToString(), false);
                this._canStartLoading = false;
            }
            else
            {
                this._canStartLoading = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeLabels()
        {
            this._goalIcon.Goal = this.LevelStageInfo._goal;
            this._themeIcon.Theme = this.LevelStageInfo.ThemeId;
            if (!this.LevelStageInfo.IsCustomStage)
            {
                this._capStageNr.Value = this.LevelStageInfo.StageNr;
            }
            else
            {
                this._capStageNr.Value = "-";
            }
            this._capMode.Value = Formater.FormatModeName(this.LevelStageInfo._goal);
            this._capTime.Value = this.LevelStageInfo._targetTime;
            this._capTime.Text = (this.LevelStageInfo._goal == GoalType.TimeAttack ? LanguageManager.GetString("LBL_BRIEFING_TIME_LIMIT") : LanguageManager.GetString("LBL_BRIEFING_REF_TIME"));
            this._capTotalSnails.Value = this.LevelStageInfo._snailsToRelease;
            this._capToDeliver.Value = this.LevelStageInfo._snailsToSave;
            if (this.LevelStageInfo._goal != GoalType.SnailKing)
            {
                this._capToDeliver.Text = (this.LevelStageInfo._goal == GoalType.SnailKiller ?
                    LanguageManager.GetString("LBL_BRIEFING_TO_KILL") :
                    LanguageManager.GetString("LBL_BRIEFING_TO_DELIVER"));
                this._capToDeliver.Visible = true;
                this._capToDeliver.Position = this._label4Pos;
                this._capTime.Position = this._label5Pos;
            }
            else
            {
                this._capToDeliver.Visible = false;
                this._capTime.Position = this._label4Pos;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (this._canStartLoading)
            {
                if (!this._loadStarted)
                {
                    this.ShowTip();
                    if (SnailsGame.GameSettings.UseAsyncLoading)
                    {
                        this.LoadStageAsync();
                    }
                    else
                    {
                        this.LoadStageSync();
                    }
                    this._loadStarted = true;
                }
            }

        }

        /*
        /// <summary>
        /// 
        /// </summary>
        private void ShowTip()
        {
            if (this.TipMessageVisible)
            {
                this._loadHintMessage.InitializeHintText();
                this._loadHintMessage.Visible = true;
                this._asyncLoad.MinimumTime = (this._loadHintMessage.MessageSize * 60);
            }
   
        }
        */


        /// <summary>
        /// 
        /// </summary>
        private void ShowTip()
        {
            /*if (this.TipMessageVisible)
            {
                this._imgFreeSnailsAd.Visible = (SnailsGame.ProfilesManager.CurrentProfile.LastHintMessageSeen % 5 == 0);
                if (this._imgFreeSnailsAd.Visible &&
                    this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.SNAILS_PAID_AD_SEEN_ON_LOADING, false))
                {
                    this._imgFreeSnailsAd.Visible = false;
                }

                if (!this._imgFreeSnailsAd.Visible)
                {
                    this._loadHintMessage.InitializeHintText();
                    this._loadHintMessage.Visible = true;
                    this._asyncLoad.MinimumTime = (this._loadHintMessage.MessageSize * 60);
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.SNAILS_PAID_AD_SEEN_ON_LOADING, false);
                }
                else
                {
                    this._asyncLoad.MinimumTime = 3000;
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.SNAILS_PAID_AD_SEEN_ON_LOADING, true);
                    this._imgFreeSnailsAd.Sprite = BrainGame.ResourceManager.GetSprite("spriteset/snails-free-ad/Ad", ResourceManagerIds.PAID_SNAILS_AD);
                }
            }*/
        }

        #region Button actions
        /// <summary>
        /// 
        /// </summary>
        void btnMainMenu_OnClick(IUIControl sender)
        {
            this.State = ScreenState.MainMenu;
            this.DisableInput();
            this._board.Hide();
            
        }

        /// <summary>
        /// 
        /// </summary>
        void btnStart_OnClick(IUIControl sender)
        {
            this.StartStage(false, false);
        }
  
        /// <summary>
        /// 
        /// </summary>
        void btnStageSelection_OnBack(IUIControl sender)
        {
            this.State = ScreenState.GoBack;
            this.DisableInput();
            this._board.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        void btnLoadSolution_OnClick(IUIControl sender)
        {
            this.StartStage(true, false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        void btnStartAndSaveSolution_OnClick(IUIControl sender)
        {
            this.StartStage(false, true);
        }
        #endregion
    /*    
        #region Screen events
        /// <summary>
        /// 
        /// </summary>
        void StageStartScreen_OnBeforeControlsDraw(IUIControl sender)
        {
            this._leafs.Draw(this.SpriteBatch);
        }

        #endregion
        */
        #region Board events



        /// <summary>
        /// 
        /// </summary>
        void _board_OnHide(BrainEngine.UI.Controls.IUIControl sender)
        {
            switch (this.State)
            {
                case StageStartScreen.ScreenState.GoBack:
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, true);
                    this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), ScreenTransitions.LeafsClosed, ScreenTransitions.LeafsOpening);
                    break;
                case StageStartScreen.ScreenState.StartGame:
                    // save last played stage
                    if (SnailsGame.ProfilesManager.CurrentProfile != null)
                    {
                        LevelStage levelStage = Levels.CurrentLevelStage;
                        if (levelStage != null && !levelStage.IsCustomStage) // Don't store last saved if it's a custom stage
                        {
                            SnailsGame.ProfilesManager.CurrentProfile.LastPlayedStageNr = Levels.CurrentStageNr;
                            SnailsGame.ProfilesManager.CurrentProfile.LastPlayedTheme = Levels.CurrentTheme;
                            SnailsGame.ProfilesManager.Save();
                        }
                    }
                    this.StartGame();
                    break;

                case StageStartScreen.ScreenState.MainMenu:
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleAndMenuVisible);
                    this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), ScreenTransitions.LeafsClosed, ScreenTransitions.LeafsOpening);
                    break;
            }
        }
        #endregion

        #region XBoxControls control events
        /// <summary>
        /// 
        /// </summary>
        void _xboxController_OnDismiss(IUIControl sender)
        {
            if (this.ShowStageBriefing)
            {
                this._board.Show();
            }
            this.StartGame();
        }

        /// <summary>
        /// 
        /// </summary>
        void _xboxController_OnDismissButtonShown(IUIControl sender)
        {
            this.EnableInput();
            this._xboxController.Focus();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        void StartStage(bool loadSolution, bool saveSolution)
        {
            BrainGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.GAMEPLAY_RECORDER_LOAD_SOLUTION, loadSolution);
            BrainGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.GAMEPLAY_RECORDER_SAVE_SOLUTION, saveSolution);

            this.State = ScreenState.StartGame;
            this.DisableInput();
            this._board.Hide();

            if (this._btnStartAndSaveSolution.Visible)
            {
                this._btnStartAndSaveSolution.Hide();
            }
            if (this._btnLoadSolution.Visible)
            {
                this._btnLoadSolution.Hide();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void LoadStageAsync()
        {
            BrainGame.IsLoading = true;

            BrainGame.GameCursor.SetCursor(GameCursors.Busy);

            this._asyncLoad.ClearOperations();
            this._asyncLoad.AddOperation(Levels._instance);
            // Load tutorial on first use 
            if (SnailsGame.Tutorial._loaded == false)
            {
                this._asyncLoad.AddOperation(SnailsGame.Tutorial); // This could go elsewhere. Tutorial is valid for all stages
            }
            this._asyncLoad.StartLoad();
        }

        /// <summary>
        /// 
        /// </summary>
        void LoadStageSync()
        {
            Levels._instance.BeginLoad();
            SnailsGame.Tutorial.BeginLoad();
            this.LoadEnded();
        }
        /// <summary>
        /// 
        /// </summary>
        void _asyncLoad_OnAsyncOperationEnded(IUIControl sender)
        {
            BrainGame.IsLoading = false;
            this.LoadEnded();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadEnded()
        {
            BrainGame.GameCursor.SetCursor(GameCursors.Default);
            if (this.ShowStageBriefing == true)
            {
                this._btnStart.Show();
                this._btnStageSelection.Show();
                this._btnMainMenu.Show();
#if GAMEPLAY_RECORDING_SUPPORT
                this._btnStartAndSaveSolution.Show();
                this._btnLoadSolution.Show();
#endif
                Levels.CurrentLevel.StageSound.PlayMusic();
            }
            else
                if (this.ShowXBoxHelp && this._xboxController != null)
            {
                this._xboxController.ShowDismissButton();
            }
            else
            if (!this.ShowXBoxHelp)
            {
                this.StartGame();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartGame()
        {
            this._imgFreeSnailsAd.Visible = false;
            BrainGame.ResourceManager.Unload(ResourceManagerIds.PAID_SNAILS_AD);
            this.NavigateTo(ScreenType.Gameplay.ToString(), null, ScreenTransitions.LeafsOpening);
        }


        /// <summary>
        /// 
        /// </summary>
        void _imgFreeSnailsAd_OnAccept(IUIControl sender)
        {
            SnailsGame.RedirectToFreeSnailsOnStore();
        }
	}
}
