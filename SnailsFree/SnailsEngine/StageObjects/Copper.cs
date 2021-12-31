using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class Copper : Box
    {
        #region Constants
        public new const string ID = "COPPER";
        #endregion

        public Copper()
            : base(StageObjectType.Copper)
        {
        }

        public Copper(Copper other)
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
        public override void OnLastFrame()
        {
            Stage.CurrentStage.Board.SetTileAt(this._tile, this.BoardX, this.BoardY);

			// Check collisions with any objects and destroy them method OnCollide() is called when collisions happen
			this.Quadtree.DoCollisions(this, Stage.QUADTREE_SNAIL_LIST_IDX);
            this.Quadtree.DoCollisions(this, Stage.QUADTREE_STAGEOBJ_LIST_IDX);

            Stage.CurrentStage.RemoveObject(this);
            this.SwitchObjects();
        }

    }
}
