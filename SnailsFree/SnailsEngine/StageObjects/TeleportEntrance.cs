using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.StageObjects
{
    public partial class TeleportEntrance : StageObject
    {
        #region Members
        #endregion

        public TeleportEntrance()
            : base(StageObjectType.TeleportEntrance)
        {
        }

        public TeleportEntrance(TeleportEntrance other)
            : base(other)
        {
            Copy(other);
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
        }

        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
        }

		/// <summary>
		/// 
		/// </summary>
		public override void OnCollide(StageObject obj)
		{
            Snail snail = obj as Snail;
            if (snail != null)
            {
                // TODO
            }
		}

        #region IDataFileSerializable Members
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
        }

        public override DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = base.ToDataFileRecord();
            return record;
        }
        #endregion
    }
}
