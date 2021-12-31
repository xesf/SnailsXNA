using Microsoft.Xna.Framework.Content;

using TRead = TwoBrainsGames.Snails.Tutorials.Tutorial;
using TwoBrainsGames.Snails.Tutorials;
using TwoBrainsGames.Snails;

namespace TwoBrainsGames.BrainEngine.Data.Content
{

    public class TutorialReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            Tutorial tutorial = new Tutorial();
            GenericContentReader.Read(input, tutorial, SnailsGame.Ek);
            return tutorial;
        }
    }
}
