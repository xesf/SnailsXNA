using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Screens;

namespace TwoBrainsGames.Snails.Winforms
{
    public partial class GameSettingsForm : Form
    {
        public GameSettingsForm()
        {
            InitializeComponent();

            this._cmbStartup.Items.Add("Beginning");
            this._cmbStartup.Items.Add("Main menu");
            this._cmbStartup.Items.Add("Stage start");
            this._cmbStartup.Items.Add("Stage editor");
            this._cmbStartup.Items.Add("Awards");

            this._cmbSelectedTheme.Items.Add("Wild Nature");
            this._cmbSelectedTheme.Items.Add("Ancient Egypt");
            this._cmbSelectedTheme.Items.Add("The Graveyard");
            this._cmbSelectedTheme.Items.Add("The Goldmines");

            this._cmbMode.Items.Add(GameSettings.GameplayModeType.Retail);
            this._cmbMode.Items.Add(GameSettings.GameplayModeType.Demo);
            this._cmbMode.Items.Add(GameSettings.GameplayModeType.Beta);

            this._numSelectedStage.Minimum = 1;
            this._numSelectedStage.Maximum = Levels.MAX_NUMBER_STAGES_PER_THEME;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GameSettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                SnailsGame.GameSettings.LoadFromFile();
                this.RefreshData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshData()
        {
            this._cmbStartup.SelectedIndex = (int)SnailsGame.GameSettings.EntryPoint;
            this._chkFullscreen.Checked = SnailsGame.GameSettings.IsFullScreen;
            this._chkShowFps.Checked = SnailsGame.GameSettings.ShowDebugInfo;
            if (SnailsGame.GameSettings.StartupStageNr >= this._numSelectedStage.Minimum &&
                SnailsGame.GameSettings.StartupStageNr <= this._numSelectedStage.Maximum)
            {
                this._numSelectedStage.Value = SnailsGame.GameSettings.StartupStageNr;
            }
            this._cmbSelectedTheme.SelectedIndex = (int)SnailsGame.GameSettings.StartupTheme;
            this._chkShowBB.Checked = SnailsGame.GameSettings.ShowBoundingBoxes;
            this._chkShowSpriteFrame.Checked = SnailsGame.GameSettings.ShowSpriteFrame;
            this._chkDeleteProfile.Checked = SnailsGame.GameSettings.DeletePlayerProfile;
            this._chkWinAlwaysActive.Checked = SnailsGame.GameSettings.WindowAlwaysActive;
            this._cmbMode.SelectedItem = SnailsGame.GameSettings.GameplayMode;
            this._chkUnlockAll.Checked = SnailsGame.GameSettings.AllStagesUnlocked;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                SnailsGame.GameSettings.EntryPoint = (GameSettings.GameEntryPoint)this._cmbStartup.SelectedIndex;
                switch (SnailsGame.GameSettings.EntryPoint)
                {
                    case GameSettings.GameEntryPoint.Beginning: // Beginning
                        SnailsGame.GameSettings.StartupScreenGroup = "Intro";
                        SnailsGame.GameSettings.StartupScreen = "Startup";
                        break;

                    case GameSettings.GameEntryPoint.MainMenu: // Main menu
                        SnailsGame.GameSettings.StartupScreenGroup = "MainMenu";
                        SnailsGame.GameSettings.StartupScreen = "MainMenu";
                        BrainGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleVisibleMenuHidden);
                        break;

                    case GameSettings.GameEntryPoint.StageBriefing: // Stage briefing
                        SnailsGame.GameSettings.StartupScreenGroup = "InGame";
                        SnailsGame.GameSettings.StartupScreen = "StageStart";
                        SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, true);
                        break;
                    case GameSettings.GameEntryPoint.StageEditor: // Stage editor
                        SnailsGame.GameSettings.StartupScreenGroup = "InGame";
                        SnailsGame.GameSettings.StartupScreen = "StageStart";
                        break;

                    case GameSettings.GameEntryPoint.Awards: 
                        SnailsGame.GameSettings.StartupScreenGroup = "MainMenu";
                        SnailsGame.GameSettings.StartupScreen = "Awards";
                        break;
                }

                SnailsGame.GameSettings.ShowDebugInfo = this._chkShowFps.Checked;
                SnailsGame.GameSettings.IsFullScreen = this._chkFullscreen.Checked;
                SnailsGame.GameSettings.StartupTheme = (ThemeType)this._cmbSelectedTheme.SelectedIndex;
                SnailsGame.GameSettings.StartupStageNr = (int)this._numSelectedStage.Value;
                SnailsGame.GameSettings.ShowBoundingBoxes = this._chkShowBB.Checked;
                SnailsGame.GameSettings.ShowSpriteFrame = this._chkShowSpriteFrame.Checked;
                SnailsGame.GameSettings.DeletePlayerProfile = this._chkDeleteProfile.Checked;
                SnailsGame.GameSettings.WindowAlwaysActive = this._chkWinAlwaysActive.Checked;
                SnailsGame.GameSettings.GameplayMode = (GameSettings.GameplayModeType)this._cmbMode.SelectedItem;
                SnailsGame.GameSettings.AllStagesUnlocked = this._chkUnlockAll.Checked;
                SnailsGame.GameSettings.SaveToFile();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void _cmbStartup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
