using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Screens.CommonControls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens
{
    class ThemeUnlockedScreen : SnailsScreen
    {
        #region Consts
        private const float LINE_SPACING = 400f;
        #endregion

        #region Vars
        private LeafTransition _leafs;
        protected UISnailsBoard _board;
        protected UISnailsMenuTitle _title;
        protected UISnailsButton _btnStageSel;
        protected UISnailsButton _btnProceed;
        protected UISnailsThemeIcon _imgTheme;
        protected UICaption[] _capHelp;
        protected UIStars _stars;
        private Sample _unlockSound;
        #endregion

        ThemeType UnlockedTheme { get; set; }
        protected ScreenType NextScreen { get; set; }
        string NextGroup { get; set; }
        LeafTransition OpeningTransition { get; set; }
        private Vector2 MessagePosition { get; set; } 


        public ThemeUnlockedScreen(ScreenNavigator owner) :
            base(owner, ScreenType.ThemeUnlocked)
        { 
        
        }

        #region Screen events
        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.Name = "ThemeUnlocked";
            this.BackgroundImage = null;
            this.BackgroundColor = new Color(0, 0, 0, 175);
            this.OnBeforeControlsDraw += new UIControl.UIEvent(ThemeUnlockedScreen_OnBeforeControlsDraw);
            this.OnInitializeFromContent += new UIControl.UIEvent(ThemeUnlockedScreen_OnInitializeFromContent);
            this.OnAfterInitializeFromContent += new UIControl.UIEvent(ThemeUnlockedScreen_OnAfterInitializeFromContent);

            // Leafs background
            this._leafs = new LeafTransition(LeafTransition.State.ClosedStopped);
            this._leafs.Initialize();

            // Board
            this._board = new UISnailsBoard(this, UISnailsBoard.BoardType.LightWoodMedium);
            this._board.Name = "_board";
            this._board.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._board.OnShow += new UIControl.UIEvent(_board_OnShow);
            this._board.Size = new BrainEngine.UI.Size(this._board.Width, 5800f);
            this.Controls.Add(this._board);

            // Title - Stage Completed
            this._title = new UISnailsMenuTitle(this);
            this._title.Name = "_title";
            this._title.TextResourceId = "TITLE_THEME_UNLOCK";
            this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
            this._board.Controls.Add(this._title);

            // Theme image
            this._imgTheme = new UISnailsThemeIcon(this);
            this._imgTheme.Name = "_imgTheme";
            this._imgTheme.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._board.Controls.Add(this._imgTheme);

            string  [] helpText = LanguageManager.GetMultiString("LBL_THEME_UNLOCK_HELP");
            this._capHelp = new UICaption[helpText.Length];
            for (int i = 0; i < helpText.Length; i++)
            {
                this._capHelp[i] = new UICaption(this, helpText[i], Color.White, UICaption.CaptionStyle.NormalTextSmall);
                this._capHelp[i].ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
                this._board.Controls.Add(this._capHelp[i]);
            }

            // Button stage selection
            this._btnStageSel = new UISnailsButton(this, "BTN_STAGE_SELECTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, this.btnStageSel_OnAccept, false);
            this._btnStageSel.Name = "_btnStageSel";
            this._btnStageSel.ParentAlignment = BrainEngine.UI.AlignModes.Bottom;
            this._btnStageSel.ButtonAction = UISnailsButton.ButtonActionType.StageSelection;
            this._board.Controls.Add(this._btnStageSel);

            // Button proceed
            this._btnProceed = new UISnailsButton(this, "BTN_PROCEED", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnProceed_OnAccept, false);
            this._btnProceed.Name = "_btnProceed";
            this._btnProceed.ParentAlignment = BrainEngine.UI.AlignModes.Bottom;
            this._btnProceed.ButtonAction = UISnailsButton.ButtonActionType.Next;
            this._board.Controls.Add(this._btnProceed);

            // Stars
            this._stars = new UIStars(this, 20, 2600, 500, 1000);
            this._stars.Name = "_stars";
            this._stars.Size = new BrainEngine.UI.Size(this._board.Size.Width, 500);
            this._board.Controls.Add(this._stars);

            this._unlockSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.THEME_UNLOCKED);

        }

        /// <summary>
        /// 
        /// </summary>
        void ThemeUnlockedScreen_OnInitializeFromContent(IUIControl sender)
        {
            this.MessagePosition = this.GetContentPropertyValue<Vector2>("messagePosition", this.MessagePosition);         
        }

        /// <summary>
        /// 
        /// </summary>
        void ThemeUnlockedScreen_OnAfterInitializeFromContent(IUIControl sender)
        {
            string[] helpText = LanguageManager.GetMultiString("LBL_THEME_UNLOCK_HELP");
            Vector2 pos = this.MessagePosition;
            for (int i = 0; i < helpText.Length; i++)
            {
                this._capHelp[i].Position = pos;
                pos += new Vector2(0f, LINE_SPACING);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this.DisableInput();
            this._board.Visible = false;
            this._board.Show();
            this.NextScreen = this.Navigator.GlobalCache.Get<ScreenType>(GlobalCacheKeys.THEME_UNLOCK_NEXT_SCREEN, ScreenType.None);
            this.NextGroup = this.Navigator.GlobalCache.Get<string>(GlobalCacheKeys.THEME_UNLOCK_NEXT_SCREEN_GROUP, null);
            this.OpeningTransition = this.Navigator.GlobalCache.Get<LeafTransition>(GlobalCacheKeys.THEME_UNLOCK_OPEN_TRANSITION, null);
            this.UnlockedTheme = this.Navigator.GlobalCache.Get<ThemeType>(GlobalCacheKeys.THEME_UNLOCK_UNLOCKED_THEME, ThemeType.None);

            if (this.NextScreen == ScreenType.ThemeSelection)
            {
                this._btnStageSel.Visible = false;
            //    this._btnProceed.Position = new Vector2(1300, 4550);
                this._btnProceed.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
            }
            else
            {
                this._btnStageSel.Visible = true;
//                this._btnStageSel.Position = new Vector2(100, 4550);
                this._btnProceed.ParentAlignment = AlignModes.Bottom;
                //this._btnProceed.Position = new Vector2(2500, 4550);
            }

            this._imgTheme.Theme = this.UnlockedTheme;

            // Set the help captions color according with the theme
            foreach (UICaption cap in this._capHelp)
            {
                cap.BlendColor = Colors.ThemeCaptions[(int)this.UnlockedTheme];
            }

            this._stars.Initialize();
            this.DisableInput();

            switch (this.UnlockedTheme)
            {
                case ThemeType.ThemeB:
                    SnailsGame.AchievementsManager.Notify((int)AchievementsType.UnlockEgyptTheme);
                    break;
                case ThemeType.ThemeC:
                    SnailsGame.AchievementsManager.Notify((int)AchievementsType.UnlockGraveyardTheme);
                    break;
                case ThemeType.ThemeD:
                    SnailsGame.AchievementsManager.Notify((int)AchievementsType.UnlockGoldminesTheme);
                    break;
            }
            this._unlockSound.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        void ThemeUnlockedScreen_OnBeforeControlsDraw(IUIControl sender)
        {
           // this._leafs.Draw(this.SpriteBatch);
        }

       
        #endregion

        /// <summary>
        /// 
        /// </summary>
        void btnProceed_OnAccept(IUIControl sender)
        {
            this.DisableInput();
            // Clear this flag because if we unlocked a new theme, show the themes icons to the user
            // It doesn't matter where the user is going from here, clear the flag anyways
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, false); 
            this.NavigateTo(this.NextGroup, this.NextScreen.ToString(), ScreenTransitions.LeafsClosing, this.OpeningTransition);
        }

        /// <summary>
        /// 
        /// </summary>
        void btnStageSel_OnAccept(IUIControl sender)
        {
            this.DisableInput();
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, false); // Clear this flag because if we unlocked a new theme, show the themes icons to the user
            this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

        #region Board events
       
        /// <summary>
        /// 
        /// </summary>
        void _board_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this._btnProceed.Focus();
        }

        #endregion
    }
}
