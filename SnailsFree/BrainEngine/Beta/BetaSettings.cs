using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
using System.IO;

namespace TwoBrainsGames.BrainEngine.Beta
{
    public class BetaSettings
    {
        private static string Filename
        {
            get {
                return Path.Combine(BrainGame.GameUserFolderName, "BetaSettings.bdf");
            }
        }

        public static bool ShouldShowQueryBetaForm { get; set;}
        public static bool IsBetaTester { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public static void Load()
        {
            BetaSettings.IsBetaTester = true;
            BetaSettings.ShouldShowQueryBetaForm = true;
            if (!File.Exists(BetaSettings.Filename))
            {
                return;
            }
            BinaryDataFileReader reader = new BinaryDataFileReader();

            DataFile dataFile = reader.Read(BetaSettings.Filename);
            BetaSettings.IsBetaTester = dataFile.RootRecord.GetFieldValue<bool>("isBetaTester", BetaSettings.IsBetaTester);
            BetaSettings.ShouldShowQueryBetaForm = dataFile.RootRecord.GetFieldValue<bool>("shouldShowQueryBetaForm", BetaSettings.ShouldShowQueryBetaForm);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Save()
        {
            BinaryDataFileWriter writer = new BinaryDataFileWriter();

            DataFile dataFile = new DataFile();
            dataFile.RootRecord = new DataFileRecord("BetaSettings");
            dataFile.RootRecord.AddField("isBetaTester", BetaSettings.IsBetaTester);
            dataFile.RootRecord.AddField("shouldShowQueryBetaForm", BetaSettings.ShouldShowQueryBetaForm);
            writer.Write(BetaSettings.Filename, dataFile);
       }
    }
}
