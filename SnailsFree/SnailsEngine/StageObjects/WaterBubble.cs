using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.Effects;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class WaterBubble : MovingObject
    {
        public enum BubbleSize
        {
            Large = 1,
            Medium = 2,
            Small = 3,
            Random = 4
        }

        enum BubbleStatus
        { 
            None,
            Floating,
            BubbleOut
        }

        public const string ID = "WATER_BUBBLE";

        FloatingEffect _floatEffect;
        BubbleSize _size;
        Sprite _bubbleOutSprite;
        BubbleStatus _status;

        #region Properties
        public BubbleSize Size 
        { 
            get { return _size; } 
            set 
            { 
                _size = value;
                if (value == BubbleSize.Random)
                {
                    float scale = (float)(2 + BrainGame.Rand.Next(3)) / 10f;
                    this.Scale = new Vector2(scale, scale);
                }
                else
                {
                    this.Scale /= (int)_size;
                }
            } 
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public WaterBubble()
            : this(StageObjectType.WaterBubble)
        { }

        /// <summary>
        /// 
        /// </summary>
        protected WaterBubble(StageObjectType type)
            : base(type)
        { }

        /// <summary>
        /// 
        /// </summary>
        public WaterBubble(StageObject other)
            : base(other)
        {
            this.Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            this._bubbleOutSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "BubblePickUp");
        }

        public void FloatToSurface(Water inWaterObj, float speed)
        {
            _status = BubbleStatus.Floating;

            _floatEffect = new FloatingEffect(speed, this.Position, inWaterObj);
            _floatEffect.Amplitude = 2;
            _floatEffect.Interval = 4;

            this.EffectsBlender.Add(_floatEffect);
        }

        public override void OnLastFrame()
        {
            base.OnLastFrame();
            if (_status == BubbleStatus.BubbleOut)
            {
                this.FadeOut(5f);
            }
        }

        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            if (_status == BubbleStatus.Floating && _floatEffect.Ended)
            {
                this._status = BubbleStatus.BubbleOut;
                this.Sprite = this._bubbleOutSprite;
                this.CurrentFrame = 0;
            }
        }
    }
}
