using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TwoBrainsGames.Snails
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		private SnailsGame game;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			game = new SnailsGame();
			game.Run();

			return true;
		}
	}
}

