using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Stages.Hints
{
    public class HintItemObject : HintItem, IHintItem
    {
        public const string ITEM_TYPE_NAME = "StageObject";
        StageObject _obj;
        
        public HintItemObject(StageObject obj)
        {
            this._obj = obj;
            this.ColorEffect = new ColorEffect(this._obj.BlendColor, new Color(90, 90, 90, 255), 0.020f, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public static new HintItemObject CreateFromDataFileRecord(HintManager hintManager, DataFileRecord itemRecord)
        {
            string id = itemRecord.GetFieldValue<string>("id");
            StageObject obj = hintManager.Stage.StageData.GetObjectNoInitialize(id);
            obj.InitFromDataFileRecord(itemRecord);
            obj.PreviousPosition = obj.Position;
            obj.LoadContent();
            obj.Initialize();
            HintItemObject item = new HintItemObject(obj);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            this._obj.HintInitialize();
        }

     
        #region IHintItem Members
      

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            this._obj.BlendColor = this.ColorEffect.Color;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch, bool useColorEffect)
        {
            this._obj.Draw(false);
            this._obj.HintDraw(useColorEffect ? this.ColorEffect.Color : this._obj.BlendColor);
        }

        /// <summary>
        /// 
        /// </summary>
        public object ItemObject
        {
            get { return this._obj; }
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
            this._obj.InitFromDataFileRecord(record);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = this._obj.ToDataFileRecord(context);
            record.Name = HINT_ELEM_NAME;
            record.AddField(ITEM_TYPE_ATTRIB_NAME, ITEM_TYPE_NAME);
            return record;
        }

        #endregion
    }
}
