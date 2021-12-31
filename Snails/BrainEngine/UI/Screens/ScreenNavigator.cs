using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using System.Collections;
using System.Threading;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI.Controls;
using System.IO;

namespace TwoBrainsGames.BrainEngine.UI.Screens
{
    public class ScreenNavigator
    {
        enum TransitionType
        {
            Open,
            Close
        }

        enum ScreenActionType
        {
            PopUp,
            Close,
            NavigateTo,
            GroupLoad,
            OpenTransition,
            CloseTransition,
            RemoveTransition
        }

        class ScreenAction
        {
            public ScreenActionType _actionType;
            public string _screenId;
            public object _param;
            public bool _syncronous; // In a sycronous action, the loop in PostQueuedScreenActions breaks in the action

            public ScreenAction(ScreenActionType actionType, string screenId, object param) :
                this(actionType, screenId, param, false)
            {
            }

            public ScreenAction(ScreenActionType actionType, string screenId, object param, bool syncronous)
            {
                this._actionType = actionType;
                this._screenId = screenId;
                this._param = param;
                this._syncronous = syncronous;
            }

        }

        BrainGame _game;
        ScreensData _screensData;
        List<Screen> _screens;
        List<Screen> _activeScreens;
        List<ScreenAction> _queuedScreenActions;
        string _currentGroupId;
        int LastScreenIdx { get { return _activeScreens.Count - 1; } }
        Transition _transition;
        TransitionType _currentTransitionType;

#if !WIN8
	    Thread _loadingThread;
#endif
        public bool Enabled { get; private set; }
     //   bool _previousGameActiveState;
        #region Properties

        public InputBase InputController { get; private set; }

        public SpriteBatch SpriteBatch
        {
            get { return BrainGame.SpriteBatch;  }
        }

        public ScreenGlobalCache GlobalCache { get; private set; }
            
        // Assync loading
        public bool UseAssyncGroupLoading { get; set; }
        private bool _isLoadingGroup = false;
        public bool StencilBufferEnabled { get; set; }
        public bool DrawEnabled { get; set; }
        
        public Screen ActiveScreen
        {
            get
            {
                if (this.LastScreenIdx >= 0 && this.LastScreenIdx < this._activeScreens.Count)
                {
                    return (this._activeScreens[this.LastScreenIdx]);
                }
                return null;
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        public ScreenNavigator(BrainGame game)
        {
            this._queuedScreenActions = new List<ScreenAction>();
            this._game = game;
            this.GlobalCache = new ScreenGlobalCache();
            this.InputController = new InputBase();
            this.InputController.Initialize();
            this.DrawEnabled = true;
            this.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            if (string.IsNullOrEmpty(BrainGame.Settings.NavigatorContentId))
            {
                throw new BrainException("Error loading Navigator data. BrainGame.Settings.NavigatorContentId must be initialized");
            }
            if (string.IsNullOrEmpty(BrainGame.Settings.NavigatorContentFolder))
            {
                throw new BrainException("Error loading Navigator data. BrainGame.Settings.NavigatorContentFolder must be initialized");
            }
            this._activeScreens = new List<Screen>();
            this._screens = new List<Screen>();
            this._screensData = BrainGame.ResourceManager.Load<ScreensData>(Path.Combine(BrainGame.Settings.NavigatorContentFolder, BrainGame.Settings.NavigatorContentId), ResourceManager.ResourceManagerCacheType.Static);
        }


        /// <summary>
        /// 
        /// </summary>
        private void LoadGroupThread(object param)
        {
            string groupId = param.ToString();
            this.LoadGroupData((string)groupId);
            BrainGame.IsLoading = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGroup(string groupId)
        {
            this.UnloadCurrentGroup();

 		    if (this.UseAssyncGroupLoading)
 		    {
                if (BrainGame.HddAccessIcon != null)
                {
                    BrainGame.HddAccessIcon.Visible = true;
                }
#if !WIN8
                BrainGame.IsLoading = true;
                this._loadingThread = new Thread(new ParameterizedThreadStart(this.LoadGroupThread));
                this._isLoadingGroup = true;
			    this._loadingThread.Start(groupId);
#endif
            }
		    else
		    {
			    this.LoadGroupData(groupId);
		    }
    	}

        /// <summary>
        /// 
        /// </summary>
        private void LoadGroupData(string groupId)
        {
            foreach (ScreensData.ScreenData screenData in this._screensData._groupsData[groupId].ScreensData)
            {
                Screen screen = this._game.CreateScreen(this, screenData.ScreenId);
                if (screen == null)
                {
                    throw new BrainException("Could not create screen with Id [" + screenData.ScreenId + "]. Please check overriden method YourGame.CreateScreen()");
                }
                screen._id = screenData.ScreenId;
                screen._skipTime = screenData.SkipTime;
                screen.Initialize(this);
                screen.Load();
                screen.InitializeFromContent();
                this._screens.Add(screen);
            }
            this._currentGroupId = groupId;
        }


        /// <summary>
        /// 
        /// </summary>
        private void UnloadCurrentGroup()
        {
            if (this._currentGroupId == null)
            {
                return;
            }
            foreach (Screen screen in this._screens)
            {
                screen.OnUnload();
            }
            BrainGame.ResourceManager.UnloadTemporary();
            this._activeScreens.Clear();
            this._screens.Clear();
            this._currentGroupId = null;

            // Force a GC collect here, because we have unloaded a screen group, this way the
            // new group will open with a clean GC
            GC.Collect();
        }

        /// <summary>
        /// 
        /// </summary>
        public void NavigateToStartUp()
        {
            this.NavigateTo(this._screensData._startupGroup, this._screensData._startupScreenId);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NavigateTo(string screenId)
        {
            this.NavigateTo(this._currentGroupId, screenId);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NavigateTo(string screenId, Transition closeTransition, Transition openTransition)
        {
            this.NavigateTo(this._currentGroupId, screenId, closeTransition, openTransition);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NavigateTo(string groupId, string screenId, Transition closeTransition, Transition openTransition)
        {
            if (closeTransition != null)
            {
                this._queuedScreenActions.Add(new ScreenAction(ScreenActionType.CloseTransition, screenId, closeTransition, true));
            }
            this._queuedScreenActions.Add(new ScreenAction(ScreenActionType.GroupLoad, screenId, groupId, true));
            this._queuedScreenActions.Add(new ScreenAction(ScreenActionType.NavigateTo, screenId, groupId));
            if (openTransition != null)
            {
                this._queuedScreenActions.Add(new ScreenAction(ScreenActionType.OpenTransition, screenId, openTransition, true));
            }
            this._queuedScreenActions.Add(new ScreenAction(ScreenActionType.RemoveTransition, screenId, openTransition));
        }

        /// <summary>
        /// 
        /// </summary>
        public void NavigateTo(string groupId, string screenId)
        {
            this._queuedScreenActions.Add(new ScreenAction(ScreenActionType.GroupLoad, screenId, groupId, true));
            this._queuedScreenActions.Add(new ScreenAction(ScreenActionType.NavigateTo, screenId, groupId));
        }

        /// <summary>
        /// 
        /// </summary>
        public void PopUp(string screenId)
        {
            this._queuedScreenActions.Add(new ScreenAction(ScreenActionType.PopUp, screenId, null, true));
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessQueuedScreenActions(BrainGameTime gameTime)
        {

            // Remove disposed screens
            for (int i = 0; i < this._screens.Count; i++)
            {
                if (this._screens[i]._closed)
                {
                    if (this._activeScreens.Contains(this._screens[i]))
                    {
                        this._activeScreens.Remove(this._screens[i]);
                        this._screens[i].OnClose();
                    }
                }
            } 
            
            if (this._queuedScreenActions.Count > 0)
            {
                for(int i = 0; i < this._queuedScreenActions.Count; i++)
                {
                    this.ProcessScreenAction(this._queuedScreenActions[i], gameTime);
                    if (this._queuedScreenActions[i]._syncronous) // Syncronous actions break the loop. Next action should only run when current action ends
                    {
                        this._queuedScreenActions.Remove(this._queuedScreenActions[i]);
                        break;
                    }
                    this._queuedScreenActions.Remove(this._queuedScreenActions[i]);
                    i--;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        Screen FindScreen(string screenId)
        {
            foreach (Screen screen in this._screens)
            {
                if (screen._id == screenId)
                {
                    return screen;
                }
            }
            return null;
        }

        /// <summary>
        /// A popup was closed, so we have to make previous window active
        /// </summary>
        internal void PopUpClosed(Screen popUpScreen)
        {
            for (int i = 0; i < this._activeScreens.Count; i++)
            {
                if (this._activeScreens[i] == popUpScreen)
                {
                    if (i - 1 >= 0)
                    {
                        this._activeScreens[i - 1]._active = true;
                        this._activeScreens[i - 1].InvokePopUpClosed();
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendScreenModeChangedToScreens()
        {
            for (int i = 0; i < this._activeScreens.Count; i++)
            {
                if (this._activeScreens[i]._active)
                {
                    this._activeScreens[i].ScreenModeChanged();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendLostFocusMessageToScreens()
        {
            for (int i = 0; i < this._activeScreens.Count; i++)
            {
                if (this._activeScreens[i]._active)
                {
                    this._activeScreens[i].GameLostFocus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendGotFocusMessageToScreens()
        {
            for (int i = 0; i < this._activeScreens.Count; i++)
            {
                if (this._activeScreens[i]._active)
                {
                     this._activeScreens[i].GameGotFocus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void LanguageChanged()
        {
            foreach (Screen screen in this._screens)
            {
                screen.InternalLanguageChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void GameplayModeChanged()
        {
            foreach (Screen screen in this._screens)
            {
                screen.InternalGameplayModeChanged();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal void GameWindowActivated()
        {
            this.SendGotFocusMessageToScreens();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void GameWindowDeactivated()
        {
            this.SendLostFocusMessageToScreens();
        }

        #region IBrainComponent Members

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            if (this.Enabled == false)
            {
                return;
            }

            if (BrainGame.IsGameActive)
            {
                this.InputController.Update(gameTime);
                BrainGame.GameCursor.Update(gameTime);
            }

            if (BrainGame.DisplayHDDAccessIcon && 
                BrainGame.HddAccessIcon != null && 
                BrainGame.HddAccessIcon.Visible)
            {
                BrainGame.HddAccessIcon.Update(gameTime);
            }

            if (BrainGame.Settings.UseAchievements &&
                BrainGame.AchievementsManager.HasAchievementsInQueue)
            {
                BrainGame.AchievementsManager.Update(gameTime);
            }

            if (this._isLoadingGroup)
	        {
#if !WIN8
                this._isLoadingGroup = this._loadingThread.IsAlive;
#endif

                if (!this._isLoadingGroup)
                {
                    if (BrainGame.HddAccessIcon != null)
                    {
                        BrainGame.HddAccessIcon.Visible = false;
                    }

                    this.ProcessQueuedScreenActions(gameTime);
                }
                return;
	        }

            if (this._transition != null) // There's a screen transition, update the transition
            {                             // screen updates are not run if there's a fade transition for instance
                this._transition.Update(gameTime);
                if (this._transition._ended)
                {
                    if (this.LastScreenIdx >= 0 && this._currentTransitionType == TransitionType.Open)
                    {
                        this._activeScreens[this.LastScreenIdx].LauchOnOpenTransitionEnded();
                    }
                    this.ProcessQueuedScreenActions(gameTime);  // Go to next queued action only when transitions end
                }
            }
            else // Update the screen
            {
                this.ProcessQueuedScreenActions(gameTime);
                if (this.LastScreenIdx >= 0)
                {
                    if (BrainGame.IsGameActive)
                    {
                        this._activeScreens[this.LastScreenIdx]._inputController.Update(gameTime);
                    }

                    for (int i = 0; i < this._activeScreens.Count; i++)
                    {
                        Screen screen = this._activeScreens[i];
                        if (screen._active)
                        {
                            // allow screens to know if skip time as already passed
                            if (!screen._skipped && screen._skipTime != 0)
                            {
                                screen._elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (screen._elapsedTime >= screen._skipTime)
                                {
                                    screen._elapsedTime = 0;
                                    screen._skipped = true;
                                }
                            }
                            // The idea here, is instead of using a global multiplier, each screen has it's own
                            gameTime.SetMultiplier(screen.TimeMultiplier);

                            screen.Update(gameTime);
                           
                            screen._inputController.Reset();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Should return false to stop processing screen actions
        /// </summary>
        private void ProcessScreenAction(ScreenAction action, BrainGameTime gameTime)
        {
            switch (action._actionType)
            {
                case ScreenActionType.PopUp:
                    Screen popUpScreen = this.FindScreen(action._screenId);
#if DEBUG

                    if (popUpScreen == null)
                        throw new BrainException("Could not find screen with Id " + action._screenId);
                    if (popUpScreen._active == true)
                        throw new BrainException("Tried to pop up a screen [" + action._screenId + "] that is already on the active screens list.");
#endif
                    popUpScreen._started = false;
                    popUpScreen._active = true;
                    if (this._activeScreens.Count > this.LastScreenIdx) // Deactivate previous screen
                    {
                        this._activeScreens[this.LastScreenIdx]._active = false;
                    }
                    this._activeScreens.Add(popUpScreen);
                    popUpScreen._inputController.Reset();
                    popUpScreen.Start();
                    break;
                
                case ScreenActionType.GroupLoad:
                    if ((string)action._param != this._currentGroupId)
                    {
                        this.LoadGroup((string)action._param);
                    }
                    break;

                case ScreenActionType.NavigateTo:
                    Screen navigateScreen = this.FindScreen(action._screenId);
                    if (navigateScreen == null)
                    {
                        throw new BrainException("Could not find screen with Id " + action._screenId);
                    }

                    if (ActiveScreen != null)
                    {
                        this.ActiveScreen.OnClose();
                    }
                    this._activeScreens.Clear();
                    this._activeScreens.Add(navigateScreen);
                    navigateScreen.Start();
                    navigateScreen.Update(gameTime);
                    break;

                case ScreenActionType.OpenTransition:
                    this._transition = (Transition)action._param;
                    this._transition.Initialize();
                    this._currentTransitionType = TransitionType.Open;
                    this._transition.OnStart();
                    break;

                case ScreenActionType.CloseTransition:
                    this._transition = (Transition)action._param;
                    this._transition.Initialize();
                    this._currentTransitionType = TransitionType.Close;
                    this._transition.OnStart();
                    break;

                case ScreenActionType.RemoveTransition:
                    this._transition = null;
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            if (this.DrawEnabled == false || this.Enabled == false)
            {
                return;
            }

            BrainGame.Graphics.Clear(ClearOptions.Target, BrainGame.ClearColor, 0f, 0);
            if (!this._isLoadingGroup)
            {
                foreach (Screen screen in this._activeScreens)
                {
                    if (screen._started)
                    {
                        screen.Draw();

                    }
                }
            }
            
            if (this._transition != null) // There's a screen transition, update the transition
            {
                this._transition.Draw();
            }

            if (BrainGame.DisplayHDDAccessIcon && 
                BrainGame.HddAccessIcon != null && 
                BrainGame.HddAccessIcon.Visible)
            {
                this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
                BrainGame.HddAccessIcon.Draw(this.SpriteBatch);
                this.SpriteBatch.End();
            }

            if (BrainGame.Settings.UseAchievements &&
                BrainGame.AchievementsManager.HasAchievementsInQueue)
            {
                this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
                BrainGame.AchievementsManager.Draw(this.SpriteBatch);
                this.SpriteBatch.End();
            }

            if (BrainGame.GameCursor.Visible) 
            {
                BrainGame.GameCursor.Draw(this.SpriteBatch);
            }
           
        }



        /// <summary>
        /// 
        /// </summary>
        public void Enable()
        {
            this.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disable()
        {
            this.Enabled = false;
        }
        #endregion
    }
}
