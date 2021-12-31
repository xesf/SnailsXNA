using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Tutorials
{
    class TutorialText : TutorialItem
    {
        string _text;
        public TextFont _font;
        public Color _color;

        public TutorialText(string text, Color color)
        {
	        this._text = text;
		    this._displayTime = text.Length * 40;
            this._color = color;
        }

	    /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            this._font = this._parentTopic._font;
        }

	    /// <summary>
        /// 
        /// </summary>
        public override void Draw(Vector2 topicTopLefPosition, Color color, SpriteBatch spriteBatch)
        {
            // Color blending? Blend text color with the blend color, used to create fade in/out effects
            if (color != Color.White)
            {
                Vector4 vClr = (color.ToVector4() * this._color.ToVector4());
                color = new Color(vClr);
            }
            else
            {
                color = this._color;
            }
            this._font.DrawString(spriteBatch, this._text, topicTopLefPosition + this.Position, this._parentTopic._scale, color);  
        }

 
	    /// <summary>
        /// 
        /// </summary>
        public override float GetWidth()
        {
            return this._font.MeasureString(this._text, this._parentTopic._scale);
        }

        /// <summary>
        /// 
        /// </summary>
        public float GetHeight()
        {
            return this._font.MeasureStringHeight(this._text, this._parentTopic._scale);
        }
        
    }
}
