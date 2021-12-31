using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Web
{
	public class BEWebException : ApplicationException
	{
		public BEWebException()
		{
		}

		public BEWebException(string message) :
			base(message)
		{
		}
	}
}
