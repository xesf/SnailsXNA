using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.UI;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Screens.ThemeSelection;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens
{
    class ThemeSelectionLDScreen : SnailsScreen
    {
        enum ScreenState
        {
            ThemeSelection,
            StageSelection
        }

        #region Consts
        public const int THEME_COUNT = 4;
        #endregion

        private Levels Levels { get; set; }

        private ScreenState _state;
        private UISnailsMenuTitle _title;
        private UIThemeScrollablePanel _themesPanel;
        private UIStagesPanelLD _stagesPanel;
        private UIStageInfo _stageInfo;
        private LevelStage _lastLevelStage;
        private UIBackButton _btnBack;
        private Sample _stageSelectSound;
        private UISnailsButton _btnLeaderboards;

#if DEBUG
        protected UILocker _imgStageUnlock;
#endif
        private bool StageAutoselected
        { 
            get { return this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.AUTO_SELECT_STAGE, false); } 
            set { this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, value); } 
        }

        public ThemeSelectionLDScreen(ScreenNavigator navigator) :
            base(navigator, ScreenType.ThemeSelection)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
#if DEBUG
            // Unlock all levels
            this._imgStageUnlock = new UILocker(this);
            this._imgStageUnlock.Name = "_imgStageUnlock";
            this._imgStageUnlock.Scale = new Vector2(0.8f, 0.8f);
            this._imgStageUnlock.Position = new Vector2(500f, 800f);
            this._imgStageUnlock.OnAccept += new UIControl.UIEvent(_imgStageUnlock_OnAccept);
            this._imgStageUnlock.AcceptControllerInput = true;
            this.Controls.Add(this._imgStageUnlock);
#endif


            // Loads levels data
            // This is needed because the targets and general info about each stage are needed here 
            this.BackgroundImageBlendColor = Colors.ThemeSelectionScrBkColor;
            this.Levels = Levels.Load();

            // All controls below should be inside the scrollable panel

            // Title
            this._title = new UISnailsMenuTitle(this);
            this._title.TextResourceId = "TITLE_STAGE_SELECTION";
            this._title.ParentAlignment = AlignModes.Horizontaly;
            this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
            this._title.Position = new Vector2(0f, 500f);
            this._title.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.04f, this.BlendColor, new Vector2(1.0f, 1.0f));
            this.Controls.Add(this._title);

            this._themesPanel = new UIThemeScrollablePanel(this);
            this._themesPanel.Position = new Vector2(200f, 3100f);
            this._themesPanel.OnThemeSelectedStarted += new UIControl.UIEvent(_themesPanel_OnThemeSelectedStarted);
            this._themesPanel.OnThemeSelected += new UIControl.UIEvent(_themesPanel_OnThemeSelected);
            this._themesPanel.OnShow += new UIControl.UIEvent(_themesPanel_OnShow);
            this._themesPanel.OnCancelEnded += new UIControl.UIEvent(_themesPanel_OnCancelEnded);
            this.Controls.Add(this._themesPanel);

            // Stages panel
            this._stagesPanel = new UIStagesPanelLD(this);
            this._stagesPanel.Name = "_stagesPanel";
            this._stagesPanel.Visible = false;
            this._stagesPanel.OnShow += new UIControl.UIEvent(_stagesPanel_OnShow);
            this._stagesPanel.OnHide += new UIControl.UIEvent(_stagesPanel_OnHide);
            this._stagesPanel.OnStageSelected = this.StagesPanel_OnStageSelected;
            this._stagesPanel.OnStageDoubleSelected = this.StagesPanel_OnStageDoubleSelected;
            this._stagesPanel.OnBack += new UIControl.UIEvent(_stagesPanel_OnBack);
            this._stagesPanel.ParentAlignment = AlignModes.Right;
            this._stagesPanel.Margins.Right = 130f;
            this._stagesPanel.Position = new Vector2(0.0f, 2600.0f);
            this._stagesPanel.Size = new Size(4450f, 7000f);
            this.Controls.Add(this._stagesPanel);

            // Stage info
            this._stageInfo = new UIStageInfo(this);
            this._stageInfo.Position = new Vector2(100, 2450);
            this._stageInfo.Visible = false;
            this._stageInfo.AcceptControllerInput = true;
            this._stageInfo.OnAccept += new UIControl.UIEvent(_stageInfo_OnAccept);
            this.Controls.Add(this._stageInfo);

            // Back to theme selection button
            this._btnBack = new UIBackButton(this);
            this._btnBack.ScreenAlignment = UIBackButton.ButtonScreenAlignment.BottomLeft;
            this._btnBack.OnPress += new UIControl.UIEvent(btnBack_OnPress);
            this.Controls.Add(this._btnBack);

            // Purchase button
            this._btnLeaderboards = new UISnailsButton(this, "BTN_LEADERBOARDS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnLeaderboards_OnClick, true);
            this._btnLeaderboards.Name = "_btnLeaderboards";
            this._btnLeaderboards.ParentAlignment = AlignModes.Bottom;
            this._btnLeaderboards.Position = new Vector2(3600f, 0f);
            this._btnLeaderboards.Visible = SnailsGame.GameSettings.UseLeaderboards;
            this.Controls.Add(this._btnLeaderboards);

            this.ShowTrialTag = true;
            this.ShowRateTag = true;
            this.OnBack += btnBack_OnPress;

            this._stageSelectSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MENU_ITEM_SELECTED);
            this.BackgroundType = ScreenBackgroundType.Image;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnUnload()
        {
            base.OnUnload();
            BrainGame.ResourceManager.Unload(ResourceManagerIds.STAGE_THUMBNAILS);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            this.DisableInput();
            this._themesPanel.Initialize();
            this._stagesPanel.ResetPanel();
            this._themesPanel.Visible = true;
            this._stagesPanel.Visible = false;
            this._stageInfo.Visible = false;
            this._btnLeaderboards.Visible = false;
            this._btnBack.Visible = true;
            this._title.Visible = true;

            // Set theme unlock goals
            if (this._themesPanel.AncientEgyptTheme.Locked)
            {
                this._themesPanel.AncientEgyptTheme.UpdateUnlockGoal();
            }
            if (this._themesPanel.BotFactoryTheme.Locked)
            {
                this._themesPanel.BotFactoryTheme.UpdateUnlockGoal();
            }
            if (this._themesPanel.OuterSpaceTheme.Locked)
            {
                this._themesPanel.OuterSpaceTheme.UpdateUnlockGoal();
            }

            if (this.StageAutoselected) // If the stage is to be autoselected. Usualy when the back button is pressed in the stage selectetion
            {
                int stageNr = Levels.CurrentStageNr;
                ThemeType theme = Levels.CurrentTheme;
                this._themesPanel.SelectThemeWithoutAnimations(theme);
                this._themesPanel.Visible = false;
                this._stagesPanel.SetTheme(this.Levels, theme);
                this._stagesPanel.Visible = true;
                this._stagesPanel.Enabled = true;
                this._stagesPanel.Focus(stageNr);
                this._stagesPanel.AcceptControllerInput = true;
                this._stagesPanel.SelectStage(Levels.CurrentLevelStage);
                this._stageInfo.Visible = true;
                this._btnLeaderboards.Visible = SnailsGame.GameSettings.UseLeaderboards;
                this._lastLevelStage = Levels.CurrentLevelStage;
                this._stageInfo.Initialize(Levels.CurrentLevelStage);
                this._state = ScreenState.StageSelection;
            }
            this.EnableInput(); // Or else any object.Focus() will do nothing

            // If we came from ingame, the music is stopped
            if (!BrainGame.MusicManager.IsMusicActive)
            {
                SnailsGame.ThemeMusic.Play(true);
            }
   
#if DEBUG
            this._imgStageUnlock.Locked = !SnailsGame.GameSettings.AllStagesUnlocked;
#endif
        }





        /// <summary>
        /// 
        /// </summary>
        void _stageInfo_OnAccept(IUIControl sender)
        {
            if (this._lastLevelStage != null)
            {
                this._stageSelectSound.Play();
                this.StartSelectedStage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void btnBack_OnPress(IUIControl sender)
        {
            switch (this._state)
            {
                case ScreenState.ThemeSelection:
                    this.DisableInput();
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleAndMenuVisible);
                    this.NavigateTo("MainMenu", ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
                    break;

                case ScreenState.StageSelection:
                    this.DisableInput();
                    this._stageInfo.Hide();
                    this._btnLeaderboards.Hide();
                    this._stagesPanel.Hide();
                    this.StageAutoselected = false; // reset autoselect if going back
                    this._state = ScreenState.ThemeSelection;
                    // auto snap theme page
                    int pageSnap = (int)Levels.CurrentTheme;
                    if (pageSnap < 2)
                    {
                        pageSnap = 0;
                    }
                    else if (pageSnap >= 2 && pageSnap < 4)
                    {
                        pageSnap = 1;
                    }
                   // this._themesPanel.PositionInPixels = new Vector2(pageSnap * (this._themesPanel.SizeInPixels.Width - this.SizeInPixels.Width) * -1, 0);
                    break;
            }
        }

#if DEBUG
        protected void _imgStageUnlock_OnAccept(IUIControl sender)
        {
            SnailsGame.GameSettings.AllStagesUnlocked = !SnailsGame.GameSettings.AllStagesUnlocked;
            this._imgStageUnlock.Locked = !SnailsGame.GameSettings.AllStagesUnlocked;
            if (this._state == ScreenState.ThemeSelection)
            {
                this._themesPanel.Initialize(); // refresh locked/unlocked themes
            }
            else if (this._state == ScreenState.StageSelection)
            {
                this._stagesPanel.SetTheme(Levels.CurrentLevel, Levels.CurrentTheme);
            }
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnShow(IUIControl sender)
        {
/*            this.EnableInput();
            this._themesPanel.AcceptControllerInput = true;
            this._themesPanel.Focus();
            this._state = ScreenState.ThemeSelection;*/
#if DEBUG
            this._themesPanel.Initialize(); // only to refresh locked/unlocked themes
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnCancelEnded(IUIControl sender)
        {
            this.EnableInput();
            this._themesPanel.Focus();
            this._themesPanel.AcceptControllerInput = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnThemeSelectedStarted(IUIControl sender)
        {
            this.DisableInput();
            //this._btnBack.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnThemeSelected(IUIControl sender)
        {
            if (Levels.CurrentTheme != this._themesPanel.SelectedTheme.ThemeId)
            {
                this._stagesPanel.ScrollToTop();
            }
            Levels.CurrentTheme = this._themesPanel.SelectedTheme.ThemeId;
            this._themesPanel.Enabled = false;
            this._themesPanel.Visible = false;
            this._lastLevelStage = null;
            this._stageInfo.Show();
            if (SnailsGame.GameSettings.UseLeaderboards)
            {
                this._btnLeaderboards.Show();
            }
            this._stagesPanel.Show(this.Levels, this._themesPanel.SelectedTheme.ThemeId);
        }

        #region StagePanel events

        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnHide(IUIControl sender)
        {
            this._themesPanel.Visible = true;
            this._themesPanel.CancelSelection();
            this._themesPanel.Enabled = true;
            //this._btnBack.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this._stagesPanel.FocusOnLastUnlocked();
            this._state = ScreenState.StageSelection;
        }

        private void StagesPanel_OnStageDoubleSelected(IUIControl sender)
        {
            UIStage stage = (UIStage)sender;

            if (SnailsGame.ThemeMusic != null && SnailsGame.ThemeMusic.IsPlaying)
            {
                BrainGame.MusicManager.FadeMusic(0, AudioTags.MUSIC_FADE_MSECONDS);
            }

            if (stage.Locked && SnailsGame.IsTrial && !stage.LevelStageInfo.AvailableInDemo && SnailsGame.GameSettings.WithAppStore)
            {
                ((SnailsScreen)this).NavigateToPurchase();
                return;
            }

            if (stage.Locked)
            {
                return;
            }

            this.DisableInput();
            this._stagesPanel.RaiseStageLeaveEvent = false;
            stage.DoOnLeaveEffect = false; // This will avoid the minimize effect on the stage control

            StartStage(stage.StageNr, stage.LevelStageInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StagesPanel_OnStageSelected(IUIControl sender)
        {
            UIStage stage = (UIStage)sender;
            
            LevelStage levelStage = this.Levels.GetLevelStage(this._themesPanel.SelectedTheme.ThemeId, stage.StageNr);

            if (levelStage.AvailableInDemo == false && stage.Locked)
            {
                if (SnailsGame.GameSettings.WithAppStore)
                {
                    this.NavigateToPurchase();
                }
                return;
            }

            if (_lastLevelStage == null ||
                _lastLevelStage != null && (_lastLevelStage.StageNr != levelStage.StageNr || 
                                            _lastLevelStage.ThemeId != levelStage.ThemeId))
            {
                _lastLevelStage = levelStage;
                stage.LevelStageInfo = levelStage;
                this._stageInfo.Initialize(levelStage);
                Levels.CurrentStageNr = stage.StageNr;
                Levels.CurrentLevelStage = _lastLevelStage;
            }
            else
            {
                this.StartSelectedStage(); 
            }
        }

        void _stageInfo_OnButtonClicked(IUIControl sender)
        {
            this.StartSelectedStage(); 
        }

        void StartStage(int stageNr, LevelStage levelStage)
        {
            // because it will lost focus
            Levels.CurrentStageNr = stageNr;
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, true);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.SELECTED_STAGE_INFO, levelStage);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, (SnailsGame.Settings.Platform == BrainSettings.PlaformType.XBox));
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.ThemeSelection);

            this.NavigateTo("InGame", ScreenType.StageStart.ToString(), ScreenTransitions.LeafsClosing, null);
        }

        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnStageEnter(IUIControl sender)
        {
            UIStage stage = (UIStage)sender;

            if (stage.Locked == false)
            {
                LevelStage levelStage = this.Levels.GetLevelStage(this._themesPanel.SelectedTheme.ThemeId, stage.StageNr);
                stage.LevelStageInfo = levelStage;
                this._stageInfo.Initialize(levelStage);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnStageLeave(IUIControl sender)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnBack(IUIControl sender)
        {
            
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        void StartSelectedStage()
        {
            BrainGame.MusicManager.FadeMusic(0, AudioTags.MUSIC_FADE_MSECONDS);
            StartStage(this._lastLevelStage.StageNr, this._lastLevelStage); 
        }

        /// <summary>
        /// 
        /// </summary>
        void btnLeaderboards_OnClick(IUIControl sender)
        {
            SnailsGame.ScreenNavigator.PopUp(ScreenType.Leaderboards.ToString());
        }

    }
}
