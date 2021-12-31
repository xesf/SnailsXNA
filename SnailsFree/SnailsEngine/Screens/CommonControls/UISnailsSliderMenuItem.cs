using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Controls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsSliderMenuItem : UISnailsMenuItem
    {
        #region Events
        public event UIEvent OnValueChanged;
        public event UIEvent OnValueChangedEnded;
        #endregion

        #region Vars
        UISnailsSlider _slider;
        UITextFontLabel _plusLabel;
//        UITextFontLabel _minusLabel;
        #endregion


        #region Properties
        public float Value
        {
            get { return this._slider.Value; }
            set { this._slider.Value = value; }
        }
        public bool PlayChangedSound 
        {
            get { return this._slider.PlayChangedSound; }
            set { this._slider.PlayChangedSound = value; }
        }
       
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UISnailsSliderMenuItem(UIScreen ownerScreen, UISnailsMenu snailsMenuOwner) :
            base(ownerScreen, null, null, snailsMenuOwner, false)
        {
            this.HotSpotBBIndex = 1;
            this.Image = null;
            this._slider = new UISnailsSlider(ownerScreen);
            this._slider.OnValueChanged += new UIEvent(_slider_OnValueChanged);
            this._slider.OnValueChangedEnded += new UIEvent(_slider_OnValueChangedEnded);
            this._slider.OnFocus += new UIEvent(_slider_OnFocus);
            this._slider.OnLostFocus += new UIEvent(_slider_OnLostFocus);
            this.Controls.Add(this._slider);

            // Plus
            this._plusLabel = new UITextFontLabel(ownerScreen, snailsMenuOwner.MenuItemsFontUnselected);
            this._plusLabel.Text = "+";
            this._plusLabel.ParentAlignment = BrainEngine.UI.AlignModes.Right;
            this._plusLabel.Margins.Right = this.NativeResolutionX(150f);
            this._plusLabel.Position = this.NativeResolution(new Vector2(0, 340));
            this._slider.Controls.Add(this._plusLabel);

            // Minus
            // I think minus label is not needed
         //   this._minusLabel = new UITextFontLabel(ownerScreen, snailsMenuOwner.MenuItemsFontUnselected);
         //   this._minusLabel.Text = "-";
         //   this._minusLabel.Position = this.NativeResolution(new Vector2(1000, 340));
           // this._slider.Controls.Add(this._minusLabel);

            // Text
            this.Label.ParentAlignment = BrainEngine.UI.AlignModes.None;
            this.Label.Position = this.NativeResolution(new Vector2(250, 340));
            this.Label.BringToFront();

            this.Size = this._slider.Size;
            this.AutoHideMenu = false;
            this.BackgroundImage = this._slider.Image;
        
            this.Reset();
        }



        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();
         /*   if (this._minusLabel != null)
            {
                this._minusLabel.Font = this.SnailsMenuOwner.MenuItemsFontUnselected;
                this._minusLabel.BlendColor = Colors.MenuItem;
                this._minusLabel.BlendScaleWithParent = false;
            }*/
            if (this._plusLabel != null)
            {
                this._plusLabel.Font = this.SnailsMenuOwner.MenuItemsFontUnselected;
                this._plusLabel.BlendColor = Colors.MenuItem;
                this._plusLabel.BlendScaleWithParent = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _slider_OnLostFocus(IUIControl sender)
        {
            this.InvokeOnLostFocus();
        }

        /// <summary>
        /// 
        /// </summary>
        void _slider_OnFocus(IUIControl sender)
        {
            this.InvokeOnFocus();
        }

        /// <summary>
        /// 
        /// </summary>
        void _slider_OnValueChanged(IUIControl sender)
        {
            if (this.OnValueChanged != null)
            {
                this.OnValueChanged(this);
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        void _slider_OnValueChangedEnded(IUIControl sender)
        {
            if (this.OnValueChangedEnded != null)
            {
                this.OnValueChangedEnded(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Resize()
        {
            base.Resize();
            if (this._slider != null)
            {
                this.Size = this._slider.Size;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void GotFocus()
        {
            base.GotFocus();

        /*    this._minusLabel.BlendColor = Colors.MenuItemSelected;
            this._minusLabel.BlendScaleWithParent = true;
            this._minusLabel.Font = this.SnailsMenuOwner.MenuItemsFont;
*/
            this._plusLabel.BlendColor = Colors.MenuItemSelected;
            this._plusLabel.BlendScaleWithParent = true;
            this._plusLabel.Font = this.SnailsMenuOwner.MenuItemsFont;
        }
    }
}


