using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects.Shades;
using TwoBrainsGames.Snails.Screens;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;


namespace TwoBrainsGames.Snails.StageObjects
{
    public class Lava : Liquid
    {
        #region Constants
        #endregion

        #region Members
        #endregion

        #region Properties

        #endregion

        public Lava()
            : base(StageObjectType.Lava)
        { }



        /// <summary>
        /// 
        /// </summary>
        protected override void Resize()
        {
            base.Resize();
            this._crateCollisionBB = this._liquidAABB;
        }


        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
        }

        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);

            return record;
        }
        #endregion
    }
}
