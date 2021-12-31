using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Data.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;

using TRead = TwoBrainsGames.BrainEngine.Resources.TextFont;

namespace TwoBrainsGames.BrainEngine.Data.Content
{
    public class TextFontReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            TextFont font = new TextFont();
            GenericContentReader.Read(input, font, BrainGame.Ek);
            font.LoadContent(input.ContentManager);
            return font;
        }
    }
}
