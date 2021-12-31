using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;
using TOutput = TwoBrainsGames.Snails.Stages.StageData;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageObjects;

namespace Snails.ContentExtension.StageDataContent
{
    [ContentProcessor(DisplayName = "StageData - Snails")]
    public class StageDataProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            StageObjectFactory.Initialize();
            return StageData.FromDataFileRecord(input.RootRecord);
        }
    }
}