using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using System.Collections.Generic;
using System;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// WaterPipe 
    /// A water pipe, is just a visual element. It's used to connect tabs and pumps to water pools, but does nothing
    /// It only has a specific object, because it holds some drawing logic
    /// 
    /// The pipe is composed of a list of PipeSection that is built in Initialize
    /// the draw method just takes this list and draws the pipes
    /// 
    /// The pipeString is a string representation of the pipe Ex: "L3;U2;L5"
    /// Lx means x pipes left
    /// Rx means x pipes right
    /// Ux means x pipes up
    /// Dx means x pipes down
    /// </summary>
    class LiquidPipe : StageObject, ISnailsDataFileSerializable
    {
        #region Enums
        [Flags]
        public enum PipeLinkType
        {
            None = 0x00,
            Up = 0x01,
            Down = 0x02,
            Left = 0x04,
            Right = 0x08
        }

        #endregion

        #region Structs
        struct PipeSection
        {
            public PipeLinkType _direction;
            public int _quantity;

            public static PipeSection Parse(string str)
            {
                string dir = str.Substring(0, 1).ToUpper();
                int quantity = Convert.ToInt32(str.Substring(1));

                PipeSection pipe = new PipeSection();
                if (dir == "R") pipe._direction = PipeLinkType.Right;
                if (dir == "L") pipe._direction = PipeLinkType.Left;
                if (dir == "U") pipe._direction = PipeLinkType.Up;
                if (dir == "D") pipe._direction = PipeLinkType.Down;
                pipe._quantity = quantity;
                return pipe;
            }

            public static List<PipeSection> ParsePipeString(string str)
            {
                List<PipeSection> sectionList = new List<PipeSection>();
                foreach(string pipe in str.Split(';'))
                {
                    sectionList.Add(PipeSection.Parse(pipe));
                }
                return sectionList;
            }
        }
    
        class PipePart
        {
            public PipeLinkType _links;
            public int _frame;
            public PipeLinkType _pumpAttachment;
            public PipeLinkType _terminator;
            public bool IsPumpAttachment { get { return (this._pumpAttachment != PipeLinkType.None); } }
            public bool IsTerminator { get { return (this._terminator != PipeLinkType.None); } }

        }

        struct PipePartSprite
        {
            public Vector2 _position;            
            public int _frame;
        }
        #endregion

        #region Vars
        public string PipeString { get; set; }
        private List<PipePartSprite> _pipeSprites;
        private List<PipePart> _pipeParts;
        public PipeLinkType Terminator { get; set; }// Where should the terminator pipe point to?
        public PipeLinkType PumpAttachment { get; set; } // Where is the pipe attachment? To the left or right to the first pipe
        #endregion


        public LiquidPipe()
            : base(StageObjectType.LiquidPipe)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            LiquidPipe pipe = (LiquidPipe)other;
            this._pipeParts = pipe._pipeParts;
            this.PumpAttachment = pipe.PumpAttachment;
            this.Terminator = pipe.Terminator;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.BuildPipe();
        }

        /// <summary>
        /// Builds the pipe sprite list that will be used in draw
        /// </summary>
        private void BuildPipe()
        {
            this._pipeSprites = new List<PipePartSprite>();
            if (string.IsNullOrEmpty(this.PipeString))
            {
                return;
            }

            Vector2 position = this.Position;
            List<PipeSection> pipeSections = PipeSection.ParsePipeString(this.PipeString);
            PipeLinkType previousLink = PipeLinkType.None;
            PipeLinkType nextLink;

            for(int i = 0; i < pipeSections.Count; i++)
            {
                for(int j = 0; j < pipeSections[i]._quantity; j++)
                {
                    PipePartSprite pipe = new PipePartSprite();
                    pipe._position = position;
                    
                    nextLink = pipeSections[i]._direction;
                    if (j + 1 == pipeSections[i]._quantity)
                    {
                        if (i + 1 < pipeSections.Count)
                        {
                            nextLink = pipeSections[i + 1]._direction;
                        }
                        else
                        {
                            nextLink = PipeLinkType.None;
                        }
                    }

                    PipePart part = this.GetPipePartByLink(previousLink, nextLink);
                    pipe._frame = part._frame;
                   
                    this._pipeSprites.Add(pipe);
                    switch (nextLink)
                    {
                        case PipeLinkType.Right:
                            position += new Vector2(this.Sprite.Frames[pipe._frame].Rect.Width, 0f);
                            break;
                        case PipeLinkType.Left:
                            position -= new Vector2(this.Sprite.Frames[pipe._frame].Rect.Width, 0f);
                            break;
                        case PipeLinkType.Down:
                            position += new Vector2(0f, this.Sprite.Frames[pipe._frame].Rect.Height);
                            break;
                        case PipeLinkType.Up:
                            position -= new Vector2(0f, this.Sprite.Frames[pipe._frame].Rect.Height);
                            break;
                    }

                    switch (nextLink)
                    {
                        case PipeLinkType.Left:
                            previousLink = PipeLinkType.Right;
                            break;
                        case PipeLinkType.Right:
                            previousLink = PipeLinkType.Left;
                            break;
                        case PipeLinkType.Down:
                            previousLink = PipeLinkType.Up;
                            break;
                        case PipeLinkType.Up:
                            previousLink = PipeLinkType.Down;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private PipePart GetPipePartByLink(PipeLinkType prevLink, PipeLinkType nextLink)
        {
            PipePart partToReturn = null;
            foreach (PipePart part in this._pipeParts)
            {
                if (part._links == (prevLink | nextLink))
                {
                    partToReturn = part;
                    if (partToReturn._pumpAttachment == this.PumpAttachment &&
                        partToReturn.IsPumpAttachment && prevLink == PipeLinkType.None)
                    {
                        break;
                    }
                    if (partToReturn._terminator == this.Terminator &&
                        partToReturn.IsTerminator && nextLink == PipeLinkType.None)
                    {
                        break;
                    }
                }

            }
#if DEBUG
            if (partToReturn == null)
            {
                throw new SnailsException("Could not find a matching PipePart for specified links.");
            }    
#endif
            return partToReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            //base.Draw(shadow);
            foreach (PipePartSprite part in this._pipeSprites)
            {
                if (!shadow)
                {
                    this.Sprite.Draw(part._position, part._frame, this.BlendColor, Stage.CurrentStage.SpriteBatch);
                }
                else
                {
                    this.Sprite.Draw(part._position + GenericConsts.ShadowDepth, part._frame, this.ShadowColor, Stage.CurrentStage.SpriteBatch);
                }
            }
        }
        
        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            record.AddField("terminator", this.Terminator.ToString());
            record.AddField("pumpAttachment", this.PumpAttachment.ToString());
            record.AddField("pipeString", this.PipeString);
            if (context == ToDataFileRecordContext.StageDataSave)
            {
                foreach (PipePart part in this._pipeParts)
                {
                    DataFileRecord partRec = new DataFileRecord("PipePart");
                    partRec.AddField("frame", part._frame);
                    partRec.AddField("links", part._links.ToString());
                    partRec.AddField("pumpAttachment", part._pumpAttachment.ToString());
                    partRec.AddField("terminator", part._terminator.ToString());
                    record.AddRecord(partRec);
                }
            }
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.PipeString = record.GetFieldValue<string>("pipeString", this.PipeString);
            this.Terminator = (PipeLinkType)Enum.Parse(typeof(PipeLinkType), record.GetFieldValue<string>("terminator", PipeLinkType.None.ToString()), true);
            this.PumpAttachment = (PipeLinkType)Enum.Parse(typeof(PipeLinkType), record.GetFieldValue<string>("pumpAttachment", PipeLinkType.None.ToString()), true);

            DataFileRecordList pipePartsRecs = record.SelectRecords("PipePart");
            if (pipePartsRecs.Count > 0)
            {
                this._pipeParts = new List<PipePart>();
                foreach (DataFileRecord partRec in pipePartsRecs)
                {
                    PipePart part = new PipePart();
                    part._frame = partRec.GetFieldValue<int>("frame");
                    part._links = (PipeLinkType)Enum.Parse(typeof(PipeLinkType), partRec.GetFieldValue<string>("links"), true);
                    part._pumpAttachment = (PipeLinkType)Enum.Parse(typeof(PipeLinkType), partRec.GetFieldValue<string>("pumpAttachment", PipeLinkType.None.ToString()), true);
                    part._terminator = (PipeLinkType)Enum.Parse(typeof(PipeLinkType), partRec.GetFieldValue<string>("terminator", PipeLinkType.None.ToString()), true);
                    this._pipeParts.Add(part);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        #endregion

     }
}
