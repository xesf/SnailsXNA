using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using TRead = TwoBrainsGames.BrainEngine.Resources.SpriteSet;


namespace TwoBrainsGames.BrainEngine.Data.Content
{
    public class SpriteSetReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            SpriteSet spriteSet = new SpriteSet();
            GenericContentReader.Read(input, spriteSet, BrainGame.Ek);
            spriteSet.LoadContent(input.ContentManager);
            return spriteSet;
        }
    }
}
