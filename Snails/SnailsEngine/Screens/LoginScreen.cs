using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;
//using TwoBrainsGames.BrainEngine.RemoteServices;

namespace TwoBrainsGames.Snails.Screens
{
    class LoginScreen : SnailsScreen
    {
        UIPanel _pnlBody;
        UISnailsBoard _snailsBoard;
        UICaption _capMsg;
        UICaption _capUsername;
        UITextBox _txtUsername;
        UISnailsButton _btnOk;
        UISnailsButton _btnSkip;

        private string Password { get; set; }
        private string Username { get { return this._txtUsername.Text; } }
 
        public LoginScreen(ScreenNavigator owner) :
            base(owner, ScreenType.Login)
        {
            this.Name = "HowToPlay";
            this.BackgroundColor = new Color(0, 0, 0, 200);
            this.OnBack += new BrainEngine.UI.Controls.UIControl.UIEvent(UsernamePasswordScreen_OnBack);
            
            // Body
            this._pnlBody = new UIPanel(this);
            this._pnlBody.ParentAlignment = AlignModes.HorizontalyVertically;
            this._pnlBody.Size = new Size(6000f, 8700f);
            this.Controls.Add(this._pnlBody);

            // Board
            this._snailsBoard = new UISnailsBoard(this, UISnailsBoard.BoardType.LightWoodMedium);
            this._snailsBoard.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._pnlBody.Controls.Add(this._snailsBoard);

            // Msg
            this._capMsg = new UICaption(this, "", Color.Yellow, UICaption.CaptionStyle.NormalText);
            this._capMsg.Position = new Vector2(500f, 800f);
            this._capMsg.TextResourceId = "MSG_LOGIN";
            this._snailsBoard.Controls.Add(this._capMsg);

            // Username label 
            this._capUsername = new UICaption(this, "", Color.Orange, UICaption.CaptionStyle.NormalText);
            this._capUsername.Position = new Vector2(900f, 2500f);
            this._capUsername.TextResourceId = "LBL_USERNAME";
            this._snailsBoard.Controls.Add(this._capUsername);

            // Username
            this._txtUsername = new UITextBox(this, BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static), 10);
            this._txtUsername.Position = new Vector2(900f, 3100f);
            this._txtUsername.Size = new BrainEngine.UI.Size(3000f, 800f);
            this._txtUsername.KeyboardInputTitleResourceId = "MSG_KEYB_INPUT_TITLE";
            this._txtUsername.KeyboardInputDescriptionResourceId = "MSG_KEYB_INPUT_USER";
            this._snailsBoard.Controls.Add(this._txtUsername);

            // Button - Ok
            this._btnOk = new UISnailsButton(this, "BTN_LOGIN", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnOk_OnAccept, true);
            this._btnOk.Name = "_btnOk";
            this._btnOk.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
            this._btnOk.ParentAlignmentOffset = new Vector2(500f, 0f);
            this._btnOk.ButtonAction = UISnailsButton.ButtonActionType.Undefined;
            this._pnlBody.Controls.Add(this._btnOk);

            // Button - Skip
            this._btnSkip = new UISnailsButton(this, "BTN_SKIP", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this._btnSkip_OnAccept, true);
            this._btnSkip.Name = "_btnSkip";
            this._btnSkip.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
            this._btnSkip.ButtonAction = UISnailsButton.ButtonActionType.Undefined;
            this._btnSkip.ParentAlignmentOffset = new Vector2(-500f, 0f);
            this._pnlBody.Controls.Add(this._btnSkip);

        }


        /// <summary>
        /// 
        /// </summary>
        void UsernamePasswordScreen_OnBack(BrainEngine.UI.Controls.IUIControl sender)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void btnOk_OnAccept(IUIControl sender)
        {
            this.Password = Guid.NewGuid().ToString();
            SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.RPC_METHOD, RPCCalls.RPCMethod.CreateNewUser);
            SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.RPC_PARAM, new RPCCalls.NewUser_RemoteCallParams(this.Username, this.Password));
            //RemoteAPICallScreen.OnAPICallCallback = this.CreateUserCallBack;
            SnailsGame.ScreenNavigator.PopUp(ScreenType.RemoteAPICall.ToString());
            /*
            SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.RPC_METHOD, RPCCalls.RPCMethod.Login);
            SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.RPC_PARAM, new RPCCalls.NewUser_RemoteCallParams(this._txtUsername.Text, password));
            RemoteAPICallScreen.OnAPICallCallback = this.LoginCallBack;
            SnailsGame.ScreenNavigator.PopUp(ScreenType.RemoteAPICall.ToString());*/
        }


        /// <summary>
        /// 
        /// </summary>
        protected void _btnSkip_OnAccept(IUIControl sender)
        {
            this.Close();
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        protected void btnNewUser_OnNewUser(IUIControl sender)
        {
            SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.RPC_METHOD, RPCCalls.RPCMethod.CreateNewUser);
            SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.RPC_PARAM, new RPCCalls.NewUser_RemoteCallParams(this._txtUsername.Text, this._txtPassword.Text));
            RemoteAPICallScreen.OnAPICallCallback = this.CreateUserCallBack;
            SnailsGame.ScreenNavigator.PopUp(ScreenType.RemoteAPICall.ToString());
        }
        */
        /// <summary>
        /// 
        /// </summary>
        /*private void LoginCallBack(RemoteAPICallResult result)
        {
            if (!result.WithError)
            {
                SnailsGame.ProfilesManager.CurrentProfile.RemoteServicesPassword = this.Password;
                SnailsGame.ProfilesManager.CurrentProfile.RemoteServicesPassword = this.Username;
                SnailsGame.ProfilesManager.Save();
                this.Close();
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /*private void CreateUserCallBack(RemoteAPICallResult result)
        {
            if (!result.WithError)
            {
                SnailsGame.ProfilesManager.CurrentProfile.RemoteServicesPassword = this.Password;
                SnailsGame.ProfilesManager.CurrentProfile.RemoteServicesPassword = this.Username;
                SnailsGame.ProfilesManager.Save();
                this.Close();
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        private void ShowError(string message)
        {
            SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.MESSAGE_BOX_MESSAGE, message);
            SnailsGame.ScreenNavigator.PopUp(ScreenType.MessageBox.ToString());
        }
    }
}
