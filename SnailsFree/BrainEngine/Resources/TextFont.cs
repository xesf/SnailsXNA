using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;

namespace TwoBrainsGames.BrainEngine.Resources
{
    public class TextFont : Image, IDataFileSerializable
    {
        public delegate void DrawCharCallback(SpriteBatch spriteBatch, Vector2 pos, TextFontChar charToPrint, Color color, object param, Vector2 scale);
        #region Constants

        public enum TextHorizontalAlign
        {
            Left,
            Right,
            Center
        }

        public enum TextVerticalAlign
        {
            Top,
            Bottom,
            Middle
        }

        #endregion

        #region Members

        private TextFontChar[] Chars { get; set; }
        private int SpaceWidth { get; set; }
        private int CharSpacing { get; set; }
        public int LineHeight { get; private set; }
        private string ImageId { get; set; }
#if FORMS_SUPPORT
        public System.Drawing.Image Image { get; private set; }
#endif
        public Color Opacity { get; set; }
        #endregion

        public TextFont()
        {
            Opacity = Color.White;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            this.Texture = contentManager.Load<Texture2D>(this.ImageId);
        }

        public override bool Release(ContentManager contentManager)
        {
            return base.Release(contentManager);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawChar(SpriteBatch spriteBatch, Vector2 position, TextFontChar charToPrint, Color color, object param, Vector2 scale)
        {
#if DEBUG
            if (BrainGame.Settings.ShowSprites)
            {
#endif
                Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, (int)(charToPrint.Rectangle.Width * scale.X), (int)(charToPrint.Rectangle.Height * scale.Y));
                spriteBatch.Draw(this.Texture, destRect, charToPrint.Rectangle, color, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
#if DEBUG
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position)
        {
            this.DrawStringWithCallback(spriteBatch, text, position, Color.White, this.DrawChar, null, new Vector2(1.0f, 1.0f));
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Vector2 scale)
        {
            this.DrawStringWithCallback(spriteBatch, text, position, Color.White, this.DrawChar, null, scale);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Vector2 scale, Color color)
        {
            this.DrawStringWithCallback(spriteBatch, text, position, color, this.DrawChar, null, scale);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawString(SpriteBatch spriteBatch, string text, Rectangle destRect, TextHorizontalAlign horizAlign)
        {
            this.DrawStringWithCallback(spriteBatch, text, destRect, horizAlign,  Color.White, this.DrawChar, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawStringWithCallback(SpriteBatch spriteBatch, string text, Rectangle destRect, TextHorizontalAlign horizAlign, Color color, DrawCharCallback drawCallback, object param)
        {
            Vector2 position = new Vector2(destRect.X, destRect.Y); // Defaults to top-left corner

            float width = this.MeasureString(text, new Vector2(1.0f, 1.0f));

            if (horizAlign == TextHorizontalAlign.Right)
            {
                position = new Vector2(destRect.X + destRect.Width - width, destRect.Y);
            }
            else
            if (horizAlign == TextHorizontalAlign.Center)
            {
                position = new Vector2(destRect.X + (destRect.Width / 2) - (width / 2), destRect.Y);
            }

            this.DrawStringWithCallback(spriteBatch, text, position, color, drawCallback, param, new Vector2(1.0f, 1.0f));
        }
      

         /// <summary>
        /// 
        /// </summary>
        public void DrawStringWithCallback(SpriteBatch spriteBatch, string text, Vector2 position, Color color, DrawCharCallback drawCallback, object param, Vector2 scale)
       {
           for (int i = 0; i < text.Length; i++)
           {
               if (text[i] == ' ')
               {
                   position.X += (this.SpaceWidth + this.CharSpacing) * scale.X;
                   continue;
               }

               TextFontChar charToPrint = this.Chars[text[i]];
               if (charToPrint == null)
                   continue;
              
               if (drawCallback != null)
               {
                   drawCallback(spriteBatch, new Vector2(position.X + charToPrint.Spacing, position.Y), charToPrint, color, param, scale);
               }
               position.X += (charToPrint.Rectangle.Width + this.CharSpacing + charToPrint.Spacing + charToPrint.SpacingAfter) * scale.X;
           }
       }

       /// <summary>
       /// A single method could be used to measure width / height of string. I just don't feel like doing it...
       /// </summary>
        public float MeasureStringHeight(string text, Vector2 scale)
       {
           float height = 0;

           for (int i = 0; i < text.Length; i++)
           {

               TextFontChar charToPrint = this.Chars[text[i]];
               if (charToPrint == null)
                   continue;

               if (charToPrint.Rectangle.Height > height)
                   height = charToPrint.Rectangle.Height;
           }

           return height;
       }
         /// <summary>
        /// 
        /// </summary>
        public float MeasureString(string text)
        {
            return this.MeasureString(text, new Vector2(1f, 1f));
        }

        /// <summary>
        /// 
        /// </summary>
        public float MeasureString(string text, Vector2 scale)
        {
            float witdh = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    witdh += (this.SpaceWidth + this.CharSpacing);
                    continue;
                }

#if DEBUG
                if (this.Chars.Length <= text[i])
                {
                    throw new BrainException("Char does not exit in the char array.");
                }
#else
                if (this.Chars.Length <= text[i])
                {
                  continue;
                }

#endif
                TextFontChar charToPrint = this.Chars[text[i]];
                if (charToPrint == null)
                    continue;

                witdh += charToPrint.Rectangle.Width;
                if (i + 1 < text.Length)
                    witdh += this.CharSpacing + charToPrint.Spacing + charToPrint.SpacingAfter;
            }

            return witdh;
        }

        public static TextFont FromDataFileRecord(DataFileRecord record)
        {
            TextFont font = new TextFont();
            font.InitFromDataFileRecord(record);
            return font;
        }
#if FORMS_SUPPORT
        /// <summary>
        /// 
        /// </summary>
        public void UpdateImage()
        {
            this.Image = this.ToImage();
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Drawing.Image ToImage()
        {
            byte[] textureData = new byte[4 * this.Texture.Width * this.Texture.Height];
            this.Texture.GetData<byte>(textureData);
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(this.Texture.Width, this.Texture.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, this.Texture.Width, this.Texture.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            IntPtr safePtr = bmpData.Scan0; System.Runtime.InteropServices.Marshal.Copy(textureData, 0, safePtr, textureData.Length);
            bmp.UnlockBits(bmpData);
            return (System.Drawing.Image)bmp;
        }
#endif

        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            this.Chars = new TextFontChar[256];
            this.ImageId = record.GetFieldValue<string>("ImageId");

            DataFileRecord charsRecord = record.SelectRecord("Characters");
            if (charsRecord != null)
            {
                this.SpaceWidth = charsRecord.GetFieldValue<int>("SpaceWidth", 0);
                this.CharSpacing = charsRecord.GetFieldValue<int>("BetweenCharsWidth", 0);
                this.LineHeight = charsRecord.GetFieldValue<int>("LineHeight", 0);
                DataFileRecordList charRecords = charsRecord.SelectRecords("Character");
                foreach (DataFileRecord chRecord in charRecords)
                {
                    TextFontChar ch = TextFontChar.FromDataFile(chRecord);
                    this.Chars[ch.Character] = ch;
                }
            }
        }

        public override DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("Font");
            record.AddField("ImageId", this.ImageId);

            DataFileRecord charsRecord = new DataFileRecord("Characters");
            charsRecord.AddField("SpaceWidth", this.SpaceWidth);
            charsRecord.AddField("BetweenCharsWidth", this.CharSpacing);
            charsRecord.AddField("LineHeight", this.LineHeight);
            record.AddRecord(charsRecord);

            foreach (TextFontChar ch in this.Chars)
            {
                if (ch != null)
                {
                    charsRecord.AddRecord(ch.ToDataFileRecord());
                }
            }

            return record;
        }

        #endregion
    }
}
