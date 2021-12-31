using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.StageEditor.ToolboxItems
{
    public class ObjectToolboxItem : ToolboxItem, IObjectToolboxItem
    {
        public StageObject StageObject { get; set; }
        public ThemeType ValidTheme { get; set; }

        public ObjectToolboxItem()
        {
        }

        public ObjectToolboxItem(StageObject obj)
        {
            this.StageObject = obj;
        }


        /// <summary>
        /// 
        /// </summary>
        public new static IObjectToolboxItem FromDataFileRecord(DataFileRecord record)
        {
            string objectClass = record.GetFieldValue<string>("tileToolboxItemClass", null);
            ObjectToolboxItem item = (ObjectToolboxItem)ToolboxItem.FromDataFileRecord(record);
            string type = record.GetFieldValue<string>("stageDataObjId");
            item.StageObject = StageEditor.StageData.GetObject(type);
            if (item.ThumbnailType == ThumbnailSourceType.Sprite &&
                string.IsNullOrEmpty(item.ThumbnailResource))
            {
                item.Sprite = item.StageObject.Sprite;
                item.StageObject.Sprite = item.Sprite;
            }
            item.ValidTheme = (ThemeType)Enum.Parse(typeof(ThemeType), record.GetFieldValue<string>("validTheme", ThemeType.All.ToString()));

            return (IObjectToolboxItem)item;
        }
    }
}
