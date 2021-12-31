
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TwoBrainsGames.Snails.StageEditor;
using TwoBrainsGames.Snails.Stages;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;
using TwoBrainsGames.Snails.StageEditor.Forms;
using TwoBrainsGames.BrainEngine.Collision;
using System.Drawing.Imaging;

namespace TwoBrainsGames.Snails.StageObjects
{
    public partial class StageObject
    {
        const int PROP_LINE_SPACE = 15;

        public bool Selected { get; set; }
        private ObjectBehaviour _objBehaviour;
        public ObjectBehaviour EditorBehaviour
        {
            get
            {
                if (this._objBehaviour == null)
                {
                    this._objBehaviour = Settings.GetObjectBehaviour(this.Id);
                }
                return this._objBehaviour;
            }

        }

        public Microsoft.Xna.Framework.Rectangle SelectionRectXna
        {
            get
            {
                Rectangle rc = this.SelectionRect;

                return new Microsoft.Xna.Framework.Rectangle(rc.Left, rc.Top, rc.Width, rc.Height);
            }
        }

        public Rectangle SelectionRect
        {
            get
            {
                Rectangle rc;
                Microsoft.Xna.Framework.Rectangle frameRc = this.Sprite.Frames[this.CurrentFrame].Rect;

                if (this is Liquid)
                {
                    frameRc = new Microsoft.Xna.Framework.Rectangle(0, 0, 60, 60); 
                    Liquid w = this as Liquid;
                    int px = (int)this.Position.X - (int)this.Sprite.OffsetX + (int)this.Sprite.OffsetX;
                    int py = (int)this.Position.Y - (int)this.Sprite.OffsetY + (int)this.Sprite.OffsetY;

                    return new Rectangle(px, py, frameRc.Width * (int)w.Size.X, frameRc.Height * (int)w.Size.Y);
                }

                switch ((int)this.Rotation)
                {
                    case 90:
                        rc = new Rectangle(
                                (int)this.Position.X + (int)this.Sprite.OffsetY - frameRc.Height,
                                (int)this.Position.Y - (int)this.Sprite.OffsetX,
                                frameRc.Height, frameRc.Width);
                        break;
                    case 180:
                        rc = new Rectangle(
                                (int)this.Position.X - (int)this.Sprite.OffsetX,
                                (int)this.Position.Y - frameRc.Height + (int)this.Sprite.OffsetY,
                                frameRc.Width, frameRc.Height);
                        break;
                    case 270:
                        rc = new Rectangle(
                                            (int)this.Position.X - (int)this.Sprite.OffsetY,
                                            (int)this.Position.Y - (int)this.Sprite.OffsetX,
                                            frameRc.Height, frameRc.Width);
                        break;
                    default: 
                        rc = new Rectangle(
                                (int)this.Position.X - (int)this.Sprite.OffsetX,
                                (int)this.Position.Y - (int)this.Sprite.OffsetY,
                                frameRc.Width, frameRc.Height);
                        break;
                  
                }

                return rc;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ObjectBehaviour.TilePlacement GetTilePlacementY()
        {
 
            if (Rotation == 0)
            {
                return this.EditorBehaviour.TilePlacementY;
            }

            if (Rotation == 90)
            {
                switch (this.EditorBehaviour.TilePlacementX)
                {
                    case ObjectBehaviour.TilePlacement.Center:
                        return ObjectBehaviour.TilePlacement.Center;
                    case ObjectBehaviour.TilePlacement.Arbitrary:
                        return ObjectBehaviour.TilePlacement.Arbitrary;


                }
            }
            else if (Rotation == 180)
            {
                switch (this.EditorBehaviour.TilePlacementY)
                {
                    case ObjectBehaviour.TilePlacement.Bottom:
                        return ObjectBehaviour.TilePlacement.Top;

                }
            }
            else if (Rotation == 270)
            {
                switch (this.EditorBehaviour.TilePlacementX)
                {
                    case ObjectBehaviour.TilePlacement.Center:
                        return ObjectBehaviour.TilePlacement.Center;
                    case ObjectBehaviour.TilePlacement.Arbitrary:
                        return ObjectBehaviour.TilePlacement.Arbitrary;

                }
            }
            return this.EditorBehaviour.TilePlacementY;
        }

        /// <summary>
        /// 
        /// </summary>
        private ObjectBehaviour.TilePlacement GetTilePlacementX()
        {

            if (Rotation == 0)
            {
                return this.EditorBehaviour.TilePlacementX;
            }

            if (Rotation == 90)
            {
                switch (this.EditorBehaviour.TilePlacementY)
                {
                    case ObjectBehaviour.TilePlacement.Bottom:
                        return ObjectBehaviour.TilePlacement.Left;

                }
            }
            else
            if (Rotation == 270)
            {
                switch (this.EditorBehaviour.TilePlacementY)
                {
                    case ObjectBehaviour.TilePlacement.Bottom:
                        return ObjectBehaviour.TilePlacement.Right;

                }
            }
            return this.EditorBehaviour.TilePlacementX;
        }

        /// <summary>
        /// 
        /// </summary>
        private Microsoft.Xna.Framework.Vector2 GetPixelOffset()
        {

            if (Rotation == 0)
            {
                return this.EditorBehaviour.PlacementOffset;
            }

            if (Rotation == 90)
            {
                return new Microsoft.Xna.Framework.Vector2(-this.EditorBehaviour.PlacementOffset.Y, this.EditorBehaviour.PlacementOffset.X);
            }

            if (Rotation == 180)
            {
                return new Microsoft.Xna.Framework.Vector2(-this.EditorBehaviour.PlacementOffset.X, -this.EditorBehaviour.PlacementOffset.Y);
            }

            if (Rotation == 270)
            {
                return new Microsoft.Xna.Framework.Vector2(this.EditorBehaviour.PlacementOffset.Y, -this.EditorBehaviour.PlacementOffset.X);
            }

            return Microsoft.Xna.Framework.Vector2.Zero;
        }

        /// <summary>
        /// Returns the position of an object in the board given a point
        /// The returned point takes into account the behaviour defined in the ObjectBehaviour
        /// Used in the stage editor to display the object at the correct position when the user clicks
        /// on the board to place the object, or when the object is drawed when the cursor moves
        /// </summary>
        public System.Drawing.Point GetPositionInBoardFromPoint(System.Drawing.Point position)
        {
            int rowx = (position.X / Stage.TILE_WIDTH);
            int rowy = (position.Y / Stage.TILE_HEIGHT);
            int x = 0, y = 0;
            
            this.UpdateBoundingBox();
            BoundingSquare bs = this.TransformToObjectSpace(new BoundingSquare(this.SelectionRectXna));
            ObjectBehaviour.TilePlacement yPlacement = this.GetTilePlacementY();
            ObjectBehaviour.TilePlacement xPlacement = this.GetTilePlacementX();
            Microsoft.Xna.Framework.Vector2 pixelOffset = this.GetPixelOffset();
            switch (yPlacement)
            {
                case  ObjectBehaviour.TilePlacement.TileSnap:
                    y = (int)((position.Y) / Stage.TILE_HEIGHT) * Stage.TILE_HEIGHT;
                    break;

                case ObjectBehaviour.TilePlacement.Bottom:
                    y = (int)((rowy * Stage.TILE_HEIGHT) + Stage.TILE_HEIGHT - bs.Height -bs.Top);
                    break;

                case ObjectBehaviour.TilePlacement.Top:
                    y = (int)((rowy * Stage.TILE_HEIGHT) - bs.Top);
                    break;

                case ObjectBehaviour.TilePlacement.Center:
                    y = (int)((rowy * Stage.TILE_HEIGHT) + (Stage.TILE_HEIGHT / 2) - (bs.Height / 2) - bs.Top);
                    break;

                case ObjectBehaviour.TilePlacement.Arbitrary:
                    y = position.Y;
                    break;
            }
            y += (int)pixelOffset.Y;
   
            switch (xPlacement)
            {
                case ObjectBehaviour.TilePlacement.TileSnap:
                    x = (int)((position.X) / Stage.TILE_WIDTH) * Stage.TILE_WIDTH;
                    break;

                case ObjectBehaviour.TilePlacement.Center:
                    x = (int)((rowx * Stage.TILE_WIDTH) + (Stage.TILE_WIDTH / 2) - (bs.Width / 2) - bs.Left);
                    break;

                case ObjectBehaviour.TilePlacement.Left:
                    x = (int)((rowx * Stage.TILE_WIDTH) - bs.Left);
                    break;

                case ObjectBehaviour.TilePlacement.Right:
                    x = (int)((rowx * Stage.TILE_WIDTH) + Stage.TILE_WIDTH - bs.Right);
                    break;

                case ObjectBehaviour.TilePlacement.Arbitrary:
                    x = position.X;
                    break;
            }

            x += (int)pixelOffset.X;

            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Microsoft.Xna.Framework.Vector2 GetPositionInBoardFromPoint(Microsoft.Xna.Framework.Vector2 position)
        {
            Point pt = new Point((int)position.X, (int)position.Y);

            Point pt1 = this.GetPositionInBoardFromPoint(pt);

            return new Microsoft.Xna.Framework.Vector2(pt1.X, pt1.Y);
        }
        

        /// <summary>
        /// 
        /// </summary>
        public void Draw(Graphics graphics, int offsetx, int offsety, bool cursorDragging)
        {
            ImageAttributes ia = new ImageAttributes();

            if (this.BlendColor != Microsoft.Xna.Framework.Color.White)
            {
                Microsoft.Xna.Framework.Vector4 v4 = this.BlendColor.ToVector4();
                ColorMatrix m = new ColorMatrix(new float[][]{ new float[] {v4.X,  0,  0,  0, 0},       
                                                                   new float[] {0,  v4.Y,  0,  0, 0},        
                                                                   new float[] {0,  0,  v4.Z,  0, 0},        
                                                                   new float[] {0,  0,  0,  v4.W, 0},       
                                                                   new float[] {0f, 0f, 0f, 0f, 1} });

                ia.SetColorMatrix(m);
            }

            int px = (int)this.Position.X - (int)this.Sprite.OffsetX + offsetx;
            int py = (int)this.Position.Y - (int)this.Sprite.OffsetY + offsety; 

            Microsoft.Xna.Framework.Rectangle frameRect = this.Sprite.Frames[this.CurrentFrame].Rect;
            Rectangle rc = new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
            
            switch ((int)this.Rotation)
            {
                case 90:
                case 180:
                case 270: // What follows is a big mess. Rotation in GDI+ is made using 3 points
   
                Rectangle rcSel = this.SelectionRect;

                    Point[] points = new Point[3];
                    if (this.Rotation == 90)
                    {
                        /*
                        points[0] = new Point(px + rc.Height, py);
                        points[1] = new Point(px + rc.Height, py + rc.Width);
                        points[2] = new Point(px, py);*/

                        points[0] = new Point(rcSel.Left + rc.Height, rcSel.Top);
                        points[1] = new Point(rcSel.Left + rc.Height, rcSel.Top + rc.Width - 1); // I don't get this fk, this crashes in some cases, -1 seems to solve
                        points[2] = new Point(rcSel.Left, rcSel.Top);
                    }
                    else if (this.Rotation == 180)
                    {
                      /*  points[0] = new Point(px + rc.Width, py + rc.Height);
                        points[1] = new Point(px, py + rc.Height);
                        points[2] = new Point(px + rc.Width -1, py); // If I take -1 this crashes, strange...*/

                        points[0] = new Point(rcSel.Left + rc.Width, rcSel.Top + rc.Height);
                        points[1] = new Point(rcSel.Left, rcSel.Top + rc.Height);
                        points[2] = new Point(rcSel.Left + rc.Width - 1, rcSel.Top);
                    }
                    else if (this.Rotation == 270)
                    {
                       /* points[0] = new Point(px, py + rc.Width);
                        points[1] = new Point(px, py);
                        points[2] = new Point(px + rc.Height, py + rc.Width);*/
                        points[0] = new Point(rcSel.Left, rcSel.Top + rc.Width);
                        points[1] = new Point(rcSel.Left, rcSel.Top);
                        points[2] = new Point(rcSel.Left + rc.Height, rcSel.Top + rc.Width -1);
                    }
                 //   try
                   // {
                      graphics.DrawImage(this.Sprite.Image, points, rc, GraphicsUnit.Pixel, ia);
//                    }
  //                  catch (System.Exception ex)
    //                {
                       // string s = ex.Message;
      //              }
                    break;

                default:
                    Point [] pts = new Point[3];
                    pts[0] = new Point(px, py);
                    pts[1] = new Point(px + rc.Width, py);
                    pts[2] = new Point(px, py + rc.Height);
                    graphics.DrawImage(this.Sprite.Image, pts, rc, GraphicsUnit.Pixel, ia);
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawSelectionRect(Graphics graphics, int offsetx, int offsety, bool cursorDragging)
        {

            if (this.Selected)
            {


                if (!cursorDragging || (cursorDragging && this.EditorBehaviour.ShowSelectionRect))
                {
                    Rectangle rc = new Rectangle(this.SelectionRect.X + offsetx, this.SelectionRect.Y + offsety,
                                        this.SelectionRect.Width, this.SelectionRect.Height);
                    graphics.DrawRectangle(Settings.ObjectSelectionPenBack, rc);
                    graphics.DrawRectangle(Settings.ObjectSelectionPen, rc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawWater(Graphics graphics, int offsetx, int offsety, bool cursorDragging, int scaleX, int scaleY, Color color)
        {
            int px = (int)this.Position.X - (int)this.Sprite.OffsetX + offsetx;
            int py = (int)this.Position.Y - (int)this.Sprite.OffsetY + offsety;

            Microsoft.Xna.Framework.Rectangle frameRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 60, 60); // this.Sprite.Frames[this.CurrentFrame].Rect;
            Rectangle rc = new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
            Rectangle rcDest = new Rectangle(px, py, frameRect.Width * scaleX, frameRect.Height * scaleY);
            //Rectangle rcDest = new Rectangle(px, py, frameRect.Width, frameRect.Height);

            //graphics.DrawImage(this.Sprite.Image, rcDest, rc, GraphicsUnit.Pixel);
            graphics.FillRectangle(new SolidBrush(color), rcDest);

            if (this.Selected)
            {
                if (!cursorDragging || (cursorDragging && this.EditorBehaviour.ShowSelectionRect))
                {
                    rc = new Rectangle(px, py, frameRect.Width * scaleX, frameRect.Height * scaleY);
                    rc = new Rectangle(this.SelectionRect.X + offsetx, this.SelectionRect.Y + offsety,
                                        this.SelectionRect.Width, this.SelectionRect.Height);
                    graphics.DrawRectangle(Settings.ObjectSelectionPenBack, rc);
                    graphics.DrawRectangle(Settings.ObjectSelectionPen, rc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Rectangle ComputePropBoxSize(Graphics graphics)
        {
          
            List<ObjectBehaviour.ObjectProperty> propsToShow = this.GetObjectsPropToShow();
            int width = 0, height = 0;
            foreach (ObjectBehaviour.ObjectProperty prop in propsToShow)
            {
                SizeF size = graphics.MeasureString(this.FormatPropertyCaptionValue(prop), Settings.ObjectPropsFont);
                if (size.Width > width)
                {
                    width = (int)size.Width;
                }
                height += PROP_LINE_SPACE;
            }
           
            return new Rectangle(0, 0, width + 5, height);
        }

        /// <summary>
        /// 
        /// </summary>
        private string GetPropertyValueAsString(string propName)
        {
            if (this.GetType().GetProperty(propName) == null)
            {
                throw new ApplicationException("Property with name '" + propName + "' not found in StageObject");
            }
            object val = this.GetType().GetProperty(propName).GetValue(this, null);

            if (val == null)
            {
                return null;
            }
            return val.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private string FormatPropertyCaptionValue(ObjectBehaviour.ObjectProperty prop)
        {
            return string.Format(prop.FormatText, this.GetPropertyValueAsString(prop.Name));
        }

        /// <summary>
        /// 
        /// </summary>
        private List<ObjectBehaviour.ObjectProperty> GetObjectsPropToShow()
        {
            
            List<ObjectBehaviour.ObjectProperty> props = new List<ObjectBehaviour.ObjectProperty>();
            if (this.EditorBehaviour != null)
            {
                foreach (ObjectBehaviour.ObjectProperty prop in this.EditorBehaviour.PropertiesToShow)
                {
                    if ((prop.ShowWhenUnselected && this.Selected == false) ||
                        (prop.ShowWhenSelected && this.Selected == true))
                    {
                        props.Add(prop);
                    }
                }
            }
            return props;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void DrawObjectProps(Graphics graphics, int px, int py)
        {
            List<ObjectBehaviour.ObjectProperty> propsToShow = this.GetObjectsPropToShow();
            px += (int)EditorBehaviour.PropertiesDrawOffset.X;
            py += (int)EditorBehaviour.PropertiesDrawOffset.Y;
            Rectangle rc = this.ComputePropBoxSize(graphics);
            rc.Location = new Point(px, py);
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Black)), rc);
            foreach (ObjectBehaviour.ObjectProperty prop in propsToShow)
            {
                graphics.DrawString(this.FormatPropertyCaptionValue(prop), Settings.ObjectPropsFont, Settings.ObjectPropsColor, new PointF(px, py));
                py += PROP_LINE_SPACE;
           }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void EditProperties(IWin32Window owner, EditorStage currentStage)
        {

            if (this.EditorBehaviour != null &&
                !string.IsNullOrEmpty(this.EditorBehaviour.PropertiesForm.FormTypeName))
            {
                System.Runtime.Remoting.ObjectHandle handle = Activator.CreateInstance(System.Reflection.Assembly.GetExecutingAssembly().FullName, this.EditorBehaviour.PropertiesForm.FormTypeName);
                ObjectPropsBaseForm form = (ObjectPropsBaseForm)handle.Unwrap();
                form.ShowDialog(owner, currentStage, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Bitmap RotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(returnBitmap);
            //move rotation point to center of image
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            //draw passed in image onto graphics object
            g.DrawImage(b, new Point(0, 0));
            return returnBitmap;
        }
    }
}
