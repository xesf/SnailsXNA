using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

//using TWrite = TwoBrainsGames.Snails.Screens.ScreensData;
using TWrite = TwoBrainsGames.BrainEngine.UI.Screens.ScreensData;
using TwoBrainsGames.Snails.ContentReaders;

namespace Snails.ContentExtension.ScreensDataContent
{
	
	[ContentTypeWriter]
	public class ScreensDataWriter : ContentTypeWriter<TWrite>
	{
		protected override void Write(ContentWriter output, TWrite value)
		{
			GenericContentWriter.Write(output, value);
		}

		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return typeof(ScreensDataReader).AssemblyQualifiedName;
		}
	}
}
