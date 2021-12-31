using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Effects;
using System;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class LaserCannonBase : MovingObject, ISwitchable, ISnailsDataFileSerializable
    {
        protected enum LaserBeamSourceState
        {
            TurningOn,
            On,
            Off
        }

        #region vars
        protected LaserBeam _laserBeam;
        protected LaserBeamSourceState _state;
        protected double _turningOnTime;
        protected BlinkEffect _blinkEffect;
        protected Color _laserXnaColor;
        protected Sample _laserSound;
        #endregion

        #region Properties
      
        public double BlinkTimeOn { get; set; }
        public double BlinkTimeOff { get; set; }
        public bool WithBlink { get; set; }
        public bool TurnedOn { get; set; }
        public LaserBeam.LaserBeamColor LaserColor { get; set; }

        #endregion

        public LaserCannonBase(StageObjectType cannonType)
            : base(cannonType)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._laserSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.LASER, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._laserXnaColor = LaserBeam.GetXnaColor(this.LaserColor);
            // Used in intermitent lasers            
            this._blinkEffect = new BlinkEffect(this.BlinkTimeOn, this.BlinkTimeOn);
            this._blinkEffect.Active = false;
            this._blinkEffect.AutoDeleteOnEnd = false;
            this.EffectsBlender.Add(this._blinkEffect);

            if (this._laserBeam == null)
            {
                this._laserBeam = LaserBeam.Create(this);
                this._laserBeam.Activate();
            }
            if (this.TurnedOn)
            {
                this.SetTurningOnState(false);
            }
            else
            {
                this._laserBeam.Hide();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void StageStartupPhaseEnded()
        {
            base.StageStartupPhaseEnded();
            if (this._state == LaserBeamSourceState.On)
            {
             //   this._laserSound.Play(true);
            }
        }

        /// <summary>
        /// Counts the number of times the beam has collided with a mirror
        /// This is used to avoid loops in reflections
        /// </summary>
        public int CountBeamMirrorCollisions(LaserBeamMirror mirror)
        {
            int count = 0;
            LaserBeam currentBeam = this._laserBeam;
            while (currentBeam != null && currentBeam.IsVisible)
            {
                if (currentBeam.ReflectingMirror == mirror)
                {
                    count++;
                }
                currentBeam = currentBeam.NextBeam;
            }
            return count;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            switch (this._state)
            {
                case LaserBeamSourceState.On:
                    if (this.WithBlink)
                    {
                        this._laserBeam.Visible = this._blinkEffect.Visible;
                        this._blinkEffect.Update(gameTime);
                    }
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetTurningOnState(bool playSound)
        {
            this._state = LaserBeamSourceState.On;
            this._laserBeam.Visible = true;
        }

        #region ISwitchable Members
        /// <summary>
        /// 
        /// </summary>
        public void SwitchOn()
        {
            if (!this.TurnedOn)
            {
                this.SetTurningOnState(true);
            }
            else
            {
                this.SwitchOff();
            }
        //    this._laserSound.Play(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void SwitchOff()
        {
            this._laserBeam.Visible = false;
            this.TurnedOn = false;
            this._blinkEffect.Active = false;
            this._state = LaserBeamSourceState.Off;

        }

        public bool IsOn
        {
            get { return (this._state != LaserBeamSourceState.Off); }
        }
        #endregion
   
        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);

            record.AddField("turnedOn", this.TurnedOn);
            record.AddField("blinkTimeOn", this.BlinkTimeOn);
            record.AddField("blinkTimeOff", this.BlinkTimeOff);
            record.AddField("withBlink", this.WithBlink);
            record.AddField("colorType", this.LaserColor.ToString());
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.TurnedOn = record.GetFieldValue<bool>("turnedOn", this.TurnedOn);
            this._state = (this.TurnedOn ? LaserBeamSourceState.TurningOn : LaserBeamSourceState.Off);
            this.BlinkTimeOn = record.GetFieldValue<double>("blinkTimeOn", this.BlinkTimeOn);
            this.BlinkTimeOff = record.GetFieldValue<double>("blinkTimeOff", this.BlinkTimeOff);
            this.WithBlink = record.GetFieldValue<bool>("withBlink", this.WithBlink);
            this.LaserColor = (LaserBeam.LaserBeamColor)Enum.Parse(typeof(LaserBeam.LaserBeamColor), record.GetFieldValue<string>("colorType", this.LaserColor.ToString()), true);
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
