using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.UI.Controls;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// This is a box that has a counter. This counter is decremented every time a snail touches it
    /// when the counter reaches 0, there's a delay and then the box explodes (this delay is to avoid to kill the last snail)
    /// If the counter is 0, and another snail touches it, the box explodes immediately
    /// </summary>
    class DynamiteBoxCounted : DynamiteBox
    {
        #region Const
        const int EXPLOSTION_DELAY_TIME = 3500;
        #endregion

        #region Vars
        Color _saveCrateColor;
        #endregion

        #region Properties
        public int SnailsAllowed { get; set; }
        List<Snail> _currentCollidingSnails;
        List<Snail> _collidingSnails;
        BlinkEffect _counterBlinkEffect;
        UITimer _flashTimer;
        #endregion

        public DynamiteBoxCounted() : 
            base(StageObjectType.DynamiteBoxCounted)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            this.SnailsAllowed = ((DynamiteBoxCounted)other).SnailsAllowed;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._counterVisible = true;
            this._currentSecond = this.SnailsAllowed;
            this._currentCollidingSnails = new List<Snail>();
            this._collidingSnails = new List<Snail>();
            this._counterBlinkEffect = new BlinkEffect(500, 100, null, EXPLOSTION_DELAY_TIME, 150f);
            this._counterBlinkEffect.UseRealTime = false;
            this._counterBlinkEffect.MinBLink = 50f;
            this._flashTimer = new UITimer(null, 100, false);
            this._flashTimer.OnTimer += new UIControl.UIEvent(_flashTimer_OnTimer);
            this._saveCrateColor = this.BlendColor;
        }

        /// <summary>
        /// 
        /// </summary>
        void _flashTimer_OnTimer(IUIControl sender)
        {
            this.BlendColor = this._saveCrateColor;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void BoxDeployed(bool addTile, bool addPaths, bool checkCollisions)
        {
            base.BoxDeployed(addTile, addPaths, checkCollisions);
            this._status = DynamiteBoxStatus.Idle;
            this.SetSpriteWhenIdle();
            this.PreComputeCounterData(this.SnailsAllowed);
            this._deployStatus = BoxDeployStatus.Deployed;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this._flashTimer.Enabled)
            {
                this._flashTimer.Update(gameTime);
            }

            this._currentCollidingSnails.Clear();
            if (this.SnailsAllowed > 0)
            {
                this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            }
            for(int i = 0; i < this._collidingSnails.Count; i++)
            {
                if (!this._currentCollidingSnails.Contains(this._collidingSnails[i]))
                {
                    this._collidingSnails.Remove(this._collidingSnails[i]);
                    i--;
                }
            }

            if (this._status == DynamiteBoxStatus.ExplosionDelay)
            {
                this._counterBlinkEffect.Update(gameTime);
                this._counterVisible = this._counterBlinkEffect.Visible;
                this._elapsedTimeToExplode += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this._elapsedTimeToExplode > EXPLOSTION_DELAY_TIME)
                {
                    this.Explode();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void SnailCollided(Snail snail)
        {

            if (!this._currentCollidingSnails.Contains(snail))
            {
                this._currentCollidingSnails.Add(snail);
            }

            if (!this._collidingSnails.Contains(snail))
            {
                this._tickSample.Play();
                this.SnailsAllowed--;
                if (this.SnailsAllowed >= 0)
                {
                    this._currentSecond--;
                    this._collidingSnails.Add(snail);
                    this.PreComputeCounterData(this.SnailsAllowed);

                    if (this._flashTimer.Enabled == false)
                    {
                        this.BlendColor = new Microsoft.Xna.Framework.Color(255, 255, 255, 128);
                        this._flashTimer.Enabled = true;
                    }

                    if (this._currentSecond == 0)
                    {
                        this._status = DynamiteBoxStatus.ExplosionDelay;
                        this._counterBlinkEffect.Reset();
                        this._counterBlinkEffect.Active = true;
                    }
                }
            }
        }

        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.SnailsAllowed = record.GetFieldValue<int>("snailsAllowed", this.SnailsAllowed);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

       
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            if (context == ToDataFileRecordContext.StageSave)
            {
                record.AddField("snailsAllowed", this.SnailsAllowed);
            }
            return record;
        }

        #endregion

    }
}
