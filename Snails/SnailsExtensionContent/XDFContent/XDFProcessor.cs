using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using TInput = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;
using TOutput = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;

namespace Snails.ContentExtension.XDFContent
{
    [ContentProcessor(DisplayName = "XDFProcessor - Snails")]
    public class XDFProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return input;
        }
    }
}