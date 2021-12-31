using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public partial class Spikes : StageObject, ISwitchable
    {
        enum SpikesStateType
        { 
            StartingUp,
            Idle,
            Activated,
            Releasing,
            WaitToRelease,
            Off
        }

        #region Constants
        public const string ID = "SPIKES";
        private string RES_SPIKES = "spriteset/stage-objects";
        private string SPRITE_SPIKES_UP = "SpikesUp";
        private string SPRITE_SPIKES_DOWN = "SpikesDown";

        public const int ACTIVATION_TIME = 3000;
        public const int STARTUP_DELAY = 2000;
        public const int RETREAT_TIME = 2000;
        #endregion

        #region Members
        double _elapsedTimeToActivate = 0;
        double _elapsedTimeToRelease = 0;

        Sprite _SpriteSpikesIdle;
        Sprite _SpriteSpikesUp;
        Sprite _SpriteSpikesDown;
        Snail _empaledSnail;
        Sample _spikeSample;
        Sample _spikeHitSnailSound;
        Sample _spikesClosingSound;

        public int ActivationTime { get; set; }
        public int StartupDelay { get; set; }
        SpikesStateType State { get; set; }
        public bool TurnedOn { get; set; }
        #endregion

        public Spikes()
            : base(StageObjectType.Spikes)
        {
            this.ActivationTime = ACTIVATION_TIME;
            this.StartupDelay = STARTUP_DELAY;
            this.TurnedOn = true;
        }

        public Spikes(Spikes other)
            : base(other)
        {
            Copy(other);
        }

        public override void Copy(StageObject other)
        {
            this.ActivationTime = ((Spikes)other).ActivationTime;
            this.StartupDelay = ((Spikes)other).StartupDelay;
            base.Copy(other);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            // Sprites
            _SpriteSpikesIdle = this.Sprite;
            _SpriteSpikesUp = BrainGame.ResourceManager.GetSpriteTemporary(RES_SPIKES, SPRITE_SPIKES_UP);
            _SpriteSpikesDown = BrainGame.ResourceManager.GetSpriteTemporary(RES_SPIKES, SPRITE_SPIKES_DOWN);
            // Sounds
            _spikeSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SPIKES, this);
            _spikeHitSnailSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SPIKES_HIT_SNAIL, this);
            _spikesClosingSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SPIKES_CLOSING, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            BoundingSquare bs = this.Sprite.BoundingBox;
            this._crateCollisionBB = this.TransformBoundingBox(bs).ToBoundingSquare();//.Draw(Color.Red, SnailsGame.Instance.ActiveCamera.Position);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            switch(this.State)
            {
                // A delay before start working. This makes it possible to have spikes with equal times but not sychronized
                case SpikesStateType.StartingUp:
                    if (this.IsOn)
                    {
                        _elapsedTimeToActivate += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (_elapsedTimeToActivate > this.StartupDelay)
                        {
                            this.State = SpikesStateType.Idle;
                            // Force the first time, this way we will not have a second delay
                            // Somar o tempo que sobrou por causa dos timmings
                            this._elapsedTimeToActivate = this.ActivationTime + (_elapsedTimeToActivate - this.StartupDelay);
                        }
                    }
                    break;

                case SpikesStateType.Idle:
                    if (this.IsOn)
                    {
                        _elapsedTimeToActivate += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (_elapsedTimeToActivate > this.ActivationTime)
                        {
                            _elapsedTimeToActivate = 0;
                            this.CurrentFrame = 0;
                            this.Sprite = _SpriteSpikesUp;
                            this.State = SpikesStateType.Activated;
                            this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
                            _elapsedTimeToRelease = _elapsedTimeToActivate - this.ActivationTime; // Temos que somar logo o tempo excedente
                        }
                    }
                    break;

                case SpikesStateType.Activated:
                    if (this.IsOn)
                    {
                        this.State = SpikesStateType.WaitToRelease;
                        if (_spikeSample != null && !_spikeSample.IsPlaying)
                        {
                            _spikeSample.Play();
                        }                     
                        this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
                    }
                    break;

                case SpikesStateType.WaitToRelease:
                    _elapsedTimeToRelease+= gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (_elapsedTimeToRelease > RETREAT_TIME)
                    {
                        this.CurrentFrame = 0;
                        this.Sprite = _SpriteSpikesDown;
                        this.State = SpikesStateType.Releasing;
                        _elapsedTimeToActivate = _elapsedTimeToRelease - RETREAT_TIME; // Inicializar o tempo para a próxima activação. N começar no 0 caso contrario perde-se o excedente
                        this._spikesClosingSound.Play();
                    }
                    break;
                    
                case SpikesStateType.Releasing:
                    if (this._empaledSnail != null)
                    {
                        this.PlaceSnailOnSpikes();
                    }
                   
                    break;
            }
        }
/*
#if DEBUG
        public override void  Draw(bool shadow)
        {
            base.Draw(shadow);
            this._crateCollisionBB.Draw(Stage.CurrentStage.SpriteBatch, Color.Blue, Vector2.Zero);
            this.crateBs1.Draw(Stage.CurrentStage.SpriteBatch, Color.Green, Vector2.Zero);
            //    this.BoundingBox.Draw(Color.Blue, Vector2.Zero);
        }
#endif*/
        public override void OnLastFrame()
        {
            base.OnLastFrame();
            if (this.State == SpikesStateType.Releasing)
            {
                this.CurrentFrame = 0;
                this.Sprite = _SpriteSpikesIdle;
                this.State = SpikesStateType.Idle;
                if (this._empaledSnail != null)
                {
                    this._empaledSnail.UnempaleFromSpikes();
                    this._empaledSnail = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
#if DEBUG_ASSERTIONS
            if (obj as Snail == null || listIdx != Stage.QUADTREE_SNAIL_LIST_IDX)
            {
                throw new BrainException("Expected snail object!");
            }
#endif
            // Spikes only kill one snail at a time
            if (this._empaledSnail != null)
            {
                return;
            }

            Snail snail = obj as Snail;
            snail.KillBySpikes(this);
            _empaledSnail = snail;
            this.PlaceSnailOnSpikes();
            this._spikeHitSnailSound.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlaceSnailOnSpikes()
        {
            BoundingSquare bs = this.TransformCurrentFrameBB().ToBoundingSquare();
            // This places snail BB on top of the spikes BB
            switch ((int)this.Rotation)
            {
                case 0:
                    this._empaledSnail.Position = new Microsoft.Xna.Framework.Vector2
                                (bs.Left - this._empaledSnail.Sprite.BoundingBox.Left,
                                 bs.Top - this._empaledSnail.Sprite.BoundingBox.Top - this._empaledSnail.Sprite.BoundingBox.Height);
                    break;
                case 90:
                    this._empaledSnail.Position = new Microsoft.Xna.Framework.Vector2(
                                    bs.Left + bs.Width + this._empaledSnail.Sprite.BoundingBox.Top + this._empaledSnail.Sprite.BoundingBox.Height, 
                                    bs.Top - this._empaledSnail.Sprite.BoundingBox.Left);
                    break;
                case 180:
                    this._empaledSnail.Position = new Microsoft.Xna.Framework.Vector2(
                                    bs.Left - this._empaledSnail.Sprite.BoundingBox.Left, 
                                    bs.Top + this._empaledSnail.Sprite.BoundingBox.Top + this._empaledSnail.Sprite.BoundingBox.Height + bs.Height);
                    break;
                case 270:
                    this._empaledSnail.Position = new Microsoft.Xna.Framework.Vector2(
                                    bs.Left - this._empaledSnail.Sprite.BoundingBox.Top - this._empaledSnail.Sprite.BoundingBox.Height,
                                    bs.Top - this._empaledSnail.Sprite.BoundingBox.Left);
                    break;
            }
        }

        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            if (context == ToDataFileRecordContext.StageSave)
            {
                record.AddField("activationTime", this.ActivationTime);
                record.AddField("startupDelay", this.StartupDelay);
                record.AddField("on", this.TurnedOn);
            }
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.ActivationTime = record.GetFieldValue<int>("activationTime", ACTIVATION_TIME);
            this.StartupDelay = record.GetFieldValue<int>("startupDelay", STARTUP_DELAY);
            this.TurnedOn  = record.GetFieldValue<bool>("on", true);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        #endregion
        

        /// <summary>
        /// See virtual method for details
        /// </summary>
        public override bool CrateToolIsValid(BoundingSquare crateBs)
        {
            return !(crateBs.Collides(this._crateCollisionBB));
        }

        #region ISwitchable Members

        public void SwitchOn()
        {
            this.TurnedOn = true;
         
        }

        public void SwitchOff()
        {
            this.TurnedOn = false;
        }

        public bool IsOn
        {
            get { return this.TurnedOn; }
        }

        #endregion
    }
}
