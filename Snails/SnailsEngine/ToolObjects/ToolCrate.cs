using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.ToolObjects
{
    /// <summary>
    /// The base for all box tools
    /// </summary>
    public class ToolCrate : ToolObject
    {
        #region Members
        string _objectId; // Id of the box in the StageData object list
        #endregion

         /// <summary>
        /// 
        /// </summary>
        public ToolCrate(ToolObjectType objType)
            : base(objType)
        {
            switch(objType)
            {
                case ToolObjectType.Box:
                    this._objectId = Box.ID;
                    break;
                case ToolObjectType.Copper:
                    this._objectId = Copper.ID;
                    break;
                case ToolObjectType.DynamiteBox:
                    this._objectId = DynamiteBox.ID;
                    break;
                case ToolObjectType.DynamiteBoxTriggered:
                    this._objectId = DynamiteBoxTriggered.ID;
                    break;
                case ToolObjectType.DirectionalBoxCW:
                    this._objectId = DirectionalBox.ID_CW;
                    break;
                case ToolObjectType.DirectionalBoxCCW:
                    this._objectId = DirectionalBox.ID_CCW;
                    break;
                default:
                    throw new SnailsException("Invalid object type for ToolBox [" + objType.ToString() + "]");
            }   
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(ToolObject other)
        {
            base.Copy(other);
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
        public override void Action(Vector2 position)
        {
            base.Action(position);
            StageObject obj = Stage.CurrentStage.StageData.GetObject(this._objectId);
            obj.Position = position;
            obj.SnapIt();
            obj.UpdateBoundingBox();

            Stage.CurrentStage.AddObjectInRuntime(obj);
            ((Box)obj).BoxDeployed();
        }

        /// <summary>
        /// Boxes are valid if:
        /// -There's no tile at the same spot
        /// -There a tile in top | bottom | left | right (diagonals are not allowed)
        /// </summary>
        public override bool IsValidAtPosition(Vector2 position)
        {
            bool valid = base.IsValidAtPosition(position);
            if (!valid)
            {
                return false;
            }

           

            TileCellCoords coords = Stage.CurrentStage.Board.GetCoordsFromPosition(position);
            int boardX = coords.ColIndex;
            int boardY = coords.RowIndex;

            // Crate ->object test
            // Crates cannot be placed over some objects
            BoundingSquare bs = new BoundingSquare(new Vector2((boardX * Stage.TILE_WIDTH) + 2, (boardY * Stage.TILE_HEIGHT) + 2),
                                                        Stage.TILE_WIDTH - 4, Stage.TILE_HEIGHT - 4);
            foreach (StageObject obj in Stage.CurrentStage.Objects)
            {
                if (obj.CrateToolIsValid(bs) == false)
                {
                    return false;
                }
            }
         
            // Easy test. Tile not empty, just return false
            if (Stage.CurrentStage.Board.Tiles[boardY, boardX] != null)
            {
                return false;
            }

            // Any tile at left?
            if (boardX > 0 && Stage.CurrentStage.Board.Tiles[boardY, boardX - 1] != null)
            {
                return true;
            }

            // Any tile at right?
            if (boardX + 1 < Stage.CurrentStage.Board.Columns && 
                Stage.CurrentStage.Board.Tiles[boardY, boardX + 1] != null)
            {
                return true;
            }


            // Any tile at bottom?
            if (boardY + 1 < Stage.CurrentStage.Board.Rows &&
                Stage.CurrentStage.Board.Tiles[boardY + 1, boardX] != null)
            {
                return true;
            }


            // Any tile at top?
            if (boardY  > 0 &&
                Stage.CurrentStage.Board.Tiles[boardY - 1, boardX] != null)
            {
                return true;
            }

            return false;
        }
    }
}
