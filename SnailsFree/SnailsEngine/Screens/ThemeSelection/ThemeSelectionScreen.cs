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
		public const int THEME_COUNT = 3;
		#endregion

		private Levels Levels { get; set; }
		private UIStagesPanel _stagesPanel;
		private UIThemesPanel _themesPanel;
		private UIStageInfo _stageInfo;
		private ScreenState _state;
		private UITimer _tmrShowTitle;
		private UIBackButton _btnBack;
		private UISnailsMenuTitle _title;
		private UIStage _selectedStage;
		private UISnailsButton _btnYouTube;
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
			this._title.Position = new Vector2(0f, 100f);
			this._title.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.04f, this.BlendColor, new Vector2(1.0f, 1.0f));
			this.Controls.Add(this._title);

			// Themes panel
			this._themesPanel = new UIThemesPanel(this);
			this._themesPanel.Name = "_themesPanel";
			this._themesPanel.OnThemeSelectedStarted += new UIControl.UIEvent(_themesPanel_OnThemeSelectedStarted);
			this._themesPanel.OnThemeSelected += new UIControl.UIEvent(_themesPanel_OnThemeSelected);
			this._themesPanel.OnShow += new UIControl.UIEvent(_themesPanel_OnShow);
			this._themesPanel.OnCancelEnded += new UIControl.UIEvent(_themesPanel_OnCancelEnded);
			this._themesPanel.ParentAlignment = AlignModes.Horizontaly;
			this._themesPanel.Position = new Vector2(0f, this._title.Bottom);
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
			this._stagesPanel.Position = new Vector2(0.0f, 5600.0f);
			this.Controls.Add(this._stagesPanel);

			// Stage info
			this._stageInfo = new UIStageInfo(this);
			this._stageInfo.Position = new Vector2(0.0f, this._title.Bottom); 
			this._stageInfo.AcceptControllerInput = true;
			this._stageInfo.OnAccept += new UIControl.UIEvent(stageInfo_OnAccept);
			this._stageInfo.OnStartLoadingThumbnails += new UIControl.UIEvent(stageInfo_OnStartLoadingThumbnails);
			this._stageInfo.OnEndLoadingThumbnails += new UIControl.UIEvent(stageInfo_OnEndLoadingThumbnails);
			this._stageInfo.ParentAlignment = AlignModes.Right;
			this._stageInfo.Margins.Right = 500;
			this.Controls.Add(this._stageInfo);

			// Youtube button
			this._btnYouTube = new UISnailsButton(this, "", UISnailsButton.ButtonSizeType.YouTube, InputBase.InputActions.None, this.btnYouTube_OnClick, true);
			this._btnYouTube.Name = "_btnYouTube";
			this._btnYouTube.Position = this.PixelsToScreenUnits(new Vector2(0.0f, 365.0f));
			this._btnYouTube.ParentAlignment = AlignModes.Right;
			this._btnYouTube.Margins.Right = this._stageInfo.Margins.Right;
			this._btnYouTube.Visible = false;
			this.Controls.Add(this._btnYouTube);

			// Show title timer
			this._tmrShowTitle = new UITimer(this, 150, false);
			this._tmrShowTitle.Enabled = false;
			this._tmrShowTitle.OnTimer += new UIControl.UIEvent(_tmrShowTitle_OnTimer);
			//    this.Controls.Add(this._tmrShowTitle);

			if (SnailsGame.GameSettings.ShowBackButtonInThemeSelection)
			{
				// Back button
				this._btnBack = new UIBackButton(this);
				this._btnBack.Position = new Vector2 (200, this._themesPanel.Bottom);
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
			SnailsGame.UnloadThumbnails ();
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
			this._selectedStage = null;
			this._stageInfo.Visible = false;
			this._btnYouTube.Visible = false;

			// Set theme unlock goals
			if (this._themesPanel.AncientEgyptTheme.Locked)
			{
				this._themesPanel.AncientEgyptTheme.UpdateUnlockGoal();
			}
			if (this._themesPanel.BotFactoryTheme.Locked)
			{
				this._themesPanel.BotFactoryTheme.UpdateUnlockGoal();
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
				this._selectedStage = this._stagesPanel.Focus(stageNr);
				this._selectedStage.Selected = true;

				this._state = ScreenState.StageSelection;
				this.ShowStageInfo (this._selectedStage);
				this.DisableInput();
			}
			else
			{
				this._themesPanel.Visible = true;
				this._title.Visible = true;
				this._btnBack.Visible = true;
				this.EnableInput();
				this._themesPanel.AcceptControllerInput = true;
				this._themesPanel.Focus();
				this._state = ScreenState.ThemeSelection;
			}

			// If we came from ingame, the music is stopped
			if (!BrainGame.MusicManager.IsMusicActive &&
				SnailsGame.ThemeMusic != null)
			{
				SnailsGame.ThemeMusic.Play(true);
			}

			#if DEBUG
			this._imgStageUnlock.Locked = !SnailsGame.GameSettings.AllStagesUnlocked;
			#endif
			SnailsGame.LoadThumbnails ();
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
				this.DisableInput ();
				this._stageInfo.Visible = false;
				this._btnYouTube.Visible = false;
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
			this._themesPanel.AcceptControllerInput = true;
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
			this._stageInfo.Visible = (this._inputController.WithTouch);
			this._stageInfo.ShowHideTapMessage(true);
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
			//   BrainGame.ResourceManager.Unload(ResourceManagerIds.STAGE_THUMBNAILS);
		}

		/// <summary>
		/// 
		/// </summary>
		void _stagesPanel_OnShow(IUIControl sender)
		{
			this.EnableInput();
			this._stagesPanel.FocusOnLastUnlocked();
			this._state = ScreenState.StageSelection;
			if (this._selectedStage != null) 
			{
				this._selectedStage.Selected = false;
			}
			this._selectedStage = null;
		}

		/// <summary>
		///  
		/// </summary>
		private void StagesPanel_OnStageSelected(IUIControl sender)
		{
			UIStage stage = (UIStage)sender;
			if (this.InputController.WithTouch) 
			{
				if (this._selectedStage != stage) 
				{
					if (this._selectedStage != null)
					{
						this._selectedStage.Selected = false;
					}
					if (stage.Locked == false) 
					{
						this.ShowStageInfo (stage);
					}
					this._selectedStage = stage;
					stage.Selected = true;
					return;
				}
			}

			if (SnailsGame.ThemeMusic != null && SnailsGame.ThemeMusic.IsPlaying)
			{
				BrainGame.MusicManager.FadeMusic(0, AudioTags.MUSIC_FADE_MSECONDS);
			}

			this.DisableInput();
			this._stagesPanel.RaiseStageLeaveEvent = false;

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
			if (!this._inputController.WithMouse) {
				return;
			}
			UIStage stage = (UIStage)sender;
			if (stage.Locked == false) {
				this.ShowStageInfo(stage);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		void _stagesPanel_OnStageLeave(IUIControl sender)
		{
			if (!this._inputController.WithMouse) {
				return;
			}
			UIStage stage = (UIStage)sender;
			if (this._stageInfo.Visible)
			{
				if (this._stageInfo.StageInfo.StageNr == stage.StageNr)
				{
					this._stageInfo.Visible = false;
					this._btnYouTube.Visible = false;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void _stagesPanel_OnBack(IUIControl sender)
		{
		}

		/// <summary>
		/// Shows the stage info.
		/// </summary>
		private void ShowStageInfo(UIStage stage)
		{
			LevelStage levelStage = this.Levels.GetLevelStage (this._themesPanel.SelectedTheme.ThemeId, stage.StageNr);
			stage.LevelStageInfo = levelStage;
			this._stageInfo.Show (levelStage);
			this._btnYouTube.Visible = ThemeSelectionLDScreen.CheckShowYouTubeButton(stage.LevelStageInfo);
			this.EnableInput ();
		}

		/// <summary>
		/// 
		/// </summary>
		private void stageInfo_OnStartLoadingThumbnails(IUIControl sender)
		{
			// Isto é para mostrar o icon de loading no stage info quando tem leitura sincrona
			this.DisableInput ();
		}

		/// <summary>
		/// 
		/// </summary>
		private void stageInfo_OnEndLoadingThumbnails(IUIControl sender)
		{
			// Isto é para mostrar o icon de loading no stage info quando tem leitura sincrona
			this.EnableInput ();
		}
		/// <summary>
		/// 
		/// </summary>
		void stageInfo_OnAccept(IUIControl sender)
		{
			if (this._selectedStage != null) {
				StagesPanel_OnStageSelected (this._selectedStage);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void btnYouTube_OnClick(IUIControl sender)
		{
			if (this._selectedStage == null)
			{
				return;
			}
			SnailsGame.OpenUrlInBrowser(this._selectedStage.LevelStageInfo.YouTubeUrl);
		}
		#endregion

	}
}
