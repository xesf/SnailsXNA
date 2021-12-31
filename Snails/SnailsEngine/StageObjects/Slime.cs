using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class Slime : StageObject
    {
        public const string ID = "SLIME";
        Sample _slimeSound;

        /// <summary>
        /// 
        /// </summary>
        public Slime()
            : base(StageObjectType.Slime)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._slimeSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SNAIL_ENTERING_STAGE, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            this.SpriteAnimationActive = false;
            this.CurrentFrame = this.Sprite.FrameCount - 1;
            this.FadeOut(0.5f);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnAddedToStage()
        {
            base.OnAddedToStage();
            if (BrainGame.Rand.Next(5) == 0)
            {
                this._slimeSound.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
