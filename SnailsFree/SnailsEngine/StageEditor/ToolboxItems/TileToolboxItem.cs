using System;
using System.Collections.Generic;
using System.Reflection;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using LevelEditor.Forms;

namespace TwoBrainsGames.Snails.StageEditor.ToolboxItems
{
    public class TileToolboxItem : ToolboxItem, ITileToolboxItem
    {
        public int StageDataGroupId { get; set; }
        public Tile Tile { get; set; }

        public TileToolboxItem()
        {
        }

        public TileToolboxItem(Tile tile)
        {
            this.Tile = tile;
        }

        /// <summary>
        /// 
        /// </summary>
        public new static ITileToolboxItem FromDataFileRecord(DataFileRecord record)
        {
            string objectClass = record.GetFieldValue<string>("tileToolboxItemClass", null);
            TileToolboxItem item = (TileToolboxItem)ToolboxItem.FromDataFileRecord(record);
            item.StageDataGroupId = record.GetFieldValue<int>("stageDataGroupId");

            List<int> groupList = StageEditor.StageData.GetStyleGroupIdList();

            foreach (int id in groupList)
            {
                if (id == item.StageDataGroupId)
                {
                    item.Tile = StageEditor.StageData.GetTileMatchingWalkFlags(id, WalkFlags.All);
                    break;
                }
            }
            if (item.Tile == null)
            {
                throw new ApplicationException("Could not create Tile Toolbox Item. Tile with group id [" + item.StageDataGroupId.ToString() + "] not found in StageData");
            }
           
 
            item.Sprite = item.Tile.Sprite;
            item.SpriteFrameNr = item.Tile.CurrentFrame;

            return item;
        }

    }
}
