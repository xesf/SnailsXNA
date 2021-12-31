using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    /// <summary>
    /// A window is a board with a title and a dismiss button
    /// Used to display the xbox controller help, tutorial topics help
    /// </summary>
    class UISnailsWindow : UIControl
    {


        public enum ButtonType
        {
            Start,
            Dismiss,
            YesNo
        }

        #region Events
        public event UIEvent OnDismiss;
        public event UIEvent OnDismissBegin;
        public event UIEvent OnDismissPressed;
        public event UIEvent OnDismissButtonShown;
        public event UIEvent OnYes;
        public event UIEvent OnNo;
        #endregion

        private UISnailsBoard _board;
        private UISnailsMenuTitle _title;
        private UISnailsButton _btnButton1;
        private UISnailsButton _btnButton2;
        private UICloseButton _btnClose; // The cross on the top right corner
        private ButtonType _buttonType;
        public UISnailsBoard Board { get { return this._board; } }
        protected UISnailsButton ContinueButton { get { return this._btnButton1; } }
        protected UICloseButton CloseButton { get { return this._btnClose; } }
        public UIControl Body { get { return this.Board.BoardImage; } }

        public string TitleResourceId
        {
            set { this._title.TextResourceId = value; }
        }
        #region Properties
        public UISnailsBoard.BoardType BoardType
        {
            get
            {
                return this._board.Type;
            }
            set
            {
                this._board.Type = value;
                this._board.Size = new Size(this._board.Size.Width, this._board.Size.Height + this.NativeResolutionY(1200f));
                this.Size = this._board.Size;
            }
        }

        public bool DismissButtonVisible
        {
            get
            {
                return this._btnButton1.Visible;
            }
            set
            {
                this._btnButton1.Visible = value;
            }
        }

        public bool CloseButtonVisible
        {
            get
            {
                return this._btnClose.Visible;
            }
            set
            {
                this._btnClose.Visible = value;
            }
        }
        public ButtonType ButtonCaptionType
        {
            get { return this._buttonType; }
            set
            {
                this._buttonType = value;
                switch (this._buttonType)
                {
                    case UIXBoxControls.ButtonType.Dismiss:
                        this._btnButton1.TextResourceId = "BTN_DISMISS";
                        this._btnButton1.ButtonAction = UISnailsButton.ButtonActionType.Back;
                        this._btnButton1.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
                        this._btnButton2.Visible = false;
                        this._btnButton1.ControllerActionCode = 0;
                        break;

                    case UIXBoxControls.ButtonType.Start:
                        this._btnButton1.TextResourceId = "BTN_START";
                        this._btnButton1.ButtonAction = UISnailsButton.ButtonActionType.Start;
                        this._btnButton1.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
                        this._btnButton2.Visible = false;
                        this._btnButton1.ControllerActionCode = 0;
                        break;

                    case UIXBoxControls.ButtonType.YesNo:
                        this._btnButton1.TextResourceId = "BTN_NO";
                        this._btnButton1.ButtonAction = UISnailsButton.ButtonActionType.No;
                        this._btnButton1.ParentAlignment = AlignModes.Bottom;
                        this._btnButton1.Position = new Vector2(this.Center.X + this.NativeResolutionX(200f), 0f);
                        this._btnButton1.ControllerActionCode = (int)(InputBase.InputActions.Back);

                        this._btnButton2.Visible = true;
                        this._btnButton2.ButtonAction = UISnailsButton.ButtonActionType.Yes;
                        this._btnButton2.TextResourceId = "BTN_YES";
                        this._btnButton2.ParentAlignment = AlignModes.Bottom;
                        this._btnButton2.Position = new Vector2(this.Center.X - this.NativeResolutionX(1200f), 0f);
                        break;

                }
            }
        }

        public string TitleText
        {
            get { return this._title.Text; }
            set { this._title.Text = value; }
        }

        public bool WithTitle
        {
            get;
            set;
        }

        public string Button2TextResource
        {
            get { return this._btnButton2.TextResourceId; }
            set
            {
                this._btnButton2.TextResourceId = value;
                this._btnButton2.ButtonAction = UISnailsButton.ButtonActionType.Undefined;
            }
        }

        public string Button1TextResource
        {
            get { return this._btnButton1.TextResourceId; }
            set
            {
                this._btnButton1.TextResourceId = value;
                this._btnButton1.ButtonAction = UISnailsButton.ButtonActionType.Undefined;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public UISnailsWindow(UIScreen screenOwner) :
            base(screenOwner)
        {
            // Wooden board
            this._board = new UISnailsBoard(screenOwner, UISnailsBoard.BoardType.LightWoodMediumLong);
            this._board.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._board.OnHide += new UIEvent(_board_OnHide);
            this._board.OnShow += new UIEvent(_board_OnShow);
            this.Controls.Add(this._board);

            // Title - Game controls
            this._title = new UISnailsMenuTitle(screenOwner);
            this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
            this._title.ParentAlignment = AlignModes.Horizontaly;
            this._title.Visible = false;
            this._board.Controls.Add(this._title);

            // Button 1
            this._btnButton1 = new UISnailsButton(screenOwner, "BTN_DISMISS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, this.btnContinue_OnClick, false);
            this._btnButton1.OnClickBegin += new UIEvent(ContinueButton_OnClickBegin);
            this._btnButton1.OnShow += new UIEvent(_btnButton1_OnShow);
            this._btnButton1.Name = "Button 1";
            this._btnButton1.ButtonAction = UISnailsButton.ButtonActionType.Back;
            this._btnButton1.Margins.Bottom = this.NativeResolutionY(100f);
            this._board.Controls.Add(this._btnButton1);

            // Button 2
            this._btnButton2 = new UISnailsButton(screenOwner, "BTN_DISMISS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnButton2_OnClick, false);
            this._btnButton2.Name = "Button 2";
            this._btnButton2.Margins.Bottom = this.NativeResolutionY(100f);
            this._board.Controls.Add(this._btnButton2);

            // Close button
            this._btnClose = new UICloseButton(screenOwner);
            this._btnClose.ParentAlignment = BrainEngine.UI.AlignModes.Right | BrainEngine.UI.AlignModes.Top;
            this._btnClose.Margins.Top = -400f;
            this._btnClose.Margins.Right = -200f;
            this._btnClose.OnPress += new UIControl.UIEvent(_btnClose_OnPress);
            this._btnClose.OnAcceptBegin += new UIEvent(ContinueButton_OnClickBegin);
            this._board.Controls.Add(this._btnClose);

            this.ButtonCaptionType = UIXBoxControls.ButtonType.Dismiss;
            this.DismissButtonVisible = true;
            this.BoardType = UISnailsBoard.BoardType.LightWoodMediumLong;
            this.OnAfterInitializeFromContent += new UIEvent(UISnailsWindow_OnAfterInitializeFromContent);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void InitializeFromContent()
        {
            base.InitializeFromContent();
            this.InitializeFromContent("UISnailsWindow");
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsWindow_OnAfterInitializeFromContent(IUIControl sender)
        {
            this._title.Position = this.GetContentPropertyValue<Vector2>("titlePosition", this._title.Position);
            this._title.Scale = this.GetContentPropertyValue<Vector2>("titleScale", this._title.Scale);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowWithoutAnimation()
        {
            this._title.Visible = this.WithTitle;
            this.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void _board_OnShow(IUIControl sender)
        {
            this.InvokeOnShow();
        }

        /// <summary>
        /// 
        /// </summary>
        void _board_OnHide(IUIControl sender)
        {
            this.Visible = false;
            this.InvokeOnDismiss();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Show()
        {
            base.Show();
            this._title.Visible = this.WithTitle;
            this._board.Show();
        }


        /// <summary>
        /// 
        /// </summary>
        void InvokeOnDismiss()
        {
            if (this.OnDismiss != null)
            {
                this.OnDismiss(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void btnButton2_OnClick(IUIControl sender)
        {
            if (this.ButtonCaptionType == ButtonType.YesNo)
            {
                if (this.OnYes != null)
                {
                    this.OnYes(this);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void btnContinue_OnClick(IUIControl sender)
        {
            if (this.ButtonCaptionType == ButtonType.YesNo)
            {
                if (this.OnNo != null)
                {
                    this.OnNo(this);
                }
            }
            else
            {
                if (this.OnDismissPressed != null)
                {
                    this.OnDismissPressed(this);
                }
                this.Dismiss();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void ContinueButton_OnClickBegin(IUIControl sender)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowDismissButton()
        {
            this._btnButton1.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnButton1_OnShow(IUIControl sender)
        {
            if (this.OnDismissButtonShown != null)
            {
                this.OnDismissButtonShown(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Focus()
        {
            this._btnButton1.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Dismiss()
        {
            if (this.OnDismissBegin != null)
            {
                this.OnDismissBegin(this);
            }
            ((SnailsScreen)this.ScreenOwner).DisableInput();
            this.Hide();
        }

        public void Close()
        {
            this.Dismiss();
        }

        void _btnClose_OnPress(IUIControl sender)
        {
            this.Dismiss();
        }

    }
}
