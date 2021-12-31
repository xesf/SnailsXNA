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

namespace TwoBrainsGames.Snails.Screens
{
    class ThemeSelectionScreenLD : SnailsScreen
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
        private UIStagesPanelWP7 _stagesPanel;
        private UIStageInfo _stageInfo;
        private LevelStage _lastLevelStage;
        private UISnailsButton _btnBackToThemes;
        private UISnailsButton _btnStart;

        private bool StageAutoselected
        { 
            get { return this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.AUTO_SELECT_STAGE, false); } 
            set { this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, value); } 
        }

        public ThemeSelectionScreenLD(ScreenNavigator navigator) :
            base(navigator)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
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

            this._themesPanel = new UIThemeScrollablePanel(this, Vector2.Zero, UIScrollablePanel.PanelOrientation.Horizontal, 4, 2);
            //this._themesPanel.BackgroundColor = Color.Yellow;
            this._themesPanel.ParentAlignment = AlignModes.Top;
            this._themesPanel.Margins.Top = 2700;
            this._themesPanel.OnThemeSelectedStarted += new UIControl.UIEvent(_themesPanel_OnThemeSelectedStarted);
            this._themesPanel.OnThemeSelected += new UIControl.UIEvent(_themesPanel_OnThemeSelected);
            this._themesPanel.OnShow += new UIControl.UIEvent(_themesPanel_OnShow);
            this._themesPanel.OnCancelEnded += new UIControl.UIEvent(_themesPanel_OnCancelEnded);
            this.Controls.Add(this._themesPanel);

            // Stages panel
            this._stagesPanel = new UIStagesPanelWP7(this);
            this._stagesPanel.Name = "_stagesPanel";
            this._stagesPanel.Visible = false;
            this._stagesPanel.OnShow += new UIControl.UIEvent(_stagesPanel_OnShow);
            this._stagesPanel.OnHide += new UIControl.UIEvent(_stagesPanel_OnHide);
            this._stagesPanel.OnStageSelected = this.StagesPanel_OnStageSelected;
            this._stagesPanel.OnStageDoubleSelected = this.StagesPanel_OnStageDoubleSelected;
            //this._stagesPanel.OnStageEnter += new UIControl.UIEvent(_stagesPanel_OnStageEnter);
            //this._stagesPanel.OnStageLeave += new UIControl.UIEvent(_stagesPanel_OnStageLeave);
            this._stagesPanel.OnBack += new UIControl.UIEvent(_stagesPanel_OnBack);
            this._stagesPanel.ParentAlignment = AlignModes.Right;
            this._stagesPanel.Margins.Right = 1600;
            this._stagesPanel.Position = new Vector2(0.0f, 2350.0f);
            this.Controls.Add(this._stagesPanel);

            // Stage info
            this._stageInfo = new UIStageInfo(this);
            this._stageInfo.Position = new Vector2(200, 2450);
            this._stageInfo.Visible = false;
            this._stageInfo.AcceptControllerInput = true;
            this._stageInfo.OnButtonClicked += new UIControl.UIEvent(_stageInfo_OnButtonClicked);
            this._stageInfo.OnInfoCleared += new UIControl.UIEvent(_stageInfo_OnInfoCleared);
            this._stageInfo.OnInfoFilled += new UIControl.UIEvent(_stageInfo_OnInfoFilled);
            this.Controls.Add(this._stageInfo);

            // Back to theme selection button
            this._btnBackToThemes = new UISnailsButton(this, "BTN_BACK", UISnailsButton.ButtonType.Medium, InputBase.InputActions.Back, this.btnBack_OnPress, true);
            this._btnBackToThemes.ParentAlignment = AlignModes.Bottom;
            this._btnBackToThemes.Position = new Vector2(100f, 0f);
            this._btnBackToThemes.Margins.Bottom = 50f;
            this.Controls.Add(this._btnBackToThemes);
            
            // Start button
            this._btnStart = new UISnailsButton(this, "BTN_START", UISnailsButton.ButtonType.Medium, InputBase.InputActions.None, this._btnBack_OnClick, true);
            this._btnStart.ParentAlignment = AlignModes.Bottom;
            this._btnStart.Margins.Bottom = this._btnBackToThemes.Margins.Bottom;
            this._btnStart.Position = new Vector2(1900f, 0f);
            this.Controls.Add(this._btnStart);

            this.OnBack += btnBack_OnPress;
            this.OnOpenTransitionEnded += new UIControl.UIEvent(ThemeSelectionScreenLD_OnOpenTransitionEnded);
        }


        public override void OnStart()
        {
            base.OnStart();

            this.DisableInput();
            this._themesPanel.Initialize();
            this._stagesPanel.Reset();
            this._themesPanel.Visible = false;
            this._stagesPanel.Visible = false;
            this._btnBackToThemes.Visible = false;
            this._btnStart.Visible = false;
            this._title.Visible = false;

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
                this._title.Visible = true;
                this._themesPanel.AcceptControllerInput = false;
                this._themesPanel.SelectThemeWithoutAnimations(theme);
                this._stagesPanel.SetTheme(this.Levels, theme);
                this._stagesPanel.Visible = true;
                this._stagesPanel.Enabled = true;
                this._stagesPanel.Focus(stageNr);
                this._stagesPanel.AcceptControllerInput = true;
                this._stageInfo.Visible = true;
                this._btnBackToThemes.Visible = true;
                this._btnStart.Visible = true;
                _lastLevelStage = Levels.CurrentLevelStage;
                this._stageInfo.Initialize(Levels.CurrentLevelStage);
                this._state = ScreenState.StageSelection;
            }
      
        }

        public override void OnUpdate(BrainGameTime gameTime)
        {
            base.OnUpdate(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        void ThemeSelectionScreenLD_OnOpenTransitionEnded(IUIControl sender)
        {
            if (!this.StageAutoselected)
            {
                this._btnBackToThemes.Show();
                this._themesPanel.Show();
                this._title.Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnBack_OnClick(IUIControl sender)
        {
            StartStage(Levels.CurrentStageNr, Levels.CurrentLevelStage); // FIXME
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
                    this._stagesPanel.Hide();
                    this._btnStart.Hide();
                    this.StageAutoselected = false; // reset autoselect if going back
                    this._state = ScreenState.ThemeSelection;
                    break;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this._themesPanel.AcceptControllerInput = true;
            this._themesPanel.Focus();
            this._state = ScreenState.ThemeSelection;
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
            //this._btnBackToThemes.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        void _themesPanel_OnThemeSelected(IUIControl sender)
        {
            Levels.CurrentTheme = this._themesPanel.SelectedTheme.ThemeId;
            this._themesPanel.Enabled = false;
            this._themesPanel.Visible = false;
            _lastLevelStage = null;
            this._stageInfo.Show();
            this._stagesPanel.Show(this.Levels, this._themesPanel.SelectedTheme.ThemeId);
            //this._btnBackToThemes.Show();
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
            //this._btnBackToThemes.Show();
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
                SnailsGame.ThemeMusic.Fade(0, new TimeSpan(0, 0, AudioTags.MUSIC_FADE_SECONDS));
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

            if (_lastLevelStage == null ||
                _lastLevelStage != null && (_lastLevelStage.StageNr != levelStage.StageNr || 
                                            _lastLevelStage.ThemeId != levelStage.ThemeId))
            {
                _lastLevelStage = levelStage;
                stage.LevelStageInfo = levelStage;
                //this._stageInfo.Visible = true;
                this._stageInfo.Initialize(levelStage);
                Levels.CurrentStageNr = stage.StageNr;
                Levels.CurrentLevelStage = _lastLevelStage;
            }
            /*else
            {
                StagesPanel_OnStageDoubleSelected(sender);
            }*/
        }

        void _stageInfo_OnButtonClicked(IUIControl sender)
        {
            StartStage(Levels.CurrentStageNr, Levels.CurrentLevelStage); // FIXME
        }

        void StartStage(int stageNr, LevelStage levelStage)
        {
            // because it will lost focus
            Levels.CurrentStageNr = stageNr;
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.SELECTED_STAGE_INFO, levelStage);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false);
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, (SnailsGame.Settings.Platform == BrainSettings.PlaformType.XBox));

            BrainGame.SampleManager.StopAll();
            this.NavigateTo("InGame", ScreenType.StageStart.ToString(), ScreenTransitions.LeafsClosing, null);
        }

        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnStageEnter(IUIControl sender)
        {
            UIStage stage = (UIStage)sender;

            LevelStage levelStage = this.Levels.GetLevelStage(this._themesPanel.SelectedTheme.ThemeId, stage.StageNr);
            stage.LevelStageInfo = levelStage;
            //this._stageInfo.Visible = true;
            this._stageInfo.Initialize(levelStage);
        }


        /// <summary>
        /// 
        /// </summary>
        void _stagesPanel_OnStageLeave(IUIControl sender)
        {
            UIStage stage = (UIStage)sender;
            // Check if the stageInfo that is currently display is from "sender" (it may not be if OnEnter from other stage runed first the this onLeave)
            /*if (this._stageInfo.StageInfo.StageNr == stage.StageNr)
            {
                this._stageInfo.Visible = false;
            }*/
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
        void _stageInfo_OnInfoFilled(IUIControl sender)
        {
            if (!this.StageAutoselected)
            {
                this._btnStart.Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _stageInfo_OnInfoCleared(IUIControl sender)
        {
            if (!this.StageAutoselected)
            {
                this._btnStart.Hide();
            }
        }
    }
}
