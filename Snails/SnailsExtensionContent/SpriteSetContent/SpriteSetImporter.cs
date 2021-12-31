using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

using TImport = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;

namespace Snails.ContentExtension.SpriteSetContent
{
    [ContentImporter(".ss", DisplayName = "SpriteSet Importer", DefaultProcessor = "SpriteSetProcessor")]
    public class SpriteSetImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            XmlDataFileReader reader = new XmlDataFileReader();
            DataFile dataFile = reader.Read(filename);
            return dataFile;
        }
    }
}
