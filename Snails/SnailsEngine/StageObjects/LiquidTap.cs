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
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class LiquidTap : LiquidSwitch, ISnailsDataFileSerializable
    {
        public enum TapOpenDirection
        {
            Clockwise,
            CounterClockwise
        }

        public enum TapSignToShow
        {
            PumpIn,
            PumpOut
        }

        #region Consts
        private const float ROTATION_SPEED = 0.1f;
        private const double ROTATION_TIME = 3000;
        private const int SNAIL_COLLISION_BB_IDX = 0;
        private const int HANDLE_POS_BB_IDX = 1;
        private const int SIGN_POS_BB_IDX = 2;

        private const double TAP_PUMPING_TIME = 1000;
        #endregion

        #region Vars
        int _direction;
        float _tapRotation;
        Sprite _handleSprite;
        Vector2 _handlePosition;
        bool _pumpingActive;
        double _ellapsedTime;

        // Sign
        Sprite _pumpInSignSprite;
        Sprite _pumpOutSignSprite;
        Sprite _signSprite;
        Vector2 _signPosition;
        SpriteEffects _signSpriteEffect;
        Sample _tapSound;

        #endregion

        public TapOpenDirection OpenDirection { get; set; }
        public TapSignToShow SignToShow { get; set; }

        public LiquidTap()
            : base(StageObjectType.LiquidTap)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            this.OpenDirection = ((LiquidTap)other).OpenDirection;
            this.SignToShow = ((LiquidTap)other).SignToShow;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._handleSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/water", "TapHandle");
            this._pumpInSignSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/water", "TapSignPumpIn");
            this._pumpOutSignSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/water", "TapSignPumpOut");
            this._tapSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.WATER_TAP, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._handlePosition = this.TransformSpriteFrameBB(HANDLE_POS_BB_IDX).GetCenter();

            // Sign
            if (this.SignToShow == TapSignToShow.PumpIn)
            {
                this._signSprite = this._pumpInSignSprite;
            }
            else
            {
                this._signSprite = this._pumpOutSignSprite;
            }

            this._signPosition = this.TransformSpriteFrameBB(SIGN_POS_BB_IDX).GetCenter();
            if (this.OpenDirection == TapOpenDirection.CounterClockwise)
            {
                this._signSpriteEffect = SpriteEffects.FlipHorizontally;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this._pumpingActive)
            {
                // What follows is strange, but needed because of timmings
                // Water pumping depends on the time, if the time limit has reached, we have to pump
                // only the time that was remaining
                double timeDelta = gameTime.ElapsedGameTime.TotalMilliseconds;

                if (this._ellapsedTime + timeDelta > TAP_PUMPING_TIME)
                {
                    timeDelta = TAP_PUMPING_TIME - this._ellapsedTime; // Calculate remaining time
                    this._ellapsedTime = 0;
                    this._pumpingActive = false;
                    this._tapSound.Stop();
               }
                else
                {
                    this._ellapsedTime += timeDelta;
                }

                if (timeDelta > 0)
                {
                    // PumpSpeed is measured in % pumped per turn
                    this._tapRotation += (ROTATION_SPEED * (float)timeDelta * this._direction * (this.OpenDirection == TapOpenDirection.CounterClockwise? -1 : 1));

                    foreach (Liquid liquid in this.LinkedObjects)
                    {
                        liquid.PumpLiquid(this.PumpSpeed * 0.03f * (float)timeDelta / 1000f * this._direction);
                    }
                }
            }
            else
            {
                this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            if (!shadow)
            {
                this._signSprite.Draw(this._signPosition, 0, 0f, this._signSpriteEffect, Stage.CurrentStage.SpriteBatch);
            }
            else
            {
                this._signSprite.Draw(this._signPosition + GenericConsts.ShadowDepth, 0, 0f, this._signSpriteEffect, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
            }
            base.Draw(shadow);

            this._handleSprite.Draw(this._handlePosition + new Vector2(3f, 3f), 0, this._tapRotation, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
            this._handleSprite.Draw(this._handlePosition, 0, this._tapRotation, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnSnailCollided(Snail snail)
        {
            // Deal with double collisions
            if (this._pumpingActive)
            {
                return;
            }

            if (snail.CollidedWithLiquidTap(this) == false)
            {
                return;
            }

            this._pumpingActive = true;
            this._ellapsedTime = 0;

            this._direction = (snail.Direction == MovingObject.WalkDirection.Clockwise ? -1 : 1);
            if (this.OpenDirection == TapOpenDirection.CounterClockwise)
            {
                this._direction *= -1;
            }
            
            // Check if there's something to pump in /out
            if (this._direction == 1)
            {
                bool allFilled = true;
                foreach (Liquid liquid in this.LinkedObjects)
                {
                    if (!liquid.IsFull)
                    {
                        allFilled = false;
                        break;
                    }
                }
                if (allFilled)
                {
                    this._pumpingActive = false;
                    return; // Nothing to pump, just exit
                }
            }
            else
            {
                bool allEmpty = true;
                foreach (Liquid liquid in this.LinkedObjects)
                {
                    if (!liquid.IsEmpty)
                    {
                        allEmpty = false;
                        break;
                    }
                }
                if (allEmpty)
                {
                    this._pumpingActive = false;
                    return; // Nothing to pump, just exit
                }
            }
            this._tapSound.Play();
        }

        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            record.AddField("openDirection", this.OpenDirection.ToString());
            record.AddField("signToShow", this.SignToShow.ToString());
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.OpenDirection = (TapOpenDirection)Enum.Parse(typeof(TapOpenDirection),
                                   record.GetFieldValue<string>("openDirection", TapOpenDirection.Clockwise.ToString()), true);
            this.SignToShow = (TapSignToShow)Enum.Parse(typeof(TapSignToShow),
                                  record.GetFieldValue<string>("signToShow", TapSignToShow.PumpIn.ToString()), true);
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
