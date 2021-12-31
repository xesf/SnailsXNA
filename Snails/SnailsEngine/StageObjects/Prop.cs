using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageObjects
{
    // A prop is a generic object used for everything that has no intelligence
    // Snails accessories are transformed into Props when we want to throw the accessories for instance
    // The main diference from a StageProp, is that a StageProp is just a sprite tused to visually enrich the stage
    // StageProps have no update, but Props do
    public class Prop : MovingObject, ISnailsDataFileSerializable                         
    {
      
        /// <summary>
		/// 
		/// </summary>
        public Prop()
            : base(StageObjectType.Prop)
        {
        }

        public Prop(Prop other)
            : base(other)
        {
            Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Prop CreateProp(string spriteSet, string spriteName, Vector2 position)
        {
            Prop prop = new Prop();
            prop.LoadContent();
            prop.Initialize();
            prop.Sprite = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, spriteName);
            prop.Position = position;
            return prop;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Prop CreateRocket(Vector2 position)
        {
            return Prop.CreateProp("spriteset/snail-props", "Rocket", position);
        }


        /// <summary>
        /// 
        /// </summary>
        public static Prop CreateHelmet(Vector2 position)
        {
            return Prop.CreateProp("spriteset/snail-props", "Helmet", position);
        }


        /// <summary>
        /// 
        /// </summary>
        public static Prop CreateCrown(Vector2 position)
        {
            return Prop.CreateProp("spriteset/snail-props", "Crown", position);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Prop CreateKingsCape(Vector2 position)
        {
            return Prop.CreateProp("spriteset/snail-props", "KingsCape", position);
        }


        /// <summary>
        /// 
        /// </summary>
        public static Prop CreateShell(Vector2 position)
        {
            return Prop.CreateProp("spriteset/snail-props", "Shell", position);
        }
    
    }
}
