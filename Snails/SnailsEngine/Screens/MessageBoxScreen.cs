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

namespace TwoBrainsGames.Snails.Screens
{
    class MessageBoxScreen : SnailsScreen
    {
        UIPanel _pnlBody;
        UISnailsBoard _snailsBoard;
        UISnailsButton _btnCancel;
        UICaption _capMsg;

        public MessageBoxScreen(ScreenNavigator owner) :
            base(owner, ScreenType.MessageBox)
        {
            this.Name = "HowToPlay";
            this.BackgroundColor = new Color(0, 0, 0, 200);
            this.OnBack += new BrainEngine.UI.Controls.UIControl.UIEvent(MessageBoxScreen_OnBack);

            // Body
            this._pnlBody = new UIPanel(this);
            this._pnlBody.ParentAlignment = AlignModes.HorizontalyVertically;
            this._pnlBody.Size = new Size(6000f, 5000f);
            this._pnlBody.BackgroundColor = Color.Red;
         //   this.Controls.Add(this._pnlBody);

            // Board
            this._snailsBoard = new UISnailsBoard(this, UISnailsBoard.BoardType.LightWoodMediumLong);
            this._snailsBoard.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
          //  this._pnlBody.Controls.Add(this._snailsBoard);

            // Button - Cancel
            this._btnCancel = new UISnailsButton(this, "BTN_CANCEL", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnCancel_OnCancel, true);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
            this._btnCancel.ButtonAction = UISnailsButton.ButtonActionType.Back;
            this.Controls.Add(this._btnCancel);


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
            this._capMsg.Text = this.Navigator.GlobalCache.Get<string>(GlobalCacheKeys.MESSAGE_BOX_MESSAGE);
        }


        /// <summary>
        /// 
        /// </summary>
        void MessageBoxScreen_OnBack(BrainEngine.UI.Controls.IUIControl sender)
        {
            this.Close();
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
