using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class EvilSnail : Snail
    {
        #region Const
        public new const string ID = "EVIL_SNAIL";
        #endregion


        public bool CanKillSnail
        {
            get { return (!this.IsEating) && (!this.IsHidding); }
        }

        /// <summary>
        /// 
        /// </summary>
        public EvilSnail()
            : this(StageObjectType.EvilSnail)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected EvilSnail(StageObjectType type)
            : base(type)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
        //    this.BlendColor = Color.Red; // Change this later with a specific snail sprite
            // Check collisions with other snails, and kill them!!
            this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            Snail snail = (Snail) obj;
            if (snail.CanBeKilledByEvilSnail && this.CanKillSnail)
            {
                snail.KillByEvilSnail();
            }
        }
    }
}
