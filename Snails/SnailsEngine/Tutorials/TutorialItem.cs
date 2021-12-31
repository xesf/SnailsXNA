using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.Tutorials
{
    public class TutorialItem
    {
        public float _displayTime; // Total time in miliseconds the item will be displayed
        public TutorialTopic _parentTopic;
        public Vector2 Position { get; set; }

        public TutorialItem()
	    {
	    }

	    /// <summary>
        /// 
        /// </summary>
        public virtual void LoadContent()
        {
        }

	    /// <summary>
        /// 
        /// </summary>
        public virtual void Draw(Vector2 topicTopLefPosition, Color color, SpriteBatch spriteBatch)
        {
        }

        
	    /// <summary>
        /// 
        /// </summary>
        public virtual float GetWidth()
        {
            return 0f;
        }

    }
}
