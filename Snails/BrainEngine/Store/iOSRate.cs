#if IOS
using MTiRate;

namespace TwoBrainsGames.BrainEngine.Store
{

	public class iOSRate : Rate
	{
		MTiRate.iRate _mtiRate;
		public override int DaysUntilPrompt {
			get { return (int)_mtiRate.DaysUntilPrompt;}
			set { _mtiRate.DaysUntilPrompt = value;}
		}
		public override uint UsesUntilPrompt{
			get { return _mtiRate.UsesUntilPrompt;}
			set { _mtiRate.UsesUntilPrompt = value;}
		}

		public iOSRate()
		{
			this._mtiRate = iRate.SharedInstance;
			this._mtiRate.UserDidAttemptToRateApp += OnUserRated;	
			this._mtiRate.UserDidDeclineToRateApp += OnUserDeclinedToRate;
			this._mtiRate.UserDidRequestReminderToRateApp += OnRemindLater;
			this._mtiRate.Message = "Please rate, it will only take a second.";
			this._mtiRate.CancelButtonLabel = "No, Thanks";
			this._mtiRate.RemindButtonLabel = "Remind Me Later";
			this._mtiRate.OnlyPromptIfLatestVersion = true;
			this._mtiRate.MessageTitle = "Snails";
			this._mtiRate.RateButtonLabel = "Rate It Now";
#if DEBUG
			this._mtiRate.PreviewMode = true;
#endif
			this.Active = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void RateGameAsync()
		{
			this._mtiRate.OnlyPromptIfLatestVersion = false;
			this._mtiRate.MessageTitle = BrainGame.Settings.GameName;
			this._mtiRate.Message = "Please rate, it will only take a second.";
			this._mtiRate.CancelButtonLabel = "No, Thanks";
			this._mtiRate.RemindButtonLabel = "Remind Me Later";
			this._mtiRate.RateButtonLabel = "Rate It Now";
			this._mtiRate.PromptIfNetworkAvailable ();
		}
	}

}
#endif
