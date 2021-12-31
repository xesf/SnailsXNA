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

namespace Snails.ContentExtension.XDFContent
{
    [ContentImporter(".xdf", DisplayName = "Xml DataFile Importer", DefaultProcessor = "XDFProcessor")]
    public class XDFImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            XmlDataFileReader reader = new XmlDataFileReader();
            DataFile dataFile = (DataFile)reader.Read(filename);
            return dataFile;
        }
    }
}
