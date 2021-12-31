using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;
#if BETA_TESTING
using TwoBrainsGames.BrainEngine.Beta;
#endif

namespace TwoBrainsGames.BrainEngine.UI.Screens
{
  

    public class Screen
    {

        public event UIControl.UIEvent OnOpenTransitionEnded;
        public event UIControl.UIEvent OnScreenModeChanged;
        public event UIControl.UIEvent OnGameLostFocus;
        public event UIControl.UIEvent OnGameGotFocus;
        public event UIControl.UIEvent OnLanguageChanged;
        public event UIControl.UIEvent OnGameplayModeChanged;
        public event UIControl.UIEvent OnPopupClosed; // Occurs when a pop up that was opened on top of the screen was closed
        public event UIControl.UIEvent OnInitializeFromContent;
 

        #region Vars
        internal string _id;
        internal bool _closed;
        internal bool _active;  // When the stage is "active", OnUpdate is called
                                // If there are transitions, the stage is not "active" when transitions are running
        internal bool _started; // When the stage is "started", OnDraw is called
                                // If there are transactions, the stage is "started" when transitions are running
        internal bool _skipped;  // To know if the current screen as been _skipped
        private ScreenNavigator _navigator;
        public SpriteBatch _spriteBatch;
        public InputBase _inputController;
        internal float _skipTime;
        internal float _elapsedTime;
        #endregion

        #region Properties
        public string Name { get; set; }
        public float TimeMultiplier { get; set; }
        public bool Closed { get { return this._closed; } }
        public bool IsActive { get { return this._active; } }
        public bool IsSkipped { get { return this._skipped; } }
        public ScreenNavigator Navigator { get { return this._navigator; } }
        internal DataFileRecord ScreenContentRootRecord { get; set; }

        public SpriteBatch SpriteBatch
        {
            get { return this._spriteBatch; }
        }
        internal bool Starting { get; set; }

		#endregion

        public Screen(ScreenNavigator navigator)
        {
            this._navigator = navigator;
            this._active = false;
            this._skipped = false;
            this.TimeMultiplier = 1f;
        }

		/// <summary>
		/// 
		/// </summary>
        internal void Initialize(ScreenNavigator navigator)
        {
            this._navigator = navigator;
            this._inputController = new InputBase();
            this._inputController.Initialize();
            this._spriteBatch = new SpriteBatch(BrainGame.Graphics);
		}

        /// <summary>
        /// this method should only run once in the cycle of this screen
        /// </summary>
        internal virtual void Load()
        {
            this.OnLoad();
            this.OnAfterLoad();
        }
         
        /// <summary>
        /// 
        /// </summary>
        internal virtual void InitializeFromContent()
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                this.ScreenContentRootRecord = BrainGame.ResourceManager.Load<DataFileRecord>(Path.Combine(BrainGame.Settings.NavigatorContentFolder, this.Name), ResourceManager.ResourceManagerCacheType.Temporary);
            }
        }

        /// <summary>
        /// this method should only run once in the cycle of this screen
        /// </summary>
        internal virtual void Start()
        {
#if BETA_TESTING
			ClosedBeta.LogInfo("Screen [" + this._id + "] started");
#endif
            this.Starting = true;
            this._active = true;
            this._closed = false;
            this._skipped = false;
            this._started = true;

            // Update positions for all controls
            this.OnStart();
            this.OnAfterStart();
            this.Starting = false;
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void Draw()
        {
            this.OnDraw();
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void Update(BrainGameTime gameTime)
        {
            this.OnUpdate(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void ScreenModeChanged()
        {
            if (this.OnScreenModeChanged != null)
            {
                this.OnScreenModeChanged(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void GameLostFocus()
        {
            if (this.OnGameLostFocus != null)
            {
                this.OnGameLostFocus(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void GameGotFocus()
        {
            if (this.OnGameGotFocus != null)
            {
                this.OnGameGotFocus(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void InternalLanguageChanged()
        {
            if (this.OnLanguageChanged != null)
            {
                this.OnLanguageChanged(null);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal virtual void InternalGameplayModeChanged()
        {
            if (this.OnGameplayModeChanged != null)
            {
                this.OnGameplayModeChanged(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Close()
        {
            this._closed = true;
            this._active = false;
            this.Navigator.PopUpClosed(this);
        }

        /// <summary>
        /// Occurs when screen group is loaded
        /// </summary>
        public virtual void OnLoad()
        { }

        /// <summary>
        /// Occurs after screen is loaded
        /// </summary>
        public virtual void OnAfterLoad()
        { }

        /// <summary>
        /// Occurs once before the screen opens
        /// </summary>
        public virtual void OnStart()
        { }

        /// <summary>
        /// Occurs after screen opens
        /// </summary>
        public virtual void OnAfterStart()
        { }

        /// <summary>
        /// Occurs once after the screen closes
        /// </summary>
        public virtual void OnClose()
        { }

        /// <summary>
        /// Occurs when screen group is unloaded
        /// </summary>
        public virtual void OnUnload()
        { }


        /// <summary>
        /// Occurs once every loop. Do any screen logic here
        /// </summary>
        public virtual void OnUpdate(BrainGameTime gameTime)
        { }

        /// <summary>
        /// Occurs once every loop. Draw the screen components here
        /// </summary>
        public virtual void OnDraw()
        { }

        /// <summary>
        /// 
        /// </summary>

        public void NavigateTo(string groupId, string screenId)
        {
            this._active = false;
            this._navigator.NavigateTo(groupId, screenId);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NavigateTo(string screenId)
        {
            this._active = false;
            this._navigator.NavigateTo(screenId);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NavigateTo(string screenId, Transition closeTransition, Transition openTransition)
        {
            this._active = false;
            this._navigator.NavigateTo(screenId, closeTransition, openTransition);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NavigateTo(string groupId, string screenId, Transition closeTransition, Transition openTransition)
        {
            this._active = false;
            this._navigator.NavigateTo(groupId, screenId, closeTransition, openTransition);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PopUp(string screenId, bool currentRemainActive)
        {
            this._active = currentRemainActive;
            this._navigator.PopUp(screenId);
        }

        public override string ToString()
        {
            return (this._id != null? this._id : "");
        }

        /// <summary>
        /// 
        /// </summary>
        public void LauchOnOpenTransitionEnded()
        {
            if (this.OnOpenTransitionEnded != null)
            {
                this.OnOpenTransitionEnded(null); // This sould not be null, but i dont feel like creating a new event type
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CenterCursor()
        {
            int x = (BrainGame.ScreenWidth / 2);
            int y = (BrainGame.ScreenHeight / 2);

            // This is at the least STRANGE

            // Set motion position does nothing in the XBox (only affects the mouse)
            this._inputController.SetMotionPosition(new Vector2(x, y));
            // This stragenly does not affect the mouse cursor, but affects the xbox cursor
            BrainGame.GameCursor.Position = new Vector2(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InvokeInitializeFromContent()
        {
            if (OnInitializeFromContent != null)
            {
                this.OnInitializeFromContent(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InvokePopUpClosed()
        {
            if (this.OnPopupClosed != null)
            {
                this.OnPopupClosed(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected T ReadCustomContentField<T>(string dataFileRecordPath, string name)
        {
            if (this.ScreenContentRootRecord == null)
            {
                throw new BrainException("Screen.InitializeFromContent() not called. Set Screen.Name to a valid xdf file in the game content.");
            }
            DataFileRecord record = this.ScreenContentRootRecord.SelectRecord(dataFileRecordPath);
            if (record == null)
            {
                throw new BrainException("Screen content record with path [" + dataFileRecordPath + "] not found.");
            }

            DataFileField field = record.GetFieldByName(name);
            if (field == null)
            {
                throw new BrainException("Screen content field [" + name + "] in record with path [" + dataFileRecordPath + "] not found.");
            }

            return record.GetFieldValue<T>(name);
        }

        /// <summary>
        /// 
        /// </summary>
        protected T ReadCustomContentField<T>(string dataFileRecordPath, string name, string conditionFieldName, object conditionFieldValue)
        {
            if (this.ScreenContentRootRecord == null)
            {
                throw new BrainException("Screen.InitializeFromContent() not called. Set Screen.Name to a valid xdf file in the game content.");
            }
            DataFileRecord record = this.ScreenContentRootRecord.SelectRecordByField(dataFileRecordPath, conditionFieldName, conditionFieldValue);
            if (record == null)
            {
                throw new BrainException("Screen content record with path [" + dataFileRecordPath + "] not found.");
            }

            DataFileField field = record.GetFieldByName(name);
            if (field == null)
            {
                throw new BrainException("Screen content field [" + name + "] in record with path [" + dataFileRecordPath + "] not found.");
            }

            return record.GetFieldValue<T>(name);
        }

	}
}
