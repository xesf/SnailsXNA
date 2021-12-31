using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Collision
{
    public class BoundingCircle : IDataFileSerializable
    {
        public Vector2 _center;
        public float _radius;
        public BoundingCircle()
        {

        }

        public BoundingCircle(Vector2 center, float radius)
        {
            this._center = center;
            this._radius = radius;
        }

        /// <summary>
        /// 
        /// </summary>
        public BoundingCircle Transform(Vector2 pos)
        {
            return new BoundingCircle(this._center + pos, this._radius);
        }

        /// <summary>
        /// Returns true if line p0-p1 intersects the circle
        /// </summary>
        public bool IntersectsLine(Vector2 p1, Vector2 p2, out Vector2 nearestIntersection)
        {
            float dx, dy, A, B, C, det, t;

            nearestIntersection = Vector2.Zero;

            dx = p2.X - p1.X;
            dy = p2.Y - p1.Y;
            
            A = dx * dx + dy * dy;
            B = 2 * (dx * (p1.X - this._center.X) + dy * (p1.Y - this._center.Y));
            C = (p1.X - this._center.X) * (p1.X - this._center.X) + (p1.Y - this._center.Y) * (p1.Y - this._center.Y) - this._radius * this._radius;

            det = B * B - 4 * A * C;
            if ((A <= 0.0000001) || (det < 0))
            {
                return false;
            }

            if (det == 0)
            {
                // One solution.
                t = -B / (2 * A);
                nearestIntersection = new Vector2(p1.X + t * dx, p1.Y + t * dy);
                return ((nearestIntersection - p1).LengthSquared() < (p2-p1).LengthSquared());
            }
            
           

             t = (float)((-B - Math.Sqrt(det)) / (2 * A));
             nearestIntersection = new Vector2(p1.X + t * dx, p1.Y + t * dy);
             return ((nearestIntersection - p1).LengthSquared() < (p2 - p1).LengthSquared());
        }

        /// <summary>
        /// 
        /// </summary>
        public BoundingCircle Transform(Vector2 pos, float rotation)
        {
            Matrix matRot = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
            Vector2 v = Vector2.Transform(this._center, matRot);

            return new BoundingCircle(v + pos, this._radius);
        }
        public static BoundingCircle FromDataFileRecord(DataFileRecord record)
        {
            BoundingCircle circle = new BoundingCircle();
            circle.InitFromDataFileRecord(record);
            return circle;
        }

        /// <summary>
        /// Returns the square that contains the circle
        /// </summary>
        public BoundingSquare GetContainingSquare()
        {
            return new BoundingSquare(new Vector2(this._center.X - this._radius, this._center.Y - this._radius),
                                                  this._radius * 2, this._radius * 2);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Draw(Color color, Vector2 camPos)
        {
            Vector2 v = new Vector2(1.0f, 0.0f);
            Vector2 v1;
            v *= this._radius;
            for (int i = 0; i < 360; i += 20)
            {
                Matrix mat = Matrix.CreateRotationZ(MathHelper.ToRadians(20));
                v1 = Vector2.Transform(v, mat);
                BrainGame.DrawLine(v - camPos + this._center, v1 - camPos + this._center, color);
                v = v1;
            }
         
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(Vector2 p)
        {
            Vector2 diff = this._center - p;
            return (diff.Length() <= this._radius); 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Collides(BoundingSquare bs)
        {
            // all wrong...
     /*       if (this.Contains(bs.UpperLeft))
                return true;
            if (this.Contains(bs.UpperRight))
                return true;
            if (this.Contains(bs.LowerLeft))
                return true;
            if (this.Contains(bs.LowerRight))
                return true;
            return false;*/

            // Taken from here
            // http://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection/402010#402010
            float circleDistancex = Math.Abs(this._center.X- bs.UpperLeft.X - (bs.Width/2));
            float circleDistancey = Math.Abs(this._center.Y - bs.UpperLeft.Y - (bs.Height/2));

            if (circleDistancex > (bs.Width / 2 + this._radius)) { return false; }
            if (circleDistancey > (bs.Height / 2 + this._radius)) { return false; }

            if (circleDistancex <= (bs.Width / 2)) { return true; } 
            if (circleDistancey <= (bs.Height / 2)) { return true; }

            float cornerDistance_sq = ((circleDistancex - bs.Width / 2) * (circleDistancex - bs.Width / 2)) +
                                      ((circleDistancey - bs.Height / 2) * (circleDistancey - bs.Height / 2));

            return (cornerDistance_sq <= (this._radius * this._radius));


        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Radius: {2}", this._center.X, this._center.Y, this._radius);
        }
        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            float x = record.GetFieldValue<float>("X", this._center.X);
            float y = record.GetFieldValue<float>("Y", this._center.Y);
            this._radius = record.GetFieldValue<float>("Radius", this._radius);
            this._center = new Vector2(x, y);

        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("BoundingSphere"); // Should be bounding circle! lol

            record.AddField("X", this._center.X);
            record.AddField("Y", this._center.Y);
            record.AddField("Radius", this._radius);
            return record;
        }

        #endregion
    }
}
