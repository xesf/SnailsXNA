using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIXBoxControls : UISnailsWindow
    {
        struct ButtonCaption
        {
            public Size _size;
            public Vector2 _position;
            public string _textResourceId;
            public HorizontalTextAligment _alignment;

            public ButtonCaption(string textResId, Vector2 pos, Size size, HorizontalTextAligment alignment)
            {
                this._position = pos;
                this._textResourceId = textResId;
                this._size = size;
                this._alignment = alignment;
            }
        }

       

        #region Vars
        UIImage _imgController;
        #endregion


        ButtonCaption[] _captions = new ButtonCaption[]
                {   
                    new ButtonCaption("LBL_XBOX_PAUSE",           new Vector2(400f, 800f), new Size(1400f, 400f), HorizontalTextAligment.Right),
                    new ButtonCaption("LBL_XBOX_MOVE_CURSOR",     new Vector2(400f, 1300f), new Size(1400f, 400f), HorizontalTextAligment.Right),
                    new ButtonCaption("LBL_XBOX_CURSOR_SNAP",     new Vector2(400f, 1800f), new Size(1400f, 800f), HorizontalTextAligment.Right),
                    new ButtonCaption("LBL_XBOX_QUICK_SEL_TOOL",  new Vector2(4600f, 250f), new Size(1400f, 400f), HorizontalTextAligment.Left),
                    new ButtonCaption("LBL_XBOX_TIME_WARP",       new Vector2(4600f, 1000f), new Size(1400f, 400f), HorizontalTextAligment.Left),
                    new ButtonCaption("LBL_XBOX_DISMISS_TUTORIAL",new Vector2(4600f, 1400f), new Size(1400f, 400f), HorizontalTextAligment.Left),
                    new ButtonCaption("LBL_XBOX_ACTION",          new Vector2(4600f, 1930f), new Size(1400f, 400f), HorizontalTextAligment.Left),
                    new ButtonCaption("LBL_XBOX_MAP_PAN",         new Vector2(4600f, 2550f), new Size(1400f, 400f), HorizontalTextAligment.Left),
                    new ButtonCaption("LBL_XBOX_RESTART",         new Vector2(1950f, 3200f), new Size(2500f, 400f), HorizontalTextAligment.Center)
                    };

        /// <summary>
        /// 
        /// </summary>
        public UIXBoxControls(UIScreen screenOwner) :
            base(screenOwner)
        {

            // Controller image
            this._imgController = new UIImage(screenOwner, "spriteset/common-elements-1/XBoxController", ResourceManager.RES_MANAGER_ID_STATIC);
            this._imgController.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._imgController.Position = new Vector2(0f, 250f);
            this.Board.Controls.Add(this._imgController);

            // Labels
            foreach (ButtonCaption button in this._captions)
            {
                UICaption cap = new UICaption(screenOwner, "", Colors.ControllerHelp, UICaption.CaptionStyle.ControllerHelp);
                cap.TextResourceId = button._textResourceId;
                cap.Autosize = false;
                cap.HorizontalAligment = button._alignment;
                cap.VerticalAligment = VerticalTextAligment.Center;
                cap.Position = button._position;
                cap.Size = button._size;
                this.Board.Controls.Add(cap);
            }

            this.TitleResourceId = "TITLE_GAME_CONTROLS";
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
         /*   if (this.AcceptControllerInput)
            {
                if (this.ScreenOwner.InputController.ActionBack ||
                    this.ScreenOwner.InputController.ActionCancel)
                {
                    this.Dismiss();
                }
            }*/
        }

    }
}
