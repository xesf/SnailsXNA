using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Debugging;

namespace TwoBrainsGames.BrainEngine.Beta
{
	class RegisterStageStatsDispatcherItem : IDataDispatcherItem
	{
		SnailsStageStats Stats { get; set; }
		public RegisterStageStatsDispatcherItem(SnailsStageStats stats)
		{
			this.Stats = stats;
		}

		#region IDataDispatcherItem Members
		/// <summary>
		/// 
		/// </summary>
		public void Dispatch()
		{
			RegisterStageStatsRequest request = new RegisterStageStatsRequest(ClosedBeta.SID, this.Stats.Theme, this.Stats.StageNumber, this.Stats.Status);
			request.Bronzecoins = this.Stats.Bronzecoins;
			request.Goldcoins = this.Stats.Goldcoins;
			request.Score = this.Stats.Score;
			request.Silvercoins = this.Stats.Silvercoins;
			request.TimeStarted = this.Stats.TimeStarted;
			request.TimeTaken = this.Stats.TimeTaken;
			request.Unusedtools = this.Stats.Unusedtools;
			request.StageBuildNr = this.Stats.BuildNr;
            request.Medal = this.Stats.Medal;

            BETrace.WriteLine("Dispatching RegisterStageStats message Theme[{0}] StageNr[{1}] Score[{2}] Status[{3}]",
                request.Theme, request.StageNumber, request.Score, request.Status);
            ClosedBeta.WSService.RegisterStageStats(request);
		}

		#endregion
	}
}
