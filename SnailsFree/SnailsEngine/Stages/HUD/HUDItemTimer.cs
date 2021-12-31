using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    class HUDItemTimer : HUDItem
    {
 

        #region Vars
        SpriteAnimation _spriteTimer;
        Sprite _spriteTimeWarp;
        Vector2 _timerPosition;
        Vector2 _timeWarpPosition;
        Vector2 _stringPosition;
        Color _timerColor; // Color for the timer
        string _timerString;
        bool _timerStopped;
        private float _timeWarpRotation;
        double _timerEllapsed; // Actualy works the inverse way. It decreases from 1000 to 0
        //BoundingSquare _bsClock; // Clock BS, used to catch clicks to enter time warp mode
        private Sample _tickSample;
        private Sample _timerUpSample;

        #endregion

        public HUDItemTimer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            this._timerPosition = position + new Vector2(0f, 10f); // Pull a little bit down
            this._stringPosition = position + new Vector2(40f, 10f);
            this._timeWarpPosition = position + new Vector2(60f, 20f);

            this.UpdateString();
            this._width = 125f;
            this._timerColor = Colors.StageHUDInfoColor;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            // Timer
            this._spriteTimer = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "Timer"));
            this._spriteTimeWarp = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "TimeWarp");

            _tickSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TIMER_TICK);
            _timerUpSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TIME_UP);
            //this._bsClock = new BoundingSquare(this._timerPosition + new Vector2(3f, 3f), 95f, 23f);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void HandleInput(BrainGameTime gameTime)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
           
            if (Stage.CurrentStage.InTimeWarp)
            {
                this._timeWarpRotation -= 0.5f * (float)gameTime.ElapsedRealTime.TotalMilliseconds;
                if (this._timeWarpRotation < 0f)
                {
                    this._timeWarpRotation += 360f;
                }
            } 
            
            if (!this._timerStopped)
            {
                if (Stage.CurrentStage.LevelStage._goal == GoalType.TimeAttack)
                {
                    if (Stage.CurrentStage.Stats.Timer.TotalSeconds > 0)
                    {
                        Stage.CurrentStage.Stats.Timer = Stage.CurrentStage.Stats.Timer.Subtract(gameTime.ElapsedGameTime);
                        if (Stage.CurrentStage.Stats.Timer.TotalSeconds <= 0)
                        {
                            this.StopTimer();
                            Stage.CurrentStage.Stats.Timer = new TimeSpan(0);
                            _timerUpSample.Play();
                        }
                    }
                }
                else
                {
                    Stage.CurrentStage.Stats.Timer = Stage.CurrentStage.Stats.Timer.Add(gameTime.ElapsedGameTime);
                }
            }

            this._timerEllapsed -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (this._timerEllapsed < 0)
            {
                double decimalSecs = (double)Stage.CurrentStage.Stats.Timer.TotalMilliseconds / 1000;
                if (!this._timerStopped) // Don't do this first, or else the timestring will not be updated after the timer has stopped
                {
                    if (Stage.CurrentStage.LevelStage._goal == GoalType.TimeAttack)
                    {
                        if (decimalSecs < 10)
                        {
                            this._timerColor = Colors.StageHUDTimerLowColor;
                            if (Stage.CurrentStage.Stats.Timer.TotalSeconds > 0)
                            {
                                _tickSample.Play();
                            }

                        }
                        // Increment the clock sprite frame. This will ensure that the sprite and the text are synchronized
                        this._spriteTimer.DecrementFrame();
                    }
                    else
                    {
                        // Increment the clock sprite frame. This will ensure that the sprite and the text are synchronized
                        this._spriteTimer.IncrementFrame();
                    }

                    this._timerEllapsed = 1000 + this._timerEllapsed;
                }
                this.UpdateString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            // Timer
            if (Stage.CurrentStage.InTimeWarp)
            {
                this._spriteTimeWarp.Draw(this._timeWarpPosition, 0, this._timeWarpRotation, SpriteEffects.None, spriteBatch);
            }
            this._spriteTimer.Draw(this._timerPosition, spriteBatch);
            this._font.DrawString(spriteBatch, this._timerString, this._stringPosition, new Vector2(1.0f, 1.0f), this._timerColor);
       //     this._bsClock.Draw(spriteBatch, Color.Red, Vector2.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopTimer()
        {
            this._timerStopped = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateString()
        {
            TimeSpan ts = Stage.CurrentStage.Stats.Timer;
            this._timerString = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

            // if we take more than an hour to accomplish a game than just fix the time on the HUD
            if (ts.Hours > 0) // fix displayed time (we still keep the original timer
            {
                this._timerString = "60:00";
            }
        }
    }
}
