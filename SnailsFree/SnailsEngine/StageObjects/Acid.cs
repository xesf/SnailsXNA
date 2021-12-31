using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects.Shades;
using TwoBrainsGames.Snails.Screens;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.StageObjects
{
    public class Acid : Liquid
    {
        #region Constants
        private const int BUBBLES_PER_TILE = 1;
        private const float BUBBLES_SPEED = 50f;
        private const float NO_BUBBLES_LEVEL = 0.1f;
        #endregion

        class Bubble
        {
            public Vector2 _position;
            public float _scale;
            public SpriteAnimation _popAnimation;
            public Bubble()
            {
                this._position = Vector2.Zero;
                this._scale = 1f;
            }

            public void RandomizeScale()
            {
                this._scale = (float)(5 + BrainGame.Rand.Next(5)) / 10f;
            }

            public float RandomizeX(float xInterval)
            {
                return BrainGame.Rand.Next((int)xInterval);
            }
        }

        #region Members
        private Sprite _bubbleSprite;
        private List<Bubble> _bubbles;
        private int _bubbleCount;
        private Sample [] _bubblesSound;
        #endregion

        #region Properties
        #endregion

        public Acid()
            : base(StageObjectType.Acid)
        { }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._bubbleSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/acid", "Bubble");
            this._bubblesSound = new Sample[3];
            this._bubblesSound[0] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.BUBBLE_POP, this);
            this._bubblesSound[1] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.BUBBLE_POP_2, this);
            this._bubblesSound[2] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.BUBBLE_POP_3, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._bubbles = new List<Bubble>();
            this._bubbleCount = (int)(BUBBLES_PER_TILE * this.Size.X * this.Size.Y);
            for (int i = 0; i < this._bubbleCount; i++)
            {
                Bubble bubble = new Bubble();
                bubble._popAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/acid", "BubblePop"));
                bubble._popAnimation.Visible = false;
                bubble._popAnimation.Autohide = true;
                bubble._position = new Vector2(bubble.RandomizeX(this._liquidAABB.Width), this._sizeInPixels.Y - BrainGame.Rand.Next((int)this._liquidAABB.Height));
                bubble._position += this.Position;
                bubble.RandomizeScale();
                this._bubbles.Add(bubble);
            }
            this.RefreshBubbleCount();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshBubbleCount()
        {
            this._bubbleCount = (int)(this._liquidLevel * this._bubbles.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            if (!this.IsEmpty)
            {
                // Make the bubble count depend on the liquid level
                this.RefreshBubbleCount();
                if (this._liquidLevel < NO_BUBBLES_LEVEL)
                {
                    this._bubbleCount = 0;
                }
                // Bubbles update
                for (int i = 0; i < this._bubbleCount; i++)
                {
                    Bubble bubble = this._bubbles[i];
                    float speed = BUBBLES_SPEED * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f) * bubble._scale;
                    bubble._position = new Vector2(bubble._position.X,
                                                                bubble._position.Y - speed);
                    if (bubble._position.Y < this._drawRectangle.Y)
                    {
                        this.PlayPop(bubble);
                        bubble._popAnimation.Position = new Vector2(bubble._position.X, this._drawRectangle.Y);
                        bubble._popAnimation.Visible = true;
                        bubble._popAnimation.Scale = new Vector2(bubble._scale, bubble._scale);

                        bubble._position = new Vector2(bubble.RandomizeX(this._sizeInPixels.X), this._sizeInPixels.Y);
                        bubble._position += this.Position;
                        bubble.RandomizeScale();
                    }

                    if (bubble._popAnimation.Visible)
                    {
                        bubble._popAnimation.Update(gameTime);
                        bubble._popAnimation.Position = new Vector2(bubble._popAnimation.Position.X, this._drawRectangle.Y);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayPop(Bubble bubble)
        {
            int sound = BrainGame.Rand.Next(this._bubblesSound.Length);
            this._bubblesSound[sound].Play(bubble._scale);

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            if (!this.IsEmpty)
            {
                for (int i = 0; i < this._bubbleCount; i++)
                {
                    Bubble bubble = this._bubbles[i];
                    this._bubbleSprite.Draw(bubble._position, 0, 0f, SpriteEffects.None, Color.White, bubble._scale, Stage.CurrentStage.SpriteBatch);
                    if (bubble._popAnimation.Visible)
                    {
                        bubble._popAnimation.Draw(Stage.CurrentStage.SpriteBatch);
                    }
                }
            }
        }

        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
        }

        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);

            return record;
        }
        #endregion
    }
}
