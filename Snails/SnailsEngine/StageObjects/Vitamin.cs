using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class Vitamin :  MovingObject
    {
        #region Constants
        public const string ID = "VITAMIN";
        private const int DEFAULT_VITAMINIZED_MILIS = 10000;
        #endregion

        #region Members
        private int _elapsedGameTime;
        private Snail _snail = null;
        private Sample _rocketSound;
        private Sample _underWaterRocketSound;

        #endregion

        public Vitamin()
            : base(StageObjectType.Vitamin)
        {
        }

        public Vitamin(Vitamin other)
            : base(other)
        {
            Copy(other);
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._rocketSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.ROCKET, this);
            this._underWaterRocketSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.ROCKET_UNDERWATER, this);
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
            Snail snail = obj as Snail;
            if (snail.CanEatVitamins)
            {
                if (snail.CheckCollisionWithVitamin(this.AABoundingBox))
                {
                    _snail = snail;
                    _snail.SetVitaminized(this);
                    this.PlayRocketSound();
                    this.Hide();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this.IsDead || this.IsDisposed)
            {
                this.StopRocketSound();
                base.Update(gameTime);
                return;
            }

            if (_snail != null)
            {
                if (_snail.IsVitaminized)
                {
                    _elapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (_elapsedGameTime > DEFAULT_VITAMINIZED_MILIS)
                    {
                        _snail.SetUnvitaminized();
                        Stage.CurrentStage.RemoveObject(this);
                        this.StopRocketSound();
                    }
                }
            }
            else
            {
                base.Update(gameTime);
                this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SnailRemoved()
        {
            this.StopRocketSound();
            Stage.CurrentStage.RemoveObject(this);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayRocketSound()
        {
            this.StopRocketSound();
            if (this._snail.IsUnderLiquid)
            {
                this._underWaterRocketSound.Play(true);
            }
            else
            {
                this._rocketSound.Play(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopRocketSound()
        {
            this._underWaterRocketSound.Stop();
            this._rocketSound.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnSnailEnteredLiquid()
        {
            this.PlayRocketSound();
        }


        /// <summary>
        /// 
        /// </summary>
        public void OnSnailExitedLiquid()
        {
            this.PlayRocketSound();
        }
    }
}
