namespace Scoreoid.Kit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class SKLocalPlayer
    {
        #region Constants and Fields

        private static SKLocalPlayer _localPlayer;

        #endregion

        #region Constructors and Destructors

        private SKLocalPlayer(string username, string password)
        {
            this.Username = username;
            this.Password = password;
                        
            this.Platform = SKSettings.Platform;
        }

        #endregion

        #region Properties

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            private set;
        }
        
        public string Email
        {
            get;
            private set;
        }

        public string Platform
        {
            get;
            private set;
        }

        internal static SKLocalPlayer LocalPlayer
        {
            get
            {
                if (_localPlayer == null)
                    throw new InvalidOperationException("Player must be created with CreatePlayer() first.");

                return _localPlayer;
            }
        }

        #endregion

        #region Public Methods

        public static SKLocalPlayer CreatePlayer()
        {
            _localPlayer =
                new SKLocalPlayer(SKSettings.PlayerUsername, SKSettings.PlayerPassword);

            return _localPlayer;
        }

        public void CreateNew(Action<SKError> callback)
        {
            SKWebHelper.SubmitRequest("createPlayer", SKSettings.Apikey, SKSettings.GameID,
                // Parameters
                new 
                { 
                    username = this.Username, 
                    password = this.Password,
                },
                // Success
                xml =>
                {                    
                    try
                    {
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
        
        public void Authenticate(Action<SKError> callback)
        {
            SKWebHelper.SubmitRequest("getPlayer", SKSettings.Apikey, SKSettings.GameID,
                // Parameters
                new 
                { 
                    username = this.Username, 
                    password = this.Password,
                    email = this.Email 
                },
                // Success
                xml =>
                {                    
                    /*    
                        <players>
                            <player username="" 
                                password="" 
                                unique_id="" 
                                first_name="" 
                                last_name="" 
                                email=""
                                created="" 
                                updated="">
                            </player>
                        </players>

                    */

                    try
                    {
                        XElement xplayers = xml.Element("players");
                        if (xplayers == null)
                            throw new InvalidOperationException("Scoreid was unable to authenticate.");

                        XElement xplayer = xplayers.Element("player");
                        if (xplayer == null)
                            throw new InvalidOperationException("Scoreid was unable to authenticate.");

                        // Populate user specific data

                        this.Email = xplayer.Attribute("email").Value;

                        // Notify currently logged OS
                       // NotifyCurrentOS();

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
                    callback(new SKError(error));

                });
        }

        #endregion

        #region Methods

        private void NotifyCurrentOS()
        {
            this.Platform = SKSettings.Platform;
            
            SKWebHelper.SubmitRequest("editPlayer", SKSettings.Apikey, SKSettings.GameID,
                // Parameters
                new 
                { 
                    username = this.Username, 
                    platform = SKSettings.Platform
                },
                // Success
                xml =>
                {
                    /* Do Nothing */
                },
                // Failure
                error =>
                {
                    /* Do Nothing */
                });
        }

        #endregion
    }
}
