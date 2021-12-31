using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UISlider : UIControl
    {
        public event UIEvent OnValueChanged; 
        public event UIEvent OnValueChangedEnded;

        #region Vars
        UIImage _imgBackground;
        UIImage _imgSlider;
        float _value;
        float _minValue;
        float _maxValue;
        bool _dragStarted;
        SliderOrientation _orientation;
        bool _needSliderUpdate;
        float _sliderLength;
        float _sliderMargin;
        #endregion

       
        #region Properties
        private Vector2 GrabOffset { get; set; } // In pixels because Screen.CursorPosition is also in pixels
        public float SliderMargin 
        {
            get { return this._sliderMargin; }
            set
            {
                this._sliderMargin = value;
                this._needSliderUpdate = true;
            }
        } 

        private bool DragStarted 
        { 
            get
            {
                return this._dragStarted;
            }
            set
            {
                this._dragStarted = value;
                if (this._dragStarted)
                {
                    this.ScreenOwner.CaptureCursor(this);
                }
                else
                {
                    this.ScreenOwner.ReleaseCursor();
                }
            }
        }

        public Vector2 SliderStartPosition { get; set; }
        public float SliderLength
        {
            get { return this._sliderLength; }
            set
            {
                this._sliderLength = value;
                this._needSliderUpdate = true;
            }
        }

        public SliderOrientation Orientation 
        {
            get { return this._orientation; }
            set
            { 
                this._orientation = value;
                this._needSliderUpdate = true;
            }
        }
        public float Step { get; set; }
        public UIImage Image { get { return this._imgBackground; } }
        public bool AutoSetSliderSlot { get; set; }

        public float Value
        {
            get { return this._value; }
            set
            {
                if (this._value != value)
                {
                    this._value = value;
                    if (this._value < this.MinValue)
                    {
                        this._value = this.MinValue;
                    }
                    if (this._value > this.MaxValue)
                    {
                        this._value = this.MaxValue;
                    }
                    this.SetSliderPosition();
                    if (this.OnValueChanged != null)
                    {
                        this.OnValueChanged(this);
                    }
                }
            }
        }

        public float MinValue
        {
            get { return this._minValue; }
            set
            {
                this._minValue = value;
            }
        }


        public float MaxValue
        {
            get { return this._maxValue; }
            set
            {
                this._maxValue = value;
            }
        }

        public new bool DropShadow
        {
            get { return base.DropShadow; }
            set
            {
                base.DropShadow = value;
                this._imgSlider.DropShadow = value;
                this._imgBackground.DropShadow = value;
            }
        }

        public new Vector2 ShadowDistance
        {
            get { return base.ShadowDistance; }
            set
            {
                base.ShadowDistance = value;
                this._imgSlider.ShadowDistance = value;
                this._imgBackground.ShadowDistance = value;
            }        
        }

        public new Color ShadowColor
        {
            get { return base.ShadowColor; }
            set
            {
                base.ShadowColor = value;
                this._imgSlider.ShadowColor = value;
                this._imgBackground.ShadowColor = value;
            }
        }


        public override Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                this._needSliderUpdate = true;
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        public UISlider(UIScreen screenOwner, string backgroundImgResourceName, string sliderImgResourceName) :
            base(screenOwner)
        {

            // Background
            if (backgroundImgResourceName != null)
            {
                this._imgBackground = new UIImage(screenOwner, backgroundImgResourceName);
                this.Controls.Add(this._imgBackground);
            }

            this.CursorStop = false;
            this.OnFocus += new UIEvent(UISlider_OnFocus);

            // Slider
            this._imgSlider = new UIImage(screenOwner, sliderImgResourceName);
            this._imgSlider.OnMotionPointerDown += new UIEvent(_imgSlider_OnMotionPointerDown);
            this._imgSlider.OnMotionPointerUp += new UIEvent(_imgSlider_OnMotionPointerUp);
            this._imgSlider.AcceptControllerInput = true;
            this._imgSlider.Name = "_Slider";
            this._imgSlider.OnFocus += new UIEvent(_imgSlider_OnFocus);
            this.Controls.Add(this._imgSlider);

            // Component properties
            this.MinValue = 0.0f;
            this.MaxValue = 100.0f;
            this.Step = 5.0f;
            if (this._imgBackground != null)
            {
                this.Size = this._imgBackground.Size;
            }
            this.Orientation = SliderOrientation.Horizontal;
            this.Value = 0.0f;

            this.CursorStop = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void UISlider_OnFocus(IUIControl sender)
        {
            this._imgSlider.Focus();    
        }

        /// <summary>
        /// 
        /// </summary>
        void _imgSlider_OnFocus(IUIControl sender)
        {
            if (this.Parent is UIControl)
            {
                ((UIControl)this.Parent).InvokeOnFocus();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void SetSliderPosition()
        {
           
            float pos = 0.0f;
            if (this.MaxValue != 0)
            {
                pos = this.SliderLength * this.Value / this.MaxValue;
            }

            switch (this.Orientation)
            {
                case SliderOrientation.Horizontal:
                    this._imgSlider.Position = this.SliderStartPosition + new Vector2(pos, 0f);
                    break;

                case SliderOrientation.Vertical:
                    this._imgSlider.Position = this.SliderStartPosition + new Vector2(0f, pos);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _imgSlider_OnMotionPointerDown(IUIControl sender)
        {
            this.DragStarted = true;
            this.Focus();
            this.GrabOffset = this.PixelsToScreenUnits(BrainGame.GameCursor.Position - this._imgSlider.AbsolutePositionInPixels);
        }

        /// <summary>
        /// 
        /// </summary>
        void _imgSlider_OnMotionPointerUp(IUIControl sender)
        {
            this.DragStarted = false;
            if (this.OnValueChangedEnded != null)
            {
                this.OnValueChangedEnded(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this._needSliderUpdate && this.AutoSetSliderSlot)
            {
                this.SetSliderToControlSize(); // Optimization possible. No need to do this in all updates...
                this.SetSliderPosition();
                this._needSliderUpdate = false;
            }

            if (this.DragStarted)
            {
                float pos;
                switch (this.Orientation)
                {
                    case SliderOrientation.Horizontal:
                        pos = this.PixelsToScreenUnits(BrainGame.GameCursor.Position).X - this.AbsolutePosition.X - this.GrabOffset.X;
                        if (pos < this.SliderStartPosition.X - this.PixelsToScreenUnitsX(this._imgSlider.Sprite.Width))
                        {
                            pos = this.SliderStartPosition.X - this.PixelsToScreenUnitsX(this._imgSlider.Sprite.Width);
                        }
                        if (pos > this.SliderStartPosition. X + this.SliderLength)
                        {
                            pos = this.SliderStartPosition.X + this.SliderLength;
                        }
                        pos += this.PixelsToScreenUnitsX(this._imgSlider.Sprite.Offset.X);
                        this._imgSlider.Position = new Vector2(pos, this._imgSlider.Position.Y) ;
                        this.Value = this.MaxValue * (this._imgSlider.Position.X - this.SliderStartPosition.X) /  this.SliderLength;
                        break;

                    case SliderOrientation.Vertical:
                        pos = this.PixelsToScreenUnits(BrainGame.GameCursor.Position).Y - this.AbsolutePosition.Y - this.GrabOffset.Y;
                        if (pos < this.SliderStartPosition.Y - this.PixelsToScreenUnitsY(this._imgSlider.Sprite.Height))
                        {
                            pos = this.SliderStartPosition.Y - this.PixelsToScreenUnitsY(this._imgSlider.Sprite.Height);
                        }
                        if (pos > this.SliderStartPosition.Y + this.SliderLength)
                        {
                            pos = this.SliderStartPosition.Y + this.SliderLength;
                        }
                        pos += this.PixelsToScreenUnitsY(this._imgSlider.Sprite.Offset.Y);
                        this._imgSlider.Position = new Vector2(this._imgSlider.Position.X, pos);
                        this.Value = this.MaxValue * (this._imgSlider.Position.Y - this.SliderStartPosition.Y) / this.SliderLength;
                        break;
                }
            }

            if (this.CursorInside && this.ScreenOwner.CursorMode == CursorModes.SnapToControl)
            {
                if (this.ScreenOwner.InputController.ActionLeft)
                {
                    this.Value -= (float)(this.Step * (float)gameTime.ElapsedGameTime.TotalMilliseconds / (float)100.0f);
              //      BrainGame.GameCursor.Position = this._imgSlider.AbsolutePositionInPixels + this._imgSlider.Sprite.Offset;
                }
                if (this.ScreenOwner.InputController.ActionRight)
                {
                    this.Value += (float)(this.Step * (float)gameTime.ElapsedGameTime.TotalMilliseconds / (float)100.0f);
             //       BrainGame.GameCursor.Position = this._imgSlider.AbsolutePositionInPixels + this._imgSlider.Sprite.Offset;
                }

                if (this.ScreenOwner.InputController.ActionLeftReleased || 
                    this.ScreenOwner.InputController.ActionRightReleased)
                {
                    if (this.OnValueChangedEnded != null)
                    {
                        this.OnValueChangedEnded(this);
                    }
                }
            }

            if (this.ScreenOwner.CursorMode == CursorModes.SnapToControl && this._imgSlider.HasFocus)
            {
                BrainGame.GameCursor.Position = this._imgSlider.AbsolutePositionInPixels + this._imgSlider.Sprite.Offset;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetSliderToControlSize()
        {
            switch (this.Orientation)
            {
                case SliderOrientation.Horizontal:
                    this.SliderLength = this.Size.Width - (this.SliderMargin * 2);
                    this.SliderStartPosition = new Vector2(this.SliderMargin, this.Size.Height / 2);
                    break;

                case SliderOrientation.Vertical:
                    this.SliderLength = this.Size.Height - (this.SliderMargin * 2);
                    this.SliderStartPosition = new Vector2(this.Size.Width / 2, this.SliderMargin);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetSliderSlotToSpriteBB(bool centerInSlot)
        {
            if (this._imgBackground != null && this._imgBackground.Sprite != null)
            {
                BoundingSquare bs = this.PixelsToScreenUnits(this._imgBackground.Sprite.BoundingBox);
                this.SliderStartPosition = bs.UpperLeft;
                if (centerInSlot)
                {
                    switch(this.Orientation)
                    {
                        case SliderOrientation.Horizontal:
                            this.SliderStartPosition = new Vector2(bs.UpperLeft.X, bs.UpperLeft.Y + (bs.Height / 2));
                            break;
                        case SliderOrientation.Vertical:
                            this.SliderStartPosition = new Vector2(bs.UpperLeft.X + (bs.Width / 2), bs.UpperLeft.Y);
                            break;
                    }
                }
                this.SliderLength = bs.Width;
                this.SetSliderPosition();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Hide()
        {
            base.Hide();
        }
    }
}
