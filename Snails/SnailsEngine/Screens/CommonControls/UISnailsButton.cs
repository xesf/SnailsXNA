using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.Snails.Effects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsButton : UIControl
    {
        public enum ButtonSizeType
        {
            Small,
            Medium
        }

        public enum ButtonLabelType
        {
            Text,
            Image
        }

        public enum ButtonActionType
        {
            Undefined,
            Back,
            Next,
            Start,
            Previous,
            MainMenu,
            StageSelection,
            Retry,
            No,
            Yes
        }

        #region Events
        public event UIEvent OnPress;
        public event UIEvent OnClickBegin;
        #endregion

        private static Vector2 DEFAULT_SCALE = new Vector2(0.85f, 0.85f);
        private UIPanel _pnlContainer;
        private UIButton _btnButton;
        private UITextFontLabel _lblCaption1;
        private UITextFontLabel _lblCaption2;
        private UIImage _image;

        ButtonSizeType SizeType { get; set; }
        private ButtonLabelType LabelType
        {
            get { return this._labelType; }
            set
            {
                this._labelType = value;
                switch (this._labelType)
                {
                    case ButtonLabelType.Text:
                        this._lblCaption1.Visible = true;
                        this._lblCaption1.Visible = true;
                        this._image.Visible = false;
                        this.UpdateLabels();
                        break;

                    case ButtonLabelType.Image:
                        this._lblCaption1.Visible = false;
                        this._lblCaption1.Visible = false;
                        this._image.Visible = true;
                        break;
                }
            }
        }

        public ButtonActionType ButtonAction
        {
            get
            {
                return this._buttonAction;
            }
            set
            {
                string resource = null;
                this._image.Sprite = null;
                this._buttonAction = value;
                switch (this._buttonAction)
                {
                    case ButtonActionType.Back:
                        resource = "spriteset/button-icons/BackIcon";
                        break;
                    case ButtonActionType.Next:
                        resource = "spriteset/button-icons/NextIcon";
                        break;
                    case ButtonActionType.Start:
                        resource = "spriteset/button-icons/StartIcon";
                        break;
                    case ButtonActionType.Previous:
                        resource = "spriteset/button-icons/PreviousIcon";
                        break;
                    case ButtonActionType.MainMenu:
                        resource = "spriteset/button-icons/MainMenuIcon";
                        break;
                    case ButtonActionType.StageSelection:
                        resource = "spriteset/button-icons/StageSelIcon";
                        break;
                    case ButtonActionType.Retry:
                        resource = "spriteset/button-icons/RetryIcon";
                        break;
                }

                if (resource != null)
                {
                    this._image.Sprite = BrainGame.ResourceManager.GetSpriteStatic(resource);
                }

             }
        }

        public new int ControllerActionCode
        {
            get { return base.ControllerActionCode; }
            set
            {
                base.ControllerActionCode = value;
                this._btnButton.ControllerActionCode = value;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.UpdateLabels();
            }
        }

        private Vector2 Caption1PositionSmall { get; set; }
        private Vector2 Caption2PositionSmall { get; set; }
        private Vector2 Caption1PositionMedium { get; set; }
        private Vector2 Caption2PositionMedium { get; set; }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                if (value)
                {
                    if (this._pnlContainer != null)
                    {
                        this._pnlContainer.Visible = true;
                    }
                }
            }
        }

        Sample _focusSound;
        Sample _shownSound;
        ButtonLabelType _labelType;
        ButtonActionType _buttonAction;

        /// <summary>
        /// 
        /// </summary>
        public UISnailsButton(UIScreen screenOwner, string textResourceId, ButtonSizeType type, InputBase.InputActions action, UIEvent pressCallback, bool useShowEffects) :
            base(screenOwner)
        {
            this.SizeType = type;
            this.OnEnter += new UIEvent(UISnailsButton_OnEnter);
            this.OnLeave += new UIEvent(UISnailsButton_OnLeave);
            this.OnScreenStart += new UIEvent(UISnailsButton_OnScreenStart);
            this.OnInitializeFromContent += new UIEvent(UISnailsButton_OnInitializeFromContent);
            
            // Container
            this._pnlContainer = new UIPanel(screenOwner);
            this._pnlContainer.Name = "_pnlContainer";
            this._pnlContainer.ParentAlignment = AlignModes.HorizontalyVertically;
            if (useShowEffects)
            {
                this._pnlContainer.ShowEffect = new SquashEffect(0.7f, 4.0f, 0.08f, this.BlendColor, UISnailsButton.DEFAULT_SCALE);
                this._pnlContainer.HideEffect = new PopOutEffect(new Vector2(1.3f, 1.3f), 6.0f);
            }
            this._pnlContainer.OnShow += new UIEvent(_pnlContainer_OnShow);
            this._pnlContainer.OnHide += new UIEvent(_pnlContainer_OnHide);
            this.Controls.Add(this._pnlContainer);

            // Button
            this._btnButton = new UIButton(screenOwner);
            this._btnButton.SizeMode = ImageSizeMode.Center;
            this._btnButton.PressEffect = new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130);
            this._btnButton.OnPress += new UIEvent(_btnButton_OnPress);
            this._btnButton.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.03f, this.BlendColor, this.Scale);
            this._btnButton.ParentAlignment = AlignModes.HorizontalyVertically;
            this._btnButton.OnShow += new UIEvent(_btnButton_OnShow);
            this._btnButton.OnShowBegin += new UIEvent(_btnButton_OnShowBegin);
            this._btnButton.OnAccept += new UIEvent(_btnButton_OnAccept);
            this._btnButton.PressSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MENU_ITEM_SELECTED);
            this._pnlContainer.Controls.Add(this._btnButton);
            switch (type)
            {
                case ButtonSizeType.Small:
                    this._btnButton.ImageResource = "spriteset/boards/ButtonSmall";
                    break;
                case ButtonSizeType.Medium:
                    this._btnButton.ImageResource = "spriteset/boards/ButtonMedium";
                    break;
            }

            Vector2 size = this.PixelsToScreenUnits(new Vector2(this._btnButton.Sprite.Width, this._btnButton.Sprite.Height));
            this._btnButton.Size = new Size(size.X, size.Y);

            // Label 1
            this._lblCaption1 = new UITextFontLabel(this.ScreenOwner);
            this._lblCaption1.ParentAlignment = AlignModes.HorizontalyVertically;
            this._lblCaption1.Visible = false;
            this._lblCaption1.BlendColor = Colors.ButtonsText;
            this._btnButton.Controls.Add(this._lblCaption1);

            // Label 2
            this._lblCaption2 = new UITextFontLabel(this.ScreenOwner);
            this._lblCaption2.ParentAlignment = AlignModes.HorizontalyVertically;
            this._lblCaption2.Visible = false;
            this._lblCaption2.BlendColor = Colors.ButtonsText;
            this._btnButton.Controls.Add(this._lblCaption2);

            // Image - for image captioned buttons
            this._image = new UIImage(this.ScreenOwner);
            this._image.ParentAlignment = AlignModes.HorizontalyVertically;
            this._image.Visible = false;
            this._image.BlendColor = Colors.ButtonsText;
            this._image.BlendColor = Colors.ButtonsText;
            this._btnButton.Controls.Add(this._image);

            this.TextResourceId = textResourceId;
            this.Size = this._btnButton.Size;
            this._pnlContainer.Size = this.Size;
            this._pnlContainer.Scale = UISnailsButton.DEFAULT_SCALE;
            this.ControllerActionCode =(int)action;
            if (pressCallback != null)
            {
                this.OnPress += new UIControl.UIEvent(pressCallback);
            }
            this._focusSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_FOCUS);
            this._shownSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SHOWN);

            this.OnAccept += new UIEvent(UISnailsButton_OnAccept);
            this.UpdateLabels();

            if (SnailsGame.GameSettings.UseButtonIcons)
            {
                this.LabelType = ButtonLabelType.Image;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitializeFromContent()
        {
            base.InitializeFromContent();
            this.InitializeFromContent("screens", "UISnailsButton");
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsButton_OnInitializeFromContent(IUIControl sender)
        {
            this.Caption1PositionSmall = this.GetContentPropertyValue<Vector2>("caption1PositionSmall", this.Caption1PositionSmall);
            this.Caption2PositionSmall = this.GetContentPropertyValue<Vector2>("caption2PositionSmall", this.Caption2PositionSmall);
            this.Caption1PositionMedium = this.GetContentPropertyValue<Vector2>("caption1PositionMedium", this.Caption1PositionMedium);
            this.Caption2PositionMedium = this.GetContentPropertyValue<Vector2>("caption2PositionMedium", this.Caption2PositionMedium);
            this.UpdateLabels();
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsButton_OnAccept(IUIControl sender)
        {
            if (this.OnClickBegin != null)
            {
                this.OnClickBegin(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnButton_OnAccept(IUIControl sender)
        {
            if (this.OnClickBegin != null)
            {
                this.OnClickBegin(this);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void _btnButton_OnShowBegin(IUIControl sender)
        {
            this._shownSound.Play();

        }

        /// <summary>
        /// 
        /// </summary>
        void _btnButton_OnShow(IUIControl sender)
        {
            this.InvokeOnShow();
        }


        /// <summary>
        /// 
        /// </summary>
        void _btnButton_OnPress(IUIControl sender)
        {
            if (this.OnPress != null)
            {
                this.OnPress(this);
            }
          
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsButton_OnLeave(IUIControl sender)
        {
            this.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsButton_OnEnter(IUIControl sender)
        {
            this._pnlContainer.Effect = new ScaleEffect(UISnailsButton.DEFAULT_SCALE, 4.0f, new Vector2(1.0f, 1.0f), false);
            this._focusSound.Play();
            this._lblCaption1.BlendColor = Colors.ButtonsFocusText;
            this._lblCaption2.BlendColor = Colors.ButtonsFocusText;
            this._image.BlendColor = Colors.ButtonsFocusText;
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsButton_OnScreenStart(IUIControl sender)
        {
            this.Reset();
        }

        /// <summary>
        /// Resets the button to its default initial state
        /// </summary>
        void Reset()
        {
            this._pnlContainer.Effect = null;
            this._pnlContainer.Scale = UISnailsButton.DEFAULT_SCALE;
            this._lblCaption1.BlendColor = Colors.ButtonsText;
            this._lblCaption2.BlendColor = Colors.ButtonsText;
            this._image.BlendColor = Colors.ButtonsText;
            if (this._image.Sprite == null)
            {
                this.LabelType = ButtonLabelType.Text;
            }  
        }

        /// <summary>
        /// 
        /// </summary>
        void _pnlContainer_OnShow(IUIControl sender)
        {
            this.InvokeOnShow();
        }

        /// <summary>
        /// 
        /// </summary>
        void _pnlContainer_OnHide(IUIControl sender)
        {
            this.InvokeOnHide();
            this.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateLabels()
        {
            if (this._labelType != ButtonLabelType.Text)
            {
                this._lblCaption1.Visible = false;
                this._lblCaption2.Visible = false;
                return;
            }

            string[] lines = this.Text.Split('|');
            if (lines.Length >= 2)
            {
                this._lblCaption1.Visible = true;
                this._lblCaption1.Text = lines[0];
                this._lblCaption1.ParentAlignment = AlignModes.Horizontaly;
                this._lblCaption1.Position = new Vector2(0, 150);
                this._lblCaption1.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);

                this._lblCaption2.Visible = true;
                this._lblCaption2.Text = lines[1];
                this._lblCaption2.ParentAlignment = AlignModes.Horizontaly;
                this._lblCaption2.Position = new Vector2(0, 550);
                this._lblCaption2.Font = this._lblCaption1.Font;

                switch (this.SizeType)
                {
                    case ButtonSizeType.Small:
                        this._lblCaption1.Position = this.Caption1PositionSmall;
                        this._lblCaption2.Position = this.Caption2PositionSmall;
                        break;

                    case ButtonSizeType.Medium:
                        this._lblCaption1.Position = this.Caption1PositionMedium;
                        this._lblCaption2.Position = this.Caption2PositionMedium;
                        break;
                }

            }
            else
            if (lines.Length == 1)
            {
                this._lblCaption1.Visible = true;
                this._lblCaption1.Text = lines[0];
                this._lblCaption1.ParentAlignment = AlignModes.HorizontalyVertically;
                this._lblCaption2.Visible = false;
                switch (this.SizeType)
                {
                    case ButtonSizeType.Small:
                        this._lblCaption1.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
                        break;
                    case ButtonSizeType.Medium:
                        this._lblCaption1.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Show()
        {
            this._pnlContainer.Visible = true;
            base.Show();
        }
    }
}
