using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Web.Types;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
	public class LoginResponse : IResponse
	{
		public LoginStatusType Status { get; private set; }
		public string StatusMessage { get; private set; }
		public string SID { get; private set; }

		public LoginResponse(LoginStatusType status, string statusMessage, string sid)
		{
			this.Status = status;
			this.SID = sid;
			this.StatusMessage = statusMessage;
		}
	}
}
