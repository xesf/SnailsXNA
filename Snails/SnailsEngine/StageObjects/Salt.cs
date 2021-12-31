using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class Salt : MovingObject
    {
        public enum SaltPosition
        {
            Left,
            Floor,
            Right,
            Ceiling,
            None
        }

        private enum SaltState
        {
            Idle,
            Dissolving
        }

        public const string ID = "SALT";
        public SaltPosition _tilePosition;
        public Sprite _dissolvingSprite;
        private SaltState _state;
        //private Sample _saltDissolvingSound; 

        public Salt()
            : base(StageObjectType.Salt)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._dissolvingSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects", "SaltDissolving");
            //this._saltDissolvingSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SALT_DISSOLVING);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._state = SaltState.Idle;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AfterBoardInitialize()
        {
            base.AfterBoardInitialize();
            BoardPathNode node = Stage.CurrentStage.Board.PathCollidesWithObject(this);
            if (node != null)
            {

                this.PlaceOnPath(this.Position.X, this.Position.Y, node);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            base.OnLastFrame();
            if (this._state == SaltState.Dissolving)
            {
                this.DisposeFromStage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            // Salt should only collide with snails, don't need to check obj type
            Snail snail = obj as Snail;
            if (!snail.CanBeAffectedBySalt) // if is not yet on Path, than don't collide
            {
                return;
            }

            // Ignore tail only collisions
            if (snail.CheckCollisionWithHead(this.AABoundingBox))
            {
                snail.CollidedWithSalt(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this._state == SaltState.Dissolving)
            {
                return;
            }

            this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);

            // if loose path attachment, than avoid all collisions with paths
            if (!this.IsAttachedToPath && this.CanCollide)
            {
                this.StaticFlags &= ~StageObjectStaticFlags.CanCollide;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnEnterLiquid(Liquid liquid)
        {
            base.OnEnterLiquid(liquid);
            this.Sprite = this._dissolvingSprite;
            this._state = SaltState.Dissolving;
            this.MoveToBackground();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnExitLiquid()
        {
            // Just dispose it, we don't want the dissolve animation to be out of the water
            this.DisposeFromStage();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlaceOnPath(float x, float y, BoardPathNode node)
        {
            SaltPosition pathPos = SaltPosition.None;
            switch (node.Value.WallType)
            {
                case PathSegment.SegmentType.Ceiling:
                    pathPos = SaltPosition.Ceiling;
                    break;
                case PathSegment.SegmentType.Floor:
                    pathPos = SaltPosition.Floor;
                    break;
                case PathSegment.SegmentType.LeftWall:
                    pathPos = SaltPosition.Left;
                    break;
                case PathSegment.SegmentType.RightWall:
                    pathPos = SaltPosition.Right;
                    break;
            }
            this.PlaceOnPath(x, y, pathPos, node);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlaceOnPath(float x, float y, Salt.SaltPosition pos, BoardPathNode node)
        {
            this._tilePosition = pos;
            this.AttachToPath(node);
            this.Rotation = node.Value.Rotation;
            this.Position = new Vector2(x, y);
            this.AdjustObjectToPath(0f);
            this.UpdateBoundingBox();
            this.PlaceObjectInsidePath();


            // For touch devices there will be an exception:
            // if the there isn't a path at the left and right of the path where the salt is placed,
            // we will force the salt at the middle of the path (within a threshold)
            if (SnailsGame.GameSettings.UseTouch)
            {
                if (this.PathNode.Next != null &&
                    this.PathNode.Next.Value.WallType != this.PathNode.Value.WallType &&
                    this.PathNode.Previous != null &&
                    this.PathNode.Previous.Value.WallType != this.PathNode.Value.WallType)
                {
                    // Check threshold by determinig the distance from the salt to the center
                    float dist = (this.PathNode.Value.Center - this.Position).Length();
                    if (dist < 10f)
                    {
                        this.Position = this.PathNode.Value.Center;
                        this.AdjustObjectToPath(0f);
                        this.UpdateBoundingBox();
                        this.PlaceObjectInsidePath();
                    }

                }
            }
        }

    }
}
