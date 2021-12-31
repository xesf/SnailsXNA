using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI.Controls;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Screens
{
    class RateScreen : SnailsScreen
    {
        UISnailsWindow _window;
        UICaption _capMessage;
        UIImage _imgStars;

        public RateScreen(ScreenNavigator owner) :
            base(owner, ScreenType.Rate)
        {
            // Window
            this._window = new UISnailsWindow(this);
            this._window.BoardType = UISnailsBoard.BoardType.LightWoodLongNarrow;
            this._window.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._window.ButtonCaptionType = UISnailsWindow.ButtonType.YesNo;
            this._window.CloseButtonVisible = false;
            this._window.OnYes += new UIControl.UIEvent(_window_OnRate);
            this._window.OnNo += new UIControl.UIEvent(_window_OnClose);
            this._window.WithTitle = true;
            this._window.Button2TextResource = "BTN_RATE";
            this._window.Button1TextResource = "BTN_IGNORE";
            this.Controls.Add(this._window);

            // Caption
            this._capMessage = new UICaption(this, "", Color.Orange, UICaption.CaptionStyle.NormalText);
            this._capMessage.TextResourceId = "MSG_PLEASE_RATE";
            this._capMessage.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._capMessage.Position = new Vector2(0f, 700f);
            this._capMessage.LineSpacing = 150f;
            this._capMessage.HorizontalAligment = BrainEngine.UI.HorizontalTextAligment.Center;
            this._window.Controls.Add(this._capMessage);

            // Stars
            this._imgStars = new UIImage(this, "spriteset/common-elements-1/RateStars");
            this._imgStars.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._imgStars.Position = new Vector2(0f, 2900f);
            this._imgStars.Margins.Bottom = 1000f;
            this._window.Controls.Add(this._imgStars);

        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this._window.Body.Scale = Vector2.One;

        }

        /// <summary>
        /// 
        /// </summary>
        void _window_OnClose(IUIControl sender)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        void _window_OnRate(IUIControl sender)
        {
            SnailsGame.OpenStoreReviewPanel();
            this.Close();
        }
    }
}
