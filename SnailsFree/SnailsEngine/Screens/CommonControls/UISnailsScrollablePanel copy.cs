using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsScrollablePanel : UIPanel
    {
        #region Vars
        UIPanel _pnlContainer;
        UIScrollablePanel _panel;
        UISlider _slider;
        UIImage _imgPanelTop;
        UIImage _imgPanelBottom;
        UIArrow _arrowBeginning;
        UIArrow _arrowEnd;
        HooverEffect _arrowEffect;
        #endregion 

        private bool WithSlider { get; set; }
        public bool ShowScrollIndicators { get; set; }
        public float Length
        {
            get
            {
                if (this._panel.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
                {
                    return this._panel.Height;
                }

                return this._panel.Width;
            }
            set
            {
                if (this._panel.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
                {
                    this._panel.Height = value;
                }
                else
                {
                    this._panel.Width = value;
                }
                this.Refresh();
            }
        }

        float Distance
        {
            get
            {
                if (this._panel.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
                {
                    return this._panel.Width;
                }

                return this._panel.Height;
            }
            set
            {
                if (this._panel.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
                {
                    this._panel.Width = value;
                }
                else
                {
                    this._panel.Height = value;
                }

            }
        }
        public UIScrollablePanel.PanState State { get { return this._panel.State; } }
        public UIScrollablePanel.PanState PreviousState { get { return this._panel.PreviousState; } }

        public UIScrollablePanel.PanelOrientation Orientation
        {
            get { return this._panel.Orientation; }
            set
            {
                this._panel.Orientation = value;
            }
        }

        public float SheetLength
        {
            set
            {
                this._panel.Size = new Size(value, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UISnailsScrollablePanel(UIScreen screenOwner, UIScrollablePanel.PanelOrientation orientation, bool withSlider, float sheetLength) :
            base(screenOwner)
        {
            // Container
            this._pnlContainer = new UIPanel(screenOwner);
            this._pnlContainer.ParentAlignment = AlignModes.HorizontalyVertically;
            this._pnlContainer.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.03f, Color.White, Vector2.One);
            this._pnlContainer.HideEffect = new PopOutEffect(new Vector2(1.2f, 1.2f), 6.0f);
            this._pnlContainer.BackgroundColor = new Color(0, 0, 0, 100);
            this._pnlContainer.OnHide += new UIEvent(_pnlContainer_OnHide);
            this._pnlContainer.OnShow += new UIEvent(_pnlContainer_OnShow);
            this.Controls.Add(this._pnlContainer);

            // Scrollable Panel
            this._panel = new UIScrollablePanel(screenOwner, orientation);
            this._panel.ClipToParent = true;
            this.SheetLength = sheetLength;
            this._panel.OnScroll += new UIEvent(_panel_OnScroll);
            this._pnlContainer.Controls.Add(this._panel);

            // Slider
            this._slider = new UISlider(screenOwner, null, "spriteset/boards/ScrollManipulator");
            this._slider.Orientation = (orientation == UIScrollablePanel.PanelOrientation.Vertical? SliderOrientation.Vertical : SliderOrientation.Horizontal);
            this._slider.BackgroundColor = new Color(0, 0, 0, 200);
            this._slider.AutoSetSliderSlot = true;
            this._slider.MinValue = 0;
            this._slider.MaxValue = this._panel.Height;
            this._slider.Visible = withSlider;
            this._pnlContainer.Controls.Add(this._slider);
            
            // Panel top
            this._imgPanelTop = new UIImage(screenOwner);
            this._imgPanelTop.Scale = this.FromNativeResolution(Vector2.One);
            this._pnlContainer.Controls.Add(this._imgPanelTop);

            // Panel bottom
            this._imgPanelBottom = new UIImage(screenOwner);
            this._imgPanelBottom.Scale = this.FromNativeResolution(Vector2.One);
            this._pnlContainer.Controls.Add(this._imgPanelBottom);

            // Scroll indicators
            this._arrowBeginning = new UIArrow(this.ScreenOwner, UIArrow.ArrowType.Up, UIArrow.ArrowSize.Small, _arrowEffect);
            this._arrowBeginning.Visible = true;
            this._pnlContainer.Controls.Add(this._arrowBeginning);

            this._arrowEnd = new UIArrow(this.ScreenOwner, UIArrow.ArrowType.Down, UIArrow.ArrowSize.Small, _arrowEffect);
            this._arrowEnd.Visible = true;
            this._pnlContainer.Controls.Add(this._arrowEnd);

            // Control
            if (withSlider)
            {
                this._panel.ConnectSlider(this._slider);
            }

            this.WithSlider = withSlider;
            this.ShowScrollIndicators = true;
            this.OnBeforeControlAdded += new UIControlAddedEvent(UISnailsScrollablePanel_OnBeforeControlAdded);
            this.OnSizeChanged += new UIEvent(UISnailsScrollablePanel_OnSizeChanged);
            this.Refresh();
        }


        
        /// <summary>
        /// 
        /// </summary>
        void _pnlContainer_OnShow(IUIControl sender)
        {
            this.InvokeOnShow();

        }

        /// <summary>
        /// 
        /// </summary>
        void _pnlContainer_OnHide(IUIControl sender)
        {
            this.InvokeOnHide();
            this.Visible = false;
        }
       
        /// <summary>
        /// 
        /// </summary>
        void UISnailsScrollablePanel_OnSizeChanged(IUIControl sender)
        {
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            _arrowEffect.Update(gameTime);
            if (this._arrowBeginning.Visible)
            {
                this._arrowBeginning.DoHoover(_arrowEffect.PositionV2);
            }
            if (this._arrowEnd.Visible)
            {
                this._arrowEnd.DoHoover(_arrowEffect.PositionV2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Refresh()
        {
            this._pnlContainer.Size = this.Size;

            switch (this._panel.Orientation)
            {
                case UIScrollablePanel.PanelOrientation.Horizontal:
                    if (this.WithSlider)
                    {
                        this._slider.Size = new Size(this.Width, 100f); // Check this 100f later...
                        this._slider.Position = new Vector2(0f, this.Height);
                        this._slider.SliderMargin = (this._imgPanelTop.Width / 2);
                        this._slider.MaxValue = this._panel.Width - this.Width;
                    }
                    this._panel.Size = new Size(this._panel.Width, this.Height);     
                    break;

                case UIScrollablePanel.PanelOrientation.Vertical:
                    if (this.WithSlider)
                    {
                        this._slider.Size = new Size(100f, this.Height); // Check this 100f later...
                        this._slider.Position = new Vector2(this.Width, 0f);
                        this._slider.SliderMargin = (this._imgPanelTop.Height / 2);
                        this._slider.MaxValue = this._panel.Height - this.Height;
                    }
                    this._panel.Size = new Size(this.Width, this._panel.Height);
                    break;
            }

            if (this.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
            {
                if (this.Distance > 5000f)
                {
                    this._imgPanelTop.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/main-menu-objects2/LogLong");
                }
                else
                {
                    this._imgPanelTop.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/main-menu-objects2/LogSmall");
                }
            }
            else
            {
                this._imgPanelTop.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/main-menu-objects2/LogSmallV");
            }
            this._imgPanelBottom.Sprite = this._imgPanelTop.Sprite;
           // this._imgPanelTop.Margins.Top = this._imgPanelTop.Height;

            // Update panel top/bottom position, can only be done after sprites have been set

            switch (this._panel.Orientation)
            {
                case UIScrollablePanel.PanelOrientation.Horizontal:
                    this._imgPanelTop.Margins.Top = 0;
                    this._imgPanelTop.Margins.Left = -(this.FromNativeResolutionX(this._imgPanelTop.Width)) / 2f;
                    this._imgPanelTop.ParentAlignment = AlignModes.Vertically | AlignModes.Left;

                    this._imgPanelBottom.Margins.Right = -(this.FromNativeResolutionX(this._imgPanelBottom.Width)) / 2f;
                    this._imgPanelBottom.Margins.Bottom = 0;
                    this._imgPanelBottom.ParentAlignment = AlignModes.Vertically | AlignModes.Right;

                    this._arrowBeginning.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
                    this._arrowBeginning.Margins.Clear();
                    this._arrowBeginning.Margins.Left = this.FromNativeResolutionX(300f);
                    this._arrowBeginning.Orientation = UIArrow.ArrowType.Left;

                    this._arrowEnd.ParentAlignment = AlignModes.Bottom | AlignModes.Right;
                    this._arrowEnd.Margins.Clear();
                    this._arrowEnd.Margins.Right = this.FromNativeResolutionX(300f);
                    this._arrowEnd.Orientation = UIArrow.ArrowType.Right;
                    this._arrowEffect = new HooverEffect(1f, 3f, 90f);
                    break;

                case UIScrollablePanel.PanelOrientation.Vertical:
                    this._imgPanelTop.Margins.Left = 0;
                    this._imgPanelTop.Margins.Top = -(this.FromNativeResolutionY(this._imgPanelTop.Height)) / 2f;
                    this._imgPanelTop.ParentAlignment = AlignModes.Horizontaly | AlignModes.Top;

                    this._imgPanelBottom.Margins.Bottom = -(this.FromNativeResolutionY(this._imgPanelBottom.Height)) / 2f;
                    this._imgPanelBottom.Margins.Right = 0;
                    this._imgPanelBottom.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
                    
                    this._arrowBeginning.ParentAlignment = AlignModes.Right | AlignModes.Top;
                    this._arrowBeginning.Margins.Clear();
                    this._arrowBeginning.Margins.Top = this.FromNativeResolutionY(500f);
                    this._arrowBeginning.Orientation = UIArrow.ArrowType.Up;

                    this._arrowEnd.ParentAlignment = AlignModes.Right | AlignModes.Bottom;
                    this._arrowEnd.Margins.Clear();
                    this._arrowEnd.Margins.Bottom = this.FromNativeResolutionY(500f);
                    this._arrowEnd.Orientation = UIArrow.ArrowType.Down;
                    this._arrowEffect = new HooverEffect(1f, 3f, 0f);
                    break;
            }
            this.RefreshScrollIndicators();
        }

        /// <summary>
        /// 
        /// </summary>
        private bool UISnailsScrollablePanel_OnBeforeControlAdded(IUIControl sender, UIControl control)
        {
            this._panel.Controls.Add(control);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            this._panel.Controls.Clear();
        }

       
        public bool AtLeftBound()
        {
            return this._panel.AtLeftBound();
        }
        public bool AtRightBound()
        {
            return this._panel.AtRightBound();
        }
        public bool AtUpBound()
        {
            return this._panel.AtUpBound();
        }
        public bool AtDownBound()
        {
            return this._panel.AtDownBound();
        }

   

        /// <summary>
        /// 
        /// </summary>
        public override void Hide()
        {
            this._pnlContainer.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Show()
        {
            this.Visible = true;
            this._pnlContainer.Show();
        }

        public void ScrollToTop()
        {
            this._panel.SetScroll(0);
        }


        void _panel_OnScroll(IUIControl sender)
        {
            this.RefreshScrollIndicators();
        }

        void RefreshScrollIndicators()
        {
            this._arrowBeginning.Visible = (!this._panel.AtBeginning() && !this.WithSlider && this.ShowScrollIndicators);
            this._arrowBeginning.DoHoover(_arrowEffect.PositionV2);
            this._arrowEnd.Visible = (!this._panel.AtEnd() && !this.WithSlider && this.ShowScrollIndicators);
            this._arrowEnd.DoHoover(_arrowEffect.PositionV2);
        }

        public void Reset()
        {
            this.ScrollToTop();
        }

        public void StopFlick()
        {
            if (this._panel != null)
            {
                this._panel.StopFlick();
            }
        }
    }
}
