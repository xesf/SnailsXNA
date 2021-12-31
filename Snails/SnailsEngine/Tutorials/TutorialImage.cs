using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Tutorials
{
    class TutorialImage : TutorialItem
    {
        string _imageResource;
        Sprite _sprite;

        public TutorialImage(string imageResource)
        {
           this._imageResource = imageResource;
        }

	    /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
  	        this._sprite = BrainGame.ResourceManager.GetSpriteTemporary(this._imageResource);
           this._displayTime = 1000;
        }


	    /// <summary>
        /// 
        /// </summary>
        public override void Draw(Vector2 topicTopLefPosition, Color color, SpriteBatch spriteBatch)
        {
            this._sprite.Draw(topicTopLefPosition + this.Position, 0, 0f, Vector2.Zero, this._parentTopic._scale.X, this._parentTopic._scale.Y, color, spriteBatch);
        }

	    /// <summary>
        /// 
        /// </summary>
        public override float GetWidth()
        {
          return this._sprite.Width;
        }
    }
}
