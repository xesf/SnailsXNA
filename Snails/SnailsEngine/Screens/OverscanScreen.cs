using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;

namespace TwoBrainsGames.Snails.Screens
{
    class OverscanScreen : SnailsScreen
    {
        enum ScreenState
        {
            Starting,
            Active,
            Accepted
        }

        private const int OVERSCAN_INCREMENT = 2;
        private const int LINE_SPACING = 500;
        //
		//private static Vector2 TextSize = new Vector2(0.7f, 0.7f);
        private UISnailsButton _btnContinue;

        #region Vars
        Rectangle _viewportArea;
        Rectangle _viewportSafeArea;
        float _viewportWidth;
        float _viewportHeight;
        float _viewportX;
        float _viewportY;

        int _screenWidth;
        int _screenHeight;

        ScreenType _caller; // The screen that called this screen

        Viewport _vp = new Viewport();
        //TextFont _fntNormalText;
        UISnailsBoard _board;
        UITimer _tmrAccept;
        UIImage _imgStick;
        List<UISnail> _snails;
        UICaption [] _capInstructions; // The instructions message
        UICaption _capConfigLater;
        #endregion

        #region Propertiesat
        private ScreenState State { get; set; }
        #endregion

        public OverscanScreen(ScreenNavigator owner) :
            base(owner, ScreenType.Overscan)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.BackgroundImageBlendColor = Colors.OverscanScrBkColor1;

            //this._fntNormalText = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);

            // Board
            this._board = new UISnailsBoard(this, UISnailsBoard.BoardType.LightWoodMediumLong);
            this._board.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._board.OnShow += new UIControl.UIEvent(_board_OnShow);
            this.Controls.Add(this._board);

            this.SetupInstructionMessage();

            // Accept Timer - starts counting when the user acceps the settings
            this._tmrAccept = new UITimer(this, 500, false);
            this._tmrAccept.OnTimer += new UIControl.UIEvent(_tmrAccept_OnAccept);
            this.Controls.Add(this._tmrAccept);

            // Stick
            this._imgStick = new UIImage(this, "spriteset/main-menu-objects2/LeftStick", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgStick.ParentAlignment = AlignModes.Horizontaly;
            this._imgStick.Position = new Vector2(0f, 1900f);
            this._board.Controls.Add(this._imgStick);

            // Settings can be changed later warning
            this._capConfigLater = new UICaption(this, "", new Color(230, 230, 230), UICaption.CaptionStyle.NormalTextSmall);
            this._capConfigLater.TextResourceId = "LBL_OVERSCAN_CONFIG_LATER";
            this._capConfigLater.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
            this._capConfigLater.Margins.Bottom = 400f;
            this._board.Controls.Add(this._capConfigLater);

            // Accept button
            this._btnContinue = new UISnailsButton(this, "BTN_ACCEPT", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Accept, this.btnContinue_OnClick, false);
            this._btnContinue.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
            this._btnContinue.Margins.Bottom = -1200f;
            this._board.Controls.Add(this._btnContinue);

            this.OnOpenTransitionEnded += new UIControl.UIEvent(OverscanScreen_OnOpenTransitionEnded);
            this.OnLanguageChanged += new UIControl.UIEvent(OverscanScreen_OnLanguageChanged);


            _viewportSafeArea = SnailsGame.SafeArea;
            _viewportArea = _viewportSafeArea;


            _screenWidth = SnailsGame.ScreenWidth;
            _screenHeight = SnailsGame.ScreenHeight;

        }



        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            this._viewportArea = SnailsGame.ProfilesManager.CurrentProfile.Viewport.Bounds;

            _viewportWidth = _viewportArea.Width;
            _viewportHeight = _viewportArea.Height;
            _viewportX = _viewportArea.X;
            _viewportY = _viewportArea.Y;

            this.State = ScreenState.Starting;
            this.CenterSafeFrame();
            this.ValidateOverscanBounds();
            this.UpdateViewport();
            this.DisableInput();
            this._board.Visible = false;
            BrainGame.GameCursor.Visible = false;
            BrainGame.ClearColor = Colors.BBOverscanScreen;

            // Snails
            this._snails = new List<UISnail>();
            for (int i = 0; i < 20; i++)
            {
                UISnail snail = new UISnail(this);
                this._snails.Add(snail);
                this.Controls.Add(snail);
            }

            this.BackgroundImageBlendColor = Colors.OverscanScrBkColor1;
            this._capConfigLater.Visible = true;

            this._caller = this.Navigator.GlobalCache.Get<ScreenType>(GlobalCacheKeys.OVERSCAN_CALLER_SCREEN, ScreenType.None);
            if (this._caller == ScreenType.Options)
            {
                this.BackgroundImageBlendColor = Colors.OverscanScrBkColor2;
                this._capConfigLater.Visible = false;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {

            switch (this.State)
            {

                case ScreenState.Active:
                    this.UpdateStateActive(gameTime);
                    break;

                case ScreenState.Accepted:
                    this.UpdateStateAccepted();
                    break;
             }
        }

        /// <summary>
        /// 
        /// </summary>
        void OverscanScreen_OnOpenTransitionEnded(IUIControl sender)
        {
            this._board.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void OverscanScreen_OnLanguageChanged(IUIControl sender)
        {
            this.SetupInstructionMessage();
        }

        /// <summary>
        /// State when users accepts
        /// First there's an effect of the snails falling and the letters disappearing
        /// The navigation is done afterwards when a timer ellapses
        /// </summary>
        private void UpdateStateAccepted()
        {
            if (this._snails.Count > 0)
            {
                for (int i = 0; i < this._snails.Count; i++)
                {
                    if (this._snails[i].Position.Y > this.PixelsToScreenUnits(new Vector2(0.0f, SnailsGame.ScreenHeight)).Y + this._snails[i].Size.Height)
                    {
                        this.Controls.Remove(this._snails[i]);
                        this._snails.Remove(this._snails[i]);
                        i--;
                    }
                }
                if (this._snails.Count == 0)
                {
                    this._tmrAccept.Enabled = true;
                }
            }
            BrainGame.ClearColor = Colors.BBDefaultColor;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateStateActive(BrainGameTime gameTime)
        {
   
                if (this._inputController.ActionUp)
                {
                    _viewportHeight += OVERSCAN_INCREMENT;
                    _viewportY--;

                }
                if (this._inputController.ActionDown)
                {
                    _viewportHeight -= OVERSCAN_INCREMENT;
                    _viewportY++;

                }
                if (this._inputController.ActionRight)
                {
                    _viewportWidth -= OVERSCAN_INCREMENT;
                    _viewportX++;
                }
                if (this._inputController.ActionLeft)
                {
                    _viewportWidth += OVERSCAN_INCREMENT;
                    _viewportX--;
                }
                if (SnailsGame.GameSettings.UseGamepad)
                {
                    if (this._inputController.MotionPosition.X != 0)
                    {
                        this._viewportX += this._inputController.MotionPosition.X * (OVERSCAN_INCREMENT / 2);
                        this._viewportWidth -= (this._inputController.MotionPosition.X * OVERSCAN_INCREMENT);
                    }
                    if (this._inputController.MotionPosition.Y != 0)
                    {
                        this._viewportY -= this._inputController.MotionPosition.Y * (OVERSCAN_INCREMENT / 2);
                        this._viewportHeight += (this._inputController.MotionPosition.Y * OVERSCAN_INCREMENT);  
                    }
                }               

                ValidateOverscanBounds();
               

                _viewportArea.X = (int)this._viewportX;
                _viewportArea.Width = (int)this._viewportWidth;
                _viewportArea.Y = (int)this._viewportY;
                _viewportArea.Height = (int)this._viewportHeight;

                //#endif
                this.UpdateViewport();
              
        }

        
        private void UpdateViewport()
        {
              _vp.X = _viewportArea.X;
              _vp.Y = _viewportArea.Y;
              _vp.Width = _viewportArea.Width;
              _vp.Height = _viewportArea.Height;
              BrainGame.SetViewport(_vp);
        }
        /// <summary>
        /// Bounds validation are not as linear has they seemed
        /// If the safe are is not centered, previous validations would fail. New validations take that into account
        /// </summary>
        private void ValidateOverscanBounds()
        {
           
            // Not less then the safeArea
            if (this._viewportX > this._viewportSafeArea.X)
            {
                this._viewportX = this._viewportSafeArea.X;
            }
            if (this._viewportY > this._viewportSafeArea.Y)
            {
                this._viewportY = this._viewportSafeArea.Y;
            }
           

            // Not bigger then the screen
            if (this._viewportX < 0)
            {
                this._viewportX = 0;
            }
            if (this._viewportY < 0)
            {
                this._viewportY = 0;
            }


            if (this._viewportX + this._viewportWidth < this._viewportSafeArea.X + this._viewportSafeArea.Width)
            {
                this._viewportWidth = this._viewportSafeArea.X + this._viewportSafeArea.Width - this._viewportX;
            }
            if (this._viewportY + this._viewportHeight < this._viewportSafeArea.Y + this._viewportSafeArea.Height)
            {
                this._viewportHeight = this._viewportSafeArea.Y + this._viewportSafeArea.Height - this._viewportY;
            }

            if (this._viewportX + this._viewportWidth > _screenWidth)
            {
                this._viewportWidth = _screenWidth - this._viewportX;
            }

            if (this._viewportY + this._viewportHeight > _screenHeight)
            {
                this._viewportHeight = _screenHeight - this._viewportY;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CenterSafeFrame()
        {
            _viewportArea.X = (int)((_screenWidth - _viewportWidth) / 2);
            _viewportArea.Width = (int)_viewportWidth;
            _viewportArea.Y = (int)((_screenHeight - _viewportHeight) / 2);
            _viewportArea.Height = (int)_viewportHeight;
        }

        /// <summary>
        /// 
        /// </summary>
        private void AcceptSettings()
        {
            Viewport vp = new Viewport();
            vp.X = _viewportArea.X;
            vp.Y = _viewportArea.Y;
            vp.Width = _viewportArea.Width;
            vp.Height = _viewportArea.Height;

            SnailsGame.ProfilesManager.CurrentProfile.OverscanSet = true;
            SnailsGame.ProfilesManager.CurrentProfile.Viewport = vp;
            SnailsGame.ProfilesManager.Save();

            BrainGame.SetViewport(vp);

            foreach (UISnail snail in this._snails)
            {
                snail.Kill();
            }
            this.State = ScreenState.Accepted;
            this.DisableInput();
            this._board.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        void _tmrAccept_OnAccept(IUIControl sender)
        {
            this.DisableInput();
            if (this._caller  == ScreenType.Options)
            {
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.OPTIONS_STARTUP_MODE, OptionsScreen.StartupType.MenuVisible);
                this.NavigateTo(ScreenType.Options.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
            }
            else
            {
                this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.AllHidden);
                this.NavigateTo(ScreenType.MainMenu.ToString(), null, null);
           }
        }

        /// <summary>
        /// 
        /// </summary>
        void _board_OnShow(IUIControl sender)
        {
            this.EnableInput();

            this.InstructionBar.HideAllLabels();
            this.InstructionBar.ShowLabel(CommonControls.UIInstructionLabel.LabelActionTypes.Accept);
            this.InstructionBar.ShowLabel(CommonControls.UIInstructionLabel.LabelActionTypes.AdjustSize);

            this._btnContinue.Focus();
            this.State = ScreenState.Active;

        }

        /// <summary>
        /// 
        /// </summary>
        void btnContinue_OnClick(IUIControl sender)
        {
            this.AcceptSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        void SetupInstructionMessage()
        {
            string[] lines = LanguageManager.GetMultiString("MSG_OVERSCAN");
            if (this._capInstructions != null)
            {
                foreach (UICaption caption in this._capInstructions)
                {
                    this._board.Controls.Remove(caption);
                }
            }
            this._capInstructions = new UICaption[lines.Length];
            Vector2 pos = new Vector2(0.0f, 650.0f);
            for (int i = 0; i < lines.Length; i++)
            {
                this._capInstructions[i] = new UICaption(this, lines[i], Colors.OverscanMessageText, UICaption.CaptionStyle.OverscanMessage);
                this._capInstructions[i].ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
                this._capInstructions[i].Position = pos;
                pos += new Vector2(0, LINE_SPACING);
                this._board.Controls.Add(this._capInstructions[i]);
            }
        }
    }
}
