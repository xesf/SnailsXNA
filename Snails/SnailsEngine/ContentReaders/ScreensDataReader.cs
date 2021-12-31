using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.Content;
//using TwoBrainsGames.Snails.Screens;
//using TRead = TwoBrainsGames.Snails.Screens.ScreensData;
using TRead = TwoBrainsGames.BrainEngine.UI.Screens.ScreensData;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.Snails.ContentReaders
{
	public class ScreensDataReader : ContentTypeReader<TRead>
	{
		protected override TRead Read(ContentReader input, TRead existingInstance)
		{
			ScreensData screensData = new ScreensData();
            GenericContentReader.Read(input, screensData, SnailsGame.Ek);
			return screensData;
		}
	}
}
