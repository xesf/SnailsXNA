using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.UI.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using System.IO;

namespace TwoBrainsGames.BrainEngine.UI.Screens
{

    public class UIScreen : Screen, IUIControl
    {
        public const int MAX_SCREEN_WITDH_IN_POINTS = 10000;
        public const int MAX_SCREEN_HEIGHT_IN_POINTS = 10000;

        #region Events
        public event UIControl.UIEvent OnAccept;// Occurs when the user presses the action Accept 
        public event UIControl.UIEvent OnBack;// Occurs when the user presses the action Back 
        public UIControl.UIEvent OnCancel;// Occurs when the user presses the action Cancel 
        public event UIControl.UIEvent OnBeforeControlsDraw;
        public event UIControl.UIEvent OnAfterInitializeFromContent;
        #endregion

        #region Vars
        private UITimer _tmrCursorSnap; // This timer is used to slow down cursor movement when cursor mode is set to snap to control

        internal ContainerControl _container;
        internal ControlCollection _controls; // List of users controls to be drawed before Screen.OnDraw is called
        private static Texture2D _clearTexture; 
        public static Texture2D ClearTexture // Used to clear the background. Static is just fine because we can use this for all controls
        {
            get
            {
                if (UIScreen._clearTexture == null)
                {
                    UIScreen._clearTexture = new Texture2D(BrainGame.Graphics, 1, 1);
                    UIScreen._clearTexture.SetData<Color>(new Color[1] { Color.White });
                }
                return UIScreen._clearTexture;
            }
        }
        private Color _blendColor;
        internal UIControl _focusControl;
        public bool CanFocus
        {
            get { return true; } // Should check some other stuff like enabled, visible etc}
        }
        #endregion

        #region Properties
        internal UIControl FocusControl 
        {
            get { return this._focusControl; }
            set
            {
                if (this._focusControl != value)
                {
                    if (this._focusControl != null && (value == null || (value != null && this._focusControl != value.Parent)))
                    {
                        this._focusControl.UnFocus();
                    }
                    UIControl prevFocusControl = this._focusControl;
                    this._focusControl = value;
                    if (this._focusControl != null && (prevFocusControl == null || (prevFocusControl != null && this._focusControl != prevFocusControl.Parent)))
                    {
                        this._focusControl.Focus();
                    }
                   
                }
            }
        } /// Holds the control that has the focus when CursorMode is SnapToControl

        #region IUIControl members
        public bool Enabled { get; set; }
        public UnitsType UnitType { get; set; }
        public Color BackgroundColor { get; set; }
        public Color BackgroundImageBlendColor { get; set; }
        public ScreenBackgroudImageMode BackgroundImageMode { get; set; }
        public Sprite BackgroundImage { get; set; }
        public AlignModes ParentAlignment { get; set; } // No effect for now
        public Size Size  { get; set; }

        public virtual Size SizeScaled
        {
            get
            {
                return new Size(this.Size.Width * this.Scale.X, this.Size.Height * this.Scale.Y);
            }
            set
            {
                this.Size = new Size(value.Width * this.Scale.X, value.Height * this.Scale.Y);
            }
        }
        public Vector2 Position { get; set; }
        public CursorModes CursorMode { get; set; }
        public bool CursorEnabled { get; set; }
        public bool IgnoreControlFocus { get; set; } // Controls don't get focus when this flag is set
        public Vector2 Scale { get; set; }
        public bool AcceptControllerInput { get; set; }
        public bool Visible { get; set; }
        public RenderMask Mask { get; set; }
        public bool CursorInside 
        {
            get
            {
                BoundingSquare bs = new BoundingSquare(this.PositionInPixels,
                                                      this.SizeInPixels.Width, this.SizeInPixels.Height);

                return bs.Contains(BrainGame.GameCursor.Position);
            }
        }

        public bool HasFocus
        {
            get { return (this.Navigator.ActiveScreen != null && this.Navigator.ActiveScreen == this); }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool CheckCursorInside()
        {
            return true; // Not implemented... just return true
        }

        #endregion

        public Size SizeInPixels
        { get { return this.ScreenUnitToPixels(this.Size); } }
        public Vector2 PositionInPixels
        { get { return this.ScreenUnitToPixels(this.Position); } }
        public Vector2 CenterInPixels
        { get { return this.PositionInPixels + new Vector2(this.SizeInPixels.Width / 2, this.SizeInPixels.Height / 2); } }
        public ControlCollection Controls
        {
            get { return this._controls; }
        }

        public bool IsFullscreen
        {
            get
            {
                if (this.PositionInPixels.X != 0 && this.PositionInPixels.Y != 0)
                    return false;
                if (this.SizeInPixels.Width != BrainGame.ScreenWidth && this.SizeInPixels.Height != BrainGame.ScreenHeight)
                    return false;

                return true;
            }

        }

        public UIScreen ScreenOwner
        {
            get 
            {
                throw new BrainException("This property cannot be accessed. You probably are trying to access this property in Screen.OnLoad(). Use 'this' instead. This is a design issue in the infraestructure...)");
            }
        }

        public IUIControl Parent
        {
            get { return null; }
            set { }
        }

        public InputBase InputController
        {
            get { return this._inputController; }
            set { this._inputController = value; }
        }
        internal bool Loading { get; set; }

        private bool SnapInputTimeEllapsed { get; set; }
        public UIControl CursorCaptureControl { get; set; } // Stores the control that capture the mouse. See UIControl.CaptureCursor() for more info on captures
        public bool IsCursorCaptured { get { return this.CursorCaptureControl != null; } }
        public SnapDirection CursorSnapDirections { get; set; } // Valid directions to search when the cursor snaps to another control

        internal DataFileRecord ControlsContentRootRecord { get; set; }
        BlendState LastBlendState { get; set;}


        /// <summary>
        /// The bounding box in pixels
        /// </summary>
        public virtual BoundingSquare BoundingBox
        {
            get
            {
                return new BoundingSquare(this.AbsolutePositionInPixels,
                                          this.Size.Width, this.Size.Height);
            }
        }
        #endregion

        public UIScreen(ScreenNavigator owner):
            base(owner)
        {
            this._controls = new ControlCollection(this);
            this._container = new ContainerControl(this._controls);
            this.UnitType = UnitsType.Point;
            this.Size = this.PixelsToScreenUnits(new Size(BrainGame.ScreenWidth, BrainGame.ScreenHeight));
            this.Position = Vector2.Zero;
            this.BlendColor = Color.White;
            this.CursorEnabled = true;
            this.Scale = new Vector2(1.0f, 1.0f);
            this.AcceptControllerInput = true;
            this.BackgroundImageBlendColor = Color.White;
            this.BackgroundColor = Color.Transparent;
            this.CursorSnapDirections = SnapDirection.All;

            // Timer used to slow down cursor snap
            this._tmrCursorSnap = new UITimer(this, 225, false);
            this._tmrCursorSnap.OnTimer += new UIControl.UIEvent(_tmrCursorSnap_OnTimer);
            this.Controls.Add(this._tmrCursorSnap);

            this.SnapInputTimeEllapsed = true;
            this.CursorMode = BrainGame.Settings.MenuCursorMode;
        }


        /// <summary>
        /// this method should only run once in the cycle of this screen
        /// </summary>
        internal override void Load()
        {
            this.Loading = true;
            this.OnLoad();
            foreach (UIControl control in this.Controls)
            {
                control.InternalLoad();
            }
            this.Loading = false;

        }

        /// <summary>
        /// 
        /// </summary>
        internal override void InitializeFromContent()
        {
            base.InitializeFromContent();
            if (this.ScreenContentRootRecord != null)
            {
                this.InitializeFromDataFileRecord();
                this.ControlsContentRootRecord = this.ScreenContentRootRecord.SelectRecord("Controls");
            }

            foreach (UIControl control in this.Controls)
            {
                control.InitializeFromContent();
            }
            if (this.OnAfterInitializeFromContent != null)
            {
                this.OnAfterInitializeFromContent(this);
            }

            foreach (UIControl control in this.Controls)
            {
                control.InternalAfterInitializeFromContent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeFromDataFileRecord()
        {
            this.BlendColor = this.GetContentPropertyValue<Color>("blendColor", this.BlendColor);
            this.InvokeInitializeFromContent();
        }
      
        /// <summary>
        /// Reads a property from the content file
        /// </summary>
        protected T GetContentPropertyValue<T>(string propName, T defaultValue)
        {
            T val = defaultValue;
            // First try to get the generic presentation property
            DataFileRecord propertiesRecord = this.ScreenContentRootRecord.SelectRecordByField("Properties", "presentationMode", "");
            if (propertiesRecord != null)
            {
                val = this.GetPropertyValue<T>(propertiesRecord, propName, defaultValue);
            }
            // Try to get the specific presentation property
            propertiesRecord = this.ScreenContentRootRecord.SelectRecordByField("Properties", "presentationMode", BrainGame.Settings.PresentationModeString);
            if (propertiesRecord != null)
            {
                val = this.GetPropertyValue<T>(propertiesRecord, propName, defaultValue);
            }
            return val;
        }

        /// <summary>
        /// Reads a property from the content file
        /// </summary>
        private T GetPropertyValue<T>(DataFileRecord parentRecord, string propName, T defaultValue)
        {
            return parentRecord.GetFieldValue<T>(propName, defaultValue);
        }

        /// <summary>
        /// this method should only run once in the cycle of this screen
        /// </summary>
        internal override void Start()
        {
            base.Start();
     
            // Invoke OnScreenStart on child controls
            foreach (UIControl control in this.Controls)
            {
                control.InternalOnScreenStart();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void Update(BrainGameTime gameTime)
        {
            if (this.Closed)
            {
                return;
            }

            // Controller actions
            if (this.CursorEnabled && this.AcceptControllerInput)
            {
                if (this.InputController.ActionAccept && this.OnAccept != null)
                {
                    this.OnAccept(this);
                }
                if (this.InputController.ActionBack && this.OnBack != null)
                {
                    this.OnBack(this);
                }
                if (this.InputController.ActionCancel && this.OnCancel != null)
                {
                    this.OnCancel(this);
                }

              /* Don't enable this code again.
               * Cursir position must be update in the Cursor update method
                {
                    BrainGame.GameCursor.Position = this._inputController.MotionPosition;
                }
                else*/
                if (this.CursorMode == CursorModes.SnapToControl)
                {
                    this.CheckControlSnap();
                }
            }

            // Loop all UIControls and call OnUpdate on them
            foreach (UIControl ctl in this._controls)
            {
                ctl.InternalUpdate(gameTime);
                if (this.Closed)
                {
                    return;
                }
            }    

            // Determine wich control gets the focus
            this.FocusControl = this.GetFocusControl(this);

            // Call derived class update event
            this.OnUpdate(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void InternalLanguageChanged()
        {
            // Loop all UIControls and call OnUpdate on them
            foreach (UIControl ctl in this._controls)
            {
                ctl.InternalLanguageChanged();
            } 
            
            base.InternalLanguageChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void InternalGameplayModeChanged()
        {
            // Loop all UIControls and call OnUpdate on them
            foreach (UIControl ctl in this._controls)
            {
                ctl.InternalGameplayModeChanged();
            }

            base.InternalGameplayModeChanged();
        }
        /// <summary>
        /// 
        /// </summary>
        public void BeginDraw(BlendState blendState)
        {
            this.LastBlendState = blendState;
            if (this.Mask != null)
            {
                this._spriteBatch.Begin(SpriteSortMode.Immediate, blendState, BrainGame.CurrentSampler, this.Mask.State, null, BrainGame.RenderEffect);
            }
            else
            {
                this._spriteBatch.Begin(SpriteSortMode.Immediate, blendState, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndDraw()
        {
            this._spriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SuspendDraw()
        {
            this.EndDraw();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResumeDraw()
        {
            this.BeginDraw(this.LastBlendState);
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void Draw()
        {
            if (this.BackgroundColor != Color.Transparent || this.BackgroundImage != null)
            {
                if (this.BackgroundColor != Color.Transparent && this.BackgroundColor.A != 255)
                {
                    this.BeginDraw(BlendState.AlphaBlend);
                }
                else
                {
                    this.BeginDraw(BlendState.Opaque);
                }
                if (this.BackgroundColor != Color.Transparent)
                {
                   
                    this._spriteBatch.Draw(UIScreen.ClearTexture, new Rectangle((int)this.PositionInPixels.X, (int)this.PositionInPixels.Y,
                                                                               (int)this.SizeInPixels.Width, (int)this.SizeInPixels.Height),
                                                                               this.BackgroundColor);
                }

                if (this.BackgroundImage != null)
                {
                    switch (this.BackgroundImageMode)
                    {
                        case ScreenBackgroudImageMode.Normal:
                            this.BackgroundImage.Draw(this.PositionInPixels, 0, this.BackgroundImageBlendColor, this.SpriteBatch);
                            break;
                        case ScreenBackgroudImageMode.FitToScreen:
                            this.BackgroundImage.Draw(this.PositionInPixels, BrainGame.ScreenRectangle, this.BackgroundImageBlendColor, this.SpriteBatch);
                            break;
                    }
                }
                this.EndDraw();
            }

            this.BeginDraw(BlendState.AlphaBlend);
            if (this.OnBeforeControlsDraw != null)
            {
                this.OnBeforeControlsDraw(this);
            }
            this.DrawControls();
            this.OnDraw();
            this.EndDraw();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawControls()
        {
            foreach (UIControl ctl in this._controls)
            {
                ctl.InternalDraw();
            }    
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckControlSnap()
        {
            // Only let snap, if the snap time has ellapsed. This is needed because cursor snap happens
            // when the buttons are down, if we don't add this, the movement will be too fast
            // A timer is used to control the snap time
            if (this.SnapInputTimeEllapsed == false)
            {
                return;
            }

            UIControl controlToSnap = null;
            Vector2 snapVector = Vector2.Zero;

            if (this._inputController.ActionRight && 
                (this.CursorSnapDirections & SnapDirection.Right) == SnapDirection.Right)
            {
                snapVector = new Vector2(1.0f, 0.0f);
            }
            else
            if (this._inputController.ActionDown &&
               (this.CursorSnapDirections & SnapDirection.Down) == SnapDirection.Down)
            {
                snapVector = new Vector2(0.0f, -1.0f);
            }
            else
            if (this._inputController.ActionLeft &&
                (this.CursorSnapDirections & SnapDirection.Left) == SnapDirection.Left)
            {
                snapVector = new Vector2(-1.0f, 0.0f);
            }
            else
            if (this._inputController.ActionUp &&
                (this.CursorSnapDirections & SnapDirection.Up) == SnapDirection.Up)
            {
                snapVector = new Vector2(0.0f, 1.0f);
            }
            else
            if (this._inputController.MotionPosition != Vector2.Zero &&
                !this._inputController.WithMouse)
            {
                snapVector = this._inputController.MotionPosition;

                // Clear vector if movement not allowed
                // Right
                if (snapVector.X > snapVector.Y && 
                    snapVector.X > 0 && 
                    (this.CursorSnapDirections & SnapDirection.Right) != SnapDirection.Right)
                {
                    snapVector = Vector2.Zero;
                }
                // left
                if (snapVector.X > snapVector.Y && 
                    snapVector.X < 0 && 
                    (this.CursorSnapDirections & SnapDirection.Left) != SnapDirection.Left)
                {
                    snapVector = Vector2.Zero;
                }
                // Down
                if (snapVector.X < snapVector.Y && 
                    snapVector.Y > 0 &&
                    (this.CursorSnapDirections & SnapDirection.Down) != SnapDirection.Down)
                {
                    snapVector = Vector2.Zero;
                }
                // Up
                if (snapVector.X < snapVector.Y && 
                    snapVector.Y < 0 &&
                    (this.CursorSnapDirections & SnapDirection.Up) != SnapDirection.Up)
                {
                    snapVector = Vector2.Zero;
                }
            }

            if (snapVector != Vector2.Zero)
            {
                controlToSnap = this.FindControlToSnap(snapVector);
                if (controlToSnap != null)
                {
                    this.FocusControl = controlToSnap;
                    this._tmrCursorSnap.Enabled = true;
                    this.SnapInputTimeEllapsed = false;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private UIControl FindControlToSnap(Vector2 directionalVector)
        {
            float nearDistance = 999999.0f;
            return this.FindControlToSnap(BrainGame.GameCursor.Position, this.Controls, directionalVector, ref nearDistance);
        }

        /// <summary>
        /// Finds the next control to snap to in snap mode
        /// For now container controls are not taken into account
        /// </summary>
        /// <param name="directionalVector">Direction where the search will be made</param>
        /// <returns></returns>
        private UIControl FindControlToSnap (Vector2 position, ControlCollection controls, Vector2 directionalVector, ref float nearDistance)
        {
           
            // Calculate a perpendicular vector with the directionalVector
            Vector2 ray = position + new Vector2(directionalVector.Y, directionalVector.X);
            UIControl nearestControl = null;
            foreach (UIControl control in controls)
            {
                if (control != this.FocusControl && 
                    control.CanFocus &&
                    !control.AnyChildAllowsFocus())
                {
                    // Classify the 4 corners of the control BB 
                    // The control will only elegible for snapping if all corners lie in the same side of the ray
                    float c1 = Mathematics.ClassifyPointInRay(control.BoundingBox.UpperLeft, position, ray);
                    float c2 = Mathematics.ClassifyPointInRay(control.BoundingBox.UpperRight, position, ray);
                    float c3 = Mathematics.ClassifyPointInRay(control.BoundingBox.LowerLeft, position, ray);
                    float c4 = Mathematics.ClassifyPointInRay(control.BoundingBox.LowerRight, position, ray);

                    if (c1 < 0 && c2 < 0 && c3 < 0 && c4 < 0)
                    {
                        // Determine the distance beetween focus control and the control that passed the previous test
                        // This is needed because we want to snap to the nearest control
                        float length = (position - control.CenterInPixels).Length();
                        if (length < nearDistance && control.CursorStop)
                        {
                            nearDistance = length;
                            nearestControl = control;
                        }
                    }
                
                }
            
                if (control.CanFocus)
                {
                    UIControl ctl = this.FindControlToSnap(position, control.Controls, directionalVector, ref nearDistance);
                    if (ctl != null)
                    {
                        nearestControl = ctl;
                    }
                }
            }

          
            return nearestControl;
        }

        /// <summary>
        /// 
        /// </summary>
        private UIControl GetFocusControl(IUIControl control)
        {
            // If the cursor is captured return the current focus control
            if (this.IsCursorCaptured)
            {
                return this.FocusControl;
            }

            if (this.InputController.IsMotionPointerDown == false)
            {
                return null;
            }

            if (control.CursorInside || control.CanFocus)
            {
                for (int i = control.Controls.Count - 1; i >= 0; i--)
                {
                    if (control.Controls[i].CheckCursorInside() && control.Controls[i].CanFocus)
                    {
                        UIControl childWithFocus = this.GetFocusControl(control.Controls[i]);
                        if (childWithFocus != null)
                        {
                            return childWithFocus;
                        }
                        return control.Controls[i];
                    }
                }
                
                return control as UIControl;
            }
           
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        void _tmrCursorSnap_OnTimer(IUIControl sender)
        {
            this.SnapInputTimeEllapsed = true;
        }

        /// <summary>
        /// Sets a capture on the cursor to this control
        /// When the cursor is captured, OnFocus, OnEnter events will not be launched on other controls until the capture is released
        /// This is used when dragging the control for instance
        /// </summary>
        public void CaptureCursor(UIControl control)
        {
            this.CursorCaptureControl = control;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReleaseCursor()
        {
            this.CursorCaptureControl = null;
        }

        #region Unit conversions

        /// <summary>
        /// Converts Point to screen units
        /// </summary>
		public static float ScreenUnitToPixelsY_(float val)
        {
            return (val * BrainGame.ScreenHeight / UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS);
        }
        /// <summary>
        /// Converts Point to screen units
        /// </summary>
        public float ScreenUnitToPixelsX(float val)
        {
            return (val * BrainGame.ScreenWidth / UIScreen.MAX_SCREEN_WITDH_IN_POINTS);
        }

        /// <summary>
        /// Converts Point to screen units
        /// </summary>
        public float ScreenUnitToPixelsY(float val)
        {
            return (val * BrainGame.ScreenHeight / UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS);
        }

        /// <summary>
        /// Converts vector to screen units
        /// </summary>
        public Vector2 ScreenUnitToPixels(Vector2 vector)
        {
            return new Vector2(this.ScreenUnitToPixelsX(vector.X),
                               this.ScreenUnitToPixelsY(vector.Y));
        }

        /// <summary>
        /// Converts Point to screen units
        /// </summary>
        public Size ScreenUnitToPixels(Size size)
        {
            return new Size(this.ScreenUnitToPixelsX(size.Width),
                            this.ScreenUnitToPixelsY(size.Height));
        }


        /// <summary>
        /// Converts float pixel to screen units X axis
        /// </summary>
        public float PixelsToScreenUnitsX(float val)
        {
            return (val * UIScreen.MAX_SCREEN_WITDH_IN_POINTS / BrainGame.ScreenWidth);
        }

        /// <summary>
        /// Converts float pixel to screen units Y axis
        /// </summary>
        public float PixelsToScreenUnitsY(float val)
        {
            return (val * UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS / BrainGame.ScreenHeight);
        }

		/// <summary>
		/// Converts float pixel to screen units Y axis
		/// </summary>
		public static float PixelsToScreenUnitsY_(float val)
		{
			return (val * UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS / BrainGame.ScreenHeight);
		}

		/// <summary>
		/// Converts float pixel to screen units Y axis
		/// </summary>
		public static float PixelsToViewportUnitsY_(float val)
		{
			return (val * UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS / BrainGame.Viewport.Height);
		}


        /// <summary>
        /// 
        /// </summary>
        public Size PixelsToScreenUnits(Size size)
        {
            return new Size(this.PixelsToScreenUnitsX(size.Width),
                            this.PixelsToScreenUnitsY(size.Height));
        }


        /// <summary>
        /// Converts rectangle to screen units
        /// </summary>
        public BoundingSquare PixelsToScreenUnits(BoundingSquare bb)
        {
            Vector2 ul = this.PixelsToScreenUnits(bb.UpperLeft);
            Vector2 lr = this.PixelsToScreenUnits(bb.LowerRight);
            return new BoundingSquare(ul, lr);
        }

        /// <summary>
        /// Converts vector to screen units
        /// </summary>
        public Vector2 PixelsToScreenUnits(Vector2 vector)
        {
            return new Vector2(this.PixelsToScreenUnitsX(vector.X),
                               this.PixelsToScreenUnitsY(vector.Y));
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Color BlendColor
        {
            get
            {
                return this._blendColor;
            }
            set
            {
                this._blendColor = value;
            }
        }
      

        public Vector2 AbsolutePositionInPixels
        {
            get { return this.PositionInPixels; }
        }

        public Vector2 AbsolutePosition
        {
            get { return this.Position; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 NativeResolution(Vector2 vector)
        {
            return new Vector2(vector.X * (float)BrainGame.NativeScreenWidth / (float)BrainGame.ScreenWidth,
                               vector.Y * (float)BrainGame.NativeScreenHeight / (float)BrainGame.ScreenHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        protected float NativeResolutionY(float y)
        {
            return (y * (float)BrainGame.NativeScreenHeight / (float)BrainGame.ScreenHeight);
        }
        /// <summary>
        /// 
        /// </summary>
        protected Size NativeResolution(Size size)
        {
            return new Size(size.Width * (float)BrainGame.NativeScreenWidth / (float)BrainGame.ScreenWidth,
                            size.Height * (float)BrainGame.NativeScreenHeight / (float)BrainGame.ScreenHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        protected float FromNativeResolutionY(float val)
        {
            return (val * (float)BrainGame.ScreenHeight / (float)BrainGame.NativeScreenHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        protected float FromNativeResolutionX(float val)
        {
            return (val * (float)BrainGame.ScreenWidth / (float)BrainGame.NativeScreenWidth);
        }

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 FromNativeResolution(Vector2 vector)
        {
            return new Vector2(vector.X * (float)BrainGame.ScreenWidth / (float)BrainGame.NativeScreenWidth,
                               vector.Y * (float)BrainGame.ScreenHeight / (float)BrainGame.NativeScreenHeight);
        }
    }
}
