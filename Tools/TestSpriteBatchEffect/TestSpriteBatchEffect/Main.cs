using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TestSpriteBatchEffect
{
	/*public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}*/
	[Register ("AppDelegate")]
	class Application : UIApplicationDelegate 
	{
		private Game1 game;
		
		public override void FinishedLaunching (UIApplication app)
		{
			game = new Game1();
			game.Run();
		}
		
		static void Main (string [] args)
		{
			UIApplication.Main (args,null,"AppDelegate");
		}
	}
}
