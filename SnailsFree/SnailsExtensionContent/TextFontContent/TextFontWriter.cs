using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using TwoBrainsGames.BrainEngine.Data.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;

using TWrite = TwoBrainsGames.BrainEngine.Resources.TextFont;

namespace Snails.ContentExtension.FontContent
{
    [ContentTypeWriter]
    public class FontWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            GenericContentWriter.Write(output, value);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TextFontReader).AssemblyQualifiedName;
        }
    }
}
