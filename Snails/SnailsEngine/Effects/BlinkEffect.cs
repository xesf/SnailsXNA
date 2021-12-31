using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Effects
{
    public class BlinkEffect : TransformEffectBase
    {

        private double _ellapsed;
        private double _currentVisibleTime;
        private double _currentHiddenTime;
        private double _ellapsedDuration;
        private Sample _blinkSound;

        public bool Visible { get; private set; }
        public double Duration { get; set; }
        public double Decay { get; set; }
        public double VisibleTime { get; set; }
        public double HiddenTime { get; set; }
        public float MinBLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BlinkEffect(double timeVisible, double timeHidden) :
            this(timeVisible, timeHidden, null, 0, 0)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        public BlinkEffect(double timeVisible, double timeHidden, Sample blinkSound, double duration, double decay)
        {
            this._blinkSound = blinkSound;
            this.Duration = duration;
            this.Decay = decay;
            this.VisibleTime = this._currentVisibleTime = timeVisible;
            this.HiddenTime = this._currentHiddenTime = timeHidden;
            this.MinBLink = 10f;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            this._ellapsed += (this.UseRealTime? gameTime.ElapsedRealTime.TotalMilliseconds : gameTime.ElapsedGameTime.TotalMilliseconds);

            if (this._ellapsed > this._currentVisibleTime && this.Visible)
            {
                this._ellapsed = this._ellapsed - this._currentVisibleTime;
                this.Visible = false;
            }
            else
            if (this._ellapsed > this._currentHiddenTime && !this.Visible)
            {
                this._ellapsed = this._ellapsed - this._currentHiddenTime;
                this.Visible = !this.Visible;
                if (this._blinkSound != null)
                {
                    this._blinkSound.Play();
                }
            }

            if (this.Duration != 0)
            {
                this._ellapsedDuration += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this._ellapsedDuration > this.Duration)
                {
                    this.Ended = true;
                }
            }

            if (this.Decay != 0)
            {
                this._currentHiddenTime -= this.Decay * (gameTime.ElapsedGameTime.TotalMilliseconds / (double)1000);
                this._currentVisibleTime -= this.Decay * (gameTime.ElapsedGameTime.TotalMilliseconds / (double)1000);
                if (this._currentHiddenTime < this.MinBLink)
                {
                    this._currentHiddenTime = this.MinBLink;
                }
                if (this._currentHiddenTime < this.MinBLink)
                {
                    this._currentVisibleTime = this.MinBLink;
                }

                if (Duration == 0)
                {
                    this.Ended = true;
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
            this._ellapsedDuration = 0;
            this._currentVisibleTime = this.VisibleTime;
            this._currentHiddenTime = this.VisibleTime;
            this.Visible = true;
        }
    }
}
