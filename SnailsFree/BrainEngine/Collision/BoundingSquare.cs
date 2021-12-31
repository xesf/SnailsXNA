using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Collision
{
    // Class for axis aligned bounding boxes
    // and AABB may be defined by a simple rectangle. This type of BB cannot be rotated
    public struct BoundingSquare : IDataFileSerializable
    {

        public Vector2 UpperLeft;
        public Vector2 LowerRight;
        public Vector2 LowerLeft;
        public Vector2 UpperRight;
        public BoundingBox Rectangle;

        #region Properties
        public float Width { get { return (this.LowerRight.X - this.LowerLeft.X); } }
        public float Height { get { return (this.LowerRight.Y - this.UpperLeft.Y); } }
         public float Left
        {
            get { return this.UpperLeft.X; }
        }
        public float Right
        {
            get { return this.LowerRight.X; }
        }
        public float Top
        {
            get { return this.UpperLeft.Y; }
        }
        public float Bottom
        {
            get { return this.LowerRight.Y; }
        }

        public Vector2 Center
        {
            get { return this.UpperLeft + new Vector2(this.Width / 2, this.Height / 2); }
        }
        #endregion

        public BoundingSquare(Vector2 ul, Vector2 lr)
        {
            this.UpperLeft = ul;
            this.LowerRight = lr;
            this.LowerLeft = new Vector2(ul.X, lr.Y);
            this.UpperRight = new Vector2(lr.X, ul.Y);
            this.Rectangle = new BoundingBox();
            this.UpdateRect();
        }
       
        public BoundingSquare(Rectangle rect)
            : this(new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height))
        {
        }

        public BoundingSquare(Vector2 ul, float width, float height)
            : this(ul, new Vector2(ul.X + width, ul.Y + height))
        {
        }

        public Rectangle ToRect()
        {
            return new Rectangle((int)this.UpperLeft.X, (int)this.UpperLeft.Y, (int)this.Width, (int)this.Height);
        }

        public void Draw(Color color, Vector2 camPos)
        {
            BrainGame.DrawLine(this.UpperLeft - camPos, this.UpperRight - camPos, color);
            BrainGame.DrawLine(this.UpperRight - camPos, this.LowerRight - camPos, color);
            BrainGame.DrawLine(this.LowerRight - camPos, this.LowerLeft - camPos, color);
            BrainGame.DrawLine(this.LowerLeft - camPos, this.UpperLeft - camPos, color);
        }

        public void Draw(SpriteBatch spriteBatch, Color color, Vector2 camPos)
        {
            Rectangle rect = ToRect();
            rect.X -= (int)camPos.X;
            rect.Y -= (int)camPos.Y;

            BrainGame.DrawRectangleFrame(spriteBatch, rect, color, 1);
        }

        // Applies the transformations to the points
        public OOBoundingBox Transform(Matrix mat)
        {
            return new OOBoundingBox(Vector2.Transform(this.UpperLeft, mat),
                                     Vector2.Transform(this.UpperRight, mat),
                                     Vector2.Transform(this.LowerRight, mat),
                                     Vector2.Transform(this.LowerLeft, mat));
        }

        /// <summary>
        /// Transforms the bounding box points with to the specified position and rotation
        /// Returns the transformed bounding box
        /// </summary>
        public OOBoundingBox Transform(Vector2 position, float rotation)
        {
            Matrix matRot = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
            Vector3 pos = new Vector3(position.X, position.Y, 0);
            Matrix matTrans = Matrix.CreateTranslation(pos);
            Matrix matFinal = matTrans * matRot;

            return this.Transform(matFinal);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TransformInPlace(Vector2 translation)
        {
            this.UpperLeft += translation;
            this.UpperRight += translation;
            this.LowerLeft += translation;
            this.LowerRight += translation;
            this.UpdateRect();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateRect()
        {
            Vector3 p1 = new Vector3(this.UpperLeft.X, this.UpperLeft.Y, 0.0f);
            Vector3 p2 = new Vector3(this.LowerRight.X, this.LowerRight.Y, 0.0f);
            this.Rectangle = new BoundingBox(p1, p2);
        }
        /// <summary>
        /// Transforms the bounding box points with to the specified position and rotation
        /// Returns the transformed bounding box
        /// </summary>
        public OOBoundingBox Transform(float rotation, Vector2 position)
        {
            if (rotation != 0)
            {
                Matrix matRot = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
                Vector3 pos = new Vector3(position.X, position.Y, 0);
                Matrix matTrans = Matrix.CreateTranslation(pos);
                Matrix matFinal = matRot * matTrans;

                return this.Transform(matFinal);
            }

            return this.Transform(Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0)));
        }

        /// <summary>
        /// Returns the circle that fits inside the square
        /// </summary>
        public BoundingCircle GetContainedCircle()
        {
            if (this.Height < this.Width)
            {
                return new BoundingCircle(this.Center, (this.Height / 2));
            }
            return new BoundingCircle(this.Center, (this.Width / 2));
        }
        /// <summary>
        /// Translates the BB to a new location
        /// Because there's no rotation we can return a BoundindSquare instead of a OOBoundingBox
        /// </summary>
        public BoundingSquare Transform(Vector2 position)
        {
          return new BoundingSquare(this.UpperLeft + position, this.Width, this.Height);
        }

        // returns true if the specified point is inside the bbox
        public bool Contains(Vector2 point)
        {
            if (point.X < this.UpperLeft.X)
                return false;
            if (point.X > this.LowerRight.X)
                return false;
            if (point.Y < this.UpperLeft.Y)
                return false;
            if (point.Y > this.LowerRight.Y)
                return false;

            return true;
        }


        // returns true if the specified point is inside the bbox
        public bool Contains(BoundingSquare bs)
        {
            return (this.Rectangle.Contains(bs.Rectangle) == ContainmentType.Contains);
        }

        // returns true if the specified point is inside the bbox
        public bool Intersects(BoundingSquare bs)
        {
            return (this.Rectangle.Intersects(bs.Rectangle));
        }

        // Checks for collisions with a OOBB
        // There's a collision if any of the vertices that define the ooBbox is inside on the AABB
        public bool Collides(OOBoundingBox ooBbox)
        {
            if (this.Contains(ooBbox.P0))
                return true;
            if (this.Contains(ooBbox.P1))
                return true;
            if (this.Contains(ooBbox.P2))
                return true;
            if (this.Contains(ooBbox.P3))
                return true;

            return false;
        }

        // Checks for collisions with a AABB
        public bool Collides(BoundingSquare bbox)
        {
            return (this.Rectangle.Intersects(bbox.Rectangle));
        }

        public static BoundingSquare FromDataFileRecord(DataFileRecord record)
        {
            BoundingSquare box = new BoundingSquare();
            box.InitFromDataFileRecord(record);
            return box;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IntersectsLine(Vector2 lineP0, Vector2 lineP1,
                                   out Vector2 nearestColliddingPoint)
        {
            nearestColliddingPoint = Vector2.Zero;
            Vector2 intersetionPoint;
            float nearestLength = 999999;
            float length = -1;

            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.UpperLeft, this.UpperRight, out intersetionPoint))
            {
                length = nearestLength = (lineP0 - intersetionPoint).Length();
                nearestColliddingPoint = intersetionPoint;
            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.UpperRight, this.LowerRight, out intersetionPoint))
            {
                length = (lineP0 - intersetionPoint).Length();
                if (length < nearestLength)
                {
                    nearestLength = length;
                    nearestColliddingPoint = intersetionPoint;
                }
           
            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.LowerRight, this.LowerLeft, out intersetionPoint))
            {
                length = (lineP0 - intersetionPoint).Length();
                if (length < nearestLength)
                {
                    nearestLength = length;
                    nearestColliddingPoint = intersetionPoint;
                }
            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.LowerLeft, this.UpperLeft, out intersetionPoint))
            {
                length = (lineP0 - intersetionPoint).Length();
                if (length < nearestLength)
                {
                    nearestLength = length;
                    nearestColliddingPoint = intersetionPoint;
                }
            }

            return (length != -1);

        }

        #region IDataFileSerializable Members

        public void InitFromDataFileRecord(DataFileRecord record)
        {
            float left = record.GetFieldValue<float>("Left", this.UpperLeft.X);
            float top = record.GetFieldValue<float>("Top", this.UpperLeft.Y);
            float width = record.GetFieldValue<float>("Width", this.Width);
            float height = record.GetFieldValue<float>("Height", this.Height);

            this.UpperLeft = new Vector2(left, top);
            this.LowerRight = new Vector2(left + width, top + height);
            this.UpperRight = new Vector2(left + width, top);
            this.LowerLeft = new Vector2(left, top + height);
            this.UpdateRect();
        }


		/// <summary>
		/// 
		/// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("BoundingBox");

            record.AddField("Left", this.UpperLeft.X);
            record.AddField("Top", this.UpperLeft.Y);
            record.AddField("Width", this.Width);
            record.AddField("Height", this.Height);

            return record;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0} {1}", 
                this.UpperLeft.ToString(), this.LowerRight.ToString());
        }

		/// <summary>
		/// 
		/// </summary>
		public static BoundingSquare CreateFromDataFileRecord(DataFileRecord record)
		{
			BoundingSquare bs = new BoundingSquare();
			bs.InitFromDataFileRecord(record);
			return bs;
		}
    }
}
