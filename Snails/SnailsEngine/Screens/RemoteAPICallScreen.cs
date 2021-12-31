using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.RemoteServices;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens
{
    class RemoteAPICallScreen : SnailsScreen
    {
        public static RemoteServicesManager.RemoteAPICallback OnAPICallCallback { get; set; }

        UISnailsButton _btnClose;
        UICaption _capMsg;

        public RemoteAPICallScreen(ScreenNavigator owner) :
            base(owner, ScreenType.MessageBox)
        {
            this.Name = "HowToPlay";
            this.BackgroundColor = new Color(0, 0, 0, 200);

            // Button - Cancel
            this._btnClose = new UISnailsButton(this, "BTN_CANCEL", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnCancel_OnCancel, true);
            this._btnClose.Name = "_btnClose";
            this._btnClose.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
            this._btnClose.ButtonAction = UISnailsButton.ButtonActionType.Back;
            this.Controls.Add(this._btnClose);

            // Msg
            this._capMsg = new UICaption(this, "", Color.Red, UICaption.CaptionStyle.NormalText);
            this._capMsg.ParentAlignment = AlignModes.HorizontalyVertically;
            this.Controls.Add(this._capMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this._capMsg.Text = "Authenticating. Please wait...";
            this._btnClose.Visible = false;
            this._capMsg.BlendColor = Color.LightBlue;

            RPCCalls.RPCMethod method = SnailsGame.ScreenNavigator.GlobalCache.Get<RPCCalls.RPCMethod>(GlobalCacheKeys.RPC_METHOD);
            switch (method)
            {
                case RPCCalls.RPCMethod.Login:
                    RPCCalls.NewUser_RemoteCallParams loginPar = SnailsGame.ScreenNavigator.GlobalCache.Get<RPCCalls.NewUser_RemoteCallParams>(GlobalCacheKeys.RPC_PARAM);
                    BrainGame.RemoteServicesManager.Login(loginPar.Username, loginPar.Password, this.LoginCallBack);
                    break;

                case RPCCalls.RPCMethod.CreateNewUser:
                    RPCCalls.NewUser_RemoteCallParams createUserPar = SnailsGame.ScreenNavigator.GlobalCache.Get<RPCCalls.NewUser_RemoteCallParams>(GlobalCacheKeys.RPC_PARAM);
                    BrainGame.RemoteServicesManager.CreateNewUser(createUserPar.Username, createUserPar.Password, this.CreateUserCallBack);
                    break;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        private void LoginCallBack(RemoteAPICallResult result)
        {
            if (result.WithError)
            {
                this.ShowError(result.Message);
            }
            else
            {
                this.Success(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateUserCallBack(RemoteAPICallResult result)
        {
            if (result.WithError)
            {
                this.ShowError(result.Message);
            }
            else
            {
                this.Success(result);
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Success(RemoteAPICallResult result)
        {
            if (RemoteAPICallScreen.OnAPICallCallback != null)
            {
                RemoteAPICallScreen.OnAPICallCallback(result);
            }
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowError(string message)
        {
            this._capMsg.Text = message;
            this._capMsg.BlendColor = Color.Red;
            this._btnClose.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void btnCancel_OnCancel(IUIControl sender)
        {
            this.Close();
        }
    }
}