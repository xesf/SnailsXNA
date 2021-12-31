using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Input
{
    class DebugOptionsInput : InputBase
	{
		public const Keys KEY_SHOW_HIDE_HELP = Keys.F9;
		public const Keys KEY_SHOW_HIDE_SPRITES = Keys.F2;
		public const Keys KEY_SHOW_HIDE_TILES = Keys.T;
		public const Keys KEY_SHOW_HIDE_BOUNDINGBOXES = Keys.B;
		public const Keys KEY_SHOW_HIDE_PATHS = Keys.P;
		public const Keys KEY_SHOW_QUADTREE = Keys.Q;
		public const Keys KEY_STAGE_EDITOR = Keys.F11;
		public const Keys KEY_RELOAD_STAGE = Keys.R;
		public const Keys KEY_NEXT_STAGE = Keys.M;
		public const Keys KEY_PREV_STAGE = Keys.N;
		public const Keys KEY_SHOW_HIDE_DEBUG_INFO = Keys.D;
		public const Keys KEY_ENABLE_AVERAGES = Keys.A;
		public const Keys KEY_DEBUG_INFO_POSITION = Keys.I;
        public const Keys KEY_GENERATE_ALL_THUMBS = Keys.G;
        public const Keys KEY_GENERATE_CURRENT_THUMB = Keys.H;

	    [Flags]
		public enum GameHelpButtons
		{
			None = 0x0,
			ShowHideSprites = 0x002,
			ShowTiles = 0x004,
			ShowBoundingBoxes = 0x008,
			ShowHidePaths = 0x010,
			ShowHideQuatree = 0x020,
			XXX1 = 0x040,
			ShowDebugInfo = 0x080,
			EnableAverage = 0x100,
			DebugInfoChangePosition = 0x200,
			ShowStageEditor = 0x400,
			XXX2 = 0x800,
			ReloadStage = 0x1000,
			NextStage = 0x2000,
			PrevStage = 0x4000,
			HideDebugOptionsInput = 0x8000,
            GenerateAllThumbs = 0x10000,
            GenerateCurrentThumb = 0x20000,
		}

		public GameHelpButtons HelpButtons { get; private set; }

		public DebugOptionsInput()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsHelpButtonSet(GameHelpButtons help)
		{
			return (HelpButtons & help) == help;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Update(BrainGameTime gameTime)
		{
			this.HelpButtons = GameHelpButtons.None;

			if (_gamepad != null)
            {
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.Start))
                {
				    this.HelpButtons |= GameHelpButtons.HideDebugOptionsInput;
                }

                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.A))
                {
                    this.HelpButtons |= GameHelpButtons.ShowBoundingBoxes;
                }

                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.DPadUp))
                {
                    this.HelpButtons |= GameHelpButtons.ShowHidePaths;
                }
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.DPadDown))
                {
                    this.HelpButtons |= GameHelpButtons.ShowTiles;
                }
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.DPadLeft))
                {
                    this.HelpButtons |= GameHelpButtons.ShowHideSprites;
                }
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.DPadRight))
                {
                    this.HelpButtons |= GameHelpButtons.ShowHideQuatree;
                }
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.B))
                {
                    this.HelpButtons |= GameHelpButtons.ShowDebugInfo;
                }

                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.Y))
                {
                    this.HelpButtons |= GameHelpButtons.EnableAverage;
                }

                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.X))
                {
                    this.HelpButtons |= GameHelpButtons.ReloadStage;
                }
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.RightTrigger))
                {
                    this.HelpButtons |= GameHelpButtons.NextStage;
                }
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.LeftTrigger))
                {
                    this.HelpButtons |= GameHelpButtons.PrevStage;
                }
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.RightShoulder))
                {
                    this.HelpButtons |= GameHelpButtons.DebugInfoChangePosition;
                }
            }

			if (_keyboard != null)
			{
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_SHOW_HIDE_HELP))
				{
					this.HelpButtons |= GameHelpButtons.HideDebugOptionsInput;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_SHOW_HIDE_SPRITES))
				{
					this.HelpButtons |= GameHelpButtons.ShowHideSprites;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_SHOW_HIDE_TILES))
				{
					this.HelpButtons |= GameHelpButtons.ShowTiles;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_SHOW_HIDE_PATHS))
				{
					this.HelpButtons |= GameHelpButtons.ShowHidePaths;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_SHOW_QUADTREE))
				{
					this.HelpButtons |= GameHelpButtons.ShowHideQuatree;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_SHOW_HIDE_BOUNDINGBOXES))
				{
					this.HelpButtons |= GameHelpButtons.ShowBoundingBoxes;
				}

				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_SHOW_HIDE_DEBUG_INFO))
				{
					this.HelpButtons |= GameHelpButtons.ShowDebugInfo;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_DEBUG_INFO_POSITION))
				{
					this.HelpButtons |= GameHelpButtons.DebugInfoChangePosition;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_ENABLE_AVERAGES))
				{
					this.HelpButtons |= GameHelpButtons.EnableAverage;
				}

				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_STAGE_EDITOR))
				{
					this.HelpButtons |= GameHelpButtons.ShowStageEditor;
				}

				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_STAGE_EDITOR))
				{
					this.HelpButtons |= GameHelpButtons.ShowStageEditor;
				}

				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_RELOAD_STAGE))
				{
					this.HelpButtons |= GameHelpButtons.ReloadStage;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_NEXT_STAGE))
				{
					this.HelpButtons |= GameHelpButtons.NextStage;
				}
				if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_PREV_STAGE))
				{
					this.HelpButtons |= GameHelpButtons.PrevStage;
				}
                
                if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_GENERATE_ALL_THUMBS))
                {
                    this.HelpButtons |= GameHelpButtons.GenerateAllThumbs;
                }
                if (_keyboard.IsKeyPressed(DebugOptionsInput.KEY_GENERATE_CURRENT_THUMB))
                {
                    this.HelpButtons |= GameHelpButtons.GenerateCurrentThumb;
                }
			}
		}

    /// <summary>
    /// 
    /// </summary>
    public override void Reset()
    {
      this.HelpButtons = GameHelpButtons.None;
    }
  }
}
