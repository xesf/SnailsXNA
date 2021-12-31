using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.StageObjects.SpriteAccessories
{
    class RocketAccessory : SnailSpriteAccessory, ISnailSpriteAccessory
    {
        Sprite _spriteWalkAir;
        Sprite _spriteTurnDownAir;
        Sprite _spriteTurnUpAir;

        Sprite _spriteWalkWater;
        Sprite _spriteTurnDownWater;
        Sprite _spriteTurnUpWater;
        public RocketAccessory(Snail snail) :
            base(snail)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent("spriteset/snail-rocket");
            this._spriteWalkAir = this._spriteWalk;
            this._spriteTurnDownAir = this._spriteTurnDown;
            this._spriteTurnUpAir = this._spriteTurnUp;

            this._spriteWalkWater = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snail-rocket", "SnailWalkBubbles");
            this._spriteTurnDownWater = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snail-rocket", "SnailTurnDownBubbles");
            this._spriteTurnUpWater = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snail-rocket", "SnailTurnUpBubbles");
       }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this.Visible == false)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnEnterLiquid()
        {
            this.UpdateActiveSprite();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnExitLiquid()
        {
            this.UpdateActiveSprite();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateActiveSprite()
        {
            if (this._snail.IsUnderLiquid)
            {
                this._spriteWalk = this._spriteWalkWater;
                this._spriteTurnDown = this._spriteTurnDownWater;
                this._spriteTurnUp = this._spriteTurnUpWater; 
            }
            else
            {
                this._spriteWalk = this._spriteWalkAir;
                this._spriteTurnDown = this._spriteTurnDownAir;
                this._spriteTurnUp = this._spriteTurnUpAir;               
            }
            base.UpdateActiveSprite();
        }

        /// <summary>
        /// 
        /// </summary>
        public Prop CreateProp()
        {
            return Prop.CreateRocket(this._snail.Position);
        }

    }
}
