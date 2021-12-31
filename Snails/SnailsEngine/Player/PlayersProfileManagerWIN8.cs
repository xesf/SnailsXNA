using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;

namespace TwoBrainsGames.Snails.Player
{
    class PlayersProfileManagerWIN8 : PlayersProfileManager
    {
        public override void Save()
        {
            DataFile dataFile = GetDataFileToSave();
            IDataFileWriter writer = new BinaryDataFileWriter();
            writer.Write(SAVE_FILENAME, dataFile);
        }

        public override void BeginLoad()
        {
            Load();
        }

        public override void Load()
        {
            IDataFileReader reader = new BinaryDataFileReader();
            DataFile dataFile = reader.Read(SAVE_FILENAME);
            LoadDataFile(dataFile);

            LoadProfile();
        }
    }
}
