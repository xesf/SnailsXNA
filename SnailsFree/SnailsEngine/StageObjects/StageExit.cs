using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.StageObjects
{
    public partial class StageExit : StageObject
    {
        public const string SPRITE_PUB_NAME = "ExitAnimation";
        const int CRATE_TOOL_VALID_BB_IDX = 1; // Index for the collision box in the sprite for the crate tool valid test

        #region Members
        int _snailsArrived;
        SpriteAnimation _exitAnimation;
        #endregion

        #region Properties
        public SnailCounter SnailCounter { get; set; }
        public Vector2 DoorPosition
        {
            get
            {
                return new Vector2(this.AABoundingBox.LowerLeft.X + (this.AABoundingBox.Width / 2), this.AABoundingBox.Bottom);  
            }
        }
        #endregion

        public StageExit()
            : base(StageObjectType.StageExit)
        {
            _snailsArrived = 0;
        }

        public StageExit(StageEntrance other)
            : base(other)
        {
            Copy(other);
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
            StageExit stageExit = ((StageExit)other);
            this._snailsArrived = stageExit._snailsArrived;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._exitAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + SPRITE_PUB_NAME, this._contentManagerId));
            if (SnailCounter != null)
            {
                SnailCounter.LoadContent();
            }
        }

		/// <summary>
        /// 
        /// </summary>
		public override void OnCollide(IQuadtreeContainable obj, int listIdx)
		{
			Snail snail = obj as Snail;
            if (snail.CanEnterStage == false)
            {
                return;
            }

            if (snail.CheckCollisionWithHead(this.AABoundingBox))
            {
                SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe++;
                if (snail is SnailKing)
                {
                    SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe++;
                }

                this._snailsArrived++;
                if (this.SnailCounter != null)
                {
                    this.SnailCounter.SetCounter(this._snailsArrived);
                }
                snail.OnEnteringStageExit(this);
            }
		}

		/// <summary>
		/// 
		/// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            // Test collisions with snails
			this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            this._exitAnimation.Update(gameTime);
		}

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            this._exitAnimation.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
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
                this.SnailCounter.SetCounter(this._snailsArrived);
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

    }
}
