using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using TImport = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace Snails.ContentExtension.TutorialContent
{
    [ContentImporter(".tut", DisplayName = "Tutorial Importer", DefaultProcessor = "TutorialProcessor")]
    public class TutorialImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            XmlDataFileReader reader = new XmlDataFileReader();
            DataFile dataFile = reader.Read(filename);
            return dataFile;
        }
    }
}
