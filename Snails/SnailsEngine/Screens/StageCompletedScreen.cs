using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI;

namespace TwoBrainsGames.Snails.Screens
{
    /// <summary>
    /// Each line of the stage completed info is shown 1 by 1
    /// When the lines show end, the medal is shown
    /// When the medal show ends, the input is enabled
    /// </summary>
	class StageCompletedScreen : SnailsScreen
    {

        protected enum ScreenState
        {
            Idle,
            ShowingBoard,
            ShowingLabels,
            ShowLabelsSkipped,
            Bluring
        }

        #region Members

        protected UISnailsMenuTitle _title;
        protected UIValuedCaption _capSnailsDelivered;
        protected UIValuedCaption _capTimeTaken;
        protected UIValuedCaption _capSnailsBonus;
        protected UIValuedCaption _capCoinsBonus;
        protected UIValuedCaption _capTimeBonus;
        protected UIValuedCaption _capTotalPoints;
        protected UISnailsBoard _leafsBoard;
        protected UISnailsBoard _board;
        protected UITimer _tmrShowCaptions;
        protected UISnailsMedal _medal;
        protected UIImage _imgHighscore;
        protected UISnailsButton _btnNext;
        protected UISnailsButton _btnQuit;
        protected UISnailsButton _btnAgain;
        protected ThemeType _unlockedTheme;
        protected bool _allowNextStage;
        private LevelStage _nextStage;

        ThemeType UnlockedTheme
        {
            get
            {
                return this._unlockedTheme;
            }
            set
            {
                this._unlockedTheme = value;
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.THEME_UNLOCK_UNLOCKED_THEME, this._unlockedTheme);
            }
        }

        protected ScreenState State { get; set; }
        protected bool _hasHighscore;
        protected Sample _goldMedalSound;
        protected Sample _ticticSound;
        #endregion

        #region Properties
        private StageStats StageStatistics
        {
            get { return Stage.CurrentStage.Stats; }
        }

        private float TopMargin { get; set; }
        private float LineSpacing { get; set; }
        private float SectionSpacing { get; set; }
        private float CaptionWidth { get; set; }
        private float CaptionLeftMargin { get; set; }
        #endregion

        public StageCompletedScreen(ScreenNavigator owner) :
            base(owner, ScreenType.StageCompleted)
        { }


        #region Screen events
        /// <summary>
		/// 
		/// </summary>
		public override void OnLoad()
		{
            base.OnLoad();
            this.BackgroundImage = null;
            this.Name = "StageCompleted";
            this.OnInitializeFromContent += new UIControl.UIEvent(StageCompletedScreen_OnInitializeFromContent);
            this.OnAfterInitializeFromContent += new UIControl.UIEvent(StageCompletedScreen_OnAfterInitializeFromContent);

            // Leafs background
            this._leafsBoard = new UISnailsBoard(this, UISnailsBoard.BoardType.LeafsMedium);
            this._leafsBoard.Name = "_leafsBoard";
            this.Controls.Add(this._leafsBoard);

            // Board
            this._board = new UISnailsBoard(this, UISnailsBoard.BoardType.LightWoodMedium);
            this._board.Name = "_board";
            this._board.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._board.OnShow += new UIControl.UIEvent(_board_OnShow);
            this._leafsBoard.Controls.Add(this._board);

            // Title - Stage Completed
            this._title = new UISnailsMenuTitle(this);
            this._title.Name = "_title";
            this._title.TextResourceId = "TITLE_STAGE_COMPLETED";
            this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
            this._board.Controls.Add(this._title);
         
            Color color = Colors.StageCompletedCaptions;
            // Caption - Snails delivered/killed
            this._capSnailsDelivered = new UIValuedCaption(this, "LBL_STAGE_COMPL_SNAILS_DELIVERED", 0, color, color, UICaption.CaptionStyle.StageCompletedCaptions, 0, true);
            this._capSnailsDelivered.Name = "_capSnailsDelivered";
            this._capSnailsDelivered.Mode = UIValuedCaption.CaptionMode.Simple;
            this._capSnailsDelivered.OnShow += new UIControl.UIEvent(this._captions_OnShow);
            this._board.Controls.Add(this._capSnailsDelivered);

            // Caption - Time taken
            this._capTimeTaken = new UIValuedCaption(this, "LBL_STAGE_COMPL_TIME_TAKEN", new TimeSpan(0, 0, 0), color, color, UICaption.CaptionStyle.StageCompletedCaptions, 0, true);
            this._capTimeTaken.OnShow += new UIControl.UIEvent(this._captions_OnShow);
            this._capTimeTaken.Mode = UIValuedCaption.CaptionMode.Simple;
            this._board.Controls.Add(this._capTimeTaken);

            // Caption - Snails bonus
            color = Colors.StageCompletedBonusCaptions;
            this._capSnailsBonus = new UIValuedCaption(this, "LBL_STAGE_COMPL_SNAILS_BONUS", 6, color, color, UICaption.CaptionStyle.StageCompletedCaptions, 0, true);
            this._capSnailsBonus.OnShow += new UIControl.UIEvent(this._captions_OnShow);
            this._capSnailsBonus.Mode = UIValuedCaption.CaptionMode.Simple;
            //this._capSnailsBonus.Increment = 1;
            this._board.Controls.Add(this._capSnailsBonus);

            // Caption - Time bonus
            this._capTimeBonus = new UIValuedCaption(this, "LBL_STAGE_COMPL_TIME_BONUS", new TimeSpan(0, 0, 0), color, color, UICaption.CaptionStyle.StageCompletedCaptions, 0, true);
            this._capTimeBonus.OnShow += new UIControl.UIEvent(this._captions_OnShow);
            this._capTimeBonus.Mode = UIValuedCaption.CaptionMode.Multiplier;
            this._capTimeBonus.Mode = UIValuedCaption.CaptionMode.Simple;
           // this._capTimeBonus.Increment = 5;
            this._board.Controls.Add(this._capTimeBonus);

            // Caption - 
            this._capCoinsBonus = new UIValuedCaption(this, "LBL_STAGE_COMPL_COIN_BONUS", 0, color, color, UICaption.CaptionStyle.StageCompletedCaptions, 0, true);
            this._capCoinsBonus.OnShow += new UIControl.UIEvent(this._captions_OnShow);
            this._capCoinsBonus.Mode = UIValuedCaption.CaptionMode.Simple;
           // this._capCoinsBonus.Increment = 1;
            this._board.Controls.Add(this._capCoinsBonus);

            // Caption - Total points
            this._capTotalPoints = new UIValuedCaption(this, "LBL_STAGE_COMPL_TOTAL_POINTS", 0, color, color, UICaption.CaptionStyle.StageCompletedCaptions, 0, true);
            this._capTotalPoints.OnShow += new UIControl.UIEvent(this._captions_OnShow);
            this._capTotalPoints.Mode = UIValuedCaption.CaptionMode.Simple;
            //this._capTotalPoints.Increment = 10;
            this._board.Controls.Add(this._capTotalPoints);

            // Show labels timer
            this._tmrShowCaptions = new UITimer(this, 600, false);
            this._tmrShowCaptions.OnTimer += new UIControl.UIEvent(_tmrShowCaptions_OnTimer);
            this.Controls.Add(this._tmrShowCaptions);

            // Button - Stage Selection
            this._btnQuit = new UISnailsButton(this, "BTN_STAGE_SELECTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, this.btnBack_OnAccept, true);
            this._btnQuit.Name = "_btnQuit";
            this._btnQuit.ParentAlignment = AlignModes.Bottom;
            this._btnQuit.Position = new Vector2(0, 0);
            this._btnQuit.ButtonAction = UISnailsButton.ButtonActionType.StageSelection;
            this._board.Controls.Add(this._btnQuit);

            // Button - Play Again
            this._btnAgain = new UISnailsButton(this, "BTN_PLAY_AGAIN", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnAgain_OnAccept, true);
            this._btnAgain.Name = "_btnAgain";
            this._btnAgain.ParentAlignment = AlignModes.Bottom;
            this._btnAgain.Position = new Vector2(1300, 0);
            this._btnAgain.ButtonAction = UISnailsButton.ButtonActionType.Retry;
            this._board.Controls.Add(this._btnAgain);

            // Button - Next stage
            this._btnNext = new UISnailsButton(this, "BTN_NEXT_STAGE", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnNext_OnAccept, true);
            this._btnNext.Name = "_btnNext";
            this._btnNext.ParentAlignment = AlignModes.Bottom;
            this._btnNext.Position = new Vector2(2600, 0);
            this._btnNext.ButtonAction = UISnailsButton.ButtonActionType.Next;
            this._board.Controls.Add(this._btnNext);

            // Medal
            this._medal = new UISnailsMedal(this, Snails.MedalType.None);
            this._medal.Name = "_medal";
          //  this._medal.Position = new Vector2(300.0f, 4050.0f);
            this._medal.OnShow += new UIControl.UIEvent(_medal_OnShow);
            this._board.Controls.Add(this._medal);

            // New Highscore banner
            this._imgHighscore = new UIImage(this, "spriteset/common-elements-1/NewHighscore", ResourceManager.RES_MANAGER_ID_STATIC);
            this._imgHighscore.Name = "_imgHighscore";
            this._imgHighscore.ShowEffect = new SquashEffect(0.8f, 4.0f, 0.04f, this.BlendColor, this.Scale);
            this._imgHighscore.Position = new Vector2(2630f, 3650f);
           // this._board.Controls.Add(this._imgHighscore);

            _goldMedalSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.GOLD_MEDAL);
            this._ticticSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.STAGE_COMPLETE_TICTIC);
#if DEBUG
         //   this.ConnectedCaption = cap;
#endif
            this.WithBlurEffect = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void StageCompletedScreen_OnInitializeFromContent(IUIControl sender)
        {
            this.TopMargin = this.GetContentPropertyValue<float>("topMargin", this.TopMargin);
            this.LineSpacing = this.GetContentPropertyValue<float>("lineSpacing", this.LineSpacing);
            this.SectionSpacing = this.GetContentPropertyValue<float>("sectionSpacing", this.SectionSpacing);
            this.CaptionWidth = this.GetContentPropertyValue<float>("captionWidth", this.CaptionWidth);
            this.CaptionLeftMargin = this.GetContentPropertyValue<float>("captionLeftMargin", this.CaptionLeftMargin);
        }

        /// <summary>
        /// 
        /// </summary>
        void StageCompletedScreen_OnAfterInitializeFromContent(IUIControl sender)
        {
            this._capSnailsDelivered.Position = new Vector2(this.CaptionLeftMargin, this.TopMargin);
            this._capTimeTaken.Position = this._capSnailsDelivered.Position + new Vector2(0.0f, this.LineSpacing);
            this._capSnailsBonus.Position = this._capTimeTaken.Position + new Vector2(0.0f, this.SectionSpacing);
            this._capTimeBonus.Position = this._capSnailsBonus.Position + new Vector2(0.0f, this.LineSpacing);
            this._capCoinsBonus.Position = this._capTimeBonus.Position + new Vector2(0.0f, this.LineSpacing);
            this._capTotalPoints.Position = this._capCoinsBonus.Position + new Vector2(0.0f, this.SectionSpacing);

            this._capSnailsDelivered.Width = this.CaptionWidth;
            this._capTimeTaken.Width = this.CaptionWidth;
            this._capSnailsBonus.Width = this.CaptionWidth;
            this._capTimeBonus.Width = this.CaptionWidth;
            this._capCoinsBonus.Width = this.CaptionWidth;
            this._capTotalPoints.Width = this.CaptionWidth;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            BrainGame.MusicManager.FadeMusic(0, AudioTags.MUSIC_FADE_MSECONDS);

            this._nextStage = null;
           // unlock stages and update player statistics
            this.ProfileStatsUpdate(); 

            this.DisableInput();
            this._capSnailsDelivered.Visible = false;
            this._capTimeTaken.Visible = false;
            this._capSnailsBonus.Visible = false;
            this._capTimeBonus.Visible = false;
            this._capTotalPoints.Visible = false;
            this._capCoinsBonus.Visible = false;

            this._leafsBoard.Visible = false;

            this._tmrShowCaptions.Parameter = 0;
            this._tmrShowCaptions.Enabled = false;
            this._medal.Visible = false;
            this._medal.Reset();
            this.State = ScreenState.Bluring;

            // Init values
            // Snails delivered

            if (Stage.CurrentStage.LevelStage._goal == GoalType.SnailKiller)
            {
                this._capSnailsDelivered.Value = Stage.CurrentStage.Stats.NumSnailsDead;
                this._capSnailsDelivered.Text = LanguageManager.GetString("LBL_STAGE_COMPL_SNAILS_KILLED");
            }
            else
            {
                this._capSnailsDelivered.Value = Stage.CurrentStage.Stats.NumSnailsSafe;
                this._capSnailsDelivered.Text = LanguageManager.GetString("LBL_STAGE_COMPL_SNAILS_DELIVERED");
            }
            // Time taken
            this._capTimeTaken.Value = Stage.CurrentStage.Stats.TimeTaken;
            // Snails bonus
            this._capSnailsBonus.Value = Stage.CurrentStage.Stats.SnailsDeliveredPointsWon;
            // Time bonus
            this._capTimeBonus.Value = Stage.CurrentStage.Stats.TimePointsWon;
            // Coin bonus
            this._capCoinsBonus.Value = Stage.CurrentStage.Stats.CoinPointsWon;
            // Total score
            this._capTotalPoints.Value = Stage.CurrentStage.Stats.TotalScore;
            //Medal
            this._medal.Face = Stage.CurrentStage.Stats.MedalWon;
            // New highscore banner
            this._imgHighscore.Visible = false;
            BrainGame.GameCursor.SetCursor(GameCursors.Default);
            this._btnAgain.Visible = false;
            this._btnNext.Visible = false;
            this._btnQuit.Visible = false;

            this.OnBlurEffectEnded += new EventHandler(StageCompletedScreen_OnBlurEffectEnded);
            this._ticticSound.Play(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_goldMedalSound != null)
            {
                _goldMedalSound.FadeUpdate(gameTime);
            }

            switch (this.State)
            {
                case ScreenState.Idle:
                    break;
                case ScreenState.Bluring:
                    break;
                case ScreenState.ShowingBoard:
                    break;
                case ScreenState.ShowingLabels:
                    if (this.InputController.ActionAccept)
                    {
                        this._tmrShowCaptions.Enabled = false;
                        this._capSnailsDelivered.QuickShow();
                        this._capTimeTaken.QuickShow();
                        this._capSnailsBonus.QuickShow();
                        this._capTimeBonus.QuickShow();
                        this._capCoinsBonus.QuickShow();
                        this._capTotalPoints.QuickShow();
                        this.State = ScreenState.ShowLabelsSkipped;
                        this.AcceptControllerInput = true;
                    }
                    break;
                case ScreenState.ShowLabelsSkipped:
                    break;
            }
        }

        void FadeSamples()
        {
            if (_goldMedalSound != null)
            {
                _goldMedalSound.FadeOut(new TimeSpan(0,0,1));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void btnAgain_OnAccept(IUIControl sender)
        {
            FadeSamples();

            if (this.UnlockedTheme != ThemeType.None)
            {
                this.ShowThemeUnlocked("InGame", ScreenType.StageStart, null);
            }
            else
            {
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, false);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.StageCompleted);
                this.NavigateTo(ScreenType.StageStart.ToString(), ScreenTransitions.LeafsClosing, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void btnBack_OnAccept(IUIControl sender)
        {
            FadeSamples();

            if (this.UnlockedTheme != ThemeType.None)
            {
                this.ShowThemeUnlocked("MainMenu", ScreenType.ThemeSelection, ScreenTransitions.LeafsOpening);
            }
            else
            {
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, true);
                this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void btnNext_OnAccept(IUIControl sender)
        {
            FadeSamples();

            if (this._allowNextStage && this._nextStage != null)
            {
               // Levels.CurrentLevel.IncrementStage();
              //  LevelStage levelStage = Levels.CurrentLevel.GetCurrentStageInfo();
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.SELECTED_STAGE_INFO, this._nextStage);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false); // never shows stage briefing
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, false);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, true);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.StageCompleted);

                if (this.UnlockedTheme != ThemeType.None)
                {
                    this.ShowThemeUnlocked("InGame", ScreenType.StageStart, null);
                }
                else
                {
                    this.NavigateTo(ScreenType.StageStart.ToString(), ScreenTransitions.LeafsClosing, null);
                }
            }
            else // Open purchase screen
            {
                ((SnailsScreen)this).NavigateToPurchase();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void ShowThemeUnlocked(string screenGroupToNavigate, ScreenType screenToNavigate, LeafTransition openingTransition)
        {
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.THEME_UNLOCK_NEXT_SCREEN_GROUP, screenGroupToNavigate);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.THEME_UNLOCK_NEXT_SCREEN, screenToNavigate);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.THEME_UNLOCK_OPEN_TRANSITION, openingTransition);
            this.PopUp(ScreenType.ThemeUnlocked.ToString(), false);
        }

        #endregion

        #region Caption eventw
        /// <summary>
        /// 
        /// </summary>
        protected void _captions_OnShow(IUIControl sender)
        {
            switch (this.State)
            {
                case ScreenState.ShowLabelsSkipped:
                    if (this._medal.Visible == false)
                    {
                        this.ShowMedal();
                    }
                    break;
                case ScreenState.ShowingLabels:
                    this._tmrShowCaptions.Reset();
                    this._tmrShowCaptions.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Blur events
        /// <summary>
        ///  
        /// </summary>
        protected void StageCompletedScreen_OnBlurEffectEnded(object sender, EventArgs e)
        {
            this._leafsBoard.Show();
            this.State = ScreenState.ShowingBoard;
        }
        #endregion

        #region Timer events
        /// <summary>
        /// 
        /// </summary>
        protected void _tmrShowCaptions_OnTimer(IUIControl sender)
        {
 
            int capIdx = (int)this._tmrShowCaptions.Parameter;
            switch (capIdx++)
            {
                case 0:
                    this._capSnailsDelivered.Show();
                    break;
                case 1:
                    this._capTimeTaken.Show();
                    break;
                case 2:
                    this._capSnailsBonus.Show();
                    break;
                case 3:
                    this._capTimeBonus.Show();
                    break;
/*                case 4:
                    this._capUnusedTools.Show();
                    break;*/
                case 4:
                    this._capCoinsBonus.Show();
                    break;
                case 5:
                    this._capTotalPoints.Show();
                    break;
                case 6:
                    if (this._medal.Visible == false)
                    {
                        this.ShowMedal();
                    }
                    break;
            }

            this._tmrShowCaptions.Parameter = capIdx;
        }
        #endregion

        #region Board events


        /// <summary>
        /// 
        /// </summary>
        protected void _board_OnShow(IUIControl sender)
        {
           this.State = ScreenState.ShowingLabels;
           this._tmrShowCaptions.Reset();
           this._tmrShowCaptions.Enabled = true;
        }
        #endregion

        #region Medal events
        /// <summary>
        /// 
        /// </summary>
        protected void _medal_OnShow(IUIControl sender)
        {
            this.ShowLabelsEnded();
        }
        #endregion

        /// <summary>
        /// Unlocks next stage and saves the profile
        /// If a theme is unlocked, the unlocked theme is returned
        /// </summary>
        private void ProfileStatsUpdate()
        {
            this.UnlockedTheme = ThemeType.None;
            // save last played theme
            if (SnailsGame.ProfilesManager.CurrentProfile != null)
            {
                // save the last played stage
                
                LevelStage nextLevelStage = Levels.CurrentLevel.GetNextStageInfo(); // this will predict the next stage to play
                if (nextLevelStage != null && !nextLevelStage.IsCustomStage)
                {
                    // Se for trial e está bloqueado, não setamos
                    if (!SnailsGame.IsTrial || (SnailsGame.IsTrial && nextLevelStage.AvailableInDemo))
                    {
                        SnailsGame.ProfilesManager.CurrentProfile.LastPlayedStageNr = nextLevelStage.StageNr;
                        SnailsGame.ProfilesManager.CurrentProfile.LastPlayedTheme = nextLevelStage.ThemeId;
                    }
                    else // Se o proximo esta bloqueado, setamos o actual
                    {
                        SnailsGame.ProfilesManager.CurrentProfile.LastPlayedStageNr = Stage.CurrentStage.LevelStage.StageNr;
                        SnailsGame.ProfilesManager.CurrentProfile.LastPlayedTheme = Stage.CurrentStage.LevelStage.ThemeId;
                    }
                }
                // Store the theme locked states
                // This will allow to know if any theme was unlocked
                // No need to check theme A, that's always unlocked
                bool themeBUnlocked = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeB);
                bool themeCUnlocked = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeC);
                bool themeDUnlocked = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeD);

                // Update Player stats
                Player.PlayerStageStats playerStats = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(Stage.CurrentStage.LevelStage.StageId);
                if (playerStats != null)
                {
                    playerStats.TimesPlayed++;
                    if (StageStatistics.NumSnailsSafe > playerStats.NumSnailsSafe)
                    {
                        playerStats.NumSnailsSafe = StageStatistics.NumSnailsSafe;
                    }
                    if (playerStats.NumSnailsSafe == StageStatistics.NumSnailsToSave)
                    {
                        SnailsGame.AchievementsManager.Notify((int)AchievementsType.SafelyEscortAllSnailsInOneSpecificStage);
                    }
                    /*if (playerStats.NumSnailsSafe == 0)
                    {
                        SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillEverySnailInOneSpecificStage);
                    }*/
                    if (StageStatistics.NumGoldCoins > playerStats.NumGoldCoins)
                    {
                        playerStats.NumGoldCoins = StageStatistics.NumGoldCoins;
                    }
                    if (StageStatistics.NumSilverCoins > playerStats.NumSilverCoins)
                    {
                        playerStats.NumSilverCoins = StageStatistics.NumSilverCoins;
                    }
                    if (StageStatistics.NumBronzeCoins > playerStats.NumBronzeCoins)
                    {
                        playerStats.NumBronzeCoins = StageStatistics.NumBronzeCoins;
                    }

                    // Determine the medal and the highscore
                    this._hasHighscore = false;
                    if (Stage.CurrentStage.Stats.TotalScore >= playerStats.Highscore)
                    {
                        playerStats.Highscore = Stage.CurrentStage.Stats.TotalScore;
                        this._hasHighscore = true;
                        playerStats.CompletionTime = new TimeSpan(Stage.CurrentStage.Stats.TimeTaken.Ticks);
                    }
                    if ((int)Stage.CurrentStage.Stats.MedalWon > (int)playerStats.Medal)
                    {
                        playerStats.Medal = Stage.CurrentStage.Stats.MedalWon;
                    }
                }

                // unlock next stage if its locked and save profile
                this._nextStage = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.UnlockNextStage(Stage.CurrentStage.LevelStage, true);

                if (this._nextStage != null)
                {
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.NEXT_STAGE_AVAILABLE, true);
                    this.UnlockedTheme = this.CheckIfThemeUnlocked(themeBUnlocked, themeCUnlocked, themeDUnlocked);
                 //   this.UnlockedTheme = ThemeType.ThemeB;
                    // If a new theme was unlocked, then unlock the first stage in that theme
                    if (this.UnlockedTheme != ThemeType.None)
                    {
                        SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.UnlockFirstStage(this.UnlockedTheme);
                    }
                    this._allowNextStage = true;
                }
                else
                {
                    this._allowNextStage = false;
                }

                int totalStagesUnlocked = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.GetClearStagesForTheme(Levels.CurrentTheme);
                if (totalStagesUnlocked == Levels.MAX_NUMBER_STAGES_PER_THEME) // all stages unlocked
                {
                    switch (Levels.CurrentTheme)
                    {
                        case ThemeType.ThemeA:
                            SnailsGame.AchievementsManager.Notify((int)AchievementsType.ClearAllWildNatureStages);
                            break;
                        case ThemeType.ThemeB:
                            SnailsGame.AchievementsManager.Notify((int)AchievementsType.ClearAllEgyptStages);
                            break;
                        case ThemeType.ThemeC:
                            SnailsGame.AchievementsManager.Notify((int)AchievementsType.ClearAllGraveyardStages);
                            break;
                        case ThemeType.ThemeD:
                            SnailsGame.AchievementsManager.Notify((int)AchievementsType.ClearAllGoldminesStages);
                            break;
                    }
                }

                if (SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsAllMedalsGet(MedalType.Bronze))
                {
                    SnailsGame.AchievementsManager.Notify((int)AchievementsType.GetAllBronzeMedals);
                }
                if (SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsAllMedalsGet(MedalType.Silver))
                {
                    SnailsGame.AchievementsManager.Notify((int)AchievementsType.GetAllSilverMedals);
                }
                if (SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsAllMedalsGet(MedalType.Gold))
                {
                    SnailsGame.AchievementsManager.Notify((int)AchievementsType.GetAllGoldMedals);
                }

                SnailsGame.ProfilesManager.Save();
            }
        }

        /// <summary>
        /// Checks if any theme was unlocked,
        /// it receives the unlock state prior to the player stats update
        /// if the unlock state changedm, then that theme was unlocked
        /// </summary>
        private ThemeType CheckIfThemeUnlocked(bool themeBUnlocked, bool themeCUnlocked, bool themeDUnlocked)
        {
            // Alright, we have unlocked a stage, now let's check the theme locked state again
            // any changes will tell us that a theme was unlocked
            if (themeBUnlocked == false &&
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeB) == true)
            {
                // State has changed! Theme was unlocked!!
                return ThemeType.ThemeB;
            }

            if (themeCUnlocked == false &&
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeC) == true)
            {
                // State has changed! Theme was unlocked!!
                return ThemeType.ThemeC;
            }

            if (themeDUnlocked == false &&
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeD) == true)
            {
                // State has changed! Theme was unlocked!!
                return ThemeType.ThemeD;
            }

            return ThemeType.None;
        }
      
        /// <summary>
        /// 
        /// </summary>
        private void ShowLabelsEnded()
        {
            this.EnableInput();
            this.InstructionBar.HideAllLabels();
            this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.StartNextStage);
            this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Quit);
            this._board.Focus();
            this.State = ScreenState.Idle;
            this._btnAgain.Show();
            // Next is only display if 
            // -Can play next tage
            // -Or can't play next stage but it's not a free version of the game (next button will navigate to the purchase screen)
            if (this._allowNextStage || (!this._allowNextStage && SnailsGame.GameSettings.WithAppStore))
            {
                this._btnNext.Show();
            }
            this._btnQuit.Show();


            if (this._btnNext.Visible)
            {
                this._btnNext.Focus();
            }
            else
            {
                this._btnQuit.Focus();
            }

            this._ticticSound.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowMedal()
        {
            if (this._medal.Face != MedalType.None)
            {

                this._medal.Show();
                if (this._medal.Face == MedalType.Gold)
                {
                    _goldMedalSound.Play();
                }
            }
            else // IF we don't show the medal, medal show end event will not run and this will cause the ShowLabels ended not to be called
            {
                this.ShowLabelsEnded();
            }
            if (this._hasHighscore)
            {
                this._imgHighscore.Show();
            }
        }
	}
}
