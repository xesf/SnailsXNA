using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Player;

namespace TwoBrainsGames.Snails.Player
{
    public class PlayerStats : IDataFileSerializable
    {
     

        #region Members
        protected List<PlayerStageStats> _stagesStat;
        long _tutorialToolsFlags = 0;   // bitwise - 0 or 1 (if displayed)

        // totals of all times
        // this stats are increment counters and we should add all those infos for each stage played
        protected TimeSpan _totalPlayingTime; // all the time playing the game (will only count stages times)
        protected TimeSpan _totalRunningTime; // all the time running the game (playing or idle)
        protected int _totalSnailsSafe; // reach the exit of stage
        protected int _totalSnailsKingSafe; // total snails king saved
        // total snails deads
        protected int _totalSnailsDeadByFire;
        protected int _totalSnailsDeadBySpikes;
        protected int _totalSnailsDeadByDynamite;
        protected int _totalSnailsDeadByCrate;
        protected int _totalSnailsDeadByWater;
        protected int _totalSnailsDeadByAcid;
        protected int _totalSnailsDeadByLaser;
        protected int _totalSnailsDeadBySacrifice;
        protected int _totalSnailsDeadByCrateExplosion;
        protected int _totalSnailsDeadByOutOfStage;
        protected int _totalSnailsDeadByEvilSnail;
        // total coins grabbed
        protected int _totalGoldMedals;
        protected int _totalSilverMedals;
        protected int _totalBronzeMedals;
        // others
        protected int _totalBoots;
        #endregion

        #region Properties
        public List<PlayerStageStats> StagesStat
        {
            get { return _stagesStat; }
            set { _stagesStat = value; }
        }

        public TimeSpan TotalPlayingTime
        {
            get { return _totalPlayingTime; }
            set { _totalPlayingTime = value; }
        }

        public TimeSpan TotalRunningTime
        {
            get { return _totalRunningTime; }
            set { _totalRunningTime = value; }
        }

        public int TotalSnailsSafe
        {
            get { return _totalSnailsSafe; }
            set 
            { 
                _totalSnailsSafe = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.SafelyEscort100Snails);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.SafelyEscort300Snails);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.SafelyEscort600Snails);
            }
        }

        public int TotalSnailsKingSafe
        {
            get { return _totalSnailsKingSafe; }
            set
            {
                _totalSnailsKingSafe = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Save5SnailsKing);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Save10SnailsKing);
            }
        }

        public int TotalSnailsDead
        {
            get 
            {
                return _totalSnailsDeadByFire +
                       _totalSnailsDeadBySpikes +
                       _totalSnailsDeadByDynamite +
                       _totalSnailsDeadByCrate +
                       _totalSnailsDeadByWater +
                       _totalSnailsDeadByLaser + 
                       _totalSnailsDeadByAcid +
                       _totalSnailsDeadByCrateExplosion +
                       _totalSnailsDeadByOutOfStage +
                       _totalSnailsDeadBySacrifice +
                       _totalSnailsDeadByEvilSnail; 
            }
        }

        public int SnailsDeadInDifferentWays
        {
            get
            {
                int countDeads = 0;
                if (_totalSnailsDeadByFire > 0)
                    countDeads++;
                if (_totalSnailsDeadBySpikes > 0)
                    countDeads++;
                if (_totalSnailsDeadByDynamite > 0)
                    countDeads++;
                if (_totalSnailsDeadByCrate > 0)
                    countDeads++;
                if (_totalSnailsDeadByWater > 0)
                    countDeads++;
                if (_totalSnailsDeadByLaser > 0)
                    countDeads++;
                if (_totalSnailsDeadByAcid > 0)
                    countDeads++;
                if (_totalSnailsDeadByCrateExplosion > 0)
                    countDeads++;
                if (_totalSnailsDeadByOutOfStage > 0)
                    countDeads++;
                if (_totalSnailsDeadBySacrifice > 0)
                    countDeads++;
                if (_totalSnailsDeadByEvilSnail > 0)
                    countDeads++;

                return countDeads;
            }
        }

        public int TotalSnailsDeadByFire
        {
            get { return _totalSnailsDeadByFire; }
            set 
            { 
                _totalSnailsDeadByFire = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Burn20Snails);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadBySpikes
        {
            get { return _totalSnailsDeadBySpikes; }
            set 
            { 
                _totalSnailsDeadBySpikes = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Kill20SnailsInSpikes);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadByDynamite
        {
            get { return _totalSnailsDeadByDynamite; }
            set 
            { 
                _totalSnailsDeadByDynamite = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Kill50SnailsWithDynamite);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadByCrateExplosion
        {
            get { return _totalSnailsDeadByCrateExplosion; }
            set
            {
                _totalSnailsDeadByCrateExplosion = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadByCrate
        {
            get { return _totalSnailsDeadByCrate; }
            set 
            {
                _totalSnailsDeadByCrate = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadByWater
        {
            get { return _totalSnailsDeadByWater; }
            set 
            { 
                _totalSnailsDeadByWater = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadByAcid
        {
            get { return _totalSnailsDeadByAcid; }
            set
            {
                _totalSnailsDeadByAcid = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadByLaser
        {
            get { return _totalSnailsDeadByLaser; }
            set
            {
                _totalSnailsDeadByLaser = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Kill20SnailsWithLazer);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadBySacrifice
        {
            get { return _totalSnailsDeadBySacrifice; }
            set
            {
                _totalSnailsDeadBySacrifice = value;
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadByOutOfStage
        {
            get { return _totalSnailsDeadByOutOfStage; }
            set
            {
                _totalSnailsDeadByOutOfStage = value;
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalSnailsDeadByEvilSnail
        {
            get { return _totalSnailsDeadByEvilSnail; }
            set
            {
                _totalSnailsDeadByEvilSnail = value;
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.KillSnailsIn11DifferentWays);
            }
        }

        public int TotalBronzeCoins
        {
            get { return this._totalBronzeMedals; }
            set
            {
                _totalBronzeMedals = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get50BronzeCoins);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get100BronzeCoins);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get200BronzeCoins);
            }
        }

        public int TotalSilverCoins
        {
            get { return this._totalSilverMedals; }
            set
            {
                _totalSilverMedals = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get50SilverCoins);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get100SilverCoins);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get200SilverCoins);
            }
        }

        public int TotalGoldCoins
        {
            get { return this._totalGoldMedals; }
            set
            {
                _totalGoldMedals = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get50GoldCoins);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get100GoldCoins);
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Get200GoldCoins);
            }
        }

        public int TotalBoosts
        {
            get { return this._totalBoots; }
            set 
            {
                _totalBoots = value;

                SnailsGame.AchievementsManager.Notify((int)AchievementsType.Boost50Snails);
            }
        }
        #endregion

        public PlayerStats()
        {
            // Just start with an empty list
            // When a stage is unlocked, a new entry for that stage is added
            // Stages that are on the list are unlocked, the ones that aren't are locked
            // StageId is the key for this list, a dictionary could be used, but I don't want to give the trouble...
            this.StagesStat = new List<PlayerStageStats>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnlockFirstStage(ThemeType theme)
        {
            Levels levels = Levels.Load();
            // Get the stage ID from the first stage for the theme
            LevelStage levelStage = levels.GetFirstLevelStageFromTheme(theme);
            if (levelStage != null)
            {
                this.UnlockStage(levelStage.StageId);
            }
        }

        /// <summary>
        /// Unlocks a stage
        /// Just add the stage to the list of unlocked stages 
        /// Check if the stage is no already unlocked just for safety
        /// </summary>
        public PlayerStageStats UnlockStage(string stageId)
        {
#if DEBUG
            // Martelada! em debug todos os stages esão desbloqueados, mas pode não haver o profile no user...
            // Precisamos do stageStats para o nível para poder gravar as hints vistas
            if (this.IsStageUnlocked(stageId) && this.GetStageStats(stageId) == null)
            {
                PlayerStageStats stageStats = new PlayerStageStats(stageId);
                this._stagesStat.Add(stageStats);
                return stageStats;
            }

#endif
            if (this.IsStageUnlocked(stageId) == false)
            {
                PlayerStageStats stageStats = new PlayerStageStats(stageId);
                this._stagesStat.Add(stageStats);
                return stageStats;
            }
            return this.GetStageStats(stageId);
        }

        /// <summary>
        /// Unlocks the next stage and returns the result of the unlock
        /// It may not be possible to unlock the stage when
        ///   -The stage is not available (not available in the beta for instance)
        ///   -No more stages to unlock (if the last stage in the theme for instance)
        /// </summary>
        public LevelStage UnlockNextStage(LevelStage currentLevelStage, bool unlockStage)
        {
            LevelStage nextStage = Levels.CurrentLevel.GetNextStage(currentLevelStage);
            if (nextStage == null)
            {
                return null;
            }
            if (unlockStage)
            {
                this.UnlockStage(nextStage.StageId);
            }

#if !RETAIL 
            if (!SnailsGame.GameSettings.AllStagesUnlocked)
            {
                switch (SnailsGame.GameSettings.GameplayMode)
                {
                    case Configuration.GameSettings.GameplayModeType.Beta:
                    case Configuration.GameSettings.GameplayModeType.Demo:
                        if (!nextStage.AvailableInDemo)
                        {
                            nextStage = Levels._instance.GetLevelStage(nextStage.ThemeId, Levels._instance.GetStagesCountForTheme(nextStage.ThemeId));
                            return UnlockNextStage(nextStage, false);
                        }
                        break;
                }
            }
#endif
            
            return nextStage;
        }

        /// <summary>
        /// Returns the number of stages still needed for 'theme' to unlock the 'themeToQuery' theme
        /// </summary>
        public int StagesNeededToUnlockTheme(ThemeType theme, ThemeType themeToQuery)
        {
            int totalStagesNeeded = Levels.UnlockedStagesNeeded(theme, themeToQuery);
            int totalStagesUnlocked = this.GetTotalUnlockedStagesForTheme(themeToQuery);
            int n = totalStagesNeeded - totalStagesUnlocked;
            return (n < 0 ? 0 : n);
        }

        /// <summary>
        /// (1)Instead of using the logic "if stage 1 is unlocked then theme is unlocked" logic, we will
        /// reavaluate the conditions that make the theme unlocked
        /// This is because IsThemeUnlocked is used when checking if a new theme was unlocked when moving to the next stage
        /// We could not use the logic in (1) because the reverse logic is used: if theme is unlocked, then unlock first stage
        /// </summary>
        public bool IsThemeUnlocked(ThemeType theme)
        {
#if DEBUG
            if (SnailsGame.GameSettings.AllStagesUnlocked)
            {
                return true;
            }
#endif
		  // No need to check ThemeD because ThemeD, because ThemeD doesn't depend on itself
           return (this.StagesNeededToUnlockTheme(theme, ThemeType.ThemeA) <= 0 &&
                   this.StagesNeededToUnlockTheme(theme, ThemeType.ThemeB) <= 0 &&
                   this.StagesNeededToUnlockTheme(theme, ThemeType.ThemeC) <= 0);
        }

        /// <summary>
        /// Stage is unlocked if it's on the stage stats list
        /// </summary>
        public bool IsStageUnlocked(string stageId)
        {
#if DEBUG
            if (SnailsGame.GameSettings.AllStagesUnlocked)
            {
                return true;
            }
#endif
		  // Always unlock 1st stage
            Levels._instance.GetLevelStage(stageId);

            return (this.GetStageStats(stageId) != null);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTutorialToolSet(ToolObjects.ToolObjectType toolType)
        {
            int idx = (int)toolType;

            if (_tutorialToolsFlags >> idx == 1)
                return false;

            return false;
        }

        // OLD CODE, new code again. The stage key should be stageId and not index
        // This is because stage order might stage, this way we are independent
        public PlayerStageStats GetStageStats(string stageId)
        {
            PlayerStageStats stage = _stagesStat.Find(delegate(PlayerStageStats match) { return match.StageId == stageId; });
            return stage;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetTotalMedalsForTheme(MedalType medalType, ThemeType theme)
        {
            int medals = 0;
            foreach (PlayerStageStats stat in this._stagesStat)
            {
                LevelStage levelStage = Levels._instance.GetLevelStage(stat.StageId);
                if (levelStage != null)
                {
                    if (levelStage.ThemeId == theme &&
                        stat.Medal == medalType)
                    {
                        medals++;
                    }
                }
            }
            return medals;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetUnlockedStagesForTheme(ThemeType theme)
        {
            int unlocked = 0;
            foreach (PlayerStageStats stat in this._stagesStat)
            {
                LevelStage levelStage = Levels._instance.GetLevelStage(stat.StageId);
                if (levelStage != null)
                {
                    if (levelStage.ThemeId == theme)
                    {
                        unlocked++;
                    }
                }
            }
            return unlocked;
        }

        /// <summary>
        /// Returns the total unlocked stages for a specified theme
        /// </summary>
        private int GetTotalUnlockedStagesForTheme(ThemeType theme)
        {
            int locked = 0;
            foreach (PlayerStageStats stats in this._stagesStat)
            {
                LevelStage levelStage = Levels._instance.GetLevelStage(stats.StageId);
                if (levelStage != null && levelStage.ThemeId == theme)
                {
                    locked++;
                }
            }

            return locked;
        }


        #region IDataFileSerializable Members

        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._totalPlayingTime = new TimeSpan(record.GetFieldValue<long>("totalPlayingTime", 0));
            this._totalRunningTime = new TimeSpan(record.GetFieldValue<long>("totalRunningTime", 0));

            this._totalSnailsSafe = record.GetFieldValue<int>("totalSnailsSafe", 0);
            this._totalSnailsKingSafe = record.GetFieldValue<int>("totalSnailsKingSafe", 0);

            this._totalSnailsDeadByFire = record.GetFieldValue<int>("totalSnailsDeadByFire", 0);
            this._totalSnailsDeadBySpikes = record.GetFieldValue<int>("totalSnailsDeadBySpikes", 0);
            this._totalSnailsDeadByDynamite = record.GetFieldValue<int>("totalSnailsDeadByDynamite", 0);
            this._totalSnailsDeadByCrate = record.GetFieldValue<int>("totalSnailsDeadByCrate", 0);
            this._totalSnailsDeadByWater = record.GetFieldValue<int>("totalSnailsDeadByWater", 0);
            this._totalSnailsDeadByLaser = record.GetFieldValue<int>("totalSnailsDeadByLaser", 0);
            this._totalSnailsDeadBySacrifice = record.GetFieldValue<int>("totalSnailsDeadBySacrifice", 0);
            this._totalSnailsDeadByCrateExplosion = record.GetFieldValue<int>("totalSnailsDeadByCrateExplosion", 0);
            this._totalSnailsDeadByAcid = record.GetFieldValue<int>("totalSnailsDeadByAcid", 0);
            this._totalSnailsDeadByOutOfStage = record.GetFieldValue<int>("totalSnailsDeadByOutOfStage", 0);
            this._totalSnailsDeadByEvilSnail = record.GetFieldValue<int>("totalSnailsDeadByEvilSnail", 0);
            
            this._totalGoldMedals = record.GetFieldValue<int>("totalGoldCoins", 0);
            this._totalSilverMedals = record.GetFieldValue<int>("totalSilverCoins", 0);
            this._totalBronzeMedals = record.GetFieldValue<int>("totalBronzeCoins", 0);

            this._totalBoots = record.GetFieldValue<int>("totalBoots", 0);

            // player stats node
            DataFileRecord recordStages = record.SelectRecord("Stages");
            if (recordStages != null)
            {
                DataFileRecordList recordStagesList = recordStages.SelectRecords("Stage");
                this._stagesStat = new List<PlayerStageStats>(recordStagesList.Count); // create instance with the right size
                foreach (DataFileRecord recordStage in recordStagesList)
                {
                    PlayerStageStats stats = PlayerStageStats.CreateFromDataFileRecord(recordStage);
                    this._stagesStat.Add(stats);
                }
            }
        }

        public DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("Statistics");

            record.AddField("totalPlayingTime", this._totalPlayingTime.Ticks);
            record.AddField("totalRunningTime", this._totalRunningTime.Ticks);
            
            record.AddField("totalSnailsSafe", this._totalSnailsSafe);
            record.AddField("totalSnailsKingSafe", this._totalSnailsKingSafe);

            record.AddField("totalSnailsDeadByFire", this._totalSnailsDeadByFire);
            record.AddField("totalSnailsDeadBySpikes", this._totalSnailsDeadBySpikes);
            record.AddField("totalSnailsDeadByDynamite", this._totalSnailsDeadByDynamite);
            record.AddField("totalSnailsDeadByCrate", this._totalSnailsDeadByCrate);
            record.AddField("totalSnailsDeadByWater", this._totalSnailsDeadByWater);
            record.AddField("totalSnailsDeadByLaser", this._totalSnailsDeadByLaser);
            record.AddField("totalSnailsDeadBySacrifice", this._totalSnailsDeadBySacrifice);
            record.AddField("totalSnailsDeadByCrateExplosion", this._totalSnailsDeadByCrateExplosion);
            record.AddField("totalSnailsDeadByAcid", this._totalSnailsDeadByAcid);
            record.AddField("totalSnailsDeadByOutOfStage", this._totalSnailsDeadByOutOfStage);
            record.AddField("totalSnailsDeadByEvilSnail", this._totalSnailsDeadByEvilSnail);

            record.AddField("totalGoldCoins", this._totalGoldMedals);
            record.AddField("totalSilverCoins", this._totalSilverMedals);
            record.AddField("totalBronzeCoins", this._totalBronzeMedals);

            record.AddField("totalBoots", this._totalBoots);

            // Stages stats nodes
            DataFileRecord recordStages = new DataFileRecord("Stages");
            if (_stagesStat != null && _stagesStat.Count > 0)
            {
                foreach (PlayerStageStats stage in _stagesStat)
                {
                    recordStages.AddRecord(stage.ToDataFileRecord());
                }
            }
            record.AddRecord(recordStages);

            return record;
        }

        #endregion

        public int Level { get; set; }

        internal int GetClearStagesForTheme(ThemeType themeType)
        {
            int cleared = 0;
            foreach (PlayerStageStats stats in this._stagesStat)
            {
                if (stats.WasPlayed)
                {
                    LevelStage levelStage = Levels._instance.GetLevelStage(stats.StageId);
                    if (levelStage != null && levelStage.ThemeId == themeType)
                    {
                        cleared++;
                    }
                }
            }

            return cleared;
        }

        public bool IsAllMedalsGet(MedalType medal)
        {
            int cleared = 0;
            foreach (PlayerStageStats stats in this._stagesStat)
            {
                cleared++;
                if ((int)stats.Medal < (int)medal)
                {
                    return false;
                }
            }

            if (cleared == Levels._instance.StageCount) // all stages
            {
                return true;
            }

            return false;
        }
    }
}
