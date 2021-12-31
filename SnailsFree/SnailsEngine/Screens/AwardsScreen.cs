using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Screens.CommonControls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.Snails.Screens.Transitions;

namespace TwoBrainsGames.Snails.Screens
{
    class AwardsScreen : SnailsScreen
    {
        #region consts
        const float TOP_MARGIN = 700f;
        const float BOTTOM_MARGIN = 400f;
        const float SPACING = 350f;
        const float LEFT_MARGIN = 250f;
        //Color CaptionColorWon = Color.LightBlue;
        //Color CaptionColorNotWon = Color.Gray;
        #endregion

        #region Vars
        UISnailsMenuTitle _title;
        UIBackButton _btnBack;
        UISnailsScrollablePanel _panel;
        #endregion

        public AwardsScreen(ScreenNavigator owner) :
            base(owner, ScreenType.Awards)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            // Title
            this._title = new UISnailsMenuTitle(this);
            this._title.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._title.Position = new Vector2(0f, 200f);
            this._title.TextResourceId = "TITLE_SNAILS_AWARDS";
            this.Controls.Add(this._title);

            // Panel
            this._panel = new UISnailsScrollablePanel(this, UIScrollablePanel.PanelOrientation.Vertical, (SnailsGame.Settings.UseTouch == false), 10000f);
            this._panel.ParentAlignment = AlignModes.Horizontaly;
            this._panel.Size = new Size(7000f, 6500f);
            this._panel.Position = new Vector2(0f, 2400f);
            this.Controls.Add(this._panel);

            // Back button
            this._btnBack = new UIBackButton(this);
            this._btnBack.ParentAlignment = AlignModes.Left | AlignModes.Bottom;
            this._btnBack.Margins.Left = 150f;
            this._btnBack.Margins.Bottom = 250f;
            this._btnBack.OnPress += new UIControl.UIEvent(btnBack_OnPress);
            this.Controls.Add(this._btnBack);

            // Screen props
            this.BackgroundImageBlendColor = Colors.AwardsScrBkColor;
            this.BackgroundType = ScreenBackgroundType.Image;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            Vector2 pos = new Vector2(LEFT_MARGIN, TOP_MARGIN);
            this._panel.Clear();

            // Build achievement labels
            foreach (BrainAchievement achievement in SnailsGame.AchievementsManager.Achievements.Values)
            {
                if (!SnailsGame.GameSettings.WithAppStore &&
                    achievement.ShowOnAppStore) // skip achievements that only appear in App Stores
                {
                    continue;
                }

                UIAchievement ach = new UIAchievement(this, achievement);
                ach.Position = pos;
                this._panel.Controls.Add(ach);
                pos += new Vector2(0f, ach.Height + SPACING);
            }
            this._panel.Length = pos.Y + BOTTOM_MARGIN;
            this._panel.Reset();
            this.EnableInput();
        }
        

        /// <summary>
        /// 
        /// </summary>
        void btnBack_OnPress(IUIControl sender)
        {
            this.DisableInput();
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleAndMenuVisible);
            this.NavigateTo(ScreenType.MainMenu.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }

    }
}
