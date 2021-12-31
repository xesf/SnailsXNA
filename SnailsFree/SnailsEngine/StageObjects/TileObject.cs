using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class TileObject : StageObject, ISnailsDataFileSerializable
    {
        protected string _tileId;
        protected Tile _tile;

        public TileObject(StageObjectType type)
            : base(type)
        {
        }

        public TileObject(TileObject other)
            : base(other)
        {
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
            this._tileId = (other as TileObject)._tileId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            if (!string.IsNullOrEmpty(this._tileId)) // Might be null if loaded from a stage file (not from StageData)
            {
                this._tile = Levels._instance.StageData.GetTile(this._tileId) as Tile;
                this.BlendColor = this._tile.BlendColor;
            }
        }
        #region ISnailsDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            if (context == ToDataFileRecordContext.StageDataSave)
            {
                record.AddField("tileId", this._tileId);
            }
            return record;

        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._tileId = record.GetFieldValue<string>("tileId", this._tileId);
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
