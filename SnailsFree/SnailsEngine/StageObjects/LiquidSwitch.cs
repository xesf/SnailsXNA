using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// Base class for switches that are connected to Liquids
    /// </summary>
    public class LiquidSwitch : Switch
    {

        public const float MAX_PUMP_SPEED = 10f;
        public const float MIN_PUMP_SPEED = 0.01f;
        private const float DEFAULT_SPEED = 5f;

        #region Vars
        public float PumpSpeed { get; set; } // The speed at the switch will pump liquid into the liquid pool
        #endregion

        public LiquidSwitch(StageObjectType objType) :
            base(objType)
        {
            this.PumpSpeed = DEFAULT_SPEED;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }


        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            record.AddField("pumpSpeed", this.PumpSpeed);
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.PumpSpeed = record.GetFieldValue<float>("pumpSpeed");
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        #endregion
    }
}
