using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    class Fire : StageObject
    {
        static Sample _fire;
        Sample FireSample
        {
            get
            {
                return Fire._fire;
            }
            set
            {
                Fire._fire = value;
            }
        }

        public Fire()
            : base(StageObjectType.Fire)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (this.FireSample == null)
            {
                //this.FireSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.FIRE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._crateCollisionBB = this.GetCurrentFrameRectTransformed();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StageStartupPhaseEnded()
        {
            base.StageStartupPhaseEnded();
//            this.FireSample.Play(true);
        }

        /// <summary>
        ///  
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            StageObject stageObj = obj as StageObject;
            if (stageObj.CanDieWithFire)
            {
                stageObj.KillByFire();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            this.DoQuadtreeCollisions(Stage.QUADTREE_STAGEOBJ_LIST_IDX);
            // Randomize next fram
/*
            if (!Stage.CurrentStage.FireIsPlaying && 
                _fire != null && 
                !_fire.IsPlaying && 
                !_fire.IsDisposed)
            {
                Stage.CurrentStage.FireIsPlaying = true;
                _fire.Play(0.3f, true); // sound a bit lower than the rest
            }*/
        }


        /// <summary>
        /// See virtual method for details
        /// </summary>
        public override bool CrateToolIsValid(BoundingSquare crateBs)
        {
            return !(crateBs.Collides(this._crateCollisionBB));
        }

    }
}
