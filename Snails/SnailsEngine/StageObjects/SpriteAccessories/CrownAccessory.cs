using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.StageObjects.SpriteAccessories
{
    class CrownAccessory : SnailSpriteAccessory, ISnailSpriteAccessory
    {

        public CrownAccessory(Snail snail) :
            base(snail)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent("spriteset/snail-king");
        }

        public Prop CreateProp()
        {
            return Prop.CreateCrown(this._snail.Position);
        }

        /// <summary>
        /// Projects the king
        /// </summary>
        public override void Project()
        {
            base.Project();
            this.Project(Prop.CreateKingsCape(this._snail.Position));
        }
    }
}
