using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UIScrollablePanel : UIPanel
    {
        public event UIEvent OnScroll;

        public enum PanelOrientation
        {
            Horizontal,
            Vertical
        }

        /*public enum PanState
        {
            None,
            DragStart,
            Dragging,
            Flick,
            Flicking
        }*/

        public enum PanState
        {
            None,
            DragStart,
            Dragging,
            DragEnd,
            Flicking
        }

        public enum BoundType
        {
            Up,
            Down,
            Left,
            Right
        }


        protected Vector2 PanStartPosition;
        protected Vector2 PreviousPosition;
        protected Vector2 LastOffset;
        public PanState State;
        public PanState PreviousState;
        public PanelOrientation Orientation;
        public Rectangle PageRect;
        public Rectangle ClipRect;
        public float PanDistance;
        public double PanEllapsedTime;
        private FlickEffect _flickEffect;
        public UISlider _connectedSlider;

        public Rectangle PageRectInUnits
        {
            get
            {
                return new Rectangle((int)this.ScreenOwner.PixelsToScreenUnitsX(this.PageRect.X),
                                     (int)this.ScreenOwner.PixelsToScreenUnitsY(this.PageRect.Y),
                                     (int)this.ScreenOwner.PixelsToScreenUnitsX(this.PageRect.Width),
                                     (int)this.ScreenOwner.PixelsToScreenUnitsY(this.PageRect.Height));
            }

            set
            {
                this.PageRect = new Rectangle((int)this.ScreenUnitToPixelsX(value.X),
                                              (int)this.ScreenUnitToPixelsY(value.Y),
                                              (int)this.ScreenUnitToPixelsX(value.Width),
                                              (int)this.ScreenUnitToPixelsY(value.Height));
            }
        }

        public Rectangle ClipRectInUnits
        {
            get
            {
                return new Rectangle((int)this.ScreenOwner.PixelsToScreenUnitsX(this.ClipRect.X),
                                     (int)this.ScreenOwner.PixelsToScreenUnitsY(this.ClipRect.Y),
                                     (int)this.ScreenOwner.PixelsToScreenUnitsX(this.ClipRect.Width),
                                     (int)this.ScreenOwner.PixelsToScreenUnitsY(this.ClipRect.Height));
            }

            set
            {
                this.ClipRect = new Rectangle((int)this.ScreenUnitToPixelsX(value.X),
                                              (int)this.ScreenUnitToPixelsY(value.Y),
                                              (int)this.ScreenUnitToPixelsX(value.Width),
                                              (int)this.ScreenUnitToPixelsY(value.Height));
            }
        }

        public bool ClipToParent { get; set; }
        public float BottomMargin { get; set; }


        public UIScrollablePanel(UIScreen screenOwner, PanelOrientation orientation) :
            this(screenOwner, Vector2.Zero, orientation)
        {
        }

        public UIScrollablePanel(UIScreen screenOwner, Vector2 position, PanelOrientation orientation) :
            base(screenOwner)
        {

            this.Position = position;
            this.Orientation = orientation;


            // default clipping and page area
            this.PageRect = new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight);
            this.ClipRect = new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight);


            this._flickEffect = new FlickEffect(Vector2.Zero);
            this._flickEffect.Active = false;
            this._flickEffect.AutoDeleteOnEnd = false;
            this.EffectsBlender.Add(this._flickEffect);
            this.OnScreenStart += new UIEvent(UIScrollablePanel_OnScreenStart);
        }


        /// <summary>
        /// 
        /// </summary>
        void UIScrollablePanel_OnScreenStart(IUIControl sender)
        {
            this.StopFlick();
        }

        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            // Update Clipping and page rects, may be optimized, because this will only change if parent changes...
            if (this.ClipToParent)
            {
                this.ClipRect = this.Parent.BoundingBox.ToRect();
			    this.ClipRect = new Rectangle(this.ClipRect.X, this.ClipRect.Y, this.ClipRect.Width, this.ClipRect.Height - (int)this.ScreenUnitToPixelsY(BottomMargin));
                if (this.ClipRect.X + this.ClipRect.Width > BrainGame.ScreenWidth)
                {
                    this.ClipRect = new Rectangle(this.ClipRect.X, this.ClipRect.Y,
                                                  BrainGame.ScreenWidth - this.ClipRect.X, this.ClipRect.Height);
                }
                if (this.ClipRect.Y + this.ClipRect.Height > BrainGame.ScreenHeight)
                {
                    this.ClipRect = new Rectangle(this.ClipRect.X, this.ClipRect.Y,
                                                    this.ClipRect.Width,
                                                    BrainGame.ScreenHeight - this.ClipRect.Y);
                }
			    this.PageRect = this.ClipRect;
            }

            this.PreviousState = this.State;

            if ((this.State == PanState.Flicking && !this._flickEffect.Active) ||
                (this.State != PanState.Flicking && this.ScreenOwner.InputController.ActionNone))
            {
                this.State = PanState.None;
            }


            if (this.ClipToParent && this.Parent.CheckCursorInside() ||
                !this.ClipToParent)
            {
                if (this.Orientation == PanelOrientation.Horizontal && this.ScreenOwner.InputController.ActionHorizontalDrag ||
                    this.Orientation == PanelOrientation.Vertical && this.ScreenOwner.InputController.ActionVerticalDrag)
                {
                    if (this.State == PanState.None ||
                        this.State == PanState.Flicking)
                    {
                        this.State = PanState.DragStart;
                    }
                }
            }

            if (this.State != PanState.None &&
                this.State != PanState.Flicking &&
                this.ScreenOwner.InputController.ActionDragComplete)
            {
                this.State = PanState.DragEnd;
            }

            if (this.State == PanState.DragStart)
            {
                this.PanEllapsedTime = 0;
                this.PanDistance = 0;
                this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
                this.PreviousPosition = Position;
                this.State = PanState.Dragging;
            }
            if (this.State == PanState.Dragging)
            {
                this.PanEllapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
                this.LastOffset = (this.ScreenOwner.InputController.MotionPosition - this.PanStartPosition);

                if (this.LastOffset != Vector2.Zero)
                {
                    this.PanDistance += this.LastOffset.Length();

                    if (this.Orientation == PanelOrientation.Horizontal)
                    {
                        float x = this.PositionInPixels.X + this.LastOffset.X;
                        // check bounds
                        if (x > 0)
                        {
                            x = 0;
                            this.StopFlick();
                        }
                        float xMaxLimit = Math.Abs(this.SizeInPixels.Width - this.Parent.SizeInPixels.Width) * -1;
                        if (x < xMaxLimit)
                        {
                            x = xMaxLimit;
                            this.StopFlick();
                        }

                        this.PositionInPixels = new Vector2(x, this.PositionInPixels.Y);
                        this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
                        base.UpdateLayout();
                        this.InvokeOnScroll();
                    }

                    if (this.Orientation == PanelOrientation.Vertical &&
                         (
                          (this.LastOffset.Y < 0 && !this.AtEnd()) ||
                          (this.LastOffset.Y > 0 && !this.AtBeginning())
                         )
                       )
                    {
                        float y = this.PositionInPixels.Y + this.LastOffset.Y;
                        // check bounds
                        if (y > 0)
                        {
                            y = 0;
                            this.StopFlick();
                        }
                        float yMaxLimit = Math.Abs(this.SizeInPixels.Height - (this.PageRect.Height)) * -1;

                        if (y < yMaxLimit)
                        {
                            y = yMaxLimit;
                            this.StopFlick();
                        }

                        this.PositionInPixels = new Vector2(this.PositionInPixels.X, y);
                        this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
                        base.UpdateLayout();
                        this.InvokeOnScroll();
                    }
                }
            }
            if (this.State == PanState.DragEnd)
            {
                if (this.PanDistance > 20f &&
                     (
                      (PanDistance < 0 && !this.AtEnd()) ||
                      (PanDistance > 0 && !this.AtBeginning())
                     )
                   )
                {
                    this.State = PanState.Flicking;

                    float speed = this.PanDistance / (float)this.PanEllapsedTime;
                    speed = this.LastOffset.Length() / (float)gameTime.ElapsedRealTime.TotalMilliseconds;
                    Vector2 panVector = this.LastOffset;
                    if (panVector != Vector2.Zero)
                    {
                        panVector.Normalize();
                        panVector *= speed;
                        panVector *= 1000f;

                        if (this.Orientation == PanelOrientation.Horizontal)
                        {
                            panVector = new Vector2(panVector.X, 0);
                        }
                        if (this.Orientation == PanelOrientation.Vertical)
                        {
                            panVector = new Vector2(0, panVector.Y);
                        }

                        this._flickEffect.Reset(panVector);
                        this._flickEffect.Active = true;
                    }
                }
                else
                {
                    this.State = PanState.None;
                }
            }

            if (this.State == PanState.Flicking)
            {
                if (this.Orientation == PanelOrientation.Horizontal)
                {
                    float x = this.PositionInPixels.X;
                    float xMaxLimit = Math.Abs(this.SizeInPixels.Width - this.Parent.SizeInPixels.Width) * -1;

                    // check bounds
                    if (x > 0)
                    {
                        x = 0;
                        this.State = PanState.None;
                    }
                    if (x < xMaxLimit)
                    {
                        x = xMaxLimit;
                        this.State = PanState.None;
                    }
                    if (this.State == PanState.None)
                    {
                        this.StopFlick();
                        this.PositionInPixels = new Vector2(x, this.PositionInPixels.Y);
                        this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
                        base.UpdateLayout();
                    }
                    this.InvokeOnScroll();
                }
                if (this.Orientation == PanelOrientation.Vertical && !this.AtEnd())
                {
                    float y = this.PositionInPixels.Y;
                    float yMaxLimit = Math.Abs(this.SizeInPixels.Height - (this.PageRect.Height)) * -1;

                    if (y > 0)
                    {
                        y = 0;
                        this.State = PanState.None;
                    }
                    if (y < yMaxLimit)
                    {
                        y = yMaxLimit;
                        this.State = PanState.None;
                    }

                    if (this.State == PanState.None)
                    {
                        this.StopFlick();
                        this.PositionInPixels = new Vector2(this.PositionInPixels.X, y);
                        this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
                        base.UpdateLayout();
                    }
                    this.InvokeOnScroll();
                }
            }

            // Check limits
            if (this.Orientation == PanelOrientation.Horizontal)
            {
                if (this.Position.X > 0)
                {
                    this.Position = new Vector2(0f, this.Position.Y);
                    this.StopFlick();
                    this.InvokeOnScroll();
                }
                if (Math.Abs(this.Position.X) > Math.Abs(this.Width - this.Parent.Size.Width))
                {
                    this.Position = new Vector2(this.Width - this.Parent.Size.Width, 0f);
                    this.StopFlick();
                    this.InvokeOnScroll();
                }
            }
            else
            {
                if (this.Position.Y > 0)
                {
                    this.Position = new Vector2(this.Position.X, 0f);
                    this.StopFlick();
                    this.InvokeOnScroll();
                }

                if (Math.Abs(this.Position.Y) > Math.Abs(this.Height - this.Parent.Size.Height))
                {
                    this.Position = new Vector2(0f, this.Parent.Size.Height - this.Height);
                    this.StopFlick();
                    this.InvokeOnScroll();
                }
            }


            if (this._connectedSlider != null)
            {
                this._connectedSlider.Value = -this.Position.Y;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public void StopFlick()
        {
            this._flickEffect.Active = false;
            this.State = PanState.None;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool AtBeginning()
        {
            if (this.Orientation == PanelOrientation.Horizontal)
            {
                return (this.Position.X == 0);
            }
            return (this.Position.Y == 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AtEnd()
        {
            if (this.Orientation == PanelOrientation.Horizontal)
            {
                if (this.Width - this.Parent.Size.Width < 0)
                {
                    return true;
                }

                return (Math.Abs(this.Position.X) + 0.5f >= Math.Abs(this.Width - this.Parent.Size.Width));
            }

            if (this.Height - this.Parent.Size.Height < 0)
            {
                return true;
            }
            return (Math.Abs(this.Position.Y)  + 0.5f >= Math.Abs(this.Height - this.Parent.Size.Height));
        }

        protected bool AtBound(BoundType bound)
        {
            float threshold = 0.01f;
            bool atBound = false;

            float x = Math.Abs(this.PositionInPixels.X);
            float y = Math.Abs(this.PositionInPixels.Y);
            float xMaxLimit = Math.Abs(this.SizeInPixels.Width - this.ScreenOwner.SizeInPixels.Width);
            float yMaxLimit = Math.Abs(this.SizeInPixels.Height - (this.PageRect.Height));

            if (Orientation == PanelOrientation.Horizontal)
            {
                switch (bound)
                {
                    case BoundType.Left:
                        atBound = (x < threshold);
                        break;
                    case BoundType.Right:
                        atBound = (x > xMaxLimit - threshold);
                        break;
                }
            }
            else if (Orientation == PanelOrientation.Vertical)
            {
                switch (bound)
                {
                    case BoundType.Up:
                        atBound = (y < threshold);
                        break;
                    case BoundType.Down:
                        atBound = (y > yMaxLimit - threshold);
                        break;
                }
            }

            return atBound;
        }

        public bool AtLeftBound()
        {
            return AtBound(BoundType.Left);
        }
        public bool AtRightBound()
        {
            return AtBound(BoundType.Right);
        }
        public bool AtUpBound()
        {
            return AtBound(BoundType.Up);
        }
        public bool AtDownBound()
        {
            return AtBound(BoundType.Down);
        }



        public override void BeginDraw()
        {
            base.BeginDraw();

            if (this.ClipToParent == false)
            {
                return;
            }

            // Clipping draw area

		    this.ScreenOwner.SuspendDraw();

            RasterizerState rastState = new RasterizerState();
            rastState.ScissorTestEnable = true;

			Vector2 v1;
			Vector2 v2 = new Vector2 (this.ClipRect.Width, this.ClipRect.Height);

			v1.X = BrainGame.GraphicsManager.GraphicsDevice.Viewport.Width * this.ClipRect.X / BrainGame.PresentationNativeScreenWidth;
			v1.Y = BrainGame.GraphicsManager.GraphicsDevice.Viewport.Height * this.ClipRect.Y / BrainGame.PresentationNativeScreenHeight;
			v2.X = BrainGame.GraphicsManager.GraphicsDevice.Viewport.Width * this.ClipRect.Width / BrainGame.PresentationNativeScreenWidth;
			v2.Y = BrainGame.GraphicsManager.GraphicsDevice.Viewport.Height * this.ClipRect.Height / BrainGame.PresentationNativeScreenHeight;
			BrainGame.GraphicsManager.GraphicsDevice.ScissorRectangle = new Rectangle((int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y);

			this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, rastState, BrainGame.RenderEffect);

        }

	    /// <summary>
        /// 
        /// </summary>
        public override void EndDraw()
        {
            base.EndDraw();

            if (this.ClipToParent == false)
            {
                return;
            }
		    this.SpriteBatch.End();
			this.ScreenOwner.ResumeDraw();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetScroll(float value)
        {
            switch (this.Orientation)
            {
                case PanelOrientation.Horizontal:
                    this.Position = new Vector2(-value, this.Position.Y);
                    if (this._connectedSlider != null)
                    {
                        this._connectedSlider.Value = -this.Position.X;
                    }
                    break;

                case PanelOrientation.Vertical:
                    this.Position = new Vector2(this.Position.X, -value);
                    if (this._connectedSlider != null)
                    {
                        this._connectedSlider.Value = -this.Position.Y;
                    }
                    break;
            }
            this.InvokeOnScroll();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ConnectSlider(UISlider slider)
        {
            if (this._connectedSlider != null)
            {
                this._connectedSlider.OnValueChanged -= _connectedSlider_OnValueChanged;
            }

            this._connectedSlider = slider;
            this._connectedSlider.OnValueChanged += new UIEvent(_connectedSlider_OnValueChanged);
            this._connectedSlider.MinValue = 0;
            this._connectedSlider.MaxValue = this.Height - this.Parent.Size.Height;
        }

        /// <summary>
        /// 
        /// </summary>
        void _connectedSlider_OnValueChanged(IUIControl sender)
        {
            this.SetScroll(this._connectedSlider.Value);
        }

        void InvokeOnScroll()
        {
            if (this.OnScroll != null)
            {
                this.OnScroll(this);
            }
        }

    }
}
