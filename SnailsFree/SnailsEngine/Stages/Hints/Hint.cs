using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Stages.Hints
{
    public class Hint : IDataFileSerializable
    {
        public List<IHintItem> HintItems { get; private set; }
        public List<StageObject> StageObjectItems { get; private set; }
        public List<TileCell> TileItems { get; private set; }
        HintManager HintManager { get; set; }

        public Hint(HintManager hintManager)
        {
            this.HintItems = new List<IHintItem>();
            this.StageObjectItems = new List<StageObject>();
            this.TileItems = new List<TileCell>();
            this.HintManager = hintManager;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Hint FromDataFileRecord(HintManager hintManager, DataFileRecord record)
        {
            Hint hint = new Hint(hintManager);
            hint.InitFromDataFileRecord(record);
            return hint;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (IHintItem item in this.HintItems)
            {
                item.Initialize();
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            foreach (IHintItem item in this.HintItems)
            {
                ((HintItem)item).Update(gameTime);
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, bool useColorEffect)
        {
            foreach (IHintItem item in this.HintItems)
            {
                item.Draw(spriteBatch, useColorEffect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddObject(StageObject obj)
        {
            HintItemObject item = new HintItemObject(obj);
            this.StageObjectItems.Add(obj);
            this.HintItems.Add((IHintItem)item);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveObject(StageObject obj)
        {
            this.StageObjectItems.Remove(obj);
            foreach (IHintItem item in this.HintItems)
            {
                if (item.ItemObject == obj)
                {
                    this.HintItems.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddTile(TileCell tile)
        {
            HintItemTile item = new HintItemTile(tile);
            this.RemoveTileAt(tile.BoardX, tile.BoardY);
            this.TileItems.Add(tile);
            this.HintItems.Add((IHintItem)item);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveTileAt(int x, int y)
        {
            foreach (TileCell tile in this.TileItems)
            {
                if (tile.BoardX == x &&
                    tile.BoardY == y)
                {
                    this.TileItems.Remove(tile);
                    foreach (IHintItem item in this.HintItems)
                    {
                        if (item.ItemObject == tile)
                        {
                            this.HintItems.Remove(item);
                            return;
                        }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnBeforeShow()
        {
            foreach (IHintItem item in this.HintItems)
            {
                ((HintItem)item).OnBeforeShow();
            }
        }
        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this.HintItems.Clear();
            DataFileRecordList hinItemRecords = record.SelectRecords("HintItems\\HintItem");
            foreach (DataFileRecord itemRecord in hinItemRecords)
            {
                this.HintItems.Add(HintItem.CreateFromDataFileRecord(this.HintManager, itemRecord));
            }

            this.StageObjectItems.Clear();
            foreach (IHintItem item in this.HintItems)
            {
                if (item.ItemObject is StageObject)
                {
                    this.StageObjectItems.Add((StageObject)item.ItemObject);
                }
                else
                if (item.ItemObject is TileCell)
                {
                    this.TileItems.Add((TileCell)item.ItemObject);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("Hint");
            DataFileRecord itemRecords = record.AddRecord("HintItems");
            foreach (IHintItem item in this.HintItems)
            {
                itemRecords.AddRecord(item.ToDataFileRecord(context));
            }

            return record;
        }

        #endregion
    }
}
