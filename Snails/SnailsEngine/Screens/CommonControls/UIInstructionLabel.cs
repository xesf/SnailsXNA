using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIInstructionLabel : UIControl
    {
        public enum LabelActionTypes
        {
            Select,
            Back,
            TuneUpDown,
            AdjustSize,
            Accept,
            ToggleSlider,
            Continue,
            Start,
            StartNextStage,
            Quit,
            Retry
        }

        // This can be keyboard keys
        [Flags]
        public enum ControllerKeys
        {
            LeftStick,
            RightStick,
            A,
            B,
            Y,
            X,
            LeftRight
        }

        #region Vars
        public LabelActionTypes _labelType;
        private UISpriteFontLabel _lblCaption;
        private UIImage _imgIcon;
        private ControllerKeys _controllerKey;
        #endregion

        #region Properties
        public ControllerKeys ControllerKey 
        { 
            get { return this._controllerKey; } 
            private set
            {
                this._controllerKey = value;
                switch(this._controllerKey)
                {
                    case ControllerKeys.A:
                      this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "A");
                      break;
                    case ControllerKeys.B:
                      this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "B");
                      break;
                    case ControllerKeys.LeftStick:
                      this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "AnalogStick");
                      break;
                    case ControllerKeys.RightStick:
                      this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "AnalogStick");
                      break;
                    case ControllerKeys.LeftRight:
                      this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "DPad");
                      break;
                }
            }
        }

        public LabelActionTypes LabelType 
        {
            get
            {
                return this._labelType;
            }
            set
            {
                this._labelType = value;
                switch (this._labelType)
                {
                    case LabelActionTypes.Quit:
                        this.Caption = "Quit";
                        this.ControllerKey = ControllerKeys.B;
                        break;

                    case LabelActionTypes.StartNextStage:
                        this.Caption = "Next Stage";
                        this.ControllerKey = ControllerKeys.A;
                        break;

                    case LabelActionTypes.Select:
                        this.Caption = "Select";
                        this.ControllerKey = ControllerKeys.A;
                        break;

                    case LabelActionTypes.Back:
                        this.Caption = "Back";
                        this.ControllerKey = ControllerKeys.B;
                        break;

                    case LabelActionTypes.TuneUpDown:
                        this.Caption = "Tune Up/Down";
                        this.ControllerKey = ControllerKeys.LeftStick;
                        break;

                    case LabelActionTypes.AdjustSize:
                        this.Caption = "Adjust Size";
                        this.ControllerKey = ControllerKeys.LeftStick;
                        break;

                    case LabelActionTypes.Accept:
                        this.Caption = "Accept";
                        this.ControllerKey = ControllerKeys.A;
                        break;

                    case LabelActionTypes.ToggleSlider:
                        this.Caption = "Toggle";
                        this.ControllerKey = ControllerKeys.LeftRight;
                        break;

                    case LabelActionTypes.Continue:
                        this.Caption = "Continue";
                        this.ControllerKey = ControllerKeys.A;
                        break;

                    case LabelActionTypes.Start:
                        this.Caption = "Start";
                        this.ControllerKey = ControllerKeys.A;
                        break;

                    case LabelActionTypes.Retry:
                        this.Caption = "Retry";
                        this.ControllerKey = ControllerKeys.A;
                        break;
                }

                this.ResizeControl();
            }
        }

        public string Caption
        {
            get
            {
                return this._lblCaption.Text;
            }
            set
            {
                this._lblCaption.Text = value;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UIInstructionLabel(UIScreen ownerScreen, LabelActionTypes type) :
            base(ownerScreen)
        {

            // Label
            this._lblCaption = new UISpriteFontLabel(ownerScreen, "fonts/instructionLabel", "");
            this._lblCaption.ParentAlignment = AlignModes.Vertically;
            this._lblCaption.DropShadow = true;
            this._lblCaption.ShadowDistance = new Vector2(1.0f, 1.0f);
            this._lblCaption.ShadowColor = Color.Black;
            this.Controls.Add(this._lblCaption);

            // Image
            this._imgIcon = new UIImage(ownerScreen);
            this._imgIcon.ParentAlignment = AlignModes.Vertically;
            this.Controls.Add(this._imgIcon);

            this.ParentAlignment = AlignModes.Vertically;
            this.LabelType = type;
            this.ResizeControl();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResizeControl()
        {
            this._imgIcon.Position = new Vector2(0.0f, 0.0f);
            this._lblCaption.Position = new Vector2(this._imgIcon.Size.Width + 50.0f, 0.0f);
            this.Size = new Size(this._lblCaption.Position.X + this._lblCaption.Size.Width,
                                 Math.Max(this._imgIcon.Size.Height, this._lblCaption.Size.Height));
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return this._lblCaption.Text;
        }
    }
}
