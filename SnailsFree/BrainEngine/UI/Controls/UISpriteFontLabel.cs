using System;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using System.IO;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UISpriteFontLabel : UILabel
    {
        #region Properties
        public SpriteFont Font { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UISpriteFontLabel(UIScreen screenOwner) :
            this(screenOwner, (SpriteFont)null, "")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public UISpriteFontLabel(UIScreen screenOwner, SpriteFont font) :
            this(screenOwner, font, "")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public UISpriteFontLabel(UIScreen screenOwner, SpriteFont font, string text) :
            base(screenOwner)
        {
            this.Initialize(font, text);
        }

        /// <summary>
        /// 
        /// </summary>
        public UISpriteFontLabel(UIScreen screenOwner, string fontResourceName, string text) :
            base(screenOwner)
        {
            this.Initialize(BrainGame.ResourceManager.Load<SpriteFont>(fontResourceName), text);

        }

        /// <summary>
        /// 
        /// </summary>
        private void Initialize(SpriteFont font, string text)
        {
            this.Autosize = true;
            this.Font = font;
            this.Text = text;
            this.CalculateSize();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            if (this.Font != null)
            {
                Vector2 pos = this.AbsolutePositionInPixels;
                pos = new Vector2((int)pos.X, (int)pos.Y);
                if (this.DropShadow)
                {
                    this.SpriteBatch.DrawString(this.Font, this.Text, pos + this.ShadowDistance, this.ShadowColor, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1.0f);
                }
                this.SpriteBatch.DrawString(this.Font, this.Text, pos, this.BlendColor, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1.0f);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void CalculateSize()
        {
            if (this.Font != null && this.Text != null)
            {
                Vector2 textSize = this.Font.MeasureString(this.Text);
                Size size = new Size((int)textSize.X, (int)textSize.Y);
                this.Size = this.PixelsToScreenUnits(size);
            }
        }
    }
}
