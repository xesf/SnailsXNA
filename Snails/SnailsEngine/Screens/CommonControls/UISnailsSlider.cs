using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Audio;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsSlider : UISlider
    {
        #region Members
        private Sample _sliderSample;
        #endregion

        #region Properties
        public bool PlayChangedSound { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UISnailsSlider(UIScreen screenOwner) :
            base(screenOwner, "spriteset/boards/Slider", "spriteset/boards/SliderManipulator")
        {
            this.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this.SetSliderSlotToSpriteBB(true);
            this.OnValueChangedEnded += new UIEvent(UISnailsSlider_OnValueChangedEnded);
            this.OnValueChanged += new UIEvent(UISnailsSlider_OnValueChanged);
            this._sliderSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.SLIDER_VALUE_CHANGED);
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsSlider_OnValueChangedEnded(IUIControl sender)
        {
            if (this.PlayChangedSound)
            {
                _sliderSample.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsSlider_OnValueChanged(IUIControl sender)
        {
            if (this.PlayChangedSound)
            {
                _sliderSample.Play();
            }
        }


    }
}
