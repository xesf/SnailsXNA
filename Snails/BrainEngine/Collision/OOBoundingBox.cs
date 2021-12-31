using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Collision
{
    // Object oriented bounding box
    // The main diference from a regular BoundingSquare is that a OOBB may not be aligend to the axis (it may be rotated)
    // That's why this type of BB requires 4 points to be defined.
    public struct OOBoundingBox
    {
        public enum CollidingSegment
        {
            None,
            P0P1,
            P1P2,
            P2P3,
            P3P0
        }
        #region Members
        public Vector2 P0;
        public Vector2 P1;
        public Vector2 P2;
        public Vector2 P3;
        #endregion


        public OOBoundingBox(Rectangle rect)
        {
            P0 = new Vector2(rect.Left, rect.Top);
            P1 = new Vector2(rect.Left + rect.Width, rect.Top);
            P2 = new Vector2(rect.Left + rect.Width, rect.Top + rect.Height);
            P3 = new Vector2(rect.Left, rect.Top + rect.Height);
        }

        public OOBoundingBox(Vector2 ul, Vector2 lr)
        {
            P0 = new Vector2(ul.X, ul.Y);
            P1 = new Vector2(lr.X, ul.Y);
            P2 = new Vector2(lr.X, lr.Y);
            P3 = new Vector2(ul.X, lr.Y);
        }

        public OOBoundingBox(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            P0 = p1;
            P1 = p2;
            P2 = p3;
            P3 = p4;
        }

        public OOBoundingBox(float left, float top, float width, float height)
        {
          P0 = new Vector2(left, top);
          P1 = new Vector2(left + width, top);
          P2 = new Vector2(left + width, top + height);
          P3 = new Vector2(left, top + height);
        }

        public void Draw(Color color, Vector2 camPos)
        {
            BrainGame.DrawLine(this.P0 - camPos, this.P1 - camPos, color);
            BrainGame.DrawLine(this.P1 - camPos, this.P2 - camPos, color);
            BrainGame.DrawLine(this.P2 - camPos, this.P3 - camPos, color);
            BrainGame.DrawLine(this.P3 - camPos, this.P0 - camPos, color);
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 GetCenter()
        {
            Vector2 v;
            Mathematics.LineLineIntersection(P0, P2, P1, P3, out v);
            return v;
        }

        // Applies the transformations to the points
        public OOBoundingBox Transform(Matrix mat)
        {
            Vector2 p0 = Vector2.Transform(this.P0, mat);
            Vector2 p1 = Vector2.Transform(this.P1, mat);
            Vector2 p2 = Vector2.Transform(this.P2, mat);
            Vector2 p3 = Vector2.Transform(this.P3, mat);

            return new OOBoundingBox(p0, p1, p2, p3);
        }

        /// <summary>
        /// Transforms the bounding box points with to the specified position and rotation
        /// Returns the transformed bounding box
        /// First applies position transforms and then applies rotation
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
        public void TransformInPlace(Vector2 position)
        {
            this.P0 += position;
            this.P1 += position;
            this.P2 += position;
            this.P3 += position;
        }

        /// <summary>
        /// Transforms the bounding box points with to the specified position and rotation
        /// Returns the transformed bounding box
        /// First applies rotation and then applies position
        /// </summary>
        public OOBoundingBox Transform(float rotation, Vector2 position)
        {
            Matrix matRot = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
            Vector3 pos = new Vector3(position.X, position.Y, 0);
            Matrix matTrans = Matrix.CreateTranslation(pos);
            Matrix matFinal = matRot * matTrans;

            return this.Transform(matFinal);
        }

        // Check if collides with a aabb
        public bool Collides (BoundingSquare bbox)
        {
            return bbox.Collides(this); // Already implemented in the aabb. hehehe
        }

        // Check if collides with a oobb
        // This is possible, but I'm don't feel like implementing it, besides it requires some heavy processing
        // We will check collision with oobb in a diferent way
        public bool Collides(OOBoundingBox bbox)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rectangle ToRect()
        {
            float minx, miny, maxx, maxy;
            minx = this.P0.X;
            miny = this.P0.Y;

            maxx = this.P0.X;
            maxy = this.P0.Y;

            if (minx > this.P1.X)
                minx = this.P1.X;
            if (minx > this.P2.X)
                minx = this.P2.X;
            if (minx > this.P3.X)
                minx = this.P3.X;

            if (miny > this.P1.Y)
                miny = this.P1.Y;
            if (miny > this.P2.Y)
                miny = this.P2.Y;
            if (miny > this.P3.Y)
                miny = this.P3.Y;

            if (maxx < this.P1.X)
                maxx = this.P1.X;
            if (maxx < this.P2.X)
                maxx = this.P2.X;
            if (maxx < this.P3.X)
                maxx = this.P3.X;

            if (maxy < this.P1.Y)
                maxy = this.P1.Y;
            if (maxy < this.P2.Y)
                maxy = this.P2.Y;
            if (maxy < this.P3.Y)
                maxy = this.P3.Y;

            return new Rectangle((int)minx, (int)miny, (int)(maxx - minx), (int)(maxy - miny));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(int x, int y)
        {
            return this.ToRect().Contains(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInside(Vector2 vLowerLeft, Vector2 vUpperRight)
        {
		    if (this.P3.X < vLowerLeft.X || this.P1.X > vUpperRight.X)
		    {
			    return false;
		    }
		    if (this.P3.Y < vLowerLeft.Y || this.P1.Y > vUpperRight.Y)
		    {
			    return false;
		    }

		    return true;
  	    } 

        /// <summary>
        /// Converts oo bounding box to an aa bounding box
        /// </summary>
        public BoundingBox ToBoundingBox()
        {
            return new BoundingBox(new Vector3(this.P0.X, this.P0.Y, 0.0f),
                                   new Vector3(this.P2.X, this.P2.Y, 0.0f));
        }

        /// <summary>
        /// 
        /// </summary>
        public CollidingSegment IntersectsLine(Vector2 lineP0, Vector2 lineP1,
                                   out Vector2 nearestColliddingPoint)
        {
            nearestColliddingPoint = Vector2.Zero;
            CollidingSegment collidingSegment = CollidingSegment.None;
            Vector2 intersetionPoint;
            float nearestLength = 999999;
            float length = -1;

            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P0, this.P1, out intersetionPoint))
            {
                nearestLength = (lineP0 - intersetionPoint).Length();
                nearestColliddingPoint = intersetionPoint;
                collidingSegment = CollidingSegment.P0P1;
            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P1, this.P2, out intersetionPoint))
            {
                length = (lineP0 - intersetionPoint).Length();
                if (length < nearestLength)
                {
                    nearestLength = length;
                    nearestColliddingPoint = intersetionPoint;
                    collidingSegment = CollidingSegment.P1P2;
                }

            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P2, this.P3, out intersetionPoint))
            {
                length = (lineP0 - intersetionPoint).Length();
                if (length < nearestLength)
                {
                    nearestLength = length;
                    nearestColliddingPoint = intersetionPoint;
                    collidingSegment = CollidingSegment.P2P3;
                }
            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P3, this.P0, out intersetionPoint))
            {
                length = (lineP0 - intersetionPoint).Length();
                if (length < nearestLength)
                {
                    nearestLength = length;
                    nearestColliddingPoint = intersetionPoint;
                    collidingSegment = CollidingSegment.P3P0;
                }
            }

            return collidingSegment;

        }
        /// <summary>
        /// 
        /// </summary>
        public bool IntersectsLine(Vector2 lineP0, Vector2 lineP1,
                                   out Vector2 intersectingPoint1,
                                   out Vector2 intersectingPoint2)
        {
            int hits = 0;
            intersectingPoint1 = Vector2.Zero;
            intersectingPoint2 = Vector2.Zero;
            Vector2 intersetionPoint;

            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P0, this.P1, out intersetionPoint))
            {
                intersectingPoint1 = intersetionPoint;
                hits++;
            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P1, this.P2, out intersetionPoint))
            {
                if (hits > 0)
                    intersectingPoint2 = intersetionPoint;
                else
                    intersectingPoint1 = intersetionPoint;
                hits++;
            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P2, this.P3, out intersetionPoint))
            {
                if (hits > 0)
                    intersectingPoint2 = intersetionPoint;
                else
                    intersectingPoint1 = intersetionPoint;
                hits++;
            }
            if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P3, this.P0, out intersetionPoint))
            {
                if (hits > 0)
                    intersectingPoint2 = intersetionPoint;
                else
                    intersectingPoint1 = intersetionPoint;
                hits++;
            }

            return (hits > 0);
            
        }

        /// <summary>
        /// Converts oo bounding box to an aa bounding box
        /// </summary>
        public BoundingSquare ToBoundingSquare()
        {
            float minx = Math.Min(this.P0.X, this.P1.X);
            minx = Math.Min(minx, this.P2.X);
            minx = Math.Min(minx, this.P3.X);

            float maxx = Math.Max(this.P0.X, this.P1.X);
            maxx = Math.Max(maxx, this.P2.X);
            maxx = Math.Max(maxx, this.P3.X);

            float miny = Math.Min(this.P0.Y, this.P1.Y);
            miny = Math.Min(miny, this.P2.Y);
            miny = Math.Min(miny, this.P3.Y);

            float maxy = Math.Max(this.P0.Y, this.P1.Y);
            maxy = Math.Max(maxy, this.P2.Y);
            maxy = Math.Max(maxy, this.P3.Y);

            return new BoundingSquare(new Vector2(minx, miny), new Vector2(maxx, maxy));

        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize(Vector2 ul, Vector2 lr)
        {
          this.P0 = new Vector2(ul.X, ul.Y);
          this.P1 = new Vector2(lr.X, ul.Y);
          this.P2 = new Vector2(lr.X, lr.Y);
          this.P3 = new Vector2(ul.X, lr.Y);
        }
       
        /// <summary>
        /// Transforms in place transforms the current instance of the BB
        /// </summary>
        public void TransformInPlace(float rotation, Vector2 position)
        {
            if (rotation != 0)
            {
                Matrix matRot = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
                Vector3 pos = new Vector3(position.X, position.Y, 0);
                Matrix matTrans = Matrix.CreateTranslation(pos);
                Matrix matFinal = matRot * matTrans;

                this.TransformInPlace(matFinal);
            }

            this.TransformInPlace(Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0)));
        }

        /// <summary>
        /// Applies the transformations to the points
        /// </summary>
        public void TransformInPlace(Matrix mat)
        {
          this.P0 = Vector2.Transform(this.P0, mat);
          this.P1 = Vector2.Transform(this.P1, mat);
          this.P2 = Vector2.Transform(this.P2, mat);
          this.P3 = Vector2.Transform(this.P3, mat);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Equals(OOBoundingBox bb)
        {
            return (bb.P0 == this.P0 &&
                    bb.P1 == this.P1 &&
                    bb.P2 == this.P2 &&
                    bb.P3 == this.P3);
        }
    }
}
