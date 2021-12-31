using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;

namespace TwoBrainsGames.Snails.Player
{
    class PlayersProfileManagerWP7 : PlayersProfileManager
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
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IDataFileReader reader = new BinaryDataFileReader();

            if (storage.FileExists(SAVE_FILENAME))
            {
                try
                {
                    DataFile dataFile = reader.Read(SAVE_FILENAME);
                    LoadDataFile(dataFile);
                }
                catch(System.Exception ) // Ignorar erros load (estava a dar um DataFileFormaException em PRD)
                {                        // Dá erro, cria novo...
                }
            }

            LoadProfile();
        }
    }
}
