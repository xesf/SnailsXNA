using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace TwoBrainsGames.BrainEngine.UI
{
    public class Cursor
    {
        public Vector2 Position { get ; set; }
   
        public virtual bool Visible { get; set; }

        public Cursor()
        { }

        public virtual void SetCursor(int id)
        { }

        public virtual void LoadCursor(string resourceName, int id)
        { }

        public virtual void LoadCursor(Assembly assembly, string resourceName, int id)
        { }

        public virtual void Update(BrainGameTime gameTime)
        {
            if (BrainGame.Settings.UseTouch)
            {
                this.Position = BrainGame.ScreenNavigator.InputController.MotionPosition;
            }
            if (BrainGame.Settings.UseMouse)
            {
                // Don't update cursor position from motion position if the current screen mode is SnapToControl
                if (BrainGame.ScreenNavigator.ActiveScreen is UIScreen &&
                    ((UIScreen)BrainGame.ScreenNavigator.ActiveScreen).CursorMode != CursorModes.SnapToControl)
                {
                    this.Position = BrainGame.ScreenNavigator.InputController.MotionPosition;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        { }
    }
}
