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
    /// GaussianBlur effect in Gray color tones
    /// </summary>
    public class GaussianBlurGray : GaussianBlur
    {
        #region Constants
        private const string GAUSSIAN_BLUR_RES = "effects/gaussian-blur-gray";
        #endregion

        public GaussianBlurGray(Screen screen)
            : base(screen)
        { }

        public GaussianBlurGray(Screen screen, Effect effect, int width, int height)
            : base(screen, effect, width, height)
        { }

        public override void LoadContent()
        {
            _effect = BrainGame.ResourceManager.Load<Effect>(GAUSSIAN_BLUR_RES, TwoBrainsGames.BrainEngine.Resources.ResourceManager.ResourceManagerCacheType.Static);
            InitRenderTargets();
        }
    }
}
