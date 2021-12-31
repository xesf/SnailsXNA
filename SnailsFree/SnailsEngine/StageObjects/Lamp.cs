using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// A ceiling lamp
    /// This lamp has a spotlight lightsource and has some effects
    /// When it is switched on, if randomly flahes before becoming steadly on
    /// When it is on, it randomly loses strength for a few msecs and the recovers
    /// </summary>
    class Lamp : SingleLightEmitter, ISwitchable
    {
        const float MAX_SHAKE_ANGLE = 10f;
        const float MAX_SHAKE_DISTANCE = 800f;

        enum LampState
        {
            Off,
            SwitchingOn,
            On,
            Flicker
        }

        private LampState _lampState;

        private int _switchingOnFlashingTimes; // The numer of times the light will flash before turning on
        private double _ellapsedFlashingTime; // Controls the time that the light will be off/on while flashing

        private int _flickerCounter;

        private bool _shaking;
        private int _shakeDirection;
        private float _maxShakingAngle;
        //private float _shakeSpeed;
        private float _shakeAngle;
        private bool _shakesWithExplosions;


        public Lamp()
            : base(StageObjectType.Lamp)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            this._lampState = (this.State == LightSource.LightState.On ? LampState.On : LampState.Off);
            this._shakesWithExplosions = ((Lamp)other)._shakesWithExplosions;
        }


        /// <summary>
        ///
        /// </summary>
        public override StageObject Clone()
        {
            StageObject obj;

            obj = StageObjectFactory.Create(this.Type);
            obj.Copy(this);

            return obj;
        }

        /// <summary>
        ///
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.LightSource.SetState(this.State);
        }


        /// <summary>
        ///
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            switch(this._lampState)
            {
               case LampState.SwitchingOn: // Do a flash effect
                   if (this._ellapsedFlashingTime == 0)
                   {
                       this._ellapsedFlashingTime = 30 + BrainGame.Rand.Next(80); // Randomize the time for the next flash
                   }

                   this._ellapsedFlashingTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                   if (this._ellapsedFlashingTime <= 0)
                   {
                      if (this.LightSource.IsOff)
                      {
                         this.LightSource.SwitchOn();
                      }
                      else
                      {
                         this.LightSource.SwitchOff();
                      }

                      // Decrement the number of flashing times
                      this._switchingOnFlashingTimes--;
                      if (this._switchingOnFlashingTimes <= 0) // Flash ended? Just turn on and change state
                      {
                         this.LightSource.SwitchOn();
                         this._lampState = LampState.On;
                      }
                      this._ellapsedFlashingTime = 0;             
                   }
                   break;
                  
               case LampState.On: // Do a flicker effect
                   if (this._ellapsedFlashingTime == 0)
                   {
                       this._ellapsedFlashingTime = 2000 + BrainGame.Rand.Next(3000);
                   }
                   this._ellapsedFlashingTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                   if (this._ellapsedFlashingTime < 0)
                   {
                       this._lampState = LampState.Flicker;
                       this._ellapsedFlashingTime = 0;
                       this._flickerCounter = 8;
                   }
                   break;

                case LampState.Flicker:
                   if (this._ellapsedFlashingTime == 0)
                   {
                       this._ellapsedFlashingTime = 80;
                   }
                   this._ellapsedFlashingTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                   if (this._ellapsedFlashingTime < 0)
                   {
                       this._ellapsedFlashingTime = 0;
                       this._flickerCounter--;
                       if (this.LightSource.Power == LightSource.FullPower)
                       {
                           this.LightSource.Power = (float)((180 + BrainGame.Rand.Next(50)) / 256f);
                       }
                       else
                       {
                           this.LightSource.Power = LightSource.FullPower;
                       }
                       if (this._flickerCounter < 0)
                       {
                           this._lampState = LampState.On;
                           this.LightSource.Power = LightSource.FullPower;
                           this._ellapsedFlashingTime = 0;
                       }
                   }
                   break;
            }

            if (this._shaking)
            {
                // Speed depends on the angle, the bigger the angle the slower
                // This can't be 0, thus the 0.5f
                float speed = (this._maxShakingAngle - Math.Abs(this._shakeAngle * 0.5f));
                float delta = (speed * this._shakeDirection * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100);
                this._shakeAngle += delta;
              
                if (Math.Abs(this._shakeAngle) > this._maxShakingAngle)
                {
                    this._maxShakingAngle *= 0.80f;
                    this._shakeAngle = (this._maxShakingAngle * this._shakeDirection);
                    this._shakeDirection *= -1;
                    if (this._maxShakingAngle < 0.5f)
                    {
                        this._shakeAngle = 0;
                        this._shaking = false;
                    }
                }
                this.Rotation = this.LightSource.Rotation = this._shakeAngle;
            }
        }

        /// <summary>
        /// Objects recieve this notification when an explosion happens
        /// </summary>
        public override void OnExplosion(Explosion explosion)
        {
            if (!this._shakesWithExplosions)
            {
                return;
            }

            float distance = (explosion.Position - this.Position).Length();
            if (distance > MAX_SHAKE_DISTANCE) // Too far don't shake the lamp
            {
                return;
            }
            this.Shake(1f - (distance / MAX_SHAKE_DISTANCE));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shake(float power)
        {
            
            // We're taking into account if the lamp is already shaking
            if (!this._shaking)
            {
                this._shakeAngle = 0;
                // Randomize the direction, 1 or -1
                this._shakeDirection = BrainGame.Rand.Next(2);
                if (this._shakeDirection == 0)
                {
                    this._shakeDirection = -1;
                }
            }
            this._shaking = true;
            if (MAX_SHAKE_ANGLE * power > this._maxShakingAngle)
            {
                this._maxShakingAngle = MAX_SHAKE_ANGLE * power;
            }
            //this._shakeSpeed = this._maxShakingAngle;
        }

        #region ISwitchable
        /// <summary>
        ///
        /// </summary>
        public void SwitchOn()
        {
            this._lampState = LampState.SwitchingOn;
            this.LightSource.Power = LightSource.FullPower;
            this._switchingOnFlashingTimes = 3 + BrainGame.Rand.Next(4);
            this._ellapsedFlashingTime = 0;
            this.LightSource.SwitchOn();
        }

        /// <summary>
        ///
        /// </summary>
        public void SwitchOff()
        {
            this.LightSource.SwitchOff();
            this._lampState = LampState.Off;
        }

        public new bool IsOn
        {
            get { return (this._lampState == LampState.On); }
        }

        #endregion

        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._shakesWithExplosions = record.GetFieldValue<bool>("shakesWithExplosions", this._shakesWithExplosions);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);

            if (context == ToDataFileRecordContext.StageDataSave)
            {
                record.AddField("shakesWithExplosions", this._shakesWithExplosions);
            }

            return record;
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