using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.BrainEngine.Graphics
{
    public class SpriteAnimationQueue
    {
        public delegate void AnimationEndedHandler();
        public event AnimationEndedHandler OnAnimationEnded;

        private bool _loop;
        private int _currentItemIndex;
        private List<SpriteAnimationQueueItem> _items;
        private List<SpriteAnimationQueueItem> _activeItems;


        public bool Active { get; private set;}
    
        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimationQueue(bool loop, bool active)
        {
            this._items = new List<SpriteAnimationQueueItem>();
            this._activeItems = new List<SpriteAnimationQueueItem>();
            this._loop = loop;
            this.Active = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddItem(SpriteAnimationQueueItem item)
        {
            this._items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimationQueueItem AddItem(SpriteAnimation animation, Vector2 position, bool drawFirstFrame, bool wait)
        {
            return this.AddItem(animation, position, drawFirstFrame, wait, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimationQueueItem AddItem(SpriteAnimation animation, Vector2 position, bool drawFirstFrame, bool wait, string soundResource, Object2D autidoEmmiter)
        {
            return this.AddItem(animation, position, drawFirstFrame, wait, soundResource, autidoEmmiter, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimationQueueItem AddItem(SpriteAnimation animation, Vector2 position, bool drawFirstFrame, bool wait, SpriteAnimationQueueItem.LastFrameCallBackDelegate lastFrameCallback)
        {
            return this.AddItem(animation, position, drawFirstFrame, wait, null, null, lastFrameCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimationQueueItem AddItem(SpriteAnimation animation, Vector2 position, bool drawFirstFrame, bool wait, string soundResource, Object2D autidoEmmiter, SpriteAnimationQueueItem.LastFrameCallBackDelegate lastFrameCallback)
        {
            SpriteAnimationQueueItem anim = new SpriteAnimationQueueItem(animation, position, drawFirstFrame, wait, lastFrameCallback);
            if (!string.IsNullOrEmpty(soundResource))
            {
                anim.Sound = BrainGame.ResourceManager.GetSampleTemporary(soundResource, autidoEmmiter);
            }
            this.AddItem(anim);
            return anim;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            this._activeItems.Clear();
            this._currentItemIndex = -1;
            if (this.Active)
            {
                this.ActivateNextItems();
            }
        }
         
        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            
            if (this.Active == false ||
                this._activeItems.Count == 0)
            {
                return;
            }

            for (int i = 0; i < this._activeItems.Count; i++)
            {
                this._activeItems[i].Update(gameTime);
                if (this._activeItems[i]._ended)
                {
                    this._activeItems[i].IsActive = false;
                    this._activeItems[i]._animation.CurrentFrame = 0;
                    this._activeItems.RemoveAt(i);
                    i--;
                }
            }

            if (this._activeItems.Count == 0)
            {
                if (this._currentItemIndex >= this._items.Count - 1)
                {
                    if (this.OnAnimationEnded != null)
                    {
                        this.OnAnimationEnded();
                    }
                    if (!this._loop)
                    {
                        this.Active = false;
                        this.Reset();
                        return;
                    }
                    this._currentItemIndex = -1;
                }
                this.ActivateNextItems();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(Vector2 position,  Color blendColor, SpriteBatch spriteBatch)
        {
            foreach (SpriteAnimationQueueItem item in this._items)
            {
                if (item.IsActive || item.DrawFirstFrame)
                {
                    item.Draw(position, blendColor, spriteBatch);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActivateNextItems()
        {
            this._currentItemIndex++;
            for (int i = this._currentItemIndex; i < this._items.Count; i++, this._currentItemIndex++)
            {
                this._items[i]._ended = false;
                this._items[i].IsActive = true;
                if (this._items[i].DrawFirstFrame) // Skip first frame if the animation is always displayed even when inactive
                {
                    this._items[i]._animation.CurrentFrame = 1;
                }
                this._activeItems.Add(this._items[i]);
                if (this._items[i].Sound != null)
                {
                    this._items[i].Sound.Play();
                }
                if (this._items[i]._wait)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Activate()
        {
            this.Active = true;
            this.ActivateNextItems();
        }
    }
}
