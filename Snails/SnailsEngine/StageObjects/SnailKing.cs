using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.StageObjects.SpriteAccessories;

namespace TwoBrainsGames.Snails.StageObjects
{
    class SnailKing : Snail
    {
        #region Vars
        SnailSpriteAccessory _crownAccessory;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public SnailKing()
            : base(StageObjectType.SnailKing)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            //SnailKing king = (SnailKing)other;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.AddAccessory((this._crownAccessory = new CrownAccessory(this)));
            this._crownAccessory.Visible = true;
        }
    }
}
