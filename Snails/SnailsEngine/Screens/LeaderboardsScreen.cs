using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Screens.ThemeSelection;
using TwoBrainsGames.Snails.Leaderboards;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Screens
{
    class LeaderboardsScreen : SnailsScreen
    {
        const float TOP_MARGIN = 700f;
        const float BOTTOM_MARGIN = 400f;
        const float SPACING = 50f;
        const float LEFT_MARGIN = 250f;

        private UIBackButton _btnBack;
        private UISnailsScrollablePanel _panel;
        private UISnailsMenuTitle _title;
        private List<LeaderboardPlayerData> LeaderboarsData { get; set; }
        private LeaderboardPlayerData CurrentPlayerData { get; set; }

        public LeaderboardsScreen(ScreenNavigator owner) :
            base(owner, ScreenType.Leaderboards)
        {
            this.Name = "HowToPlay";
            this.BackgroundColor = new Color(0, 0, 0, 200);
            this.OnBack += new BrainEngine.UI.Controls.UIControl.UIEvent(LeaderboardsScreen_OnBack);

            // Back to theme selection button
            this._btnBack = new UIBackButton(this);
            this._btnBack.ScreenAlignment = UIBackButton.ButtonScreenAlignment.BottomLeft;
            this._btnBack.OnPress += new UIControl.UIEvent(_btnBack_OnPress);
            this.Controls.Add(this._btnBack);

            // Panel
            this._panel = new UISnailsScrollablePanel(this, UIScrollablePanel.PanelOrientation.Vertical, (SnailsGame.Settings.UseTouch == false), 0f);
            this._panel.Size = new Size(7000f, 7200f);
            this._panel.Position = new Vector2(2500f, 2400f);
            this._panel.ParentAlignment = AlignModes.Horizontaly;
            this._panel.BackgroundColor = new Color(200, 200, 200, 255);
            this._panel.Footer.Visible = true;
            this._panel.Footer.Height = 1200f;
            this._panel.BottomMargin = 1550f;
            this.Controls.Add(this._panel);

            // Title
            this._title = new UISnailsMenuTitle(this);
            this._title.TextResourceId = "TITLE_LEADERBOARDS";
            this._title.SubTitleTextResourceId = "TITLE_LEADERBOARDS";
            this._title.ShowSubtitle = true;
            this._title.ParentAlignment = AlignModes.Horizontaly;
            this._title.BoardSize = UISnailsMenuTitle.TitleSize.Medium;
            this._title.Position = new Vector2(0f, 500f);
            this._title.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.04f, this.BlendColor, new Vector2(1.0f, 1.0f));
            this.Controls.Add(this._title);

        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this.LeaderboarsData = new List<LeaderboardPlayerData>();
            this.LeaderboarsData.Add(new LeaderboardPlayerData(1, "FMF1", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(2, "FMF2", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(3, "FMF3", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(4, "FMF4", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(5, "FMF5", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(6, "FMF6", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(7, "FMF7", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(888888, "12345678901234567890", 100000, new TimeSpan(10, 10, 10)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(9, "FMF1", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(10, "FMF2", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(11, "FMF1", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(12, "FMF2", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(13, "Limanima", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(14, "FMF4", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(15, "FMF5", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(16, "FMF6", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(17, "FMF7", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(888888, "12345678901234567890", 100000, new TimeSpan(10, 10, 10)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(19, "FMF1", 10, new TimeSpan(0, 1, 1)));
            this.LeaderboarsData.Add(new LeaderboardPlayerData(20, "FMF2", 10, new TimeSpan(0, 1, 1)));
            
            SnailsGame.ProfilesManager.CurrentProfile.Name = "Limanima";
            this.CurrentPlayerData = new LeaderboardPlayerData(100, "Limanima", 11, new TimeSpan(0,1,1));
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnBack_OnPress(IUIControl sender)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        void LeaderboardsScreen_OnBack(BrainEngine.UI.Controls.IUIControl sender)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        void Refresh()
        {
            // Title
            this._title.SubTitleText = string.Format(LanguageManager.GetString("TITLE_LEADERBOARDS_STAGE"),
                                            Formater.GetThemeName(ThemeType.ThemeA), 1);

            // Items
            this._panel.Clear();
            float posY = TOP_MARGIN;
            foreach (LeaderboardPlayerData playerData in this.LeaderboarsData)
            {
                UILeaderboardItem item = new UILeaderboardItem(this, playerData);
                item.Position = new Vector2(LEFT_MARGIN, posY);
                this._panel.Controls.Add(item);
                posY += item.Height + SPACING;
            }

            this._panel.Length = posY + BOTTOM_MARGIN;
            this._panel.Reset();

            this._panel.Footer.Controls.Clear();
            if (this.CurrentPlayerData != null)
            {
                UILeaderboardItem playerDataItem = new UILeaderboardItem(this, this.CurrentPlayerData);
                playerDataItem.Position = new Vector2(LEFT_MARGIN, 0f);
                this._panel.Footer.Controls.Add(playerDataItem);
            }
        }
    }
}
