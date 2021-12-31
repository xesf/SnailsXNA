using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

using TInput = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;
using TOutput = TwoBrainsGames.BrainEngine.Resources.TextFont;

namespace Snails.ContentExtension.FontContent
{
    [ContentProcessor(DisplayName = "TextFont - Snails")]
    public class FontProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return TextFont.FromDataFileRecord(input.RootRecord);
        }
    }
}