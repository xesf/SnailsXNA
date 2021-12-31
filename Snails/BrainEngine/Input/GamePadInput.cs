using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TwoBrainsGames.BrainEngine.Input
{
    public class GamePadInput : GameComponent
    {
        GamePadState PreviousState { get; set; }
        GamePadState CurrentState { get; set; }

        public PlayerIndex PlayerIndex { get; set; }
        public bool AllowVibration { get; set; }

        public bool IsConnected { get { return CurrentState.IsConnected; } }
        public bool IsDisconnected { get { return !IsConnected; } }

        public GamePadInput() :
            base(BrainGame.Instance)
        {
            this.PlayerIndex = Microsoft.Xna.Framework.PlayerIndex.One;
        }

        public GamePadInput(PlayerIndex _playerIndex) :
            base(BrainGame.Instance)
        {
            this.PlayerIndex = _playerIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.CurrentState = GamePad.GetState(this.PlayerIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            this.PreviousState = this.CurrentState;
            this.CurrentState = GamePad.GetState(this.PlayerIndex);
        }

        public bool IsButtonClicked(Buttons button)
        {
            bool isPressed = false;

            if (this.CurrentState.IsConnected)
            {
                isPressed = (this.CurrentState.IsButtonDown(button) && this.PreviousState.IsButtonUp(button));
            }

            return isPressed;
        }

        public bool IsButtonPressed(Buttons button)
        {
            bool isPressed = false;

            if (this.CurrentState.IsConnected)
            {
                isPressed = this.CurrentState.IsButtonDown(button);
            }

            return isPressed;
        }

        public bool IsButtonReleased(Buttons button)
        {
            bool isReleased = false;

            if (this.CurrentState.IsConnected)
            {
                isReleased = (this.CurrentState.IsButtonUp(button) && this.PreviousState.IsButtonDown(button));
            }

            return isReleased;
        }

        public Vector2 GetLeftThumbStickPosition()
        {
            Vector2 thumbPosition = Vector2.Zero;

            if (this.CurrentState.IsConnected)
            {
                thumbPosition = this.CurrentState.ThumbSticks.Left;
            }

            return thumbPosition;
        }

        public Vector2 GetRightThumbStickPosition()
        {
            Vector2 thumbPosition = Vector2.Zero;

            if (this.CurrentState.IsConnected)
            {
                thumbPosition = this.CurrentState.ThumbSticks.Right;
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
            if (this.AllowVibration && this.CurrentState.IsConnected)
            {
                return GamePad.SetVibration(this.PlayerIndex, leftMotor, rightMotor);
            }
#endif
            return false;
        }

        /// <summary>
        /// Check if the start button was pressed and returns the controller index
        /// that pressed the key
        /// This code was taken directly from
        /// http://create.msdn.com/en-US/education/catalog/article/bestpractices_31
        /// </summary>
        public int CheckActionStartPressed()
        {
            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                if (GamePad.GetState(index).Buttons.Start == ButtonState.Pressed)
                {
                    return (int)index;
                }
            }

            return -1;
        }

        public bool SkipAny()
        {
            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                if (GamePad.GetState(index).Buttons.A == ButtonState.Pressed)
                {
                    return true;
                }
            }

            return false;
        }

        public int IsControllerConnected(PlayerIndex playerIndex)
        {
            // check for current player index controller
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return (int)playerIndex;
            }

            // check for other controllers
            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                if (GamePad.GetState(index).IsConnected)
                {
                    return (int)index;
                }
            }

            return -1;
        }
    }
}
