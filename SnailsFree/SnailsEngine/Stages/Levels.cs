using System.Collections.Generic;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.ToolObjects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.UI.Screens;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.IO;
using TwoBrainsGames.Snails.Configuration;
using Microsoft.Xna.Framework;
#if XMLDATAFILE_SUPPORT
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
#endif
#if STAGE_EDITOR
using TwoBrainsGames.Snails.StageEditor;
#endif

namespace TwoBrainsGames.Snails.Stages
{
  

    public class LevelStage
    {
        public string Theme { get { return this.ThemeId.ToString(); } }
        public string StageId;
        public string StageKey;
        public ThemeType ThemeId;
        public int StageNr;
        // This data is doubled from Stage.cs
        // This is needed because when selecting the stage we have to load this info from all stages
        // Levels already do this so we are taking advantage from that (it was not good to load all stages to get this data)
        // A tool should be made to replicate this data from each stage into levels.xdf
        public int _snailsToSave;     // Snails to save
        public int _snailsToRelease;  // Total snails to be released
        public TimeSpan _targetTime;  // Target time to beat the stage
        public TimeSpan _goldMedalTime;
        public int _goldMedalScore;
        public GoalType _goal; // Stage goal
        public bool AvailableInDemo{ get; set;}
        public bool IsCustomStage { get; set;}
        public string CustomStageFilename { get; set; }
        public string YouTubeUrl { get; set; }

        public override string ToString()
        {
            return string.Format("Theme:{0},Id:{1}", this.Theme, this.StageId);
        }

        public static LevelStage CreateForCustomStage(ThemeType theme, string id)
        {
            LevelStage levelStage = new LevelStage();
            levelStage.StageId = id;
            levelStage.IsCustomStage = true;
            levelStage.StageKey = null;
            levelStage.ThemeId = theme;
            levelStage.AvailableInDemo = true;
            return levelStage;
        }
    }

    public class ThemeSettings
    {
        public ThemeType _themeType;
        public StageSound _sounds;
        public Color _shadowColor;
        public bool _visible;
    }

    public class Levels : IBrainComponent, ISnailsDataFileSerializable, IAsyncOperation
    {

        #region Constants
        public const int CUSTOM_STAGE_NR = 9999;
        private const int NUM_THEMES = 4;
        #endregion

        #region Members
        public static Levels _instance;
        public static ThemeType CurrentTheme = ThemeType.ThemeA;
        public static LevelStage CurrentLevelStage;

        public static int CurrentStageNr = 1; // Stage number inside the theme (goes from 1 to MAX_NUMBER_STAGES_PER_THEME)
        public static string CurrentCustomStageFilename;

        private List<LevelStage> _stages = new List<LevelStage>();
        private List<LevelStage> []  _stagesByTheme;
        private StageData _stageData;
        private Stage _stage;
        private string _previousTheme = string.Empty;
        private string _currentStageResourceId;
        ResourceManager _stageResourceManager;
        private SpriteBatch _spriteBatch;
        private StageSound _currentStageSound;
        private ThemeSettings[] _themeSettings;
        public static ThemeSettings CurrentThemeSettings;
        #endregion

        #region Properties
        public int StageCount { get { return this.Stages.Count; } }
        public int ThemeAStageCount { get { return this.GetStagesCountForTheme(ThemeType.ThemeA); } }
        public int ThemeBStageCount { get { return this.GetStagesCountForTheme(ThemeType.ThemeB); } }
        public int ThemeCStageCount { get { return this.GetStagesCountForTheme(ThemeType.ThemeC); } }
        public int ThemeDStageCount { get { return this.GetStagesCountForTheme(ThemeType.ThemeD); } }
        public int MaxStagesPerTheme
        {
            get
            {
                return Math.Max(this.ThemeAStageCount, Math.Max(ThemeBStageCount, ThemeCStageCount));
            }
        }
        public static ThemeSettings[] ThemeSettings { get { return Levels._instance._themeSettings; } }
        public Stage Stage
        {
            get { return _stage; }
        }

        public StageData StageData
        {
            get { return _stageData; }
        }

        public List<LevelStage> Stages
        {
            get { return _stages; }
            set { _stages = value; }
        }

        public List<LevelStage> [] StagesByTheme
        {
            get { return _stagesByTheme; }
            set { _stagesByTheme = value; }
        }

        public int StageIdx { get; private set; }
        public LevelStage FirstLevelStage { get { return _stages[0]; } }
        internal static Levels CurrentLevel
        {
            get { return _instance; }
        }

        public StageSound StageSound
        {
            get { return _currentStageSound; }
        }
        #endregion

		public Levels()
        {
            if (Levels._instance != null)
            {
                throw new SnailsException("Ups! Levels is a singleton, why create two instances!? I had problems with this, so I've added this exception.");
            }
            _instance = this;
            this._stageResourceManager = BrainGame.CreateResourceManager();
            this._spriteBatch = new SpriteBatch(BrainGame.Graphics);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Levels Load() // I'm wondering why this needs the screen...
        {
            // Levels is a singleton, I had problems before with two instances of this
            // Just return the current instance. Levels does not change during the game
            if (Levels._instance != null)
            {
                return Levels._instance;
            }
            Levels levels = new Levels();
            DataFileRecord record = BrainGame.ResourceManager.Load<DataFileRecord>("stages\\levels", ResourceManager.ResourceManagerCacheType.Static);
            levels.InitFromDataFileRecord(record);
            levels.Initialize();
            return levels;
        }

        #region IBrainComponent Members
       
        public SpriteBatch SpriteBatch
        {
            get 
            { 
                return this._spriteBatch; 
            }
        }

        public void Initialize()
        {
            //this.StageIdx = 0;
            //Levels.StartupStageNr = 1;
        }

     

        /// <summary>
        /// 
        /// </summary>
        public void HandleEvents(BrainGameTime gameTime)
        {
           _stage.HandleEvents(gameTime);
        }

        public void Update(BrainGameTime gameTime)
        {
           _stage.Update(gameTime);
        }

        public void Draw()
        {
            // WARNING: if a sprite draw is requested in this stage a new SpriteBatch Begin/End
            // must be added before or after Stage draw. Stage uses SpriteBatch inside

            _stage.DrawWaterTexture(); // FIXME fix this while returning from InGame Menu
            _stage.Draw();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnloadContent()
        {
            BrainGame.ResourceManager.Unload(ResourceManagerIds.STAGE_THEME_RESOURCES);
            BrainGame.ResourceManager.Unload(ResourceManagerIds.TUTORIAL_RESOURCES);
            this.UnloadCurrentStage();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UnloadCurrentStage()
        {
            this._currentStageResourceId = null;
            if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.UnloadContent();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this.ReloadStageData();

            _currentStageSound = this._themeSettings[(int)Levels.CurrentTheme]._sounds;
            _currentStageSound.Initialize();
            _currentStageSound.LoadContent();

        }

        /// <summary>
        /// 
        /// </summary>
        public void ReloadStageData()
        {
            _stageData = BrainGame.ResourceManager.Load<StageData>("stages/stagedata", ResourceManagerIds.STAGE_THEME_RESOURCES);
            _stageData.LoadContent(Levels.CurrentTheme);
        }
        #endregion
        public LevelStage GetCurrentStageInfo()
        {
            if (Levels.CurrentLevelStage != null &&
                Levels.CurrentLevelStage.IsCustomStage)
            {
                return Levels.CurrentLevelStage;
            }
            return this.FindStageInfo(Levels.CurrentTheme, Levels.CurrentStageNr);
        }

        public LevelStage GetNextStageInfo()
        {
            // No next stage on custom stages
            if (this._stage.LevelStage.IsCustomStage)
            {
                return null;
            }

            ThemeType nextTheme = Levels.CurrentTheme;
            int nextStage = Levels.CurrentStageNr + 1;

            if (nextStage > this.GetStagesCountForTheme(Levels.CurrentTheme))
            {
                nextStage = 1;
                nextTheme = (ThemeType)((int)Levels.CurrentTheme + 1);
                if (nextTheme > ThemeType.ThemeC)
                {
                    nextTheme = ThemeType.ThemeA;
                }
            }

            return this.FindStageInfo(nextTheme, nextStage);
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetStagesCountForTheme(ThemeType theme)
        {
            if (Levels._instance._stagesByTheme[(int)theme] == null)
            {
                return 0;
            }
            return (Levels._instance._stagesByTheme[(int)theme].Count);
        }


        public void StartNextStage()
        {
            // Review this Levels static vars
            Levels.CurrentStageNr++;
            if (Levels.CurrentStageNr > this.GetStagesCountForTheme(Levels.CurrentTheme))
            {
                Levels.CurrentStageNr = 1;
                Levels.CurrentTheme = (ThemeType)((int)Levels.CurrentTheme + 1);
                if (Levels.CurrentTheme > ThemeType.ThemeD)
                {
                    Levels.CurrentTheme = ThemeType.ThemeA;
                }
            }
            LoadStage(Levels.CurrentTheme, Levels.CurrentStageNr);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartPrevStage()
        {
            Levels.CurrentStageNr--;
            if (Levels.CurrentStageNr <= 0)
            {
                Levels.CurrentStageNr = 1;
                Levels.CurrentTheme = (ThemeType)((int)Levels.CurrentTheme - 1);
                if ((int)Levels.CurrentTheme < 0)
                {
                    Levels.CurrentTheme = ThemeType.ThemeA;
                }

            }

            LoadStage(Levels.CurrentTheme, Levels.CurrentStageNr);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadStage(ThemeType theme, int stageNr)
        {
#if STAGE_EDITOR
            if (!string.IsNullOrEmpty(Levels.CurrentCustomStageFilename))
            {
                this.LoadCustomStage(Levels.CurrentCustomStageFilename, Snails.Stages.Stage.StageLoadingContext.Gameplay);
                return;
            }
#endif
            LevelStage stage = this.FindStageInfo(theme, stageNr);
            this.LoadStage(stage.StageId, Stage.StageLoadingContext.Gameplay);
        }

        /// <summary>
        /// if useResourceManager is false, the stage is loaded directly from disk using datafiles
        /// This is needed in the stage editor to load the stage without the need to recompile
        /// In this case, contentPath is the path where the file can be found
        /// </summary>
        public Stage LoadStage(string stageId, Stage.StageLoadingContext loadingContext)
        {
            Stage.LoadingContext = loadingContext; // Store the context where the stage is being loaded
                                                   // This is because there might be somethings that we don't want to be made
                                                   // when loading the stage from the editor.
                                                   // Adding tiles for example in object initialization. We don't want this to be added
                                                   // in the editor because those would be stored in the stage itself

            this.StageIdx = FindStageIndexById(stageId);
            if (this.StageIdx == -1)
            {
                this.StageIdx = 0;
            }
            LevelStage levelStage = this._stages[this.StageIdx];
            Levels.CurrentTheme = levelStage.ThemeId;
            Levels.CurrentStageNr = levelStage.StageNr;
            Levels.CurrentThemeSettings = this._themeSettings[(int)Levels.CurrentTheme];
            if (_previousTheme != levelStage.Theme) // if theme changed, than unload all resources from previous theme
            {
                if (!string.IsNullOrEmpty(_previousTheme))
                {
                    UnloadContent();
                }

                this.LoadContent();

                _previousTheme = levelStage.Theme;
            }

            this.UnloadCurrentStage();
            this._currentStageResourceId = string.Format(Stage.STAGES_ID, levelStage.Theme, levelStage.StageId);
            DataFileRecord record;
 
#if STAGE_EDITOR
            // When the stage editor is active, stages are always loaded directly from the disc
            XmlDataFileReader reader = new XmlDataFileReader();
            string path = System.IO.Path.Combine(GameSettings.StagesOutputFolder, this._currentStageResourceId.Replace("/", "\\") + ".xdf");
            if (File.Exists(path))
            {
                DataFile dataFile = reader.Read(path);
                record = dataFile.RootRecord;
            }
            else
            {
                record = BrainGame.ResourceManager.Load<DataFileRecord>(this._currentStageResourceId, ResourceManager.ResourceManagerCacheType.Temporary);
            }
#else
            record = BrainGame.ResourceManager.Load<DataFileRecord>(this._currentStageResourceId, ResourceManager.ResourceManagerCacheType.Temporary);
#endif
            this._stage = new Stage(levelStage);
            this._stage.InitFromDataFileRecord(record);
            this._stage.Initialize();
            this._stage.LoadContent();
            this._stage.LevelStage.StageNr = levelStage.StageNr;
            Levels.CurrentLevelStage = this._stage.LevelStage;
            // Don't check the security key if we are loading from the stage editor
            if (loadingContext == Snails.Stages.Stage.StageLoadingContext.Gameplay)
            {
                // Check security key
                if (this._stage.Key != levelStage.StageKey)
                {
                    throw new SnailsException("Unable to load stage."); // Just throw some generic error. If this crashed after closing the stage editor, just re run and this will be ok.
                                                                        // This crashes in stages that don't already have a key 
                }
            }

            return this._stage;
        }
#if STAGE_EDITOR
        /// <summary>
        ///
        /// </summary>
        public CustomStage LoadCustomStage(string filename, Stage.StageLoadingContext loadingContext)
        {
            Stage.LoadingContext = loadingContext; // Store the context where the stage is being loaded
            LevelStage levelStage = EditorCustomStage.GetLevelStageFromFile(filename);
            Levels.CurrentTheme = levelStage.ThemeId;
            Levels.CurrentStageNr = levelStage.StageNr;
            if (this._previousTheme != levelStage.Theme) // if theme changed, than unload all resources from previous theme
            {
                if (!string.IsNullOrEmpty(this._previousTheme))
                {
                    this.UnloadContent();
                }
                this.LoadContent();
                this._previousTheme = levelStage.Theme;
            }

            this.UnloadCurrentStage();
            CustomStage stage = CustomStage.FromFile(filename);
            this._stage = (Stage)stage;
            this._stage.Initialize();
            this._stage.LoadContent();
            Levels.CurrentLevelStage = this._stage.LevelStage;

            return stage;
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        public int FindStageIndexById(string id)
        {
            for (int i = 0; i < this._stages.Count; i++)
            {
                if (this._stages[i].StageId == id)
                {
                    return i;
                }
            }
            return -1;
        }
      

        /// <summary>
        /// 
        /// </summary>
        public LevelStage FindStageInfo(ThemeType theme, int stageNr)
        {
            for (int i = 0; i < this._stages.Count; i++)
            {
                if (this._stages[i].StageNr == stageNr &&
                    this._stages[i].ThemeId == theme)
                {
                    return this._stages[i];
                }
            }
            throw new SnailsException("Stage with theme [" + theme.ToString() + "] and stageNr[" + stageNr.ToString() + "] not found.");
        }
        
        /// <summary>
        /// 
        /// </summary>
        public LevelStage GetLevelStage(ThemeType theme, int nr)
        {
            for (int i = 0; i < this._stages.Count; i++)
            {
                if (this._stages[i].ThemeId == theme &&
                    this._stages[i].StageNr == nr)
                {
                    return this._stages[i];
                }
            }
            throw new SnailsException("LevelStage not found [theme=" + theme.ToString() + "] [stageNr=" + nr .ToString() + "]");
        }

        /// <summary>
        /// 
        /// </summary>
        public LevelStage GetFirstLevelStageFromTheme(ThemeType theme)
        {
            foreach (LevelStage levelStage in this._stages)
            {
                if (levelStage.ThemeId == theme)
                {
                    return levelStage;
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public LevelStage GetLevelStage(int stageIdx)
        {
            return (this._stages[stageIdx]);
        }

        /// <summary>
        /// 
        /// </summary>
        public LevelStage GetLevelStage(string stageId)
        {
            foreach (LevelStage stage in this._stages)
            {
                if (stage.StageId == stageId)
                {
                    return stage;
                }
            }
            return null;
        }

        public int GetStageIndex(LevelStage levelStage)
        {
            for(int i = 0; i < this.Stages.Count; i++)
            {
                if (this.Stages[i].StageKey == levelStage.StageKey)
                {
                    return i;
                }
            }
            throw new SnailsException("Stage Key=[" + levelStage.StageKey + "] not found in stage list.");
        }



        /// <summary>
        /// 
        /// </summary>
        public LevelStage GetNextStage(LevelStage levelStage)
        {
            if (levelStage == null)
            {
                // Return the first entry. This is usefull in PlayerStats.UnlockFirstStage()
                return this._stages[0];
            }
            if (levelStage.IsCustomStage)
            {
                return null;
            }
            int stageIdx = GetStageIndex(levelStage) + 1; // because its zero-based
            if (stageIdx >= this.StageCount)
            {
                return null;
            }
            return this._stages[stageIdx];
        }

        /// <summary>
        /// 
        /// </summary>
        public static Levels FromDataFileRecord(DataFileRecord record)
        {
            Levels levels = new Levels();
            levels.InitFromDataFileRecord(record);
            return levels;
        }


        /// <summary>
        /// Checks how many stages in "theme" are needed to unlock "themeToUnLock"
        /// </summary>
        /// <returns></returns>
        public static int UnlockedStagesNeeded(ThemeType themeToUnlock, ThemeType theme)
        {
            switch (themeToUnlock)
            {
                case ThemeType.ThemeA:
                    return 0; // No stages needed for all themes
                case ThemeType.ThemeB:
                    if (theme == ThemeType.ThemeA)
                    {
                        return 5; // Stages needed in "The Garden"
                    }
                    return 0; // No stages needed for the other themes

                case ThemeType.ThemeC:
                    switch (theme)
                    {
                        case ThemeType.ThemeA:
                            return 9;
                        case ThemeType.ThemeB:
                            return 5;
                    }
                    return 0; // No stages needed for the other themes

            /*    case ThemeType.ThemeD:
                    switch (theme)
                    {
                        case ThemeType.ThemeA:
                            return 17;
                        case ThemeType.ThemeB:
                            return 13;
                        case ThemeType.ThemeC:
                            return 9;
                    }
                    return 0; // No stages needed for the other themes*/
            }

            return 0;
        }



#if XMLDATAFILE_SUPPORT
        /// <summary>
        /// This method saves the leves xdf file
        /// </summary>
        public void Save(string filename)
        {
            XmlDataFileWriter writer = new XmlDataFileWriter();
            DataFile levelsDataFile = new DataFile();
            levelsDataFile.RootRecord = this.ToDataFileRecord(ToDataFileRecordContext.StageSave);
            writer.Write(filename, levelsDataFile);
        }
#endif

        /// <summary>
        /// Returns true if all stages in the theme are locked in the demo 
        /// </summary>
        public bool IsLockedInDemo(ThemeType theme)
        {
            foreach (LevelStage stage in this._stagesByTheme[(int)theme])
            {
                if (stage.AvailableInDemo)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static ThemeSettings GetThemeSettings(ThemeType theme)
        {
            if (Levels._instance._themeSettings == null)
            {
                return null;
            }

            for (int i = 0; i < Levels._instance._themeSettings.Length; i++)
            {
                if (Levels._instance._themeSettings[i]._themeType == theme)
                {
                    return Levels._instance._themeSettings[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public static int GetTotalVisibleThemes()
        {
            int visible = 0;
            for (int i = 0; i < Levels._instance._themeSettings.Length; i++)
            {
                if (Levels._instance._themeSettings[i]._visible)
                {
                    visible++;
                }
            }

            return visible;

        }

        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._stagesByTheme = new List<LevelStage>[NUM_THEMES];
            this._themeSettings = new ThemeSettings[NUM_THEMES];

            DataFileRecordList levelRecords = record.SelectRecords("Level");

            foreach (DataFileRecord levelRecord in levelRecords)
            {
                int idx = 1; // stage index always start from 1 in each theme
                string themeId = levelRecord.GetFieldValue<string>("theme");
                ThemeType theme = (ThemeType)Enum.Parse(typeof(ThemeType), themeId, true);
                this._stagesByTheme[(int)theme] = new List<LevelStage>();
                this._themeSettings[(int)theme] = new ThemeSettings();
                this._themeSettings[(int)theme]._sounds = new StageSound();
                DataFileRecord soundRecord = levelRecord.SelectRecord("Sound");
                if (soundRecord != null)
                {
                    this._themeSettings[(int)theme]._sounds.InitFromDataFileRecord(soundRecord);
                }
                this._themeSettings[(int)theme]._shadowColor = levelRecord.GetFieldValue<Color>("shadowColor");
                this._themeSettings[(int)theme]._themeType = theme;
                this._themeSettings[(int)theme]._visible = levelRecord.GetFieldValue<bool>("visible"); ;

                DataFileRecordList stageRecords = levelRecord.SelectRecords("Stages\\Stage");
                foreach (DataFileRecord stageRecord in stageRecords)
                {
                    LevelStage stage = new LevelStage();
                    stage.ThemeId = theme;
                    stage.StageId = stageRecord.GetFieldValue<string>("id");
                    stage.StageKey = stageRecord.GetFieldValue<string>("key");
                    stage._snailsToSave = stageRecord.GetFieldValue<int>("snailsToSave");
                    stage._snailsToRelease = stageRecord.GetFieldValue<int>("snailsToRelease");
                    stage._targetTime = stageRecord.GetFieldValue<TimeSpan>("targetTime");
                    stage._goldMedalTime = stageRecord.GetFieldValue<TimeSpan>("goldMedalTime");
                    stage._goldMedalScore = stageRecord.GetFieldValue<int>("goldMedalScore");
                    stage._goal = (GoalType)stageRecord.GetFieldValue<int>("goal");
                    stage.AvailableInDemo = stageRecord.GetFieldValue<bool>("availableInDemo", false);
                    stage.YouTubeUrl = stageRecord.GetFieldValue<string>("youTubeUrl", null);
                    stage.StageNr = idx++;
                    _stages.Add(stage);
                    this._stagesByTheme[(int)theme].Add(stage);
                }
            }
        }
		/// <summary>
		/// 
		/// </summary>
		public DataFileRecord ToDataFileRecord()
		{
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
		}

		/// <summary>
		/// 
		/// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
		{
            List<string> themes = new List<string>();

            string auxTheme = null;
            foreach (LevelStage stage in _stages)
            {
                if (auxTheme != stage.Theme)
                {
                    themes.Add(stage.Theme);
                    auxTheme = stage.Theme;
                }
            }

            DataFileRecord record = new DataFileRecord("Levels");

            foreach (ThemeSettings theme in this._themeSettings)
            {
                DataFileRecord themeRecord = new DataFileRecord("Level");
                themeRecord.AddField("theme", theme._themeType.ToString());
                themeRecord.AddField("shadowColor", theme._shadowColor);
                themeRecord.AddField("visible", theme._visible);

                themeRecord.AddRecord(theme._sounds.ToDataFileRecord());

                DataFileRecord stagesRecord = new DataFileRecord("Stages");
                foreach (LevelStage stage in _stages)
                {
                    if (stage.ThemeId != theme._themeType)
                    {
                        continue;
                    }


                    DataFileRecord stageRecord = new DataFileRecord("Stage");
                    stageRecord.AddField("id", stage.StageId);
                    if (stage.StageKey != null)
                    {
                        stageRecord.AddField("key", stage.StageKey);
                    }
                    stageRecord.AddField("snailsToSave", stage._snailsToSave);
                    stageRecord.AddField("snailsToRelease", stage._snailsToRelease);
                    stageRecord.AddField("targetTime", stage._targetTime);
                    stageRecord.AddField("goal", (int)stage._goal);
                    stageRecord.AddField("availableInDemo", stage.AvailableInDemo);
                    stageRecord.AddField("goldMedalTime", stage._goldMedalTime);
                    stageRecord.AddField("goldMedalScore", stage._goldMedalScore);
                    stageRecord.AddField("youTubeUrl", stage.YouTubeUrl);
                    stagesRecord.AddRecord(stageRecord);
                }
                themeRecord.AddRecord(stagesRecord);

                record.AddRecord(themeRecord);
            }

            return record;
        }

        #endregion

	    #region IAssyncOperation
        /// <summary>
        /// 
        /// </summary>
	    public void BeginLoad()
        {
            this.LoadStage(Levels.CurrentTheme, Levels.CurrentStageNr);
        }

        public object AsyncLoadingParams
        {
            set { }
        }
	    #endregion
    }
}
