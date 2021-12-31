using System;
#if DEBUG
using TwoBrainsGames.BrainEngine.Debugging;
#endif
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
                    Console.WriteLine("Running game...");
                    game.Run();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                //BETrace.Write(ex);
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
                Console.WriteLine("DONE!!!");
            }
        }
    }
}

