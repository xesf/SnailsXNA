namespace Scoreoid.Kit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class SKLeaderboard
    {
        #region Constructors and Destructors

        public SKLeaderboard(string leaderboardID)
        {
            this.LeaderboardID = leaderboardID;

            this.OrderBy = "score";
            this.Direction = "desc";

            this.Platform = SKLocalPlayer.LocalPlayer.Platform;
        }

        #endregion

        #region Properties

        public string LeaderboardID
        {
            get;
            internal set;
        }

        public string OrderBy
        {
            get;
            set;
        }

        public string Direction
        {
            get;
            set;
        }

        public DateTime? FromDate
        {
            get;
            set;
        }

        public DateTime? ToDate
        {
            get;
            set;
        }

        public int PageStart
        {
            get;
            set;
        }
        
        public int PageSize
        {
            get;
            set;
        }

        public string Platform
        {
            get;
            private set;
        }

        #endregion

        #region Public Methods

        public void LoadScores(Action<SKScore[], SKError> callback)
        {
            string limit = String.Format("{0},{1}", this.PageStart * this.PageSize, this.PageSize);
            if (limit == "0,0")
                limit = "10";

            string startDate = null;
            if (this.FromDate.HasValue)
                startDate = this.FromDate.GetValueOrDefault().ToString("yyyy-MM-dd");

            string endDate = null;
            if (this.ToDate.HasValue)
                endDate = this.ToDate.GetValueOrDefault().ToString("yyyy-MM-dd");
            
            SKWebHelper.SubmitRequest("getBestScores", SKSettings.Apikey, SKSettings.GameID,
                // Parameters
                new { 
                    order_by = this.OrderBy, 
                    order = this.Direction, 
                    limit = limit, 
                    start_date = startDate, 
                    end_date = endDate,
                    //platform = this.Platform, 
                    difficulty = Int32.Parse(this.LeaderboardID) 
                },            
                // Success
                xml =>
                {        
                    /*
                        <scores>
                          <player username="" email="" first_name="" last_name="" platform="">
                            <score created="" difficulty="" platform="" score="" >
                            </score>
                          </player>
                        </scores>
                    */
                     
                    try
                    {
                        XElement xscores = xml.Element("scores");
                        if (xscores == null)
                            throw new InvalidOperationException("Scoreid was unable to load scores.");

                        List<SKScore> scores = new List<SKScore>();
                        foreach (XElement xplayer in xscores.Elements("player"))
                        {
                            string username = xplayer.Attribute("username").Value;

                            XElement xscore = xplayer.Element("score");
                            if (xscore == null)
                                continue;

                            string leaderboardID = xscore.Attribute("difficulty").Value;                            

                            DateTime? creationDate = null;
                            DateTime date = DateTime.MinValue;
                            if (DateTime.TryParse(xscore.Attribute("created").Value, out date))
                                creationDate = date;

                            int value = 0;
                            Int32.TryParse(xscore.Attribute("score").Value, out value);

                            scores.Add(new SKScore(leaderboardID, username, value, creationDate));
                        }

                        callback(scores.ToArray(), null);
                    }
                    catch(Exception ex)
                    {
                        callback(null, new SKError(ex.Message));
                    }

                },
                // Failure
                error =>
                {
                    callback(null, new SKError("Scoreid was unable to connect to the server."));

                });
        }

        #endregion
    }
}
