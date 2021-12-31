using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.StageObjects.SpriteAccessories
{
    class HelmetAccessory : SnailSpriteAccessory, ISnailSpriteAccessory
    {

        public HelmetAccessory(Snail snail) :
            base(snail)
        {
        }


        public override void LoadContent()
        {
            base.LoadContent("spriteset/snail-helmet");
        }

        public Prop CreateProp()
        {
            return Prop.CreateHelmet(this._snail.Position);
        }
    }
}
