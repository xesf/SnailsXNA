using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.BrainEngine.Graphics
{
    public class SpriteAnimation
    {
        public delegate void LastFrameHandler();
        public event LastFrameHandler OnLastFrame;

        public SpriteEffects _spriteEffect;
        public Sprite Sprite;
        public int CurrentFrame;
        public int ElapsedFrameTime;
        private bool _paused;
        public ColorEffect _fadeEffect;

        public double TotalPlayTime { get { return this.Sprite.TotalTime; } }
        public int FrameCount
        {
            get {
                return (this.Sprite == null ? 0 : this.Sprite.FrameCount);
            }
        }
        public float LayerDepth
        {
            get { return this.Sprite.LayerDepth; }
            set { this.Sprite.LayerDepth = value; }
        }
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }
        public bool Autohide { get; set; } // Animation will auto hide when animation ends
        public TransformBlender EffectsBlender { get; set; }
        public Color BlendColor { get; set; }
        public Vector2 Scale { get; set; }
        public int PauseAtEndInMsecs { get; set; }

        public int Height { get { return this.Sprite.Frames[0].Height; } }

        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimation(Sprite sprite)
        {
            this.Sprite = sprite;
            this.EffectsBlender = new TransformBlender();
            this.BlendColor = Color.White;
            this._paused = false;
            this.Scale = new Vector2(1f, 1f);
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteAnimation(string resourceId, string resourceManagerId):
            this(BrainGame.ResourceManager.GetSprite(resourceId, resourceManagerId))
        {
           
        }

        /// 
        /// </summary>
        public SpriteAnimation Clone()
        {
            SpriteAnimation anim = new SpriteAnimation(this.Sprite);
            anim.Visible = this.Visible;
            anim.Position = this.Position;
            anim.Autohide = this.Autohide;
            anim.BlendColor = this.BlendColor;
            anim._paused = this._paused;
            anim.Scale = this.Scale;
            return anim;
        }

        /// <summary>
        ///
        /// </summary>
        public void SetSprite(Sprite sprite)
        {
            this.Sprite = sprite;
            this.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            this.CurrentFrame = 0;
            this._paused = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            this.Update(gameTime, false);
        }

        /// <summary>
        /// 
        /// </summary>
        private int GetCurrentFrameTime()
        {
            if (this.Sprite.Frames[this.CurrentFrame].PlayTime == 0)
            {
                if (this.Sprite.Fps == 0)
                {
                    return 0;
                }
                return (1000 / this.Sprite.Fps); // Replace this by pre calculated value in Sprite
            }

            return this.Sprite.Frames[this.CurrentFrame].PlayTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime, bool useRealTime)
        {
            if (this._paused)
            {
                return;
            }
            if (this.EffectsBlender.Count > 0)
            {
                this.EffectsBlender.Update(gameTime);
                this.Position += this.EffectsBlender.PositionV2;
                this.BlendColor = this.EffectsBlender.Color;
            }


            if (this.Sprite.FrameCount >= 1)
            {
                if (!useRealTime)
                {
                    this.ElapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    this.ElapsedFrameTime += gameTime.ElapsedRealTime.Milliseconds;
                }
                // Change to a new frame?
                int frameTime = this.GetCurrentFrameTime();
                do
                {
                    if (this.ElapsedFrameTime > frameTime)
                    {
                        this.CurrentFrame++;// = ((this.CurrentFrame + 1) % this.Sprite.FrameCount);
                        if (this.CurrentFrame >= this.Sprite.FrameCount)
                        {
                           
                            this.CurrentFrame = 0;
                            if (this.OnLastFrame != null)
                            {
                                this.OnLastFrame();
                            }
                            this.Visible = !this.Autohide;
                        }
                        this.ElapsedFrameTime -= frameTime;
                        frameTime = this.GetCurrentFrameTime();
                    }
                }
                while (this.ElapsedFrameTime > frameTime && frameTime != 0);

                /*
                if (this.ElapsedFrameTime > (1000 / this.Sprite.Fps) + this.Sprite.Frames[this.CurrentFrame].PlayTime)
                {
                    this.CurrentFrame = ((this.CurrentFrame + 1) % this.Sprite.FrameCount);
                    this.ElapsedFrameTime -= (1000 / this.Sprite.Fps) + this.Sprite.Frames[this.CurrentFrame].PlayTime;
                    if (this.CurrentFrame == 0)
                    {
                        if (this.OnLastFrame != null)
                        {
                            this.OnLastFrame();
                        }
                    }
                }*/
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(this.Position, spriteBatch);
        }

        public void Draw(Vector2 position, SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(position, this.CurrentFrame, 0.0f, _spriteEffect,   this.BlendColor, this.Scale.X, spriteBatch);
        }

        public void Draw(Vector2 position, float rotation, SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(position, this.CurrentFrame, rotation, _spriteEffect, this.BlendColor, this.Scale.X, spriteBatch);
        }

        public void Draw(Vector2 position, float rotation, SpriteEffects effect, SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(position, this.CurrentFrame, rotation, effect, this.BlendColor, this.Scale.X, spriteBatch);
        }
   
        public void Draw(Vector2 position, float rotation, Color color, SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(position, this.CurrentFrame, rotation, _spriteEffect, color, this.Scale.X, spriteBatch);
        }

        public void Draw(Vector2 position, Color opacity, SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(position, this.CurrentFrame, 0.0f, _spriteEffect, opacity, this.Scale.X, spriteBatch);
        }

        public void Draw(Vector2 position, Color opacity, Vector2 scale, SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(position, this.CurrentFrame, 0.0f, this.Sprite.Offset, scale.X, scale.Y, opacity, spriteBatch);
        }

        public void Draw(Vector2 position, float rotation, Color opacity, Vector2 scale, Vector2 pivotPoint, SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(position, this.CurrentFrame, rotation, pivotPoint, scale.X, scale.Y, opacity, spriteBatch);
        }

        public void Draw(Rectangle destRect, Color opacity, SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(destRect, this.CurrentFrame, opacity, spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        public void IncrementFrame()
        {
            this.CurrentFrame++;
            if (this.CurrentFrame >= this.Sprite.FrameCount)
            {
                this.CurrentFrame = 0;
                if (this.OnLastFrame != null)
                {
                    this.OnLastFrame();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DecrementFrame()
        {
            this.CurrentFrame--;
            if (this.CurrentFrame  < 0)
            {
                this.CurrentFrame = this.Sprite.FrameCount - 1;
                if (this.OnLastFrame != null)
                {
                    this.OnLastFrame();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PausePlay()
        {
            this._paused = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResumePlay()
        {
            this._paused = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FadeIn(float speed)
        {
            if (this._fadeEffect == null)
            {
                this.CreateFadeEffect(speed);
            }
            this.Visible = true;
            this._fadeEffect.Active = true;
            
            this._fadeEffect.Reset(new Color(0, 0, 0, 0), Color.White);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FadeOut(float speed)
        {
            if (this._fadeEffect == null)
            {
                this.CreateFadeEffect(speed);
            }
            this._fadeEffect.Active = true;

            this._fadeEffect.Reset(Color.White, new Color(0, 0, 0, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateFadeEffect(float speed)
        {
            this._fadeEffect = new ColorEffect(Color.White, new Color(0, 0, 0, 0), speed, false);
            this._fadeEffect.AutoDeleteOnEnd = false;
            this._fadeEffect.OnEnd = this._fadeEffect_OnEndEvt;
            this._fadeEffect.Active = false;
            this.EffectsBlender.Add(this._fadeEffect);
        }

        /// <summary>
        /// 
        /// </summary>
        void _fadeEffect_OnEndEvt(object param)
        {
            if (this._fadeEffect.Color.A == 0)
            {
                this.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RandomizeFrame()
        {
            this.CurrentFrame = BrainGame.Rand.Next(this.FrameCount);
        }
    }
}
