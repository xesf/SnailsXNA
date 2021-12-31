using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.BrainEngine.Graphics
{
    public struct Frame : IDataFileSerializable
    {
        float _rotation;
        public float _rotationInRads;
        public Vector2 _offset;
        public Vector2 _pivot;
        public Vector2 _offsetHorizFlipped;
        public Vector2 _pivotHorizFlipped;

        public int PlayTime;
        public Rectangle Rect;
        public BoundingSquare[] BoundingBoxes;

        public BoundingSquare BoundingBox
        {
            get { return this.BoundingBoxes[0]; }
        }
		public bool WithCollisionBox;
		public bool WithMultipleBoundingBoxes { get { return this.BoundingBoxes.Length > 0; } }

        public int X
        {
            get { return Rect.X; }
            set { Rect.X = value; }
        }

        public int Y
        {
            get { return Rect.Y; }
            set { Rect.Y = value; }
        }

        public int Width
        {
            get { return Rect.Width; }
            set { Rect.Width = value; }
        }

        public int Height
        {
            get { return Rect.Height; }
            set { Rect.Height = value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                if (this._rotation != value)
                {
                    _rotation = value;
                    _rotationInRads = MathHelper.ToRadians(this._rotation);
                }
            }
        }

        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public static Frame FromDataFileRecord(DataFileRecord record)
        {
            Frame frame = new Frame();
            frame.InitFromDataFileRecord(record);
            return frame;
        }

        #region IDataFileSerializable Members

        public void InitFromDataFileRecord(DataFileRecord record)
        {
            int left = record.GetFieldValue<int>("Left", this.Rect.Left);
            int right = record.GetFieldValue<int>("Top", this.Rect.Top);
            int width = record.GetFieldValue<int>("Width", this.Rect.Width);
            int height = record.GetFieldValue<int>("Height", this.Rect.Height);
            this.Rect = new Rectangle(left, right, width, height);

            this.Rotation = record.GetFieldValue<float>("Rotation", this._rotation);

            this._offset = new Vector2(record.GetFieldValue<float>("OffsetX", this._offset.X),
                                       record.GetFieldValue<float>("OffsetY", this._offset.Y));
            this._pivot = new Vector2(record.GetFieldValue<float>("PivotX", this._pivot.X),
                                       record.GetFieldValue<float>("PivotY", this._pivot.Y));

            // Pre compute this. They are used when drawing flipped sprites
            this._offsetHorizFlipped = new Vector2(this.Rect.Width - this._offset.X, this._offset.Y);
            this._pivotHorizFlipped = new Vector2(this.Rect.Width - this._pivot.X, this._pivot.Y);

            this.PlayTime = record.GetFieldValue<int>("PlayTime", this.PlayTime);
			this.WithCollisionBox = false;

			DataFileRecordList boundingRecords = record.SelectRecords("ColisionZones\\BoundingBox");
			if (boundingRecords.Count > 0)
			{
				this.WithCollisionBox = true;
				// Multiple bounding boxes
				this.BoundingBoxes = new BoundingSquare[boundingRecords.Count];
				for (int i = 0; i < boundingRecords.Count; i++)
				{
					Vector2 ul = new Vector2(boundingRecords[i].GetFieldValue<int>("Left"),
											 boundingRecords[i].GetFieldValue<int>("Top"));

					this.BoundingBoxes[i] = new BoundingSquare(ul, boundingRecords[i].GetFieldValue<int>("Width"),
																	boundingRecords[i].GetFieldValue<int>("Height"));
				}
			}
        }


        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("Frame");

            record.AddField("Left", this.Rect.Left);
            record.AddField("Top", this.Rect.Top);
            record.AddField("Width", this.Rect.Width);
            record.AddField("Height", this.Rect.Height);
            record.AddField("PlayTime", this.PlayTime);

			if (this.WithCollisionBox)
			{
				DataFileRecord bbListRecord = new DataFileRecord("ColisionZones");
				for (int i = 0; i < this.BoundingBoxes.Length; i++)
				{
					bbListRecord.AddRecord(this.BoundingBoxes[i].ToDataFileRecord());
				}
				record.AddRecord(bbListRecord);
			}

            record.AddField("Rotation", this._rotation);
            record.AddField("OffsetX", this._offset.X);
            record.AddField("OffsetY", this._offset.Y);
            record.AddField("PivotX", this._pivot.X);
            record.AddField("PivotY", this._pivot.Y);

            return record;
        }

        #endregion
    }
}
