using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    // A UISnailStamp is a simple object that has the effect of an object being placed on a board
    // (like the "Fail" stamp in the Mission Failed Screen)
    // This is also used in the medal in the Stage Completed Screen
    class UISnailStamp : UIControl
    {   
        #region Vars
        protected UIImage _imgImage;
        #endregion

        #region Properties
        public bool IgnoreShowEffect { get; set; }
        public Sample ShowSample { get; set; }
        #endregion
         /// <summary>
        /// 
        /// </summary>
        public UISnailStamp(UIScreen screenOwner) :
            this(screenOwner, null, null)
        {
       }

        /// <summary>
        /// 
        /// </summary>
        public UISnailStamp(UIScreen screenOwner, string imageResource, string resourceManagerId) :
            base(screenOwner)
        {
            // Image
            this._imgImage = new UIImage(screenOwner);
            if (imageResource != null)
            {
                this._imgImage.Sprite = BrainGame.ResourceManager.GetSprite(imageResource, resourceManagerId);
            }
            this._imgImage.OnShow += new UIEvent(_imgImage_OnShow);
            this._imgImage.ShowEffect = new ScaleEffect(new Vector2(10.0f, 10.0f), 30.0f, new Vector2(1.0f, 1.0f), false);
            this._imgImage.BlendColorWithParent = true;
            this.Controls.Add(this._imgImage);

            this.AcceptControllerInput = false;

            this.ShowSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.FAIL_STAMP);

            this.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Show()
        {
            if (!this.IgnoreShowEffect)
            {
                base.Show();
                if (this.ShowSample != null)
                {
                    this.ShowSample.Play();
                }
            }
            else
            {
                this.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _imgImage_OnShow(IUIControl sender)
        {
            this.Effect = new SquashEffect(0.8f, 4.0f, 0.04f, this.BlendColor, this.Scale);
            this.Effect.OnEnd = this.EffectEnded;
        }


        /// <summary>
        /// 
        /// </summary>
        private void EffectEnded(object param)
        {
            this.InvokeOnShow();
        }


        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            this.Scale = new Vector2(1.0f, 1.0f);
        }
    }
}
