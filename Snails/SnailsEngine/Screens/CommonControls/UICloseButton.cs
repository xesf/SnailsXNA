using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UICloseButton : UIButton
    {
        public enum ButtonFaceType
        {
            Dark,
            Light
        }

        UIImage _imgButton;
        ButtonFaceType _faceType;

        public ButtonFaceType FaceType
        {
            get { return this._faceType; }
            set
            {
                this._faceType = value;
                string resId = null;
                switch (this._faceType)
                {
                    case ButtonFaceType.Dark:
                        resId = "spriteset/button-icons/CloseIcon";
                        break;
                    case ButtonFaceType.Light:
                        resId = "spriteset/button-icons/CloseIconLight";
                        break;
                }
                if (resId != null)
                {
                    this._imgButton.Sprite = BrainGame.ResourceManager.GetSpriteStatic(resId);
                }
            }
        }

        // Hot key might get in conflict with other controls
        // Activate it when needed
        public bool UseHotKey
        {
            set
            {
                this.ControllerActionCode = 0;
                if (value == true)
                {
                    this.ControllerActionCode = (int)InputBase.InputActions.Back;
                }
            }

        }


        public UICloseButton(UIScreen screenOwner) :
            base(screenOwner)
        {
            this._imgButton = new UIImage(screenOwner, "spriteset/button-icons/CloseIcon");
            this._imgButton.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._imgButton.Effect = new ScaleEffect(new Vector2(1f, 1f), 0.2f, new Vector2(0.94f, 0.94f), true);
            this.Controls.Add(this._imgButton);

            this.PressEffect = new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130);
            this.OnScreenStart += new UIEvent(UICloseButton_OnScreenStart);
            this.Size = this._imgButton.Size;
            this.PressSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MENU_ITEM_SELECTED);
        }

        /// <summary>
        /// 
        /// </summary>
        void  UICloseButton_OnScreenStart(IUIControl sender)
        {
            this.BlendColor = Color.White; // This is because a previous pressEffect might have change this
        }

    }
}
