using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Debugging;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Effects;
using System;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class MovingObject : StageObject
    {
        public enum WalkDirection
        {
            None,
            Clockwise = 1,
            CounterClockwise = -1
        }


        public enum CollidingCorner
        {
            Node,
            UpperLeft,
            UpperRight,
            LowerLeft,
            LowerRight
        }

        public enum MovingState
        {
            None,
            Stopped,
            Walking,
            InnerTurning,
            OuterTurning,
            Airbone
		}

        #region Constants
        const string SPRITE_SNAIL_WALK = "SnailWalk";
        const string SPRITE_SNAIL_TURN_UP = "SnailTurnUp";
        const string SPRITE_SNAIL_TURN_DOWN = "SnailTurnDown";
        const float BOUNCE_XSPEED_DECAY = 0.8f;
        #endregion

        #region Members
        public WalkDirection Direction;
        public float Speed;
		protected Vector2 PivotPoint;
		protected MovingState State;
		public Sprite InnerTurnSprite;
        public Sprite OuterTurnSprite;
        public Sprite WalkSprite;
		protected BoardPathNode PathNode;
        protected MotionEffect _gravityEffect; // This includes gravity mixed with a direction/speed vector called the initial speed
        protected LiquidGravityEffect _liquidGravityEffect;
        private HooverEffect _hooverEffect;
        protected SpriteAnimation _bubblesMovementAnim; // Bubbles animation used when objects are moving in the water
        private Sample _bubblesSound; // Sound of the bubbles
        protected float _fallingHeight;
        Sample _impactOnTileSound;
        Sample _impactOnBreakableTileSound;
        Sample _inpactOnTileUnderwater;
        #endregion

		#region Properties
		// this.PathNode should be moved out of the StageObject and even removed and replaced entirely by
        // this property


        public PathSegment.SegmentType WalkSegmentType
        {
             get 
             {
                 if (this.IsAttachedToPath == false)
                     return PathSegment.SegmentType.None;

                 return this.PathNode.Value.WallType;
             }
        }

        public Vector2 HeadPoint 
        {
          // The BoundingBox, is an object aligned bounding box, so p2 will always be the head 
          // (lowerRight corner or lowerLeft corner if this object is moving  ccw)
          get { return (this.Direction == WalkDirection.Clockwise? this.BoundingBox.P2 : this.BoundingBox.P3); }
        }

        public Vector2 TailPoint
        {
          // The BoundingBox, is an object aligned bounding box, so p2 will always be the head 
          // (lowerRight corner or lowerLeft corner if this object is moving  ccw)
          get { return (this.Direction == WalkDirection.Clockwise ? this.BoundingBox.P3 : this.BoundingBox.P2); }
        }

        public bool IsAttachedToPath
        {
            get { return (this.PathNode != null && this.PathNode.Value != null); }
        }

        public bool IsTurning
        {
            get
            {
                return (this.State == MovingState.InnerTurning ||
                        this.State == MovingState.OuterTurning);
            }
        }

        public bool IsOuterturning
        {
            get
            {
                return (this.State == MovingState.OuterTurning);
            }
        }

        public bool IsInnerTurning
        {
            get
            {
                return (this.State == MovingState.InnerTurning);
            }
        }

         #endregion

        /// <summary>
        /// 
        /// </summary>
        public MovingObject(StageObjectType objType):
            base(objType)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public MovingObject(StageObject other):
            base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            MovingObject obj = other as MovingObject;
            this.WalkSprite = obj.WalkSprite;
            this.InnerTurnSprite = obj.InnerTurnSprite;
            this.OuterTurnSprite = obj.OuterTurnSprite;
            this.Direction = obj.Direction;
            this.Speed = obj.Speed;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            // Pre-load sprites
            this.WalkSprite = this.Sprite;
            // This only applies to objects that walk on walls
            if (this.CanWalkOnWalls)
            {
                this.InnerTurnSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_TURN_UP);
                this.OuterTurnSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, Snail.SPRITE_SNAIL_TURN_DOWN);
            }

            this._bubblesMovementAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/water", "Bubbles"));
            this._bubblesMovementAnim.Visible = false;
            this._bubblesSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.UNDERWATER_BUBBLES);

            if (Stage.CurrentStage != null)
            {
                this._impactOnTileSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TILE_IMPACT, this);
                this._impactOnBreakableTileSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.BREAKBLE_TILE_IMPACT, this);
                this._inpactOnTileUnderwater = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TILE_IMPACT_UNDERWATER, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            if (!this.EffectsBlender.Contains(TRANSF_EFFECT_GRAVITY))
            {
                this._gravityEffect = new MotionEffect(SnailsGame.GameSettings.Gravity, Vector2.Zero);
                this._gravityEffect.Active = false; // Make it inactive by default
                this.EffectsBlender.Add(this._gravityEffect, TRANSF_EFFECT_GRAVITY);
            }

            this._liquidGravityEffect = new LiquidGravityEffect(Water.WATER_GRAVITY);
            this._liquidGravityEffect.Active = false;
            this.EffectsBlender.Add(this._liquidGravityEffect, TRANSF_EFFECT_LIQUID_GRAVITY);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void EndTurning()
        {
            this.Sprite = this.WalkSprite;
            this.CurrentFrame = 0;
            this.State = MovingState.Walking;
            // Is possible that the snail is not attached to a path anymore
            // This could happen if lastframe occurs in the same loop has the snail is dettached from a path
            if (this.IsAttachedToPath)
            {
                this.Rotation = this.PathNode.Value.Rotation;
                base.UpdateBoundingBox();
                // Place the object at the beggining of the path
                this.PlaceObjectInBeginingPath();
            }
            // Update collision box according with new rotation
            // Add the excess that was walked to the object
            //    this.Position += this.PathNode.Value.Normal * excessWalked * (int)this.Direction;
            // Update with new positon
            base.UpdateBoundingBox();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Hide()
        {
            base.Hide();
            this.HideBubbles();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            if (this.IsDead) // A death animation may be playing
            {
                base.OnLastFrame();
                return;
            }
            
            // Object was playing the turn animations, because the animation ended, it's time to make it walk again
            if (this.State == MovingState.InnerTurning ||
                this.State == MovingState.OuterTurning)
            {
                this.EndTurning();
                
            }
        }
        /// <summary>
        /// This event runs when the object enters the water for the first time
        /// </summary>
        public override void OnEnterLiquid(Liquid liquid)
        {
            base.OnEnterLiquid(liquid);
            if (this.IsFalling)
            {
                this.AddGravityEffect(Vector2.Zero);
            }
        }

        /// <summary>
        /// This event runs when the object enters a liquid for the first time
        /// </summary>
        public override void OnExitLiquid()
        {
            base.OnExitLiquid();
            this.HideBubbles();
        }

        /// <summary>
        /// Places the object at the beggining of the path (object tail is placed at the begining of the path)
        /// </summary>
        protected void PlaceObjectInBeginingPath()
        {
            if (this.Direction == WalkDirection.Clockwise)
            {
              this.Position = this.PathNode.Value.P0;
			  this.Position += this.PathNode.Value.Normal * -this.Sprite.BoundingBox.Left;
            }
            else
            {
              this.Position = this.PathNode.Value.P1;
			  this.Position -= this.PathNode.Value.Normal * -this.Sprite.BoundingBox.Left;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BoardPathNode[] GetCollidingPaths(BoundingSquare bs)
        {
            List<IQuadtreeContainable> paths = Stage.CurrentStage.Board.Quadtree.GetCollidingObjects(bs, Stage.QUADTREE_PATH_LIST_IDX);
            if (paths.Count == 0)
            {
                return null;
            }
            BoardPathNode[] nodes = new BoardPathNode[paths.Count];
            for (int i = 0; i < paths.Count; i++)
            {
                nodes[i] = (BoardPathNode)paths[i];
            }
            return nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckCollisionsWithPaths(OOBoundingBox bbOld)
        {
            Vector2 pI; // point of intersection
            BoardPathNode pathI; // path of intersection
            CollidingCorner cornerOfColision;
            Vector2 bbCollidingPoint;

            // Check for collisions
            if (this.CanCollide)
            {

                // First step, check if we are starting from a position where we are already colliding
                // This can only be possible if the oldBB is colliding with a path and the new BB equals
                // to the old (this means the object isn't moving)
                if (bbOld.Equals(this.BoundingBox))
                {
                    // Get all paths that are colliding with the bb and adjust position acordingly
                    // This is not taking into account if the object is still colliding after reacting to the collisions with the paths
                    // This should only happen for objects bigger then a tile (deal with that someday...)
                    BoundingSquare bs = bbOld.ToBoundingSquare();
                    BoardPathNode[] nodes = this.GetCollidingPaths(bs);
                    if (nodes != null)
                    {
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            this.AdjustObjectToPath(nodes[i].Value, 1f, 1f, 1f, 0f);
                            this.UpdateBoundingBox();
                        }
                    }
                    bbOld = this.BoundingBox;
                }
                
                // Collides returns the number of collisions (a double collision may happen on corners)

                // Beware, bugs reacting to collisions may lead to an infinite loop
                // Begin of not so good code
                // First pass, test collisions in the current object treenode (collides only tests paths in the object treenodes)
                int nLoop = 0;
                int nColides = 0;
                while (Stage.CurrentStage.Board.Collides(this, ref bbOld, ref this.BoundingBox, out pI, out pathI, out cornerOfColision, out bbCollidingPoint) > 0)
                {
                    // Reaction to collision depends on the segment that was hit
                    this.ReactToColision(pI, pathI, cornerOfColision, bbCollidingPoint);
                    nColides++;
                    if (this.IsAttachedToPath)
                    {
                        // Break the loop if the object was attached to a path
                        break;
                    }
#if DEBUG
                    // Control the infinte loop. 10 loops is used to break this. Less then 10 is already bad news...
                    nLoop++;
                    if (nLoop > 10)
                        throw new BrainException("Infinite loop in collision detection!");
#else
                    if (nLoop > 10)
                    {
                        break; // Ignore in release
                    } 
#endif
                }
                // No colision happened, well, if the objects new position made him move to a new treenode,
                // we have to test colisions in the paths in the new node
                // This could be unecessary if the object didn't move in the tree, we are no testing for that but we should
                if (nColides == 0)
                {
#if DEBUG
                    nLoop = 0;
#endif
                    // Reposition the object in the tree
                    this.Quadtree.ObjectMoved(this);
                    // Test collisions in the new node
                    while (Stage.CurrentStage.Board.Collides(this, ref bbOld, ref this.BoundingBox, out pI, out pathI, out cornerOfColision, out bbCollidingPoint) > 0)
                    {
                        // Reaction to collision depends on the segment that was hit
                        this.ReactToColision(pI, pathI, cornerOfColision, bbCollidingPoint);
                        if (this.IsAttachedToPath)
                        {
                            // Break the loop if the object was attached to a path
                            break;
                        }
#if DEBUG
                        nLoop++;
                        if (nLoop > 10)
                            throw new BrainException("Infinite loop in collision detection!");
#else
                        if (nLoop > 10)
                        {
                           break; // Ignore in release
                        } 
#endif
                    }
                }
              
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void DoWalk(TwoBrainsGames.BrainEngine.BrainGameTime gameTime)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(TwoBrainsGames.BrainEngine.BrainGameTime gameTime)
        {
            this.PreviousPosition = this.Position;
            OOBoundingBox bbOld = this.BoundingBox;
            base.Update(gameTime);

			if (this.Killed)// This is a major martelada. This was supposed to be a temporary fix, but the truth is because I didn't commented this crap
							// Now I don't remember why I had to do this. Anyways, use this.Kill() to kill objects
            {
                this.Sprite = this.WalkSprite; // TODO this should be the kill animation
                this.CurrentFrame = 0;
                if (this.BoundingBoxChanged)
                {
                    this.RepositionObjectInQuadtree();
                }
                return;
            }
            // Object could have been disposed by it's base or killed
            // Don't move or do collisio detectection for dead or disposed objects
            if (this.IsDisposed ||
                this.IsDead)
            {
                return;
            }

            if (this.IsAttachedToPath && 
                this.CanWalk &&
                this.State == MovingState.Walking)
            {
                // Make the object walk
                this.DoWalk(gameTime);
            }

            // Process collision detection if object is not attached to a path
            if (this.IsAttachedToPath == false)
            {
                // Make if fall if it is not already
                if (this.CanFall && !this.IsFalling)
                {

#if DEBUG && DEBUG_ASSERTIONS
                    // Assert that the object doesn't already contain a gravity effect. This should never happen
                    if (this.EffectsBlender.Contains(StageObject.TRANSF_EFFECT_GRAVITY))
                        throw new AssertionException("Assertion failed. MovingObject already contains a gravity effect.");
#endif
                    this.SetStateToAirborne();

                }

                if (!this.IgnorePathCollisions && 
                     this.CanCollide)
                {
                    this.CheckCollisionsWithPaths(bbOld);
                }
            }

            if (this._bubblesMovementAnim.Visible)
            {
                this._bubblesMovementAnim.Update(gameTime);
            }

            if (this.IsFalling)
            {
                if (this.PreviousPosition.Y < this.Position.Y)
                {
                    this._fallingHeight += this.Position.Y - this.PreviousPosition.Y;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStageRemoved()
        {
            this._bubblesSound.Stop();
        }

        /// <summary>
        /// Removes the gravity effect
        /// </summary>
        protected void RemoveGravityEffect()
        {
            this._gravityEffect.Active = false;
            this._liquidGravityEffect.Active = false;
            this.HideBubbles();
        }

        /// <summary>
        /// Activates the gravity effect
        /// Recieves an initial speed/direction vector
        /// </summary>
        protected void AddGravityEffect(Vector2 initialSpeed)
        {
            if (this.IsUnderLiquid)
            {
                // Remove normal gravity effect, it might be set
                this._gravityEffect.Active = false;

                // Set gravity effect for liquids
                this._gravityEffect.Active = false;
                this._liquidGravityEffect.Active = true;
                this._liquidGravityEffect.Reset();

                this._bubblesMovementAnim.Visible = true;
                this._bubblesSound.Play(true);
            }
            else
            {
                // Remove normal gravity effect, it might be set
                this._liquidGravityEffect.Active = false;

                // Set gravity effect for air
                this._gravityEffect.Active = true;
                this._gravityEffect.Reset(initialSpeed);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void HideBubbles()
        {
            this._bubblesMovementAnim.Visible = false;
            this._bubblesSound.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void PlayHitFloorImpactSound(bool tileIsBreakble)
        {
            // Play impact sound - ignore for small falls, half tile seems ok
            if (this._fallingHeight > (Stage.TILE_HEIGHT / 2))
            {
                // Set sound volume depending on the falling height
                float volume = this._fallingHeight / (Stage.TILE_HEIGHT * 4);
                if (volume > 1f)
                {
                    volume = 1f;
                }
                if (this.IsUnderLiquid)
                {
                    this._inpactOnTileUnderwater.Play();
                }
                else
                {
                    if (tileIsBreakble)
                    {
                        this._impactOnBreakableTileSound.Play();
                    }
                    else
                    {
                        this._impactOnTileSound.Play();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnHitFloor(bool floorIsBreakable)
        {
            this.PlayHitFloorImpactSound(floorIsBreakable);
            if (this.CanWalk)
            {
                this.SetStateToWalk();
            }
        }

        /// <summary>
        /// Occurs when object hits a tile
        /// Recieves the wall where the collision happened
        /// </summary>
        protected virtual void OnTileCollision(PathSegment.SegmentType wallType)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ReactToColision(Vector2 collisionPoint, BoardPathNode nodeOfCollision, CollidingCorner cornerOfColision, Vector2 bbCollidingPoint)
        {
            // Check if the object should be attached to the path segment
            if (nodeOfCollision.Value.WallType == PathSegment.SegmentType.Floor ||
                ( this.CanWalkOnWalls && 
                  nodeOfCollision.Value.WallType != PathSegment.SegmentType.Floor &&
                  nodeOfCollision.Value.Behavior != PathSegmentBehavior.ReverseWalk) )
              //  this.CanWalkOnPath(nodeOfCollision)))
            {
                this.PathNode = nodeOfCollision;
                if (nodeOfCollision.Value.WallType== PathSegment.SegmentType.Floor)
                {

                    this.OnHitFloor(nodeOfCollision.Value.FloorIsBreakable);
                }
                else
                if (nodeOfCollision.Value.WallType == PathSegment.SegmentType.Ceiling)
                {
                    this.InvertDirection();
                }

                this.Rotation = this.PathNode.Value.Rotation;
                this.AdjustObjectToPath(0f);

                // Remove effects
                this.RemoveHooverEffect();
                this.RemoveGravityEffect();

                // Remove falling flag
                this.DynamicFlags &= ~StageObjectDynamicFlags.IsFalling;
                //this.SetStateToWalk();
                // Object is static on the ground, apply a hoover effect if that's the case
                if (this.CanHoover)
                {
					this.AddHooverEffect();
                }

                this.OnTileCollision(nodeOfCollision.Value.WallType);
            }
            else // Wall or ceiling colision for objects that can't walk on walls
            {
                // Remove all effects except the gravity effect
                //this.EffectsBlender.DeleteEffectsExcept(StageObject.TRANSF_EFFECT_GRAVITY);

                // What follows is some "ugly code" to force the object bounding box to touch the path. The +1.0f is used to
                // place the object 1 pixel away from the path, this avoids infinite collisions to occour
                switch (nodeOfCollision.Value.WallType)
                {
                    case PathSegment.SegmentType.Ceiling:
                        this.AddGravityEffect(new Vector2(this._gravityEffect.CurrentSpeed.X, 0f));
                        this.Position = new Vector2(this.Position.X, collisionPoint.Y - this.Sprite.BoundingBox.Top + 1.0f);
                        break;
                    case PathSegment.SegmentType.LeftWall:
                        this.AddGravityEffect(new Vector2(-this._gravityEffect.CurrentSpeed.X * BOUNCE_XSPEED_DECAY, this._gravityEffect.CurrentSpeed.Y));
            		//	this.Position = new Vector2(collisionPoint.X - this.Sprite.BoundingBox.Left + 1.0f, this.Position.Y);
                        this.Position = new Vector2(this.Position.X - (this.AABoundingBox.Left - collisionPoint.X) + 1f, this.Position.Y);
                        if (this.Direction == WalkDirection.CounterClockwise)
                        {
                            this.InvertDirection();
                        }
                        break;
                    case PathSegment.SegmentType.RightWall:
                        this.AddGravityEffect(new Vector2(-this._gravityEffect.CurrentSpeed.X * BOUNCE_XSPEED_DECAY, this._gravityEffect.CurrentSpeed.Y));
						this.Position = new Vector2(collisionPoint.X - this.Sprite.BoundingBox.Width - this.Sprite.BoundingBox.Left - 1.0f, this.Position.Y);
                        if (this.Direction == WalkDirection.Clockwise)
                        {
                            this.InvertDirection();
                        }
   break;

                    // Floor should not reach here. Objects that hit the floor are always attached to the ground
                    // those cases are treated if the upper if
                    //case PathSegment.SegmentType.Floor:
                }
            }
            this.UpdateBoundingBox();
            this.Quadtree.ObjectMoved(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InvertDirection()
        {
          this.SetDirection((WalkDirection)((int)(this.Direction) * -1));
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetDirection(WalkDirection walkDirection)
        {
            this.Direction = walkDirection;
            if (this.Direction == WalkDirection.Clockwise)
            {
                this.SpriteEffect = SpriteEffects.None;
            }
            else
            {
                this.SpriteEffect = SpriteEffects.FlipHorizontally;
            }
        }


        /// <summary>
        /// Determines if the object can walk on the specified path
        /// </summary>
        protected virtual bool CanWalkOnPath(BoardPathNode node)
        {
            if (node == null)
                return false;

            if (node.Value.Behavior == PathSegmentBehavior.WalkableCW &&
                this.Direction != WalkDirection.Clockwise)
            {
                return false;
            }

            if (node.Value.Behavior == PathSegmentBehavior.WalkableCCW &&
                this.Direction != WalkDirection.CounterClockwise)
            {
                return false;
            }

            if (node.Value.Behavior == PathSegmentBehavior.ReverseWalk ||
                node.Value.Behavior == PathSegmentBehavior.None)
                return false;

            if (node.Value.WallType != PathSegment.SegmentType.Floor &&
                    !this.CanWalkOnWalls)
                return false;

            return true;
        }

        /// <summary>
        /// Determines if the object can walk on the next segment
        /// </summary>
        protected virtual bool CanWalkOnNextSegment()
        {
          /*  if (this.PathNode.Next == null)
                return false;
            if (this.PathNode.Next.Value == null)
                return false;
            if (this.PathNode.Next.Value.Behavior != PathSegmentBehavior.Walkable)
                return false;
            if (this.PathNode.Next.Value.WallType != PathSegment.SegmentType.Floor && 
                    !this.CanWalkOnWalls)
                return false;
            
            return true;*/
            return (this.CanWalkOnPath(this.PathNode.Next));
        }

        /// <summary>
        /// Determines if the object can walk on the prev segment
        /// </summary>
        protected virtual bool CanWalkOnPrevSegment()
        {
    /*        if (this.PathNode.Previous == null)
                return false;
            if (this.PathNode.Previous.Value == null)
                return false;
            if (this.PathNode.Previous.Value.Behavior != PathSegmentBehavior.Walkable)
                return false;
            if (this.PathNode.Previous.Value.WallType != PathSegment.SegmentType.Floor && 
                    !this.CanWalkOnWalls)
                return false;

            return true;*/
            return (this.CanWalkOnPath(this.PathNode.Previous));
        }

        /// <summary>
        /// Places the object at the corner of a turn and sets the turn animation
        /// excessWalk is the value that the object walked behound the tile limit. We use this to set the current frame
        /// The bigger this "escessWalk" the bigger the current frame should be
        /// </summary>
        protected void SetStateToInnerTurn(float excessWalk)
        {
            this.State = MovingState.InnerTurning;
            this.Sprite = this.InnerTurnSprite;
			this.CurrentFrame = (int)(excessWalk * this.Sprite.FrameCount / this.Sprite.BoundingBox.Width);
            if (this.CurrentFrame >= this.Sprite.FrameCount) // Just to make sure...
            {
                this.CurrentFrame = this.Sprite.FrameCount - 1;
            } 
            this.Position = (this.Direction == WalkDirection.Clockwise ? this.PathNode.Value.P0 : this.PathNode.Value.P1);
            this.UpdateBoundingBox();
        }

        /// <summary>
        /// The previous comment applies here too. The diference is that this method sets the outer turn animation
        /// </summary>
        protected void SetStateToOuterTurn(float excessWalk)
        {
            this.State = MovingState.OuterTurning;
            this.Sprite = this.OuterTurnSprite;
			this.CurrentFrame = (int)(excessWalk * this.Sprite.FrameCount / this.Sprite.BoundingBox.Width);
            if (this.CurrentFrame >= this.Sprite.FrameCount) // Just to make sure...
            {
                this.CurrentFrame = this.Sprite.FrameCount - 1;
            }
            this.Position = (this.Direction == WalkDirection.Clockwise ? this.PathNode.Value.P0 : this.PathNode.Value.P1);
            this.UpdateBoundingBox();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetStateToAirborne()
        {
            this._fallingHeight = 0f;
            this.RemoveHooverEffect();
            if (this.IsTurning)
            {
                this.SetStateToWalk();
            }
            if (this._gravityEffect.Active == false)
            {
                    this.AddGravityEffect(Vector2.Zero);
            }

            this.DynamicFlags |= StageObjectDynamicFlags.IsFalling;
            this.DettachFromPath();
          //  this.Rotation = 0;
            this.UpdateBoundingBox();
            this.State = MovingState.Airbone;
        }
     
        /// <summary>
        /// 
        /// </summary>
        public void SetStateToWalk()
        {
            this.CurrentFrame = 0;
            this.Sprite = this.WalkSprite;
            this.State = MovingState.Walking;
            this._SpritePlaybackMode = AnimtionPlaybackModes.Loop;
            this.DynamicFlags &= ~StageObjectDynamicFlags.IsFalling;
            if (this.IsAttachedToPath)
            {
                if (this.PathNode.Value.WallType == PathSegment.SegmentType.Floor)
                {
                    this.AdjustObjectToFloor();
                }
            }
        }

        /// <summary>
        /// Projects the object in a random direction and speed
        /// </summary>
        public void Project(float speed)
        {
            float angle = MathHelper.ToRadians(BrainGame.Rand.Next(360));

            Vector2 v = new Vector2(1.0f, 0f);
            Matrix matRotate = Matrix.CreateRotationZ(angle);

            this.Project(Vector2.Transform(v, matRotate), speed);
        }

        /// <summary>
        /// Projects the object in the specified direction
        /// </summary>
        public virtual void Project(Vector2 direction, float speed)
        {
            direction.Normalize();

            this.AddGravityEffect(direction * speed);
        }

        /// <summary>
        /// Projects the object in a random direction and speed
        /// </summary>
        public void ProjectWithRotation(Vector2 direction, float projectionSpeed, float rotationSpeed)
        {
            this.Project(direction, projectionSpeed);
            this.EffectsBlender.Add(new RotationEffect(rotationSpeed));
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void KillWithGravityEffect()
        {
            this.DynamicFlags = StageObjectDynamicFlags.IsDead | StageObjectDynamicFlags.IsVisible;
            this.StaticFlags = StageObjectStaticFlags.CanFall;
            this.AddGravityEffect(Vector2.Zero);
            this.MoveToForeground();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Kill()
        {
            if (CanDie)
            {
                base.Kill();
                this.CurrentFrame = 0;
                this.Sprite = this.WalkSprite;
                this.State = MovingState.Stopped;
            }
        }

        /// <summary>
        /// Kill with a projection out of the board
        /// </summary>
        public virtual void KillWithProjection()
        {
            this.KillMe();
            this.Rotation = 0;
            // - 45 - 135
            float angle = BrainGame.Rand.Next(90) - 135;
            float angleSin = (float)Math.Sin(MathHelper.ToRadians(angle));
            float angleCos = (float)Math.Cos(MathHelper.ToRadians(angle));
            Vector2 dir = new Vector2(angleCos, angleSin);
            this.Project(dir, 30f);
            this._autoDisposeIfDeadOnAnimationEnd = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void KillByExplosion(Explosion exp)
        {
            this.KillWithProjection();
        }

        /// <summary>
        /// When a snail is hit by a crate a simple project effec is added (similar to the explosion)
        /// </summary>
        public virtual void KillByCrate()
        {
            if (this._projectWhenKilledByCrate)
            {
                this.KillWithProjection();
            }
            else
            {
                this.DettachFromPath();
            }
        }

        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
           
            if (this._bubblesMovementAnim.Visible)
            {
                if (this._inLiquidRef.GetDepth(this.Position) > this._bubblesMovementAnim.Height)
                {
                    this._bubblesMovementAnim.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
                }
            }
#if DEBUG
            if (SnailsGame.GameSettings.ShowPaths)
            {
                if (this.IsAttachedToPath)
                {
                    Vector2 cam = Stage.CurrentStage.Camera.Position;

                    BrainGame.DrawLine(this.PathNode.Value.P0 - cam, this.PathNode.Value.P1 - cam, Color.Blue);
                }
            }
#endif
        }

		/// <summary>
		/// 
		/// </summary>
		public void AddHooverEffect()
		{
#if DEBUG && DEBUG_ASSERTIONS
			// Assert that the effect is not already in the object. Doubles adds should indicate a bug of some type
			if (this.EffectsBlender.Contains(StageObject.TRANSF_EFFECT_HOOVER))
			{
				throw new AssertionException("Object already contains an hoover effect");
			}
#endif
			//this.EffectsBlender.Add(new HooverEffect(StageObject.OBJ_HOOVER_SPEED, StageObject.OBJ_HOOVER_POWER, 0.0f), StageObject.TRANSF_EFFECT_HOOVER);


            // Hoover effect
            if (this._hooverEffect == null)
            {
                this._hooverEffect = new HooverEffect(StageObject.OBJ_HOOVER_SPEED, StageObject.OBJ_HOOVER_POWER, 0.0f);
                this.EffectsBlender.Add(this._hooverEffect, TRANSF_EFFECT_HOOVER);
            }
            this._hooverEffect.Active = true;
            this._hooverEffect.Reset();
            // Subtract to position in the Y axix because the hoover makes the object go thru the floor
			this.Position = new Vector2(this.Position.X, this.Position.Y - 5);
		}

		/// <summary>
		/// 
		/// </summary>
		public void RemoveHooverEffect()
		{
            if (this._hooverEffect != null)
            {
                this._hooverEffect.Active = false;
                if (this.IsAttachedToPath && this.PathNode.Value.WallType == PathSegment.SegmentType.Floor)
                {
                    this.AdjustObjectToFloor();
                    this.UpdateBoundingBox();
                }
            }
		}

        /// <summary>
        /// Places the object in a way that it's tail sits on the end of a path
        /// </summary>
        protected void SetTailInEndOfPath()
        {
            if (this.IsAttachedToPath == false)
            {
                return;
            }

            if (this.Direction == WalkDirection.CounterClockwise)
            {
                switch (this.PathNode.Value.WallType)
                {
                    case PathSegment.SegmentType.Floor:
                        this.SetObjectLRCornerToPoint(this.PathNode.Value.P1);
                        break;
                    case PathSegment.SegmentType.RightWall:
                        this.SetObjectURCornerToPoint(this.PathNode.Value.P1);
                        break;
                    case PathSegment.SegmentType.LeftWall:
                        this.SetObjectLLCornerToPoint(this.PathNode.Value.P1);
                        break;
                    case PathSegment.SegmentType.Ceiling:
                        this.SetObjectULCornerToPoint(this.PathNode.Value.P1);
                        break;
                }
            }
            else
            {
                switch (this.PathNode.Value.WallType)
                {
                    case PathSegment.SegmentType.Floor:
                        this.SetObjectLLCornerToPoint(this.PathNode.Value.P0);
                        break;
                    case PathSegment.SegmentType.RightWall:
                        this.SetObjectLRCornerToPoint(this.PathNode.Value.P0);
                        break;
                    case PathSegment.SegmentType.LeftWall:
                        this.SetObjectULCornerToPoint(this.PathNode.Value.P0);
                        break;
                    case PathSegment.SegmentType.Ceiling:
                        this.SetObjectURCornerToPoint(this.PathNode.Value.P0);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetObjectLLCornerToPoint(Vector2 point)
        {
            this.Position = new Vector2(this.Position.X + (point.X - this.AABoundingBox.Left), 
                                        this.Position.Y + (point.Y - this.AABoundingBox.Bottom));
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetObjectLRCornerToPoint(Vector2 point)
        {
            this.Position = new Vector2(this.Position.X - (this.AABoundingBox.Right - point.X), 
                                        this.Position.Y + (point.Y - this.AABoundingBox.Bottom));
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetObjectULCornerToPoint(Vector2 point)
        {
            this.Position = new Vector2(this.Position.X + (point.X - this.AABoundingBox.Left),
                                        this.Position.Y - (this.AABoundingBox.Top - point.Y));
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetObjectURCornerToPoint(Vector2 point)
        {
            this.Position = new Vector2(this.Position.X - (this.AABoundingBox.Right - point.X),
                                        this.Position.Y - (this.AABoundingBox.Top - point.Y));
        }

        /// <summary>
        /// This method places the object bounding box in touch with the path it is sitting on
        /// </summary>
        public void AdjustObjectToPath(float sideWallsMargin)
        {
            if (this.IsAttachedToPath == false)
            {
                return;
            }

            this.UpdateBoundingBox();
            this.AdjustObjectToPath(this.PathNode.Value, sideWallsMargin ,0f, 0f, 0f);
        }

        /// <summary>
        /// If the object is on a path but part of it is outside (this happens if the object is on a turn )
        /// this method adjusts the object by placing it fully inside the path
        /// </summary>
        public void PlaceObjectInsidePath()
        {
            if (this.IsAttachedToPath == false)
            {
                return;
            }

            PathSegment path = this.PathNode.Value;
            // Flags that indicate of we have a turn next and previous the current path
            bool prevIsTurn = (this.PathNode.Previous.Value.Rotation!= path.Rotation);
            bool nextIsTurn = (this.PathNode.Next.Value.Rotation != path.Rotation);
            
            if (!prevIsTurn && !nextIsTurn)
            {
                return; // The object doesn not lie on a turn for sure, just exit
            }

            switch (path.WallType)
            {
                case PathSegment.SegmentType.LeftWall:
                    if (this.AABoundingBox.UpperLeft.Y < path.P0.Y && prevIsTurn)
                    {
                        this.SetObjectULCornerToPoint(path.P0);
                    }
                    if (this.AABoundingBox.LowerLeft.Y > path.P1.Y && nextIsTurn)
                    {
                        this.SetObjectLLCornerToPoint(path.P1);
                    }
                    break;
                case PathSegment.SegmentType.RightWall:
                    if (this.AABoundingBox.UpperRight.Y < path.P1.Y && nextIsTurn)
                    {
                        this.SetObjectURCornerToPoint(path.P1);
                    }
                    if (this.AABoundingBox.LowerRight.Y > path.P0.Y && prevIsTurn)
                    {
                        this.SetObjectLRCornerToPoint(path.P0);
                    }
                    break;
                case PathSegment.SegmentType.Floor:
                    if (this.AABoundingBox.LowerLeft.X < path.P0.X && prevIsTurn)
                    {
                        this.SetObjectLLCornerToPoint(path.P0);
                    }
                    if (this.AABoundingBox.LowerRight.X > path.P1.X && nextIsTurn)
                    {
                        this.SetObjectLRCornerToPoint(path.P1);
                    }
                    break;
                case PathSegment.SegmentType.Ceiling:
                    if (this.AABoundingBox.UpperLeft.X < path.P1.X && nextIsTurn)
                    {
                        this.SetObjectULCornerToPoint(path.P1);
                    }
                    if (this.AABoundingBox.UpperRight.X > path.P0.X && prevIsTurn)
                    {
                        this.SetObjectURCornerToPoint(path.P0);
                    }
                    break;
            }
        }


        /// <summary>
        /// Places the object next to the path. A margin can be specified
        /// </summary>
        public void AdjustObjectToPath(PathSegment segment, float sideWallsMargin, float topWallsMargin, float ceilingMargin, float floorMargin)
        {
            switch (segment.WallType)
            {
                case PathSegment.SegmentType.RightWall:
                    this.Position = new Vector2(this.Position.X + (segment.P0.X - this.AABoundingBox.Right) - sideWallsMargin, this.Position.Y);
                    break;
                case PathSegment.SegmentType.LeftWall:
                    this.Position = new Vector2(this.Position.X - (this.AABoundingBox.Left - segment.P0.X) + sideWallsMargin, this.Position.Y);
                    break;
                case PathSegment.SegmentType.Ceiling:
                    this.Position = new Vector2(this.Position.X, this.Position.Y - (this.AABoundingBox.Top - segment.P0.Y) + topWallsMargin);
                    break;
                case PathSegment.SegmentType.Floor:
                    this.Position = new Vector2(this.Position.X, this.Position.Y + (segment.P0.Y - this.AABoundingBox.Bottom) - floorMargin);
                    break;
            }

        }

        /// <summary>
        /// This method adjusts the object so it sits right in the floor
        /// This method does not check if the object is currently on a floor path, so that has to be done
        /// prior to calling this method
        /// This only works fine if object rotation is 0
        /// </summary>
        protected void AdjustObjectToFloor()
        {
            this.Position = new Vector2(this.Position.X, this.Position.Y + (this.PathNode.Value.P0.Y - this.AABoundingBox.Bottom));
        }

        /// <summary>
        /// Makes the object fall if it is waling on a wall or ceiling
        /// </summary>
        public void MakeFall()
        {
            if (this.IsAttachedToPath == false)
            {
                // Not walking on a path, just return
                return;
            }

            switch(this.PathNode.Value.WallType)
            {
                // Walking on the floor, just return
                case PathSegment.SegmentType.Floor: 
                    return;
                // Any other of wall type make him fall
                case PathSegment.SegmentType.LeftWall:
                    this.Sprite = this.WalkSprite;
                    this.Position = new Vector2(this.Position.X + 50 ,this.Position.Y);
                    this.UpdateBoundingBox();
                    
                    break;

            }

            this.DettachFromPath();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DettachFromPath()
        {
            this.PathNode = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AttachToPath(BoardPathNode node)
        {
            this.PathNode = node;
        }
    }
}
