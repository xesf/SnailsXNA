using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Debugging;

namespace TwoBrainsGames.Snails.Debuging
{
    class DebugItem : DrawableGameComponent
    {
        string Caption { get; set; }
        public object Value { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Color Textcolor { get; set; }
        BrainPerformanceCounter PerfCounter{ get; set; }
        DebugInfo Owner {get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DebugItem(Game game, DebugInfo debugInfo, string caption, Vector2 position, SpriteFont font, Color color, object value) :
            base(game)
        {
            this.Caption = caption;
            this.Position = position;
            this.Font = font;
            this.Textcolor = color;
            if (value is BrainPerformanceCounter)
            {
                this.PerfCounter = (BrainPerformanceCounter)value;
            }
            else
            {
                this.Value = value;
            }
            this.Owner = debugInfo;
            if (this.PerfCounter != null)
            {
                game.Components.Add(this.PerfCounter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.PerfCounter != null)
            {
                this.Value = this.PerfCounter.Counter;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if (this.Owner.Visible == false)
            {
                return;
            }
            this.SpriteBatch.Begin();
            Color color = this.Textcolor;
            string text =  string.Format("{0,-12}: {1,7:0.00}", this.Caption, this.Value); 
            if (PerfCounter != null)
            { 
                    text = string.Format("{0,-12}: {1,7:0.00} ({2,7:0.00})", this.Caption, this.Value, this.PerfCounter.Average); 
                if (PerfCounter.AlarmOn)
                {
                    color = Color.Red;
                }
                if (PerfCounter.AverageOn)
                {
                }
            }
            
            this.SpriteBatch.DrawString(this.Font, text, this.Position + this.Owner.Position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            this.SpriteBatch.End();
        }
    }
}
