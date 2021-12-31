using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.BrainEngine.Data.Content
{
    public class GenericContentReader
    {
        public static void Read(ContentReader input, IDataFileSerializable contentObject, string encryptionKey)
        {
            BinaryDataFileReader reader = new BinaryDataFileReader();
            DataFile dataFile = reader.Read(input.BaseStream, encryptionKey);

            contentObject.InitFromDataFileRecord(dataFile.RootRecord);
        }
    }
}
