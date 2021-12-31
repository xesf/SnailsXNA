using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
	class UIStagesPanel : UIControl
	{
		const float STAGE_ITEM_WIDTH = 1000;
		const float STAGE_ITEM_HEIGHT = 1200;

		#region Events
		public UIEvent OnStageSelected;
		public event UIEvent OnStageEnter;
		public event UIEvent OnStageLeave;
		#endregion

		#region private Vars
		private UIStage[] _stageControls;
		private UITimer _timer;
		private Sample _shownSample;
		private Sample _HiddenSample;
		#endregion

		#region Properties
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
		#endregion

		public UIStagesPanel(UIScreen owner) :
		base(owner)
		{
			// Stage controls
			this.CreateStageControls();

			// Set up the timer a generic timer
			this._timer = new UITimer(this.ScreenOwner, 500, true);
			this._timer.Enabled = false;
			this.Controls.Add(this._timer);

			this._shownSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SHOWN);
			this._HiddenSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.STAGE_PANEL_HIDDEN);
		}


		/// <summary>
		/// 
		/// </summary>
		private void CreateStageControls()
		{
			this._stageControls = new UIStage[Levels._instance.MaxStagesPerTheme];
			this.Position = this.NativeResolution(new Vector2(0f, 4700f));
			float posx = 0.0f;
			float posy = 0.0f;
			// Used to set the panel size
			float maxx = 0.0f;
			float maxy = 0.0f;
			for (int i = 0; i < this._stageControls.Length; i++)
			{
				this._stageControls[i] = new UIStage(this.ScreenOwner, i + 1);
				this._stageControls[i].Position = new Vector2(posx, posy);

				posx += this._stageControls[i].Width;
				if (i == 6 || i == 13) // Quebra de linha
				{
					posx = 0.0f;
					posy += this._stageControls[i].Height;
				}

				if (posx > maxx)
					maxx = posx;
				if (posy > maxy)
					maxy = posy;

				this._stageControls[i].OnEnter += new UIEvent(UIStage_OnEnter);
				this._stageControls[i].OnLeave += new UIEvent(UIStage_OnLeave);
				this._stageControls[i].OnPress += new UIEvent(UIStage_OnAccept);
				this.Controls.Add(this._stageControls[i]);
			}
			this._stageControls[0].OnShow += new UIEvent(FirstStageControl_OnShow);
			this._stageControls[0].OnHide += new UIEvent(FirstStageControl_OnHide);

			this.Size = new Size((int)maxx + this._stageControls[0].Size.Width,
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
			this._shownSample.Play();
			this.Visible = true;
			this.Reset();
			this.SetTheme(levels, themeId);
			for (int i = 0; i < this._stageControls.Length; i++)
			{
				this._stageControls[i].Selected = false;
				this._stageControls[i].Show();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Hide()
		{
			this._HiddenSample.Play();
			for (int i = 0; i < this._stageControls.Length; i++ )
			{
				this._stageControls[i].Hide();
			}
			base.Hide();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Reset()
		{
			this.RaiseStageLeaveEvent = true;
			for (int i = 0; i < this._stageControls.Length; i++)
			{
				this._stageControls[i].Reset();
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
			if (i <= 0 )
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
			//UIStage stage = (UIStage)sender;

			if (this.OnStageSelected != null)
			{
				this.OnStageSelected(sender);
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
