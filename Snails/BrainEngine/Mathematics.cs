using System;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.BrainEngine
{
  public class Mathematics
  {
    //static Vector2 NormalVectorV2 = new Vector2(1.0f, 0.0f);

    /// <summary>
    /// Returns the angle between one vector and the vector 1.0, 0.0f
    /// Note: This only works if v1 is normalized by the caller
    /// </summary>
    public static float VectorAngle(Vector2 v1)
    {
      float angle = (float)Math.Asin(v1.Y);
      angle = MathHelper.ToDegrees(angle);
      if (v1.X < 0)
        return 180 - angle;

      if (v1.Y < 0)
        return 360 + angle;

      return angle;
    }

      /// <summary>
      /// 
      /// </summary>
    public static float AngleBetweenNormalizedVectors(Vector2 v1, Vector2 v2)
    {
        float angle;

        angle = (float)Math.Acos(Vector2.Dot(v1, v2));
        // if no noticable rotation is available return zero rotation  
        // this way we avoid Cross product artifacts   
        if (Math.Abs(angle) < 0.0001)
            return 0;

        angle *= Mathematics.Signal(v1, v2);

        return MathHelper.ToDegrees(angle);
    }

    /// <summary>
    /// 
    /// </summary>
    static int Signal(Vector2 v1, Vector2 v2)
    {
        return (v1.Y * v2.X - v2.Y * v1.X) > 0 ? 1 : -1;
    }

    /// <summary>
    /// Determines if the point ptToCheck lies on the line p0-p1
    /// This only works for 2d lines
    /// </summary>
    public static bool IsPointOnLine(Vector2 p0, Vector2 p1, Vector2 ptToCheck)
    {
        float div1 = (p1.Y - p0.Y == 0 ? 1 : p1.Y - p0.Y);
        float div2 = (p1.X - p0.X == 0 ? 1 : p1.X - p0.X);

        return ((ptToCheck.Y - p0.Y) / div1) == ((ptToCheck.X - p0.X) / div2);
    }

    /// <summary>
    /// Classifys a point regarding to a ray
    /// Reference: http://alienryderflex.com/point_left_of_ray/
    /// </summary>
    public static float ClassifyPointInRay(Vector2 point, Vector2 rayStart, Vector2 rayEnd)
    {
        return (point.Y - rayStart.Y) * (rayEnd.X - rayStart.X) - (point.X - rayStart.X) * (rayEnd.Y - rayStart.Y); 
    }

    /// <summary>
    /// 
    /// </summary>
    public static Vector2 RandomizeVector(int minAngle, int maxAngle)
    {
        float angle = minAngle + BrainGame.Rand.Next(maxAngle - minAngle);
        float angleSin = (float)Math.Sin(MathHelper.ToRadians(-angle));
        float angleCos = (float)Math.Cos(MathHelper.ToRadians(angle));
        return new Vector2(angleCos, angleSin);
    }

    /// <summary>
    /// Computes the intercting point of two lines
    /// Returns false if the lines don't intersect
    /// Reference: http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
    /// </summary>
    public static bool LineLineIntersection(Vector2 p0, Vector2 p1,
                                            Vector2 p2, Vector2 p3,
                                            out Vector2 i)
    {
        i = Vector2.Zero;

        Vector2 s1, s2;
        s1.X = p1.X - p0.X;
        s1.Y = p1.Y - p0.Y;
        s2.X = p3.X - p2.X;
        s2.Y = p3.Y - p2.Y;

        float s, t;
        s = (-s1.Y * (p0.X - p2.X) + s1.X * (p0.Y - p2.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
        t = (s2.X * (p0.Y - p2.Y) - s2.Y * (p0.X - p2.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
        {
            // Collision detected
            i.X = (p0.X + (t * s1.X));
            i.Y = (p0.Y + (t * s1.Y)); 
            return true;
        }

        return false; // No collision
    }

    /// <summary>
    /// Rotates a vector
    /// Taken and adapted from here: http://www.kirupa.com/forum/showthread.php?12181-Rotating-a-Vector
    /// </summary>
    public static Vector2 RotateVector(Vector2 v, float angleInDegrees)
    {
        float angle = MathHelper.ToRadians(angleInDegrees);
        float ca = (float) Math.Cos(angle);
        float sa = (float) Math.Sin(angle);

        return new Vector2(v.X * ca - v.Y * sa, v.X * sa + v.Y * ca);
      
    }

    /// <summary>
    /// Transforms a vector. First applies rotation then applies translation
    /// </summary>
    public static Vector2 TransformVector(Vector2 toTransform, float rotation, Vector2 position)
    {
        Matrix matRot = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
        Matrix matTrans = Matrix.CreateTranslation(position.X, position.Y, 0f);
        Matrix matFinal = matRot * matTrans;

        return Vector2.Transform(toTransform, matFinal);
    }

      /// <summary>
      /// Converts an 0/360 angle to -180/180 angle
      /// </summary>
    public static float ConvertAngleTo180(float angle)
    {
        if (angle <= 180)
            return angle;

        return (angle - 360f);
    }

  }
}
