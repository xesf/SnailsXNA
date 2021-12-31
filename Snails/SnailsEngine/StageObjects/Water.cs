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
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.StageObjects
{
    public class Water : Liquid
    {
        #region Constants
        public const float WATER_GRAVITY = 5f;
        #endregion


        #region Properties
        Sample _waterSample;

        #endregion

        public Water()
            : base(StageObjectType.Water)
        { }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
        }

        public override void Initialize()
        {
            base.Initialize();

        }

        public override void LoadContent()
        {
            base.LoadContent();
            this._waterSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.WATER, this);

        }

        /// <summary>
        /// 
        /// </summary>
        public override void StageStartupPhaseEnded()
        {
            base.StageStartupPhaseEnded();
            this._waterSample.Play(true);
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
