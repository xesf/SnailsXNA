using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens.Effects;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.Shades
{
    public class WaterShadeEffect : PostProcessor
    {
        #region Constants
        private const string WATER_SHADE_RES = "effects/water-waves";
        #endregion

        #region Members
        private RenderTarget2D _renderTarget;
        private int _renderTargetWidth;
        private int _renderTargetHeight;
        private float _elapsedTime;
        #endregion

        public WaterShadeEffect(Screen screen)
            : base(screen)
        {
        }

        public WaterShadeEffect(Screen screen, Effect effect, int width, int height)
            : base(screen, effect, width, height)
        {
        }

        public override void LoadContent()
        {
            _effect = BrainGame.ResourceManager.Load<Effect>(WATER_SHADE_RES, TwoBrainsGames.BrainEngine.Resources.ResourceManager.ResourceManagerCacheType.Static);
        }

        public override void  Initialize()
        {
 	        base.Initialize();

            _renderTargetWidth = _width;// / 2;
            _renderTargetHeight = _height;// / 2;

            _renderTarget = new RenderTarget2D(this._screen.SpriteBatch.GraphicsDevice,
                _renderTargetWidth, _renderTargetHeight, false,
                this._screen.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);
            
        }

        public Texture2D PerformWaterShade(Texture2D srcTexture, RenderTarget2D renderTarget, SpriteBatch spriteBatch)
        {
            Texture2D outputTexture = null;
            Rectangle destRect1 = new Rectangle(0, 0, renderTarget.Width, renderTarget.Height);

            // Perform under water shade

            this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            _effect.CurrentTechnique = _effect.Techniques["UnderWater"];
            _effect.Parameters["fTimer"].SetValue(_elapsedTime);
            _effect.Parameters["colorMapTexture"].SetValue(srcTexture);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, BrainGame.CurrentSampler, null, null, _effect);
            spriteBatch.Draw(srcTexture, destRect1, Color.White);
            spriteBatch.End();

            // Return the shaded texture
            this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget(null);
            BrainGame.Graphics.Viewport = BrainGame.Viewport;
            outputTexture = (Texture2D)renderTarget;

            return outputTexture;
        }

        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            _elapsedTime += (float)gameTime.ElapsedGameTime.Milliseconds / 500;
        }

        /*public override void Draw()
        {
            Texture2D result = PerformWaterShade(_texture, _renderTarget, this._screen.SpriteBatch);
            Rectangle rectangle = new Rectangle(0, 0, _width, _height);

            this._screen.SpriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0f, 0);

            this._screen.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            this._screen.SpriteBatch.Draw(result, rectangle, Color.White);
            this._screen.SpriteBatch.End();
        }*/

        public Texture2D Draw(Texture2D text)
        {
            _texture = text;

            Texture2D result = PerformWaterShade(_texture, _renderTarget, this._screen.SpriteBatch);
            /*Rectangle rectangle = new Rectangle(0, 0, _width, _height);

            BrainGame.Graphics.Viewport = BrainGame.Viewport;
            this._screen.SpriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0f, 0);

            Rectangle rc = new Rectangle(0, 0, 1280, 720);
            this._screen.SpriteBatch.Begin(SpriteSortMode.Immediate, null, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            this._screen.SpriteBatch.Draw(result, this._drawRect, Color.White);
            this._screen.SpriteBatch.End();*/

            return result;
        }
    }
}
