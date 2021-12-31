using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UITimer : UIControl
    {
        #region Event
        public event UIEvent OnTimer;
        #endregion

        #region Vars
        private double EllapsedTime { get; set; }
        public double Time { get; set; }
        public bool Snooze { get; set; }
        public object Parameter { get; set; }
        public bool RaisesOnTimerWhenStarts { get; set; } // Raises the OnTimer event when the time is 0
        #endregion

        public UITimer(UIScreen screenOwner) :
            this(screenOwner, 0, false)
        {
        }

        public UITimer(UIScreen screenOwner, double time, bool snooze) :
            base(screenOwner)
        {
            this.AcceptControllerInput = false;
            this.Time = time;
            this.EllapsedTime = 0;
            this.Snooze = snooze;
            this.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this.Enabled == false)
            {
                return;
            }
            this.EllapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
            if (this.EllapsedTime > this.Time)
            {
                if (this.OnTimer != null)
                {
                    this.OnTimer(this);
                }

                if (this.Snooze)
                {
                    this.EllapsedTime -= this.Time;
                }
                else
                {
                    this.Enabled = false;
		            this.Reset();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            this.EllapsedTime = (this.RaisesOnTimerWhenStarts? this.Time : 0);

        }
    }
}
