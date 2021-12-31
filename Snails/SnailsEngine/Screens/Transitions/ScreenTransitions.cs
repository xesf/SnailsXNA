using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Screens.Transitions
{
	/// <summary>
	/// Static class to hold the screen transitions
	/// This screen transitions are used thruout the game in almost every screen group
	/// They are created at start up and are available everytime and everywere
	/// </summary>
    class ScreenTransitions
    {
		#region Consts
        const float FADE_IN_SPEED = 0.002f;
        const float FADE_OUT_SPEED = 0.008f;
		#endregion

		#region vars
        static FadeInTransition _fadeInTransition;
        static FadeInTransition _fadeInWhiteTransition;
        static FadeOutTransition _fadeOutTransition;
        static FadeOutTransition _fadeOutWhiteTransition;
        static LeafTransition _leafsClosingTransition;
        static LeafTransition _leafsClosedTransition;
        static LeafTransition _leafsOpeningTransition;
		#endregion

		#region Properties
		public static FadeInTransition FadeIn
		{ get { return ScreenTransitions._fadeInTransition; } }
        public static FadeInTransition FadeInWhite
        { get { return ScreenTransitions._fadeInWhiteTransition; } }
		public static FadeOutTransition FadeOut
		{ get { return ScreenTransitions._fadeOutTransition; } }
        public static FadeOutTransition FadeOutWhite
        { get { return ScreenTransitions._fadeOutWhiteTransition; } }
        public static LeafTransition LeafsClosing
		{ get { return ScreenTransitions._leafsClosingTransition; } }
        public static LeafTransition LeafsClosed
        { get { return ScreenTransitions._leafsClosedTransition; } }
        public static LeafTransition LeafsOpening
		{ get { return ScreenTransitions._leafsOpeningTransition; } }
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public static void Initialize()
		{
			ScreenTransitions._fadeInTransition = new FadeInTransition(ScreenTransitions.FADE_IN_SPEED);
            ScreenTransitions._fadeInWhiteTransition = new FadeInTransition(ScreenTransitions.FADE_IN_SPEED);
			ScreenTransitions._fadeOutTransition = new FadeOutTransition(ScreenTransitions.FADE_OUT_SPEED);
            ScreenTransitions._fadeOutWhiteTransition = new FadeOutTransition(ScreenTransitions.FADE_OUT_SPEED, Color.White);

			ScreenTransitions._leafsClosingTransition = new LeafTransition(LeafTransition.State.Closing);
            ScreenTransitions._leafsClosingTransition.LoadContent();
			ScreenTransitions._leafsOpeningTransition = new LeafTransition(LeafTransition.State.Opening);
            ScreenTransitions._leafsOpeningTransition.LoadContent();
            ScreenTransitions._leafsClosedTransition = new LeafTransition(LeafTransition.State.ClosedStopped);
            ScreenTransitions._leafsClosedTransition.LoadContent();
        }            
    }
}
