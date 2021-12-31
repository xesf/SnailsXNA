using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class Apple : MovingObject
    {
        #region Constants
        public const string ID = "APPLE";

        private string RES_APPLE = "spriteset/stage-objects";
        private string SPRITE_APPLE_OXIDIZED_ANIM = "AppleOxidizedAnim";
        #endregion

        enum AppleState
        {
            Good,
            Rotting
        }
        #region Members
        private int _bites;
        private bool _singleSnailEat = true;
        private Sprite _SpriteOxidizedAnim;
        private Snail _snail;
        private AppleState _state;
        private Sample _biteSample;
        #endregion
        public bool CanBeEaten { get { return (this._bites > 0) && (this.IsAttachedToPath); } }

        public Apple()
            : base(StageObjectType.Apple)
        {
            this.SpriteAnimationActive = false;
        }

        public Apple(Apple other)
            : base(other)
        {
            Copy(other);
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
            _bites = (other as Apple)._bites;
            _singleSnailEat = (other as Apple)._singleSnailEat;
            _SpriteOxidizedAnim = (other as Apple)._SpriteOxidizedAnim;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            // load auxiliar sprite animations
            this._SpriteOxidizedAnim = BrainGame.ResourceManager.GetSpriteTemporary(RES_APPLE, SPRITE_APPLE_OXIDIZED_ANIM);
            this._biteSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.APPLE_BITE, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Auto generate number of bites available according with eating frames
            // this isn't using percent of eating to avoid jumping frames
            this._bites = this.Sprite.FrameCount - 1;

            this._state = AppleState.Good;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            base.OnLastFrame();

            if (this._state == AppleState.Rotting)
            {
                this.SpriteAnimationActive = false;
                this.CurrentFrame = this.Sprite.FrameCount - 1;
                this.FadeOut(1f); // The fade will dispose the apple
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            Snail snail = obj as Snail;
            if (snail != null && snail.CanEatApple)
            {
                // Snail collided, but the BB used for the test is the Snail global BB
                // Now we have to make a second test, and test if the snail head collided with the apple
                // We have to transform the head BB because secondary BBoxes are not automatically transformed

                if (_singleSnailEat && _snail != null && _snail != snail)
                    return; // don't check collision, another snails is already eating

                if (snail.CheckCollisionWithHead(this.AABoundingBox))
                {
                    if (_singleSnailEat && _snail == null)
                        _snail = snail; // save eating snail
                    this.RemoveHooverEffect();
                    snail.SetEatingApple(this);

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SnailBite()
        {
            if (_biteSample != null && !_biteSample.IsPlaying)
            {
                _biteSample.Play();
            }

            _bites--; // decrement number of available bites
            this.CurrentFrame++;

            // if no more bites available
            if (_bites == 0)
            {
                this.CurrentFrame = 0;
                this.Sprite = this._SpriteOxidizedAnim; // set oxidized sprite animation
                this.SpriteAnimationActive = true; // toggle sprite animation on
                this.StaticFlags &= ~StageObjectStaticFlags.CanHoover;
                this._state = AppleState.Rotting;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SnailStoppedEating()
        {
            this._snail = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            // If snail is dead, remove it so another snail can eat the apple
            if (this._snail != null &&
                this._snail.IsDead)
            {
                this._snail = null;
            }

            if (this.IsDead || this.IsDisposed)
            {
                return;
            }

            if (this._bites > 0 && this.IsAttachedToPath)
            {
                this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            }
        }

        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            //this._bites = record.GetFieldValue<int>("bites", this._bites);
        }

        public override DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = base.ToDataFileRecord();
            //record.AddField("bites", (int)this._bites);
            return record;
        }
        #endregion
    }
}
