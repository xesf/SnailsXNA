using System;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UITextFontLabel : UILabel
    {
        TextFont _font;
        public float _lineSpacing;
        public float _lineSpacingInPixels;
        #region Properties
        public TextFont Font
        {
            get { return this._font; }
            set
            {
                this._font = value;
                this.CalculateSize();
            }

        }
        private Vector2[] TextLinePositions { get; set; } // In pixels
        private float Spacing 
        { 
            get 
            {
                return (this._font == null ? 0f : this._font.LineHeight) + this._lineSpacingInPixels; 
            } 
        }

        public float LineSpacing
        {
            get { return this._lineSpacing; }
            set
            {
                if (this._lineSpacing != value)
                {
                    this._lineSpacing = value;
                    this._lineSpacingInPixels = this.ScreenUnitToPixelsY(this._lineSpacing);
                    this.CalculateSize();
                }
            }
        }


        public override Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                if (!this.Autosize)
                {
                    this.CalculateSize();
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UITextFontLabel(UIScreen screenOwner) :
            base(screenOwner)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public UITextFontLabel(UIScreen screenOwner, TextFont font) :
            this(screenOwner, font, "")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public UITextFontLabel(UIScreen screenOwner, TextFont font, string text) :
            base(screenOwner)
        {
            this.Font = font;
            this.Text = text;
            this.Autosize = true;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
        
            if (this.Font != null)
            {
                for (int i = 0; i < this.TextLines.Length; i++)
                {
                    if (this.DropShadow)
                    {
                        this.Font.DrawString(this.SpriteBatch, this.TextLines[i], this.AbsolutePositionInPixels + this.TextLinePositions[i], this.Scale, this.ShadowColor);
                    }
                    this.Font.DrawString(this.SpriteBatch, this.TextLines[i], this.AbsolutePositionInPixels + this.TextLinePositions[i], this.Scale, this.BlendColor);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CalculateSize()
        {
            if (this.Autosize)
            {
                this.Size = new Size(this.MeasureWidth(), this.MeasureHeight());
            }
            if (this.TextLines != null)
            {
                float y = 0;
                switch (this.VerticalAligment)
                {
                    case VerticalTextAligment.Top:
                        break;
                    case VerticalTextAligment.Bottom:
                        y = this.ScreenUnitToPixelsY(this.Size.Height) - this.SizeInPixels.Height;
                        break;
                    case VerticalTextAligment.Center:
                        y = (this.ScreenUnitToPixelsY(this.Size.Height) / 2) - (this.SizeInPixels.Height / 2);
                        break;
                }

                this.TextLinePositions = new Vector2[this.LineCount];
                for (int i = 0; i < this.TextLines.Length; i++)
                {
                    float x = 0;
                    switch (this.HorizontalAligment)
                    {
                        case HorizontalTextAligment.Left:
                            break;
                        case HorizontalTextAligment.Right:
                            x = this.SizeInPixels.Width - (this.Font.MeasureString(this.TextLines[i], this.Scale));
                            break;
                        case HorizontalTextAligment.Center:
                            x = (this.SizeInPixels.Width / 2) - (this.Font.MeasureString(this.TextLines[i], this.Scale) / 2);
                            break;
                    }

                   
                    this.TextLinePositions[i] = new Vector2(x, y);
                    y += this.Spacing;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private float MeasureWidth()
        {
            if (this.Font == null)
            {
                return 0f;
            }
            float width = 0;
            foreach (string line in this.TextLines)
            {
                float lineWidth = this.Font.MeasureString(line, this.Scale);
                if (lineWidth > width)
                {
                    width = lineWidth;
                }
            }

            return this.PixelsToScreenUnitsX(width);
        }



        /// <summary>
        /// 
        /// </summary>
        private float MeasureHeight()
        {
            if (this.Font == null)
            {
                return 0f;
            }

            float height = 0f;

            for (int i = 0; i < this.TextLines.Length; i++)
            {
                height += this.Font.MeasureStringHeight(this.TextLines[i], this.Scale);
                if (i != this.TextLines.Length - 1)
                {
                    height += this.Spacing; 
                }
            }

            return this.PixelsToScreenUnitsY(height);
        }
    }
}
