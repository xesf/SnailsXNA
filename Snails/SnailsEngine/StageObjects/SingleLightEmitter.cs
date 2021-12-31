using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// To be used as a base class for objects that emmit a single light
    /// </summary>
    class SingleLightEmitter : StageObject
    {
        protected string LightSourceId { get; set; } // In stagedata
        protected LightSource LightSource { get; set; }
        public LightSource.LightState State { get; set; }
        protected Vector2 LightMaskScale { get; set; }


        public bool IsOn
        {
            get { return this.LightSource.IsOn; }
        }

        public bool IsOff
        {
            get { return this.LightSource.IsOff; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SingleLightEmitter(StageObjectType type) :
            base(type)
        {
            this.LightSource = new LightSource();
            this.LightMaskScale = new Vector2(1f, 1f);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            SingleLightEmitter otherLight = (SingleLightEmitter)other;
            this.LightSourceId = otherLight.LightSourceId;
            this.State = otherLight.State;
            this.LightMaskScale = otherLight.LightMaskScale;
        }

        /// <summary>
        ///
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.LightSource = Stage.CurrentStage.StageData.GetLightSource(this.LightSourceId);
            this.LightSource.Scale = this.LightMaskScale;
            this.LightSource.Position = this.BoundingBox.GetCenter();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StageInitialize()
        {
            base.StageInitialize();
            Stage.CurrentStage.LightManager.AddLightSource(this.LightSource);
        }

        /// <summary>
        ///
        /// </summary>
        public override void DisposeFromStage()
        {
            base.DisposeFromStage();
            Stage.CurrentStage.LightManager.RemoveLightSource(this.LightSource);
        }

        #region IDataFileSerializable Members
        /// <summary>
        ///
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.LightSource = new LightSource();
            this.LightSourceId = record.GetFieldValue<string>("lightSourceId", this.LightSourceId);
            if (string.IsNullOrEmpty(this.LightSourceId))
            {
                throw new SnailsException("SingleLightEmitter objects must have a lightSourceId!!");
            }
            this.State = (LightSource.LightState)Enum.Parse(typeof(LightSource.LightState), record.GetFieldValue<string>("state", LightSource.LightState.On.ToString()), true);
            float x = record.GetFieldValue<float>("lightScaleX", this.LightMaskScale.X);
            float y = record.GetFieldValue<float>("lightScaleY", this.LightMaskScale.Y);
            this.LightMaskScale = new Vector2(x, y);
        }

        /// <summary>
        ///
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }
     

        /// <summary>
        ///
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            if (context == ToDataFileRecordContext.StageDataSave)
            {
                record.AddField("lightSourceId", this.LightSourceId);
                record.AddField("lightScaleX", this.LightMaskScale.X);
                record.AddField("lightScaleY", this.LightMaskScale.Y);
            }
            record.AddField("state", this.State.ToString());
            return record;
        }
        #endregion

    }
}
