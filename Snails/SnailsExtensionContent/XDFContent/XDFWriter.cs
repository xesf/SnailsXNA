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
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;

using TWrite = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;
using System.Diagnostics;

namespace Snails.ContentExtension.XDFContent
{
    [ContentTypeWriter]
    public class XDFWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            BinaryDataFileWriter writer = new BinaryDataFileWriter();
            writer.Write(output, value, GenericContentWriter.Ek);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(XDFReader).AssemblyQualifiedName;
        }
    }
}
