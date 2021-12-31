using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.Leaderboards
{
    class LeaderboardPlayerData
    {
        public int Ranking { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public TimeSpan Time { get; set; }

        public LeaderboardPlayerData()
        {
        }

        public LeaderboardPlayerData(int ranking, string username, int score, TimeSpan time)
        {
            this.Ranking = ranking;
            this.Username = username;
            this.Score = score;
            this.Time = time;
        }
    }
}
