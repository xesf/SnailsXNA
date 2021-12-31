using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;
using LevelEditor.Controls;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;
using System.Drawing.Imaging;

namespace TwoBrainsGames.Snails.StageEditor
{
    public class BoardCursor
    {
        public bool HighLightCells { get; set; }
        public Sprite Sprite { get; set; }
        public int FrameIdx { get; set; }
        public bool Visible { get; set; }

        Pen HiLightedCellPen { get; set; }
        Pen HiLightedCellPenBack { get; set; }

        BoardCtl _parentBoard;
        Microsoft.Xna.Framework.Color _blendColor;

        public BoardCursor(BoardCtl board)
        {
            this.HiLightedCellPen = new Pen(Brushes.Green);
            this.HiLightedCellPen.Width = 3.0F;
            this.HiLightedCellPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            this.HiLightedCellPenBack = new Pen(Brushes.White);
            this.HiLightedCellPenBack.Width = 3.0F;

            this._parentBoard = board;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Set(ITileToolboxItem tile)
        {
            this.Sprite = tile.Sprite;
            this.FrameIdx = tile.SpriteFrameNr;
            this.HighLightCells = true;
            this.Visible = true;
            this._blendColor = tile.Tile.BlendColor;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Set(StageObject obj)
        {
            this.Sprite = obj.Sprite;
            this.FrameIdx = 0;
            this.HighLightCells = true;
            this.Visible = true;
            this._blendColor = obj.BlendColor;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(Graphics graphics, int x, int y)
        {
#if STAGE_EDITOR
            if (this.Sprite == null)
            {
                return;
            }

            ImageAttributes ia = new ImageAttributes();

            if (this._blendColor != Microsoft.Xna.Framework.Color.White)
            {
                Microsoft.Xna.Framework.Vector4 v4 = this._blendColor.ToVector4();
                ColorMatrix m = new ColorMatrix(new float[][]{ new float[] {v4.X,  0,  0,  0, 0},       
                                                                   new float[] {0,  v4.Y,  0,  0, 0},        
                                                                   new float[] {0,  0,  v4.Z,  0, 0},        
                                                                   new float[] {0,  0,  0,  v4.W, 0},       
                                                                   new float[] {0f, 0f, 0f, 0f, 1} });

                ia.SetColorMatrix(m);
            }
/*
            x = (x / this._parentBoard.Stage.TileSize.Width) * this._parentBoard.Stage.TileSize.Width;
            y = (y / this._parentBoard.Stage.TileSize.Height) * this._parentBoard.Stage.TileSize.Height;*/
            x -= (int)this.Sprite.OffsetX;
            y -= (int)this.Sprite.OffsetY;
            
            Microsoft.Xna.Framework.Rectangle frameRect = this.Sprite.Frames[FrameIdx].Rect;
            Rectangle rc = new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
            Point[] pts = new Point[3];
            pts[0] = new Point(x, y);
            pts[1] = new Point(x + rc.Width, y);
            pts[2] = new Point(x, y + rc.Height);
            graphics.DrawImage(this.Sprite.Image, pts, rc, GraphicsUnit.Pixel, ia);

            if (this.HighLightCells)
            {
                Rectangle rect = new Rectangle(x,
                                               y,
                                               frameRect.Width,
                                               frameRect.Height);
                graphics.DrawRectangle(this.HiLightedCellPenBack, rect);
                graphics.DrawRectangle(this.HiLightedCellPen, rect);
            }
#endif
        }
    }
}
