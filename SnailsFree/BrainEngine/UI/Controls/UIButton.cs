using System;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UIButton : UIControl
    {
        #region Events
        public event UIEvent OnPress;   // Occurs when the button is pressed
                                        // The button is pressed when 1 of this events happen:
                                        // -The OnAction event happens (the user presses the Action button when the cursor is over the button
                                        // -The button OnControllerAction is raised
        public event UIEvent OnBeforePress;
        public event UIEvent OnDoublePress; // Occurs when the button is double pressed
        public event UIEvent OnBeforeDoublePress;
        #endregion
        private Sprite _enabledSprite;
        private Sprite _disabledSprite;
        public ImageSizeMode _sizeMode;

        public new Vector2 Scale
        {
            get { return base.Scale; }
            set 
            {
                base.Scale = value; 
            }
        }

        #region Properties
        public ImageSizeMode SizeMode 
        {
            get { return this._sizeMode; }
            set
            {
                this._sizeMode = value;
                this.ComputeSize();
            }
        }
        public UIImage Image { get; set;}
        private UITextFontLabel _lblCaption { get; set; }
        public TransformEffectBase PressEffect { get; set; } // Effect to be used when the button is pressed

        public Sprite Sprite 
        {
            get { return this._enabledSprite; }
            set 
            { 
                this._enabledSprite = value;
                if (this.Enabled)
                {
                    this.Image.Sprite = this._enabledSprite;
                }
                this.ComputeSize();
            }
        }

        public Sprite DisabledSprite 
        {
            get
            {
                if (this._disabledSprite == null)
                {
                    return this._enabledSprite;
                }
                return this._disabledSprite; 
            }
            set
            { 
                this._disabledSprite = value;
                if (!this.Enabled)
                {
                    this.Image.Sprite = this._disabledSprite;
                    this.ComputeSize();
                }
            }
        }


        public new int ControllerActionCode 
        {
            get { return base.ControllerActionCode; }
            set 
            { 
                base.ControllerActionCode = value;
                this.Image.ControllerActionCode = value;
            }
        }

        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (this.Enabled == true)
                {
                    this.Image.Sprite = this.Sprite;
                }
                else
                {
                    this.Image.Sprite = this.DisabledSprite;
                }
            }
        }


        public new ITransformEffect Effect
        {
            set
            {
                this.Image.Effect = value;
            }
            get
            {
                return this.Image.Effect;
            }
        }

        public string ImageResource
        {
            set
            {
                this.Sprite = BrainGame.ResourceManager.GetSpriteStatic(value);
            }
        }

        public Sample PressSound { get; set; }

        public int CurrentFrame
        {
            get { return this.Image.CurrentFrame; }
            set { this.Image.CurrentFrame = value; }
        }

        public int FrameCount
        {
            get { return this.Image.FrameCount; }
        }

        public bool AnimateImage
        {
            get { return this.Image.Animate; }
            set { this.Image.Animate = value; }
        }

        public Vector2 ImagePosition
        {
            get { return this.Image.Position; }
            set { this.Image.Position = value; }
        }
        #endregion

        public UIButton(UIScreen screenOwner) :
            this(screenOwner, null)
        {
        }

        public UIButton(UIScreen screenOwner, string spriteResource) :
            base(screenOwner)
        {
            this.Image = new UIImage(screenOwner, spriteResource);
            this.Image.UseHotSpot = true;
            this.Controls.Add(this.Image);
            this.OnAccept += new UIEvent(this.Image_OnAccept);
            this.OnAcceptBegin += new UIEvent(UIButton_OnAcceptBegin);
            this.OnControllerAction += new UIEvent(this.Image_OnAccept);
            this.OnDoubleTapAccept += new UIEvent(UIButton_OnDoubleTapAccept);
            this.OnDoubleTapAcceptBegin += new UIEvent(UIButton_OnDoubleTapAcceptBegin);
            this.SizeMode = ImageSizeMode.Autosize;
        }

        void UIButton_OnAcceptBegin(IUIControl sender)
        {
            if (this.OnBeforePress != null)
            {
                this.OnBeforePress(this);
            }
        }

        void UIButton_OnDoubleTapAcceptBegin(IUIControl sender)
        {
            if (this.OnBeforeDoublePress != null)
            {
                this.OnBeforeDoublePress(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeSize()
        {
            if (this.Image.Sprite != null)
            {
                switch (this.SizeMode)
                {
                    case ImageSizeMode.Autosize:
                        this.Size = this.Image.Size;
                        break;
                    case ImageSizeMode.Center:
                        this.Image.ParentAlignment = AlignModes.HorizontalyVertically;
                        break;
                    case ImageSizeMode.HorizontalCenter:
                        this.Image.ParentAlignment = AlignModes.Horizontaly;
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Image_OnAccept(IUIControl sender)
        {
            
            if (this.PressEffect == null)   // OnPress event is only launched if there's no press effect
                                            // If there's a press effect the event is launched when the effect ends
            {
                if (PressSound != null)
                {
                    this.PressSound.Play();
                }
                if (this.OnPress != null)
                {
                    this.OnPress(this);
                }
            }
            else
            {
                if (PressSound != null)
                {
                    this.PressSound.Play();
                }
                this.Busy = true;
                this.PressEffect.Reset();
                this.PressEffect.OnEnd = this.PressEffect_OnEnd;
                this.EffectsBlender.Add(this.PressEffect);
            }
        }

        /// <summary>
        /// This is for double tap accept
        /// </summary>
        private void UIButton_OnDoubleTapAccept(IUIControl sender)
        {

            if (this.PressEffect == null)   // OnPress event is only launched if there's no press effect
            // If there's a press effect the event is launched when the effect ends
            {
                if (PressSound != null)
                {
                    this.PressSound.Play();
                }
                if (this.OnDoublePress != null)
                {
                    this.OnDoublePress(this);
                }
            }
            else
            {
                if (PressSound != null)
                {
                    this.PressSound.Play();
                }
                this.Busy = true;
                this.PressEffect.Reset();
                this.PressEffect.OnEnd = this.PressEffect_OnEnd;
                this.EffectsBlender.Add(this.PressEffect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PressEffect_OnEnd(object param)
        {
            if (this.OnPress != null)
            {
                this.OnPress(this);
            }
            this.Busy = false;
        }

        public override void Update(BrainGameTime gameTime)
        {
           
            base.Update(gameTime);
            this.ComputeSize();
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetText(string text, string fontResourceName)
        {
            TextFont font = BrainGame.ResourceManager.Load<TextFont>(fontResourceName, ResourceManager.ResourceManagerCacheType.Static);
            if (this._lblCaption == null)
            {
                this._lblCaption = new UITextFontLabel(this.ScreenOwner);
                this._lblCaption.ParentAlignment = AlignModes.HorizontalyVertically;
                this.Controls.Add(this._lblCaption);
            }
            this._lblCaption.Font = font;
            this._lblCaption.Text = text;
        }

    }
}
