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
#if APP_STORE_SUPPORT
using Microsoft.Phone.Tasks;
#endif

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

        private bool _loadStarted;
        private bool _canStartLoading;
		bool _isLoadingSync;
		int _ellapsedLoadingTime;

        #region Properties
        ScreenState State { get; set; }
        LevelStage LevelStageInfo { get; set; }
        UILoadingHint _loadHintMessage { get; set; }
        UIImage _imgPaidSnailsAd { get; set; }
        bool TipMessageVisible
        {
            get
            {
                return this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.SHOW_TIP_ON_LOADING, false);
            }
        }

        private bool ShouldShowRateDialog
        {
            get
            {
				#if IOS
				return false;
				#else
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
                    return false;
                }

                // Only if network is available
                if (TwoBrainsGames.BrainEngine.RemoteServices.Network.IsInternetAvailable == false)
                {
                    return false;
                }

                return true;
				#endif

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
            this.BackgroundType = ScreenBackgroundType.Leafs;
            this._asyncLoad.OnAsyncOperationEnded += new UIControl.UIEvent(_asyncLoad_OnAsyncOperationEnded);

            this._loadHintMessage = new UILoadingHint(this);
            this.Controls.Add(this._loadHintMessage);

            this._imgPaidSnailsAd = new UIImage(this);
            this._imgPaidSnailsAd.OnAccept += new UIControl.UIEvent(_imgPaidSnailsAd_OnAccept);
            this._imgPaidSnailsAd.AcceptControllerInput = true;
			this._imgPaidSnailsAd.ParentAlignment = AlignModes.HorizontalyVertically;
            this.Controls.Add(this._imgPaidSnailsAd);

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
        void _imgPaidSnailsAd_OnAccept(IUIControl sender)
        {
            SnailsGame.RedirectToPaidSnailsOnStore();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
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
		public override void OnUpdate(BrainGameTime gameTime)
		{
			base.OnUpdate(gameTime);

			if (this._isLoadingSync) {
				this._ellapsedLoadingTime += (int)gameTime.ElapsedRealTime.TotalMilliseconds;
				LoadStageSync ();
				return;
			}
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


        /// <summary>
        /// 
        /// </summary>
        private void ShowTip()
        {
            if (this.TipMessageVisible)
            {
                if (SnailsGame.GameSettings.UseAds)
                {
                    this._imgPaidSnailsAd.Visible = (SnailsGame.ProfilesManager.CurrentProfile.LastHintMessageSeen % 5 == 0);
                    if (this._imgPaidSnailsAd.Visible &&
                        this.Navigator.GlobalCache.Get<bool>(GlobalCacheKeys.SNAILS_PAID_AD_SEEN_ON_LOADING, false))
                    {
                        this._imgPaidSnailsAd.Visible = false;
                    }
                }

                if (!this._imgPaidSnailsAd.Visible)
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
                    this._imgPaidSnailsAd.Sprite = BrainGame.ResourceManager.GetSprite("spriteset/snails-paid-ad/SnailsPaidAd", ResourceManagerIds.PAID_SNAILS_AD);
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        void StartStage(bool loadSolution, bool saveSolution)
        {
            BrainGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.GAMEPLAY_RECORDER_LOAD_SOLUTION, loadSolution);
            BrainGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.GAMEPLAY_RECORDER_SAVE_SOLUTION, saveSolution);

            this.State = ScreenState.StartGame;
            this.DisableInput();
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
			if (!this._isLoadingSync) // primeira vez que aqui chega, inicializa 
			{
				this._isLoadingSync = true;
				BrainGame.ScreenNavigator.ShowLoadingIcon ();
				this._ellapsedLoadingTime = 0;

			} 
			else 
			{
				Levels._instance.BeginLoad();
				SnailsGame.Tutorial.BeginLoad();
				if (this._ellapsedLoadingTime > 2000 || (!this.TipMessageVisible)) {
					this.LoadEnded ();
					BrainGame.ScreenNavigator.HideLoadingIcon ();
				}
			}
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
            this.StartGame();
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartGame()
        {
            this._imgPaidSnailsAd.Visible = false;
            BrainGame.ResourceManager.Unload(ResourceManagerIds.PAID_SNAILS_AD);
            this.NavigateTo(ScreenType.Gameplay.ToString(), null, ScreenTransitions.LeafsOpening);
        }

	}
}
