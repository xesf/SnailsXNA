using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageObjects
{
    partial class WalkTester : MovingObject, ISnailsDataFileSerializable
    {
        #region Constants
        public const string ID = "WALK_TESTER";
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public WalkTester()
            : base(StageObjectType.WalkTester)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public WalkTester(WalkTester other)
            : base(other)
        {
            Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.Speed = 0.05f;
            this.SetDirection(WalkDirection.CounterClockwise);
            this.Position = new Microsoft.Xna.Framework.Vector2(224, 100);
            this.UpdateBoundingBox();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(TwoBrainsGames.BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
        }


        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        void IDataFileSerializable.InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
        }

        /// <summary>
        /// 
        /// </summary>
        DataFileRecord IDataFileSerializable.ToDataFileRecord()
        {
            return base.ToDataFileRecord();
        }

        #endregion
    }
}
