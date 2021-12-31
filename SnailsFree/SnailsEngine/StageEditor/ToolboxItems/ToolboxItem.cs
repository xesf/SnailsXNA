using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.Reflection;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using System.Runtime.Remoting;
using System.Drawing;
using System.Resources;
using System.IO;

namespace TwoBrainsGames.Snails.StageEditor.ToolboxItems
{
    public class ToolboxItem : IToolboxItem
    {
        public enum ThumbnailSourceType
        {
            Sprite,
            EmbeddedResource
        }
        public Sprite Sprite { get; protected set; }
        public Image Thumbnail { get; protected set; }
        public ThumbnailSourceType ThumbnailType { get; protected set; }
        public int SpriteFrameNr { get; protected set; }
        public string ThumbnailResource { get; protected set; }
        public bool Visible { get; set; }
        public ToolboxItem()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static ToolboxItem FromDataFileRecord(DataFileRecord record)
        {
            string objectClass = record.GetFieldValue<string>("itemClass", null);
            ObjectHandle objHandle = Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, objectClass);
            ToolboxItem item = (ToolboxItem)objHandle.Unwrap();
            item.ThumbnailType = (ThumbnailSourceType)Enum.Parse(typeof(ThumbnailSourceType), record.GetFieldValue<string>("thumbnailType", ThumbnailSourceType.Sprite.ToString()));
            item.SpriteFrameNr = record.GetFieldValue<int>("spriteFrameNr", 0);
            item.ThumbnailResource = record.GetFieldValue<string>("thumbnailResId"); // Id in the  resx. file
            item.Visible = record.GetFieldValue<bool>("visible", true);
            switch (item.ThumbnailType)
            {
                case ThumbnailSourceType.EmbeddedResource:
                    item.Thumbnail = StageEditor.GetImageFromResource(item.ThumbnailResource);
                    break;

                case ThumbnailSourceType.Sprite:
                    if (!string.IsNullOrEmpty(item.ThumbnailResource))
                    {
                        item.Sprite = BrainGame.ResourceManager.GetSpriteTemporary(item.ThumbnailResource);
                    }
                    break;
            }
            return item;
        }

        /// <summary>
        /// Called by the editor when the tile is placed on the board
        /// </summary>
        public virtual void OnBoardPlacement(int col, int row)
        {
        }

        /// <summary>
        /// Called by the editor when the tile is removed from the board
        /// We don't what was the tile, we just have the position, the tile can be get from the board using row, col
        /// </summary>
        public virtual void OnBoardRemove(int col, int row)
        {
        }

    }
}
