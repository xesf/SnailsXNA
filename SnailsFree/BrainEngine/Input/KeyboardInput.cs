using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Input
{
    public class KeyboardInput : GameComponent
    {
        KeyboardState PreviousState { get; set; }
        KeyboardState CurrentState{ get; set; }

        public bool IsControlDown
        {
            get
            {
                return (this.CurrentState.IsKeyDown(Keys.LeftControl) ||
                        this.CurrentState.IsKeyDown(Keys.RightControl));
            }
        }

        public bool IsShiftDown
        {
            get
            {
                return (this.CurrentState.IsKeyDown(Keys.LeftShift) ||
                        this.CurrentState.IsKeyDown(Keys.RightShift));
            }
        }

        public KeyboardInput() :
            base(BrainGame.Instance)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.CurrentState = Keyboard.GetState();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            this.PreviousState = this.CurrentState;
            this.CurrentState = Keyboard.GetState();
        }

        public Keys[] GetPressedKeys()
        {
            return this.CurrentState.GetPressedKeys();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsKeyDown(Keys key)
        {
            if (this.CurrentState.IsKeyDown(key))
                return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsKeyPressed(Keys key)
        {
            if (this.PreviousState.IsKeyUp(key))
            {
                if (this.CurrentState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsKeyReleased(Keys key)
        {
            if (this.PreviousState.IsKeyDown(key))
            {
                if (this.CurrentState.IsKeyUp(key))
                    return true;
            }

            return false;
        }
    }
}
