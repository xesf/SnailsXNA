using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace TwoBrainsGames.BrainEngine.Graphics
{
    /// <summary>
    ///  Used to create masks for rendering.
    ///  A texture is used to create the mask
    ///  
    ///  Some help from here: http://www.crappycoding.com/2010/08/texture-modification-using-render-targets-with-some-stencil-buffer-action/
    ///
    /// </summary>
    public class RenderMask
    {
        DepthStencilState _stencilBuffer;
        Sprite _sprite;
        public Vector2 Position { get; set; }
        AlphaTestEffect _alphaTestEffect;
        DepthStencilState _stencilAlways;
        SpriteBatch _spriteBatch;
        RenderTarget2D _renderTarget;
        
        public DepthStencilState State
        {
            get { return this._stencilBuffer; }
        }

        /// <summary>
        /// 
        /// </summary>
        public RenderMask()
        {
           

        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
         
            // Only draw pixels where the stencil is 0
            this._stencilBuffer = new DepthStencilState();
            this._stencilBuffer.StencilFunction = CompareFunction.Equal;
            this._stencilBuffer.StencilPass = StencilOperation.Keep;
            this._stencilBuffer.ReferenceStencil = 0;
            this._stencilBuffer.DepthBufferEnable = false;
            this._stencilBuffer.StencilEnable = true;
            this._spriteBatch = new SpriteBatch(BrainGame.Graphics);

            this._alphaTestEffect = new AlphaTestEffect(BrainGame.Graphics);
            this._alphaTestEffect.VertexColorEnabled = true;
            this._alphaTestEffect.DiffuseColor = Color.White.ToVector3();
            this._alphaTestEffect.AlphaFunction = CompareFunction.NotEqual;
            this._alphaTestEffect.World = Matrix.Identity;
            this._alphaTestEffect.View = Matrix.Identity;
            this._alphaTestEffect.ReferenceAlpha = 0;
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            this._alphaTestEffect.Projection = halfPixelOffset * BrainGame.RenderEffect.Projection;

            // set up stencil state to always replace stencil buffer with 1
            this._stencilAlways = new DepthStencilState();
            this._stencilAlways.StencilEnable = true;
            this._stencilAlways.StencilFunction = CompareFunction.Always;
            this._stencilAlways.StencilPass = StencilOperation.Replace;
            this._stencilAlways.ReferenceStencil = 0;
            this._stencilAlways.DepthBufferEnable = false;

            this._renderTarget = new RenderTarget2D(BrainGame.Graphics, BrainGame.ScreenWidth, BrainGame.ScreenHeight,
                                                        false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8,
                                                        0, RenderTargetUsage.DiscardContents);
        }

        /// <summary>
        /// Begin drawing to the mask
        /// </summary>
        public void BeginDraw()
        {
            BrainGame.Graphics.SetRenderTarget(this._renderTarget);
            this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        }

        /// <summary>
        /// End drawing to the mask , reset default target render
        /// </summary>
        public void EndDraw()
        {
            this._spriteBatch.End();
            BrainGame.Graphics.SetRenderTarget(null);
            BrainGame.Graphics.Clear(ClearOptions.Stencil, new Color(0, 0, 0, 1), 0, 0);
            this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, this._stencilAlways, null, this._alphaTestEffect);
            this._spriteBatch.Draw(this._renderTarget, Vector2.Zero, Color.White);
            this._spriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawSprite(Sprite sprite, Vector2 position)
        {
            sprite.Draw(position, this._spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        public static RenderMask CreateFromSprite(string resourceName)
        {
            RenderMask mask = new RenderMask();
            mask._sprite = BrainGame.ResourceManager.GetSpriteTemporary(resourceName);

            // Save image for testing
          /*  using (FileStream f = new FileStream("c:\\temp\\teste.png", FileMode.Create))
            {
                mask._renderTarget.SaveAsPng(f, sprite.Width, sprite.Height);
            }*/

            mask._alphaTestEffect = new AlphaTestEffect(BrainGame.Graphics);
            mask._alphaTestEffect.VertexColorEnabled = true;
            mask._alphaTestEffect.DiffuseColor = Color.White.ToVector3();
            mask._alphaTestEffect.AlphaFunction = CompareFunction.NotEqual;
            mask._alphaTestEffect.World = Matrix.Identity;
            mask._alphaTestEffect.View = Matrix.Identity;
            mask._alphaTestEffect.ReferenceAlpha = 0;
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            mask._alphaTestEffect.Projection = halfPixelOffset * BrainGame.RenderEffect.Projection;

            // set up stencil state to always replace stencil buffer with 1
            mask._stencilAlways = new DepthStencilState();
            mask._stencilAlways.StencilEnable = true;
            mask._stencilAlways.StencilFunction = CompareFunction.Always;
            mask._stencilAlways.StencilPass = StencilOperation.Replace;
            mask._stencilAlways.ReferenceStencil = 0;
            mask._stencilAlways.DepthBufferEnable = false;

            return mask;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Render()
        {
            BrainGame.Graphics.Clear(ClearOptions.Stencil, new Color(0, 0, 0, 1), 0, 1);
            this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, BrainGame.CurrentSampler, this._stencilAlways, null, this._alphaTestEffect);
            this._sprite.Draw(this.Position, this._spriteBatch);
            this._spriteBatch.End();
        }
    }
}
