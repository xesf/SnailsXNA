using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Web.Types;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
	public class LogRegisterRequest : IRequest
	{
		public string Sid { get; private set; }
		public string Message { get; private set; }
		public LogMessageSeverityTypes Severity { get; private set; }
		public DateTime Timestamp { get; private set; }

		public LogRegisterRequest(string sid, DateTime timestamp, string message, LogMessageSeverityTypes severity)
		{
			this.Sid = sid;
			this.Message = message;
			this.Severity = severity;
			this.Timestamp = timestamp;
		}

	}
}
