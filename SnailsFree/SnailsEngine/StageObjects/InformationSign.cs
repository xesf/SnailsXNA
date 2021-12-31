using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    class InformationSign : StageObject, ISnailsDataFileSerializable
    {
        public const string ID = "INFORMATION_SIGN";
        const int BB_IDX_POOL_POSITION = 0;
        const float MAX_ANGLE = 75f; // Don't use 90 because cos(90) is 0, or else the pendulum effect wil never reach 90 degreeds
        const float AMPLITUDE = 0.1f; // This controls the ampplutude of the pendulum 1f to 0f
        const float SPEED = 0.3f;

        public struct SignItem
        {
            public string _id;
            public string _resource;
            public string _sprite;

            public SignItem(string id, string res, string sprite)
            {
                this._id = id;
                this._resource = res;
                this._sprite = sprite;
            }
        }
        public static Dictionary<string, SignItem> SignItems { get; set; }
        public string SignId { get; set; }

        private Sprite _poolSprite;
        private Sprite _signSprite;
        private float _poolRotation;

        private Vector2 _poolPosition;
        private int _direction;
        private float _angle;

        /// <summary>
		/// 
		/// </summary>
        public InformationSign()
            : base(StageObjectType.InformationSign)
        {
        }

        public InformationSign(InformationSign other)
            : base(other)
        {
            Copy(other);

        }


        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            InformationSign otherSign = other as InformationSign;
            this.SignId = otherSign.SignId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            if (InformationSign.SignItems != null && this.SignId != null)
            {
#if DEBUG
                if (!InformationSign.SignItems.ContainsKey(this.SignId))
                {
                    throw new SnailsException("InformationSign with id [" + SignId + "] not found in InformationSigns list.");
                }
#endif
                string signResource = InformationSign.SignItems[this.SignId]._resource;
                string signSprite = InformationSign.SignItems[this.SignId]._sprite;
                this._signSprite = BrainGame.ResourceManager.GetSpriteTemporary(signResource, signSprite);
            }

            this._poolSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/signs/Pool");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._poolPosition = this.TransformSpriteFrameBB(BB_IDX_POOL_POSITION).ToBoundingSquare().Center;
            this._angle = - MAX_ANGLE + BrainGame.Rand.Next((int)MAX_ANGLE * 2);
            this._direction = 1;
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            if (this.SignId == null)
            {
                return base.ToString();
            }
            return (this.SignId);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            float speed = (float)Math.Cos(MathHelper.ToRadians(this._angle)) * this._direction * SPEED;
            this._angle += speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Math.Abs(this._angle) >= MAX_ANGLE)
            {
                this._angle = (MAX_ANGLE * this._direction);
                this._direction *= -1;
            }
            this._poolRotation = this._angle * AMPLITUDE;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            if (!shadow)
            {
                this._poolSprite.Draw(this._poolPosition, 0, this._poolRotation, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
                this._signSprite.Draw(this._poolPosition, 0, this._poolRotation, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
            }
            else
            {
                this._poolSprite.Draw(this._poolPosition + GenericConsts.ShadowDepth, 0, this._poolRotation, SpriteEffects.None, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
                this._signSprite.Draw(this._poolPosition + GenericConsts.ShadowDepth, 0, this._poolRotation, SpriteEffects.None, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
            }
            base.Draw(shadow);
        }

        /// <summary>
        /// 
        /// </summary>
        public static InformationSign Create(StageData stageData, string signId)
        {
            //SignItem signItem = InformationSign.SignItems[signId];
            InformationSign sign = (InformationSign)stageData.GetObjectNoInitialize(InformationSign.ID);
            sign.SignId = signId;
            sign.LoadContent();
            sign.Initialize();

            return sign;
        }

        #region ISnailsDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.SignId = record.GetFieldValue<string>("signId");

            DataFileRecordList stageSignRecords = record.SelectRecords("Signs\\Sign");
            if (stageSignRecords != null && stageSignRecords.Count > 0)
            {
                InformationSign.SignItems = new Dictionary<string, SignItem>();
                foreach (DataFileRecord stageSignRec in stageSignRecords)
                {
                    string id = stageSignRec.GetFieldValue<string>("id");
                    string resource = stageSignRec.GetFieldValue<string>("res");
                    string sprite = stageSignRec.GetFieldValue<string>("sprite");
                    InformationSign.SignItems.Add(id, new SignItem(id, resource, sprite));
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


        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            record.AddField("signId", this.SignId);

            if (context == ToDataFileRecordContext.StageDataSave)
            {
                DataFileRecord signsRecord = record.AddRecord("Signs");
                foreach (KeyValuePair<string, SignItem> sprite in InformationSign.SignItems)
                {
                    DataFileRecord signRecord = new DataFileRecord("Sign");
                    signRecord.AddField("id", sprite.Key);
                    signRecord.AddField("res", sprite.Value._resource);
                    signRecord.AddField("sprite", sprite.Value._sprite);

                    signsRecord.AddRecord(signRecord);
                }
            }
            return record;
        }
        #endregion        
    }
}
