using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    class FastForwardStrips : IBrainComponent
    {
        const int VERTICAL_STRIP_COUNT = 5;
        const float STRIPS_SPEED = 0.05f;

        private Sprite _ffSprite;
        private Rectangle [] VerticalStripRects;
        private float [] VerticalStripX;
        private ColorEffect _blink;
        public bool Visible { get; set; }

        public FastForwardStrips()
        {
        }

        #region IBrainComponent Members
        public Microsoft.Xna.Framework.Graphics.SpriteBatch SpriteBatch
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
  
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this._ffSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD/FastForward");

            // All this crap should go to Initialize, bu unfortunately initialize runs BEFORE LoadContent...
            this.VerticalStripRects = new Rectangle[VERTICAL_STRIP_COUNT];
            this.VerticalStripX = new float[VERTICAL_STRIP_COUNT];

            float x = 0;
            for (int i = 0; i < this.VerticalStripRects.Length; i++)
            {
                this.VerticalStripX[i] = x;
                this.VerticalStripRects[i] = new Rectangle((int)this.VerticalStripX[i], 0, this._ffSprite.Frames[0].Width, (int)Stage.CurrentStage.StageHUD._stageArea.Height);
                x += (Stage.CurrentStage.StageHUD._stageArea.Width / VERTICAL_STRIP_COUNT);
            }

            this._blink = new ColorEffect(new Color(1f, 1f, 1f, 0.4f), new Color(0.7f, 0.7f, 0.7f, 0.2f), 0.1f, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            if (this.Visible)
            {
                this._blink.Update(gameTime);
                for (int i = 0; i < this.VerticalStripRects.Length; i++)
                {
                    this.VerticalStripX[i] += (STRIPS_SPEED * (float)gameTime.ElapsedRealTime.Milliseconds);
                    this.VerticalStripRects[i].X = (int)this.VerticalStripX[i];
                    if (this.VerticalStripX[i] > Stage.CurrentStage.StageHUD._stageArea.Right)
                    {
                        this.VerticalStripX[i] = -10;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Visible)
            {
                for (int i = 0; i < this.VerticalStripRects.Length; i++)
                {
                    this._ffSprite.Draw(this._ffSprite.Frames[0].Rect, this.VerticalStripRects[i], this._blink.Color, spriteBatch);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnloadContent()
        {
        }

        #endregion
    }
}
