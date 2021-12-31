using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Player;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens
{
    class PlayerStatsScreen : SnailsScreen
    {
        const float TOP_MARGIN = 500f;
        const float BOTTOM_MARGIN = 500f;
        const float CAP_WIDTH = 900f;
        const float LINE_SPACING = 100f;
        const float LEFT_MARGIN = 300f;

        #region Vars
        //Color _colorCaptions;
        //Color _colorValues;

        UISnailsMenuTitle _title;
        UISnailsScrollablePanel _panel;

        UIPanel _pnlStats;
        UIPlayerStat _capPlayingTime;
        UIPlayerStat _capRunningTime;
        UIPlayerStat _capTotalSnailsSafe;
        UIPlayerStat _capTotalSnailsKingSafe;
        UIPlayerStat _capTotalSnailsDeadByFire;
        UIPlayerStat _capTotalSnailsDeadBySpikes;
        UIPlayerStat _capTotalSnailsDeadByDynamite;
        UIPlayerStat _capTotalSnailsDeadByCrate;
        UIPlayerStat _capTotalSnailsDeadByWater;
        UIPlayerStat _capTotalSnailsDeadByLaser;
        UIPlayerStat _capTotalSnailsDeadBySacrifice;
        UIPlayerStat _capTotalSnailsDeadByCrateExplosion;
        UIPlayerStat _capTotalSnailsDeadByAcid;
        UIPlayerStat _capTotalSnailsDeadByOutOfStage;
        UIPlayerStat _capTotalSnailsDeadByEvilSnail;
        UIPlayerStat _capSnailsDeadInDifferentWays;
        UIPlayerStat _capTotalGoldMedals;
        UIPlayerStat _capTotalSilverMedals;
        UIPlayerStat _capTotalBronzeMedals;
        UIPlayerStat _capTotalBoots;

        UIPanel _pnlAchievs;

        UIBackButton _btnBack;
        UISnailsButton _btnReset;
        UISnailsButton _btnClearTrophies;
        UISnailsButton _btnStats;
        UISnailsButton _btnAchievs;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public PlayerStatsScreen(ScreenNavigator owner) :
            base(owner, ScreenType.PlayerStats)
        {
            //this._colorCaptions = Color.White;
            //this._colorValues = Color.Cyan;

            // Title
            this._title = new UISnailsMenuTitle(this);
            this._title.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly | AlignModes.Top;
            this._title.TextResourceId = "TITLE_PLAYER_STATS";
            this.Controls.Add(this._title);

            // Scrollable panel
            this._panel = new UISnailsScrollablePanel(this, UIScrollablePanel.PanelOrientation.Vertical, (SnailsGame.Settings.UseTouch == false), 10000f);
            this._panel.ParentAlignment = AlignModes.Horizontaly;
            this._panel.Size = new Size(7000f, 7500f);
            this._panel.Position = new Vector2(0f, 2000f);
            this.Controls.Add(this._panel);

            // Stats panel
            this._pnlStats = new UIPanel(this);
            this._panel.Controls.Add(this._pnlStats);

            Vector2 pos = new Vector2(LEFT_MARGIN, TOP_MARGIN); 
            // Playing time
            this._capPlayingTime = this.AddStat(ref pos, "LBL_PLAY_TIME", UIPlayerStat.PlayerStatType.PlayingTime);
            this._capRunningTime = this.AddStat(ref pos, "LBL_RUNNING_TIME", UIPlayerStat.PlayerStatType.RunningTime);
            this._capTotalSnailsSafe = this.AddStat(ref pos, "LBL_TOT_SNAILS_SAFE", UIPlayerStat.PlayerStatType.TotalSnailsSafe);
            this._capTotalSnailsKingSafe = this.AddStat(ref pos, "LBL_TOT_KING_SAFE", UIPlayerStat.PlayerStatType.TotalSnailsKingSafe);
            this._capTotalSnailsDeadByFire = this.AddStat(ref pos, "LBL_TOT_DEAD_FIRE", UIPlayerStat.PlayerStatType.TotalSnailsDeadByFire);
            this._capTotalSnailsDeadBySpikes = this.AddStat(ref pos, "LBL_TOT_DEAD_SPIKES", UIPlayerStat.PlayerStatType.TotalSnailsDeadBySpikes);
            this._capTotalSnailsDeadByDynamite = this.AddStat(ref pos, "LBL_TOT_DEAD_DYNAMITE", UIPlayerStat.PlayerStatType.TotalSnailsDeadByDynamite);
            this._capTotalSnailsDeadByCrate = this.AddStat(ref pos, "LBL_TOT_DEAD_CRATE", UIPlayerStat.PlayerStatType.TotalSnailsDeadByCrate);
            this._capTotalSnailsDeadByWater = this.AddStat(ref pos, "LBL_TOT_DEAD_WATER", UIPlayerStat.PlayerStatType.TotalSnailsDeadByWater);
            this._capTotalSnailsDeadByLaser = this.AddStat(ref pos, "LBL_TOT_DEAD_LASER", UIPlayerStat.PlayerStatType.TotalSnailsDeadByLaser);
            this._capTotalSnailsDeadBySacrifice = this.AddStat(ref pos, "LBL_DEAD_SACRIFICE", UIPlayerStat.PlayerStatType.TotalSnailsDeadBySacrifice);
            this._capTotalSnailsDeadByCrateExplosion = this.AddStat(ref pos, "LBL_DEAD_EXPLOSION", UIPlayerStat.PlayerStatType.TotalSnailsDeadByCrateExplosion);
            this._capTotalSnailsDeadByAcid = this.AddStat(ref pos, "LBL_DEAD_ACID", UIPlayerStat.PlayerStatType.TotalSnailsDeadByAcid);
            this._capTotalSnailsDeadByOutOfStage = this.AddStat(ref pos, "LBL_DEAD_OUTOFSTAGE", UIPlayerStat.PlayerStatType.TotalSnailsDeadByOutOfStage);
            this._capTotalSnailsDeadByEvilSnail = this.AddStat(ref pos, "LBL_DEAD_EVILSNAIL", UIPlayerStat.PlayerStatType.TotalSnailsDeadByEvilSnail);
            this._capSnailsDeadInDifferentWays = this.AddStat(ref pos, "LBL_DEAD_DIFFERENTWAYS", UIPlayerStat.PlayerStatType.SnailsDeadInDifferentWays);
            this._capTotalBronzeMedals = this.AddStat(ref pos, "LBL_TOT_BRONZE_MEDALS", UIPlayerStat.PlayerStatType.TotalBronzeMedals);
            this._capTotalSilverMedals = this.AddStat(ref pos, "LBL_SILVER_MEDALS", UIPlayerStat.PlayerStatType.TotalSilverMedals);
            this._capTotalGoldMedals = this.AddStat(ref pos, "LBL_TOT_GOLD_MEDALS", UIPlayerStat.PlayerStatType.TotalGoldMedals);
            this._capTotalBoots = this.AddStat(ref pos, "LBL_TOT_BOOSTS", UIPlayerStat.PlayerStatType.TotalBoots);
            this._pnlStats.Size = new Size(this._panel.Width, pos.Y + BOTTOM_MARGIN);

            // Achievement panel
            this._pnlAchievs = new UIPanel(this);
            this._panel.Controls.Add(this._pnlAchievs);
            this.AddAchievements();

            // Back button
            this._btnBack = new UIBackButton(this);
            this._btnBack.ScreenAlignment = UIBackButton.ButtonScreenAlignment.BottomLeft;
            this._btnBack.OnAccept += new UIControl.UIEvent(this.btnBack_OnPress);
            this.Controls.Add(this._btnBack);

            // Button reset
            this._btnReset = new UISnailsButton(this, "BTN_RESET_STATS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnReset_OnPress, true);
            this._btnReset.ParentAlignment = AlignModes.Left | AlignModes.Bottom;
            this._btnReset.Margins.Right = 150;
            this._btnReset.Margins.Bottom = 1500;
            this._btnReset.Scale = this.FromNativeResolution(this._btnReset.Scale);
            this.Controls.Add(this._btnReset);

            // Button clear trophies
            this._btnClearTrophies = new UISnailsButton(this, "BTN_RESET_TROPHIES", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnClearTrophies_OnPress, true);
            this._btnClearTrophies.Name = "_btnClearTrophies";
            this._btnClearTrophies.ParentAlignment = AlignModes.Left | AlignModes.Bottom;
            this._btnClearTrophies.Margins.Right = 150;
            this._btnClearTrophies.Margins.Bottom = 2750;
            this._btnClearTrophies.Scale = this.FromNativeResolution(this._btnClearTrophies.Scale);
            this.Controls.Add(this._btnClearTrophies);

            // Button stats
            this._btnStats = new UISnailsButton(this, "BTN_VIEW_STATS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnStats_OnPress, true);
            this._btnStats.ParentAlignment = AlignModes.Left | AlignModes.Bottom;
            this._btnStats.Margins.Right = 150;
            this._btnStats.Margins.Bottom = 4000;
            this._btnStats.Scale = this.FromNativeResolution(this._btnStats.Scale);
            this.Controls.Add(this._btnStats);

            // Button achievements
            this._btnAchievs = new UISnailsButton(this, "BTN_VIEW_ACHIEVEMENTS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnAchievs_OnPress, true);
            this._btnAchievs.ParentAlignment = AlignModes.Left | AlignModes.Bottom;
            this._btnAchievs.Margins.Right = 150;
            this._btnAchievs.Margins.Bottom = 5250;
            this._btnAchievs.Scale = this.FromNativeResolution(this._btnAchievs.Scale);
            this.Controls.Add(this._btnAchievs);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this.RefreshValues();
            this.ActivateStatsPanel();
            this.EnableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActivateStatsPanel()
        {
            this._pnlStats.Visible = true;
            this._pnlAchievs.Visible = false;
            this._panel.Length = this._pnlStats.Height;
            this._panel.ScrollToTop();
            this.RefreshValues();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActivateAchievementsPanel()
        {
            this._pnlStats.Visible = false;
            this._pnlAchievs.Visible = true;
            this._panel.Length = this._pnlAchievs.Height;
            this._panel.ScrollToTop();
            this.RefreshValues();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshValues()
        {
            // Stats
            this._capPlayingTime.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalPlayingTime;
            this._capPlayingTime.CaptionFormat = UIValuedCaption.CaptionMode.FulltimeHours;
            this._capRunningTime.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalRunningTime;
            this._capRunningTime.CaptionFormat = UIValuedCaption.CaptionMode.FulltimeHours;
            this._capTotalSnailsSafe.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
            this._capTotalSnailsKingSafe.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe;
            this._capTotalSnailsDeadByFire.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire;
            this._capTotalSnailsDeadBySpikes.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes;
            this._capTotalSnailsDeadByDynamite.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite;
            this._capTotalSnailsDeadByCrate.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrate;
            this._capTotalSnailsDeadByWater.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByWater;
            this._capTotalSnailsDeadByLaser.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser;
            this._capTotalSnailsDeadBySacrifice.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySacrifice;
            this._capTotalSnailsDeadByCrateExplosion.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrateExplosion;
            this._capTotalSnailsDeadByAcid.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByAcid;
            this._capTotalSnailsDeadByOutOfStage.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage;
            this._capTotalSnailsDeadByEvilSnail.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByEvilSnail;
            this._capSnailsDeadInDifferentWays.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.SnailsDeadInDifferentWays;
            this._capTotalGoldMedals.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
            this._capTotalSilverMedals.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
            this._capTotalBronzeMedals.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
            this._capTotalBoots.Value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts;

            // Achievements
            foreach (UIAchievement achiev in this._pnlAchievs.Controls)
            {
                achiev.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddAchievements()
        {
            Vector2 pos = new Vector2(LEFT_MARGIN, TOP_MARGIN);
            foreach (BrainAchievement ba in BrainGame.AchievementsManager.Achievements.Values)
            {
                if (!SnailsGame.GameSettings.WithAppStore &&
                    ba.ShowOnAppStore) // skip achievements that only appear in App Stores
                {
                    continue;
                }

                UIAchievement achiev = new UIAchievement(this, ba);
                achiev.Position = pos;
#if DEBUG
                achiev.AllowToggle = true;
#endif
                this._pnlAchievs.Controls.Add(achiev);
                pos += new Vector2(0, achiev.Height + LINE_SPACING);
            }
            this._pnlAchievs.Height = pos.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        private UIPlayerStat AddStat(ref Vector2 pos, string textResource, UIPlayerStat.PlayerStatType statType)
        {
            UIPlayerStat caption = new UIPlayerStat(this, textResource, pos, statType);
            caption.OnReset += this.ResetStat;
            this._pnlStats.Controls.Add(caption);
            pos += new Vector2(0f, caption.Height + LINE_SPACING);
            return caption;
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

        /// <summary>
        /// 
        /// </summary>
        void btnAchievs_OnPress(IUIControl sender)
        {
            this.ActivateAchievementsPanel();
        }

        /// <summary>
        /// 
        /// </summary>
        void btnStats_OnPress(IUIControl sender)
        {
            this.ActivateStatsPanel();
        }

        /// <summary>
        /// 
        /// </summary>
        void btnReset_OnPress(IUIControl sender)
        {
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalPlayingTime = TimeSpan.Zero;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalRunningTime = TimeSpan.Zero;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrate = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByWater = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySacrifice = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrateExplosion = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByAcid = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByEvilSnail = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins = 0;
            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts = 0;
            SnailsGame.ProfilesManager.Save();
            this.RefreshValues();
        }

        /// <summary>
        /// 
        /// </summary>
        void btnClearTrophies_OnPress(IUIControl sender)
        {
            SnailsGame.ProfilesManager.CurrentProfile.ClearAchievements();
            SnailsGame.ProfilesManager.Save();
            this.RefreshValues();
        }

        /// <summary>
        /// 
        /// </summary>
        void ResetStat(IUIControl sender, UIPlayerStat.PlayerStatType stat)
        {
            switch (stat)
            {
                case UIPlayerStat.PlayerStatType. PlayingTime:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalPlayingTime = TimeSpan.Zero;
                    break;
                case UIPlayerStat.PlayerStatType. RunningTime:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalRunningTime = TimeSpan.Zero;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsSafe:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsKingSafe:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsDeadByFire:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsDeadBySpikes:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsDeadByDynamite:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsDeadByCrate:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrate = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsDeadByWater:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByWater = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsDeadByLaser:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSnailsDeadBySacrifice:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySacrifice = 0;
                    break;
                case UIPlayerStat.PlayerStatType.TotalSnailsDeadByCrateExplosion:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrateExplosion = 0;
                    break;
                case UIPlayerStat.PlayerStatType.TotalSnailsDeadByAcid:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByAcid = 0;
                    break;
                case UIPlayerStat.PlayerStatType.TotalSnailsDeadByOutOfStage:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage = 0;
                    break;
                case UIPlayerStat.PlayerStatType.TotalSnailsDeadByEvilSnail:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByEvilSnail = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalGoldMedals:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalSilverMedals:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalBronzeMedals:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins = 0;
                    break;
                case UIPlayerStat.PlayerStatType. TotalBoots:
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts = 0;
                    break;
            }
            SnailsGame.ProfilesManager.Save();
            this.RefreshValues();
        }
    }
}
