using TwoBrainsGames.BrainEngine.Web.Types;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
	public class RegisterStageStatsRequest : IRequest
	{
		public string SID { get; private set;}
		public string Theme { get; private set; }
		public int StageNumber { get; private set; }
		public int Status { get; private set; }
		public System.DateTime TimeStarted { get; set; }
		public int Goldcoins { get; set; }
		public int Silvercoins { get; set; }
		public int Bronzecoins { get; set; }
		public int Score { get; set; }
		public int Unusedtools { get; set; }
		public bool IsNewHighScore { get; set; }
		public int Medal { get; set; }
		public int StageBuildNr { get; set; }
		public System.DateTime TimeTaken { get; set; }

		public RegisterStageStatsRequest(string sid, string theme, int stageNumber, int status)
		{
			this.SID = sid;
			this.Theme = theme;
			this.StageNumber = stageNumber;
			this.Status = status;
		}
	}

}
