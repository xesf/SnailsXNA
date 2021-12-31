using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails;

namespace TwoBrainsGames.Snails.Debuging
{

    public class DebugWindow : DrawableGameComponent
    {
        public const Keys KEY_SHOW_HIDE_HELP = Keys.F1;
        public const Keys KEY_SHOW_HIDE_SPRITES = Keys.F2;
        public const Keys KEY_SHOW_HIDE_TILES = Keys.T;
        public const Keys KEY_SHOW_HIDE_BOUNDINGBOXES = Keys.B;
        public const Keys KEY_SHOW_HIDE_PATHS = Keys.P;
        public const Keys KEY_SHOW_QUADTREE = Keys.Q;
        public const Keys KEY_STAGE_EDITOR = Keys.F11;
        public const Keys KEY_RELOAD_STAGE = Keys.R;
        public const Keys KEY_NEXT_STAGE = Keys.M;
        public const Keys KEY_PREV_STAGE = Keys.N;
        public const Keys KEY_SHOW_HIDE_DEBUG_INFO = Keys.D;
        public const Keys KEY_ENABLE_AVERAGES = Keys.A;
        public const Keys KEY_DEBUG_INFO_POSITION = Keys.I;
        public const Keys KEY_GENERATE_THUMBS = Keys.G;

        struct KeyHelpItem
        {
            public int _XboxControlFrameNr;
            public string _HelpText;
            public Keys _Key;

            public KeyHelpItem(Keys key, int xboxControlFrameNr, string helpText)
            {
                this._XboxControlFrameNr = xboxControlFrameNr;
                this._HelpText = helpText;
                this._Key = key;
            }
        };

        Rectangle _Rect;
        Color _BackColor;
        Vector2 _Position;
        SpriteFont _Font;
        Color _Textcolor;

        List<KeyHelpItem> _KeyList;

        /// <summary>
        /// 
        /// </summary>
        public DebugWindow(SnailsGame game) :
          base(game)
        {
        }

         /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            int marginX = (int)(SnailsGame.ScreenWidth * 0.1f);
            int marginY = (int)(SnailsGame.ScreenHeight * 0.1f);
            this._Rect = new Rectangle(marginX,
                                       marginY,
                                       SnailsGame.ScreenWidth - (marginX * 2),
                                       SnailsGame.ScreenHeight - (marginY * 2));
            this._Position = new Vector2(marginX, marginY);
            this._BackColor = new Color(0, 0, 100, 128);
            this._Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/dbgWindow", ResourceManager.ResourceManagerCacheType.Static);
            this._KeyList = new List<KeyHelpItem>();
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_SHOW_HIDE_HELP, 5, "Show / hide this help"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_SHOW_HIDE_SPRITES, 9, "Show / hide sprites"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_SHOW_HIDE_TILES, 8, "Show / hide tiles"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_SHOW_HIDE_PATHS, 11, "Show / hide paths"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_SHOW_QUADTREE, 10, "Show / hide quadtree"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_SHOW_HIDE_BOUNDINGBOXES, 0, "Show / hide bounding boxes"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_SHOW_HIDE_DEBUG_INFO, 1, "Show / hide debug info"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_ENABLE_AVERAGES, 3, "Enable / disable averages"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_RELOAD_STAGE, 2, "Reset stage"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_DEBUG_INFO_POSITION, 12, "Change Debug Info position"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_NEXT_STAGE, 14, "Go to next stage"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_PREV_STAGE, 15, "Go to previous stage"));
            this._KeyList.Add(new KeyHelpItem(DebugWindow.KEY_GENERATE_THUMBS, 0, "Generate Stage Thumbnails"));

            this._Textcolor = Color.Yellow;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (this.Visible == false)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            BrainGame.SpriteBatch.Begin();
            BrainGame.DrawRectangleFilled(BrainGame.SpriteBatch, this._Rect, this._BackColor);

            float x = 20.0f;
            float y = 20.0f;
            foreach (KeyHelpItem key in this._KeyList)
            {
                Vector2 position = new Vector2(x, y);
                if (SnailsGame.GameSettings.UseGamepad)
                {
//                    this._DebugSprite.Draw(this._Position + position, key._XboxControlFrameNr, this._SpriteBatch);
                }
                else
                {
                    BrainGame.SpriteBatch.DrawString(this._Font, string.Format("({0})", key._Key.ToString()),
                    this._Position + position + new Vector2(0.0f, 15.0f), this._Textcolor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
                BrainGame.SpriteBatch.DrawString(this._Font, key._HelpText,
                    this._Position + position + new Vector2(55.0f, 15.0f), this._Textcolor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                y += 50.0f;

                if (y + 80.0f > this._Rect.Height)
                {
                    y = 20.0f;
                    x += 300.0f;
                }
            }
            BrainGame.SpriteBatch.End();
        }
    }
}
