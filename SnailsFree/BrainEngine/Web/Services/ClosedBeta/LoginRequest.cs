using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
	public class LoginRequest : IRequest
	{
		public string BetaKey { get; set; }
        public string Country { get; set; }
        public string OsVersion { get; set; }
        public string GameVersion { get; set; }

		public LoginRequest(string betaKey, string gameVersion)
		{
			this.BetaKey = betaKey;
            this.Country = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            System.OperatingSystem osInfo = System.Environment.OSVersion;
            this.OsVersion = string.Format("{0} {1} {2}", osInfo.Platform.ToString(),
                osInfo.VersionString,
                (System.Environment.Is64BitOperatingSystem? "64Bit" : ""));
		}
	}
}
