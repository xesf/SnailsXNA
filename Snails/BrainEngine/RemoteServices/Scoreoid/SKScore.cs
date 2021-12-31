namespace Scoreoid.Kit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class SKScore
    {
        #region Constructors and Destructors

        public SKScore(string leaderboardID)
            : this(leaderboardID, null, 0, null)
        {
        }

        internal SKScore(string leaderboardID, string username, int value, DateTime? creationDate)            
        {
            this.LeaderboardID = leaderboardID;

            this.Username = username;

            this.Value = value;
            this.CreationDate = creationDate;

            this.Platform = SKLocalPlayer.LocalPlayer.Platform;
        }

        #endregion

        #region Properties

        public string LeaderboardID
        {
            get;
            private set;
        }

        public string Username
        {
            get;
            private set;
        }
        
        public int Value
        {
            get;
            set;
        }

        public DateTime? CreationDate
        {
            get;
            private set;
        }

        public string Platform
        {
            get;
            private set;
        }

        #endregion

        #region Public Methods

        public void Submit(Action<SKError> callback)
        {
            SKWebHelper.SubmitRequest("createScore", SKSettings.Apikey, SKSettings.GameID,
                // Parameters
                new 
                { 
                    score = this.Value, 
                    username = SKLocalPlayer.LocalPlayer.Username, 
                    platform = this.Platform,
                    difficulty = Int32.Parse(this.LeaderboardID) 
                },            
                // Success
                xml =>
                {                    
                    try
                    {
                        XElement xsuccess = xml.Element("success");
                        if (xsuccess == null)
                            throw new InvalidOperationException("Scoreid was unable to submit the score.");                        

                        callback(null);
                    }
                    catch(Exception ex)
                    {
                        callback(new SKError(ex.Message));
                    }

                },
                // Failure
                error =>
                {
                    callback(new SKError("Scoreid was unable to connect to the server."));

                });
        }

        #endregion
    }
}
