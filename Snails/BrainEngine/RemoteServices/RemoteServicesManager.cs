using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scoreoid.Kit;


namespace TwoBrainsGames.BrainEngine.RemoteServices
{
    public class RemoteServicesManager
    {
        public delegate void RemoteAPICallback (RemoteAPICallResult result);

        public enum ManagerState
        {
            Idle,
            Executing,
            ExecutionEnded
        }

        internal string ApiKey 
        {
            get { return SKSettings.Apikey; }
            set { SKSettings.Apikey = value; }
        }

        internal string GameId 
        {
            get { return SKSettings.GameID; }
            set { SKSettings.GameID = value; }
        }

        internal string Platform 
        {
            get { return SKSettings.Platform; }
            set { SKSettings.Platform = value; }
        }

        public ManagerState State { get; private set;}
        private RemoteAPICallback OnAPICallCallback { get; set;}
        private RemoteAPICallResult APICallResult { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public RemoteServicesManager()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateNewUser(string username, string password, RemoteAPICallback callback)
        {
            this.AssertNotExecuting();

            SKSettings.PlayerUsername = username;
            SKSettings.PlayerPassword = password;

            SKLocalPlayer localPlayer = SKLocalPlayer.CreatePlayer();
            this.State = ManagerState.Executing;
            this.OnAPICallCallback = callback;
            localPlayer.CreateNew(this.APICallBack);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Login(string username, string password, RemoteAPICallback callback)
        {
            this.AssertNotExecuting();

            SKSettings.PlayerUsername = username;
            SKSettings.PlayerPassword = password;

            SKLocalPlayer localPlayer = SKLocalPlayer.CreatePlayer();
            this.State = ManagerState.Executing;
            this.OnAPICallCallback = callback;
            localPlayer.Authenticate(this.APICallBack);
        }

        /// <summary>
        /// 
        /// </summary>
        private void APICallBack<T>(T errObj) where T: SKError
        {
            this.State = ManagerState.ExecutionEnded;
            this.APICallResult = new RemoteAPICallResult(((SKError)errObj).LocalizedDescription);
            if (errObj is SKError)
            {
                this.APICallResult.WithError = true;
                this.APICallResult.Message = ((SKError)errObj).LocalizedDescription;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AssertNotExecuting()
        {
            if (this.State == ManagerState.Executing)
            {
                throw new BrainException("Cannot access Remote Services because");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            if (this.State == ManagerState.ExecutionEnded)
            {
                this.State = ManagerState.Idle;
                if (this.OnAPICallCallback != null)
                {
                    this.OnAPICallCallback(this.APICallResult);
                }
            }
        }
    }
}
