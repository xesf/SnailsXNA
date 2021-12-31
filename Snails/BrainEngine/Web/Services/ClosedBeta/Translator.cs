using System;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
	class Translator
	{
		public static string DateTimeToString(DateTime date)
		{
			return string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}",
							date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
		}
	}
}
