using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;

using TImport = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile;

namespace Snails.ContentExtension.StageDataContent
{
    [ContentImporter(".sd", DisplayName = "StageData importer", DefaultProcessor = "StageDataProcessor")]
    public class StageDataImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            XmlDataFileReader reader = new XmlDataFileReader();
            DataFile dataFile = reader.Read(filename);
            return dataFile;
        }
    }
}
