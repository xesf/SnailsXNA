using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using TwoBrainsGames.BrainEngine.Data.Content;

using TWrite = TwoBrainsGames.Snails.Stages.StageData;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Snails.ContentExtension.StageDataContent
{
    [ContentTypeWriter]
    public class StageDataWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            GenericContentWriter.Write(output, value);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(StageDataReader).AssemblyQualifiedName;
        }
    }
}
