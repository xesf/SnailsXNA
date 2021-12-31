using System;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsMenuItem : UIMenuItem
    {
        #region Consts
        public static Vector2 DEFAULT_SCALE = new Vector2(0.9f, 0.9f);
        protected const float SELECTED_ITEM_SCALE = 1.0f;// Item scale when gets focus
        #endregion

        #region Events
        public event UIControl.UIEvent OnSelect;
        #endregion

        #region Properties
        public UIInstructionLabel.LabelActionTypes InstructionLabelType { get; protected set; }
        public bool AutoHideMenu { get; set; }
        public UISnailsMenu SnailsMenuOwner { get; private set; }
        private UISnailsMenu.MenuItemSize ItemSize { get; set; }
        private Sample _itemSelectedSample;
        public new bool WithFocus { get; private set; }
        #endregion
        //private Sample _itemShownSample;
        private Sample _itemFocusSample;


        /// <summary>
        /// 
        /// </summary>
        public UISnailsMenuItem(UIScreen ownerScreen, string textResourceId, TextFont spriteFont, UISnailsMenu snailsMenuOwner, bool allowsSelection) :
            base(ownerScreen, textResourceId, spriteFont)
        {
            this.HotSpotBBIndex = 0;
            switch (snailsMenuOwner.ItemSize)
            {
                case UISnailsMenu.MenuItemSize.Big:
                    this.Image = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards", "MenuItemBig");
                    break;
                case UISnailsMenu.MenuItemSize.Medium:
                    this.Image = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards", "MenuItemMedium");
                    break;
            }

            this.AutoHideMenu = true;
            this.InvokeOnAcceptOnMotionUp = true;
            this.SnailsMenuOwner = snailsMenuOwner;
            if (allowsSelection)
            {
                this.OnAcceptBegin += new UIEvent(UISnailsMenuItem_OnAcceptBegin);
            }
            this._itemSelectedSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MENU_ITEM_SELECTED);
            this.OnFocus += new UIEvent(UISnailsMenuItem_OnFocus);
            this.Initialize();
//            _itemShownSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.MENU_ITEM_SHOWN);
//            _itemShownSample.PlaySameEffectMinTime = 0;

            // Don't use focus sound with touch
            if (!SnailsGame.GameSettings.UseTouch)
            {
                _itemFocusSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MENU_ITEM_FOCUS);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsMenuItem_OnFocus(IUIControl sender)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsMenuItem_OnAcceptBegin(IUIControl sender)
        {
            this._itemSelectedSample.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            this.Reset();
            this.TextScale = new Vector2(1.0f, 1.0f);
            this.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this.InstructionLabelType = UIInstructionLabel.LabelActionTypes.Accept;
            this.ShowEffect = new SquashEffect(0.73f, 4.0f, 0.04f, this.BlendColor, this.Scale);
            this.HideEffect = new PopOutEffect(new Vector2(1.2f, 1.2f), 6.0f, this.BlendColor, this.Scale);
            this.Label.BlendColor = Colors.MenuItem;
            this.Resize();
            this.Scale = UISnailsMenuItem.DEFAULT_SCALE;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InvokeOnSelect()
        {
            if (this.OnSelect != null)
            {
                this.OnSelect(this);
            }
        }

        /// <summary>
        /// This is stupid... Scale and color has to be reset because it maintains the last settings when
        /// it was shown...
        /// </summary>
        public override void Show()
        {
            this.Scale = UISnailsMenuItem.DEFAULT_SCALE;
            this.Label.BlendColor = Colors.MenuItem;
     //       _itemShownSample.Play();
            base.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Reset()
        {
            this.Scale = UISnailsMenuItem.DEFAULT_SCALE;
            this.Label.Effect = null;
            this.Label.BlendColor = Colors.MenuItem;
            this.Label.BlendScaleWithParent = false;
            this.Effect = null;
            this.WithFocus = false;
           
            ((UITextFontLabel)this.Label).Font = this.SnailsMenuOwner.MenuItemsFontUnselected;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Hide()
        {
            // Reset font and scale with parent
            this.Label.BlendScaleWithParent = true;
            ((UITextFontLabel)this.Label).Font = this.SnailsMenuOwner.MenuItemsFont;
            base.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void GotFocus()
        {
            this.Effect = new ScaleEffect(UISnailsMenuItem.DEFAULT_SCALE, 2.0f, new Vector2(SELECTED_ITEM_SCALE, SELECTED_ITEM_SCALE), false);
            this.Label.BlendColor = Colors.MenuItemSelected;

            this.Label.BlendScaleWithParent = true;
            ((UITextFontLabel)this.Label).Font = this.SnailsMenuOwner.MenuItemsFont;
            this.WithFocus = true;
            if (this._itemFocusSample != null)
            {
                this._itemFocusSample.Play();
            }
        }

    }
}
