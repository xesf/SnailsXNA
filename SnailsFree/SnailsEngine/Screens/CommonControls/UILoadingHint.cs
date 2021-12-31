using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UILoadingHint : UIControl
    {
        const float TIP_LEFT_MARGIN = 200f;
        string[] HintMessagesRes = new string[13] { "MSG_HINT1", "MSG_HINT2", "MSG_HINT3", "MSG_HINT4", 
                                                    "MSG_HINT5", "MSG_HINT6", "MSG_HINT7", "MSG_HINT8", 
                                                    "MSG_HINT9", "MSG_HINT10" , "MSG_HINT11", "MSG_HINT12",
                                                    "MSG_HINT13"};

        UICaption _capHintMessage;
        UICaption _capHint;

        public int MessageSize { get { return this._capHintMessage.Text.Length; } }

        public UILoadingHint(UIScreen screenOwner) :
            base(screenOwner)
        {

            this.BackgroundColor = new Color(0, 0, 0, 180);
            this.ParentAlignment = AlignModes.Bottom;
			this.Margins.Bottom = SnailsScreen.BottomMargin + this.PixelsToScreenUnitsY(-1f);
            this.Size = new Size(UIScreen.MAX_SCREEN_WITDH_IN_POINTS, this.NativeResolutionY(1500f));

            // Hint caption
            this._capHint = new UICaption(screenOwner, "", new Color(255, 255, 60), UICaption.CaptionStyle.LoadingTip);
            this._capHint.ParentAlignment = AlignModes.Top;
            this._capHint.Margins.Top = this.NativeResolutionY(150f);
            this._capHint.Position = this.NativeResolution(new Vector2(TIP_LEFT_MARGIN, 0f));
            this._capHint.TextResourceId = "LBL_TIP";
            this.Controls.Add(this._capHint);

            // Hint message
            this._capHintMessage = new UICaption(screenOwner, "", new Color(116, 227, 255), UICaption.CaptionStyle.LoadingTip);
            this._capHintMessage.Position = this.NativeResolution(new Vector2(TIP_LEFT_MARGIN, 500f));
            this._capHintMessage.LineSpacing = this.NativeResolutionY(50f);
            this._capHintMessage.HorizontalAligment = HorizontalTextAligment.Left;
            this.Controls.Add(this._capHintMessage);
        }


        /// <summary>
        /// 
        /// </summary>
        public void InitializeHintText()
        {
            this._capHintMessage.Text = this.GetNextHint();
        }

        /// <summary>
        /// 
        /// </summary>
        private string GetNextHint()
        {
            SnailsGame.ProfilesManager.CurrentProfile.LastHintMessageSeen++;
            if (SnailsGame.ProfilesManager.CurrentProfile.LastHintMessageSeen > this.HintMessagesRes.Length)
            {
                SnailsGame.ProfilesManager.CurrentProfile.LastHintMessageSeen = 1;
            }
            if (SnailsGame.ProfilesManager.CurrentProfile.LastHintMessageSeen > this.HintMessagesRes.Length)
            {
                return null;
            }
            //  SnailsGame.ProfilesManager.Save();
            return LanguageManager.GetString(this.HintMessagesRes[SnailsGame.ProfilesManager.CurrentProfile.LastHintMessageSeen - 1]);
        }
    }
}
