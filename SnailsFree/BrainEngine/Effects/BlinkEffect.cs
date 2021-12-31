using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.BrainEngine.Effects
{
    /// <summary>
    ///  A blink effect
    ///  It may be applied to a UIControl or not
    ///  Pass the UIControl in the contructor to apply to a UIControl
    /// </summary>
    public class BlinkEffect : TransformEffectBase
    {
        private double _ellapsed;
        private double _visibleTime;
        private double _hiddenTime;
        private UIControl _control;
        private Sample _blinkSound;
        public bool Visible { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public BlinkEffect(double timeVisible, double timeHidden):
            this(timeVisible, timeHidden, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public BlinkEffect(double timeVisible, double timeHidden, UIControl control) :
            this(timeVisible, timeHidden, control, null, Color.White)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        public BlinkEffect(double timeVisible, double timeHidden, UIControl control, Sample blinkSound, Color color)
        {
            this._visibleTime = timeVisible;
            this._hiddenTime = timeHidden;
            this._control = control;
            this._blinkSound = blinkSound;
            this.ColorVector = color.ToVector4();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            this._ellapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this._ellapsed > this._visibleTime && this.Visible)
            {
                this._ellapsed = this._ellapsed - this._visibleTime;
                this.Visible = false;
                if (this._control != null)
                {
                    this._control.Hidden = true;
                }
            }
            else
            if (this._ellapsed > this._hiddenTime && !this.Visible)
            {
                this._ellapsed = this._ellapsed - this._hiddenTime;
                this.Visible = !this.Visible;
                if (this._blinkSound != null)
                {
                    this._blinkSound.Play();
                }
                if (this._control != null)
                {
                    this._control.Hidden = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this._ellapsed = 0;
            this.Visible = true;
        }
    }
}
