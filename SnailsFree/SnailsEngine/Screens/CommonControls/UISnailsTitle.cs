using System;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsTitle : UIControl
    {
        public enum TitleMode
        {
            Leaf,
            Log
        }

        UIImage _imgLeaf;
        UITimer _shakeTimer;
        Vector2 _originalScale;

        public TitleMode _mode;
        public TitleMode Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
                switch(this._mode)
                {
                    case TitleMode.Leaf:
                        this._imgLeaf.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/SnailsLeafTitle");
                        break;
                    case TitleMode.Log:
                        this._imgLeaf.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/SnailsLogTitle");
                        break;

                }
            }
        }

        public bool WithShake { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISnailsTitle(UIScreen ScreenOwner) :
            base(ScreenOwner)
        {
            this.OnAfterInitializeFromContent += new UIEvent(UISnailsTitle_OnAfterInitializeFromContent);
            this.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this.HideEffect = new PopOutEffect(new Vector2(1.2f, 1.2f), 6.0f);

            // Title
            this._imgLeaf = new UIImage(ScreenOwner);
            this._imgLeaf.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._imgLeaf.DropShadow = false;
            this.Controls.Add(this._imgLeaf);

            this.Mode = TitleMode.Leaf;
            this.Size = this._imgLeaf.Size;
            this.AcceptControllerInput = false;

            this._shakeTimer = new UITimer(ScreenOwner);
            this._shakeTimer.Snooze = true;
            this._shakeTimer.OnTimer += new UIEvent(_shakeTimer_OnTimer);
            this.Controls.Add(this._shakeTimer);

            this.OnScreenStart += new UIEvent(UISnailsTitle_OnScreenStart);
            this.WithShake = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void _shakeTimer_OnTimer(IUIControl sender)
        {
            this.Effect = new SquashEffect(0.85f * this._originalScale.X, 1.2f * this._originalScale.X, 0.02f * this._originalScale.X, this.BlendColor, this._originalScale);
            this._shakeTimer.Time = 5000 + BrainGame.Rand.Next(5000);
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsTitle_OnScreenStart(IUIControl sender)
        {
            this.Scale = this._originalScale;
            this._shakeTimer.Time = 5000 + BrainGame.Rand.Next(5000);
            this._shakeTimer.Reset();
            this._shakeTimer.Enabled = this.WithShake;
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsTitle_OnAfterInitializeFromContent(IUIControl sender)
        {
            this._originalScale = this.Scale;
            // Don't remove this! This is because if the object is initialized from the content, this.Scale may have changed
            // Multiply everything by scale, or else the effect will change if scale changes
            this.ShowEffect = new SquashEffect(0.8f * this.Scale.X, 4.0f * this.Scale.X, 0.03f * this.Scale.X, this.BlendColor, this.Scale);
        }


        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}