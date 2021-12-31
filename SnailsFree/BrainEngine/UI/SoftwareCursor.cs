using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Graphics;

namespace TwoBrainsGames.BrainEngine.UI
{
    public class SoftwareCursor : Cursor
    {
        protected Sprite _cursorSprite;
        protected Dictionary<int, Sprite> _cursorSprites;

        public SoftwareCursor()
        {
            this._cursorSprites = new Dictionary<int, Sprite>();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetCursor(int id)
        {
#if DEBUG

            if (!this.IsCursorLoaded(id))
            {
                throw new BrainException("Cursor not loaded [" + id + "]");
            }
#endif
            this._cursorSprite = this._cursorSprites[id];
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadCursor(string resourceName, int id)
        {
            if (!this.IsCursorLoaded(id))
            {
                string resource = BrainPath.GetDirectoryName(resourceName);
                string sprite = BrainPath.GetFileName(resourceName);
                this._cursorSprites.Add(id, BrainGame.ResourceManager.GetSpriteStatic(resource, sprite));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw cursor
            if (this.Visible && this._cursorSprite != null)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
                this._cursorSprite.Draw(this.Position, 0, spriteBatch);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool IsCursorLoaded(int name)
        {
            return (this._cursorSprites.ContainsKey(name));
        }

    }
}
