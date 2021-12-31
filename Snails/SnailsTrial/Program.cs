using System;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails;

namespace TwoBrainsGames.SnailsTrial
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            SnailsGame game = null;

            try
            {
                using (game = new SnailsGame())
                {
                    game.Run();
                }
            }
            catch (System.Exception ex)
            {
                if (game != null)
                {
                    game.ProcessException(ex);
                }
            }

        }
    }
}

