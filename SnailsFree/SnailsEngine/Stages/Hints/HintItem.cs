using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Stages.Hints
{
    public class HintItem
    {
        protected const string ITEM_TYPE_ATTRIB_NAME = "hintItemType";
        protected const string HINT_ELEM_NAME = "HintItem";
        protected ColorEffect ColorEffect { get; set; }

        public HintItem()
        {
            this.ColorEffect = new ColorEffect(Color.White, new Color(50, 50, 50, 200), 0.025f, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            this.ColorEffect.UseRealTime = true;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public static IHintItem CreateFromDataFileRecord(HintManager hintManager, DataFileRecord itemRecord)
        {
            string itemTypeName = itemRecord.GetFieldValue<string>(ITEM_TYPE_ATTRIB_NAME);
            if (itemTypeName == HintItemObject.ITEM_TYPE_NAME)
            {
                return HintItemObject.CreateFromDataFileRecord(hintManager, itemRecord);
            }
            if (itemTypeName == HintItemTile.ITEM_TYPE_NAME)
            {
                return HintItemTile.CreateFromDataFileRecord(hintManager, itemRecord);
            }
            throw new SnailsException("Invalid Hint Object type [" + itemTypeName + "].");
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnBeforeShow()
        {
            this.ColorEffect.ResetEx();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update(BrainGameTime gameTime)
        {
            this.ColorEffect.Update(gameTime);
        }
    }
}
