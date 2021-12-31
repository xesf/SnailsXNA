using System;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.Stages
{
  
    public enum PathSegmentBehavior
    {
        None = 0,
        Walkable = 1,
        ReverseWalk = 2,
        WalkableCW = 3,
        WalkableCCW = 4
    }

    public class PathSegment
    {
        public enum SegmentType
        {
            None,
            Floor,
            RightWall,
            LeftWall,
            Ceiling
        }
        private Vector2 _p0;
        private Vector2 _p1;

        #region Properties
        public Vector2 P0 
        {
            get
            {
                return this._p0;
            }

            set
            {
                this._p0 = value;
                this.UpdateCenter();
            }
        }

        public Vector2 P1
        {
            get
            {
                return this._p1;
            }

            set
            {
                this._p1 = value;
                this.UpdateCenter();
            }
        }
        

        public PathSegmentBehavior Behavior { get; set; }
        public Vector2 Normal { get; private set; }
        public float Length { get; private set; }
        public float Rotation { get; private set; } // Rotation that the path makes with the vector (1, 0)
        public SegmentType WallType { get; private set; }
        public bool FloorIsBreakable { get; set; }
        public Vector2 Center { get; private set; }
        #endregion

        public PathSegment(Vector2 p0, Vector2 p1)
            : this(p0, p1, PathSegmentBehavior.None, true)
        { }

        public PathSegment(Vector2 p0, Vector2 p1, PathSegmentBehavior behavior, bool floorIsBreakable)
        {
            P0 = p0;
            P1 = p1;
            Behavior = behavior;
            FloorIsBreakable = floorIsBreakable;
            
            // Pre compute normal and length of the path vector
            this.Normal = Vector2.Normalize(p1 - p0);
            this.Length = (this.P1 - this.P0).Length();
            this.Rotation = BrainEngine.Mathematics.VectorAngle(this.Normal);

            if (this.P0.X < this.P1.X)
                this.WallType = SegmentType.Floor;
            else 
            if (this.P0.X > this.P1.X)
                this.WallType = SegmentType.Ceiling;
            else
            if (this.P0.X == this.P1.X && this.P0.Y < this.P1.Y)
                this.WallType = SegmentType.LeftWall;
            else
                this.WallType = SegmentType.RightWall;
        }

        #region Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(PathSegment a, PathSegment b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.P0 == b.P0 && a.P1 == b.P1;
        }

        public static bool operator !=(PathSegment a, PathSegment b)
        {
            return !(a == b);
        }

        #endregion 

        #region Collisions
     
        /// <summary>
        /// Determines if the segment intersects a BoundingSquare
        /// </summary>
        public bool Intersects(BoundingSquare bs)
        {
            Vector3 dir = new Vector3(this.P1.X - this.P0.X, this.P1.Y - this.P0.Y, 0f);
            Vector3 p0 = new Vector3(this.P0.X, this.P0.Y, 0f);
            float length = dir.Length();
            dir.Normalize();
            Ray ray1 = new Ray(p0, dir);
            

            BoundingBox bb = new BoundingBox(new Vector3(bs.UpperLeft.X, bs.UpperLeft.Y, 0f),
                                             new Vector3(bs.LowerRight.X, bs.LowerRight.Y, 0f));

            float? dist = bb.Intersects(ray1);
            return (dist != null && dist.Value < length);
        }

        public bool Intersection(PathSegment S1, ref Point ptIntersection)
        {
            PathSegment S0 = this;

            // Denominator for ua and ub are the same, so store this calculation
            double d =
               (S1.P1.Y - S1.P0.Y) * (S0.P1.X - S0.P0.X)
               -
               (S1.P1.X - S1.P0.X) * (S0.P1.Y - S0.P0.Y);

            //n_a and n_b are calculated as seperate values for readability
            double n_a =
               (S1.P1.X - S1.P0.X) * (S0.P0.Y - S1.P0.Y)
               -
               (S1.P1.Y - S1.P0.Y) * (S0.P0.X - S1.P0.X);

            double n_b =
               (S0.P1.X - S0.P0.X) * (S0.P0.Y - S1.P0.Y)
               -
               (S0.P1.Y - S0.P0.Y) * (S0.P0.X - S1.P0.X);

            // Make sure there is not a division by zero - this also indicates that
            // the lines are parallel.  
            // If n_a and n_b were both equal to zero the lines would be on top of each 
            // other (coincidental).  This check is not done because it is not 
            // necessary for this implementation (the parallel check accounts for this).
            if (d == 0)
                return false;

            // Calculate the intermediate fractional point that the lines potentially intersect.
            double ua = n_a / d;
            double ub = n_b / d;

            // The fractional point will be between 0 and 1 inclusive if the lines
            // intersect.  If the fractional calculation is larger than 1 or smaller
            // than 0 the lines would need to be longer to intersect.
            if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d)
            {
                ptIntersection.X = (int)(S0.P0.X + (ua * (S0.P1.X - S0.P0.X)));
                ptIntersection.Y = (int)(S0.P0.Y + (ua * (S0.P1.Y - S0.P0.Y)));
            
                return true;
            }

            return false;
        }

        public bool Collides(PathSegment other, ref Point I)
        {
            return Intersection(other, ref I);
        }

        public bool Intersection(PathSegment S1, ref Vector2 ptIntersection)
        {
            PathSegment S0 = this;

            // Denominator for ua and ub are the same, so store this calculation
            double d =
               (S1.P1.Y - S1.P0.Y) * (S0.P1.X - S0.P0.X)
               -
               (S1.P1.X - S1.P0.X) * (S0.P1.Y - S0.P0.Y);

            //n_a and n_b are calculated as seperate values for readability
            double n_a =
               (S1.P1.X - S1.P0.X) * (S0.P0.Y - S1.P0.Y)
               -
               (S1.P1.Y - S1.P0.Y) * (S0.P0.X - S1.P0.X);

            double n_b =
               (S0.P1.X - S0.P0.X) * (S0.P0.Y - S1.P0.Y)
               -
               (S0.P1.Y - S0.P0.Y) * (S0.P0.X - S1.P0.X);

            // Make sure there is not a division by zero - this also indicates that
            // the lines are parallel.  
            // If n_a and n_b were both equal to zero the lines would be on top of each 
            // other (coincidental).  This check is not done because it is not 
            // necessary for this implementation (the parallel check accounts for this).
            if (d == 0)
                return false;

            // Calculate the intermediate fractional point that the lines potentially intersect.
            double ua = n_a / d;
            double ub = n_b / d;

            // The fractional point will be between 0 and 1 inclusive if the lines
            // intersect.  If the fractional calculation is larger than 1 or smaller
            // than 0 the lines would need to be longer to intersect.
            if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d)
            {
                ptIntersection = new Vector2((float)(S0.P0.X + (ua * (S0.P1.X - S0.P0.X))), 
                                             (float)(S0.P0.Y + (ua * (S0.P1.Y - S0.P0.Y))));
                return true;
            }

            return false;
        }

        public bool Collides(PathSegment other, ref Vector2 I)
        {
            return Intersection(other, ref I);
        }
        /// <summary>
        /// Classifies two PathSegments relative to each other
        /// The vectors formed by the PathSegments are used to make the computation
        /// Returns 0 if vectors are paralel
        /// Returns > 0 if the cross product resulting vector faces the camera
        /// Returns < 0 if the cross product resulting vector faces away from the camera
        /// This method is used to filter undesired segments that don't matter in collision detection (segments that
        /// don't face the object are filtered)
        /// </summary>
        public float Classify(PathSegment segment)
        {
            Vector3 v1 = new Vector3(this.Normal.X, this.Normal.Y, 0.0f);
            Vector3 v2 = new Vector3(segment.Normal.X, segment.Normal.Y, 0.0f);

            return Vector3.Cross(v1, v2).Z;
        }


        #endregion

        private void UpdateCenter()
        {
            this.Center = this.P0 + new Vector2((this.P1.X - this.P0.X) / 2, (this.P1.Y - this.P0.Y) / 2);
        }

        public override string ToString()
        {
            return (String.Format("P0[{0},{1}], P1[{2},{3}]", this.P0.X, this.P0.Y, this.P1.X, this.P1.Y));
        }
    }
}
