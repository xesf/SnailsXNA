using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.ThemeSelection;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Effects;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class UIThemeScrollablePanel : UISnailsScrollablePanel
    {
        #region Events
        public event UIControl.UIEvent OnThemeSelectedStarted;
        public event UIControl.UIEvent OnThemeSelected;
        public event UIControl.UIEvent OnCancelEnded;
        #endregion

        #region Members
        private UITheme[] _themesControls;
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
        public UITheme OuterSpaceTheme
        {
            get { return this._themesControls[(int)ThemeType.ThemeD]; }
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
                if (this._themesControls != null)
                {
                    for (int i = 0; i < this._themesControls.Length; i++)
                    {
                        this._themesControls[i].Visible = value;
                    }
                }
                this.StopFlick();
            }
        }
        
        #endregion

        public UIThemeScrollablePanel(UIScreen screenOwner) :
            this(screenOwner, Vector2.Zero)
        { }

        public UIThemeScrollablePanel(UIScreen screenOwner, Vector2 position) :
            base(screenOwner, UIScrollablePanel.PanelOrientation.Horizontal, false, 18600f)
        {

            this._themesControls = new UITheme[ThemeSelectionScreen.THEME_COUNT];
            this.Orientation = UIScrollablePanel.PanelOrientation.Horizontal;

            // Garden
            this._themesControls[(int)ThemeType.ThemeA] = this.CreateThemeControl(ThemeType.ThemeA, new Vector2(0, 0));
            this._themesControls[(int)ThemeType.ThemeA].OnShow += new UIEvent(GardenTheme_OnShow);
            this._themesControls[(int)ThemeType.ThemeA].OnShowBegin += new UIEvent(UIThemesPanel_OnShowBegin);

            // Acient egypt
            this._themesControls[(int)ThemeType.ThemeB] = this.CreateThemeControl(ThemeType.ThemeB, new Vector2(4500, 0));
            // Graveyard
            this._themesControls[(int)ThemeType.ThemeC] = this.CreateThemeControl(ThemeType.ThemeC, new Vector2(9000, 0));
            // Goldmines
            this._themesControls[(int)ThemeType.ThemeD] = this.CreateThemeControl(ThemeType.ThemeD, new Vector2(13500, 0));

            this.Size = new Size(9500, 5000); // 4450
            this.SelectedTheme = null;

            // Show themes timer
            this._tmrShowThemes = new UITimer(screenOwner, 150, true);
            this._tmrShowThemes.OnTimer += new UIControl.UIEvent(_tmrShowThemes_OnTimer);
            this.Controls.Add(this._tmrShowThemes);

            // Theme selected timer. Used to throw the ThemeSelected event after a team is selected
            // The event has to be delayed because of the hide effects on the themes
            this._tmrThemeSelected = new UITimer(screenOwner, 350, false);
            this._tmrThemeSelected.OnTimer += new UIEvent(_tmrThemeSelected_OnTimer);
            this.Controls.Add(this._tmrThemeSelected);

            this._selectedSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SELECTED);
            this._shownSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SHOWN);
        }

        /// <summary>
        /// 
        /// </summary>
        private UITheme CreateThemeControl(ThemeType themeId, Vector2 position)
        {
            UITheme theme = new UIThemeLD(this.ScreenOwner, themeId);
            theme.ParentAlignment = AlignModes.Top;
            theme.Margins.Top = 125;
            theme.Scale = new Vector2(0.80f, 0.80f); // resize themes to be able to show 2 per screen
            theme.Position = position;
            theme.UpdateLayout();
            theme.OnAccept += new UIEvent(this.Theme_OnAccept);
            theme.OnUnselectEnded = this.Theme_OnUnselectEnded;
            this.Controls.Add(theme);
            return theme;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (UITheme theme in this._themesControls)
            {
                theme.Initialize();
            }
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
            if (base.State == UIScrollablePanel.PanState.None)
            {
                base.Focus();
                if (this.SelectedTheme == null)
                {
                    this.SelectedTheme = this._themesControls[0];
                }
                this.SelectedTheme.Focus();
            }
        }

        /// <summary>
        /// Event occurs when theme is selected
        /// </summary>
        private void Theme_OnAccept(IUIControl sender)
        {
            UITheme theme = (UITheme)sender;
            if (!theme.Locked /*&&
                this.PreviousState == UIScrollablePanel.PanState.None*/)
            {
                this.SelectedTheme = theme;
                this.LastSelectedTheme = theme;

                this.ShowHideUnselectedThemes(false);

                this._tmrThemeSelected.Reset();
                this._tmrThemeSelected.Enabled = true;

                _selectedSample.Play();

                if (this.OnThemeSelectedStarted != null)
                {
                    this.OnThemeSelectedStarted(sender);
                }
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
        /// Occurs when the theme un selection ends
        /// </summary>
        private void Theme_OnUnselectEnded(IUIControl sender)
        {

            this.SelectedTheme.FocusEffectEnabled = false; //true;
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
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowHideUnselectedThemes(bool show)
        {
            for (int i = 0; i < this._themesControls.Length; i++)
            {
                if (show)
                {
                    this._themesControls[i].Show();
                }
                else
                {
                    this._themesControls[i].Hide();
                }
            }


        }
    }
}
