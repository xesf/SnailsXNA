using System;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.Transitions
{
    class LeafTransition : Transition
    {

        public enum State
        {
            Closing,
            Opening,
            ClosedStopped
        }
        const float LEAF1_OPENED_ANGLE = -45.0f;
        const float LEAF2_OPENED_ANGLE = 55.0f;
        const float LEAF3_OPENED_ANGLE = -80.0f;

        const float LEAF1_CLOSED_ANGLE = 55.0f;
        const float LEAF2_CLOSED_ANGLE = -45.0f;
        const float LEAF3_CLOSED_ANGLE = 20.0f;

        const float MIN_SPEED = 0.1f;
        const float MAX_SPEED = 20.0f;

        Sprite _leafSprite;
        Vector2 _leaf1Position;
        Vector2 _leaf2Position;
        Vector2 _leaf3Position;
        float _rotation1;
        float _rotation2;
        float _rotation3;
        float _speed;

        Color _leaf1Color;
        Color _leaf2Color;
        Color _leaf3Color;

        State _state;

        bool _leaf1Ended;
        bool _leaf2Ended;
        bool _leaf3Ended;

        bool _endOnNextLoop; // This is to end the effect only on the next update after the effect ended
                             // This is needed because the last frame is not rendered when _ended is set
        Sample _leafsOpenSample;
        Sample _leafsCloseSample;
        public bool StopSounds { get; set; }
        public SpriteBatch SpriteBatch
        {
            get { return BrainGame.SpriteBatch; }
        }
        public LeafTransition(State state)
        {
            this._state = state;
            this.StopSounds = true;
        }

        public override void LoadContent()
        {
            this._leafSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/leaf", "Leaf");
            this._leafsOpenSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.LEAFS_OPEN_TRANSITION);
            this._leafsCloseSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.LEAFS_CLOSE_TRANSITION);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._leaf1Position = new Vector2(-220.0f, 0.0f);
            this._leaf2Position = new Vector2(BrainGame.ScreenWidth, -200.0f);
            this._leaf3Position = new Vector2(BrainGame.ScreenWidth + 230.0f, BrainGame.ScreenHeight);

            switch (this._state)
            {
                case State.Closing:
                    this._rotation1 = LEAF1_OPENED_ANGLE;//; 55.0f;
                    this._rotation2 = LEAF2_OPENED_ANGLE;//; -45.0f;
                    this._rotation3 = LEAF3_OPENED_ANGLE;//; 20.0f;
                    this._speed = MAX_SPEED;
                   break;
                case State.ClosedStopped:
                    this._rotation1 = LEAF1_CLOSED_ANGLE;//; 55.0f;
                    this._rotation2 = LEAF2_CLOSED_ANGLE;//; -45.0f;
                    this._rotation3 = LEAF3_CLOSED_ANGLE;//; 20.0f;
                    this._speed = 0.0f;
                    break;
                case State.Opening:
                    this._rotation1 = LEAF1_CLOSED_ANGLE;//; 55.0f;
                    this._rotation2 = LEAF2_CLOSED_ANGLE;//; -45.0f;
                    this._rotation3 = LEAF3_CLOSED_ANGLE;//; 20.0f;
                    this._speed = 0.01f;
                    break;
            }

            this._leaf1Color = new Color(255, 255, 255, 255);
            this._leaf2Color = new Color(255, 255, 130, 255);
            this._leaf3Color = new Color(255, 255, 200, 255);

            this._leaf1Ended = false;
            this._leaf2Ended = false;
            this._leaf3Ended = false;

            this._endOnNextLoop = false;

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this._endOnNextLoop)
            {
                this._ended = true;
                return;
            }

            switch (this._state)
            {
                case State.Closing:
                    this.UpdateClosing(gameTime);
                    break;
                case State.Opening:
                    this.UpdateOpening(gameTime);
                    break;
                case State.ClosedStopped:
                    this._endOnNextLoop = true;
                    break;
            }
       
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {

            if (this._state == State.Opening)
            {
                this._leafsOpenSample.Play();
            }
            else
            {
                this._leafsCloseSample.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateClosing(BrainGameTime gameTime)
        {
            float ang = this._speed * (float)gameTime.ElapsedRealTime.TotalMilliseconds / 100;

            if (this._rotation1 > LEAF1_CLOSED_ANGLE - 20.0f) // Ease the speed after a while
            {
                // leafcloseangle = 20
                // angle          = x
                this._speed = 22.0f - (this._rotation1 * MAX_SPEED / LEAF1_CLOSED_ANGLE);
                if (this._speed < MIN_SPEED)
                    this._speed = MIN_SPEED;
            }
            this._rotation1 += ang;
            this._rotation2 -= ang;
            this._rotation3 += ang;


            if (this._rotation1 > LEAF1_CLOSED_ANGLE)
            {
                this._rotation1 = LEAF1_CLOSED_ANGLE;
                this._leaf1Ended = true;
            }
            if (this._rotation2 < LEAF2_CLOSED_ANGLE)
            {
                this._rotation2 = LEAF2_CLOSED_ANGLE;
                this._leaf2Ended = true;
            }
            if (this._rotation3 > LEAF3_CLOSED_ANGLE)
            {
                this._rotation3 = LEAF3_CLOSED_ANGLE;
                this._leaf3Ended = true;
            }

            if (this._leaf1Ended && this._leaf2Ended && this._leaf3Ended)
            {
                this._endOnNextLoop = true;
                if (this.StopSounds)
                {
                    BrainGame.SampleManager.StopAll();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateOpening(BrainGameTime gameTime)
        {
            float ang = this._speed * (float)gameTime.ElapsedRealTime.TotalMilliseconds / 100;

            if (this._rotation1 > LEAF1_CLOSED_ANGLE - 20.0f) // Ease the speed after a while
            {
                this._speed = 22.0f - (this._rotation1 * MAX_SPEED / LEAF1_CLOSED_ANGLE);
                if (this._speed > MAX_SPEED)
                    this._speed = MAX_SPEED;
            }
            else
                this._speed = MAX_SPEED;

            this._rotation1 -= ang;
            this._rotation2 += ang;
            this._rotation3 -= ang;

            if (this._rotation1 < LEAF1_OPENED_ANGLE)
            {
                this._rotation1 = LEAF1_OPENED_ANGLE;
                this._leaf1Ended = true;
            }
            if (this._rotation2 > LEAF2_OPENED_ANGLE)
            {
                this._rotation2 = LEAF2_OPENED_ANGLE;
                this._leaf2Ended = true;
            }
            if (this._rotation3 < LEAF3_OPENED_ANGLE)
            {
                this._rotation3 = LEAF3_OPENED_ANGLE;
                this._leaf3Ended = true;
            }

            if (this._leaf1Ended && this._leaf2Ended && this._leaf3Ended)
            {
                this._endOnNextLoop = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            this.Draw(this.SpriteBatch);
            this.SpriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            float scale = 2.5f;
            if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LD)
            {
                scale = 2f;
            }

            this._leafSprite.Draw(this._leaf3Position, 0, this._rotation3, SpriteEffects.FlipHorizontally, this._leaf3Color, scale, spriteBatch);
            this._leafSprite.Draw(this._leaf2Position, 0, this._rotation2, SpriteEffects.FlipHorizontally, this._leaf2Color, scale, spriteBatch);
            this._leafSprite.Draw(this._leaf1Position, 0, this._rotation1, SpriteEffects.None, this._leaf1Color, scale, spriteBatch);
        }

    }
}
