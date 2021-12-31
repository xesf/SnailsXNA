using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using LevelEditor;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.Stages;
#if SAVE_XML
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
#endif
namespace TwoBrainsGames.Snails.StageEditor
{
    public class EditorCustomStage : EditorStage
    {
#region events
        public event EventHandler ThemeChanged;
#endregion

        [Browsable(true)]
        public ThemeType Theme
        {
            get
            {
                if (this.Stage == null ||
                    this.Stage.LevelStage == null)
                {
                    return ThemeType.None;
                }
                return this.Stage.LevelStage.ThemeId;
            }
            set
            {
                if (this.Stage == null ||
                    this.Stage.LevelStage == null)
                {
                    return;
                }
                if (value != this.Stage.LevelStage.ThemeId &&
                    this.ThemeChanged != null)
                {
                    this.Changed = (this.Stage.LevelStage.ThemeId != value);
                    this.Stage.LevelStage.ThemeId = value;
                    this.ThemeChanged(this, new EventArgs());
                }
            }
        }
        [Browsable(true)]
        public string Filename
        {
            get { return this.Stage.LevelStage.CustomStageFilename; }
        }

        /// <summary>
        /// 
        /// </summary>
        public EditorCustomStage(Solution parentSolution) :
            base(parentSolution)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static LevelStage GetLevelStageFromFile(string filename)
        {
            IDataFileReader reader = null;
#if SAVE_XML
            reader = new XmlDataFileReader();
#else
            reader = new BinaryDataFileReader();
#endif
            DataFile dataFile = reader.Read(filename);
            LevelStage levelStage = Stage.GetLevelStageFromDataFileRecord(dataFile.RootRecord);
            levelStage.CustomStageFilename = filename;
            if (levelStage.IsCustomStage)
            {
                levelStage.StageNr = Levels.CUSTOM_STAGE_NR;
            }
            return levelStage;
        }

        /// <summary>
        /// 
        /// </summary>
        public static EditorCustomStage CreateNew(LevelStage levelStage, Solution parentSolution)
        {
            EditorCustomStage stage = new EditorCustomStage(parentSolution);
            stage.Stage = new CustomStage();
            stage.Stage.Id = levelStage.StageId;
            stage.Stage.LevelStage = levelStage;
            stage.Stage.LevelStage.StageKey = Guid.NewGuid().ToString();
            stage.Stage.LevelStage.StageNr = Levels.CUSTOM_STAGE_NR;
            stage.Stage.LevelStage.IsCustomStage = true;

            // Setup background layers
            stage.Stage.SetLayers(Settings.GetDefaultBackgroundLayers(levelStage.ThemeId));
            stage.Save(false);
            return stage;
        }

        /// <summary>
        /// 
        /// </summary>
        public static EditorCustomStage FromFile(string filename, Solution parentSolution)
        {
            EditorCustomStage editorStage = new EditorCustomStage(parentSolution);

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

            editorStage.Stage = stage;
            editorStage.Stage.LevelStage = editorStage.Stage.LevelStage;
            editorStage.Name = editorStage.LevelStage.StageId;
            return editorStage;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Save(bool performValidation)
        {
            ((CustomStage)this.Stage).Save();
        }

    }
}
