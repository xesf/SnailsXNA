using Microsoft.Xna.Framework.Content;

using TRead = TwoBrainsGames.Snails.Stages.StageData;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails;

namespace TwoBrainsGames.BrainEngine.Data.Content
{

    public class StageDataReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            StageData stageData = new StageData();
            GenericContentReader.Read(input, stageData, SnailsGame.Ek);
            return stageData;
        }
    }
}
