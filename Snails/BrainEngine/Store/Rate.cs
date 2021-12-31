using System;


namespace TwoBrainsGames.BrainEngine.Store
{
	public class Rate
	{

		public event System.EventHandler OnUserRatedGame;
		public event System.EventHandler OnUserDeclinedGameRate;
		public event System.EventHandler OnUserAskedToRateLater;

		public bool Active { get; protected set; }
		public virtual int DaysUntilPrompt {
			get;
			set;
		}
		public virtual uint UsesUntilPrompt {
			get;
			set;
		}

		public Rate ()
		{

		}

		public static Rate Create()
		{
			Rate rate = new Rate();
			#if IOS
			rate = new iOSRate();

			#endif

			return rate;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void RateGameAsync()
		{
		}

		/// <summary>
		/// Raises the user rated event.
		/// </summary>
		protected void OnUserRated(object sender, EventArgs e)
		{
			if (OnUserRatedGame != null) {
				OnUserRatedGame (this, e);
			}
		}

		/// <summary>
		/// Raises the user declined to rate event.
		/// </summary>
		protected void OnUserDeclinedToRate(object sender, EventArgs e)
		{
			if (OnUserDeclinedGameRate != null) {
				OnUserDeclinedGameRate (this, e);
			}
		}

		/// <summary>
		/// Raises the remind later event.
		/// </summary>
		protected void OnRemindLater(object sender, EventArgs e)
		{
			if (OnUserAskedToRateLater != null) {
				OnUserAskedToRateLater (this, e);
			}
		}
	}
}

