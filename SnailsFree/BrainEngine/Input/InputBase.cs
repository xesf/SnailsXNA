using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace TwoBrainsGames.BrainEngine.Input
{
	public class InputBase
	{
		[Flags]
		public enum InputActions
		{
			None                = 0x00000,
			Accept              = 0x00001,
			Back                = 0x00002,
			Cancel              = 0x00040,
			CursorMotion        = 0x00080,
			CursorUp            = 0x00100,
			CursorDown          = 0x00200,
			CursorLeft          = 0x00400,
			CursorRight         = 0x00800,
			CursorUpClicked     = 0x01000,
			CursorDownClicked   = 0x02000,
			CursorLeftClicked   = 0x04000,
			CursorRightClicked  = 0x08000,
			AcceptDown          = 0x10000,
			AcceptUp            = 0x20000,
			CursorLeftReleased  = 0x40000,
			CursorRightReleased = 0x80000,
			Start               = 0x100000,
			FreeDrag            = 0x200000,
			HorizontalDrag      = 0x400000,
			VerticalDrag        = 0x800000,
			DragComplete        = 0x1000000,
			Flick               = 0x2000000,
			Next                = 0x4000000,
			Prev                = 0x8000000,
			DoubleTap           = 0x10000000,
		}



		public InputActions Actions;
		public static InputBase _instance;
		protected static MouseInput _mouse;
		protected static KeyboardInput _keyboard;
		public static InputBase Current { get { return _instance; } }
		public MouseInput Mouse { get { return _mouse; } }
		public KeyboardInput Keyboard { get { return _keyboard; } }

		protected bool _checkForController;
		protected static GamePadInput _gamepad;
		public GamePadInput GamePad { get { return _gamepad; } }
		public Vector2 PrevMotionPosition { get; protected set; }
		public Vector2 MotionPosition { get; protected set; }
		public Vector2 AuxiliaryMotionPosition { get; protected set; }
		public Vector2 FlickDelta { get; protected set; }

		public static TouchInput _touch;
		public TouchInput Touch { get { return InputBase._touch; } }

		public bool ActionAccept { get { return (this.Actions & InputActions.Accept) == InputActions.Accept; } }
		public bool ActionAcceptDown { get { return (this.Actions & InputActions.AcceptDown) == InputActions.AcceptDown; } }
		public bool ActionAcceptUp { get { return (this.Actions & InputActions.AcceptUp) == InputActions.AcceptUp; } }
		public bool ActionBack { get { return (this.Actions & InputActions.Back) == InputActions.Back; } }
		public bool ActionUp { get { return (this.Actions & InputActions.CursorUp) == InputActions.CursorUp; } }
		public bool ActionDown { get { return (this.Actions & InputActions.CursorDown) == InputActions.CursorDown; } }
		public bool ActionLeft { get { return (this.Actions & InputActions.CursorLeft) == InputActions.CursorLeft; } }
		public bool ActionRight { get { return (this.Actions & InputActions.CursorRight) == InputActions.CursorRight; } }
		public bool ActionCancel { get { return (this.Actions & InputActions.Cancel) == InputActions.Cancel; } }

		public bool ActionUpClicked { get { return (this.Actions & InputActions.CursorUpClicked) == InputActions.CursorUpClicked; } }
		public bool ActionDownClicked { get { return (this.Actions & InputActions.CursorDownClicked) == InputActions.CursorDownClicked; } }
		public bool ActionLeftClicked { get { return (this.Actions & InputActions.CursorLeftClicked) == InputActions.CursorLeftClicked; } }
		public bool ActionRightClicked { get { return (this.Actions & InputActions.CursorRightClicked) == InputActions.CursorRightClicked; } }
		public bool ActionLeftReleased { get { return (this.Actions & InputActions.CursorLeftReleased) == InputActions.CursorLeftReleased; } }
		public bool ActionRightReleased { get { return (this.Actions & InputActions.CursorRightReleased) == InputActions.CursorRightReleased; } }

		public bool ActionStart { get { return (this.Actions & InputActions.Start) == InputActions.Start; } }

		public bool ActionFreeDrag { get { return (this.Actions & InputActions.FreeDrag) == InputActions.FreeDrag; } }
		public bool ActionHorizontalDrag  { get { return (this.Actions & InputActions.HorizontalDrag) == InputActions.HorizontalDrag; } }
		public bool ActionVerticalDrag { get { return (this.Actions & InputActions.VerticalDrag) == InputActions.VerticalDrag; } }
		public bool ActionDragComplete { get { return (this.Actions & InputActions.DragComplete) == InputActions.DragComplete; } }
		public bool ActionFlick { get { return (this.Actions & InputActions.Flick) == InputActions.Flick; } }
		public bool ActionNext { get { return (this.Actions & InputActions.Next) == InputActions.Next; } }
		public bool ActionPrev { get { return (this.Actions & InputActions.Prev) == InputActions.Prev; } }

		public bool ActionDoubleTap { get { return (this.Actions & InputActions.DoubleTap) == InputActions.DoubleTap; } }
		public bool ActionNone { get { return (int)this.Actions == 0; } }

		public double TimeIdleMsecs { get; private set; }

		// If mouse, keyboard or xbox controller the mouse pointer is always down
		// On touch devices if the finger is not touching the controller this is set to false
		private bool PreviousMotionPointerDown { get; set; }
		private bool MotionPointerDown { get; set; }
		public bool IsMotionPointerDown { get { return this.MotionPointerDown; } }
		public bool WasMotionPointerDown { get { return this.PreviousMotionPointerDown; } }

		public PlayerIndex ControllerIndex 
		{
			get
			{
				if (this.GamePad != null)
				{ 
					return this.GamePad.PlayerIndex;
				}
				return PlayerIndex.One; // or maybe an exception...
			}
		}

		public bool WithMouse { get { return InputBase._mouse != null; } }
		public bool WithTouch { get { return InputBase._touch != null; } }

		public InputBase()
		{
			_instance = this;

		}

		public void Initialize()
		{
			this.MotionPointerDown = true;
			this.PreviousMotionPointerDown = true;

			if (BrainGame.Settings.UseMouse && _mouse == null)
			{
				_mouse = new MouseInput();
				BrainGame.AddComponent(_mouse);
			}

			if (BrainGame.Settings.UseKeyboard && _keyboard == null)
			{
				_keyboard = new KeyboardInput();
				BrainGame.AddComponent(_keyboard);
			}

			if (BrainGame.Settings.UseGamepad && _gamepad == null)
			{
				_gamepad = new GamePadInput(BrainGame.CurrentControllerIndex);
				BrainGame.AddComponent(_gamepad);
			}

			if (BrainGame.Settings.UseTouch && _touch == null)
			{
				_touch = new TouchInput();
				BrainGame.AddComponent(_touch);
				this.MotionPointerDown = false;
				this.PreviousMotionPointerDown = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Reset()
		{
			this.Actions = InputActions.None;

		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void ResetTimeIdle()
		{
			this.TimeIdleMsecs = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Update(BrainGameTime gameTime)
		{
			this.Reset();

			if (_mouse != null)
			{
				this.Actions &= ~InputActions.AcceptUp;

				// left button clicked
				if (_mouse.ButtonClicked(MouseButtons.LeftButton))
				{
					this.Actions |= InputActions.Accept;
				}
				// left button down
				if (_mouse.ButtonDown(MouseButtons.LeftButton))
				{
					this.Actions |= InputActions.AcceptDown;
					this.Actions |= InputActions.Start;
				}
				// left button up
				if (_mouse.ButtonReleased(MouseButtons.LeftButton))
				{
					this.Actions &= ~InputActions.AcceptDown;
					this.Actions |= InputActions.AcceptUp;
				}
				// right button down
				if (_mouse.ButtonDown(MouseButtons.RightButton))
				{
					this.Actions |= InputActions.Start;
				}
				// get mouse motion positions
				Vector2 mousePos = _mouse.GetPosition();
				this.MotionPosition = this.AuxiliaryMotionPosition = new Vector2(mousePos.X / BrainGame.ViewportRatioX, mousePos.Y / BrainGame.ViewportRatioY);
				//this.MotionPosition = this.AuxiliaryMotionPosition = _mouse.GetPosition();
				if (this.MotionPosition != this.PrevMotionPosition)
				{
					// set to use motion
					this.Actions |= InputActions.CursorMotion;
					this.PrevMotionPosition = this.MotionPosition;
				}
			}

			if (_gamepad != null)
			{
				// Gamplay Interaction buttons
				if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.A))
				{
					this.Actions |= InputActions.Accept;
				}

				// Gamplay Interaction buttons
				if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.X))
				{
					this.Actions |= InputActions.Cancel;
				}

				// Gamplay Interaction buttons
				if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.Y))
				{
					this.Actions |= InputActions.Cancel;
				}

				// Gamplay Interaction buttons
				if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.B) || 
					_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.Back))
				{
					this.Actions |= InputActions.Back;
				}
				// Motion inputs
				this.MotionPosition = _gamepad.GetLeftThumbStickPosition();
				this.AuxiliaryMotionPosition = _gamepad.GetRightThumbStickPosition();
				if (this.MotionPosition != Vector2.Zero || this.AuxiliaryMotionPosition != Vector2.Zero)
				{
					this.Actions |= InputActions.CursorMotion;
				}
				if (_gamepad.IsButtonPressed(Microsoft.Xna.Framework.Input.Buttons.DPadUp))
				{
					this.Actions |= InputActions.CursorUp;
					this.Actions &= ~InputActions.CursorMotion;
				}
				if (_gamepad.IsButtonPressed(Microsoft.Xna.Framework.Input.Buttons.DPadDown))
				{
					this.Actions |= InputActions.CursorDown;
					this.Actions &= ~InputActions.CursorMotion;
				}
				if (_gamepad.IsButtonPressed(Microsoft.Xna.Framework.Input.Buttons.DPadLeft))
				{
					this.Actions |= InputActions.CursorLeft;
					this.Actions &= ~InputActions.CursorMotion;
				}
				if (_gamepad.IsButtonPressed(Microsoft.Xna.Framework.Input.Buttons.DPadRight))
				{
					this.Actions |= InputActions.CursorRight;
					this.Actions &= ~InputActions.CursorMotion;
				}

				// Buttons clicks
				if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.DPadUp))
				{
					this.Actions |= InputActions.CursorUpClicked;
					this.Actions &= ~InputActions.CursorMotion;
				}
				if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.DPadDown))
				{
					this.Actions |= InputActions.CursorDownClicked;
					this.Actions &= ~InputActions.CursorMotion;
				}
				if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.DPadLeft))
				{
					this.Actions |= InputActions.CursorLeftClicked;
					this.Actions &= ~InputActions.CursorMotion;
				}
				if (_gamepad.IsButtonClicked(Microsoft.Xna.Framework.Input.Buttons.DPadRight))
				{
					this.Actions |= InputActions.CursorRightClicked;
					this.Actions &= ~InputActions.CursorMotion;
				}

				if (_gamepad.IsButtonReleased(Microsoft.Xna.Framework.Input.Buttons.DPadLeft))
				{
					this.Actions |= InputActions.CursorLeftReleased;
					this.Actions &= ~InputActions.CursorMotion;
				}

				if (_gamepad.IsButtonReleased(Microsoft.Xna.Framework.Input.Buttons.DPadRight))
				{
					this.Actions |= InputActions.CursorRightReleased;
					this.Actions &= ~InputActions.CursorMotion;
				}

				if (_gamepad.IsButtonPressed(Microsoft.Xna.Framework.Input.Buttons.Start))
				{
					this.Actions |= InputActions.Start;
				}

				if (_gamepad.IsButtonReleased(Microsoft.Xna.Framework.Input.Buttons.LeftShoulder) ||
					_gamepad.IsButtonReleased(Microsoft.Xna.Framework.Input.Buttons.LeftTrigger))

				{
					this.Actions |= InputActions.Prev;
				}

				if (_gamepad.IsButtonReleased(Microsoft.Xna.Framework.Input.Buttons.RightShoulder) ||
					_gamepad.IsButtonReleased(Microsoft.Xna.Framework.Input.Buttons.RightTrigger))
				{
					this.Actions |= InputActions.Next;
				}

				// Handle controller disconnected
				if (_gamepad.IsDisconnected && !_checkForController)
				{
					_checkForController = true;
				}
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

			}

			if (_keyboard != null)
			{

				if (_keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
				{
					this.Actions |= InputActions.Accept;
				}
				if (_keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
				{
					this.Actions |= InputActions.CursorUpClicked;
				}
				if (_keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
				{
					this.Actions |= InputActions.CursorDownClicked;
				}
				if (_keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
				{
					this.Actions |= InputActions.CursorLeftClicked;
				}
				if (_keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
				{
					this.Actions |= InputActions.CursorRightClicked;
				}
				if (_keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Back))
				{
					this.Actions |= InputActions.Back;
				}
				if (_keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
				{
					this.Actions |= InputActions.Cancel;
					this.Actions |= InputActions.Back;
				}

				// Arrow keys down
				if (_keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
				{
					this.Actions |= InputActions.CursorUp;
				}
				if (_keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
				{
					this.Actions |= InputActions.CursorDown;
				}
				if (_keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
				{
					this.Actions |= InputActions.CursorLeft;
				}
				if (_keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
				{
					this.Actions |= InputActions.CursorRight;
				}

				if (_keyboard.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Left))
				{
					this.Actions |= InputActions.CursorLeftReleased;
				}

				if (_keyboard.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Right))
				{
					this.Actions |= InputActions.CursorRightReleased;
				}

			}

			if (_touch != null)
			{
				this.PreviousMotionPointerDown = this.MotionPointerDown;
				this.FlickDelta = Vector2.Zero;
				this.MotionPosition = Vector2.Zero;

				if (_touch.CanGetPosition)
				{
					// get motion positions
					this.MotionPosition = _touch.GetTouchPosition();
					// set to use motion
					if (this.MotionPosition != Vector2.Zero)
					{
						// set to use motion
						this.Actions |= InputActions.CursorMotion;
					}
				}

				if (_touch.WasPressed || _touch.HasMoved)
				{
					this.MotionPointerDown = true;
					this.Actions |= InputActions.AcceptDown;
				}

				if (_touch.WasReleased)
				{
					this.MotionPointerDown = false;
					this.Actions &= ~InputActions.AcceptDown;
					this.Actions |= InputActions.AcceptUp;
					this.Actions |= InputActions.DragComplete;
				}

				if (_touch.IsTapping)
				{
					this.Actions |= InputActions.Accept;
					this.Actions |= InputActions.AcceptUp;
				}

				if (_touch.IsDoubleTapping)
				{
					this.Actions |= InputActions.DoubleTap;
					this.Actions |= InputActions.Start;
				}

				// treat WP7 Back Button
				if (Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				{
					this.Actions |= InputActions.Cancel;
					this.Actions |= InputActions.Back;
				}

				if (_touch.StartDragging)
				{
					this.Actions |= InputActions.FreeDrag;
					if (_touch.IsHorizontalDrag)
					{
						this.Actions |= InputActions.HorizontalDrag;
					}
					if (_touch.IsVerticalDrag)
					{
						this.Actions |= InputActions.VerticalDrag;
					}
				}

				if (_touch.EndDragging)
				{
					this.Actions |= InputActions.DragComplete;
				}

				if (_touch.HasFlick)
				{
					this.Actions |= InputActions.Flick;
					this.FlickDelta = _touch.FlickDelta;
				}
			}

			if (this.Actions == InputActions.None)
			{
				this.TimeIdleMsecs += gameTime.ElapsedRealTime.TotalMilliseconds;
			}
			else
			{
				this.TimeIdleMsecs = 0;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SetMotionPosition(Vector2 pos)
		{
			if (BrainGame.Instance.IsActive) // Don't remove this, because this will have inpact on the mouse when the game does not have the focus
			{
				if (Mouse != null)
				{
					Mouse.SetPosition((int)pos.X, (int)pos.Y);
					this.MotionPosition = pos;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Vector2 GetMotionPosition()
		{
			if (Mouse != null)
			{
				return _mouse.GetUpdatedPosition();
			}

			return new Vector2(0, 0);
		}

		/// <summary>
		/// Returns true if the starty game key was pressed
		/// If it's a gamepad, this method will check wich controller index was pressed and make that
		/// the active controller
		/// </summary>
		public bool CheckActionStartPressed()
		{
			if (this.GamePad != null)
			{
				int index = this.GamePad.CheckActionStartPressed();
				if (index != -1)
				{
					this.GamePad.PlayerIndex = (PlayerIndex)index;
					BrainGame.CurrentControllerIndex = this.GamePad.PlayerIndex;
				}
				return (index != -1);
			}

			return (this.ActionStart || this.ActionAccept);
		}

		public bool SkipAny()
		{
			if (this.GamePad != null)
			{
				return this.GamePad.SkipAny();
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void SuppressAccept()
		{
			this.Actions &= ~InputActions.Accept;
			this.Actions &= ~InputActions.AcceptDown;
			this.Actions &= ~InputActions.AcceptUp;
		}

	}
}
