using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;
using TOutput = TwoBrainsGames.Snails.Tutorials.Tutorial;
using TwoBrainsGames.Snails.Tutorials;

namespace Snails.ContentExtension.TutorialContent
{
    [ContentProcessor(DisplayName = "Tutorial - Snails")]
    public class TutorialProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return Tutorial.FromDataFileRecord(input.RootRecord);
        }
    }
}