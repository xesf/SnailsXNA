using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Debugging
{
    public class BrainPerformanceCounter : GameComponent
    {
        [Flags]
        public enum AlertConditions
        {
            None = 0x00,
            Smaller = 0x01,
            Equal = 0x02,
            Greater = 0x04
        }

        double _AlertThreshold;
        double _Counter;
        AlertConditions _AlertMode;

        public bool AlarmOn { get; private set; }
        double AverageAcumulator { get; set; }
        public double Average { get; private set; }
        public bool AverageOn { get; private set; }
        public int AverageCounter { get; private set; }

        public double AlertThreshold 
        {
            get { return this._AlertThreshold; }
            set
            {
                if (this._AlertThreshold != value)
                {
                    this._AlertThreshold = value;
                    this.AlertThresholdChanged();
                }
            }
        }

        public double Counter
        {
            get { return this._Counter; }
            set
            {
                if (this._Counter != value)
                {
                    this._Counter = value;
                    this.CounterThresholdChanged();
                }
            }
        }

        public AlertConditions AlertMode
        {
            get { return this._AlertMode; }
            set
            {
                if (this._AlertMode != value)
                {
                    this._AlertMode = value;
                    this.AlertModeThresholdChanged();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BrainPerformanceCounter(Game game) :
            base(game)
        {
        }



        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (this.AverageOn)
            {
                this.AverageCounter++;
                this.AverageAcumulator += this.Counter;
                this.Average = (this.AverageAcumulator / this.AverageCounter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Reset()
        {
            this.Counter = 0.0f;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Set(float val)
        {
            this.Counter = val;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Set(int val)
        {
            this.Counter = val;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetAlarmThreshold(float threshold, AlertConditions modes)
        {
            this.AlertMode = modes;
            this.AlertThreshold = threshold;
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginAverage()
        {
            this.Average = 0.0f;
            this.AverageAcumulator = 0.0f;
            this.AverageCounter = 0;
            this.AverageOn = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndAverage()
        {
            this.AverageOn = false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateAlarmStatus()
        {
            if (this.AlertMode == AlertConditions.None)
                return;

            this.AlarmOn = false;

            if (((this.AlertMode & AlertConditions.Equal) == AlertConditions.Equal) && this.Counter == this.AlertThreshold)
                this.AlarmOn = true;
            if (((this.AlertMode & AlertConditions.Greater) == AlertConditions.Greater) && this.Counter > this.AlertThreshold)
                this.AlarmOn = true;
            if (((this.AlertMode & AlertConditions.Smaller) == AlertConditions.Smaller) && this.Counter < this.AlertThreshold)
                this.AlarmOn = true;

        }

        /// <summary>
        /// 
        /// </summary>
        private void CounterThresholdChanged()
        {
            this.UpdateAlarmStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        private void AlertThresholdChanged()
        {
            this.UpdateAlarmStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        private void AlertModeThresholdChanged()
        {
            this.UpdateAlarmStatus();
        }

    }
}
