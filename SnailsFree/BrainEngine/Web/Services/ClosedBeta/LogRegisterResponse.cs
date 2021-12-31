using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Web.Types;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
	public class LogRegisterResponse : IResponse
	{
		public LogRegisterStatusTypes Status { get; private set; }
		public string StatusMessage { get; private set; }

		public LogRegisterResponse(LogRegisterStatusTypes status, string statusMessage)
		{
			this.Status = status;
			this.StatusMessage = statusMessage;
		}
	}
}
