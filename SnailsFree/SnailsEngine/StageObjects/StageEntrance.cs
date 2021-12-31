using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;

namespace TwoBrainsGames.Snails.StageObjects
{
    public partial class StageEntrance : StageObject, ISnailsDataFileSerializable
    {
        #region Consts
        const int CRATE_TOOL_VALID_BB_IDX = 0; // Index for the collision box in the sprite for the crate tool valid test
        #endregion

        public enum StageEntranceState
        {
            InitialDelay,
            Idle,
            Shaking,
            Releasing
        }

        public enum EntranceReleaseDirection
        {
            Clockwise,
            CounterClockwise,
            Both
        }

        #region Constants
        const int DEFAULT_RELEASE_INTERVAL = 1000;
        const string SPRITE_ENTRANCE_IDLE = "EntranceIdle";
        const string SPRITE_ENTRANCE_RELEASING = "Entrance";
        const string SPRITE_SNAIL_RELEASE_RIGHT = "SnailReleaseRight";
        const string SPRITE_SNAIL_RELEASE_LEFT = "SnailReleaseLeft";
        const int KING_SIGN_BB_IDX = 0;
        const int EVIL_SNAIL_SIGN_BB_IDX = 1;

        #endregion

        #region Members
        protected Sample _eyeBlinkSample;
        protected Sample _entranceSample;
        #endregion

        #region Properties
        // Most of this are public because of the Stage Editor
        public StageEntranceState State;
        public int TotalSnailsToRelease;  // Total snails to release
        public int SnailsToReleaseBeforeKing; // Number of snails to be released before releasing the king
        public int SnailsToRelease { get; set;} // Total snails that are yet to be released
        public int IntervalToRelease { get; set;}
        public int InitialReleaseDelay;
        public EntranceReleaseDirection ReleaseDirection;
        public SnailCounter SnailCounter { get; set; }
        public bool ReleasesSnailKing; // True if the entrance releases the snail king
        public bool _snailKingReleased; // True if the snail king was released
        double _ellapsedReleaseTime;
        double _ellapsedInitialTime;
        private MovingObject.WalkDirection _walkDirection;
        public string SnailsToReleaseId { get; set; } // Stage data ID of the snail type to release
        public bool ReleasesEvilSnails { get { return (this.SnailsToReleaseId == EvilSnail.ID); } }
        private Sprite _kingSignSprite;
        private Sprite _evilSnailSignSprite;
        private Vector2 _kingSignPos;
        private Vector2 _evilSnailSignPos;
        private Sprite _releasingSnailSprite;
        private Sprite _releasingSnailLeft;
        private Sprite _releasingSnailRight;
        private Sprite _idleSprite;
        #endregion

        public StageEntrance()
            : base(StageObjectType.StageEntrance)
        {
            TotalSnailsToRelease = SnailsToRelease = 0;
            SnailsToReleaseBeforeKing = 0;
            IntervalToRelease = DEFAULT_RELEASE_INTERVAL;
            InitialReleaseDelay = 0;
            ReleaseDirection = EntranceReleaseDirection.Clockwise;
            State = StageEntranceState.InitialDelay;
            SpriteId = SPRITE_ENTRANCE_IDLE;
            _walkDirection = MovingObject.WalkDirection.Clockwise;
            this.SnailsToReleaseId = Snail.ID;
        }

        public StageEntrance(StageEntrance other)
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

            StageEntrance stageEntrance = (StageEntrance)other;
            this.SnailsToRelease = stageEntrance.SnailsToRelease;
            this.TotalSnailsToRelease = this.SnailsToRelease;
            this.SnailsToReleaseBeforeKing = stageEntrance.SnailsToReleaseBeforeKing;
            this.IntervalToRelease = stageEntrance.IntervalToRelease;
            this.InitialReleaseDelay = stageEntrance.InitialReleaseDelay;
            this.ReleaseDirection = stageEntrance.ReleaseDirection;
            this.State = stageEntrance.State;
            this.ReleasesSnailKing = stageEntrance.ReleasesSnailKing;
            this._snailKingReleased = stageEntrance._snailKingReleased;
            this._walkDirection = stageEntrance._walkDirection;
            this._eyeBlinkSample = stageEntrance._eyeBlinkSample;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            // Make the first snail take only 1 second to be released
            this._ellapsedReleaseTime = this.IntervalToRelease - 1000;
            this._ellapsedInitialTime = 0;
            this.State = StageEntranceState.InitialDelay;
            this._kingSignPos = this.TransformSpriteFrameBB(KING_SIGN_BB_IDX).GetCenter();
            this._evilSnailSignPos = this.TransformSpriteFrameBB(EVIL_SNAIL_SIGN_BB_IDX).GetCenter();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (SnailCounter != null) // What the heck...
            {
                SnailCounter.LoadContent();
            }

            // Sfx
            this._eyeBlinkSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.EYE_BLINK_SAMPLE, this);
            this._entranceSample = BrainGame.ResourceManager.GetSample(AudioTags.SNAIL_ENTRANCE, ResourceManagerIds.STAGE_THEME_RESOURCES, this);

            // Sprites
            this._kingSignSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects/SnailEntranceKing");
            this._evilSnailSignSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects/SnailEntranceEvil");
            this._releasingSnailSprite = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + StageEntrance.SPRITE_ENTRANCE_RELEASING, ResourceManagerIds.STAGE_THEME_RESOURCES);
            this._releasingSnailLeft = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + StageEntrance.SPRITE_SNAIL_RELEASE_LEFT, ResourceManagerIds.STAGE_THEME_RESOURCES);
            this._releasingSnailRight = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + StageEntrance.SPRITE_SNAIL_RELEASE_RIGHT, ResourceManagerIds.STAGE_THEME_RESOURCES);
            this._idleSprite = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + StageEntrance.SPRITE_ENTRANCE_IDLE, ResourceManagerIds.STAGE_THEME_RESOURCES);
        }
     
        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            switch(this.State )
            {
                case StageEntranceState.Shaking:
                    base.CurrentFrame = 0;
                    this.State = StageEntranceState.Releasing;
                    if (this._walkDirection == MovingObject.WalkDirection.Clockwise)
                    {
                        Sprite = this._releasingSnailRight;
                    }
                    else 
                    {
                        Sprite = this._releasingSnailLeft;
                    }                   
                    break;

                case StageEntranceState.Releasing:
                    base.CurrentFrame = 0;
                    this.State = StageEntranceState.Idle;
                    Sprite = this._idleSprite;

                    if (this.ReleasesSnailKing && !this._snailKingReleased && (this.TotalSnailsToRelease - SnailsToRelease) == this.SnailsToReleaseBeforeKing)
                    {
                        Stage.CurrentStage.ReleaseSnailKing(this.Position, this._walkDirection);
                        this._snailKingReleased = true;
                    }
                    else
                    {
                        Stage.CurrentStage.ReleaseSnail(this.Position, this._walkDirection, this.SnailsToReleaseId);
                    }
                    // decrease number of snails to release
                    SnailsToRelease--;

                    // Invert walk direction if release mode is both directions
                    if (this.ReleaseDirection == EntranceReleaseDirection.Both)
                    {
                        this._walkDirection = (MovingObject.WalkDirection)((int)this._walkDirection  * -1);
                    }

                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
		
            base.Update(gameTime);

            if (this.SnailCounter != null)
            {
                this.SnailCounter.SetCounter(SnailsToRelease);
            }

            switch (this.State)
            {
                case StageEntranceState.InitialDelay:
                    this._ellapsedInitialTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (this._ellapsedInitialTime > this.InitialReleaseDelay)
                    {
                        this.State = StageEntranceState.Idle;
                        //this._ellapsedReleaseTime += this.InitialReleaseDelay - this._ellapsedInitialTime;
                    }
                    break;

                case StageEntranceState.Idle:
                    if (this.SnailsToRelease > 0)
                    {
                        this._ellapsedReleaseTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (this._ellapsedReleaseTime > this.IntervalToRelease)
                        {
                            this.CurrentFrame = 0;
                            this.Sprite = this._releasingSnailSprite;
                            this.State = StageEntranceState.Shaking;
                            this._ellapsedReleaseTime = this._ellapsedReleaseTime - this.IntervalToRelease; // rest first release interval
                            this._ellapsedReleaseTime = 0;

                            _entranceSample.Play(); // Snail slime sound only sometimes
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            if (Stage.CurrentStage.State == Stage.StageState.Startup)
            {
                if (this.ReleasesSnailKing)
                {
                    this._kingSignSprite.Draw(this._kingSignPos, Stage.CurrentStage.SpriteBatch);
                }
                if (this.ReleasesEvilSnails)
                {
                    this._evilSnailSignSprite.Draw(this._evilSnailSignPos, Stage.CurrentStage.SpriteBatch);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AddLinkedObject(StageObject obj)
        {
            base._linkedObjects.Add(obj);
            if (obj as SnailCounter != null)
            {
                this.SnailCounter = (SnailCounter)obj;
                this.SnailCounter.SetCounter(this.SnailsToRelease);
                this.SnailCounter.ConnectToEntrance(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetSnailCounter(SnailCounter counter)
        {
            this.SnailCounter = counter;
            this.LinkTo(counter);
        }

        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);

            SnailsToRelease = record.GetFieldValue<int>("snails", 0);
            TotalSnailsToRelease = SnailsToRelease;
            IntervalToRelease = record.GetFieldValue<int>("interval", this.IntervalToRelease);
            SnailsToReleaseBeforeKing = record.GetFieldValue<int>("snailsToReleaseBeforeKing", this.SnailsToReleaseBeforeKing);
            InitialReleaseDelay = record.GetFieldValue<int>("initialDelay", this.InitialReleaseDelay);
            this.SnailsToReleaseId = record.GetFieldValue<string>("snailsToReleaseId", this.SnailsToReleaseId);
            ReleaseDirection = (EntranceReleaseDirection)Enum.Parse(typeof(EntranceReleaseDirection), record.GetFieldValue<string>("direction", EntranceReleaseDirection.Clockwise.ToString()), false);
            State = (StageEntranceState)Enum.Parse(typeof(StageEntranceState), record.GetFieldValue<string>("state", StageEntranceState.Idle.ToString()), false);
            this.ReleasesSnailKing = record.GetFieldValue<bool>("releasesSnailKing", this.ReleasesSnailKing);
            if (this.ReleaseDirection == EntranceReleaseDirection.CounterClockwise)
            {
                this._walkDirection = MovingObject.WalkDirection.CounterClockwise;
            }
            else
            {
                this._walkDirection = MovingObject.WalkDirection.Clockwise;
            }
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

            if (context == ToDataFileRecordContext.StageSave)
            {
                record.AddField("snails", this.SnailsToRelease);
                record.AddField("interval", this.IntervalToRelease);
                record.AddField("initialDelay", this.InitialReleaseDelay);
                record.AddField("direction", this.ReleaseDirection.ToString());
                record.AddField("releasesSnailKing", this.ReleasesSnailKing);
                record.AddField("snailsToReleaseId", this.SnailsToReleaseId);
                record.AddField("snailsToReleaseBeforeKing", this.SnailsToReleaseBeforeKing);
            }
            return record;
        }
        #endregion
    }
}
