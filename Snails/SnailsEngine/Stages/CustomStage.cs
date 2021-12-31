using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
using TwoBrainsGames.Snails.Configuration;
#if XMLDATAFILE_SUPPORT
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
#endif

namespace TwoBrainsGames.Snails.Stages
{
    public class CustomStage : Stage, IDataFileSerializable
    {
        public ThemeType Theme { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomStage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        { 
            // Update LevelStage
            this.LevelStage._snailsToRelease = this.GetTotalSnailsToRelease();
            this.IncrementBuildNr();
            DataFile dataFile = new DataFile();
            dataFile.RootRecord = this.ToDataFileRecord(ToDataFileRecordContext.StageSave);
            IDataFileWriter writer = null;
#if SAVE_XML
            writer = new XmlDataFileWriter();
#else
            writer = new BinaryDataFileWriter();
#endif
            writer.Write(this.LevelStage.CustomStageFilename, dataFile);
        }

        /// <summary>
        /// 
        /// </summary>
        public static CustomStage FromFile(string filename)
        {
            CustomStage stage = new CustomStage();

            IDataFileReader reader = null;
#if SAVE_XML
            reader = new XmlDataFileReader();
#else
            reader = new BinaryDataFileReader();
#endif
            DataFile dataFile = reader.Read(filename);
            stage.InitFromDataFileRecord(dataFile.RootRecord);
            stage.LevelStage.CustomStageFilename = filename;
            return stage;
        }


        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            return base.ToDataFileRecord(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public static string BuildFilename(string stageId)
        {
            return Path.Combine(SnailsGame.GameSettings.CustomStagesFolder, stageId + "." + GameSettings.CustomStagesExtension);
        }


        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.LevelStage.IsCustomStage = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return base.ToDataFileRecord();
        }

        #endregion
    }
}
