using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TwoBrainsGames.BrainEngine.Input
{
    public class ControllerInput
    {
        #region Members
        bool _allowVibration;
        PlayerIndex _playerIndex;
        // Keyboard
        KeyboardState _keyboardState;
        KeyboardState _lastKeyboardState;
        Dictionary<Buttons, Keys> _keyboardMap;
        // Gamepad
        GamePadState _gamePadState;
        GamePadState _lastGamePadState;
        #endregion

        #region Properties
        public bool AllowVibration 
        {
            get
            {
                return _allowVibration;
            }
            set
            {
                _allowVibration = value;
            }
        }
        #endregion

        public ControllerInput(PlayerIndex playerIndex) 
            : this(playerIndex, null)
        { }

        public ControllerInput(PlayerIndex playerIndex, Dictionary<Buttons, Keys> keyboardMap)
        {
           this._playerIndex = playerIndex;
           this._keyboardMap = keyboardMap;
           this._allowVibration = true; // default value
        }

        public void Update()
        {
            // update keyboard states
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState(_playerIndex);
            // update gamepad states
            _lastGamePadState = _gamePadState;
            _gamePadState = GamePad.GetState(_playerIndex);
        }

        public bool IsKeyPressed(Buttons button)
        {
            bool isPressed = false;

            if (_gamePadState.IsConnected)
            {
                isPressed = (_gamePadState.IsButtonDown(button) && _lastGamePadState.IsButtonUp(button));
            }
            else if (_keyboardMap != null)
            {
                Keys key = _keyboardMap[button];
                isPressed = (_keyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key));
            }

            return isPressed;
        }

        public bool TestKeyPressed(Buttons button)
        {
            bool isPressed = false;

            if (_gamePadState.IsConnected)
            {
                isPressed = _gamePadState.IsButtonDown(button);
            }
            else if (_keyboardMap != null)
            {
                Keys key = _keyboardMap[button];
                isPressed = _keyboardState.IsKeyDown(key);
            }

            return isPressed;
        }

        public bool IsKeyReleased(Buttons button)
        {
            bool isReleased = false;

            if (_gamePadState.IsConnected)
            {
                isReleased = (_gamePadState.IsButtonUp(button) && _lastGamePadState.IsButtonDown(button));
            }
            else if (_keyboardMap != null)
            {
                Keys key = _keyboardMap[button];
                isReleased = (_keyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key));
            }

            return isReleased;
        }

        public Vector2 GetLeftThumbStickPosition()
        {
            Vector2 thumbPosition = Vector2.Zero;

            if (_gamePadState.IsConnected)
            {
                thumbPosition = _gamePadState.ThumbSticks.Left;
            }
            else if (_keyboardMap != null)
            {
                if (_keyboardState.IsKeyDown(_keyboardMap[Buttons.LeftThumbstickUp]))
                {
                    thumbPosition.Y = 1;
                }
                else if (_keyboardState.IsKeyDown(_keyboardMap[Buttons.LeftThumbstickDown]))
                {
                    thumbPosition.Y = -1;
                }

                if (_keyboardState.IsKeyDown(_keyboardMap[Buttons.LeftThumbstickRight]))
                {
                    thumbPosition.X = 1;
                }
                else if (_keyboardState.IsKeyDown(_keyboardMap[Buttons.LeftThumbstickLeft]))
                {
                    thumbPosition.X = -1;
                }
            }
            return thumbPosition;
        }

        public Vector2 GetRightThumbStickPosition()
        {
            Vector2 thumbPosition = Vector2.Zero;

            if (_gamePadState.IsConnected)
                thumbPosition = _gamePadState.ThumbSticks.Right;
            else if (_keyboardMap != null)
            {
                if (_keyboardState.IsKeyDown(_keyboardMap[Buttons.RightThumbstickUp]))
                {
                    thumbPosition.Y = 1;
                }
                else if (_keyboardState.IsKeyDown(_keyboardMap[Buttons.RightThumbstickDown]))
                {
                    thumbPosition.Y = -1;
                }

                if (_keyboardState.IsKeyDown(_keyboardMap[Buttons.RightThumbstickRight]))
                {
                    thumbPosition.X = 1;
                }
                else if (_keyboardState.IsKeyDown(_keyboardMap[Buttons.RightThumbstickLeft]))
                {
                    thumbPosition.X = -1;
                }
            }

            return thumbPosition;
        }

        /// <summary>
        /// Set default game pad vibration
        /// </summary>
        /// <param name="leftMotor"></param>
        /// <param name="rightMotor"></param>
        /// <returns></returns>
        public bool StopVibration()
        {
            return SetVibration(0, 0);
        }

        /// <summary>
        /// Set vibration game pad motor values
        /// </summary>
        /// <param name="leftMotor"></param>
        /// <param name="rightMotor"></param>
        /// <returns></returns>
        public bool SetVibration(float leftMotor, float rightMotor)
        {
#if !WIN8 && !MONOGAME
            if (_allowVibration && _gamePadState.IsConnected)
            {
                return GamePad.SetVibration(_playerIndex, leftMotor, rightMotor);
            }
#endif
            return false;
        }
    }
}
