using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;

namespace TwoBrainsGames.Snails.Stages
{
    public class LightManager
    {

        List<LightSource> _lightSources;
        RenderTarget2D _lightMapRenderTarget;
        BlendState _blendState;
        BlendState _lightTintblendState;


        public bool LightEnabled { get; set; }
        public Color LightColor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public LightManager()
        {
            this._lightSources = new List<LightSource>();
            this.LightEnabled = false;
            this.LightColor = new Color(0f,0f,0f,1f);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this._lightMapRenderTarget = new RenderTarget2D(BrainGame.Graphics, BrainGame.Viewport.Width,
                                                                                BrainGame.Viewport.Height);
            
            // Create a custom blend state
            // This blend state will open "holes" in the black texture mask
            this._blendState = new BlendState();
            this._blendState.AlphaBlendFunction = BlendFunction.ReverseSubtract;
            this._blendState.AlphaSourceBlend = Blend.DestinationAlpha;
            this._blendState.AlphaDestinationBlend = Blend.One;
            this._blendState.ColorBlendFunction = this._blendState.AlphaBlendFunction;
            this._blendState.ColorDestinationBlend = this._blendState.AlphaDestinationBlend;
            this._blendState.ColorSourceBlend = this._blendState.AlphaSourceBlend;
            this._blendState.BlendFactor = Color.White;
            this._blendState.MultiSampleMask = -1;

            this._lightTintblendState = new BlendState();
            this._lightTintblendState.AlphaBlendFunction = BlendFunction.Add;
            this._lightTintblendState.AlphaSourceBlend = Blend.One;
            this._lightTintblendState.AlphaDestinationBlend = Blend.Zero;
            this._lightTintblendState.ColorBlendFunction = this._lightTintblendState.AlphaBlendFunction;
            this._lightTintblendState.ColorDestinationBlend = this._lightTintblendState.AlphaDestinationBlend;
            this._lightTintblendState.ColorSourceBlend = this._lightTintblendState.AlphaSourceBlend;
            this._lightTintblendState.BlendFactor = Color.White;
            this._lightTintblendState.MultiSampleMask = -1;

        }

        /// <summary>
        /// 
        /// </summary>
        public void Unload()
        {
            if (this._lightMapRenderTarget != null)
            {
                this._lightMapRenderTarget.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddLightSource(LightSource light)
        {
            this._lightSources.Add(light);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveLightSource(LightSource light)
        {
            this._lightSources.Remove(light);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            if (this.LightEnabled == false)
            {
                return;
            }
            foreach (LightSource source in this._lightSources)
            {
                if (source.IsOn)
                {
                    source.Update(gameTime);
                }
            }
            this.CreateMap();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            if (this.LightEnabled == false)
            {
                return;
            }

            // Tint the lights
            BrainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, Stage.CurrentStage.Camera.StageRenderEffect); 
            foreach (LightSource source in this._lightSources)
            {
                if (source.IsOn)
                {
                    source.DrawTint(BrainGame.SpriteBatch);
                }
            }
            BrainGame.SpriteBatch.End();
            
            // Draw the light map
            BrainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            BrainGame.SpriteBatch.Draw(this._lightMapRenderTarget, Vector2.Zero, Color.White);
            BrainGame.SpriteBatch.End();
        }

        /// <summary>
        /// Creates the light texture that will be drawed over the stage
        /// </summary>
        public void CreateMap()
        {
            BrainGame.Graphics.SetRenderTarget(this._lightMapRenderTarget);
            BrainGame.Graphics.Clear(this.LightColor);

            if (this.LightColor.A == 0)
            {
				BrainGame.Graphics.SetRenderTarget(null);
                BrainGame.Graphics.Viewport = BrainGame.Viewport;
                return;
            }

            BrainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, this._blendState, null, null, null, Stage.CurrentStage.Camera.StageRenderEffect); 
            foreach(LightSource source in this._lightSources)
            {
                if (source.IsOn)
                {
                    source.Draw(BrainGame.SpriteBatch);
                }
            }
            BrainGame.SpriteBatch.End(); 
           
            BrainGame.Graphics.SetRenderTarget(null);
            BrainGame.Graphics.Viewport = BrainGame.Viewport;
        }
    }
}
