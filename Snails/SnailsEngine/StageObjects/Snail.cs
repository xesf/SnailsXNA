using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.StageObjects.SpriteAccessories;
using TwoBrainsGames.BrainEngine.Resources;
using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Player;

namespace TwoBrainsGames.Snails.StageObjects
{
   
    public class Snail : MovingObject
    {
        #region Consts

        public const string ID = "SNAIL";
        public const string KING_ID = "SNAIL_KING";
        public const float SNAIL_LAYER_DEPTH = 0.9f;
        public const float WALKING_SPEED = 0.03f;
        public const float WALKING_SPEED_VITAMINIZED = 0.06f;
        public const float WALKING_SPEED_UNDERWATER = 0.5f; // Miltiply this by WALKING_SPEED and WALKING_SPEED_VITAMINIZED
        public const string SPRITE_SNAIL_WALK = "SnailWalk";
        public const string SPRITE_SNAIL_TURN_UP = "SnailTurnUp";
        public const string SPRITE_SNAIL_TURN_DOWN = "SnailTurnDown";
        protected const string SPRITE_SNAIL_ENTERING_STAGE_EXIT = "ExitingStage";
        public const string SPRITE_SNAIL_HIDDING_IN_SHELL = "HiddingInShell";
        public const string SPRITE_SNAIL_EMPALED = "EmpaledSnail";
        public const string SPRITE_SNAIL_DEAD_BROKEN_SHELL = "DeadSnailBrokenShell";
        public const string SPRITE_DEATH_STAGE_EXIT = "DeathWithStageExit";
        public const string SPRITE_SNAIL_BYTING = "Byte";
        public const string SPRITE_SNAIL_CHEWING = "Chew";
        public const string SPRITE_SNAIL_BREATHING_BUBBLES= "Bubbles";
        public const int EATING_APPLE_TIME = 1700;
        public const int UNDER_WATER_TIME = 40000;
        public const int UNDER_WATER_BREATHING_MAX_TIME = 3000;
        public const float FLOAT_SPEED = 0.15f;

        public const int SHELL_BB_INDEX = 0;
        public const int HEAD_BB_INDEX = 1; // Index of the snail head bb in the sprite bb array
        public const int TRAMPOLIM_BB_INDEX = 2; // Index of the snail BB used to bounce in the trampolim
        public const int VITAMIN_BB_INDEX = 0; // Index of the snail BB used to colide with vitamins (rockets)

        public const int SNAIL_HIDDING_SCARE_TIME = 2000; // Time that the snail stays hidden when scared by something
        #endregion

        public enum SnailStatus
		{
			Normal,
            Hidding,
            DeathByFire,
            DeathBySpikes,
            DeathWithStageExitEffect,
            Byting,
            Chewing,
            ExitingStage,
            Hibernate,
            DeathByFireSmoke,
            DeathByWater
         }

        #region Members
        public TransformBlender _effectsBlender;
        private double _eatingElapsedGameTime;
		Sprite _EnteringStageExitSprite;
        public Sprite SpriteHiddingInShell;
        public Sprite SpriteChewing;
        public Sprite SpriteByting;
        public Sprite DeathWithStageExitSprite;
        public Sprite SpriteEmpaled;
        public Sprite SpriteDeadBrokenShell;
        RocketAccessory _rocketAccessory;
        HelmetAccessory _helmetAccessory;
        private List<SnailSpriteAccessory> _spriteAcessories;

        double _genericTimer;
        public SnailStatus _snailStatus;

        // sfx
        Sample [] _soundsGenericSnailDeath;
        Sample _snailEnteringExit;
        Sample _snailBreathUnderwaterSound;

        protected double _inWaterElapsedGameTime;

        // Bubbles when under water
        private SpriteAnimation _bubblesAnim; // Snail breathing when under water
        private double _showBubblesEllapsedTime; // when expires, shows the bubbles again

        private bool _allowRocketPickUp;
        private bool _allowAffectedBySalt;
        private bool _allowEatApple;
        private bool _allowJumpOnTrampoline;
        private bool _allowObjectPickup;
        private bool _allowKilledByEvilSnail;
        public bool AllowStageStatistics { get; set; } // If true, the snail wil be used for snail kills/saved statistics, etc
        private bool _allowSwitchActivation;
        private bool _allowEnterStageExit;
        private Apple _apple;
        private Vitamin _vitamin;

        public bool _deathAccounted;
        public bool _inactiveAccounted;
        #endregion

        #region Properties
        public bool IsEating { get { return ((this.DynamicFlags & StageObjectDynamicFlags.IsEating) == StageObjectDynamicFlags.IsEating); } }
        protected bool IsHidding
        {
            get
            {
                return (this._snailStatus == SnailStatus.Hibernate ||
                        this._snailStatus == SnailStatus.Hidding);
            }
        }

        public bool CanBeSuckedBySwitch
        {
            get
            {
                return (this._allowSwitchActivation &&
                        this._snailStatus == SnailStatus.Normal &&
                        this.IsVisible && this.IsAttachedToPath);
            }
        }

        public bool CanActivateSwitch
        {
            get
            {
                return (this._allowSwitchActivation &&
                        this._snailStatus == SnailStatus.Normal &&
                        this.IsVisible && this.IsAttachedToPath);
            }
        }

        public bool CanPickupObject
        {
            get { return this._allowObjectPickup; }
        }

        public bool CanEatApple
        {
            get
            {
                return (this._allowEatApple &&
                        this._snailStatus == SnailStatus.Normal && 
                        !this.IsEating && this.IsVisible && !this.IsUnderLiquid &&
                        this.IsAttachedToPath && this.PathNode.Value.WallType == PathSegment.SegmentType.Floor);
            }
        }

        public bool CanBeAffectedBySalt
        {
            get
            {
                return (this._allowAffectedBySalt &&
                        this._snailStatus != Snail.SnailStatus.Hidding && //if its already hid in cell
                        this.IsVisible &&
                        this.IsAttachedToPath &&
                        !this.IsUnderLiquid);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual bool CanEatVitamins
        {
            get
            {
                return (this._allowRocketPickUp &&
                        this._snailStatus != SnailStatus.Byting &&
                        this._snailStatus != SnailStatus.Chewing &&
                        this._snailStatus != SnailStatus.ExitingStage &&
                        this.IsVisible &&
                        this.IsVitaminized == false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanJumpTranpoline
        {
            get
            {
                // Attached to floor and ignore turns
                return (this._allowJumpOnTrampoline && 
                        this.IsAttachedtoFloor() &&
                        this.IsVisible &&
                        !this.IsUnderLiquid &&
                        (this.State == MovingState.Walking ||
                         this.State == MovingState.Stopped));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanEnterStage
        {
            get
            {
                 return (this._allowEnterStageExit &&
                         this.IsAttachedToPath &&
                         this.State == MovingState.Walking &&
                         this.IsVisible &&
                         this._snailStatus != SnailStatus.ExitingStage);
            }
        }

        public bool CanBeKilledByEvilSnail
        {
            get
            {
                return (this._allowKilledByEvilSnail &&
                        this.IsDead == false &&
                        this.IsVisible);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override Sprite Sprite
        {
            get
            {
                return base.Sprite;
            }
            set
            {
                if (base.Sprite != value)
                {
                    base.Sprite = value;
                    this.SpriteChanged();
                }
            }
        }

        public bool IsSnailKing
        {
            get
            {
                return (this.Type == StageObjectType.SnailKing);
            }
        }

        public bool CanBeAffectedByWater
        {
            get
            {
                return (this.IsVisible &&
                        this.IsAttachedToPath &&
                        !this.IsDead);
            }
        }

        public bool IsEvilSnail
        {
            get { return this is EvilSnail; }
        }

         #endregion

        /// <summary>
        /// 
        /// </summary>
        public Snail()
            : this(StageObjectType.Snail)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected Snail(StageObjectType type)
            : base(type)
        {
            this._effectsBlender = new TransformBlender();
            this.Speed = Snail.WALKING_SPEED;
            this._spriteAcessories = new List<SnailSpriteAccessory>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Snail(StageObject other)
            : base(other)
        {
            this.Copy(other);
        }

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            Snail snail = (Snail)other;
            this._EnteringStageExitSprite = snail._EnteringStageExitSprite;
            this.SpriteHiddingInShell = snail.SpriteHiddingInShell;
            this.SpriteByting = snail.SpriteByting;
            this.SpriteChewing = snail.SpriteChewing;
            this.DeathWithStageExitSprite = snail.DeathWithStageExitSprite;
            this.SpriteEmpaled = snail.SpriteEmpaled;
            this.SpriteDeadBrokenShell = snail.SpriteDeadBrokenShell;
            this._allowRocketPickUp = snail._allowRocketPickUp;
            this._allowEatApple = snail._allowEatApple;
            this._allowAffectedBySalt = snail._allowAffectedBySalt;
            this._allowJumpOnTrampoline = snail._allowJumpOnTrampoline;
            this._allowObjectPickup = snail._allowObjectPickup;
            this.AllowStageStatistics = snail.AllowStageStatistics;
            this._allowKilledByEvilSnail = snail._allowKilledByEvilSnail;
            this._allowSwitchActivation = snail._allowSwitchActivation;
            this._allowEnterStageExit = snail._allowEnterStageExit;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            // This should go to LoadContent...Hum? It is in the LoadContent!!
            this.WalkSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_WALK);
            this.InnerTurnSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_TURN_UP);
            this.OuterTurnSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_TURN_DOWN);
            this._EnteringStageExitSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_ENTERING_STAGE_EXIT);
            this.SpriteHiddingInShell = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_HIDDING_IN_SHELL);
            this.SpriteByting = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_BYTING);
            this.SpriteChewing = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_CHEWING);
            this.DeathWithStageExitSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_DEATH_STAGE_EXIT);
            this.SpriteEmpaled = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_EMPALED);
            this.SpriteDeadBrokenShell = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_DEAD_BROKEN_SHELL);
            this._bubblesAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/anim-snails", Snail.SPRITE_SNAIL_BREATHING_BUBBLES));
            this._bubblesAnim.Autohide = true;

            this.AddAccessory((this._rocketAccessory = new RocketAccessory(this)));
            this.AddAccessory((this._helmetAccessory = new HelmetAccessory(this)));

            
            // Sounds
             _killedByFire = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_KILLED_FIRE, this);
            _soundsGenericSnailDeath = new Sample[4];
            _soundsGenericSnailDeath[0] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_DEATH_1, this);
            _soundsGenericSnailDeath[1] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_DEATH_2, this);
            _soundsGenericSnailDeath[2] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_DEATH_3, this);
            _soundsGenericSnailDeath[3] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_DEATH_4, this);
            _snailEnteringExit = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_ENTERING_EXIT, this);
            this._snailBreathUnderwaterSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_BREATH_UNDERWATER, this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void AddAccessory(SnailSpriteAccessory accessory)
        {
            accessory.LoadContent();
            this._spriteAcessories.Add(accessory);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            switch (this._snailStatus)
            {
                // Fire death has 2 steps - 
                //   1st step snail turning into smoke
                //   2nd step smoke 
                // This are 2 separate animations because 2nd animation will always have a 0 rotation
                case SnailStatus.DeathByFire:
                    this.Position = new Vector2(this.AABoundingBox.Left + (this.AABoundingBox.Width / 2),
                                                this.AABoundingBox.Top + (this.AABoundingBox.Height / 2));
                    this.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects", StageObject.SPRITE_DEATH_BY_FIRE);
                    this.CurrentFrame = 0;
                    this.Rotation = 0;
                    this._snailStatus = SnailStatus.DeathByFireSmoke;
                    break;

                case SnailStatus.DeathByFireSmoke:
                    this.DisposeFromStage();
                    break;

                case SnailStatus.DeathBySpikes:
                case SnailStatus.DeathWithStageExitEffect:
                    break;

                case SnailStatus.Byting:
                    this.SetChewingApple();
                    break;

                case SnailStatus.ExitingStage:
                    Stage.CurrentStage.Stats.NumSnailsSafe++;
                    this.DisposeFromStage();
                    break;
                case SnailStatus.Hidding:
                    break;
               
                default:
                    // The base onLastFrame method removes the object from the stage if it is dead
                    // On a snail this cannot happen. The snail death is diferent
                    // Snails fall from the stage until they are out of the stage area
                    // base.OnLastFrame was preventing this to happen
                    if (!this.IsDead)
                    {
                        base.OnLastFrame();
                    }
                    break;
            }
        }

        /// <summary>
        /// Occurs when object hits a tile
        /// Recieves the wall where the collision happened
        /// </summary>
        protected override void OnTileCollision(PathSegment.SegmentType wallType)
        {
            switch (this._snailStatus)
            {
                case SnailStatus.Hibernate:
                case SnailStatus.Hidding:
                    this.State = MovingState.Stopped;
                    break;

                case SnailStatus.Normal:
                    this.State = MovingState.Walking;
                    break;

                default:
                    this.State = MovingState.Stopped;
                    break;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void DoWalk(TwoBrainsGames.BrainEngine.BrainGameTime gameTime)
        {
#if DEBUG && DEBUG_ASSERTIONS
            if (this.Direction != WalkDirection.Clockwise &&
                this.Direction != WalkDirection.CounterClockwise)
            {
                throw new AssertionException("MovingObject direction must CW or CCW.");
            }

#endif
            // Update position
            this.Position += this.PathNode.Value.Normal * this.Speed * (int)this.Direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // Update collision box according with changes in Position
            this.UpdateBoundingBox();
            Vector2 startPoint = (Direction == WalkDirection.Clockwise ? this.PathNode.Value.P0 : this.PathNode.Value.P1);
            float distanceWalked = (startPoint - this.HeadPoint).Length();
            float excessWalked = distanceWalked - this.PathNode.Value.Length;
            // Object moved more than the size of the segment, make it move to a new segment
            if (excessWalked > 0)
            {
                //WalkDirection previousDir = this.Direction;
                BoardPathNode prevPathNode = this.PathNode;
                BoardPathNode nextPathNode = (this.Direction == WalkDirection.Clockwise ? this.PathNode.Next : this.PathNode.Previous);
                if (this.CanWalkOnPath(nextPathNode))
                {
                    this.PathNode = nextPathNode;

                    // Has the angle of the new path changed? If it did, we have to adjust the object to
                    // fit the new path orientation
                    if (this.PathNode.Value.Rotation != this.Rotation)
                    {
                        // The angle of the path has changed, this means that we have to play the turn
                        // animation

                        // Cross product between the paths normal vectors tells us if it is a inner or outer turn
                        // Because the order the cross product is done influences the return value, we have
                        // to use direction (which is 1 or -1) in the comparison
                        if (prevPathNode.Value.Classify(this.PathNode.Value) == (int)this.Direction)
                        {
                            this.SetStateToOuterTurn(excessWalked);
                        }
                        else
                        {
                            this.SetStateToInnerTurn(excessWalked);
                        }
                    }
                }
                else // Cannot walk on path
                {
                    this.InvertDirection();
                    this.SetTailInEndOfPath();
                    this.UpdateBoundingBox();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPathReversesWalk()
        {
            if (this.PathNode.Value.Behavior == PathSegmentBehavior.ReverseWalk)
            {
                return true;
            }
            if (this.PathNode.Value.Behavior == PathSegmentBehavior.WalkableCCW &&
                this.Direction != WalkDirection.CounterClockwise)
            {
                return true;
            }
            if (this.PathNode.Value.Behavior == PathSegmentBehavior.WalkableCW &&
                this.Direction != WalkDirection.Clockwise)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
 
            // if its attached to a copper path, than hide in shell
            if (this.IsAttachedToPath &&
                this._snailStatus != SnailStatus.Hibernate &&
                this.CurrentPathReversesWalk())
            {
                // Copper has ReverseWalk paths, it should be something like "CantWalk" path
                if (this.PathNode.Value.Behavior == PathSegmentBehavior.ReverseWalk)
                {
                    Hibernate();
                }
                else // This is a true reverse walk tile, invert direction
                {
                    this.InvertDirection();
                }
            }


            switch (this._snailStatus)
            {
                case SnailStatus.Chewing:
                    this._eatingElapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (this._eatingElapsedGameTime > Snail.EATING_APPLE_TIME)
                    {
                        //this.RemoveEatingApple();
                        if (this.Collides(this._apple) && this._apple.CanBeEaten)
                        {
                            this.SetBitingApple();
                        }
                        else
                        {
                            this.RemoveEatingApple();
                        }
                    }
                    break;

                case SnailStatus.Hidding:
                    if (!this.IsFalling)
                    {
                        this._genericTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (this._genericTimer < 0)
                        {
                            this.SetStateToWalk(); // Wake up the snail
                            this.UpdateBoundingBox();
                            this.RepositionObjectInQuadtree();
                            // WARNING: is there a reason to only check Tail collision with Salt HERE?!
                            //          this code as been commented because it was making snails to be hide in shell forever due to Tail collision check
                            List<IQuadtreeContainable> colObject = this.Quadtree.GetCollidingObjects(this, Stage.QUADTREE_STAGEOBJ_LIST_IDX);
                            bool headInSalt = false;
                            //bool tailInSalt = false;
                            foreach (IQuadtreeContainable obj in colObject)
                            {
                                Salt salt = obj as Salt;
                                if (salt != null)
                                {
                                    if (this.CheckCollisionWithHead(salt.AABoundingBox))
                                        headInSalt = true;
                                }

                                if (headInSalt) // Hhead already in salt, just break no need to do more checks
                                    break;
                            }

                            if (headInSalt)
                            {
                                this.InvertDirection();
                            }

                            this.StaticFlags |= StageObjectStaticFlags.CanWalkOnWalls;
                            this._snailStatus = SnailStatus.Normal;
                            this.SetStateToWalk();
                        }
                    }
                    break;

                case SnailStatus.Hibernate: // at this time this won't be used because we can't destroy Copper tiles
                    if (this.IsAttachedToPath &&
                       //this.PathNode.Value.Behavior != PathSegmentBehavior.ReverseWalk) // if loose attach
                        !this.CurrentPathReversesWalk())// if loose attach
                    {
                        this.StaticFlags |= StageObjectStaticFlags.CanWalkOnWalls;
                        this._snailStatus = SnailStatus.Normal;
                        this.SetStateToWalk();
                    }
                    break;
            }

            if (base.IsUnderLiquid && this._snailStatus != SnailStatus.DeathByWater)
            {
                _inWaterElapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this._inWaterElapsedGameTime > Snail.UNDER_WATER_TIME)
                {
                    this.KillByWater();
                }
            }

            foreach (SnailSpriteAccessory acessory in this._spriteAcessories)
            {
                acessory.Update(gameTime);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void AfterUpdate(BrainGameTime gameTime)
        {
            base.AfterUpdate(gameTime);
            
            if (!this.IsDead && !this.IsDisposed)
            { 
                // Breathing bubbles on snails
                if (this.IsUnderWater) 
                {
                    if (!this._bubblesAnim.Visible)
                    {
                        if (this._showBubblesEllapsedTime == 0) // First time reaching here, initialize the timer
                        {
                            this._showBubblesEllapsedTime = (Snail.UNDER_WATER_TIME - this._inWaterElapsedGameTime) * Snail.UNDER_WATER_BREATHING_MAX_TIME / Snail.UNDER_WATER_TIME;
                        }

                        this._showBubblesEllapsedTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                        // Time to show?
                        // -Time expired
                        // -More then 500 ms to kill the snail (to avoid bubbles anim cut because the snail dies)
                        // -Headpoint is at least at a depth of X (to avoid showing bubbles out of the water)
                        if (this._showBubblesEllapsedTime <= 0 &&
                            this._inLiquidRef.GetDepth(this.HeadPoint) > this._bubblesAnim.Sprite.Frames[0].Height &&
                            (this._inWaterElapsedGameTime < Snail.UNDER_WATER_TIME - this._bubblesAnim.TotalPlayTime))
                        {
                            this._bubblesAnim.Visible = true;
                            this._bubblesAnim.Reset();
                            this._showBubblesEllapsedTime = 0;
                            this._bubblesAnim.Position = this.GetHeadBBTramsformed().Center;
                            this._snailBreathUnderwaterSound.Play();
                        }
                    }
                    else
                    {
                      //  this._bubblesAnim.Position = this.GetHeadBBTramsformed().Center;
                        this._bubblesAnim.Update(gameTime);
                    }
                }
            }
        }

        /// <summary>
        /// This event runs when the object enters the water for the first time
        /// </summary>
        public override void OnEnterLiquid(Liquid liquid)
        {
            base.OnEnterLiquid(liquid);
            this.SetUnderWater();
            if (this.IsFalling)
            {
                if (!this.IsHidding)
                {
                    this.HideInShell(SnailStatus.Hidding);
                }
            }

            foreach (SnailSpriteAccessory access in this._spriteAcessories)
            {
                access.OnEnterLiquid();
            }

            if (this.IsVitaminized)
            {
                this._vitamin.OnSnailEnteredLiquid();
            }
        }

        /// <summary>
        /// This event runs when the object enters the water for the first time
        /// </summary>
        public override void OnExitLiquid()
        {
            base.OnExitLiquid();
            this.ResetUnderWater();

            foreach (SnailSpriteAccessory access in this._spriteAcessories)
            {
                access.OnExitLiquid();
            }


            if (this.IsVitaminized)
            {
                this._vitamin.OnSnailExitedLiquid();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnEnterStage()
        {
            this.ReleaseSlime();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReleaseSlime()
        {
            Slime slime = (Slime)Stage.CurrentStage.StageData.GetObject(Slime.ID);
            slime.Position = this.Position;
            slime.Rotation = this.Rotation;
            Stage.CurrentStage.AddObjectInRuntime(slime);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStageRemoved()
        {
            this.RemoveVitaminIfNeeded();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveVitaminIfNeeded()
        {
            if (this._vitamin != null)
            {
                this._vitamin.SnailRemoved();
                this._vitamin = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);

            foreach (SnailSpriteAccessory acessory in this._spriteAcessories)
            {
                acessory.Draw(shadow, Stage.CurrentStage.SpriteBatch);
            }

            if (this._bubblesAnim.Visible)
            {
                this._bubblesAnim.Draw(this._bubblesAnim.Position, 0, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
            }
#if DEBUG
            this.DrawQuadtreeNode(); // Draw is overriden and base.Draw() is not called
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnKill()
        {
            this.RemoveVitaminIfNeeded();
        }

        /// <summary>
        /// Kill with a projection out of the board
        /// </summary>
        public override void KillWithProjection()
        {
            this.PlayRandomDeathSound();
            this.KillMe();
            this.Rotation = 0;
            // - 45 - 135
            float angle = BrainGame.Rand.Next(90) - 135;
            float angleSin = (float)Math.Sin(MathHelper.ToRadians(angle));
            float angleCos = (float)Math.Cos(MathHelper.ToRadians(angle));
            Vector2 dir = new Vector2(angleCos, angleSin);
            this.DeathWithProjection(dir, 30f);
        }

        /// <summary>
        /// Under water death, snails dies and the shell floats in the water
        /// </summary>
        private void KillWithFloatingShell()
        {
            this.AddFloatingShell();
            this.OnKill();
        }


        /// <summary>
        /// When a snail is hit by a crate a simple project effec is added (similar to the explosion)
        /// </summary>
        public override void KillByCrate()
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrate++;
            }

            if (!this.IsUnderLiquid)
            {
                this.KillWithProjection();
            }
            else
            {
                this.KillWithFloatingShell();
            }
            this.OnKill();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void KillByTouchingDeadlyLiquid()
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByAcid++;
            }
            this.KillWithProjection();
            this.OnKill();
        }

        /// <summary>
        /// 
        /// </summary>
        public void KillByEvilSnail()
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByEvilSnail++;
            }
            this.KillWithProjection();
            this.OnKill();
        }

        /// <summary>
        /// Death is equal to the crate death
        /// </summary>
        public void KillByLaser()
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser++;
            }
            this.KillWithProjection();
            this.OnKill();
        }

        /// <summary>
        /// 
        /// </summary>
        public void KillBySpikes(Spikes spikes)
        {
            this.PlayRandomDeathSound();
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes++;
            }

            if (this.IsFalling)
            {
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.EmpaleOneSnailWhileFalling);
            }

            this.KillMe();
            this.Sprite = this.SpriteEmpaled;
            this.CurrentFrame = 0;
            this._SpritePlaybackMode = AnimtionPlaybackModes.PlayOnce;
            this._snailStatus = SnailStatus.DeathBySpikes;
            this.UpdateBoundingBox();
            if (!(this._rotation == 0 && spikes.Rotation == 180))
            {
                this._rotation = spikes.Rotation;
            }
            this.ProjectAccessories(true);
            this.OnKill();

            // Martelada!! Snails take too long to die when killed by spikes
            // We have to account the statistics immediately
            // In late development we don't want to leave bugs, so let's just do it like this
            if (this.AllowStageStatistics && !this._deathAccounted)
            {
                Stage.CurrentStage.Stats.NumSnailsDisposed++;
                this._deathAccounted = true;
            }
            if (this.AllowStageStatistics && !this._inactiveAccounted)
            {
                Stage.CurrentStage.Stats.NumSnailsActive--;
                this._inactiveAccounted = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void KillByExplosion(Explosion exp)
        {
            if (!this.IsEvilSnail)
            {
                if (exp.ExplosionSource is Box)
                {
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrateExplosion++;
                }
                if (exp.ExplosionSource is Dynamite)
                {
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite++;
                }
            }
            this.KillWithProjection();
            this.OnKill();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void KillByFire()
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire++;
            }
            base.KillByFire();
            this._snailStatus = SnailStatus.DeathByFire;
            this.ProjectAccessories(true);
            this.OnKill();
        }

        /// <summary>
        /// 
        /// </summary>
        public void KillBySacrificeSwitch()
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySacrifice++;
            }
            this.Hide();
            this.ProjectAccessories(true);
            this.OnKill();
        }


        /// <summary>
        /// 
        /// </summary>
        public void KillByWater()
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByWater++;
            }
            this.Kill();
            this.AddFloatingShell();
            this.OnKill();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddFloatingShell()
        {
            SnailShell shellObj = (SnailShell)Stage.CurrentStage.StageData.GetObject(SnailShell.ID);
            shellObj.Position = this.Position;
            shellObj.Rotation = this.Rotation;
            shellObj.Direction = this.Direction; 
            shellObj.SetDirection(shellObj.Direction);
            shellObj.FloatDeath(this._inLiquidRef, FLOAT_SPEED, (!this.IsHidding && !this.IsTurning));
            shellObj.UpdateBoundingBox();
            if (this.IsAttachedToPath)
            {
                // This will move the shell out of the wall if needed
                shellObj.AdjustObjectToPath(this.PathNode.Value, 0f, 0f, 0f, 0f);
            }
            Stage.CurrentStage.AddObjectInRuntime(shellObj);

            this.DisposeFromStage();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnHitFloor(bool floorIsBreakable)
        {
            this.PlayHitFloorImpactSound(floorIsBreakable);
            if (this.Rotation == 90)
            {
                if (this.Direction == WalkDirection.CounterClockwise)
                {
                    // Comentado porque se o snail está a andar numa parede e a parede desaparece
                    // ficava errado, testar com fadeout boxes
               //     this.InvertDirection(); 
                }
            }
            else if (this.Rotation == 270)
            {
                if (this.Direction == WalkDirection.Clockwise)
                {
                    // Comentado porque se o snail está a andar numa parede e a parede desaparece
                    // ficava errado, testar com fadeout boxes
                    //  this.InvertDirection();
                }
            }
            else if (this.Rotation == 180)
            {
                this.InvertDirection();
            }

   
            if (this._snailStatus == SnailStatus.Normal)
            {
                this.SetStateToWalk();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SpriteChanged()
        {
            foreach (SnailSpriteAccessory acessory in this._spriteAcessories)
            {
                if (acessory.Visible)
                {
                    acessory.UpdateActiveSprite();
                }
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        private BoundingSquare GetHeadBBTramsformed()
        {
            if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox) // Per frame BB?
            {   // Not all sprites have head BB
                if (Snail.HEAD_BB_INDEX < this.Sprite.Frames[this.CurrentFrame].BoundingBoxes.Length)
                {
                    return this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBoxes[Snail.HEAD_BB_INDEX]).ToBoundingSquare();
                }
            }
            else
            {   // Not all sprites have head BB
                if (Snail.HEAD_BB_INDEX < this.Sprite.BoundingBoxes.Length)
                {
                    return this.TransformBoundingBox(this.Sprite.BoundingBoxes[Snail.HEAD_BB_INDEX]).ToBoundingSquare();
                }
            }
            return new BoundingSquare();
        }

        /// <summary>
        /// 
        /// </summary>
        private BoundingSquare GetShellBBTramsformed()
        {
            if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox) // Per frame BB?
            {   // Not all sprites have head BB
                if (Snail.SHELL_BB_INDEX < this.Sprite.Frames[this.CurrentFrame].BoundingBoxes.Length)
                {
                    return this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBoxes[Snail.SHELL_BB_INDEX]).ToBoundingSquare();
                }
            }
            else
            {   // Not all sprites have head BB
                if (Snail.SHELL_BB_INDEX < this.Sprite.BoundingBoxes.Length)
                {
                    return this.TransformBoundingBox(this.Sprite.BoundingBoxes[Snail.SHELL_BB_INDEX]).ToBoundingSquare();
                }
            }
            return new BoundingSquare();
        }
        /// <summary>
        /// Check collision with a BB and the snail head
        /// </summary>
        public bool CheckCollisionWithHead(BoundingSquare bs)
        {
            return this.GetHeadBBTramsformed().Collides(bs);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckLineCollisionWithShell(Vector2 p0, Vector2 p1)
        {
            Vector2 v;
            return this.GetShellBBTramsformed().IntersectsLine(p0, p1, out v);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckCollisionWithTrampolim(BoundingSquare bs)
        {
            return this.CheckCollisionWithBB(Snail.TRAMPOLIM_BB_INDEX, bs);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckCollisionWithVitamin(BoundingSquare bs)
        {
            return this.CheckCollisionWithBB(Snail.VITAMIN_BB_INDEX, bs);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckCollisionWithLaserBeam(LaserBeam laser)
        {
            return (this.CheckLineCollisionWithShell(laser.BeamOrigin, laser.BeamEndPoint));
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CheckCollisionWithLiquid(Liquid liquid)
        {
            // If snail is hiddin, use the shell
            if (this.IsHidding)
            {
                return this.CheckCollisionWithBB(SHELL_BB_INDEX, liquid.QuadtreeCollisionBB);
            }

            return this.CheckCollisionWithHead(liquid.QuadtreeCollisionBB);
        }

        /// <summary>
        /// Check collision with a BB and the snail body
        /// </summary>
        public bool CheckCollisionWithBB(int bbIndex, BoundingSquare bs)
        {
            BoundingSquare bb;
            if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox) // Per frame BB?
            {   // Not all sprites have head BB
                if (bbIndex < this.Sprite.Frames[this.CurrentFrame].BoundingBoxes.Length)
                {
                    bb = this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBoxes[bbIndex]).ToBoundingSquare();
                    return (bb.Collides(bs));
                }
            }
            else
            {   // Not all sprites have head BB
                if (bbIndex < this.Sprite.BoundingBoxes.Length)
                {
                    bb = this.TransformBoundingBox(this.Sprite.BoundingBoxes[bbIndex]).ToBoundingSquare();
                    return (bb.Collides(bs));
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetEatingApple(Apple apple)
        {
            // Added this just for safety
            if (!this.CanEatApple)
            {
                return;
            }
            // take vitaminized if snail is at this state
            this.SetUnvitaminized();
            if (this.IsTurning)
            {
               
                this.EndTurning();
                if (this.Direction == WalkDirection.CounterClockwise)
                {
                    this.SetObjectLLCornerToPoint(apple.AABoundingBox.LowerLeft);
                }
                else
                {
                    this.SetObjectLRCornerToPoint(apple.AABoundingBox.LowerRight);
                }

            }
            this._apple = apple;
            this.SetBitingApple();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetBitingApple()
        {
            this._eatingElapsedGameTime = 0;
            this.Sprite = this.SpriteByting;
            this.CurrentFrame = 0;
            this._snailStatus = SnailStatus.Byting;
            this.State = MovingState.Stopped;
            this.DynamicFlags |= StageObjectDynamicFlags.IsEating;
            this._apple.SnailBite();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetChewingApple()
        {
            this._eatingElapsedGameTime = 0;
            this.Sprite = this.SpriteChewing;
            this.CurrentFrame = 0;
            this._snailStatus = SnailStatus.Chewing;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveEatingApple()
        {
            this._eatingElapsedGameTime = 0;
            this.SetStateToWalk();
            this.DynamicFlags &= ~StageObjectDynamicFlags.IsEating;
            this._snailStatus = SnailStatus.Normal;
            this._apple.SnailStoppedEating();
            this._apple = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetUnvitaminized()
        {
            this.DynamicFlags &= ~StageObjectDynamicFlags.IsVitaminized;
            this.AdjustSnailSpeed();
            this._rocketAccessory.Visible = false;
            this._helmetAccessory.Visible = false;
            if (this._vitamin != null)
            {
                this._vitamin.SnailRemoved();
                this._vitamin = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetVitaminized(Vitamin vitamin)
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts++;
            }
            this._vitamin = vitamin;
            this.DynamicFlags |= StageObjectDynamicFlags.IsVitaminized;
            this.AdjustSnailSpeed();
            this._rocketAccessory.Visible = true;
            this._helmetAccessory.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetUnderWater()
        {
            this.DynamicFlags |= StageObjectDynamicFlags.IsUnderLiquid;
            this.AdjustSnailSpeed();
            //this._frameUpdateMultiplier = Snail.WALKING_SPEED_UNDERWATER / Snail.WALKING_SPEED;
           // this._frameUpdateMultiplier = 0.8f;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AdjustSnailSpeed()
        {
            if (this.IsVitaminized)
            {
                this.Speed = Snail.WALKING_SPEED_VITAMINIZED;
            }
            else
            {
                this.Speed = Snail.WALKING_SPEED;
            }

            if (this.IsUnderLiquid)
            {
                this.Speed *= Snail.WALKING_SPEED_UNDERWATER;
            }
          
            this._frameUpdateMultiplier = this.Speed / Snail.WALKING_SPEED;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetUnderWater()
        {
            this._inWaterElapsedGameTime = 0;
            this.AdjustSnailSpeed();
            //this.Speed = Snail.WALKING_SPEED;
            //this._frameUpdateMultiplier = 1f;
        }

		/// <summary>
		/// 
		/// </summary>
		public void OnEnteringStageExit(StageExit exit)
		{
            if (_snailEnteringExit != null && !_snailEnteringExit.IsPlaying)
            {
                _snailEnteringExit.Play();
            }

			this.Sprite = this._EnteringStageExitSprite;
            this.CurrentFrame = 0;
            this._snailStatus = SnailStatus.ExitingStage;
            this.State = MovingState.None;
            this.Position = exit.DoorPosition;
            if (this.IsSnailKing)
            {
                Stage.CurrentStage.SnailKingDelivered();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void JumpOnTrampoline(Trampoline trampoline)
        {
            this.SetStateToAirborne();
            this.Rotation = 0;
            float angleSin = (float)Math.Sin(MathHelper.ToRadians(90.0f - trampoline.Angle));
            float angleCos = (float)Math.Cos(MathHelper.ToRadians(90.0f - trampoline.Angle));
            Vector2 direction = new Vector2(angleCos, -angleSin) * trampoline.JumpSpeed;

            // Invert snail direction depending on the direction it will be throwned,
            // use a threshold
            if ((this.Direction == WalkDirection.Clockwise && direction.X < -0.01) ||
                (this.Direction == WalkDirection.CounterClockwise && direction.X > 0.01))
            {
                this.InvertDirection();
            }


            this.Project(direction, trampoline.JumpSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Hibernate()
        {
            if (!this.IsEvilSnail)
            {
                SnailsGame.AchievementsManager.Notify((int)AchievementsType.HibernateASnail);
            }

            this.HideInShell(SnailStatus.Hibernate);
            this.DettachFromPath();
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideInShell(SnailStatus hiddingReason)
        {
            //int saveFrame = this.CurrentFrame;
            this.Rotation = 0;
            this.Sprite = this.SpriteHiddingInShell;
            this.CurrentFrame = 0;
            this.UpdateBoundingBox(); // Update BB because the sprite has changed
            this._SpritePlaybackMode = AnimtionPlaybackModes.PlayOnce;

            // Turns are special cases, this will force the corners of the BB in the corner of the path
            // This will avoid snails inside walls
            switch(this.State)
            {
                case MovingState.InnerTurning:
                    if (this.Direction == WalkDirection.Clockwise)
                    {
                        if (this.IsWalkingOnCeiling())
                        {
                            this.SetObjectURCornerToPoint(this.Position);
                            this.Position -= new Vector2(30f, 0f);
                        }
                        else
                        if (this.IsClimbingAWall())
                        {
                            this.SetObjectLLCornerToPoint(this.Position);
                        }
                        else
                        if (this.IsWalkingOnFloor())
                        {
                            this.SetObjectLRCornerToPoint(this.Position);
                        }
                        else // Going down a wall
                        {
                            this.SetObjectULCornerToPoint(this.Position);
                            this.Position += new Vector2(10f, 0f);
                        }
                    }
                    else
                    {
                        if (this.IsWalkingOnCeiling())
                        {
                            this.SetObjectLRCornerToPoint(this.Position);
                        }
                        else
                        if (this.IsClimbingAWall())
                        {
                            this.SetObjectLRCornerToPoint(this.Position);
                        }
                        else
                        if (this.IsWalkingOnFloor())
                        {
                            this.SetObjectLRCornerToPoint(this.Position);
                        }
                        else // Going down a wall
                        {
                            this.SetObjectURCornerToPoint(this.Position + new Vector2(-10f, 0f)); // This -1 was a last minute hammering because o DB&P
                                                                                                 // this takes the snail away from the wall. I think the others ifs should get
                                                                                                 // the same treatment
                        } 
                    }
                    this.UpdateBoundingBox(); // Update BB because the position has changed
                    break;

                case MovingState.OuterTurning:
                    if (this.Direction == WalkDirection.Clockwise)
                    {
                        if (this.IsWalkingOnFloor())
                        {
                            this.SetObjectLRCornerToPoint(this.Position);
                            this.Position -= new Vector2(10f, 0f);
                        }
                        else
                        if (this.IsClimbingAWall())
                        {
                            this.SetObjectURCornerToPoint(this.Position);
                            this.Position += new Vector2(-10f, 1f);
                        }
                        else
                        if (this.IsWalkingOnCeiling())
                        {
                            this.SetObjectULCornerToPoint(this.Position);
                            this.Position += new Vector2(1f, 1f);
                        }
                        else
                        {
                            this.SetObjectLLCornerToPoint(this.Position);
                            this.Position += new Vector2(10f, 0f);
                        }
                    }
                    else
                    {
                        if (this.IsWalkingOnFloor())
                        {
                            this.SetObjectLLCornerToPoint(this.Position);
                            this.Position += new Vector2(10f, 0f);
                        }
                        else
                        if (this.IsClimbingAWall())
                        {
                            this.SetObjectULCornerToPoint(this.Position);
                            this.Position += new Vector2(10f, 1f);
                        }
                        else
                        if (this.IsWalkingOnCeiling())
                        {
                            this.SetObjectURCornerToPoint(this.Position);
                            this.Position += new Vector2(-1f, 1f); // N sobtraír os -10 neste
                        }
                        else
                        {
                            this.SetObjectLRCornerToPoint(this.Position);
                            this.Position -= new Vector2(10f, 0f);
                        }
                    }
                    this.UpdateBoundingBox(); // Update BB because the position has changed
                    this.DettachFromPath(); // Detatch from path, because with object adujstments to corners, snail
                                            // might be out of the tiles
                    break;

                // If not in a corner, adjust the snail to whatever path is walking on
                default:
                    this.AdjustObjectToPath(10f);
                    break;
            }
            this.State = MovingState.Stopped;

            this._snailStatus = hiddingReason;
            if (this._snailStatus == SnailStatus.Hidding)
            {
                this._genericTimer = SNAIL_HIDDING_SCARE_TIME;
            }

            this.StaticFlags &= ~StageObjectStaticFlags.CanWalkOnWalls;

            if (this.IsAttachedToPath && this.PathNode.Value.WallType != PathSegment.SegmentType.Floor)
            {
                this.DettachFromPath();
            }
        }

        /// <summary>
        /// </summary>
        public bool CollidedWithLiquidTap(LiquidTap tap)
        {
            if (this.IsAttachedToPath &&
                this.State == MovingState.Walking)
            {
                return this.CheckCollisionWithHead(tap.AABoundingBox);
            }
            return false;
        }

        /// <summary>
        /// </summary>
        public void CollidedWithSalt(Salt salt)
        {
            // Not turning
            if (!this.IsTurning)
            {
                if (this.IsWalkingOnCeiling())
                {
                    this.Rotation = 0; // remove rotation, this way the snail will not fall on it's back.
                    this.InvertDirection();
                }
                else if (this.IsClimbingAWall())
                {
                    this.InvertDirection();
                }
                this.HideInShell(SnailStatus.Hidding);
            }
            else if (this.IsOuterturning)
            {
                bool isFirstHalfOfAnimation = (this.CurrentFrame < (this.Sprite.FrameCount / 2));
                if (this.IsWalkingOnCeiling())
                {
                    this.Rotation = 0;
                    this.HideInShell(SnailStatus.Hidding);
                    if (!isFirstHalfOfAnimation)
                    {
                        if (this.Direction == WalkDirection.Clockwise)
                        {
                            this.SetObjectULCornerToPoint(salt.AABoundingBox.UpperRight + new Vector2(0f, 1f)); // Subtract 1 because we don't want the snailt to be inside of the path
                        }
                        else
                        {
                            this.SetObjectURCornerToPoint(salt.AABoundingBox.UpperLeft + new Vector2(0f, 1f));
                        }
                        this.InvertDirection();
                    }
                }
                else
                if (this.IsWalkingOnFloor())
                {
                    this.HideInShell(SnailStatus.Hidding);
                    if (!isFirstHalfOfAnimation)
                    {
                        if (this.Direction == WalkDirection.Clockwise)
                        {
                            this.SetObjectLRCornerToPoint(salt.AABoundingBox.LowerLeft + new Vector2(0f, -1f)); // Subtract 1 because we don't want the snailt to be inside of the path
                        }
                        else
                        {
                            this.SetObjectLLCornerToPoint(salt.AABoundingBox.LowerRight + new Vector2(0f, -1f));
                        }
                    }
                    else
                    {
                        this.InvertDirection();
                    }
                }
                else if (this.IsClimbingAWall())
                {
                    this.HideInShell(SnailStatus.Hidding);
                    this.InvertDirection();
                    if (isFirstHalfOfAnimation)
                    {
                        if (this.Direction == WalkDirection.Clockwise)
                        {
                            this.SetObjectURCornerToPoint(salt.AABoundingBox.LowerLeft + new Vector2(0f, +1f)); // Subtract 1 because we don't want the snailt to be inside of the path
                        }
                        else
                        {
                            this.SetObjectULCornerToPoint(salt.AABoundingBox.LowerRight + new Vector2(0f, +1f));
                        }
                    }
                }
                else // Going down a wall
                {
                    //Vector2 storePosition = this.Position; // Because hideInShell is going to change 
                    this.HideInShell(SnailStatus.Hidding);

                }
            }
            else // Inner turning
            {
                bool isFirstHalfOfAnimation = (this.CurrentFrame < (this.Sprite.FrameCount / 2));
                if (this.IsClimbingAWall())
                {
                    if (!isFirstHalfOfAnimation)
                    {
                        Vector2 storePosition = this.Position;
                        this.HideInShell(SnailStatus.Hidding);
                        this.InvertDirection();
                        if (this.Direction == WalkDirection.Clockwise)
                        {
                            this.SetObjectLLCornerToPoint(storePosition + new Vector2(1f, -1f));
                        }
                        else
                        {
                            this.SetObjectLRCornerToPoint(storePosition + new Vector2(-1f, -1f));
                        }
                    }
                }
                else
                if (this.IsWalkingOnCeiling())
                {
                    Vector2 storePosition = this.Position;
                    this.HideInShell(SnailStatus.Hidding);
                    this.InvertDirection();
                    if (this.Direction == WalkDirection.Clockwise)
                    {
                        this.SetObjectULCornerToPoint(storePosition + new Vector2(1f, 1f));
                    }
                    else
                    {
                        this.SetObjectURCornerToPoint(storePosition + new Vector2(-1f, 1f));
                    }
                }
                else
                if (this.IsWalkingOnFloor())
                {
                    Vector2 storePosition = this.Position;
                    this.HideInShell(SnailStatus.Hidding);
                    if (this.Direction == WalkDirection.Clockwise)
                    {
                        this.SetObjectLLCornerToPoint(storePosition + new Vector2(1f, 1f));
                    }
                    else
                    {
                        this.SetObjectLRCornerToPoint(storePosition + new Vector2(-1f, 1f));
                    }
                }
                else
                {
                    this.HideInShell(SnailStatus.Hidding);
                }
            }
        }

        /// <summary>
        /// THis applies the jump out of stage death animation 
        /// </summary>
        public void DeathWithProjection(Vector2 direction, float speed)
        {
            this.Project(direction, speed);
            this.ProjectAccessories(true);
            this._snailStatus = SnailStatus.DeathWithStageExitEffect;
            this.Sprite = this.DeathWithStageExitSprite;
            this.CurrentFrame = 0;
        }



        /// <summary>
        /// 
        /// </summary>
        public void UnempaleFromSpikes()
        {
            this.EffectsBlender.Add(new GravityEffect(SnailsGame.GameSettings.Gravity, 0f), StageObject.TRANSF_EFFECT_GRAVITY);
            this.Sprite = this.SpriteDeadBrokenShell;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ProjectAccessories(bool projectCrown)
        {
            for (int i = 0; i < this._spriteAcessories.Count; i++ )
            {
                SnailSpriteAccessory acessory = this._spriteAcessories[i];
                if (acessory.Visible)
                {
                    if (acessory is CrownAccessory && projectCrown == false)
                    {
                        continue;
                    }
                    acessory.Project();
                    this._spriteAcessories.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsClimbingAWall()
        {
            if (!this.IsAttachedToPath)
            {
                return false;
            }
            if (this.PathNode == null)
            {
                return false;
            }

            if (this.Direction == WalkDirection.Clockwise && 
                this.PathNode.Value.WallType == PathSegment.SegmentType.RightWall)
            {
                return true;
            }

            if (this.Direction == WalkDirection.CounterClockwise &&
                this.PathNode.Value.WallType == PathSegment.SegmentType.LeftWall)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsWalkingOnCeiling()
        {
            return (this.IsAttachedToPath &&
                   this.PathNode != null &&
                   this.PathNode.Value.WallType == PathSegment.SegmentType.Ceiling);
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsWalkingOnFloor()
        {
            return (this.IsAttachedToPath &&
                    this.PathNode != null &&
                    this.PathNode.Value.WallType == PathSegment.SegmentType.Floor);
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsAttachedtoFloor()
        {
            return (this.IsAttachedToPath &&
                   this.PathNode != null &&
                   this.PathNode.Value.WallType == PathSegment.SegmentType.Floor);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void PlayRandomDeathSound()
        {
            int sound = BrainGame.Rand.Next(this._soundsGenericSnailDeath.Length);
            this._soundsGenericSnailDeath[sound].Play();
        }

        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._allowRocketPickUp = record.GetFieldValue<bool>("allowRocketPickUp", true);
            this._allowEatApple = record.GetFieldValue<bool>("allowEatApple", true);
            this._allowAffectedBySalt = record.GetFieldValue<bool>("allowAffectedBySalt", true);
            this._allowJumpOnTrampoline = record.GetFieldValue<bool>("allowJumpOnTrampoline", true);
            this._allowObjectPickup = record.GetFieldValue<bool>("allowObjectPickup", true);
            this.AllowStageStatistics = record.GetFieldValue<bool>("allowStageStatistics", true);
            this._allowKilledByEvilSnail = record.GetFieldValue<bool>("allowKilledByEvilSnail", true);
            this._allowSwitchActivation = record.GetFieldValue<bool>("allowSwitchActivation", true);
            this._allowEnterStageExit = record.GetFieldValue<bool>("allowEnterStageExit", true);
        
        }

        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            switch (context)
            {
                case ToDataFileRecordContext.StageDataSave:
                    record.AddField("allowRocketPickUp", this._allowRocketPickUp);
                    record.AddField("allowEatApple", this._allowEatApple);
                    record.AddField("allowAffectedBySalt", this._allowAffectedBySalt);
                    record.AddField("allowJumpOnTrampoline", this._allowJumpOnTrampoline);
                    record.AddField("allowObjectPickup", this._allowObjectPickup);
                    record.AddField("allowStageStatistics", this.AllowStageStatistics);
                    record.AddField("allowKilledByEvilSnail", this._allowKilledByEvilSnail);
                    record.AddField("allowSwitchActivation", this._allowSwitchActivation);
                    record.AddField("allowEnterStageExit", this._allowEnterStageExit);
                    break;
                case ToDataFileRecordContext.StageSave:
                    break;
            }
            return record;
        }
        #endregion
    }
}
