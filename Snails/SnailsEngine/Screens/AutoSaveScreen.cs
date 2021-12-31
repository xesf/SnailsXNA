using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Input;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;

namespace TwoBrainsGames.Snails.Screens
{
    class AutoSaveScreen : SnailsScreen
    {
        #region Conts
        const float LINE_SPACING = 450f;
        const double SCREEN_ALLOW_SKIP_TIME = 1000;
        const double SHOW_BOARD_TIME = 200;
        #endregion

        UISnailsBoard _board;
        UITimer _tmrShowBoard;
        UITimer _tmrSkip;
        //Vector2 _hddIndicatorPosition;
        UISnailsButton _btnContinue;
        UICaption [] _capMessages;

        public AutoSaveScreen(ScreenNavigator owner) :
            base(owner, ScreenType.AutoSave)
        { 
		}

		/// <summary>
		/// 
		/// </summary>
        public override void OnLoad()
		{
            base.OnLoad();

            this.BackgroundImageBlendColor = Colors.AutoSaveScrBkColor;

            // Board
            this._board = new UISnailsBoard(this, UISnailsBoard.BoardType.LightWoodLongNarrow);
            this._board.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this.Controls.Add(this._board);

            // Autosave message
            this.SetupMessage();

            // Timer
            this._tmrShowBoard = new UITimer(this, SHOW_BOARD_TIME, false);
            this._tmrShowBoard.OnTimer += new UIControl.UIEvent(_tmrShowBoard_OnTimer);
            this.Controls.Add(this._tmrShowBoard);

            // Skip screen timer
            this._tmrSkip = new UITimer(this, SCREEN_ALLOW_SKIP_TIME, false);
            this._tmrSkip.OnTimer += new UIControl.UIEvent(_tmrSkip_OnTimer);
            this.Controls.Add(this._tmrSkip);

            // Continue button
            this._btnContinue = new UISnailsButton(this, "BTN_CONTINUE", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Accept, this.btnContinue_OnClick, false);
            this._btnContinue.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
            this._btnContinue.Margins.Bottom = -1200f;
            this._btnContinue.OnShow += new UIControl.UIEvent(_btnContinue_OnShow);
            this._board.Controls.Add(this._btnContinue);
            //this._hddIndicatorPosition = new Vector2((BrainGame.ScreenWidth / 2) , (BrainGame.ScreenHeight / 2) + 70f);

            this.OnLanguageChanged += new UIControl.UIEvent(AutoSaveScreen_OnLanguageChanged);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this.DisableInput();
            this._board.Visible = false;
            this._tmrShowBoard.Reset();
            this._tmrShowBoard.Enabled = true;
            this._tmrSkip.Enabled = false;
            this._tmrSkip.Reset();
            this._btnContinue.Visible = false;
        }


		/// <summary>
        /// 
        /// </summary>
        void _tmrShowBoard_OnTimer(IUIControl sender)
        {
            this._board.Show();
            this._tmrSkip.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void _tmrSkip_OnTimer(IUIControl sender)
        {
            this._btnContinue.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void btnContinue_OnClick(IUIControl sender)
        {
            this.NavigateToMainMenu();
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnContinue_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this.InstructionBar.HideAllLabels();
            this.InstructionBar.ShowLabel(CommonControls.UIInstructionLabel.LabelActionTypes.Continue);
            this._btnContinue.Focus();
        }


        /// <summary>
        /// 
        /// </summary>
        private void NavigateToMainMenu()
        {
            this.DisableInput();
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.IntroPicture);
            this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
        }


        /// <summary>
        /// 
        /// </summary>
        void AutoSaveScreen_OnLanguageChanged(IUIControl sender)
        {
            this.SetupMessage();
        }

        /// <summary>
        /// 
        /// </summary>
        void SetupMessage()
        {
            string[] msg = LanguageManager.GetMultiString("MSG_AUTOSAVE_LINE");
            this._capMessages = new UICaption[msg.Length];
            Vector2 pos = new Vector2(0.0f, 720.0f);
            for (int i = 0; i < msg.Length; i++)
            {
                this._capMessages[i] = new UICaption(this, msg[i], Colors.AutoSaveWarningText, UICaption.CaptionStyle.AutoSaveMessage);
                this._capMessages[i].ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
                this._capMessages[i].Position = pos;
                this._board.Controls.Add(this._capMessages[i]);
                pos += new Vector2(0.0f, LINE_SPACING);
            }
        }

	}
}
