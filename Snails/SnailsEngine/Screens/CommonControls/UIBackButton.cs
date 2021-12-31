using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIBackButton : UIButton
    {
        public enum ButtonScreenAlignment
        {
            None,
            BottomLeft,
            BottomRight
        }

        private UIImage _imgBack;
        ButtonScreenAlignment _screenAlignment;

        public ButtonScreenAlignment ScreenAlignment
        {
            get { return this._screenAlignment; }
            set
            {
                this._screenAlignment = value;
                switch (this._screenAlignment)
                {
                    case ButtonScreenAlignment.BottomLeft:
                        this.ParentAlignment = BrainEngine.UI.AlignModes.Left | BrainEngine.UI.AlignModes.Bottom;
                        this.Margins.Left = 150f;
                        this.Margins.Bottom = 250f;
                        break;

                    case ButtonScreenAlignment.BottomRight:
                        this.ParentAlignment = BrainEngine.UI.AlignModes.Right | BrainEngine.UI.AlignModes.Bottom;
                        this.Margins.Right = 150f;
                        this.Margins.Bottom = 250f;
                        break;
                }
            }
        }

        public UIBackButton(UIScreen screenOwner) :
            base(screenOwner)
        {
            // Back button
            this._imgBack = new UIImage(screenOwner, "spriteset/button-icons/MenuBackIcon");
            this._imgBack.Effect = new HooverEffect(0.2f, 0.5f, -90.0f);
         //   this._imgBack.AcceptControllerInput = true;
         //   this._imgBack.OnAcceptEffect = new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130);
         //   this._imgBack.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.03f, this.BlendColor, this.Scale);
         //   this._imgBack.ControllerActionCode = (int)InputBase.InputActions.Back;
            this.Controls.Add(this._imgBack);

            // Control
            this.Size = this._imgBack.Size;
            this.ControllerActionCode = (int)InputBase.InputActions.Back;
            this.PressEffect = new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130);
            this.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.03f, this.BlendColor, this.Scale);
            this.PressSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MENU_ITEM_SELECTED);
        }

    }
}
