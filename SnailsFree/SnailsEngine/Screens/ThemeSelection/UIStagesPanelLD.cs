using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.Snails.Player;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Effects;

namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
	class UIStagesPanelLD : UISnailsScrollablePanel
	{
		const float STAGE_ITEM_WIDTH = 1300;
		const float STAGE_ITEM_HEIGHT = 1800;

		#region Events
		public UIEvent OnStageSelected;
		public UIEvent OnStageDoubleSelected;
		public event UIEvent OnStageEnter;
		public event UIEvent OnStageLeave;
		#endregion

		#region private Vars
		private UIStage[] _stageControls;
		private UITimer _timer;
		private Sample _shownSample;
		//private Sample _HiddenSample;

		// arrows
		private UIArrow _arrowUp;
		private UIArrow _arrowDown;
		private HooverEffect _arrowEffect;
		protected Vector2 _arrowUpAbsPos;
		protected Vector2 _arrowDownAbsPos;
		protected bool? _atBoundUpChanged = false;
		protected bool? _atBoundDownChanged = false;
		UIStage _selectedStage;
		#endregion

		#region Properties
		UIStage SelectedStage 
		{
			get
			{
				return this._selectedStage;
			}
			set
			{
				if (this._selectedStage != null)
				{
					this._selectedStage.Selected = false;
				}
				this._selectedStage = value;
				if (this._selectedStage != null)
				{
					this._selectedStage.Selected = true;
				}
			}
		}
		public bool RaiseStageLeaveEvent { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
				if (this._stageControls != null)
				{
					for (int i = 0; i < this._stageControls.Length; i++)
					{
						this._stageControls[i].Visible = value;
					}
				}
			}
		}
		public bool FocusEffectEnabled { get; set; }

		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}
		#endregion

		public UIStagesPanelLD(UIScreen owner) :
		base(owner, UIScrollablePanel.PanelOrientation.Vertical, false, 8400f)
		{

			// Set up the timer a generic timer
			this._timer = new UITimer(this.ScreenOwner, 500, true);
			this._timer.Enabled = false;
			this.Controls.Add(this._timer);

			this._shownSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SHOWN);

			_arrowEffect = new HooverEffect(1f, 1.5f, 180.0f);

			this._arrowUpAbsPos = new Vector2(5050, 5700);
			this._arrowUp = new UIArrow(this.ScreenOwner, UIArrow.ArrowType.Up, UIArrow.ArrowSize.Small, _arrowEffect);
			this._arrowUp.Position = this._arrowUpAbsPos;
			this._arrowUp.Visible = false;

			this._arrowDownAbsPos = new Vector2(5050, 6700);
			this._arrowDown = new UIArrow(this.ScreenOwner, UIArrow.ArrowType.Down, UIArrow.ArrowSize.Small, _arrowEffect);
			this._arrowDown.Position = this._arrowDownAbsPos;

			this.ShowScrollIndicators = true;

			// Stage controls
			this.CreateStageControls();
		}

		public void SelectStage(LevelStage levelStage)
		{
			foreach (UIStage stage in this._stageControls)
			{
				if (stage.LevelStageInfo.StageKey == levelStage.StageKey)
				{
					this.SelectedStage = stage;
				}
			}
		}

		public void ShowArrows()
		{
			bool atBoundUp = this.AtUpBound();
			if (!atBoundUp)
			{
				this._arrowUp.Show();
			}

			bool atBoundDown = this.AtDownBound();
			if (!atBoundDown)
			{
				this._arrowDown.Show();
			}
		}

		public void HideArrows()
		{
			bool atBoundUp = this.AtUpBound();
			if (!atBoundUp)
			{
				this._arrowUp.Hide();
			}

			bool atBoundDown = this.AtDownBound();
			if (!atBoundDown)
			{
				this._arrowDown.Hide();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		private void CreateStageControls()
		{
			const float STAGE_LEFT_MARGIN = 100f;
			this._stageControls = new UIStage[Levels._instance.MaxStagesPerTheme];
			float posx = STAGE_LEFT_MARGIN;
			float posy = 300.0f;
			// Used to set the panel size
			float maxx = 0.0f;
			float maxy = 0.0f;
			for (int i = 0; i < this._stageControls.Length; i++)
			{
				this._stageControls[i] = new UIStage(this.ScreenOwner, i + 1);
				this._stageControls[i].Position = new Vector2(posx, posy);

				// O que se está a fazer aqui é calcular a scale para ficar com 4500 de tamanho
				float f = 1300 * this._stageControls[i].SizeInPixels.Width / this._stageControls[i].Size.Width; 
				float scalex = f / (float)this._stageControls[i].SizeInPixels.Width;
				this._stageControls[i].Scale = new Vector2 (scalex, scalex);

				posx += STAGE_ITEM_WIDTH + 30;
				if (((i+1) % 3) == 0)
				{
					posx = STAGE_LEFT_MARGIN;
					posy += STAGE_ITEM_HEIGHT;
				}

				if (posx > maxx)
					maxx = posx;
				if (posy > maxy)
					maxy = posy;

				//this._stageControls[i].OnEnter += new UIEvent(UIStage_OnEnter);
				//this._stageControls[i].OnLeave += new UIEvent(UIStage_OnLeave);
				this._stageControls[i].AcceptOnEnterEvents = false;
				this._stageControls[i].OnPress += new UIEvent(UIStage_OnAccept);
				this._stageControls[i].OnDoublePress += new UIEvent(UIStage_OnDoublePress);
				this.Controls.Add(this._stageControls[i]);
			}
			this._stageControls[0].OnShow += new UIEvent(FirstStageControl_OnShow);
			this._stageControls[0].OnHide += new UIEvent(FirstStageControl_OnHide);

			this.Size = new Size((int)maxx,
				(int)maxy + this._stageControls[0].Size.Height);

		}

		/// <summary>
		/// 
		/// </summary>
		public void SetTheme(Levels levels, ThemeType themeId)
		{

			for (int i = 0; i < levels.GetStagesCountForTheme(themeId); i++)
			{
				this._stageControls[i].LevelStageInfo = levels.FindStageInfo(themeId, i + 1);
				this._stageControls[i].ThemeId = themeId;

				if (SnailsGame.ProfilesManager.CurrentProfile != null)
				{
					this._stageControls[i].Locked = !SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsStageUnlocked(this._stageControls[i].LevelStageInfo.StageId);

					PlayerStageStats stageStats = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(this._stageControls[i].LevelStageInfo.StageId);
					if (stageStats != null)
					{
						this._stageControls[i].Medal = stageStats.Medal;
					}
					else
					{
						this._stageControls[i].Medal = MedalType.None;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Show(Levels levels, ThemeType themeId)
		{
			this.Enabled = true;
			this.SelectedStage = null;
			this._shownSample.Play();
			this.Show();
			this.ResetPanel();
			this.SetTheme(levels, themeId);
			for (int i = 0; i < this._stageControls.Length; i++)
			{
				this._stageControls[i].Show();
			}

			ShowArrows();
		}

		/// <summary>
		/// 
		/// </summary>
		public void ResetPanel()
		{
			this.RaiseStageLeaveEvent = true;
			for (int i = 0; i < this._stageControls.Length; i++)
			{
				this._stageControls[i].Reset();
			}
		}

		private void EnableFocusEffect(bool enabled)
		{
			this.FocusEffectEnabled = enabled;
			for (int i = 0; i < this._stageControls.Length; i++)
			{
				this._stageControls[i].AcceptControllerInput = enabled;
			}
		}

		public void OnProcessControllerEnd(IUIControl sender)
		{
			if (this.State == TwoBrainsGames.BrainEngine.UI.Controls.UIScrollablePanel.PanState.None &&
				this.PreviousState == TwoBrainsGames.BrainEngine.UI.Controls.UIScrollablePanel.PanState.None)
			{
				EnableFocusEffect(true);
			}
			else
			{
				EnableFocusEffect(false);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public UIStage Focus(int stageNr)
		{
			if (stageNr <= 0 ||
				stageNr > this._stageControls.Length)
			{
				stageNr = 1;
			}
			this._stageControls[stageNr - 1].Focus();
			return this._stageControls [stageNr - 1];
		}

		/// <summary>
		/// 
		/// </summary>
		public void FocusOnLastUnlocked()
		{
			int last = this.GetLastUnlockedStage();
			if (last > 0)
			{
				this.Focus(last);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Cancel()
		{
			this.ScreenOwner.IgnoreControlFocus = true;
			this._timer.OnTimer += Timer_OnHideStageTimer;
			this._timer.Enabled = true;
			this._timer.Parameter = (this._stageControls.Length - 1);
		}

		/// <summary>
		/// 
		/// </summary>
		private void Timer_OnHideStageTimer(IUIControl sender)
		{
			int i = (int)this._timer.Parameter;
			this._stageControls[(int)this._timer.Parameter].Visible = false;

			this._timer.Parameter = --i;
			if (i <= 0)
			{
				this._timer.Enabled = false;
				this.Visible = false;
			}
		}



		/// <summary>
		/// Returns the stage nr, not the stage index
		/// </summary>
		public int GetLastUnlockedStage()
		{
			for (int i = this._stageControls.Length - 1; i >= 0; i--)
			{
				if (!this._stageControls[i].Locked)
				{
					return i + 1;
				}
			}

			return 0;
		}

		#region Stages events
		/// <summary>
		/// 
		/// </summary>
		private void UIStage_OnAccept(IUIControl sender)
		{
			UIStage stage = (UIStage)sender;
			this.SelectedStage = stage;
			if (this.OnStageSelected != null)
			{
				this.OnStageSelected(sender);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void UIStage_OnDoublePress(IUIControl sender)
		{
			//UIStage stage = (UIStage)sender;

			if (this.OnStageDoubleSelected != null)
			{
				this.OnStageDoubleSelected(sender);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void FirstStageControl_OnHide(IUIControl sender)
		{
			this.Visible = false;
			this.InvokeOnHide();
		}

		/// <summary>
		/// 
		/// </summary>
		void FirstStageControl_OnShow(IUIControl sender)
		{
			this.InvokeOnShow();
		}


		/// <summary>
		/// 
		/// </summary>
		void UIStage_OnEnter(IUIControl sender)
		{
			if (this.OnStageEnter != null)
			{
				this.OnStageEnter(sender);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void UIStage_OnLeave(IUIControl sender)
		{
			if (this.RaiseStageLeaveEvent && this.OnStageLeave != null)
			{
				this.OnStageLeave(sender);
			}
		}
		#endregion

	}
}
