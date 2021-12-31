using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Screens.Transitions;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    public class UILocker : UIImage
    {
        public enum LockerImageType
        {
            Normal,
            Small,
            Demo
        }
        private bool _locked;
        private LockerImageType _lockerType;

        public LockerImageType LockerType
        {
            get
            {
                return this._lockerType; 
            }
            set
            {
                this._lockerType = value;
                switch (this._lockerType)
                {
                    case UILocker.LockerImageType.Normal:
                        this._closeSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "Locker");
                       break;
                    case UILocker.LockerImageType.Demo:
                       this._closeSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/menu-elements-1", "LockedInDemo");
                       break;
                }
                this.UpdateSprite();
            }
        }

        public bool Locked
        {
            get { return this._locked; }
            set
            {
                this._locked = value;
                this.UpdateSprite();
            }

        }

        private Sprite _openSprite;
        private Sprite _closeSprite;

        /// <summary>
        /// 
        /// </summary>
        public UILocker(UIScreen screenOwner) :
            base(screenOwner)
        {
            this._openSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "LockerOpen");
            this._closeSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "Locker");
            this.Locked = true;
            // Theme locker
            this.BlendColorWithParent = false;
            this.Effect = new ScaleEffect(new Vector2(1f, 1f), 0.1f, new Vector2(0.95f, 0.95f), true);
        }


        /// <summary>
        /// 
        /// </summary>
        private void UpdateSprite()
        {
            if (this._locked)
            {
                this.Sprite = this._closeSprite;
            }
            else
            {
                this.Sprite = this._openSprite;
            }
        }
    }
}
