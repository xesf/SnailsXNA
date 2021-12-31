using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta;
using TwoBrainsGames.BrainEngine.Web.Types;

namespace TwoBrainsGames.BrainEngine.Beta
{
	class LogDispatcherItem : IDataDispatcherItem
	{
		LogMessageSeverityTypes Severity { get; set; }
		string Message { get; set; }

		public LogDispatcherItem(string message, LogMessageSeverityTypes severity)
		{
			this.Message = message;
			this.Severity = severity;
		}

		#region IWSDispatcherItem Members
		/// <summary>
		/// 
		/// </summary>
		public void Dispatch()
		{
			ClosedBeta.WSService.Log(new LogRegisterRequest(ClosedBeta.SID, DateTime.Now, this.Message, this.Severity));
		}
        #endregion
	}
}
