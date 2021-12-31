using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Beta
{

	public class SnailsStageStats
	{
		public string Theme { get; set; }
		public int StageNumber { get; set; }
		public System.DateTime TimeStarted { get; set; }
		public int Status { get; set; }
		public int Goldcoins { get; set; }
		public int Silvercoins { get; set; }
		public int Bronzecoins { get; set; }
		public int Score { get; set; }
		public int Unusedtools { get; set; }
		public System.DateTime TimeTaken { get; set; }
		public int BuildNr { get; set; }
        public int Medal { get; set; }

		public SnailsStageStats()
		{
		}
	}
}
