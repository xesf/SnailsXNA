using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine
{
    public class Parsers
    {

        /// <summary>
        /// "{R:0 G:0 B:0 A:128}"
        /// </summary>
        public static Microsoft.Xna.Framework.Color ParseColor(string colorString)
        {
            if (string.IsNullOrEmpty(colorString))
            {
                throw new BrainException("Error parsing color, colorString cannot be null or empty.");
            }
            colorString = colorString.Replace("{", "");
            colorString = colorString.Replace("}", "");

            int[] rgba = new int[4];
            string[] clrs = colorString.Split(' ');
            for (int i = 0; i < 4; i++)
            {
                string[] clr = clrs[i].Split(':');
                rgba[i] = Convert.ToInt32(clr[1]);
            }

            return new Microsoft.Xna.Framework.Color(rgba[0], rgba[1], rgba[2], rgba[3]);
        }
    }
}
