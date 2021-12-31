using System;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
	class UIThemesPanel : UIControl
	{        
		#region Events
		public event UIControl.UIEvent OnThemeSelectedStarted;
		public event UIControl.UIEvent OnThemeSelected;
		public event UIControl.UIEvent OnCancelEnded;
		#endregion

		#region Members
		private UITheme[] _themesControls;
		private UITheme TopLeftTheme { get { return this._themesControls[0]; } }
		private UITheme BottomRightTheme { get { return this._themesControls[3]; } }
		private Sample _selectedSample;
		private Sample _shownSample;
		#endregion

		#region Properties
		public UITheme GardenTheme 
		{
			get { return this._themesControls[(int)ThemeType.ThemeA]; }
		}
		public UITheme AncientEgyptTheme
		{
			get { return this._themesControls[(int)ThemeType.ThemeB]; }
		}
		public UITheme BotFactoryTheme
		{
			get { return this._themesControls[(int)ThemeType.ThemeC]; }
		}

		public UITheme SelectedTheme { get; private set; }
		public UITheme LastSelectedTheme { get; private set; }

		private UITimer _tmrShowThemes;
		private UITimer _tmrThemeSelected;

		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
				if (this._themesControls != null)// && value == false) 
				{
					for (int i = 0; i < this._themesControls.Length; i++)
					{
						this._themesControls[i].Visible = value;
					}
				}
			}
		}

		private bool FocusEffectEnabled { get; set; }



		#endregion

		public UIThemesPanel(UIScreen owner) :
		base(owner)
		{
			this._themesControls = new UITheme[ThemeSelectionScreen.THEME_COUNT];
			// Garden
			this._themesControls[(int)ThemeType.ThemeA] = this.CreateThemeControl(ThemeType.ThemeA);
			this._themesControls[(int)ThemeType.ThemeA].ParentAlignment = AlignModes.Left | AlignModes.Top;
			this._themesControls[(int)ThemeType.ThemeA].OnShow += new UIEvent(GardenTheme_OnShow);
			this._themesControls[(int)ThemeType.ThemeA].OnShowBegin += new UIEvent(UIThemesPanel_OnShowBegin);

			// Acient egypt
			this._themesControls[(int)ThemeType.ThemeB] = this.CreateThemeControl(ThemeType.ThemeB);
			this._themesControls[(int)ThemeType.ThemeB].ParentAlignment = AlignModes.Right | AlignModes.Top;

			// Graveyard
			this._themesControls[(int)ThemeType.ThemeC] = this.CreateThemeControl(ThemeType.ThemeC);
			this._themesControls[(int)ThemeType.ThemeC].ParentAlignment = AlignModes.Left | AlignModes.Bottom;

			this.Size = this.PixelsToScreenUnits(new Size(1000, 520));
			this.SelectedTheme = null;

			// Show themes timer
			this._tmrShowThemes = new UITimer(owner, 150, true);
			this._tmrShowThemes.OnTimer += new UIControl.UIEvent(_tmrShowThemes_OnTimer);
			this.Controls.Add(this._tmrShowThemes);


			// Theme selected timer. Used to throw the ThemeSelected event after a team is selected
			// The event has to be delayed because of the hide effects on the themes
			this._tmrThemeSelected = new UITimer(owner, 350, false);
			this._tmrThemeSelected.OnTimer += new UIEvent(_tmrThemeSelected_OnTimer);
			this.Controls.Add(this._tmrThemeSelected);

			_selectedSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SELECTED);
			_shownSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SHOWN);
			this.FocusEffectEnabled = false; 

		}



		/// <summary>
		/// 
		/// </summary>
		void UIThemesPanel_OnShowBegin(IUIControl sender)
		{
			this._shownSample.Play();   
		}

		/// <summary>
		/// 
		/// </summary>
		public void Initialize()
		{
			foreach (UITheme theme in this._themesControls)
			{
				theme.Initialize();
				theme.FocusEffectEnabled = this.FocusEffectEnabled;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void GardenTheme_OnShow(IUIControl sender)
		{
			this.InvokeOnShow();
		}

		/// <summary>
		/// Themes are shown 1 by 1 with a delay, thus this timer
		/// </summary>
		void _tmrShowThemes_OnTimer(IUIControl sender)
		{
			for (int i = 0; i < this._themesControls.Length; i++)
			{
				this._themesControls[i].Show();
			}
			this._tmrShowThemes.Enabled = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Show()
		{
			this._tmrShowThemes.Enabled = true;
			this.Visible = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Focus()
		{
			base.Focus();
			if (this.SelectedTheme == null)
			{
				this.SelectedTheme = this._themesControls[0];
			}
			//    this.SelectedTheme.Focus();
		}

		/// <summary>
		/// 
		/// </summary>
		private UITheme CreateThemeControl(ThemeType themeId)
		{
			UITheme theme = new UITheme(this.ScreenOwner, themeId);
			theme.OnAccept += new UIEvent(this.Theme_OnAccept);
			theme.OnUnselectEnded = this.Theme_OnUnselectEnded;
			this.Controls.Add(theme);
			return theme;
		}

		/// <summary>
		/// 
		/// </summary>
		public void CancelSelection()
		{
			if (this.SelectedTheme == null)
			{
				return;
			}

			this.SelectedTheme.Unselect();
		}

		/// <summary>
		/// 
		/// </summary>
		public void SelectThemeWithoutAnimations(ThemeType theme)
		{
			this.SelectedTheme = this._themesControls[(int)theme];
			for (int i = 0; i < this._themesControls.Length; i++)
			{
				this._themesControls[i].Visible = (this._themesControls[i].ThemeId == theme);
			}
			this.SelectedTheme.SelectWithoutAnimations(this.TopLeftTheme.PositionInPixels);
		}

		/// <summary>
		/// Event occurs when theme is selected
		/// </summary>
		private void Theme_OnAccept(IUIControl sender)
		{
			this.AcceptControllerInput = false;
			UITheme theme = (UITheme)sender;
			this.SelectedTheme = theme;
			this.LastSelectedTheme = theme;
			this.ShowHideUnselectedThemes(false);
			this.SelectedTheme.Select(this.TopLeftTheme.PositionInPixels);
			this._tmrThemeSelected.Reset();
			this._tmrThemeSelected.Enabled = true;
			_selectedSample.Play();
			if (this.OnThemeSelectedStarted != null)
			{
				this.OnThemeSelectedStarted(sender);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void _tmrThemeSelected_OnTimer(IUIControl sender)
		{
			this._tmrThemeSelected.Enabled = false;
			if (this.OnThemeSelected != null)
			{
				this.OnThemeSelected(sender);
			}
		}

		/// <summary>
		/// Occurs when the theme selection ends
		/// </summary>
		private void Theme_OnMoveToEnded(IUIControl sender)
		{
			if (this.OnThemeSelected != null)
			{
				this.OnThemeSelected(sender);
			}
		}

		/// <summary>
		/// Occurs when the theme un selection ends
		/// </summary>
		private void Theme_OnUnselectEnded(IUIControl sender)
		{
			this.SelectedTheme.FocusEffectEnabled = this.FocusEffectEnabled;
			this.SelectedTheme.Show(); // reset focus on selected theme
			this.ShowHideUnselectedThemes(true);
			this.SelectedTheme = null;
			if (this.OnCancelEnded != null)
			{
				this.OnCancelEnded(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void ShowHideUnselectedThemes(bool show)
		{
			for (int i = 0; i < this._themesControls.Length; i++)
			{
				if (this.SelectedTheme != this._themesControls[i])
				{
					if (show)
					{
						//  this._themesControls[i].Show();
						this._themesControls [i].Visible = true;
					}
					else
					{
						this._themesControls[i].Hide();
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			for (int i = 0; i < this._themesControls.Length; i++)
			{
				this._themesControls[i].Refresh();
			}
		}
	}
}
