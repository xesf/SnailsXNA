using System;
using System.Collections.Generic;
using System.Linq;
#if IOS
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace TwoBrainsGames.Snails
{
#if IOS
	[Register ("AppDelegate")]
	class Program : UIApplicationDelegate 
	{
		private SnailsGame game;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			game = new SnailsGame();
			game.Run();

			return true;
		}

		static void Main (string [] args)
		{
			TwoBrainsGames.BrainEngine.BrainGame.Rating.DaysUntilPrompt = 5;
			TwoBrainsGames.BrainEngine.BrainGame.Rating.UsesUntilPrompt = 5;

			UIApplication.Main (args,null,"AppDelegate");
		}
	}
#else
	class Program
	{
		static void Main ()
		{
			using (SnailsGame game = new SnailsGame ()) 
			{
				game.Run ();
			}
		}
	}
#endif
}

