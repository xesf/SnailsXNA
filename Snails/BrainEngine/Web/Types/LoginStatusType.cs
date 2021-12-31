using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Web.Types
{
	public enum LoginStatusType
	{
		Ok,
		OkUserNeeded,
		NotAuthorized,
        InvalidParams,
		Unknown
	}
}
