using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.Snails.Stages;
using TRead = TwoBrainsGames.Snails.Stages.Stage;
using TwoBrainsGames.BrainEngine.Data.Content;
using TwoBrainsGames.Snails;

namespace TwoBrainsGames.BrainEngine.Data.Content
{

    public class StageReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            Stage stage = new Stage();
            GenericContentReader.Read(input, stage, SnailsGame.Ek);
            return stage;
        }
    }
}
