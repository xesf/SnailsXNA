using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.Stages
{
    public enum LayerType
    {
        Background,
        Foreground
    }

    public enum LayerSizeMode
    {
        FillBoard, // Layer fills the entire board
        Sprite          // Layer suze matches the sprite (clouds for instance)
    }

    /// <summary>
    /// 
    /// </summary>
    public class SnailsBackgroundLayer : BackgroundLayer, ISnailsDataFileSerializable
    {
        Vector2 _basePosition; // The position where the layer starts. The position changes depending on the camera position
                               // This is used to store te initial position of the layer
        public float _distance; // Layer distance 1.0 same distance as the tiles, > 1.0 far
        Vector2 _moveOffset;   // Position offset for a moving layer. This offset is added this._position
        LayerSizeMode _sizeMode;
        int _frameNr; // Sprite frameNr to use (not used if LayerSizeMode is FillBoardWidth)
        public float _speed; // The horizontal speed if this is a moving layer (clouds for instance)
        public LayerType _layerType;
        private BasicEffect _backgroundEffect;
        private float _scale;

        public string Id { get; set; }
        public BasicEffect BackgroundEffect { get { return this._backgroundEffect; } }
        public Color BlendColor 
        {
            get
            {
                return new Color(this._backgroundEffect.DiffuseColor);
            }
            set
            {
                this._backgroundEffect.DiffuseColor = value.ToVector3();
            }
        }
        
        public SnailsBackgroundLayer()
        { 
            this._contentManagerId = ResourceManagerIds.STAGE_THEME_RESOURCES;
        }

        public SnailsBackgroundLayer(SnailsBackgroundLayer other)
        {
            this.Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(BackgroundLayer other)
        {
            base.Copy(other);
            SnailsBackgroundLayer layer = (SnailsBackgroundLayer)other;
            this.Id = layer.Id;
            this._basePosition = layer._basePosition;
            this._layerType = layer._layerType;
            this._distance = layer._distance;
            this._speed = layer._speed;
            this._frameNr = layer._frameNr;
            this._sizeMode = layer._sizeMode;
            this._scale = layer._scale;
        }

        /// <summary>
        /// 
        /// </summary>
        public SnailsBackgroundLayer Clone()
        {
            SnailsBackgroundLayer layer = new SnailsBackgroundLayer();
            layer.Copy(this);
            return layer;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void  LoadContent()
        {
 	        base.LoadContent();
            switch(this._sizeMode)
            {
                case LayerSizeMode.FillBoard:
                  this.Size = new Vector2(Stage.CurrentStage.Board.Width * (1 + this._distance),
                                          Stage.CurrentStage.Board.Height * (1 + this._distance));
                  break;
                case LayerSizeMode.Sprite:
                  this.Size = new Vector2(this.Sprite.Frames[this._frameNr].Width, this.Sprite.Frames[this._frameNr].Height);
                  break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this._backgroundEffect = new BasicEffect(BrainGame.Graphics);
            this._backgroundEffect.TextureEnabled = true;
            this._backgroundEffect.VertexColorEnabled = true;
            this._backgroundEffect.World = Matrix.Identity;
            this._backgroundEffect.View = Matrix.Identity;
            this._backgroundEffect.Projection = BrainGame.RenderEffect.Projection;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime, bool allowMove)
        {
            // I really don't know how to explain this maths
            // It's complicated because background layers move slower then the camera (this pos / distance)
            // In the Y axis distance is devided by 2 making it twice faster (bigger distance, slower)
            Vector2 origin = Stage.CurrentStage.Camera.Origin;
            // Calculate the layer pos
            // This will make the background coincide with the camera pos at the origin (the centre of the screen)
            // After this, we have to offset the background to the uppler left corner
            Vector2 camPos = new Vector2(Stage.CurrentStage.Camera.Position.X / this._distance, 
                                         Stage.CurrentStage.Camera.Position.Y / this._distance /*/ 2f*/);
            // Offset it to the upper left corner, this is not as easy as subtracting the hud size, because of
            // the lauer speed. For wp we have to use the max zoom out factor because we have to set this upper left corner
            // when the stage it at full zoom out
            camPos += new Vector2(origin.X - (origin.X / this._distance), 
                                  origin.Y - (origin.Y / this._distance /*/  2f */ )) / Stage.CurrentStage.Camera.MaxZoomOut;
            // Another layer offset. This is configured by stage in order to make the leves look diferent
            camPos += (Stage.CurrentStage._backgroundLayersOffset / this._scale);
            this._backgroundEffect.World = Matrix.CreateTranslation(-camPos.X / this._scale, -camPos.Y / this._scale, 0) *
                                                             Matrix.CreateScale(new Vector3(this._scale * Stage.CurrentStage.Camera.Scale.X, this._scale * Stage.CurrentStage.Camera.Scale.Y, 0f)) *
                                                             Matrix.CreateTranslation(Stage.CurrentStage.Camera.Origin.X, Stage.CurrentStage.Camera.Origin.Y, 0);
            this._position = (this._moveOffset + this._basePosition) / this._scale;

            // Moving layers
            if (this._speed != 0f && allowMove)
            {
                this._moveOffset += new Vector2(this._speed * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 100), 0f);

                // Check if the layer is out of the board - only working for layers that move from left->right at this moment
                if (this._position.X > Stage.CurrentStage.Board.Width / this._scale)
                {
                    // Make the layer wrap to the other side
                    this._basePosition.X = -this.Size.X * this._scale;
                    this._moveOffset = Vector2.Zero;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (this._sizeMode)
            {
                case LayerSizeMode.FillBoard:
                    base.Draw(spriteBatch);
                    break;

                case LayerSizeMode.Sprite:
                    Sprite.DrawNoRound(this._position, this._frameNr, spriteBatch);
                    break;
            }
        }

        #region IDataFileSerializable
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.Id = record.GetFieldValue<string>("id", this.Id);
            this._layerType = (LayerType)record.GetFieldValue<int>("type", (int)this._layerType);
            this._distance = record.GetFieldValue<float>("distance", this._distance);
            this._speed = record.GetFieldValue<float>("speed", this._speed);
            this._frameNr = record.GetFieldValue<int>("frame", this._frameNr);
            this._sizeMode = (LayerSizeMode)record.GetFieldValue<int>("sizeMode", (int)this._sizeMode);
            this._scale = record.GetFieldValue<float>("scale", this._scale);
            this._basePosition = this._position; // this._position is inicialized in base.InitFromDataFileRecord()
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = null;
            if (context == ToDataFileRecordContext.StageDataSave)
            {
                record = base.ToDataFileRecord();
                record.AddField("distance", this._distance);
                record.AddField("speed", this._speed);
                record.AddField("frame", this._frameNr);
                record.AddField("sizeMode", (int)this._sizeMode);
                record.AddField("scale", this._scale);
            }
            else
            {
                record = new DataFileRecord("Layer");

            }
            record.AddField("id", this.Id);
            record.AddField("type", (int)this._layerType);
            return record;
        }
        #endregion

        public override string ToString()
        {
            return (this.Id != null? this.Id : "");
        }

    }
}
