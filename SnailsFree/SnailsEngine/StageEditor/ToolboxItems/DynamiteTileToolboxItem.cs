using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using LevelEditor.Forms;
using LevelEditor;

namespace TwoBrainsGames.Snails.StageEditor.ToolboxItems
{
    public class DynamiteTileToolboxItem : TileToolboxItem
    {
        public DynamiteTileToolboxItem()
        {
        }

        /// <summary>
        /// Dynamite boxes are tiles with an associated DynamiteBoxTriggered object
        /// When the tile is added, an object of that type has to be added at the same spot
        /// </summary>
        public override void OnBoardPlacement(int col, int row)
        {
            StageObject obj = StageEditor.StageData.GetObject(DynamiteBoxTriggered.ID);
            obj.Position = new Vector2(col * Stage.TILE_WIDTH, row * Stage.TILE_WIDTH);
            obj.UpdateBoundingBox();
            StageEditor.CurrentStageEdited.AddObject(obj);
        }

        /// <summary>
        /// Remove any DynamiteBoxTriggered object when the tile is removed
        /// We don't know 
        /// </summary>
        public override void OnBoardRemove(int col, int row)
        {
            foreach (StageObject obj in StageEditor.CurrentStageEdited.GetObjectsAt(col * Stage.TILE_WIDTH, row * Stage.TILE_WIDTH))
            {
                if (obj is DynamiteBoxTriggered)
                {
                    StageEditor.CurrentStageEdited.RemoveObject(obj);
                }
            }
        }
    }
}
