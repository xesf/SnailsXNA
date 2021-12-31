using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using TwoBrainsGames.BrainEngine.Resources;

using TInput = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;
using TOutput = TwoBrainsGames.BrainEngine.Resources.SpriteSet;

namespace Snails.ContentExtension.SpriteSetContent
{
    [ContentProcessor(DisplayName = "SpriteSet - Snails")]
    public class SpriteSetProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return SpriteSet.FromDataFileRecord(input.RootRecord);
        }
    }
}