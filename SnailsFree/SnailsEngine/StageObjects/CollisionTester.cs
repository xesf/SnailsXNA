using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.StageObjects
{
    class CollisionTester : StageObject, ISnailsDataFileSerializable
    {
        enum ObjStatus
        {
            Idle,
            ObjectReleased
        }

        #region Constants
        public const string ID = "COLLISION_TESTER";
        #endregion

        #region Members/Properties
        float Angle { get; set; }
        float Speed { get; set; }
        ObjStatus Status { get; set; }
        int TimeToRelease { get; set; }
        int ObjectToRelease { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public CollisionTester()
            : base(StageObjectType.CollisionTester)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public CollisionTester(CollisionTester other)
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
            this.Speed = 600.0f;
            this.Angle = -45.0f;
            this.TimeToRelease = 200;
            this.ObjectToRelease = 100;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(TwoBrainsGames.BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this.ObjectToRelease <= 0)
                return;

            if (this.Status == ObjStatus.Idle)
            {
                this.TimeToRelease -= gameTime.ElapsedGameTime.Milliseconds;
                if (this.TimeToRelease <= 0)
                {
            /*          this.LaunchObject(0, 0.0f, MovingObject.WalkDirection.Clockwise);
                  this.LaunchObject(90, 0.0f, MovingObject.WalkDirection.Clockwise);
                  this.LaunchObject(180, 0.0f, MovingObject.WalkDirection.Clockwise);
                  this.LaunchObject(270, 0.0f, MovingObject.WalkDirection.Clockwise);
                  this.LaunchObject(0, 0.0f, MovingObject.WalkDirection.CounterClockwise);
                  this.LaunchObject(90, 0.0f, MovingObject.WalkDirection.CounterClockwise);
                      this.LaunchObject(180, 0.0f, MovingObject.WalkDirection.CounterClockwise);
                   this.LaunchObject(270, 0.0f, MovingObject.WalkDirection.CounterClockwise);*/
                 this.LaunchObject(BrainGame.Rand.Next(360), 0.01f,
                        (BrainGame.Rand.Next(2) == 1 ? MovingObject.WalkDirection.CounterClockwise : MovingObject.WalkDirection.Clockwise)
                        );
                    this.TimeToRelease = 200;
                    this.ObjectToRelease--;
                }
            }
        }

        void LaunchObject(float angle, float moveSpeed, MovingObject.WalkDirection direction)
        {

            StageObject obj = Stage.CurrentStage.StageData.GetObject(WalkTester.ID);
            obj.BoardX = this.BoardX;
            obj.BoardY = this.BoardY;
            obj.UpdateBoundingBox();
            //  obj.EffectsBlender.Add(new LinearMoveEffect(this.Speed, BrainEngine.BrainE.Rand.Next(360)), 99);
            obj.EffectsBlender.Add(new LinearMoveEffect(this.Speed, angle), 99);
  
            MovingObject mo = (MovingObject)obj;
            mo.SetDirection(direction);
            mo.Speed = 0.01f + (BrainGame.Rand.Next(10) / 100.0f);
            mo.Speed = moveSpeed;


            Stage.CurrentStage.AddObjectInRuntime(obj);
        }

        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
          base.InitFromDataFileRecord(record);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
          return base.ToDataFileRecord();
        }

        #endregion
    }
}
