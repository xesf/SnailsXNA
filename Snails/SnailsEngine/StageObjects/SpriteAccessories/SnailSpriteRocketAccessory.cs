using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.StageObjects.SpriteAccessories
{
    class SnailSpriteRocketAccessory : SnailSpriteAccessory
    {
        SnailSpriteAccessory _helmetAccessory;
        public override bool Visible 
        {
            get { return base.Visible; }
            set 
            {
                base.Visible = value;
                this._helmetAccessory.Visible = value;
            }
        }

        public SnailSpriteRocketAccessory(Snail snail) :
            base(snail)
        {
            this._helmetAccessory = new SnailSpriteAccessory(snail);
        }


        public override void LoadContent()
        {
            base.LoadContent("spriteset/snail-rocket");
            this._helmetAccessory.LoadContent("spriteset/snail-helmet");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Visible == false)
            {
                return;
            }
            base.Draw(spriteBatch);
            this._helmetAccessory.Draw(spriteBatch);
        }

        public override void UpdateActiveSprite()
        {
            base.UpdateActiveSprite();
            this._helmetAccessory.UpdateActiveSprite();
        }

    }
}
