using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.BrainEngine.UI.Screens.Effects
{
    /// <summary>
    /// PostProcessor effect
    /// </summary>
    public class PostProcessor : IBrainComponent
    {
        #region Members
        protected Screen _screen;
        protected Texture2D _texture; // Texture input used to create the effect
        protected Effect _effect;     // The post processor fx file
        protected int _width;         // Width of the render target
        protected int _height;        // Height of the render target
        protected Rectangle _drawRect;
        public Rectangle Rectangle;
        #endregion

        #region Properties
      
        public SpriteBatch SpriteBatch
        {
            get { return this._screen.SpriteBatch; }
        }

        public Texture2D Texture
        {
            get { return this._texture; }
            set
            {
                this._texture = value;
                if (this._effect.Parameters["colorMapTexture"] != null)
                {
                    this._effect.Parameters["colorMapTexture"].SetValue(value);
                }
            }
        }

        public Effect Effect
        {
            get { return this._effect; }
            set { this._effect = value; }
        }
        #endregion

        public PostProcessor(Screen screen)
        {
            _screen = screen;
            InitializeEffect(null, _screen.SpriteBatch.GraphicsDevice.Viewport.Width, _screen.SpriteBatch.GraphicsDevice.Viewport.Height);
        }

        public PostProcessor(Screen screen, Effect effect, int width, int height)
        {
            _screen = screen;
            InitializeEffect(effect, width, height);
        }

        public void InitializeEffect(Effect effect, int width, int height)
        {
            this._effect = effect;
            _texture = new Texture2D(_screen.SpriteBatch.GraphicsDevice, 1, 1);
            _texture.SetData<Color>(new Color[] { Color.White });
            this._width = width;
            this._height = height;
            this.Rectangle = new Rectangle(0, 0, width, height);
            this._drawRect = new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight);
        }

        public virtual void Draw()
        { }

        public virtual void Initialize()
        { }

        public virtual void LoadContent()
        { }

        public virtual void Update(BrainGameTime gameTime)
        { }

        public virtual void UnloadContent()
        { }
    }
}
