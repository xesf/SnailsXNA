using System.IO;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
#if SAVE_XML
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
#else
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
#endif

namespace TwoBrainsGames.Snails.Player
{
    class PlayersProfileManagerCROSS : PlayersProfileManager
    {
        protected override string GetUserFilename()
        {
            return Path.Combine(BrainGame.GameUserFolderName, SAVE_FILENAME);
        }

        public override void Save()
        {
#if !PSMobile			
            string filename = GetUserFilename();
            IDataFileWriter writer = null;
            DataFile dataFile = GetDataFileToSave();
            string k = null;
#if SAVE_XML
            writer = new XmlDataFileWriter();
#else
            writer = new BinaryDataFileWriter();
            k = SnailsGame.Ek;
#endif
            writer.Write(filename, dataFile, k);
#endif
        }

        public override void BeginLoad()
        {
            Load();
        }

        public override void Load()
        {
#if !WIN8
            string filename = GetUserFilename();
            IDataFileReader reader = null;
            string k = null;
#if SAVE_XML
            reader = new XmlDataFileReader();
#else
            reader = new BinaryDataFileReader();
            k = SnailsGame.Ek;
#endif
            if (File.Exists(filename))
            {
                DataFile dataFile = reader.Read(filename, k);
                LoadDataFile(dataFile);
            }
#endif

            LoadProfile();
        }
    }
}
