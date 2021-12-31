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
using TwoBrainsGames.BrainEngine.Input;

namespace TestSpriteBatchEffect
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D _logo;
        Texture2D _stagebkg;
        Texture2D _tools;

        BasicEffect _stageRenderEffect; // stage effect
        BasicEffect _hudRenderEffect;   // hud effect

        SamplerState _sampler2D;
        Vector2 posCam;

        KeyboardInput _keyboard;
#if !WINDOWS
        TouchInput _touch;
        Vector2 _startPos;
        Vector2 _deltaPos;
        bool _dragging;
#endif

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            this._sampler2D = new SamplerState();
            this._sampler2D.AddressU = TextureAddressMode.Clamp;
            this._sampler2D.AddressV = TextureAddressMode.Clamp;
            this._sampler2D.AddressW = TextureAddressMode.Clamp;
            this._sampler2D.Filter = TextureFilter.Anisotropic;

            this._hudRenderEffect = new BasicEffect(graphics.GraphicsDevice);
            this._hudRenderEffect.World = Matrix.Identity;
            this._hudRenderEffect.View = Matrix.Identity;
            // Offset the proj matrix by and half pixel or else the textures will blur
            this._hudRenderEffect.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateOrthographicOffCenter(0, 800, 480, 0, -1, 1);
            this._hudRenderEffect.TextureEnabled = true;
            this._hudRenderEffect.VertexColorEnabled = true;

            this._stageRenderEffect = new BasicEffect(graphics.GraphicsDevice);
            this._stageRenderEffect.TextureEnabled = true;
            this._stageRenderEffect.VertexColorEnabled = true;
            this._stageRenderEffect.World = Matrix.Identity;
            this._stageRenderEffect.View = Matrix.Identity;
            this._stageRenderEffect.Projection = this._hudRenderEffect.Projection;

            _keyboard = new KeyboardInput(this);
            this.Components.Add(_keyboard);
#if !WINDOWS
            _touch = new TouchInput(this);
            this.Components.Add(_touch);
#endif

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _logo = Content.Load<Texture2D>("logo");
            _stagebkg = Content.Load<Texture2D>("stagebkg");
            _tools = Content.Load<Texture2D>("tools");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        { }

        public Matrix UpdateTransform(Vector2 pos)
        {
            // Create the Transform used by any
            // spritebatch process
            return      Matrix.CreateTranslation(pos.X, pos.Y, 0) *
                        Matrix.CreateRotationZ(0) *
                        Matrix.CreateScale(new Vector3(1, 1, 1)) *
                        Matrix.CreateTranslation(0, 0, 0);
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

            // keyboard arrow keys pan
            if (_keyboard.IsKeyDown(Keys.Left))
            {
                posCam.X -= 10;
            }
            if (_keyboard.IsKeyDown(Keys.Right))
            {
                posCam.X += 10;
            }
            if (_keyboard.IsKeyDown(Keys.Up))
            {
                posCam.Y -= 10;
            }
            if (_keyboard.IsKeyDown(Keys.Down))
            {
                posCam.Y += 10;
            }

#if !WINDOWS
            // free dragging code
            if (_touch.StartDragging)
            {
                if (!_dragging)
                {
                    _startPos = _touch.GetTouchPosition();
                    _dragging = true;
                }
            }

            if (_dragging && _touch.CanGetPosition)
            {
                _deltaPos = _touch.GetTouchPosition() - _startPos;
                _startPos = _touch.GetTouchPosition();
                posCam -= _deltaPos;
            }
            else
            { _dragging = false; }

            if (_touch.EndDragging)
            {
                _dragging = false;
                _deltaPos = Vector2.Zero;
                _startPos = Vector2.Zero;
            }
#endif
            _stageRenderEffect.World = UpdateTransform(posCam);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Stage
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, _sampler2D, null, null, _stageRenderEffect);
            spriteBatch.Draw(_stagebkg, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            // HUD
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, _sampler2D, null, null, _hudRenderEffect);
            spriteBatch.Draw(_logo, new Vector2(0, 480 - _logo.Height * 0.4f), null, Color.White, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
            spriteBatch.Draw(_tools, new Vector2(800 - _tools.Width, 0), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
