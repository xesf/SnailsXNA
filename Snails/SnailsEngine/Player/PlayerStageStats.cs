using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.Player
{
    /// <summary>
    /// Stage Stats specific for the Player
    /// </summary>
    public class PlayerStageStats : IDataFileSerializable
    {
        #region Members
        protected string _stageId;
        protected TimeSpan _completionTime;
        protected int _numSnailsSafe;
        protected int _numGoldCoins;
        protected int _numSilverCoins;
        protected int _numBronzeCoins;
        protected Snails.MedalType _medal;
        protected int _highscore;
        #endregion

        #region Properties
        public string StageId
        {
            get { return _stageId; }
            set { _stageId = value; }
        }
        public TimeSpan CompletionTime
        {
            get { return _completionTime; }
            set { _completionTime = value; }
        }

        public int NumSnailsSafe
        {
            get { return _numSnailsSafe; }
            set { _numSnailsSafe = value; }
        }

        public int NumGoldCoins
        {
            get { return _numGoldCoins; }
            set { _numGoldCoins = value; }
        }

        public int NumSilverCoins
        {
            get { return _numSilverCoins; }
            set { _numSilverCoins = value; }
        }

        public int NumBronzeCoins
        {
            get { return _numBronzeCoins; }
            set { _numBronzeCoins = value; }
        }

        public Snails.MedalType Medal
        {
            get { return _medal; }
            set { _medal = value; }
        }

        public int Highscore
        {
            get { return _highscore; }
            set { _highscore = value; }
        }

        public int TimesPlayed { get; set; }
        public bool WasPlayed { get { return this.TimesPlayed > 0; } }
        #endregion

        private PlayerStageStats()
        {
        }

        public PlayerStageStats(string stageId)
        {
            this.StageId = stageId;
        }

        /// <summary>
        /// 
        /// </summary>
        public static PlayerStageStats CreateFromDataFileRecord(DataFileRecord record)
        {
            PlayerStageStats stats = new PlayerStageStats();
            stats.InitFromDataFileRecord(record);
            return stats;
        }


        #region IDataFileSerializable Members

        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._stageId = record.GetFieldValue<string>("stageId");
            this._completionTime = new TimeSpan(record.GetFieldValue<long>("completionTime", 0));
            this._numSnailsSafe = record.GetFieldValue<int>("numSnailsSafe", 0);
            this._numGoldCoins = record.GetFieldValue<int>("numGoldCoins", 0);
            this._numSilverCoins = record.GetFieldValue<int>("numSilverCoins", 0);
            this._numBronzeCoins = record.GetFieldValue<int>("numBronzeCoins", 0);
            this._medal = (Snails.MedalType)record.GetFieldValue<int>("medal", (int)Snails.MedalType.None);
            this._highscore = record.GetFieldValue<int>("highscore", 0);
            this.TimesPlayed = record.GetFieldValue<int>("timesPlayed", 0);
        }

        public DataFileRecord ToDataFileRecord()
        {
            return ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("Stage");

            record.AddField("stageId", this._stageId);
            record.AddField("completionTime", this._completionTime.Ticks);
            record.AddField("numSnailsSafe", this._numSnailsSafe);
            record.AddField("numGoldCoins", this._numGoldCoins);
            record.AddField("numSilverCoins", this._numSilverCoins);
            record.AddField("numBronzeCoins", this._numBronzeCoins);
            record.AddField("medal", (int)this._medal);
            record.AddField("highscore", (int)this._highscore);
            record.AddField("timesPlayed", (int)this.TimesPlayed);
            return record;
        }

        #endregion
    }
}
