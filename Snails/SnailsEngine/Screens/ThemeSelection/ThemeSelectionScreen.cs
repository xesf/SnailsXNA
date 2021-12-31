using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
    class ThemeSelectionScreen : SnailsScreen
    {
        enum ScreenState
        {
            None,
            ThemeSelection,
            StageSelection
        }

        #region Consts
        public const int THEME_COUNT = 4;
        #endregion

        private Levels Levels { get; set; }
        private UIStagesPanel _stagesPanel;
        private UIThemesPanel _themesPanel;
        private UIStageInfo _stageInfo;
        private ScreenState _state;
        private UITimer _tmrShowTitle;
        private UIBackButton _btnBack;
        private UISnailsMenuTitle _title;

        private bool StageAutoselected
        { get { return this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.AUTO_SELECT_STAGE, false); } }

#if DEBUG
        protected UILocker _imgStageUnlock;
#endif

        public ThemeSelectionScreen(ScreenNavigator navigator):
            base(navigator, ScreenType.ThemeSelection)
        {
           //BrainGame.MusicManager.OnFadeOut += new BrainEngine.Audio.MusicManager.FadeOutMusicHandler(MusicManager_OnFadeOut);
        }

        /// <summary>
        /// 
        /// </summary>
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

            // Title
            this._title = new UISnailsMenuTitle(this);
            this._title.TextResourceId = "TITLE_STAGE_SELECTION";
            this._title.ParentAlignment = AlignModes.Horizontaly;
            this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
            this._title.Position = new Vector2(0f, 500f);
            this._title.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.04f, this.BlendColor, new Vector2(1.0f, 1.0f));
            this.Controls.Add(this._title);

            // Themes panel
            this._themesPanel = new UIThemesPanel(this);
            this._themesPanel.Name = "_themesPanel";
            this._themesPanel.OnThemeSelectedStarted += new UIControl.UIEvent(_themesPanel_OnThemeSelectedStarted);
            this._themesPanel.OnThemeSelected += new UIControl.UIEvent(_themesPanel_OnThemeSelected);
            this._themesPanel.OnShow += new UIControl.UIEvent(_themesPanel_OnShow);
            this._themesPanel.OnCancelEnded += new UIControl.UIEvent(_themesPanel_OnCancelEnded);
            this._themesPanel.ParentAlignment = AlignModes.Horizontaly | AlignModes.Top;
            this._themesPanel.Margins.Top = 1800;

            this.Controls.Add(this._themesPanel);

            // Stages panel
            this._stagesPanel = new UIStagesPanel(this);
            this._stagesPanel.Name = "_stagesPanel";
            this._stagesPanel.Visible = false;
            this._stagesPanel.OnShow += new UIControl.UIEvent(_stagesPanel_OnShow);
            this._stagesPanel.OnHide += new UIControl.UIEvent(_stagesPanel_OnHide);
            this._stagesPanel.OnStageSelected = this.StagesPanel_OnStageSelected;
            this._stagesPanel.OnStageEnter += new UIControl.UIEvent(_stagesPanel_OnStageEnter);
            this._stagesPanel.OnStageLeave += new UIControl.UIEvent(_stagesPanel_OnStageLeave);
            this._stagesPanel.OnBack += new UIControl.UIEvent(_stagesPanel_OnBack);
            this._stagesPanel.ParentAlignment = AlignModes.Horizontaly;
            this._stagesPanel.Position = new Vector2(0.0f, 5800.0f);
            this.Controls.Add(this._stagesPanel);

            // Stage info
            this._stageInfo = new UIStageInfo(this);
            this._stageInfo.Position = new Vector2(5700, 1700);
            this._stageInfo.Visible = false;
            this._stageInfo.AcceptControllerInput = false;
            this.Controls.Add(this._stageInfo);

            // Show title timer
            this._tmrShowTitle = new UITimer(this, 150, false);
            this._tmrShowTitle.Enabled = false;
            this._tmrShowTitle.OnTimer += new UIControl.UIEvent(_tmrShowTitle_OnTimer);
            this.Controls.Add(this._tmrShowTitle);

            if (SnailsGame.GameSettings.ShowBackButtonInThemeSelection)
            {
                // Back button
                this._btnBack = new UIBackButton(this);
                this._btnBack.ScreenAlignment = UIBackButton.ButtonScreenAlignment.BottomLeft;
                this._btnBack.OnAccept += new UIControl.UIEvent(this.btnBack_OnPress);
                this.Controls.Add(this._btnBack);
            }
            else
            {
                this.OnBack += this.btnBack_OnPress;
            }

            this.OnOpenTransitionEnded += new UIControl.UIEvent(ThemeSelectionScreen_OnOpenTransitionEnded);

            this._state = ScreenState.None;
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
        public override void OnClose()
        {
            base.OnClose();
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
            this._stagesPanel.Reset();
            this._themesPanel.Visible = false;
            this._stagesPanel.Visible = false;
            this._title.Visible = false;
            this._tmrShowTitle.Enabled = false;
            this._btnBack.Visible = false;

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
                this.EnableInput(); // Or else any object.Focus() will do nothing
                int stageNr = Levels.CurrentStageNr;
                ThemeType theme = Levels.CurrentTheme;
                this._themesPanel.Visible = true;
                this._title.Visible = true;
                this._btnBack.Visible = true;
                this._themesPanel.AcceptControllerInput = false;
                this._themesPanel.SelectThemeWithoutAnimations(theme);

                this._stagesPanel.SetTheme(this.Levels, theme);
                this._stagesPanel.Visible = true;
                this._stagesPanel.Focus(stageNr);

                this._state = ScreenState.StageSelection;
                this.DisableInput();
            }
            else
            {
                this._themesPanel.Show();
                this._tmrShowTitle.Enabled = true;
            }

            // If we came from ingame, the music is stopped
            if (!BrainGame.MusicManager.IsMusicActive)
            {
                SnailsGame.ThemeMusic.Play(true);
            }

#if DEBUG
            this._imgStageUnlock.Locked = !SnailsGame.GameSettings.AllStagesUnlocked;
#endif
        }

        void _tmrShowTitle_OnTimer(IUIControl sender)
        {
            this._title.Show();
            this._btnBack.Show();

        }

        /// <summary>
        /// 
        /// </summary>
        void ThemeSelectionScreen_OnOpenTransitionEnded(IUIControl sender)
        {
            if (this.StageAutoselected)
            {
                this.EnableInput();
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
                    this._stageInfo.Visible = false;
                    this._stagesPanel.Hide();
                    this._state = ScreenState.ThemeSelection;
                    break;
            }
        }
#if DEBUG
        /// <summary>
        /// 
        /// </summary>
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
     
        #region Themes panel events
        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this._themesPanel.AcceptControllerInput = true;
            this._themesPanel.Focus();
            this._state = ScreenState.ThemeSelection;
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
        }

       

        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnThemeSelectedStarted(IUIControl sender)
        {
            this.DisableInput();
            this.InstructionBar.HideAllLabels();
        }


        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnThemeSelected(IUIControl sender)
        {
            Levels.CurrentTheme = this._themesPanel.SelectedTheme.ThemeId;
            this._themesPanel.Enabled = false;
            this._stagesPanel.Show(this.Levels, this._themesPanel.SelectedTheme.ThemeId);

            /*
            // does fade on any playing music
            if (SnailsGame.ThemeMusic != null)
            {
                SnailsGame.ThemeMusic.Fade(0, new TimeSpan(0, 0, AudioTags.MUSIC_FADE_SECONDS));
            }

            Levels.CurrentLevel.SoundsByTheme[(int)Levels.CurrentTheme].PlayMusic();*/
        }

        #endregion

        #region StagePanel events

        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnHide(IUIControl sender)
        {
            this._themesPanel.CancelSelection();
            this._themesPanel.Enabled = true;
            BrainGame.ResourceManager.Unload(ResourceManagerIds.STAGE_THUMBNAILS);
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

        /// <summary>
        /// 
        /// </summary>
        private void StagesPanel_OnStageSelected(IUIControl sender)
        {
            if (SnailsGame.ThemeMusic != null && SnailsGame.ThemeMusic.IsPlaying)
            {
                BrainGame.MusicManager.FadeMusic(0, AudioTags.MUSIC_FADE_MSECONDS);
            }

            this.DisableInput();
            this._stagesPanel.RaiseStageLeaveEvent = false;
            UIStage stage = (UIStage)sender;

            if (stage.Locked && SnailsGame.IsTrial && !stage.LevelStageInfo.AvailableInDemo && SnailsGame.GameSettings.WithAppStore)
            {
                ((SnailsScreen)this).NavigateToPurchase();
                return;
            }
            
            if (stage.Locked)
            {
                return;
            }

            stage.DoOnLeaveEffect = false; // This will avoid the minimize effect on the stage control
            // because it will lost focus
            Levels.CurrentStageNr = stage.StageNr;
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.SELECTED_STAGE_INFO, stage.LevelStageInfo);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, (SnailsGame.Settings.Platform == BrainSettings.PlaformType.XBox));
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, true);
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
                this._stageInfo.Visible = true;
                this._stageInfo.Initialize(levelStage);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnStageLeave(IUIControl sender)
        {
            UIStage stage = (UIStage)sender;
            if (this._stageInfo.Visible)
            {
                if (this._stageInfo.StageInfo.StageNr == stage.StageNr)
                {
                    this._stageInfo.Visible = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnBack(IUIControl sender)
        {
        }

        #endregion

    }
}
