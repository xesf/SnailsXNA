using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    /// <summary>
    /// This is the message that is displayed in the footer while in the menus
    /// It may be used to display game info, copyright notice, update information, etc
    /// </summary>
    class UIFooterMessage : UIControl
    {
        #region Consts
        const float SPEED = 0.3f;
        #endregion

        #region Vars
        UISpriteFontLabel _lblMessage;
        #endregion

        #region Properties
        private float Speed { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UIFooterMessage(UIScreen screenOwner) :
            base(screenOwner)
        {
            this._lblMessage = new UISpriteFontLabel(screenOwner);
            this.Controls.Add(this._lblMessage);

            this.ParentAlignment = BrainEngine.UI.AlignModes.Bottom;
            this.Speed = UIFooterMessage.SPEED;
            this.Position = new Microsoft.Xna.Framework.Vector2(this.PixelsToScreenUnitsX(SnailsGame.ScreenWidth / 1.5f), 0f);

            this.OnLanguageChanged += new UIEvent(UIFooterMessage_OnLanguageChanged);
            this.AcceptControllerInput = false;
			/*   if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.HD)
            {
                this._lblMessage.Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/footerMessage");
            }
            else
            {
                this._lblMessage.Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/footerMessageWP");
            } */
            this._lblMessage.BlendColor = Color.White;
        }


        /// <summary>
        /// 
        /// </summary>
        void UIFooterMessage_OnLanguageChanged(IUIControl sender)
        {
            this.BuildMessage(SnailsGame.FooterMessages);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            // Get the previous position
            // This is needed because every screen shows the same message but each screen has a different UIFooterMessage
            // Just set the footer message equal to the previous screen position
            this.Position = this.ScreenOwner.Navigator.GlobalCache.Get<Vector2>(GlobalCacheKeys.FOOTER_MESSAGE_POSITION, this.Position);

            this.BuildMessage(SnailsGame.FooterMessages);
            this.BringToFront();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            if (string.IsNullOrEmpty(this._lblMessage.Text))
            {
                return;
            }

            this.Position -= new Microsoft.Xna.Framework.Vector2((float)(this.Speed * gameTime.ElapsedRealTime.TotalMilliseconds), 0f);
            // Wrap message if out of screen
            if (this.Position.X + this.Size.Width < 0)
            {
                this.Position = new Vector2(this.PixelsToScreenUnitsX(SnailsGame.ScreenWidth), this.Position.Y);
            }
            // Update the global var that stores the position
            this.ScreenOwner.Navigator.GlobalCache.Set(GlobalCacheKeys.FOOTER_MESSAGE_POSITION, this.Position);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildMessage(List<string> messages)
        {
            this._lblMessage.Text = string.Empty;
            for (int i = 0; i < messages.Count; i++)
            {
                this._lblMessage.Text += messages[i];
                if (i < messages.Count - 1)
                {
                    this._lblMessage.Text += "      ";
                }
            }
            this.Size = this._lblMessage.Size;
            //this.Position = new Vector2(this.Position.X - (this.Size.Width/2), this.Position.Y);
        }
    }
}
