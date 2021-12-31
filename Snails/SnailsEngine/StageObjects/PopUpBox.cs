using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// A pop up box is a box that is minized and where the snails can't walk on
    /// When a switch is activated, the boxes pop and then are walkable
    /// Pop up boxes can be activated by switches, and can work as a switch too
    /// A pop up box when activated may activate another pop up box creating a nice pop up box 
    /// activation paralel effect
    /// </summary>
    class PopUpBox : StageObject, ISwitchable
    {
        private Box.BoxType _boxType;
        int _scaleDirection;
        float _scale;

        /// <summary>
        /// 
        /// </summary>
        public PopUpBox()
            : base(StageObjectType.PopUpBox)
        {
        }

   
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.EffectsBlender.Add(new ScaleEffect(new Vector2(1f, 1f), 0.5f, new Vector2(0.9f, 0.9f), true));
            this.SpriteAnimationActive = false;
            this._scaleDirection = 1;
            this._scale = 1f;
            this.SpriteAnimationActive = false;
            this.CurrentFrame = (int)this._boxType;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
            this._scale += ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000) * this._scaleDirection;
            if (this._scale < 0.5f)
            {
                this._scaleDirection = 1;
                this._scale = 0.5f;
            }
            else
            if (this._scale > 1f)
            {
                this._scaleDirection = -1;
                this._scale = 1f;
            }
            this.Scale = new Vector2(this._scale, this._scale);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOn()
        {
            Box newBox = Box.CreateAndDeploy(this._boxType, this.Position);
            foreach(StageObject linked in this._linkedObjects)
            {
                if (linked is ISwitchable)
                {
                    newBox.AddSwitchableObject((ISwitchable)linked);
                }
            }
            this.DisposeFromStage();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOff()
        {
            // Do nothing, pop up box can't be turned off
        }

        public bool IsOn
        {
            get { return false; } // Pop up boxes are always off, when they're on, they deploy and are no more popUpBoxes
        }

        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            if (context == ToDataFileRecordContext.StageSave)
            {
                record.AddField("boxType", (int)this._boxType);
            }
            return record;
        }

  
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._boxType = (Box.BoxType)record.GetFieldValue<int>("boxType", (int)Box.BoxType.WoodBox);
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
