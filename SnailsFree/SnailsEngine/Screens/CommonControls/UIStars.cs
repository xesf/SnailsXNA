using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIStars : UIControl
    {
        class Star
        {
            UIStars _owner;
            public Vector2 _position;
            public float _speed;
            public float _scale;
            public Color _color;
            public float _fadeSpeed;
            public float _colorAlpha;
            public double _releaseTime;
            public double _curReleaseTime;
            public double _decayTime; // When the decay time expires the star will fade out
            public double _curDecayTime;
            public bool _visible;
            public Star(UIStars owner)
            {
                this._owner = owner;
            }

            public void Randomize()
            {
                this._position = new Vector2(BrainGame.Rand.Next((int)this._owner.Size.Width),
                                             BrainGame.Rand.Next((int)this._owner.Size.Height));
                this._scale = 0.4f + ((float)BrainGame.Rand.Next(60) / 100f);
                this._speed = 0.8f + ((float)BrainGame.Rand.Next(100) / 100f);
                this._fadeSpeed = 0.04f;// +((float)BrainGame.Rand.Next(100) / 100f);
                this._color = this.RandomizeColor();
                this._colorAlpha = 1f;
                this._decayTime = this._owner.StarsDurationMinimum + BrainGame.Rand.Next(this._owner.StarsDurationMaximum - this._owner.StarsDurationMinimum);
                this._curDecayTime = 0;
            }

            private Color RandomizeColor()
            {
               /* int type = BrainGame.Rand.Next(5);
                switch (type)
                {
                    // Redish
                    case 0:
                        return  new Color(128 + BrainGame.Rand.Next(128),
                                          BrainGame.Rand.Next(30),
                                          BrainGame.Rand.Next(30));
                    // Blueish
                    case 1:
                        return  new Color(BrainGame.Rand.Next(30),
                                          BrainGame.Rand.Next(30),
                                          128 + BrainGame.Rand.Next(128));
                    // Greenish
                    case 2:
                        return  new Color(BrainGame.Rand.Next(30),
                                          128 + BrainGame.Rand.Next(128),
                                          BrainGame.Rand.Next(30));
                    // Yellowish
                    case 3:
                        return  new Color(128 + BrainGame.Rand.Next(128),
                                          128 + BrainGame.Rand.Next(128),
                                          BrainGame.Rand.Next(30));
                }
                */
                return new Color(128 + BrainGame.Rand.Next(128),
                                        128 + BrainGame.Rand.Next(128),
                                        128 + BrainGame.Rand.Next(128),
                                        255);
           }

        }


        /// <summary>
        /// 
        /// </summary>
        Sprite _starsSprite;
        Star [] Stars { get; set; }
        int StarCount { get; set; }
        int Duration { get; set; }
        int EllapsedDuration { get; set; }
        bool DurationExpired { get; set; }
        int StarsDurationMaximum { get; set; }
        int StarsDurationMinimum { get; set; }

        public UIStars(UIScreen screenOwner, int numStars, int duration, int starsDurationMinimum, int starsDurationMaximum) :
            base(screenOwner)
        {
            this._starsSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "Star");
            this.StarCount = numStars;
            this.Duration = duration;
            this.StarsDurationMaximum = starsDurationMaximum;
            this.StarsDurationMinimum = starsDurationMinimum;
            this.AcceptControllerInput = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this.Stars = new Star[this.StarCount];
            for (int i = 0; i < this.Stars.Length; i++ )
            {
                this.Stars[i] = new Star(this);
                this.Stars[i].Randomize();
                this.Stars[i]._releaseTime = BrainGame.Rand.Next(2000);
                this.Stars[i]._visible = false;
            }
            this.EllapsedDuration = 0;
            this.DurationExpired = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this.EllapsedDuration < this.Duration)
            {
                this.EllapsedDuration += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                this.DurationExpired = true;
            }

            for (int i = 0; i < this.Stars.Length; i++)
            {
                Star star = this.Stars[i];
                if (star._visible)
                {
                    star._position -= new Vector2(0f, star._speed * gameTime.ElapsedGameTime.Milliseconds);
                    star._curDecayTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (star._curDecayTime > star._decayTime) // Decay expired? Start fading the star
                    {
                        star._color = new Color((int)((float)star._color.R * star._colorAlpha),
                                                         (int)((float)star._color.G * star._colorAlpha),
                                                         (int)((float)star._color.B * star._colorAlpha),
                                                         (int)((float)star._color.A * star._colorAlpha));
                        star._colorAlpha -= this.Stars[i]._fadeSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                        if (star._color.A == 0) // Star faded? Reset it
                        {
                            star.Randomize();
                            star._visible = false;
                        }
                    }
                }
                else
                {
                    if (!this.DurationExpired)
                    {
                        star._curReleaseTime += gameTime.ElapsedGameTime.Milliseconds;
                        if (star._curReleaseTime >= star._releaseTime)
                        {
                            star._curReleaseTime = star._releaseTime; // Stars are recycled, Next time there will be no pause
                            star._visible = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            for (int i = 0; i < this.Stars.Length; i++)
            {
                if (this.Stars[i]._visible)
                {
                    this._starsSprite.Draw(this.ScreenUnitToPixels(this.Stars[i]._position) + this.AbsolutePositionInPixels, 0, 0, Vector2.Zero, this.Stars[i]._scale, this.Stars[i]._scale, this.Stars[i]._color, this.ScreenOwner.SpriteBatch);
                }
            }
        }
    }
}
