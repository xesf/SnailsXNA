using System;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.BrainEngine.Graphics
{
    public class SpriteAnimationQueueItem
    {
        public delegate void LastFrameCallBackDelegate();

        public LastFrameCallBackDelegate LastFrameCallback { get; set; }
        public SpriteAnimation _animation;
        public Vector2 _position;
        public bool _wait;
        public bool _ended;
        public bool DrawFirstFrame { get; set; }
        public bool IsActive { get; set; }
        public Sample Sound { get; set; }


        public SpriteAnimationQueueItem(SpriteAnimation animationSprite, Vector2 position, bool drawFirstFrame, bool wait) :
            this(animationSprite, position, drawFirstFrame, wait, null)
        {
          
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimationQueueItem(Sprite sprite, Vector2 position, bool wait) :
            this(new SpriteAnimation(sprite), position, false, wait)
        {
          
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimationQueueItem(SpriteAnimation animationSprite, Vector2 position, bool drawFirstFrame, bool wait, LastFrameCallBackDelegate lastFrameCallback)
        {
            this._position = position;
            this._wait = wait;
            this._animation = animationSprite;
            this._animation.OnLastFrame += this.OnLastFrame;
            this.LastFrameCallback = lastFrameCallback;
            this.DrawFirstFrame = drawFirstFrame;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            this._animation.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(Vector2 position, Color blendColor, SpriteBatch spriteBatch)
        {
            this._animation.Draw(position + this._position, blendColor, spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnLastFrame()
        {
            this._ended = true;
            if (this.LastFrameCallback != null)
            {
                this.LastFrameCallback();
            }
        }
    }

}
