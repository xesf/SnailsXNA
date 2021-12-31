using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace TwoBrainsGames.Snails.Stages
{
    public partial class TileCell
    {
        public bool Selected { get; set; }

        public bool Contains(Point pt)
        {
            Rectangle rc = new Rectangle(this.BoardX * Stage.TILE_WIDTH,
                                         this.BoardY * Stage.TILE_HEIGHT,
                                         Stage.TILE_WIDTH, Stage.TILE_HEIGHT);

            return rc.Contains(pt);

        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(Graphics graphics, int offsetX, int offsetY, Pen penSelection, Pen penBlack)
        {
#if STAGE_EDITOR
            int px = (this.BoardX * Stage.TILE_WIDTH) + offsetX;
            int py = (this.BoardY * Stage.TILE_HEIGHT) + offsetY;
            
            // Tile
            Rectangle rcTile = new Rectangle();
            if (this.Tile != null)
            {
                ImageAttributes ia = new ImageAttributes();
                if (this.Tile.BlendColor != Microsoft.Xna.Framework.Color.White)
                {
                    Microsoft.Xna.Framework.Vector4 v4 = this.Tile.BlendColor.ToVector4();
                    ColorMatrix m = new ColorMatrix(new float[][]{ new float[] {v4.X,  0,  0,  0, 0},       
                                                                   new float[] {0,  v4.Y,  0,  0, 0},        
                                                                   new float[] {0,  0,  v4.Z,  0, 0},        
                                                                   new float[] {0,  0,  0,  v4.W, 0},       
                                                                   new float[] {0f, 0f, 0f, 0f, 1} });
                                                                 
                    ia.SetColorMatrix(m);
                }

                Image bmp = this.Tile.Sprite.Image;
                int frame = this.Tile.CurrentFrame;
                Microsoft.Xna.Framework.Rectangle frameRect = this.Tile.Sprite.Frames[frame].Rect;
                rcTile = new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
                Point [] pts = new Point[3];
                pts[0] = new Point(px, py);
                pts[1] = new Point(px + rcTile.Width, py);
                pts[2] = new Point(px, py + rcTile.Height);
                graphics.DrawImage(bmp, pts, rcTile, GraphicsUnit.Pixel, ia);
                //graphics.DrawImage(bmp, px, py, rcTile, GraphicsUnit.Pixel);
            }

            // Selection
            if (this.Selected)
            {
               
                if (this.Tile != null)
                {
                    rcTile = new Rectangle(px,
                                       py,
                                       rcTile.Width,
                                       rcTile.Height);
                    graphics.DrawRectangle(penBlack, rcTile);
                    graphics.DrawRectangle(penSelection, rcTile);
                }
            }
#endif
        }
    }
}
