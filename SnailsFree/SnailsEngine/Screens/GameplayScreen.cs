using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Configuration;

namespace TwoBrainsGames.Snails.Screens
{
	public class GameplayScreen : TwoBrainsGames.BrainEngine.UI.Screens.Screen
    {
        #region Members
        #endregion

        #region Properties
        public Levels Levels { get; private set; }
		public GameplayInput Input;
		static GameplayScreen _Instance;
		public static GameplayScreen Instance { get { return GameplayScreen._Instance; } }
        public Stage CurrentStage { get { return this.Levels.Stage; } }
		#endregion

        public GameplayScreen(ScreenNavigator owner) :
            base(owner)
		{
			GameplayScreen._Instance = this;
            this.OnScreenModeChanged += new BrainEngine.UI.Controls.UIControl.UIEvent(GameplayScreen_OnScreenModeChanged);
            this.OnGameLostFocus += new BrainEngine.UI.Controls.UIControl.UIEvent(GameplayScreen_OnGameLostFocus);
            this.OnOpenTransitionEnded += new BrainEngine.UI.Controls.UIControl.UIEvent(GameplayScreen_OnOpenTransitionEnded);
        }
 
        /// <summary>
        /// 
        /// </summary>
        void GameplayScreen_OnOpenTransitionEnded(BrainEngine.UI.Controls.IUIControl sender)
        {
            this.Levels.Stage.OnOpenTransitionEnded();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
		{
			this._inputController = new GameplayInput();
			this._inputController.Initialize();
			this.Input = (GameplayInput)this._inputController;

            BrainGame.ResourceManager.Load<DataFileRecord>("stages/levels", ResourceManager.ResourceManagerCacheType.Static);

            // Levels is a singleton loaded in the ThemeSelectionScreen.
            // If we start the game directly from the StageStartScreen, they will not be loaded
            this.Levels = Levels.Load();
		}

		/// <summary>
		/// 
		/// </summary>
        public override void OnStart()
		{
            this._inputController.Reset();
            // take pause off
            this.Levels.Stage.IsPaused = false;
            this.Levels.Stage.Start();
            BrainGame.GameCursor.Visible = SnailsGame.GameSettings.ShowCursor;
            
            if (SnailsGame.GameSettings.UseGamepad)
            {
                this.CenterCursor();
            }

#if STAGE_EDITOR
            if (SnailsGame.GameSettings.EntryPoint == GameSettings.GameEntryPoint.StageEditor)
            {
                SnailsGame.GameSettings.EntryPoint = GameSettings.GameEntryPoint.StageBriefing;
            }
#endif
            BrainGame.SampleManager.UseAudibleBoundingSquare = true;

		}

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
		{
#if STAGE_EDITOR

            if (StageEditor.StageEditor.IsActive)
            {
                return;
            }

            if (this.Input.ActionLoadCustomStage &&
                SnailsGame.IsFullscreen == false)
            {
                this.Navigator.DrawEnabled = false;
                if (StageEditor.StageEditor.Instance.LoadCustomLevel() == System.Windows.Forms.DialogResult.OK)
                {
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, true);
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.SELECTED_STAGE_INFO, StageEditor.StageEditor.Instance.CurrentLevelStage);
                    this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_CHANGED_BY_THE_EDITOR, true);
                    this.NavigateTo(ScreenType.StageStart.ToString());
                }
                this.Navigator.DrawEnabled = true;
            }

            if (this.Input.ActionStageEditor && 
                SnailsGame.IsFullscreen == false)
            {
                this.Navigator.DrawEnabled = false;
                StageEditor.StageEditor.Instance.CurrentLevelStage = Stage.CurrentStage.LevelStage;
                StageEditor.StageEditor.Instance.Show(Levels.CurrentLevel);
                this.Navigator.DrawEnabled = true;
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, true);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.SELECTED_STAGE_INFO, StageEditor.StageEditor.Instance.CurrentLevelStage);
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_CHANGED_BY_THE_EDITOR, true);
                this.NavigateTo(ScreenType.StageStart.ToString());
                return;
            }
#endif
			this.Levels.HandleEvents(gameTime);
			this.Levels.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDraw()
		{
			this.Levels.Draw();
		}

		public override void OnUnload()
		{
			this.Levels.UnloadContent();
            this.Levels = Levels._instance = null; // Clear Levels._instance
		}

        /// <summary>
        /// 
        /// </summary>
        void GameplayScreen_OnScreenModeChanged(BrainEngine.UI.Controls.IUIControl sender)
        {
      /*      if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.ScreenModeChanged();
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        void GameplayScreen_OnGameLostFocus(BrainEngine.UI.Controls.IUIControl sender)
        {
            if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.GameLostFocus();
            }
        }

	}
}
