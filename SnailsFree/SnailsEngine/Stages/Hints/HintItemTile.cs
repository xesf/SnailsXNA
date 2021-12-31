using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Stages.Hints
{
    public class HintItemTile : HintItem, IHintItem
    {
        public const string ITEM_TYPE_NAME = "Tile";
        TileCell _tile;
        /// <summary>
        /// 
        /// </summary>
        public HintItemTile(TileCell tile)
        {
            this._tile = tile;
            this.ColorEffect = new ColorEffect(this._tile.Tile.BlendColor, new Color(90, 90, 90, 255), 0.020f, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public static new HintItemTile CreateFromDataFileRecord(HintManager hintManager, DataFileRecord itemRecord)
        {
            string id = itemRecord.GetFieldValue<string>("id");
            int boardX = itemRecord.GetFieldValue<int>("boardX");
            int boardY = itemRecord.GetFieldValue<int>("boardY");
            Tile tile = Stage.CurrentStage.StageData.GetTile(id);
            HintItemTile item = new HintItemTile(new TileCell(tile, boardX, boardY));
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._tile.Tile.HintInitialize();
        }

        #region IHintItem Members
        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch, bool useColorEffect)
        {
            this._tile.Tile.Draw(spriteBatch, this._tile.Position, (useColorEffect ? this.ColorEffect.Color : this._tile.Tile.BlendColor));
            this._tile.Tile.HintDraw(this._tile.Position);
        }

        /// <summary>
        /// 
        /// </summary>
        public object ItemObject
        {
            get { return this._tile; }
        }

        public Color BlendColorStageEditor
        {
            get
            {
                return this.ColorEffect.EndColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(BrainEngine.Data.DataFiles.DataFileRecord record)
        {
            string id = record.GetFieldValue<string>("id");
            int boardX = record.GetFieldValue<int>("boardX");
            int boardY = record.GetFieldValue<int>("boardY");
            Tile tile = Stage.CurrentStage.StageData.GetTile(id);

            this._tile = new TileCell(tile, boardX, boardY);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
          //  return this._tile.ToDataFileRecord();
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord tileRecord = this._tile.Tile.ToDataFileRecord(context);
            tileRecord.AddField("boardX", this._tile.BoardX.ToString());
            tileRecord.AddField("boardY", this._tile.BoardY.ToString());
            tileRecord.Name = HINT_ELEM_NAME;
            tileRecord.AddField(ITEM_TYPE_ATTRIB_NAME, ITEM_TYPE_NAME);
            return tileRecord;
        }

        #endregion
    }
}
