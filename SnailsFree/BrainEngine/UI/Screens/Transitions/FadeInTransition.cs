using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.UI.Screens.Transitions
{
    public class FadeInTransition : Transition
    {
    
        SpriteBatch _spriteBatch;
        float _alpha;
        float _alphaMin = 0.0f;
        float _alphaMax = 1.0f;
        float _speed;
        Color _fadeColor;

        public float AlphaMin
        {
            get { return _alphaMin; }
            set { _alphaMin = value; }
        }

        public float AlphaMax
        {
            get { return _alphaMax; }
            set { _alphaMax = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FadeInTransition(float fadeSpeed) :
            this(Color.Black, fadeSpeed)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public FadeInTransition(Color fadeColor, float fadeSpeed)
        {
            this._speed = fadeSpeed;
            this._fadeColor = fadeColor;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._spriteBatch = new SpriteBatch(BrainGame.Graphics);
            this._alpha = _alphaMax;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this._ended)
            {
                return;
            }
            this._alpha -= this._speed * (float)gameTime.ElapsedRealTime.TotalMilliseconds;
            if (this._alpha < _alphaMin)
            {
                this._alpha = _alphaMin;
                this._ended = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            Color fadeColor;
            fadeColor = new Color(this._fadeColor.R, this._fadeColor.G, this._fadeColor.B, this._alpha);
            this._spriteBatch.Begin(SpriteSortMode.Immediate, null, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            BrainGame.DrawRectangleFilled(this._spriteBatch, new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight), fadeColor);
            this._spriteBatch.End();
        }  
    }
}
