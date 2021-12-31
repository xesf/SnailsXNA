using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.BrainEngine.Input;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace TwoBrainsGames.Snails.Input
{
    public class GameplayInput : TwoBrainsGames.BrainEngine.Input.InputBase
    {
        [Flags]
        public enum GamePlayButtons : ulong
        {
            None = 0x0000,
            ActionClicked = 0x0001,
            ActionDown = 0x0002,
            ToolUp = 0x0004,
            ToolDown = 0x0008,
            ToolReleased = 0x0010,
            TimeWarp = 0x0040,
            CameraUp = 0x0080,
            CameraDown = 0x0100,
            CameraLeft = 0x0200,
            CameraRight = 0x0400,
            CursorMotion = 0x0800,
            CursorUp = 0x1000,
            CursorDown = 0x2000,
            CursorLeft = 0x4000,
            CursorRight = 0x8000,
            DebugInfo = 0x10000,
            StageEditor = 0x20000,
            Pause = 0x40000,
            CameraMotion = 0x80000,
            ActionReleased = 0x100000,
            ToolBox = 0x200000,
            ToolSalt = 0x400000,
            ToolVitamin = 0x800000,
            ToolTrampoline = 0x1000000,
            ToolCopperBox = 0x2000000,
            ToolDynamite = 0x4000000,
            ToolApple = 0x8000000,
            ToolTimedBox = 0x10000000,
            PanMapStarted = 0x20000000,
            PanMapEnded = 0x40000000,
            CloseTutorial = 0x80000000,
            ToolTriggerBox = 0x100000000,
            LoadCustomStage = 0x200000000,
            RestartStage = 0x400000000,
          //  FlickMap = 0x800000000,
            PinchMapStarted = 0x1000000000,
            PinchMapEnded = 0x2000000000,
            CloseUpMap = 0x4000000000,
            ActionPressed = 0x8000000000,
            StageStart = 0x10000000000
        }

        #region Events
        public delegate void InputEvent(GameplayInput sender, BrainGameTime gameTime, out Vector2 newMotionPosition);
        public event InputEvent OnAfterUpdate; // Occurs after Update
        #endregion

        public GamePlayButtons GameButtons;
        public int ScrollValue { get; protected set; }
        public Vector2 CameraMotionPosition { get; protected set; }

        public float PinchScale { get; protected set; }
        public Vector2 PinchCenterPosition { get; protected set; }

        public bool IsGameButtonSet(GamePlayButtons button) { return (GameButtons & button) == button; }
        public bool IsActionClicked { get { return (GameButtons & GamePlayButtons.ActionClicked) == GamePlayButtons.ActionClicked; } }
        public bool IsStageStartSelected { get { return (GameButtons & GamePlayButtons.StageStart) == GamePlayButtons.StageStart; } }
        public bool IsActionDown { get { return (GameButtons & GamePlayButtons.ActionDown) == GamePlayButtons.ActionDown; } }
        public bool IsActionReleased { get { return (GameButtons & GamePlayButtons.ActionReleased) == GamePlayButtons.ActionReleased; } }
        public bool IsToolUpClicked { get { return (GameButtons & GamePlayButtons.ToolUp) == GamePlayButtons.ToolUp; } }
        public bool IsToolDownClicked { get { return (GameButtons & GamePlayButtons.ToolDown) == GamePlayButtons.ToolDown; } }
        public bool IsToolReleasedClicked { get { return (GameButtons & GamePlayButtons.ToolReleased) == GamePlayButtons.ToolReleased; } }
        public bool TimeWarpSelected { get { return (GameButtons & GamePlayButtons.TimeWarp) == GamePlayButtons.TimeWarp; } }
        public bool IsCameraUpPressed { get { return (GameButtons & GamePlayButtons.CameraUp) == GamePlayButtons.CameraUp; } }
        public bool IsCameraDownPressed { get { return (GameButtons & GamePlayButtons.CameraDown) == GamePlayButtons.CameraDown; } }
        public bool IsCameraLeftPressed { get { return (GameButtons & GamePlayButtons.CameraLeft) == GamePlayButtons.CameraLeft; } }
        public bool IsCameraRightPressed { get { return (GameButtons & GamePlayButtons.CameraRight) == GamePlayButtons.CameraRight; } }
        public bool CursorMotion { get { return (GameButtons & GamePlayButtons.CursorMotion) == GamePlayButtons.CursorMotion; } }
        public bool IsCursorUpPressed { get { return (GameButtons & GamePlayButtons.CursorUp) == GamePlayButtons.CursorUp; } }
        public bool IsCursorDownPressed { get { return (GameButtons & GamePlayButtons.CursorDown) == GamePlayButtons.CursorDown; } }
        public bool IsCursorLeftPressed { get { return (GameButtons & GamePlayButtons.CursorLeft) == GamePlayButtons.CursorLeft; } }
        public bool IsCursorRightPressed { get { return (GameButtons & GamePlayButtons.CursorRight) == GamePlayButtons.CursorRight; } }
        public bool ActionOpenDebugOptions { get { return (GameButtons & GamePlayButtons.DebugInfo) == GamePlayButtons.DebugInfo; } }
        public bool ActionStageEditor { get { return (GameButtons & GamePlayButtons.StageEditor) == GamePlayButtons.StageEditor; } }
        public bool ActionLoadCustomStage { get { return (GameButtons & GamePlayButtons.LoadCustomStage) == GamePlayButtons.LoadCustomStage; } }
        public bool ActionPause { get { return (GameButtons & GamePlayButtons.Pause) == GamePlayButtons.Pause; } }
        public bool CameraMotion { get { return (GameButtons & GamePlayButtons.CameraMotion) == GamePlayButtons.CameraMotion; } }
        public bool MapPanStarted { get { return (GameButtons & GamePlayButtons.PanMapStarted) == GamePlayButtons.PanMapStarted; } }
        public bool MapPanEnded { get { return (GameButtons & GamePlayButtons.PanMapEnded) == GamePlayButtons.PanMapEnded; } }
        public bool CloseTutorialSelected { get { return (GameButtons & GamePlayButtons.CloseTutorial) == GamePlayButtons.CloseTutorial; } }
        public bool RestartSelected { get { return (GameButtons & GamePlayButtons.RestartStage) == GamePlayButtons.RestartStage; } }
        public bool MapPinchStarted { get { return (GameButtons & GamePlayButtons.PinchMapStarted) == GamePlayButtons.PinchMapStarted; } }
        public bool MapPinchEnded { get { return (GameButtons & GamePlayButtons.PinchMapEnded) == GamePlayButtons.PinchMapEnded; } }
        public bool IsActionPressed { get { return (GameButtons & GamePlayButtons.ActionPressed) == GamePlayButtons.ActionPressed; } }

        public GameplayInput()
        {
        }


        public override void Update (BrainGameTime gameTime)
		{
			GameButtons = GamePlayButtons.None;

			if (_mouse != null) {
				// left button clicked
				if (_mouse.ButtonClicked (MouseButtons.LeftButton)) {
					GameButtons |= GamePlayButtons.ActionClicked;
                    GameButtons |= GamePlayButtons.StageStart;
				}
				// left button clicked
				if (_mouse.ButtonPressed (MouseButtons.LeftButton)) {
					GameButtons |= GamePlayButtons.ActionPressed;
				}
                
				// left button pressed and not released
				if (_mouse.ButtonDown (MouseButtons.LeftButton)) {
					GameButtons |= GamePlayButtons.ActionDown;
				}
				// left button pressed and not released
				if (_mouse.ButtonClicked (MouseButtons.LeftButton)) { // Button clicked is perfect to set a mouse up event because the click event happens when the button is released
					GameButtons |= GamePlayButtons.ActionReleased;
				}
				// Right button pressed
				if (_mouse.ButtonDown (MouseButtons.RightButton)) {
					GameButtons |= GamePlayButtons.PanMapStarted;
				}
				// Right button released
				if (_mouse.ButtonReleased (MouseButtons.RightButton)) {
					GameButtons |= GamePlayButtons.PanMapEnded;
				}
				// scroll values
				ScrollValue = _mouse.GetScrollWheelValueTransformed ();
				if (ScrollValue != 0) {
					if (ScrollValue == -1) {
						GameButtons |= GamePlayButtons.ToolUp;
					} else if (ScrollValue == 1) {
						GameButtons |= GamePlayButtons.ToolDown;
					}
				}

				// set to use motion
				GameButtons |= GamePlayButtons.CursorMotion;
				// get mouse motion positions
				Vector2 mousePos = _mouse.GetPosition ();
				MotionPosition = new Vector2 (mousePos.X / SnailsGame.ViewportRatioX, mousePos.Y / SnailsGame.ViewportRatioY);
				//MotionX = _mouse.GetPositionX();
				//MotionY = _mouse.GetPositionY();
			}

			if (_gamepad != null) {
				// Gamplay Interaction buttons

				// Clicked
				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.A)) {
					GameButtons |= GamePlayButtons.ActionClicked;
				}
				// Pressed
				if (_gamepad.IsButtonPressed (Microsoft.Xna.Framework.Input.Buttons.A)) {
					GameButtons |= GamePlayButtons.ActionDown;
					GameButtons |= GamePlayButtons.ActionPressed;
				}
				// Released
				if (_gamepad.IsButtonReleased (Microsoft.Xna.Framework.Input.Buttons.A)) {
					GameButtons |= GamePlayButtons.ActionReleased;
				}

				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.B)) {
					GameButtons |= GamePlayButtons.CloseTutorial;
				}

				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.LeftShoulder) ||
					_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.LeftTrigger)) {
					GameButtons |= GamePlayButtons.ToolUp;
				}

				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.RightShoulder) ||
					_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.RightTrigger)) {
					GameButtons |= GamePlayButtons.ToolDown;
				}

				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.X)) {
					GameButtons |= GamePlayButtons.ToolReleased;
				}

				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.Y)) {
					GameButtons |= GamePlayButtons.TimeWarp;
				}

				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.Back) || 
					_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.BigButton)) {
					GameButtons |= GamePlayButtons.Pause;
				}
/*
                // Right button pressed
                if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.RightTrigger))
                {
                    GameButtons |= GamePlayButtons.PanMapStarted;
                }
                // Right button released
                if (_gamepad.IsButtonReleased(Microsoft.Xna.Framework.Input.Buttons.RightTrigger))
                {
                    GameButtons |= GamePlayButtons.PanMapEnded;
                }*/

#if XBOX
                // if guide appear or controller has been disconnected, than pause the game
                if (Guide.IsVisible || _gamepad.IsDisconnected)
                {
                    GameButtons |= GamePlayButtons.Pause;
                    if (!_checkForController)
                    {
                        _checkForController = true;
                    }
                }

                // Handle controller disconnected
                if (_checkForController)
                {
                    int index = this.GamePad.IsControllerConnected(BrainGame.CurrentControllerIndex);
                    if (index != -1)
                    {
                        this.GamePad.PlayerIndex = (PlayerIndex)index;
                        BrainGame.CurrentControllerIndex = this.GamePad.PlayerIndex;
                        _checkForController = false;
                    }
                }
#endif

				// Motion inputs
				GameButtons |= GamePlayButtons.CursorMotion;
				MotionPosition = _gamepad.GetLeftThumbStickPosition ();

				// The following code makes the cursor move slowly when the stick isn't fully moved
				// This is done to make the cursor less sensible and make tools like dynamites, salt, trampolins 
				// easyer to use
				if (MotionPosition != Vector2.Zero) {
					float motionSpeed = MotionPosition.Length ();
					if (motionSpeed < 0.9f) {
						this.MotionPosition *= 0.3f;
					}
				}

				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.DPadUp)) {
					GameButtons |= GamePlayButtons.CursorUp;
					GameButtons &= ~GamePlayButtons.CursorMotion;
				}
				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.DPadDown)) {
					GameButtons |= GamePlayButtons.CursorDown;
					GameButtons &= ~GamePlayButtons.CursorMotion;
				}
				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.DPadLeft)) {
					GameButtons |= GamePlayButtons.CursorLeft;
					GameButtons &= ~GamePlayButtons.CursorMotion;
				}
				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.DPadRight)) {
					GameButtons |= GamePlayButtons.CursorRight;
					GameButtons &= ~GamePlayButtons.CursorMotion;
				}

				if (_gamepad.IsButtonClicked (Microsoft.Xna.Framework.Input.Buttons.Start)) {
                    GameButtons |= GamePlayButtons.RestartStage;
				}

				// camera motion
				GameButtons |= GamePlayButtons.CameraMotion;
				CameraMotionPosition = _gamepad.GetRightThumbStickPosition ();
              
			}

			if (_keyboard != null) {
				// Tools shortcuts
				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.C)) {
					GameButtons |= GamePlayButtons.ToolBox;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.L)) {
					GameButtons |= GamePlayButtons.ToolSalt;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.R)) {
					GameButtons |= GamePlayButtons.ToolVitamin;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.T)) {
					GameButtons |= GamePlayButtons.ToolTrampoline;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.M)) {
					GameButtons |= GamePlayButtons.ToolCopperBox;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.B)) {
					GameButtons |= GamePlayButtons.ToolDynamite;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.P)) {
					GameButtons |= GamePlayButtons.ToolApple;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.K)) {
					GameButtons |= GamePlayButtons.ToolTimedBox;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.G)) {
					GameButtons |= GamePlayButtons.ToolTriggerBox;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.F9)) {
					GameButtons |= GamePlayButtons.DebugInfo;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.E)) {
					GameButtons |= GamePlayButtons.StageEditor;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.F3)) {
					GameButtons |= GamePlayButtons.LoadCustomStage;
				}

				if (_keyboard.IsKeyPressed (Microsoft.Xna.Framework.Input.Keys.Space)) {
					GameButtons |= GamePlayButtons.TimeWarp;
					GameButtons |= GamePlayButtons.CloseTutorial; // FIXME : review this
                    GameButtons |= GamePlayButtons.StageStart;
                }

                if (_keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    GameButtons |= GamePlayButtons.StageStart;
                }

				// camera motion
				if (_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.Up) ||
					_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.W)) {
					GameButtons |= GamePlayButtons.CameraUp;
				}
				if (_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.Down) ||
					_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.S)) {
					GameButtons |= GamePlayButtons.CameraDown;
				}
				if (_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.Left) ||
					_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.A)) {
					GameButtons |= GamePlayButtons.CameraLeft;
				}
				if (_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.Right) ||
					_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.D)) {
					GameButtons |= GamePlayButtons.CameraRight;
				}

				if (_keyboard.IsKeyDown (Microsoft.Xna.Framework.Input.Keys.Escape)) {
					GameButtons |= GamePlayButtons.Pause;
				}

                if (_keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
                {
                    GameButtons |= GamePlayButtons.RestartStage;
                }
			}

			if (_touch != null) { // same behavior as mouse input
				/*this.PinchScale = 0;
                this.FlickDelta = Vector2.Zero;*/
				this.MotionPosition = Vector2.Zero;

				if (_touch.CanGetPosition) {
					// get motion positions
					this.MotionPosition = _touch.GetTouchPosition ();
					// set to use motion
					if (this.MotionPosition != Vector2.Zero)
						this.GameButtons |= GamePlayButtons.CursorMotion;
				}

				if (_touch.IsTapping) { // only check this if we didn't double tap for TimeWrap (this will avoid adding tools)
					this.GameButtons |= GamePlayButtons.ActionClicked;
					this.GameButtons |= GamePlayButtons.ActionReleased;
                    this.GameButtons |= GamePlayButtons.StageStart;
				}

				if (_touch.IsDoubleTapping) {
					this.GameButtons |= GamePlayButtons.CloseUpMap;
				}

				if (_touch.IsDown) {
					this.GameButtons |= GamePlayButtons.ActionDown;
				}

				if (_touch.WasPressed) {
					this.GameButtons |= GamePlayButtons.ActionPressed;
				}

				if (_touch.WasReleased) {
					this.GameButtons |= GamePlayButtons.ActionReleased;
					this.GameButtons |= GamePlayButtons.PanMapEnded;
				}

				if (_touch.StartDragging) { // only if not interacting with objects
					this.GameButtons |= GamePlayButtons.PanMapStarted;
				}
				if (_touch.EndDragging) {
					this.GameButtons |= GamePlayButtons.PanMapEnded;
				}
				/*
                if (_touch.HasFlick && !this.MapPanStarted)
                {
                    this.GameButtons |= GamePlayButtons.FlickMap;
                    this.FlickDelta = _touch.FlickDelta;
                }
                */
				this.PinchScale = 1;
				if (_touch.StartPinching) {
					this.GameButtons |= GamePlayButtons.PinchMapStarted;
					this.PinchScale = _touch.PinchDistance * 0.005f;
					this.PinchScale = (_touch.PinchDistance / _touch.PinchPrevDistance);
					this.PinchCenterPosition = _touch.PinchCenterPosition;
				}
				/*  if (_touch.HasFlick)
                {
                    this.GameButtons |= GamePlayButtons.FlickMap;
                    this.FlickDelta = _touch.FlickDelta;
                }*/
				if (_touch.EndPinching) {
					this.GameButtons |= GamePlayButtons.PinchMapEnded;
				}

				// treat WP7 Back Button
				if (Microsoft.Xna.Framework.Input.GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
					this.GameButtons |= GamePlayButtons.Pause;
				}
			}

			// special condition to handle missing right button mouse on Mac OS X (and others OS)
            if (_mouse != null && _keyboard != null)
            {
                // Right button pressed
                if (_mouse.ButtonDown(MouseButtons.LeftButton) &&
                    (_keyboard.IsKeyDown(Keys.LeftControl) || _keyboard.IsKeyDown(Keys.RightControl)))
                {
                    GameButtons |= GamePlayButtons.PanMapStarted;
                }
                // Right button released
                if (_mouse.ButtonReleased(MouseButtons.LeftButton) &&
                    (_keyboard.IsKeyDown(Keys.LeftControl) || _keyboard.IsKeyDown(Keys.RightControl)))
                {
                    GameButtons |= GamePlayButtons.PanMapEnded;
                }
            }

            if (this.OnAfterUpdate != null)
            {
                Vector2 newPos;
                this.OnAfterUpdate(this, gameTime, out newPos);
                this.MotionPosition = newPos;
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        public bool IsToolShortcutPressed(int toolIndex)
        {
            Int64 key = (Int64)GameplayInput.GamePlayButtons.Tool1Shortcut;
            key = key << toolIndex;
            return (GameButtons & (GameplayInput.GamePlayButtons)key) == (GameplayInput.GamePlayButtons)key; 
        }*/

        public bool QueryActionDown(GamePlayButtons action)
        {
            return ((GameButtons & action) == action);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            this.GameButtons = GamePlayButtons.None;
        }
    }
}
