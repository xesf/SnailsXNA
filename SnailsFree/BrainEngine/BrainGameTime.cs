using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine
{
    public class BrainGameTime
    {
        private TimeSpan _elapsedGameTime = TimeSpan.Zero;
        private TimeSpan _totalGameTime = TimeSpan.Zero;
        private TimeSpan _elapsedRealTime = TimeSpan.Zero;
        private TimeSpan _totalRealTime = TimeSpan.Zero;
        private int _multiplier = 1;

        public BrainGameTime()
        { }

        public void Update(GameTime gameTime)
        {
            _elapsedRealTime = gameTime.ElapsedGameTime;
            _totalRealTime = gameTime.TotalGameTime;
            _elapsedGameTime = new TimeSpan(gameTime.ElapsedGameTime.Ticks * _multiplier);
            _totalGameTime = _totalGameTime.Add(_elapsedGameTime);
        }

        public void SetMultiplier(float multiplier)
        {
            this._multiplier = (int)multiplier;
            _elapsedGameTime = new TimeSpan(this._elapsedRealTime.Ticks * this._multiplier);
            // Not taking into account totalGameTime, it's not used anyways...
        }

        public void Reset()
        {
            _multiplier = 1;
        }

        public int Multiplier
        {
            get
            {
                return _multiplier;
            }
            set 
            { 
                _multiplier = value; 
            }
        }

        public TimeSpan ElapsedGameTime
        {
            get
            {
                return _elapsedGameTime;
            }
            set 
            { 
                _elapsedGameTime = value; 
            }
        }

        public TimeSpan TotalGameTime
        {
            get
            {
                return _totalGameTime;
            }
            set
            {
                _totalGameTime = value;
            }
        }

        public TimeSpan ElapsedRealTime
        {
            get
            {
                return _elapsedRealTime;
            }
            set
            {
                _elapsedRealTime = value;
            }
        }

        public TimeSpan TotalRealTime
        {
            get
            {
                return _totalRealTime;
            }
            set
            {
                _totalRealTime = value;
            }
        }
    }
}
