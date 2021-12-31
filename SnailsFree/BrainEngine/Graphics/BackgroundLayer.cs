using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Graphics
{
    public class BackgroundLayer : IDataFileSerializable
    {
        Vector2 _size;
        Rectangle _drawingRect;
        SamplerState _samplerState;
        public string _contentManagerId;

        #region Properties
        public string ResId;
        public string SpriteId;
        public float OffsetX;
        public float OffsetY;
        public float LayerDepth;
        public Sprite Sprite;
        public Vector2 _position;
        public Vector2 Size
        {
            get { return this._size; }
            set
            {
                this._size = value;
                this._drawingRect = new Rectangle(0, 0, (int)this._size.X, (int)this._size.Y);
            }
        }

        #endregion

        public BackgroundLayer()
        {
            this._contentManagerId = TwoBrainsGames.BrainEngine.Resources.ResourceManager.RES_MANAGER_ID_TEMPORARY;
        }


        public BackgroundLayer(BackgroundLayer other)
        {
            this.Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Copy(BackgroundLayer other)
        {
            this.ResId = other.ResId;
            this.SpriteId = other.SpriteId;
            this.OffsetX = other.OffsetX;
            this.OffsetY = other.OffsetY;
            this.LayerDepth = other.LayerDepth;
            this.Sprite = other.Sprite;
            this._drawingRect = other._drawingRect;
            this._position = other._position;
            this._samplerState = other._samplerState;
            this._size = other._size;
            this._contentManagerId = other._contentManagerId;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadContent()
        {
            this.Sprite = BrainGame.ResourceManager.GetSprite(this.ResId + "/" + this.SpriteId, this._contentManagerId);
            this.Sprite.LayerDepth = this.LayerDepth;
            this.Sprite.HasShadows = false;
            this.Size = new Vector2(this.Sprite.Width, this.Sprite.Height);

            this._samplerState = new SamplerState();
            this._samplerState.AddressU = TextureAddressMode.Wrap;
            this._samplerState.AddressV = TextureAddressMode.Wrap;

        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Update(BrainGameTime gameTime)
        { }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
		    SamplerState stateSave = BrainGame.Graphics.SamplerStates[0]; // Store previous state
			BrainGame.Graphics.SamplerStates[0] = this._samplerState; // Set the background sampler state (to wrap the texture)
            Sprite.Draw(this._position, this._drawingRect, spriteBatch);
		    BrainGame.Graphics.SamplerStates[0] = stateSave; // Restore previous state
        }

        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public virtual void InitFromDataFileRecord(DataFileRecord record)
        {
            this.ResId = record.GetFieldValue<string>("res", this.ResId);
            this.SpriteId = record.GetFieldValue<string>("sprite", this.SpriteId);
            this.OffsetX = record.GetFieldValue<float>("offsetX", this.OffsetX);
            this.OffsetY = record.GetFieldValue<float>("offsetY", this.OffsetY);
            this.LayerDepth = record.GetFieldValue<float>("layerDepth", this.LayerDepth);
            float x = record.GetFieldValue<float>("x", this._position.X);
            float y = record.GetFieldValue<float>("y", this._position.Y);
            this._position = new Vector2(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("Layer");
            record.AddField("res", this.ResId);
            record.AddField("sprite", this.SpriteId);
            record.AddField("offsetX", this.OffsetX);
            record.AddField("offsetY", this.OffsetY);
            record.AddField("layerDepth", this.LayerDepth);
            record.AddField("x", this._position.X);
            record.AddField("y", this._position.Y);

            return record;
        }

        #endregion
    }
}
