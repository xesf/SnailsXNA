using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// This class is responsible to collect all player info during a particulary Stage
    /// </summary>
    public class StageStats
    {
        #region Members
        protected string _stageId;
        
        protected TimeSpan _timer = new TimeSpan(0);
        protected int _numSnailsToRelease;
        protected int _numSnailsSafe;
        protected int _numSnailsReleased;
        protected int _numSnailsDisposed;
        protected int _numSnailsToSave;
        protected int _numSnailsActive;
        protected bool _snailKingDelivered;
        protected bool _snailKingDead;

        protected int _numGoldCoins;
        protected int _numSilverCoins;
        protected int _numBronzeCoins;

        // total snails deads
        protected int _numSnailsDeadByFire;
        protected int _numSnailsDeadBySpikes;
        protected int _numSnailsDeadByDynamite;
        protected int _numSnailsDeadByTools;

        // total of tools used
        protected int _numApplesUsed;
        protected int _numVitaminUsed;
        protected int _numBoxedUsed;
        protected int _numCopperUsed;
        protected int _numBoxesUsed;
        protected int _numDynamitesUsed;


        #endregion

        #region Properties
        public string StageId
        {
            get { return _stageId; }
            set { _stageId = value; }
        }

        public TimeSpan Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }


        public TimeSpan TimeTaken { get; set; }
        public int NumGoldCoins
        {
            get { return _numGoldCoins; }
            set { _numGoldCoins = value; }
        }

        public int NumSilverCoins
        {
            get { return _numSilverCoins; }
            set { _numSilverCoins = value; }
        }

        public int NumBronzeCoins
        {
            get { return _numBronzeCoins; }
            set { _numBronzeCoins = value; }
        }

        public int NumSnailsToRelease
        {
            get { return this._numSnailsToRelease; }
            set
            {
                if (this._numSnailsToRelease != value)
                {
                    this._numSnailsToRelease = value;
                    Stage.CurrentStage.SnailsStageStatsChanged();
                }
            }
        }

        public int NumSnailsSafe
        {
            get { return this._numSnailsSafe; }
            set
            {
                if (this._numSnailsSafe != value)
                {
                    this._numSnailsSafe = value;
                    Stage.CurrentStage.SnailsStageStatsChanged();
                }
            }
        }

        public int NumSnailsReleased
        {
            get { return this._numSnailsReleased; }
            set
            {
                if (this._numSnailsReleased != value)
                {
                    this._numSnailsReleased = value;
                    Stage.CurrentStage.SnailsStageStatsChanged();
                }
            }
        }

        public int NumSnailsDisposed
        {
            get { return this._numSnailsDisposed; }
            set
            {
                if (this._numSnailsDisposed != value)
                {
                    this._numSnailsDisposed = value;
                    Stage.CurrentStage.SnailsStageStatsChanged();
                }
            }
        }

        public int NumSnailsDead
        {
            get { return this._numSnailsDisposed - this._numSnailsSafe; }
        }

        public int NumSnailsToSave
        {
            get { return this._numSnailsToSave; }
            set
            {
                if (this._numSnailsToSave != value)
                {
                    this._numSnailsToSave = value;
                    Stage.CurrentStage.SnailsStageStatsChanged();
                }
            }
        }

        public int NumSnailsActive
        {
            get { return this._numSnailsActive; }
            set
            {
                if (this._numSnailsActive != value)
                {
                    this._numSnailsActive = value;
                    Stage.CurrentStage.SnailsStageStatsChanged();
                }
            }
        }

        public bool SnailKingDelivered
        {
            get { return this._snailKingDelivered; }
            set
            {
                if (this._snailKingDelivered != value)
                {
                    this._snailKingDelivered = value;
                }
            }
        }

        public bool SnailKingDead
        {
            get { return this._snailKingDead; }
            set
            {
                if (this._snailKingDead != value)
                {
                    this._snailKingDead = value;
                }
            }
        }
        // Total snails active or to be released
        public int TotalSnails
        {
            get { return this._numSnailsActive + this._numSnailsToRelease; } 
        }

        public int SnailsDeliveredPointsWon { get; set; }
        public int TimePointsWon { get; set; }
        public int CoinPointsWon { get; set; }
        public MedalType MedalWon { get; set; }
        public int TotalScore { get; set; }
        #endregion
    }
}
