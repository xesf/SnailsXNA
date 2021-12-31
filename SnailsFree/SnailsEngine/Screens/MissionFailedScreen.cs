using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI;

namespace TwoBrainsGames.Snails.Screens
{
	// THis displays the mission failed screen
	// Not used at this moment
	class MissionFailedScreen : SnailsScreen
	{  
		#region Constants
		private const int REASON_LINES_COUNT = 3;
		#endregion

		#region Members
		protected UISnailsBoard _leafsBoard;
		protected UISnailsBoard _board;
		protected UISnailsMenuTitle _title;
		protected UICaption[] _capReasonLines;
		protected UISnailStamp _failStamp;
		protected UISnailsButton _btnQuit;
		protected UISnailsButton _btnAgain;

		Stage.MissionFailedReasonType _missionFailedReason;

		Sample _failedSample;

		private float LineSpacing { get; set; }
		private Vector2 MessagePosition { get; set; } 
		#endregion

		public MissionFailedScreen(ScreenNavigator owner) :
		base(owner, ScreenType.MissionFailed)
		{ }

		#region Screen events
		/// <summary>
		/// 
		/// </summary>
		public override void OnLoad()
		{
			base.OnLoad();
			this.Name = "";
			// Leafs background
			this._leafsBoard = new UISnailsBoard(this, UISnailsBoard.BoardType.LeafsMedium);
			this._leafsBoard.Name = "_leafsBoard";
			this._leafsBoard.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
			this.Controls.Add(this._leafsBoard);

			// Board
			this._board = new UISnailsBoard(this, UISnailsBoard.BoardType.LightWoodMedium);
			this._board.Name = "_board";
			this._board.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
			this._board.OnShow += new UIControl.UIEvent(_board_OnShow);
			this._board.Size = new Size(this._board.Size.Width, this._board.Size.Height + this.PixelsToScreenUnitsY(170)); // The buttons have to be inside or else Action will not work when we're in control snap cursor mode
			this._board.ImagePosition = this.PixelsToScreenUnits(new Vector2 (0, 75));
			this._leafsBoard.Controls.Add(this._board);

			// Title - Stage Completed
			this._title = new UISnailsMenuTitle(this);
			this._title.Name = "_title";
			this._title.TextResourceId = "TITLE_MISSION_FAILED";
			this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
			this._title.Position = this.PixelsToScreenUnits (new Vector2(0, -20));
			this._board.Controls.Add(this._title);


			// Fail Stamp
			this._failStamp = new UISnailStamp(this, "spriteset/common-elements-1/Fail", ResourceManager.RES_MANAGER_ID_STATIC);
			this._failStamp.Name = "_failStamp";
			this._failStamp.Position = this.PixelsToScreenUnits(new Vector2(100.0f, 300.0f));
			this._failStamp.OnShow += new UIControl.UIEvent(_failStamp_OnShow);
			this._failStamp.BlendColor = new Color(180, 180, 180, 180);
			this._board.Controls.Add(this._failStamp);     

			// Fail reason captions
			this._capReasonLines = new UICaption[REASON_LINES_COUNT];
			for (int i = 0; i < this._capReasonLines.Length; i++)
			{
				// Reason line 1
				this._capReasonLines[i] = new UICaption(this, "", Colors.MissionFailedCaptions, UICaption.CaptionStyle.MissionFailedCaptions);
				this._capReasonLines[i].ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
				this._board.Controls.Add(this._capReasonLines[i]);
			}

			// Button - Stage Selection
			this._btnQuit = new UISnailsButton(this, "BTN_STAGE_SELECTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, this.btnBack_OnAccept, true);
			this._btnQuit.Name = "_btnQuit";
			this._btnQuit.ParentAlignment = AlignModes.Bottom;
			this._btnQuit.ButtonAction = UISnailsButton.ButtonActionType.StageSelection;
			this._btnQuit.Position = this.PixelsToScreenUnits(new Vector2(85, 0));
			this._board.Controls.Add(this._btnQuit);

			// Button - Play Again
			this._btnAgain = new UISnailsButton(this, "BTN_RETRY", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnAgain_OnAccept, true);
			this._btnAgain.Name = "_btnAgain";
			this._btnAgain.ParentAlignment = AlignModes.Bottom;
			this._btnAgain.Position = this.PixelsToScreenUnits(new Vector2(245, 0));
			this._btnAgain.ButtonAction = UISnailsButton.ButtonActionType.Retry;
			this._board.Controls.Add(this._btnAgain);

			// Audio
			_failedSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MISSION_FAILED);

			this.WithBlurEffect = true;
			this.OnBlurEffectEnded += new EventHandler(MissionFailedScreen_OnBlurEffectEnded);
			this.LineSpacing = this.PixelsToScreenUnitsY (35);
			this.MessagePosition = this.PixelsToScreenUnits (new Vector2(0, 170));

			// Set caption positions
			Vector2 pos = this.MessagePosition;
			for (int i = 0; i < this._capReasonLines.Length; i++)
			{
				this._capReasonLines[i].Position = pos;
				pos += new Vector2(0.0f, this.LineSpacing);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void _failStamp_OnShow(IUIControl sender)
		{
			this.EnableInput();
			this.InstructionBar.HideAllLabels();
			this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Quit);
			this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Retry);

			this._btnAgain.Focus();
		}

		/// <summary>
		/// 
		/// </summary>
		public override void OnStart()
		{
			base.OnStart();
			BrainGame.MusicManager.FadeMusic(0, AudioTags.MUSIC_FADE_MSECONDS);
			this.DisableInput();

			_failedSample.Play();

			this._missionFailedReason = this.Navigator.GlobalCache.Get<Stage.MissionFailedReasonType>(GlobalCacheKeys.MISSION_FAILED_REASON);
			string[] msg = new string[0];
			switch (this._missionFailedReason)
			{
			case Stage.MissionFailedReasonType.Incomplete:
				msg = LanguageManager.GetMultiString("MISSION_FAILED_INCOMPLETE");
				break;

			case Stage.MissionFailedReasonType.NotEnoughSnails:
				msg = LanguageManager.GetMultiString("MISSION_FAILED_NOT_ENOUGH_SNAILS");
				break;

			case Stage.MissionFailedReasonType.TimeExpired:
				msg = LanguageManager.GetMultiString("MISSION_FAILED_TIME_EXPIRED");
				break;

			case Stage.MissionFailedReasonType.KingIsDead:
				msg = LanguageManager.GetMultiString("MISSION_FAILED_KING_DEAD");
				break;

			}

			for (int i = 0; (i < msg.Length) && (i < this._capReasonLines.Length); i++)
			{
			this._capReasonLines[i].Text = msg[i];
			}
			this._leafsBoard.Visible = false;
			this._failStamp.Visible = false;
		}


		/// <summary>
		/// 
		/// </summary>
		void btnBack_OnAccept(IUIControl sender)
		{
			this.DisableInput();
			this.Navigator.GlobalCache.Set(GlobalCacheKeys.AUTO_SELECT_STAGE, true);
			this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
		}

		/// <summary>
		/// 
		/// </summary>
		void btnAgain_OnAccept(IUIControl sender)
		{
			this.DisableInput();
			// This flag disbles the stage briefing board. Nothing will be displayed, but
			// it's required because the stage load is there
			this.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, false);
			this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false);
			this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, false);
			this.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.MissionFailed);
			this.NavigateTo(ScreenType.StageStart.ToString(), ScreenTransitions.LeafsClosing, null);

		}

		#endregion


		#region Blur events
		/// <summary>
		///  
		/// </summary>
		void MissionFailedScreen_OnBlurEffectEnded(object sender, EventArgs e)
		{
			this._leafsBoard.Show();
		}

		#endregion

		#region Board events
		/// <summary>
		/// 
		/// </summary>
		void _board_OnShow(IUIControl sender)
		{
			this._failStamp.Show();
		}
		#endregion
	}
}
