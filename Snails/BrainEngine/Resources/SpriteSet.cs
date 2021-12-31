using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Globalization;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.BrainEngine.Resources
{
    public class SpriteSet : Image, IDataFileSerializable
    {
        #region Member
        private Dictionary<string, Sprite> _sprites;
        string _name;
        string _imageId;
        #endregion

        #region Operators
        public Sprite this[string id]
        {
            get
            {
                Sprite sprite;
                if (_sprites.TryGetValue(id, out sprite) == false)
                {
                    // FIXME: take all ApplicationExceptions from code. 
                    throw new BrainException("Sprite with id '" + id + "' not found in SpriteSet '" + this._name + "'.");
                }
                return sprite;
            }
        }
        #endregion

        public SpriteSet()
        {
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>(this._imageId);
            foreach (string id in _sprites.Keys)
            {
                _sprites[id].Texture = Texture;
            }
        }

        public override bool Release(ContentManager contentManager)
        {
            return base.Release(contentManager);
        }

        public static SpriteSet FromDataFileRecord(DataFileRecord record)
        {
            SpriteSet sprSet = new SpriteSet();
            sprSet.InitFromDataFileRecord(record);
            return sprSet;
        }

        public bool ContainsSprite(string spriteName)
        {
            return this._sprites.ContainsKey(spriteName);
        }

        #region IDataFileSerializable Members
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            this._name = record.GetFieldValue<string>("Name");
            this._imageId = record.GetFieldValue<string>("ImageId");
            _sprites = new Dictionary<string, Sprite>();
            DataFileRecordList spriteRecords = record.SelectRecords("Sprite");
            foreach (DataFileRecord spriteRecord in spriteRecords)
            {
                Sprite sprite = Sprite.FromDataFileRecord(spriteRecord);
                this._sprites.Add(sprite.Id, sprite);
            }
        }

        public override DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("SpriteSet");
            record.AddField("Name", this._name);
            record.AddField("ImageId", this._imageId);
            
            foreach (Sprite sprite in this._sprites.Values)
            {
                record.AddRecord(sprite.ToDataFileRecord());
            }

            return record;
        }

        #endregion
    }
}
