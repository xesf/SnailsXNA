using System;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Screens
{
	class SnailsScreen : UIScreen
	{
		#region Consts
		private const string CURSOR_SPRITE = "DefaultCursor";
		protected const float CAPTIONS_GRADIENT_INCREMENT = 0.90f;
		#endregion

		#region Events
		public event EventHandler OnBlurEffectFadeEnded;
		public event EventHandler OnBlurEffectEnded;
		#endregion

		protected enum ScreenBackgroundType
		{
			None,
			Image,
			Leafs
		}


		#region Vars
		protected UIInstructionBar _ibInstBar;
		protected UIAsync _asyncLoad;
		private bool _withBlurEffect;
		private Transition _pauseTransition;
		protected ScreenBackgroundType _backgroundType;
		private LeafTransition _leafs;
		protected ScreenType _screenType;
		private UIImage _imgRate;
		#endregion

		#region Properties
		public static float BottomMargin
		{
			get
			{
				return PixelsToScreenUnitsY_(BrainGame.Ad.HeightInPixelsIfVisible);
			}
		}

		public static float BottomMarginInPixels
		{
			get
			{
				return BrainGame.Ad.HeightInPixelsIfVisible;//(SnailsScreen.ScreenUnitToPixelsY_(SnailsScreen.BottomMargin));
			}
		}

		public UIInstructionBar InstructionBar { get { return this._ibInstBar; } }
		protected UIFooterMessage FooterMessage { get; set; }
		protected bool WithBlurEffect
		{
			get { return this._withBlurEffect; }
			set
			{
				this._withBlurEffect = value;
			}
		}

		protected ScreenBackgroundType BackgroundType
		{
			get { return this._backgroundType; }
			set
			{
				this._backgroundType = value;
				switch (this._backgroundType)
				{
				case ScreenBackgroundType.None:
					this.BackgroundImage = null;
					break;

				case ScreenBackgroundType.Image:
					this.BackgroundImage = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/menus-background", "MenusBackground");
					this.BackgroundImageMode = BrainEngine.UI.ScreenBackgroudImageMode.FitToScreen;
					break;

				case ScreenBackgroundType.Leafs:
					if (this._leafs == null)
					{
						this._leafs = new LeafTransition(LeafTransition.State.ClosedStopped);
						this._leafs.LoadContent();
					}
					this.BackgroundImage = null;
					break;
				}
			}
		}
		protected bool ShowRateTag
		{
			get;
			set;
		}

		#endregion

		#if DEBUG
		// Used to test colors on labels
		UICaption _capColor;
		UIControl _connectedCaption;
		protected UIControl ConnectedCaption
		{
			set
			{
				this._connectedCaption = value;
				if (value != null)
				{
					this._captionColor = this._connectedCaption.BlendColor;
				}
			}
		}
		Color _captionColor = Color.White;
		#endif

		/// <summary>
		/// 
		/// </summary>
		public SnailsScreen(ScreenNavigator owner, ScreenType screenType) :
		base(owner)
		{
			this._screenType = screenType;
		}

		public bool UseAudibleSoundsBoundingBox { get; set; }

		#region Screen events

		/// <summary>
		/// 
		/// </summary>
		public override void OnLoad()
		{
			this.BackgroundType = ScreenBackgroundType.None;
			this.CursorMode = SnailsGame.GameSettings.MenuCursorMode;

			// Instructions bar
			this._ibInstBar = new UIInstructionBar(this);
			if (SnailsGame.GameSettings.ShowTheInstructionBar)
			{
				this.Controls.Add(this._ibInstBar);
			}

			// Async 
			this._asyncLoad = new UIAsync(this);
			this.Controls.Add(this._asyncLoad);


			// Rate
			this._imgRate = new UIImage(this, "spriteset/common-elements-1/RateIconEN", ResourceManager.RES_MANAGER_ID_STATIC);
			this._imgRate.Position = new Vector2(850f, 850f);
			if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.HD)
			{
				this._imgRate.Effect = new ScaleEffect(new Vector2(1f, 1f), 0.2f, new Vector2(0.95f, 0.95f), true);
			}
			else
			{
				this._imgRate.Scale = new Vector2(0.7f, 0.7f);
				this._imgRate.Effect = new ScaleEffect(this._imgRate.Scale, 0.2f, new Vector2(0.65f, 0.65f), true);
			}
			this._imgRate.Rotation = -15f;
			this._imgRate.AcceptControllerInput = true;
			this._imgRate.OnAccept += new UIControl.UIEvent(_imgRate_OnAccept);
			this._imgRate.OnLanguageChanged += new UIControl.UIEvent(_imgRate_OnLanguageChanged);
			this.Controls.Add(this._imgRate);

			// Footer message
			this.FooterMessage = new UIFooterMessage(this);
			this.FooterMessage.Visible = false; // This is only displayed in main screen and in options screen so better make default to false
			if (SnailsGame.GameSettings.ShowFooterMessage)
			{
				this.Controls.Add(this.FooterMessage);
			}
			#if DEBUG
			// Debg purposes, used to display a color r,g,b
			this._capColor = new UICaption(this, "", Color.White, UICaption.CaptionStyle.NormalText);
			//            this.Controls.Add(this._capColor);
			#endif

			this.OnBeforeControlsDraw += new UIControl.UIEvent(SnailsScreen_OnBeforeControlsDraw);

			this.UseAudibleSoundsBoundingBox = false;
			this.ShowRateTag = false;
			this.InitializeRateIconForLocale();
		}


		/// <summary>
		/// 
		/// </summary>
		private void InitializeRateIconForLocale()
		{
			string res = LanguageManager.GetString("IMG_RATE");
			this._imgRate.Sprite = BrainGame.ResourceManager.GetSpriteStatic(res);
		}

		/// <summary>
		/// 
		/// </summary>
		void _imgRate_OnLanguageChanged(IUIControl sender)
		{
			this.InitializeRateIconForLocale();
		}    

		/// <summary>
		/// 
		/// </summary>
		void _imgRate_OnAccept(IUIControl sender)
		{
			SnailsGame.OpenStoreReviewPanel();
		}
		/// <summary>
		/// 
		/// </summary>
		void SnailsScreen_OnBeforeControlsDraw(IUIControl sender)
		{
			if (this.BackgroundType == ScreenBackgroundType.Leafs)
			{
				this._leafs.Draw(this.SpriteBatch);
			}

			if (this._pauseTransition != null)
			{
				this._pauseTransition.Draw(); 
			}


		}


		/// <summary>
		/// 
		/// </summary>
		public override void OnStart()
		{
			base.OnStart();

			if (this._leafs != null)
			{
				this._leafs.Initialize();
			}

			if (this.WithBlurEffect)
			{
				if (SnailsGame.GameSettings.SupportsShaderEffects)
				{
					this._pauseTransition = new SnailsBlurTransition(this, false);
				}
				else
				{
					this._pauseTransition = new SnailsGrayoutTransition(this, false);

				}
				this._pauseTransition.LoadContent();
				this._pauseTransition.Initialize();
				this._pauseTransition.OnTransitionEnded += new EventHandler(_pauseTransition_OnBlurEnded);
			}

			// only used for DEBUG to unsure we always have a profile created no matter what screen we start
			#if DEBUG // I added this, not sure if I should...
			//    SnailsGame.ProfilesManager.LoadPlayerProfileForDEBUG();
			#endif
			BrainGame.ClearColor = Colors.BBDefaultColor;
			BrainGame.DisplayHDDAccessIcon = true;

			this.FooterMessage.Initialize();
			if (this._pauseTransition != null)
			{
				this._pauseTransition.Reset();
			}


			this.Navigator.GlobalCache.Set(GlobalCacheKeys.CURRENT_SCREEN, this._screenType);

			BrainGame.SampleManager.UseAudibleBoundingSquare = this.UseAudibleSoundsBoundingBox;

			this._imgRate.Visible = (this.ShowRateTag && TwoBrainsGames.BrainEngine.RemoteServices.Network.IsInternetAvailable);
			this._imgRate.BringToFront();

#if IOS
			if (BrainGame.Settings.UseAds) {
				BrainGame.Ad.Show ();
			}
#endif
		}

		#endregion


		/// <summary>
		/// 
		/// </summary>
		protected void FadeBlurOut()
		{
			if (this._pauseTransition != null)
			{
				this._pauseTransition.TransitionOut();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public void DisableInput()
		{
			this.InstructionBar.HideAllLabels();
			this.AcceptControllerInput = false;
			BrainGame.GameCursor.Visible = (this.CursorMode != BrainEngine.UI.CursorModes.SnapToControl);
			if (!SnailsGame.GameSettings.ShowCursor)
			{
				BrainGame.GameCursor.Visible = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void EnableInput()
		{
			BrainGame.GameCursor.SetCursor(GameCursors.Default);
			BrainGame.GameCursor.Visible = SnailsGame.GameSettings.ShowCursor;
			this.InstructionBar.ShowLabel(CommonControls.UIInstructionLabel.LabelActionTypes.Accept);
			this.InstructionBar.ShowLabel(CommonControls.UIInstructionLabel.LabelActionTypes.Back);
			this.AcceptControllerInput = true;
		}

		protected virtual void EnableInput(bool hideCursor)
		{
			BrainGame.GameCursor.Visible = hideCursor;
			EnableInput();
		}




		/// <summary>
		/// 
		/// </summary>
		public override void OnUpdate(BrainEngine.BrainGameTime gameTime)
		{

			if (this._pauseTransition != null)
			{
				this._pauseTransition.Update(gameTime);
			}

			SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalRunningTime += gameTime.ElapsedGameTime;

			// A flag IsInternetAvailable pode ter mudado porque a query à rede é assincrona
			this._imgRate.Visible = (SnailsGame.GameSettings.SupportsRating && this.ShowRateTag && TwoBrainsGames.BrainEngine.RemoteServices.Network.IsInternetAvailable);

		}

		#region Blur transition effects

		/// <summary>
		/// This is all too confusing
		/// BlurTransition and GrayouTransition do the effect and the remove the effect in 2 steps
		/// Transitions shouldn't do this. To remove the transition effect, there should be another transition to remove
		/// (maybe they should, but they weren't designed like this
		/// The transitionOut flag indicates if the transition was removed
		/// </summary>
		void _pauseTransition_OnBlurEnded(object sender, EventArgs e)
		{
			bool transitionOut = ((ISnailsPauseTransition)this._pauseTransition).IsTransitionOut;

			if (transitionOut == false)
			{
				// This event happens when the transition is applied
				if (this.OnBlurEffectEnded != null)
				{
					this.OnBlurEffectEnded(this, e);
				}
			}
			else
			{// This event happens when the transition is removed
				if (this.OnBlurEffectFadeEnded != null)
				{
					this.OnBlurEffectFadeEnded(this, e);
				}
			}
		}
		#endregion


		public void NavigateToPurchase()
		{
			this.NavigateTo("MainMenu", ScreenType.Purchase.ToString(), ScreenTransitions.LeafsClosing, null);
		}
	}
}
