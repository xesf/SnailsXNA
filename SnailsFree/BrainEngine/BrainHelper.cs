using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine
{
    public static class BrainHelper
    {
        public static float FindAngleBetweenTwoVectors(Vector2 v1, Vector2 v2)
        {
            float angle;
            // turn vectors into unit vectors   
            v1.Normalize();
            v2.Normalize();

            angle = (float)Math.Acos(Vector2.Dot(v1, v2));
            // if no noticable rotation is available return zero rotation  
            // this way we avoid Cross product artifacts   
            if (Math.Abs(angle) < 0.0001)
                return 0;
            angle *= signal(v1, v2);

            return angle;
        }

        static int signal(Vector2 v1, Vector2 v2)
        {
            return (v1.Y * v2.X - v2.Y * v1.X) > 0 ? 1 : -1;
        }

    }
}
