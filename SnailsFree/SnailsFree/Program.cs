using System;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Debugging;
using System.Resources;
#if BETA_TESTING
using TwoBrainsGames.BrainEngine.Beta;
#endif


namespace TwoBrainsGames.Snails
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
                BETrace.Write(ex);
                if (game != null)
                {
                    game.ProcessException(ex);
                }
            }
            finally
            {
#if BETA_TESTING
                ClosedBeta.EndLogging();
#endif
            }
        }
    }
}

