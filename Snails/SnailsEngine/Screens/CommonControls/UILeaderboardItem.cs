using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Leaderboards;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UILeaderboardItem : UIPanel
    {
        LeaderboardPlayerData _leaderboardData;
        
        TextFont Font { get; set; }
        LeaderboardPlayerData LeaderboardData 
        {
            get
            {
                return this._leaderboardData;
            }
            set
            {
                this._leaderboardData = value;
                this.RankingString = this.LeaderboardData.Ranking.ToString();
                this.ScoreString = this.LeaderboardData.Score.ToString();
                this.TimeString = string.Format("{0:00}:{1:00},{2:0}", 
                                this.LeaderboardData.Time.Minutes,
                                this.LeaderboardData.Time.Seconds,
                                (int)Math.Floor((double)(this.LeaderboardData.Time.Milliseconds / 100)));
                if (this.LeaderboardData.Username.ToUpper() == SnailsGame.ProfilesManager.CurrentProfile.Name.ToUpper())
                {
                    this.BlendColor = Color.Yellow;
                    this.BackgroundColor = new Color(0, 0, 0, 50);
                }
             }
        }

        string RankingString { get; set; }
        string ScoreString { get; set; }
        string TimeString { get; set; }
        Vector2 RankingPosInPixels { get; set; }
        Vector2 UsernamePosInPixels { get; set; }
        Vector2 ScorePosInPixels { get; set; }
        Vector2 TimePosInPixels { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UILeaderboardItem(UIScreen screenOwner, LeaderboardPlayerData leaderboardData) :
            base(screenOwner)
        {
            this.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-small", ResourceManager.ResourceManagerCacheType.Static);
            this.Size = new Size(6560f, 500f);
            this.RankingPosInPixels = new Vector2(0f, 0f);
            this.UsernamePosInPixels = new Vector2(80f, 0f);
            this.TimePosInPixels = new Vector2(350f, 0f);
            this.ScorePosInPixels = new Vector2(440f, 0f);
            this.BlendColor = Color.Orange;
            this.BackgroundColor = new Color(0, 0, 0, 20);
            this.LeaderboardData = leaderboardData;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            // Ranking
            this.Font.DrawString(this.SpriteBatch, this.RankingString, this.AbsolutePositionInPixels + this.RankingPosInPixels, Vector2.One, this.BlendColor);
            
            // User
            this.Font.DrawString(this.SpriteBatch, this.LeaderboardData.Username, this.AbsolutePositionInPixels + this.UsernamePosInPixels, Vector2.One, this.BlendColor);

            // Time
            this.Font.DrawString(this.SpriteBatch, this.TimeString, this.AbsolutePositionInPixels + this.TimePosInPixels, Vector2.One, this.BlendColor);

            // Score
            this.Font.DrawString(this.SpriteBatch, this.ScoreString, this.AbsolutePositionInPixels + this.ScorePosInPixels, Vector2.One, this.BlendColor);
        }
    }
}
