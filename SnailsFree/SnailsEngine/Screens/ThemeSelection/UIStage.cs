using System;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Player;

namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
	class UIStage : UIControl
	{
		#region Consts
		const string IMG_LOCKER_RESOURCE = "spriteset/common-elements-1/LockerSmall";
		#endregion

		#region Events
		public event UIEvent OnPress;
		public event UIEvent OnDoublePress;
		#endregion

		#region Members
		private static Vector2 DEFAULT_SCALE = new Vector2(0.85f, 0.85f);
		public int StageNr { get; private set; }
		private bool _locked;
		private ThemeType _themeId;
		private UIButton _btnStage;
		private UIImage _imgBackground;
		private UIImage _imgLocker;
		private UIImage _imgLockerDemo;
		private UISnailsStageGoalIcon _goalIcon;
		private UICaption _capStageNr;
		private UIPanel _pnlContainer;
		private UISnailsMedal _medal;
		private Sample _focusSample;
		private Sample _selectedSample;
		private LevelStage _levelStageInfo;
		private bool _selected;
		ColorEffect _selectedEffect;
		#if DEBUG
		private UICaption _capStageId;
		#endif
		public bool AcceptOnEnterEvents = true;

		#endregion

		#region Properties

		public LevelStage LevelStageInfo
		{
			get
			{
				return this._levelStageInfo;
			}
			set
			{
				this._levelStageInfo = value;
				this.Refresh();
			}
		}

		public override BrainEngine.Collision.BoundingSquare BoundingBox
		{
			get
			{
				Vector2 pos = this._pnlContainer.AbsolutePositionInPixels + new Vector2(this._imgBackground.Sprite.BoundingBoxes[0].Left, 
					this._imgBackground.Sprite.BoundingBoxes[0].Top);
				return new BoundingSquare(pos, this._imgBackground.Sprite.BoundingBoxes[0].Width * this._pnlContainer.Scale.X,
					this._imgBackground.Sprite.BoundingBoxes[0].Height * this._pnlContainer.Scale.Y);
			}
		}
		public bool DoOnLeaveEffect { get; set; }

		public bool Locked
		{
			get
			{
				if (SnailsGame.IsTrial &&
					this._levelStageInfo != null &&
					this._levelStageInfo.AvailableInDemo == false &&
					!SnailsGame.GameSettings.AllStagesUnlocked)
				{
					return true;
				}


				return this._locked;
			}
			set
			{
				this._locked = value;
				this.Refresh();
			}
		}


		/// <summary>
		/// Setting the theme changes the Stage Icon 
		/// </summary>
		public ThemeType ThemeId 
		{
			get { return this._themeId; }
			set
			{
				this._themeId = value;
				this._btnStage.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/menu-elements-1/" + this._themeId.ToString() + "StageIcon");
			}
		}

		public Snails.MedalType Medal
		{
			get { return this._medal.Face; }
			set
			{
				this._medal.Face = value;
				switch (this._medal.Face)
				{
				case Snails.MedalType.None:
					this._medal.Visible = false;
					break;
				default:
					this._medal.Visible = true;
					break;
				}

			}
		}

		private Vector2 LockerPosition { get; set; }
		private Vector2 LockerDemoPosition { get; set; }
		private Vector2 EnterEffectScale { get; set; }

		public bool Selected
		{
			get { return this._selected; }
			set
			{
				this._selected = value;
				if (this._selected)
				{
					this.Effect = this._selectedEffect;
					this.Effect.Reset();
					this._pnlContainer.Scale = new Vector2(1.0f, 1.0f);
				}
				else
				{
					this.Effect = null;
					this._pnlContainer.Scale = DEFAULT_SCALE;
					this.BlendColor = (this.Locked? Color.Black : Color.White);
				}
				this.Refresh();
			}

		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public UIStage(UIScreen screenOwner, int stageNr):
		base(screenOwner)
		{
			this.StageNr = stageNr;
			this.Name = "Stage_" + stageNr.ToString();
			this.OnEnter += new UIEvent(this.Stage_OnEnter);
			this.OnLeave += new UIEvent(this.Stage_OnLeave);
			this.OnInitializeFromContent += new UIEvent(UIStage_OnInitializeFromContent);
			this.OnAfterInitializeFromContent += new UIEvent(UIStage_OnAfterInitializeFromContent);

			this._pnlContainer = new UIPanel(screenOwner);
			this._pnlContainer.Name = "_pnlContainer";
			this._pnlContainer.ParentAlignment = AlignModes.HorizontalyVertically;
			this._pnlContainer.ShowEffect = new SquashEffect(0.5f, 4.0f, 0.08f, this.BlendColor, UIStage.DEFAULT_SCALE);
			this._pnlContainer.HideEffect = new PopOutEffect(new Vector2(1.3f, 1.3f), 6.0f);
			this._pnlContainer.OnShow += new UIEvent(_pnlContainer_OnShow);
			this._pnlContainer.OnHide += new UIEvent(_pnlContainer_OnHide);
			this.Controls.Add(this._pnlContainer);

			this._imgBackground = new UIImage(screenOwner, "spriteset/boards/LightWoodTiny");
			this._imgBackground.ParentAlignment = AlignModes.HorizontalyVertically;
			this._pnlContainer.Controls.Add(this._imgBackground);

			this._btnStage = new UIButton(screenOwner);
			//   this._btnStage.PressEffect = new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White);
			//   this._btnStage.PressEffect._expirationTime = 150;
			// this._btnStage.OnPress += new UIEvent(_btnStage_OnPress);
			this._btnStage.OnDoublePress += new UIEvent(_btnStage_OnDoublePress);
			this._btnStage.OnBeforePress += new UIEvent(_btnStage_OnBeforePress);
			this._btnStage.Size = new Size(this._imgBackground.Size.Width - 140, this._imgBackground.Size.Height - 240);
			this._btnStage.SizeMode = ImageSizeMode.Center;
			this._btnStage.AnimateImage = false;
			this._pnlContainer.Controls.Add(this._btnStage);

			// Stage nr
			this._capStageNr = new UICaption(screenOwner, "", TwoBrainsGames.Snails.Colors.StageSelectionNumber, UICaption.CaptionStyle.StageSelectionStageNr);
			this._capStageNr.Text = (this.StageNr).ToString();
			this._capStageNr.ParentAlignment = AlignModes.Right | AlignModes.Bottom;
			this._capStageNr.Margins.Right = this._capStageNr.MeasureString().X + 50;
			this._capStageNr.Margins.Bottom = this._capStageNr.MeasureString().Y + 20;
			this._pnlContainer.Controls.Add(this._capStageNr);

			this._goalIcon = new UISnailsStageGoalIcon(screenOwner);
			this._goalIcon.IconSize = UISnailsStageGoalIcon.GoalIconSize.Small;
			this._goalIcon.Position = this.NativeResolution(new Vector2(-100, -100));
			this._pnlContainer.Controls.Add(this._goalIcon);

			// Medal
			this._medal = new UISnailsMedal(screenOwner, Snails.MedalType.None);
			this._medal.MedalSize = UISnailsMedal.MedalSizeType.Tiny;
			this._medal.IgnoreShowEffect = true;
			this._medal.Position = this.NativeResolution(new Vector2(750, -120));
			this._pnlContainer.Controls.Add(this._medal);

			// Locker
			this._imgLocker = new UIImage(screenOwner, IMG_LOCKER_RESOURCE, ResourceManager.RES_MANAGER_ID_STATIC);
			this._imgLocker.BlendColorWithParent = false;
			this._imgLocker.Effect = new ScaleEffect(new Vector2(1f, 1f), 0.2f, new Vector2(0.95f, 0.95f), true);
			this._imgLocker.Position = this.NativeResolution(new Vector2(600, 650));
			this._pnlContainer.Controls.Add(this._imgLocker);

			//Locker in demo
			this._imgLockerDemo = new UIImage(screenOwner, "spriteset/menu-elements-1/LockedInDemo", ResourceManager.RES_MANAGER_ID_TEMPORARY);
			this._imgLockerDemo.BlendColorWithParent = false;
			this._imgLockerDemo.Scale = new Vector2(0.5f, 0.5f);
			this._imgLockerDemo.Effect = new ScaleEffect(new Vector2(0.45f, 0.45f), 0.05f, new Vector2(0.42f, 0.42f), true);
			this._pnlContainer.Controls.Add(this._imgLockerDemo);

			// Stage id
			#if DEBUG
			this._capStageId = new UICaption(screenOwner, "", Color.LightGray, UICaption.CaptionStyle.StageId);
			this._pnlContainer.Controls.Add(this._capStageId);
			#endif
			// Don't set the container scale before this, because other controls are based on it's size
			this._pnlContainer.Scale = UIStage.DEFAULT_SCALE;

			this._focusSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_FOCUS);
			this._selectedSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SELECTED);
			this.Size = this._imgBackground.Size;
			this._pnlContainer.Size = this.Size;
			this.Locked = true;
			this._selectedEffect = new ColorEffect(Color.White, Color.LightGray, 0.04f, true);
		}

		/// <summary>
		/// 
		/// </summary>
		void UIStage_OnInitializeFromContent(IUIControl sender)
		{
			//     this._goalIcon.Position = this.GetContentPropertyValue<Vector2>("goalIconPosition", this._goalIcon.Position);
			//this._medal.Position = this.GetContentPropertyValue<Vector2>("medalPosition", this._medal.Position);
			// this.LockerPosition = this.GetContentPropertyValue<Vector2>("lockerPosition", this.LockerPosition);
			//  this.LockerDemoPosition = this.GetContentPropertyValue<Vector2>("lockerDemoPosition", this.LockerDemoPosition);
			//this.EnterEffectScale = this.GetContentPropertyValue<Vector2>("enterEffectScale", this.EnterEffectScale);
			this.EnterEffectScale = DEFAULT_SCALE;
		}

		/// <summary>
		/// 
		/// </summary>
		void UIStage_OnAfterInitializeFromContent(IUIControl sender)
		{
			// this._imgLocker.Position = this.LockerPosition;
			this._imgLockerDemo.Position = this.LockerDemoPosition;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void InitializeFromContent()
		{
			base.InitializeFromContent();
			//     this.InitializeFromContent("UIStage");
		}

		void _btnStage_OnBeforePress(IUIControl sender)
		{
			_selectedSample.Play();
			if (this.OnPress != null)
			{
				this.OnPress(this);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		void _pnlContainer_OnHide(IUIControl sender)
		{
			this.InvokeOnHide();
		}

		/// <summary>
		/// 
		/// </summary>
		void _pnlContainer_OnShow(IUIControl sender)
		{
			this.InvokeOnShow();
		}

		/// <summary>
		/// 
		/// </summary>
		void _btnStage_OnPress(IUIControl sender)
		{
			if (this.OnPress != null)
			{
				this.OnPress(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void _btnStage_OnDoublePress(IUIControl sender)
		{
			if (this.OnDoublePress != null)
			{
				this.OnDoublePress(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void Refresh()
		{
			this.AcceptControllerInput = (!this.Locked);
			this._imgLocker.Visible = this.Locked;
			this._imgLockerDemo.Visible = false;
			this.Enabled = !this.Locked;

			#if !RETAIL
			// Change the locker image depending on stage availability
			// Only needed if it's not retail version
			if (this.Locked && SnailsGame.GameSettings.GameplayMode != Configuration.GameSettings.GameplayModeType.Retail)
			{
				//   this._imgLocker.Sprite = BrainGame.ResourceManager.GetSpriteTemporary(UIStage.IMG_LOCKER_RESOURCE);
				//   this._imgLocker.Position = this.LockerPosition;
				this._imgLocker.Visible = true;
				this._imgLockerDemo.Visible = false;

				if (this._levelStageInfo != null)
				{
					if (!this._levelStageInfo.AvailableInDemo && SnailsGame.IsTrial)
					{
						this._imgLocker.Visible = false;
						this._imgLockerDemo.Visible = true;
						this.AcceptControllerInput = true;
						this.Enabled = true;
					}
				}
			}
			#endif
			this._capStageNr.Visible = !this.Locked;
			this._btnStage.Enabled = this.Enabled;
			this.BlendColor = (this.Locked ? Color.Black : Color.White);
			this.Medal = MedalType.None;
			this._btnStage.CurrentFrame = 0;
			#if DEBUG
			this._capStageId.Visible = SnailsGame.GameSettings.AllStagesUnlocked;
			#endif
			if (this._levelStageInfo != null)
			{
				this._goalIcon.Goal = this._levelStageInfo._goal;
				this._goalIcon.Visible = (this._goalIcon.Goal != GoalType.SnailDelivery);

				PlayerStageStats stageStats = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(this._levelStageInfo.StageId);
				if (stageStats != null)
				{
					this.Medal = stageStats.Medal;
					if (stageStats.HintsTaken > 0)
					{
						this._btnStage.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/menu-elements-1/StageHintIcon");
						int frame = stageStats.HintsTaken - 1;
						if (frame > this._btnStage.Sprite.FrameCount)
						{
							frame = this._btnStage.Sprite.FrameCount - 1;
						}
						this._btnStage.CurrentFrame = frame;
					}
				}
				#if DEBUG
				this._capStageId.Text = this._levelStageInfo.StageId;
				#endif

			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Reset()
		{
			this.DoOnLeaveEffect = true;
			this._pnlContainer.Scale = UIStage.DEFAULT_SCALE;
			this._pnlContainer.Visible = true;
		}


		/// <summary>
		/// 
		/// </summary>
		public void Stage_OnEnter(IUIControl sender)
		{
			if (!this.ScreenOwner.InputController.WithMouse) {
				return;
			}
			if (this.Locked || !this.AcceptOnEnterEvents)
				return;
			_focusSample.Play();
			this._pnlContainer.Effect = new ScaleEffect(UIStage.DEFAULT_SCALE, 2.0f, this.EnterEffectScale, false);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Stage_OnLeave(IUIControl sender)
		{
			if (!this.ScreenOwner.InputController.WithMouse) {
				return;
			}
			if (this.DoOnLeaveEffect)
			{
				this._pnlContainer.Effect = null;
				if (this.Selected == false)
				{
					this._pnlContainer.Scale = UIStage.DEFAULT_SCALE;
				}
			}
		}

	}
}
