using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace testRes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int HD_NATIVE_WIDTH = 1280;
        const int HD_NATIVE_HEIGHT = 720;

        const int DEVICE_WIDTH = 1024;
        const int DEVICE_HEIGHT = 768;

        GraphicsDeviceManager _graphicsManager;
        SpriteBatch spriteBatch;

        protected Viewport _viewport;
        protected float _viewportRatioX;
        protected float _viewportRatioY;
        protected Rectangle _screenRectangle;
        protected Rectangle _viewportRectangle;

        protected BasicEffect _renderEffect; // Needed because if the viewport does not match the screen resolution

        private Texture2D LOGO;
        private SamplerState _sampler2D;

        public Game1()
        {
            _graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            this._sampler2D = new SamplerState();
            this._sampler2D.AddressU = TextureAddressMode.Clamp;
            this._sampler2D.AddressV = TextureAddressMode.Clamp;
            this._sampler2D.AddressW = TextureAddressMode.Clamp;
            this._sampler2D.Filter = TextureFilter.Anisotropic;

            _graphicsManager.PreferredBackBufferWidth = DEVICE_WIDTH;
            _graphicsManager.PreferredBackBufferHeight = DEVICE_HEIGHT;
            _graphicsManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            _graphicsManager.IsFullScreen = false;           
            _graphicsManager.ApplyChanges();

            // set default view port
            SetViewport(_graphicsManager.GraphicsDevice.Viewport);

            // Correct the aspect ratio used with the mouse input
            _viewportRatioX = _viewportRatioY = 1f;
            if (_graphicsManager.GraphicsDevice.DisplayMode.Width < HD_NATIVE_WIDTH)
                _viewportRatioX = (float)_graphicsManager.GraphicsDevice.DisplayMode.Width / (float)HD_NATIVE_WIDTH;
            if (_graphicsManager.GraphicsDevice.DisplayMode.Height < HD_NATIVE_HEIGHT)
                _viewportRatioY = (float)_graphicsManager.GraphicsDevice.DisplayMode.Height / (float)HD_NATIVE_HEIGHT;

            this.SetupRenderViewport();
            this.SetupProjectionMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetViewport(Viewport viewport)
        {
            _viewport = new Viewport(viewport.Bounds);
            _graphicsManager.GraphicsDevice.Viewport = _viewport; // viewport;
            _graphicsManager.ApplyChanges();

            _viewportRectangle = new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height);
        }

        public void SetupRenderViewport()
        {
            // Start with our prefered viewport position and size
            int vpx = 0;
            int vpy = 0;
            int vpWidth = DEVICE_WIDTH;
            int vpHeight = DEVICE_HEIGHT;

            if (vpWidth > this._graphicsManager.GraphicsDevice.Viewport.Width)
            {
                vpWidth = this._graphicsManager.GraphicsDevice.Viewport.Width;
                // Scale vp height to the new width (this will keep the aspect ratio)
                vpHeight = DEVICE_HEIGHT * vpWidth / DEVICE_WIDTH;
            }

            if (vpHeight > this._graphicsManager.GraphicsDevice.Viewport.Height)
            {
                vpHeight = this._graphicsManager.GraphicsDevice.Viewport.Height;
                vpWidth = DEVICE_WIDTH * vpHeight / DEVICE_HEIGHT;
            }
            // Center on screen
            vpx = (this._graphicsManager.GraphicsDevice.Viewport.Width / 2) - (vpWidth / 2);
            vpy = (this._graphicsManager.GraphicsDevice.Viewport.Height / 2) - (vpHeight / 2);

            SetViewport(new Viewport(vpx, vpy, vpWidth, vpHeight));
            this._screenRectangle = new Rectangle(0, 0, DEVICE_WIDTH, DEVICE_HEIGHT);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetupProjectionMatrix()
        {
            this._renderEffect = new BasicEffect(_graphicsManager.GraphicsDevice);
            this._renderEffect.World = Matrix.Identity;
            this._renderEffect.View = Matrix.Identity;
            // Offset the proj matrix by and half pixel or else the textures will blur
            this._renderEffect.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateOrthographicOffCenter(0, DEVICE_WIDTH, DEVICE_HEIGHT, 0, -1, 1); ;
            this._renderEffect.TextureEnabled = true;
            this._renderEffect.VertexColorEnabled = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LOGO = Content.Load<Texture2D>("brains-logo");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, _sampler2D, null, null, _renderEffect);
            //Vector2 pos = new Vector2((DEVICE_WIDTH / 2) - LOGO.Width, (DEVICE_HEIGHT / 2) - LOGO.Height);
            Vector2 pos = new Vector2(0, 0);
            spriteBatch.Draw(LOGO, pos, Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
