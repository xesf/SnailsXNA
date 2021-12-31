using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// The base class for any switch
    /// </summary>
    public class Switch : StageObject
    {
        public enum SwitchState
        {
            Off,
            On
        }
        public enum SwitchOnActionType
        {
            SwitchOn,
            SwitchOff,
            Invert
        }

        //List<ISwitchable> _connectedObjects;
        #region Properties
        public SwitchState State { get; set; } // To change the state use the methods SwitchOn/SwitchOff
        public SwitchOnActionType SwitchOnAction { get; set; }

        public bool IsOn { get { return this.State == SwitchState.On; } }
        public bool IsOff { get { return this.State == SwitchState.Off; } }

        protected BoundingSquare SnailCollisionBB
        {
            get;
            set;
        }

        public override BoundingSquare QuadtreeCollisionBB
        {
            get
            {
                return this.SnailCollisionBB;
            }
        }
        #endregion

        public Switch()
        { }

        public Switch(StageObjectType type)
            : base(type)
        {
            //this._connectedObjects = new List<ISwitchable>();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            this.SwitchOnAction = ((Switch)other).SwitchOnAction;
            base.Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOn()
        {
            foreach (StageObject obj in this._linkedObjects)
            {
                if (obj is ISwitchable)
                {
                    ISwitchable switchable = obj as ISwitchable;
                    if (obj.IsVisible && !obj.IsDead && !obj.IsDisposed)
                    {
                        switch(this.SwitchOnAction)
                        {
                            case SwitchOnActionType.SwitchOn:
                                switchable.SwitchOn();
                                break;
                            case SwitchOnActionType.SwitchOff:
                                switchable.SwitchOff();
                                break;
                            case SwitchOnActionType.Invert:
                                if (switchable.IsOn)
                                {
                                    switchable.SwitchOff();
                                }
                                else
                                {
                                    switchable.SwitchOn();
                                }

                                break;
                        }
                    }
                }
            }
            this.State = SwitchState.On;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOff()
        {
            foreach (StageObject obj in this._linkedObjects)
            {
                if (obj is ISwitchable)
                {
                    if (obj.IsVisible && !obj.IsDead && !obj.IsDisposed)
                    {
                        ((ISwitchable)obj).SwitchOff();
                    }
                }
            }
            this.State = SwitchState.Off;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateBoundingBox()
        {
            base.UpdateBoundingBox();
            this.SnailCollisionBB = this.AABoundingBox;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
#if DEBUG_ASSERTIONS
            if (obj as Snail == null || listIdx != Stage.QUADTREE_SNAIL_LIST_IDX)
            {
                throw new BrainException("Expected a Snail object.");
            }
#endif
            this.OnSnailCollided((Snail)obj);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSnailCollided(Snail snail)
        {
        }


        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            switch (context)
            {
                case ToDataFileRecordContext.StageSave:
                    record.AddField("switchOnAction", this.SwitchOnAction.ToString());
                    break;
            }
            return record;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.SwitchOnAction = (SwitchOnActionType)Enum.Parse(typeof(SwitchOnActionType), record.GetFieldValue<string>("switchOnAction", this.SwitchOnAction.ToString()), true);
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
