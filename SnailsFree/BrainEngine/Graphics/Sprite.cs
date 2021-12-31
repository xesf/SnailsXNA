using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Collision;
using System.IO;

namespace TwoBrainsGames.BrainEngine.Graphics
{
    public class Sprite : IDataFileSerializable
    {
        #region Constants
        public const float SHADOW_LAYER_DEPTH = 0.1f;
        #endregion

        #region Member
        private Texture2D _texture;
        Vector2 _offset;
        private int _width;
        private string _id;
        private int _height;
        private int _fps;
        public int FrameCount { get { return this._frames.Length; } }
        private Frame [] _frames;
        private float _layerDepth;
        private bool _hasShadows;
        private Color _opacity;
		public BoundingSquare BoundingBox { get { return this.BoundingBoxes[0]; } } // Returns the first BB in the array - default behavior
		public BoundingSquare [] BoundingBoxes;
        public BoundingCircle [] _boundingSpheres;


#if FORMS_SUPPORT
        System.Drawing.Image _Image;
#endif
        #endregion

        #region Properties
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public float OffsetX
        {
            get { return _offset.X; }
            set { _offset.X = value; }
        }

        public float OffsetY
        {
            get { return _offset.Y; }
            set { _offset.Y = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int Fps
        {
            get { return _fps; }
            set { _fps = value; }
        }
        public Frame [] Frames
        {
            get { return _frames; }
            private set { _frames = value; }
        }

        
        public bool HasAnimations
        {
            get { return (_frames.Length > 1) && _fps != 0; }
        }

        public float LayerDepth
        {
            get { return _layerDepth; }
            set { _layerDepth = value; }
        }

        public TwoBrainsGames.BrainEngine.Collision.BoundingSquare BoundingSquare
        {
            get
            {
                return new TwoBrainsGames.BrainEngine.Collision.BoundingSquare(this.Offset, this.Width, this.Height);
            }
        }

        public bool HasShadows
        {
            get { return _hasShadows; }
            set { _hasShadows = value; }
        }

        public Color Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }
#if FORMS_SUPPORT
        public bool IsImageNull { get { return (this._Image == null); } } 
        public System.Drawing.Image Image 
        { 
            get
            {
                if (this._Image == null)
                {
                    this.UpdateImage();
                }
                return this._Image;
            }
            set
            {
                this._Image = value;
            }
        }
#endif

		public bool WithMultipleBoundingBoxes { get { return this.BoundingBoxes.Length > 0; } }
        public double TotalTime { get; private set; }
        #endregion

        public Sprite()
        {
            _width = _height = 0;
            _offset.X = _offset.Y = 0;
            _fps = 0;
            _layerDepth = 0.0f;
            _hasShadows = true;
            _opacity = Color.White;
            this._frames = new Frame[0];
        }

        public Sprite(Sprite other)
        {
            this._texture = other._texture;
            this._offset = other._offset;
            this._width = other._width;
            this._height = other._height;
            this._fps = other._fps;
            this._layerDepth = other._layerDepth;
            this._frames = other._frames;
            this._hasShadows = other._hasShadows;
			this.BoundingBoxes = other.BoundingBoxes;
        }

        public override string ToString()
        {
            return (this._id == null? base.ToString() : this._id);
        }

        public void SetFrames(Frame [] frames)
        {
            _frames = frames;
        }

        public Rectangle GetFrameBoundingBox(int frame)
        {
            return new Rectangle(0, 0, _frames[frame].Width, _frames[frame].Height);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// This draw methods should be reviewed
        /// </summary>
        public virtual void Draw(Vector2 position, int frameNr, Color opacity, SpriteBatch spriteBatch)
        {
          /*  Vector2 pos = new Vector2((int)position.X, (int)(int)position.Y); // Half pixels make the texture blur, so remove them
            spriteBatch.Draw(this._texture, pos + this._offset, this._frames[0].Rect,
                  opacity, 0.0f, new Vector2(0.0f, 0.0f), 1.0f, SpriteEffects.None, layerDepth);*/
            this.Draw(position, frameNr, 0f, SpriteEffects.None, _layerDepth, opacity, 1f, spriteBatch);

        }

        public virtual void Draw(Vector2 position, SpriteBatch spriteBatch)
        {
            this.Draw(position, 0, 0.0f, SpriteEffects.None, this.LayerDepth, Color.White, 1f, spriteBatch);
        }

        public virtual void Draw(Vector2 position, int frameNr, SpriteBatch spriteBatch)
        {
          //  Vector2 pos = new Vector2((int)position.X, (int)(int)position.Y); // Half pixels make the texture blur, so remove them
          //  spriteBatch.Draw(_texture, position, _frames[frameNr].Rect, Color.White, 0.0f, _offset, 1.0f, SpriteEffects.None, 1.0f);
            this.Draw(position, frameNr, 0.0f, SpriteEffects.None, this.LayerDepth, Color.White, 1f, spriteBatch);
        }

        public virtual void Draw(Vector2 position, int frame, float rotation, SpriteEffects effect, SpriteBatch spriteBatch)
        {
            Draw(position, frame, rotation, effect, _layerDepth, Color.White, 1f, spriteBatch);
        }

        public virtual void Draw(Vector2 position, int frame, float rotation, SpriteEffects effect, Vector2 origin, SpriteBatch spriteBatch)
        {
            Draw(position, frame, rotation, effect, _layerDepth, Color.White, 1f, spriteBatch);
        }

        public virtual void Draw(Vector2 position, int frame, float rotation, Vector2 origin, float scaleX, float scaleY, Color opacity, SpriteBatch spriteBatch)
        {
          //  Vector2 pos = new Vector2((int)position.X, (int)(int)position.Y); // Half pixels make the texture blur, so remove them
            Rectangle rc = new Rectangle((int)position.X, (int)position.Y,
                                          (int)((float)_frames[frame].Rect.Width * scaleX), 
                                          (int)((float)_frames[frame].Rect.Height * scaleY));

            spriteBatch.Draw(_texture, rc, _frames[frame].Rect, opacity, MathHelper.ToRadians(rotation), _offset, SpriteEffects.None, 1.0f);
        }

        public virtual void Draw(Vector2 position, Rectangle rcSource, SpriteBatch spriteBatch)
        {
          //  Vector2 pos = new Vector2((int)position.X, (int)(int)position.Y); // Half pixels make the texture blur, so remove them
            spriteBatch.Draw(_texture, position, rcSource, Color.White);
        }

        public virtual void Draw(Vector2 position, Rectangle rcDest, float rotation, SpriteBatch spriteBatch)
        {
          //  Vector2 pos = new Vector2((int)position.X, (int)(int)position.Y); // Half pixels make the texture blur, so remove them
            spriteBatch.Draw(_texture, position, rcDest, Color.White, rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 1f);
        }

        public virtual void Draw(Vector2 position, int frame, Rectangle rcDest, Color color, float rotation, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, rcDest, this.Frames[frame].Rect, color, MathHelper.ToRadians(rotation), this.Offset, SpriteEffects.None, 1f);
        }

        public virtual void Draw(Vector2 position, Rectangle rcDest, Color color, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, rcDest, color);
        }

        public virtual void Draw(Rectangle rcDest, int frame, Color color, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, rcDest, _frames[frame].Rect, color);
        }

        /// <summary>
        /// This method was copied from the previous. This method takes a SpriteBatch as a parameter
        /// This method should replace the previous
        /// </summary>
        public virtual void Draw(Vector2 position, int frame, float rotation, SpriteEffects effect, float layerDepth, Color opacity, float scale, SpriteBatch spriteBatch)
        {
#if DEBUG
            if (BrainGame.Settings.ShowSprites)
            {
#endif
                float rotationRad = MathHelper.ToRadians(rotation);
                Vector2 pos = new Vector2((int)position.X, (int)position.Y); // Half pixels make the texture blur, so remove them
               // pos += new Vector2(0.5f, 0.5f);
                if ((effect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
                {
                    Vector2 center = new Vector2(_frames[frame].Rect.Width - this.Offset.X, this.OffsetY);
                    spriteBatch.Draw(_texture, pos, _frames[frame].Rect, opacity, rotationRad, center, scale, effect, layerDepth);
                }
                else
                {
                    spriteBatch.Draw(_texture, pos, _frames[frame].Rect, opacity, rotationRad, _offset, scale, effect, layerDepth);
                }
#if DEBUG
            }
#endif

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw(Rectangle source, Rectangle dest, Color opacity, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, dest, source, opacity);
        }

        /// <summary>
        /// Draws without round to int (usefull when drawing with a camera scaled)
        /// </summary>
        public virtual void DrawNoRound(Vector2 position, int frame, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, position, _frames[frame].Rect, Color.White, 0f, this._offset, 1f, SpriteEffects.None, 1f);

        }

        /// <summary>
        /// This method was copied from the previous. This method takes a SpriteBatch as a parameter
        /// This method should replace the previous
        /// </summary>
        public virtual void Draw(Vector2 position, int frame, float rotation, SpriteEffects effect,  Color opacity, float scale, SpriteBatch spriteBatch)
        {
#if DEBUG
            if (BrainGame.Settings.ShowSprites)
            {
#endif
                float rotationRad = MathHelper.ToRadians(rotation);
                if ((effect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
                {
                    Vector2 center = new Vector2(_frames[frame].Rect.Width - this.Offset.X, this.OffsetY);
                    spriteBatch.Draw(_texture, position, _frames[frame].Rect, opacity, rotationRad, center, scale, effect, 1f);
                }
                else
                {
                    spriteBatch.Draw(_texture, position, _frames[frame].Rect, opacity, rotationRad, _offset, scale, effect, 1f);
                }
#if DEBUG
            }
#endif

        }
        /// <summary>
        /// All purpose draw method
        /// It's basically a encapsulation to SpriteBatch
        /// </summary>
        public void Draw(Vector2 position, Rectangle rect, float rotation, SpriteEffects effect, Vector2 pivot, Color opacity, SpriteBatch spriteBatch)
        {
#if DEBUG
            if (BrainGame.Settings.ShowSprites)
            {
#endif
           //     Vector2 pos = new Vector2((int)position.X, (int)position.Y); // Half pixels make the texture blur, so remove them
                spriteBatch.Draw(_texture, position, rect, opacity, rotation, pivot, 1.0f, effect, 1f);
#if DEBUG
            }
#endif
        }

        public BoundingSquare FlipCollisionBox(SpriteEffects effects)
        {
            if (effects == SpriteEffects.None)
                return this.BoundingBox;

            if ((effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
            {
				Vector2 ul = new Vector2(this.BoundingBox.Width - this.BoundingBox.LowerRight.X, this.BoundingBox.UpperLeft.Y);
				Vector2 lr = new Vector2(ul.X + this.BoundingBox.Width, this.BoundingBox.LowerRight.Y);
                return new TwoBrainsGames.BrainEngine.Collision.BoundingSquare(ul, lr);
            }

			return this.BoundingBox;
        }

        public static Sprite FromDataFileRecord(DataFileRecord record)
        {
            Sprite sprite = new Sprite();
            sprite.InitFromDataFileRecord(record);
            return sprite;
        }
#if FORMS_SUPPORT
        /// <summary>
        /// 
        /// </summary>
        public System.Drawing.Image ToImage()
        {
           
            //Memory stream to store the bitmap data.
            MemoryStream ms = new MemoryStream();
            
            //Save the texture to the stream.
            this.Texture.SaveAsPng(ms, this.Texture.Width, this.Texture.Height);

            //Seek the beginning of the stream.
            ms.Seek(0, SeekOrigin.Begin);

            //Create an image from a stream.
            System.Drawing.Image bmp2 = System.Drawing.Bitmap.FromStream(ms);

            //Close the stream, we no longer need it.
            ms.Close();
            ms = null;
            return bmp2;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateImage()
        {
          this.Image = this.ToImage();
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        public OOBoundingBox TransformBoundingBox(BoundingSquare bs, Vector2 position, float rotation, SpriteEffects spriteEffect)
        {
            if ((spriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
            {
                BoundingSquare spriteBB = bs;
                // Flip the collision box (we cannot do this in the sprite, because the flip depends on the current frame size
                BoundingSquare bb = new BoundingSquare(new Vector2(-spriteBB.LowerRight.X,
                                                    spriteBB.UpperLeft.Y),
                                                    spriteBB.Width, spriteBB.Height);
                return bb.Transform(rotation, position);
            }

            return bs.Transform(rotation, position);
        }


        #region IDataFileSerializable Members

		/// <summary>
		/// 
		/// </summary>
		public DataFileRecord ToDataFileRecord()
		{
			return this.ToDataFileRecord(0);
		}

        public DataFileRecord ToDataFileRecord(int context)
        {
            DataFileRecord record = new DataFileRecord("Sprite");
            record.AddField("Id", this._id);
            record.AddField("Fps", this._fps);
            record.AddField("Width", this._width);
            record.AddField("Height", this._height);
            record.AddField("OffsetX", this._offset.X);
            record.AddField("OffsetY", this._offset.Y);

            DataFileRecord framesRecord = new DataFileRecord("Frames");
            if (_id == "Leaf")
            {
                _id = "Leaf";
            //    if (this._frames[0].Width == 0)
            //        throw new ApplicationException("RERR");
            }
            for (int i = 0; i < this._frames.Length; i++ )
            {
				if (this._frames[i].WithCollisionBox)
				{
					for (int j = 0; j < this._frames[i].BoundingBoxes.Length; j++)
					{
						this._frames[i].BoundingBoxes[j] = this._frames[i].BoundingBoxes[j].Transform(this.Offset);
					}
				}
                framesRecord.AddRecord(this._frames[i].ToDataFileRecord());
            }

            record.AddRecord(framesRecord);

			// Multiple BB
            DataFileRecord collisionRecord = new DataFileRecord("ColisionZones");
			for (int i = 0; i < this.BoundingBoxes.Length; i++)
			{
				collisionRecord.AddRecord(this.BoundingBoxes[i].Transform(this.Offset).ToDataFileRecord());
			}

            if (this._boundingSpheres != null)
            {
                // Bounding spheres
                for (int i = 0; i < this._boundingSpheres.Length; i++)
                {
                    collisionRecord.AddRecord(this._boundingSpheres[i].Transform(this.Offset).ToDataFileRecord());
                }
            }
            record.AddRecord(collisionRecord);
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._id = record.GetFieldValue<string>("Id", _id);
            if (_id == "Leaf")
                _id = "Leaf";
            this._fps = record.GetFieldValue<int>("Fps", _fps);
            this._width = record.GetFieldValue<int>("Width", _width);
            this._height = record.GetFieldValue<int>("Height", _height);
            float x = record.GetFieldValue<float>("OffsetX", OffsetX);
            float y = record.GetFieldValue<float>("OffsetY", OffsetY);
            this._offset = new Vector2(x, y);

            DataFileRecordList frameRecords = record.SelectRecords("Frames\\Frame");
            // if its a single frame sprite
            if (frameRecords.Count == 0)
            {
                this._frames = new Frame[1];
                Frame frame = new Frame();
                frame.Width = this._width;
                frame.Height = this._height;
                frame.X = 0;
                frame.Y = 0;
                frame.PlayTime = 0;
                this._frames[0] = frame;
            }
            else
            {
                this._frames = new Frame[frameRecords.Count];
                this.Fps = record.GetFieldValue<int>("Fps", _fps);
                for (int i = 0; i < frameRecords.Count; i++ )
                {
                    Frame frame = Frame.FromDataFileRecord(frameRecords[i]);
					// We have to offset the bb before conversion to datafile
					if (frame.WithCollisionBox)
					{
						for (int j = 0; j < frame.BoundingBoxes.Length; j++)
						{
							frame.BoundingBoxes[j] = frame.BoundingBoxes[j].Transform(-this.Offset);
						}
					}
                    this._frames[i] = frame;
                    if (this._frames[i].PlayTime != 0)
                    {
                        this.TotalTime += this._frames[i].PlayTime;
                    }
                    else
                    {
                        this.TotalTime += (this.Fps == 0? 0 : (1000 / this.Fps));
                    }
                }
                this.Width = this._frames[0].Width;
                this.Height = this._frames[0].Height;
            }


            // Multiple bounding boxes
			DataFileRecordList boundingRecords = record.SelectRecords("ColisionZones\\BoundingBox");
			if (boundingRecords.Count > 0)
			{
				this.BoundingBoxes = new BoundingSquare[boundingRecords.Count];
				for (int i = 0; i < boundingRecords.Count; i++)
				{
					Vector2 ul = new Vector2(boundingRecords[i].GetFieldValue<int>("Left"),
											 boundingRecords[i].GetFieldValue<int>("Top"));

					this.BoundingBoxes[i] = new BoundingSquare(ul, boundingRecords[i].GetFieldValue<int>("Width"),
																	boundingRecords[i].GetFieldValue<int>("Height")).Transform(-this.Offset);
				}
			}
			else // No BB? Create one by default
			{
				this.BoundingBoxes = new BoundingSquare[1];
				this.BoundingBoxes[0] = new BoundingSquare(-this.Offset, this.Frames[0].Rect.Width, this.Frames[0].Rect.Height);
			}

            // Bounding spheres - This should be reviwed. There should only be one array of bounding zones and
            // not one for boxes and other for spheres...
            DataFileRecordList boundingSpheres = record.SelectRecords("ColisionZones\\BoundingSphere");
            if (boundingSpheres.Count > 0)
            {
                this._boundingSpheres = new BoundingCircle[boundingSpheres.Count];
                for (int i = 0; i < boundingSpheres.Count; i++)
                {
                    this._boundingSpheres[i] = BoundingCircle.FromDataFileRecord(boundingSpheres[i]).Transform(-this.Offset);
                }
            }
        }

        #endregion
    }
}
