using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TwoBrainsGames.BrainEngine.Input
{
    public enum MouseButtons
    {
        LeftButton = 1,
        MiddleButton = 2,
        RightButton = 4
    }

    public class MouseInput : GameComponent
    {
        MouseState PreviousState { get; set; }
        MouseState CurrentState { get; set; }

        public MouseInput() :
            base(BrainGame.Instance)
        { }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.CurrentState = Mouse.GetState();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            this.PreviousState = this.CurrentState;
            this.CurrentState = Mouse.GetState();
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetPositionX()
        {
            return this.CurrentState.X;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetPositionY()
        {
            return this.CurrentState.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 GetPosition()
        {
            return new Vector2(this.CurrentState.X, this.CurrentState.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 GetUpdatedPosition()
        {
            this.CurrentState = Mouse.GetState();
            return new Vector2(this.CurrentState.X, this.CurrentState.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetScrollWheelValue()
        {
            return this.CurrentState.ScrollWheelValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetScrollWheelValueTransformed()
        {
            int prev = this.PreviousState.ScrollWheelValue;
            int curr = this.CurrentState.ScrollWheelValue;
            return ((curr - prev)/120)*-1;
        }

    
        /// <summary>
        /// 
        /// </summary>
        public bool ButtonClicked(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    if (this.PreviousState.LeftButton == ButtonState.Pressed && this.CurrentState.LeftButton == ButtonState.Released)
                        return true;
                    break;
                case MouseButtons.MiddleButton:
                    if (this.PreviousState.MiddleButton == ButtonState.Pressed && this.CurrentState.MiddleButton == ButtonState.Released)
                        return true;
                    break;
                case MouseButtons.RightButton:
                    if (this.PreviousState.RightButton == ButtonState.Pressed && this.CurrentState.RightButton == ButtonState.Released)
                        return true;
                    break;
            }

            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool ButtonDown(MouseButtons button)
        {
            switch (button)
            { 
                case MouseButtons.LeftButton:
                    if (this.CurrentState.LeftButton == ButtonState.Pressed)
                        return true;
                    break;
                case MouseButtons.MiddleButton:
                    if (this.CurrentState.MiddleButton == ButtonState.Pressed)
                        return true;
                    break;
                case MouseButtons.RightButton:
                    if (this.CurrentState.RightButton == ButtonState.Pressed)
                        return true;
                    break;
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ButtonPressed(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    if (this.CurrentState.LeftButton == ButtonState.Pressed && this.PreviousState.LeftButton == ButtonState.Released)
                        return true;
                    break;
                case MouseButtons.MiddleButton:
                    if (this.CurrentState.MiddleButton == ButtonState.Pressed && this.PreviousState.MiddleButton == ButtonState.Released)
                        return true;
                    break;
                case MouseButtons.RightButton:
                    if (this.CurrentState.RightButton == ButtonState.Pressed && this.PreviousState.RightButton == ButtonState.Released)
                        return true;
                    break;
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ButtonReleased(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    if (this.PreviousState.LeftButton == ButtonState.Pressed &&
                        this.CurrentState.LeftButton == ButtonState.Released)
                        return true;
                    break;
                case MouseButtons.MiddleButton:
                    if (this.PreviousState.MiddleButton == ButtonState.Pressed &&
                        this.CurrentState.MiddleButton == ButtonState.Released)
                        return true;
                    break;
                case MouseButtons.RightButton:
                    if (this.PreviousState.RightButton == ButtonState.Pressed &&
                        this.CurrentState.RightButton == ButtonState.Released)
                        return true;
                    break;
            }

            return false;
        }

        public void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }
    }
}
