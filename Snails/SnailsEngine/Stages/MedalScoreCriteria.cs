using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.Stages
{
    /// <summary>
    /// This class has the settings that are needed so score a medal
    /// Instead of saying "500 pts are require for scoring gold" we say instead:
    /// "X snails are needed, the maximum time is Y, z gold coins are needed, etc
    /// </summary>
    
    
#if STAGE_EDITOR
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public class MedalScoreCriteria : IDataFileSerializable
    {
        // This values are always "at least"
        public int SnailsNeeded { get ;set; }
        public int GoldCoinsNeeded { get; set; }
        public int SilverCoinsNeeded { get; set; }
        public int BronzeCoinsNeeded { get; set; }
        public TimeSpan TimeNeeded { get; set; }
        public Stage StageOwner { get; private set; }
        public int Score
        {
            get
            {
                int snailsBonus = this.SnailsNeeded * Stage.BONUS_SNAILS_DELIVERED;
                int medalBonus = this.GoldCoinsNeeded * Stage.BONUS_GOLD_COINS +
                                 this.SilverCoinsNeeded * Stage.BONUS_SILVER_COINS +
                                 this.BronzeCoinsNeeded * Stage.BONUS_BRONZE_COINS;
                int timeBonus = (int)((this.StageOwner.LevelStage._targetTime - this.TimeNeeded).TotalSeconds * Stage.BONUS_TIME_TAKEN);
                return (snailsBonus + medalBonus + timeBonus);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MedalScoreCriteria(Stage stageOwner)
        {
            this.StageOwner = stageOwner;
        }

        /// <summary>
        /// 
        /// </summary>
        public static MedalScoreCriteria CreateFromDataFileRecord(DataFileRecord record, Stage stageOwner)
        {
            MedalScoreCriteria criteria = new MedalScoreCriteria(stageOwner);
            criteria.InitFromDataFileRecord(record);
            return criteria;
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(string recordName)
        {
            DataFileRecord record = this.ToDataFileRecord();
            record.Name = recordName;
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return this.Score.ToString();
        }
        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            if (record != null)
            {
                this.SnailsNeeded = record.GetFieldValue<int>("snailsNeeded", 0);
                this.GoldCoinsNeeded = record.GetFieldValue<int>("goldCoinsNeeded", 0);
                this.SilverCoinsNeeded = record.GetFieldValue<int>("silverCoinsNeeded", 0);
                this.BronzeCoinsNeeded = record.GetFieldValue<int>("bronzeCoinsNeeded", 0);
                this.TimeNeeded = record.GetFieldValue<TimeSpan>("timeNeeded", new TimeSpan());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("");
            record.AddField("snailsNeeded", this.SnailsNeeded);
            record.AddField("goldCoinsNeeded", this.GoldCoinsNeeded);
            record.AddField("silverCoinsNeeded", this.SilverCoinsNeeded);
            record.AddField("bronzeCoinsNeeded", this.BronzeCoinsNeeded);
            record.AddField("timeNeeded", this.TimeNeeded);
            return record;
        }

        #endregion
    }
}
