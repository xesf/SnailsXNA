using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using TInput = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;
//using TOutput = TwoBrainsGames.Snails.Screens.ScreensData;
//using TwoBrainsGames.Snails.Screens;

using TOutput = TwoBrainsGames.BrainEngine.UI.Screens.ScreensData;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace Snails.ContentExtension.ScreensDataContent
{
	[ContentProcessor(DisplayName = "ScreensData - Snails")]
	public class ScreensDataProcessor : ContentProcessor<TInput, TOutput>
	{
		public override TOutput Process(TInput input, ContentProcessorContext context)
		{
			return ScreensData.FromDataFileRecord(input.RootRecord);
		}
	}
}