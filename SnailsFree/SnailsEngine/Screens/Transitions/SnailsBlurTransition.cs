using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.Transitions
{
    class SnailsBlurTransition : BlurTransition, ISnailsPauseTransition
    {
        private const int BLUR_ITERATIONS = 6;
        private const double BLUR_ITERATION_TIME = 10;

        public SnailsBlurTransition(UIScreen screen, bool isGray) :
            base(screen, isGray, BLUR_ITERATIONS, BLUR_ITERATION_TIME)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.DrawToRenderTarget();
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.TextureToBlur = null;
            if (Stage.CurrentStage != null)
            {
             //   Stage.CurrentStage.RenderTarget = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RestoreBlur()
        {
            this.TextureToBlur = null;
            if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.DrawToRenderTarget();
            }
            this.DrawStageToTexture();
            this.PerformBlur();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
          
            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawStageToTexture()
        {
            if (Stage.CurrentStage != null)
            {
                this.TextureToBlur = Stage.CurrentStage.RenderTarget as Texture2D;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            this.ScreenOwner.EndDraw(); // because UIScreens already Begin it



            if (this.TextureToBlur == null)
            {
                if (!this.Ended)
                {
                    this.DrawStageToTexture();
                }
                else
                {
                    this.RestoreBlur();
                }
            }
            else
            {
                // Check if blur texture was lost, if so, rebuild it
                // Don't needed if the bluring has not ended
                if (this.Ended &&
                    this.RenderTarget != null &&
                    this.RenderTarget.IsContentLost &&
                    BrainGame.IsGameActive)
                {
                    this.RestoreBlur();
                }
            }

            base.Draw();
            // because UIScreens is waiting a Begin
            this.ScreenOwner.BeginDraw(BlendState.AlphaBlend);
        }
    }
}
