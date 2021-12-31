using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Web.Types;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
	public class RegisterStageStatsResponse : IResponse
	{
		public RegisterStageStatsStatusType Status { get; private set; }
		public string StatusMessage { get; private set; }

		public RegisterStageStatsResponse(RegisterStageStatsStatusType status, string statusMessage)
		{
			this.Status = status;
			this.StatusMessage = statusMessage;
		}
	}
}
