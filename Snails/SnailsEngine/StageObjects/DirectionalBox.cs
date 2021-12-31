using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class DirectionalBox : Box
    {
        public const string ID_CW = "DIRECTIONAL_BOX_CW";
        public const string ID_CCW = "DIRECTIONAL_BOX_CCW";

        const float ARROW_ROTATION_SPEED = 20f;
        enum WalkDirection
        {
            Clockwise,
            CounterClockwise
        }

        private WalkDirection _walkDirection;

        public DirectionalBox()
            : base(StageObjectType.DirectionalBox)
        {
        }

        public DirectionalBox(Copper other)
            : base(other)
        {
            Copy(other);
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
            this._walkDirection = (other as DirectionalBox)._walkDirection;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
         /*   Tile tile = Stage.CurrentStage.StageData.GetTile(this._tileId) as Tile;
            Stage.CurrentStage.Board.SetTileAt(tile, this.BoardX, this.BoardY);

            // Check collisions with any objects and destroy them method OnCollide() is called when collisions happen
            this.Quadtree.DoCollisions(this, Stage.QUADTREE_SNAIL_LIST_IDX);
            this.Quadtree.DoCollisions(this, Stage.QUADTREE_STAGEOBJ_LIST_IDX);

            Stage.CurrentStage.RemoveObject(this);
            this.SwitchObjects();*/
            base.BoxDeployed(true, true, true);
            Stage.CurrentStage.RemoveObject(this);
        }

        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            if (context == ToDataFileRecordContext.StageDataSave)
            {
                record.AddField("walkDirection", this._walkDirection.ToString());
            }
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._walkDirection = (WalkDirection)Enum.Parse(typeof(WalkDirection), record.GetFieldValue<string>("walkDirection", this._walkDirection.ToString()), true);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        #endregion
    }
}
