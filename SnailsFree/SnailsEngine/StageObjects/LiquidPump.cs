using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// LiquidPump is a switch that can fill or empty liquid pools
    /// </summary>
    class LiquidPump : StageObject, ISwitchable, ISnailsDataFileSerializable
    {
        const float SPEED_FACTOR = 0.03f;
        public enum PumpTypes
        {
            PumpIn,
            PumpOut
        }

        enum PumpState
        {
            Idle,
            Working
        }

        #region Consts
        private const int WHEEL1_POS_BB_IDX = 0;
        private const int WHEEL2_POS_BB_IDX = 1;
        private const float WHEEL_ROTATION_SPEED = 150f;
        #endregion

        #region Vars
        private Sprite _wheel1Sprite;
        private Sprite _wheel2Sprite;
        private SpriteAnimation _chainAnim;
        private Vector2 _wheel1Pos;
        private Vector2 _wheel2Pos;
        private float _wheelRotation;
        private PumpState _state;
        private Sample _engineSound;

        #endregion

        public PumpTypes PumpType { get; set; }
        public float PumpSpeed { get; set; }
        private int Direction
        {
            get
            {
                return (this.PumpType == PumpTypes.PumpIn ? 1 : -1);
            }
        }
        public LiquidPump()
            : base(StageObjectType.LiquidPump)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            this._wheel1Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/liquid-pump", "Wheel1");
            this._wheel2Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/liquid-pump", "Wheel2");
            this._chainAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/liquid-pump", "Chain"));
            this._engineSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.WATER_PUMP_ENGINE, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._wheel1Pos = this.TransformSpriteFrameBB(WHEEL1_POS_BB_IDX).GetCenter();
            this._wheel2Pos = this.TransformSpriteFrameBB(WHEEL2_POS_BB_IDX).GetCenter();
            this._chainAnim.Position = this._wheel1Pos;
            this._wheelRotation = 0f;
            this._state = PumpState.Idle;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
            switch (this._state)
            {
                case PumpState.Working:
                    this._chainAnim.Update(gameTime);
                    this._wheelRotation += this.PumpSpeed * this.Direction * (WHEEL_ROTATION_SPEED * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
                    if (this.PumpType == PumpTypes.PumpIn)
                    {
                        bool allfull = true;
                        foreach (Liquid liquid in this.LinkedObjects)
                        {
                            if (!liquid.IsFull)
                            {
                                liquid.PumpLiquid(this.PumpSpeed * SPEED_FACTOR * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
                                allfull = false;
                            }
                        }
                        if (allfull)
                        {
                            this.StopPump();
                        }
                    }
                    else
                    {
                        bool allEmpty = true;
                        foreach (Liquid liquid in this.LinkedObjects)
                        {
                            if (!liquid.IsEmpty)
                            {
                                liquid.PumpLiquid(-this.PumpSpeed * SPEED_FACTOR * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
                                allEmpty = false;
                            }
                        }
                        if (allEmpty)
                        {
                            this.StopPump();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            this._chainAnim.Draw(Stage.CurrentStage.SpriteBatch);
            this._wheel1Sprite.Draw(this._wheel1Pos, 0, this._wheelRotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
            this._wheel2Sprite.Draw(this._wheel2Pos, 0, this._wheelRotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopPump()
        {
            this._state = PumpState.Idle;
            this._engineSound.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartPump()
        {
            this._state = PumpState.Working;
            this._engineSound.Play(true);
        }

        /// <summary>
        /// See virtual method for details
        /// </summary>
        public override bool CrateToolIsValid(BoundingSquare crateBs)
        {
            return !(crateBs.Collides(this._crateCollisionBB));
        }

        #region ISwitchable Members

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOn()
        {
            this.StartPump();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOff()
        {
            this.StopPump();
        }

        public bool IsOn
        {
            get { return (this._state == PumpState.Working); }
        }
        #endregion

        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            if (context == ToDataFileRecordContext.StageSave)
            {
                record.AddField("pumpType", this.PumpType.ToString());
                record.AddField("pumpSpeed", this.PumpSpeed);
            }
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.PumpType = (LiquidPump.PumpTypes)Enum.Parse(typeof(LiquidPump.PumpTypes), record.GetFieldValue<string>("pumpType", this.PumpType.ToString()), true);
            this.PumpSpeed = record.GetFieldValue<float>("pumpSpeed", this.PumpSpeed);
        }

        /// <summary>pe
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        #endregion


    }
}
