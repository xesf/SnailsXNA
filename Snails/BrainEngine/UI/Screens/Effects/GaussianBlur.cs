using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.BrainEngine.UI.Screens.Effects
{
    /// <summary>
    /// Direction of Gaussian Blur: Horizontal or Vertical
    /// </summary>
    public enum GaussianBlurDirection
    {
        Horizontal,
        Vertical
    };

    /// <summary>
    /// GaussianBlur effect
    /// </summary>
    public class GaussianBlur : PostProcessor
    {
        #region Constants
        private const string GAUSSIAN_BLUR_RES = "effects/gaussian-blur";
        private const int RADIUS = 7; // WARNING: must be the same radius configured in the pixel shade effect
        #endregion

        #region Members
        private int _radius;
        private float _amount;
        private float _sigma;
        private float[] _kernel;
        private Vector2[] offsetsHoriz;
        private Vector2[] offsetsVert;
        private RenderTarget2D renderTarget1;
        private RenderTarget2D renderTarget2;
        private int renderTargetWidth;
        private int renderTargetHeight;
        #endregion

        public GaussianBlur(Screen screen)
            : base(screen)
        {
            ComputeKernel(RADIUS, 0.5f);
        }

        public GaussianBlur(Screen screen, Effect effect, int width, int height)
            : base(screen, effect, width, height)
        {
            ComputeKernel(RADIUS, 0.5f);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _effect = BrainGame.ResourceManager.Load<Effect>(GAUSSIAN_BLUR_RES, TwoBrainsGames.BrainEngine.Resources.ResourceManager.ResourceManagerCacheType.Static);
            InitRenderTargets();
        }

        protected void InitRenderTargets()
        {
            renderTargetWidth = _width / 2; //_texture.Width / 2;
            renderTargetHeight = _height / 2; //_texture.Height / 2;

            renderTarget1 = new RenderTarget2D(this._screen.SpriteBatch.GraphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                this._screen.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            renderTarget2 = new RenderTarget2D(this._screen.SpriteBatch.GraphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                this._screen.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            ComputeOffsets(renderTargetWidth, renderTargetHeight);
        }

        public void ComputeKernel(int blurRadius, float blurAmount)
        {
            _radius = blurRadius;
            _amount = blurAmount;

            _kernel = null;
            _kernel = new float[_radius * 2 + 1];
            _sigma = _radius / _amount;

            float twoSigmaSquare = 2.0f * _sigma * _sigma;
            float sigmaRoot = (float)Math.Sqrt(twoSigmaSquare * Math.PI);
            float total = 0.0f;
            float distance = 0.0f;
            int index = 0;

            for (int i = -_radius; i <= _radius; ++i)
            {
                distance = i * i;
                index = i + _radius;
                _kernel[index] = (float)Math.Exp(-distance / twoSigmaSquare) / sigmaRoot;
                total += _kernel[index];
            }

            for (int i = 0; i < _kernel.Length; ++i)
                _kernel[i] /= total;
        }

        public void ComputeOffsets(float textureWidth, float textureHeight)
        {
            offsetsHoriz = null;
            offsetsHoriz = new Vector2[_radius * 2 + 1];

            offsetsVert = null;
            offsetsVert = new Vector2[_radius * 2 + 1];

            int index = 0;
            float xOffset = 1.0f / textureWidth;
            float yOffset = 1.0f / textureHeight;

            for (int i = -_radius; i <= _radius; ++i)
            {
                index = i + _radius;
                offsetsHoriz[index] = new Vector2(i * xOffset, 0.0f);
                offsetsVert[index] = new Vector2(0.0f, i * yOffset);
            }
        }

        public Texture2D PerformGaussianBlur(Texture2D srcTexture, RenderTarget2D renderTarget1, RenderTarget2D renderTarget2, SpriteBatch spriteBatch)
        {
            Texture2D outputTexture = null;
            Rectangle destRect1 = new Rectangle(0, 0, renderTarget1.Width, renderTarget1.Height);
            Rectangle destRect2 = new Rectangle(0, 0, renderTarget2.Width, renderTarget2.Height);

            // Perform horizontal Gaussian blur.

            this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget1);
            _effect.CurrentTechnique = _effect.Techniques["GaussianBlur"];
            _effect.Parameters["weights"].SetValue(_kernel);
            _effect.Parameters["colorMapTexture"].SetValue(srcTexture);
            _effect.Parameters["offsets"].SetValue(offsetsHoriz);

            spriteBatch.Begin(0, BlendState.Opaque, BrainGame.CurrentSampler, null, null, _effect);
            if (srcTexture != null) // Sometimes this is null, there's a bug somewhere
            {
                spriteBatch.Draw(srcTexture, destRect1, Color.White);
            }
            spriteBatch.End();

            // Perform vertical Gaussian blur.

            this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget2);
            outputTexture = (Texture2D)renderTarget1;

            _effect.Parameters["colorMapTexture"].SetValue(outputTexture);
            _effect.Parameters["offsets"].SetValue(offsetsVert);

            spriteBatch.Begin(0, BlendState.Opaque, BrainGame.CurrentSampler, null, null, _effect);
            spriteBatch.Draw(outputTexture, destRect2, Color.White);
            spriteBatch.End();

            // Return the Gaussian blurred texture.

            this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget(null);
            BrainGame.Graphics.Viewport = BrainGame.Viewport;
            outputTexture = (Texture2D)renderTarget2;

            return outputTexture;
        }

        /// <summary>
        /// Applies Gaussian Blur to frame buffer
        /// </summary>
        public override void Draw()
        {
            Texture2D result = PerformGaussianBlur(_texture, renderTarget1, renderTarget2, this._screen.SpriteBatch);
            Rectangle rectangle = new Rectangle(0, 0, _width, _height);

            this._screen.SpriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0f, 0);

            this._screen.SpriteBatch.Begin(SpriteSortMode.Immediate, null, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            this._screen.SpriteBatch.Draw(result, rectangle, Color.White);
            this._screen.SpriteBatch.End();
        }

        public Texture2D Draw(Texture2D text)
        {
            _texture = text;

            Texture2D result = PerformGaussianBlur(_texture, renderTarget1, renderTarget2, this._screen.SpriteBatch);

            BrainGame.Graphics.Viewport = BrainGame.Viewport;
            this._screen.SpriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0f, 0);

            this._screen.SpriteBatch.Begin(SpriteSortMode.Immediate, null, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            this._screen.SpriteBatch.Draw(result, this._drawRect, Color.White);
            this._screen.SpriteBatch.End();

            return result;
        }
    }
}
