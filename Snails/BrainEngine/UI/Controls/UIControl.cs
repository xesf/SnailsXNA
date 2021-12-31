using System;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public interface IUIControl
    {
        string Name { get; }
        UIScreen ScreenOwner { get; }
        IUIControl Parent { get; set; }
        Size Size { get; set; }
        Size SizeScaled { get; set; }
        Vector2 Position { get; set; }
        Vector2 AbsolutePosition { get;}
        Size SizeInPixels { get; }
        Vector2 PositionInPixels { get; }
        Vector2 CenterInPixels { get; }
        Color BackgroundColor { get; set; }
        AlignModes ParentAlignment { get; set; }
        UnitsType UnitType { get; set; }
        ControlCollection Controls { get;}
        Color BlendColor { get; set; }
        Vector2 AbsolutePositionInPixels { get; }
        Vector2 Scale { get; set; }
        bool AcceptControllerInput { get; set; }
        bool CursorInside { get; }
        bool CanFocus { get; }
        bool HasFocus { get; }
        bool CheckCursorInside();
        BoundingSquare BoundingBox { get; }
    }


    public class UIControl : IUIControl
    {
        #region Consts
        private const int ONACCEPT_EFFECT_IDX = 0;
        private const int ONFOCUS_EFFECT_IDX = 1;
        #endregion

        enum EffectContext
        {
            None,
            Hidding,
            Showing,
            Focus
        }

        #region Events
        public delegate void UIEvent(IUIControl sender);
        public delegate bool UIControlAddedEvent(IUIControl sender, UIControl control);

        public event UIEvent OnSizeChanged; // Occurs when the size of the control changes
        public event UIEvent OnHide; // Occurs when the cursor Visible propertie is set to false
        public event UIEvent OnHideBegin;
        public event UIEvent OnShow; // Occurs when the cursor Visible propertie is set to true
        public event UIEvent OnShowBegin; // Occurs when the cursor Visible propertie is set to true
        public event UIEvent OnEnter; // Occurs when the cursor enters the control
        public event UIEvent OnLeave; // Occurs when the cursor leaves the control
        public event UIEvent OnAccept; // Occurs when the user presses the action Accept when the cursor is on the control
        public event UIControlAddedEvent OnBeforeControlAdded; // Occurs right before a control is added. If false is returned, the control is not added
        public event UIEvent OnAcceptBegin;
        public event UIEvent OnDoubleTapAccept; // Occurs when the user double tap the action Accept when the cursor is on the control
        public event UIEvent OnDoubleTapAcceptBegin;
        public event UIEvent OnBack; // Occurs when the user presses the action Back when the cursor is on the control
        public event UIEvent OnCancel; // Occurs when the user presses the action Cancel when the cursor is on the control
        public event UIEvent OnControllerAction; // Occurs when the controller action that corresponds to the ControllerActionCode is pressed
        public event UIEvent OnFocus; // Occurs when the controller gets the input focus
        public event UIEvent OnLostFocus; // Occurs when the controller gets the input focus  
        public event UIEvent OnMotionPointerDown; // Occurs when the action is pressed when the motion cursor is on the control
        public event UIEvent OnMotionPointerUp; // Occurs when the action is depressed. 
                                                // This event occurs only if the mouse was previously pressed over the control and it can occur even if the cursor is out of the control 
        public event UIEvent OnLanguageChanged;
        public event UIControl.UIEvent OnScreenStart;
        public event UIControl.UIEvent OnInitializeFromContent;
        public event UIControl.UIEvent OnAfterInitializeFromContent;
        public event UIControl.UIEvent OnGameplayModeChanged;
        public Sample ShowSoundEffect { get; set; } 
        #endregion

        #region Vars
        private Size _size;
        private UnitsType _unitType;
        private bool _visible;
        private Color _blendColor;
        private Vector2 _scale;
        private bool _acceptControllerInput;
        private bool _cursorDownOnControl;
        private Rectangle _clientRect;
        private Rectangle _clientRectInPixels;
        private IUIControl _parent;
        private string _textResourceId;
        private Vector2 _position;
        private AlignModes _parentAlignment;
        #endregion

        #region Properties
        public string Name { get; set; }
        public Vector2 Position  
        {
            get { return this._position; }
            set
            {
                if (this._position != value)
                {
                    this._position = value;
                    this.NeedUpdateLayout = true;
                }
            }
        }

        public Rectangle ClientRect 
        {
            get { return this._clientRect; }
            private set
            {
                this._clientRect = value;
                Vector2 pos = new Vector2(this._clientRect.X, this._clientRect.Y);
                Vector2 size = new Vector2(this._clientRect.Width, this._clientRect.Height);
                pos = this.ScreenUnitToPixels(pos);
                size = this.ScreenUnitToPixels(size);

                this._clientRectInPixels = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

            }

        }
        public Rectangle ClientRectInPixels 
        {
            get
            {
                return this._clientRectInPixels;
            }
        }
        public float Rotation { get; set; }
        public Color BackgroundColor { get; set; }
        public bool UseBlendColorOnBackground{ get; set; }

        public virtual bool Enabled { get; set; }
        public bool CursorStop { get; set; }
        public bool Hidden { get; set; } // When hidden, control is invisible but update processes normally
    
        public bool AcceptControllerInput 
        {
            get 
            {
                // The get logic is: if parent is true, returns self else returns parent
                // If parent is false, all childs will be false, otherwise the childs will be AcceptControllerInput
                if (this.Parent != null)
                {
                    if (this.Parent.AcceptControllerInput == true)
                    {
                        return this._acceptControllerInput;
                    }
                    return this.Parent.AcceptControllerInput;
                }
                return this._acceptControllerInput;
            }
            set
            {
                this._acceptControllerInput = value;
            }
        }

        public bool CanFocus
        {
            get {
                if (this.ScreenOwner.IsCursorCaptured)
                {
                    if (this.ScreenOwner.CursorCaptureControl != this)
                    {
                        return false;
                    }
                }

                return (this.Visible && 
                          this.Enabled &&   
                          this.AcceptControllerInput && 
                          this.Active); 
            }
        }
        public bool HasFocus
        {
            get { return this.ScreenOwner != null && this.ScreenOwner.FocusControl == this; } 
        }

        public TransformBlender EffectsBlender { get; private set; }
        private TransformBlender EventsEffectsBlender { get; set; }

        public IUIControl Parent 
        {
            get { return this._parent; }
            set
            {
                if (this._parent != value)
                {
                    this._parent = value;
                    this.ParentChanged();
                }
                else
                {
                    this._parent = value;
                }
            }
        }
        protected bool Busy { get; set; } // When busy, update does not run. This is not the same as disabled.
                                        // A control may be enabled but busy

        // Inactive controls don't process controller input nor can accept focus
        private bool Active
        {
            get
            {
                if (this.Enabled == false || this.Busy)
                {
                    return false;
                }
               
                return true;
            }
        }
        private bool NeedUpdateLayout { get; set; }
        private EffectContext CurrentEffectContext { get; set; } // The context were CurrentEffect is being used
        protected TransformEffectBase CurrentEffect { get; set; } // Current active effect. When active, overrides control effects blender
        
        // Event effects    
        public TransformEffectBase ShowEffect { get; set; } // Effect to apply when the object is about to change state from invisible to visible
        public TransformEffectBase HideEffect { get; set; } // Effect to apply when the object is about to change state from visible to invisible
        public TransformEffectBase OnFocusEffect { get; set; } // Effect to apply when the object gets the focus
        public TransformEffectBase OnAcceptEffect { get; set; } // // Effect to apply when the Accept is pressed over the control

        public bool CursorInside { get; set; } // True if the cursor is inside the control - useful to launch OnEnter and OnLeave events
        public bool BlendColorWithParent { get; set; }
        public bool BlendScaleWithParent { get; set; }
        public bool CanBeScaled { get; set; }
        public int ControllerActionCode { get; set; } // The input controller action code that makes the control launch an OnAction event
                                                      // This is an int that maps to the InputActions in the input controller (this is a bit flag field
                                                      // so it can include multiple actions). The field is an int and not the InputActions enum because
                                                      // screens may override the InputActions and create custom actions)
        public ControlCollection Controls { get; private set; }
        public AlignModes ParentAlignment 
        {
            get
            {
                return this._parentAlignment; 
            }
            set
            {
                if (this._parentAlignment != value)
                {
                    this._parentAlignment = value;
                    this.NeedUpdateLayout = true;
                }
            }
        }
        public Vector2 ParentAlignmentOffset { get; set; }
        public Margin Margins { get; private set; }
        public UIScreen ScreenOwner { get; set; }

        // Shadow support
        public bool DropShadow { get; set; }
        public Color ShadowColor { get; set; }
        public Vector2 ShadowDistance { get; set; }
        public bool InvokeOnAcceptOnMotionUp { get; set; }

        public Color BlendColor
        {
            get
            {
                if (this.Parent != null && this.BlendColorWithParent)
                {
                    Vector4 vColor = this._blendColor.ToVector4();
                    Vector4 vColorParent= this.Parent.BlendColor.ToVector4();
                    return new Color(vColor * vColorParent);
                }
                return this._blendColor;
            }
            set
            {
                this._blendColor = value;
               
            }
        }

        public virtual Vector2 Scale
        {
            get
            {
                if (this.Parent != null && this.BlendScaleWithParent)
                {
                    return this._scale * this.Parent.Scale;
                }
                return this._scale;
            }
            set
            {
                if (this._scale != value)
                {
                    this._scale = value;
                    this.NeedUpdateLayout = true;
                    this.InvalidateChildsLayout();
                }
            }
        }
        public virtual bool Visible 
        {
            get
            {
                return this._visible;
            }
            set
            {
                this._visible = value;
                if (this._visible != value)
                {
                    this._visible = value;
                    if (this.OnHide != null)
                    {
                        this.OnHide(this);
                    }
                }
            }
        }

        public ITransformEffect Effect
        {
            set
            {
                this.EffectsBlender.Clear();
                if (value != null)
                {
                    this.EffectsBlender.Add(value);
                }
            }
            get
            {
                if (this.EffectsBlender.Count == 0)
                {
                    return null;
                }
                return this.EffectsBlender[0];
            }
        }
        public UnitsType UnitType 
        { 
            get 
            {
                if (this.ScreenOwner == null) // if the control isn't attached to a screen, use the control unit
                {
                    return this._unitType;
                }
                return this.ScreenOwner.UnitType;
            }
            set
            {
                this._unitType = value; // This will have no effect if the control is attached to a screen
            }
        }

        public virtual Size SizeScaled
        {
            get
            {
                return new Size(this._size.Width * this.Scale.X, this._size.Height * this.Scale.Y);
            }
            set
            {
                this.Size = new Size(value.Width * this.Scale.X, value.Height * this.Scale.Y);
            }
        }

        public virtual Size Size
        {
            get
            {
                return this._size;
            }
            set
            {
                if (this._size.Height != value.Height || 
                    this._size.Width != value.Width)
                {
                    this._size = value;
                    this.NeedUpdateLayout = true;
                    this.InvalidateChildsLayout();
                    if (this.OnSizeChanged != null && this.LaunchOnSizeChanged)
                    {
                        this.OnSizeChanged(this);
                    }
                }
            }
        }

        public bool LaunchOnSizeChanged { get; set; }

        /// <summary>
        /// Size may be in Pixels or in Points depending on the Screen unit
        /// </summary>
        public Size SizeInPixels
        { get { return this.ScreenUnitToPixels(this.Size); } }
        public Size SizeInPixelsScaled
        { get { return this.ScreenUnitToPixels(this.SizeScaled); } }
        /// <summary>
        /// Position may be in Pixels or in Points depending on the Screen unit
        /// </summary>
        public Vector2 PositionInPixels
        { 
            get 
            { 
                return this.ScreenUnitToPixels(this.Position); 
            }
            set
            {
                this.Position = this.PixelsToScreenUnits(value);
            }
        }

        /// <summary>
        /// Center of the control in pixels
        /// </summary>
        public Vector2 CenterInPixels
        { get { return this.AbsolutePositionInPixels + new Vector2(this.SizeInPixelsScaled.Width / 2, this.SizeInPixelsScaled.Height / 2); } }
       
        public Vector2 Center
        { get { return this.AbsolutePosition + new Vector2(this.SizeScaled.Width / 2, this.SizeScaled.Height / 2); } }

        protected SpriteBatch SpriteBatch
        {
            get { return this.ScreenOwner.SpriteBatch; }
        }

        /// <summary>
        /// The bounding box in pixels
        /// </summary>
        public virtual BoundingSquare BoundingBox
        {
            get
            {
                return new BoundingSquare(this.AbsolutePositionInPixels,
                                          this.SizeInPixelsScaled.Width, this.SizeInPixelsScaled.Height);
            }
        }


        public virtual Vector2 AbsolutePositionInPixels
        {
            get
            {
                return this.ScreenUnitToPixels(this.AbsolutePosition);
            }
        }


        public virtual Vector2 AbsolutePosition
        {
            get
            {
                Vector2 pos = Vector2.Zero;

                if (this.Parent != null)
                {

                    // Important! If the parent is scaled, we also have to scale the positions using the parent scale, or else the position will be off
                    // This is needed because object positions are always relative to the parent
                    switch (this.ParentAlignment)
                    {
                        case AlignModes.None:
                            pos = this.Parent.AbsolutePosition + (this.Position * this.Parent.Scale);
                            break;
                        case AlignModes.Horizontaly:
                        case AlignModes.Left:
                        case AlignModes.Right:
                            pos = this.Parent.AbsolutePosition + (this.Position * new Vector2(1.0f, this.Parent.Scale.Y));
                            break;
                        case AlignModes.Vertically:
                        case AlignModes.Bottom:
                        case AlignModes.Top:
                            pos = this.Parent.AbsolutePosition + (this.Position * new Vector2(this.Parent.Scale.X, 1.0f));
                            break;
                        default:
                            pos = this.Parent.AbsolutePosition + this.Position;
                            break;
                    }


                    //   pos = this.Parent.AbsolutePositionInPixels + (this.PositionInPixels * this.Parent.Scale);

                }
                else
                {
                    pos = this.Position;
                }

                return pos;
            }

        }
        public bool WithFocus
        {
            get 
            {
                if (this.ScreenOwner.CursorMode == CursorModes.Free)
                {
                    return this.CursorInside;
                }
                return (this.ScreenOwner.FocusControl == this); 
            }
        }

        public float Top { get { return this.Position.Y; } }
        public float Left { get { return this.Position.X; } }
        public float Right { get { return this.Position.X + this.Size.Width; } }
        public float Bottom { get { return this.Position.Y + this.Size.Height; } }
        public Vector2 PivotPosition { get; set; }
        public float Width
        { 
            get { return this.Size.Width; }
            set
            {
                this.Size = new Size(value, this.Size.Height);
            }
        }
        public float Height
        {
            get { return this.Size.Height; }
            set
            {
                this.Size = new Size(this.Size.Width, value);
            }
        }
        public virtual string Text { get; set; }

        public virtual string TextResourceId
        {
            get
            {
                return this._textResourceId;
            }
            set
            {
                this._textResourceId = value;
                this.Text = LanguageManager.GetString(value);
            }
        }

        private DataFileRecord ControlContentRootRecord { get; set; }
        #endregion

        public UIControl(UIScreen screenOwner)
        {
         /*   if (screenOwner == null)
            {
                throw new BrainException("screenOwner cannot be null!");
            }*/
            this.ScreenOwner = screenOwner;
            this.Controls = new ControlCollection(this);
            this.Size = new Size(1000, 1000);
            this.BackgroundColor = Color.Transparent;
            this.Visible = true;
            this.Enabled = true;
            this.EffectsBlender = new TransformBlender();
            this.EventsEffectsBlender = new TransformBlender();
            this.BlendColor = Color.White;
            this.BlendColorWithParent = true;
            this.BlendScaleWithParent = true;
            this.CanBeScaled = true;
            this.AcceptControllerInput = true;
            this.Margins = new Margin(this);
            this.Scale = Vector2.One;
            this.ShadowDistance = new Vector2(10.0f, 10.0f);
            this.ShadowColor = new Color(0.0f, 0.0f, 0.0f, 0.3f);
            this.CursorStop = true;
            this.LaunchOnSizeChanged = true;
            this.NeedUpdateLayout = true;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InternalLoad()
        {
            this.UpdateLayout();
            foreach (UIControl control in this.Controls)
            {
                control.InternalLoad();
            }
            this.Load();
        }

        // This code is replicated from UIScreen, this is because there's a bad design here
        // UIControl and UIScreen should share the same base!
        /// <summary>
        /// 
        /// </summary>
        protected void InitializeFromContent(string contentName)
        {
            this.InitializeFromContent(BrainGame.Settings.NavigatorControlContentFolder, contentName);
        }      
        
        /// <summary>
        /// 
        /// </summary>
        protected void InitializeFromContent(string contentFolder, string contentName)
        {
            string controlSetupContentName = Path.Combine(contentFolder, contentName);
            this.ControlContentRootRecord = BrainGame.ResourceManager.Load<DataFileRecord>(controlSetupContentName, ResourceManager.ResourceManagerCacheType.Static);
            this.InitializeFromDataFileRecord();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void InitializeFromContent()
        {
            // 
        /*    string controlSetupContentName = Path.Combine(BrainGame.Settings.NavigatorControlContentFolder, this.GetType().Name);
            if (this.InitializeFromContent)
            {

                this.ControlContentRootRecord = BrainGame.ResourceManager.Load<DataFileRecord>(controlSetupContentName, ResourceManager.ResourceManagerCacheType.NoCache);
                this.InitializeFromDataFileRecord();
            }
            */
            if (this.ScreenOwner.ControlsContentRootRecord != null)
            {
                if (!string.IsNullOrEmpty(this.Name))
                {
                    this.ControlContentRootRecord = this.ScreenOwner.ControlsContentRootRecord.SelectRecordByField("Control", "name", this.Name);
                    if (this.ControlContentRootRecord != null)
                    {
                        this.InitializeFromDataFileRecord();
                    }
                }
            }

            if (this.OnAfterInitializeFromContent != null)
            {
                this.OnAfterInitializeFromContent(this);
            }

            foreach (UIControl control in this.Controls)
            {
                control.InitializeFromContent();
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InternalAfterInitializeFromContent()
        {
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
            this.Position = this.GetContentPropertyValue<Vector2>("position", this.Position);
            this.ParentAlignment = (AlignModes)Enum.Parse(typeof(AlignModes), this.GetContentPropertyValue<string>("parentAlignment", this.ParentAlignment.ToString()), true);
            this.Scale = this.GetContentPropertyValue<Vector2>("scale", this.Scale);
            this.Size = this.GetContentPropertyValue<Size>("size", this.Size);
            this.BackgroundColor = this.GetContentPropertyValue<Color>("backColor", this.BackgroundColor);
            this.Size = new Size(this.GetContentPropertyValue<float>("width", this.Size.Width), this.Size.Height);
            this.Size = new Size(this.Size.Width, this.GetContentPropertyValue<float>("height", this.Size.Height));
            this.Margins.Bottom = this.GetContentPropertyValue<float>("bottomMargin", this.Margins.Bottom);

            if (OnInitializeFromContent != null)
            {
                this.OnInitializeFromContent(this);
            }
        }

        /// <summary>
        /// Reads a property from the content file
        /// </summary>
        protected T GetContentPropertyValue<T>(string propName, T defaultValue)
        {
            T val = defaultValue;
            if (this.ControlContentRootRecord == null)
            {
                return val;
            }

            // First try to get the generic presentation property
            DataFileRecord propertiesRecord = this.ControlContentRootRecord.SelectRecordByField("Properties", "presentationMode", "");
            if (propertiesRecord != null)
            {
                val = this.GetPropertyValue<T>(propertiesRecord, propName, defaultValue);
            }
            // Try to get the specific presentation property
            propertiesRecord = this.ControlContentRootRecord.SelectRecordByField("Properties", "presentationMode", BrainGame.Settings.PresentationModeString);
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
        /// 
        /// </summary>
        internal void InternalUpdate(BrainGameTime gameTime)
        {
            if (this.ScreenOwner.Closed)
            {
                return;
            }

            this.UpdateLayout();

            if (this.Visible)
            {
                this.UpdateEffects(gameTime);

                if (this.UseBlendColorOnBackground)
                {
                    this.BackgroundColor = this.BlendColor;
                }
                this.Update(gameTime);

                this.UpdateLayout();


                // Update childs
                foreach (UIControl control in this.Controls)
                {
                    control.InternalUpdate(gameTime);
                }
            }

            if (!this.Enabled || ! this.Visible)
            {
                this.CursorInside = false; // This has to be set or else OnEnter event might not run
                return;
            }


            if (this.ScreenOwner != null)
            {
                if (this.CanFocus && this.CheckCursorInside())
                {
                    if (!this.CursorInside)
                    {
                        this.CursorInside = true;
                        if (this.OnEnter != null)
                        {
                            this.OnEnter(this);
                        }
                       
                    } 
                }
                else
                {
                    if (this.CursorInside)
                    {
                        this.CursorInside = false;
                        if (this.OnLeave != null)
                        {
                            this.OnLeave(this);
                        }
                    }
                }
            }

            if (this.Active == false)
            {
                return;
            }

            if (this.AcceptControllerInput)
            {
                this.ProcessController();
            }

            this.UpdateLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InternalDraw()
        {
            if (!this.Visible || this.Hidden)
            {
                return;
            }

            this.UpdateLayout();
   
            this.BeginDraw();
            if (this.BackgroundColor != Color.Transparent)
            {
                this.SpriteBatch.Draw(UIScreen.ClearTexture, this.BoundingBox.ToRect(), this.BackgroundColor);
            }

            foreach (UIControl control in this.Controls)
            {
                control.InternalDraw();
            }
            this.Draw();
            this.EndDraw();
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void InternalLanguageChanged()
        {
            if (!string.IsNullOrEmpty(this.TextResourceId))
            {
                this.Text = LanguageManager.GetString(this.TextResourceId);
            }

            if (this.OnLanguageChanged != null)
            {
                this.OnLanguageChanged(this);
            }

            foreach (UIControl control in this.Controls)
            {
                control.InternalLanguageChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InternalGameplayModeChanged()
        {
            if (this.OnGameplayModeChanged != null)
            {
                this.OnGameplayModeChanged(this);
            }

            foreach (UIControl control in this.Controls)
            {
                control.InternalGameplayModeChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InvalidateChildsLayout()
        {
            foreach (UIControl control in this.Controls)
            {
                if (control.ParentAlignment != AlignModes.None)
                {
                    control.NeedUpdateLayout = true;
                    control.InvalidateChildsLayout();
                }
            }
        }

        public virtual void Load()
        { }


        public virtual void BeginDraw()
        { } 

        public virtual void Draw()
        { }

        public virtual void EndDraw()
        { }
        

        public virtual void OnResize()
        { }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update(BrainGameTime gameTime)
        { }


        /// <summary>
        /// Returns true if any child can aquire focus (CursorFocus = true)
        /// Controls that have at least one child that can get focus will not snap the cursor even if CursorFocus is true
        /// </summary>
        internal bool AnyChildAllowsFocus()
        {
            foreach (UIControl control in this.Controls)
            {
                if (control.AcceptControllerInput)
                {
                    return true;
                }
                if (control.AnyChildAllowsFocus() == true)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Optimization possible here. This is static most of the time but is calculated all frames for all controls
        /// </summary>
        public void UpdateLayout()
        {
            if (this.NeedUpdateLayout == false)
            {
                return;
            }

            this.ClientRect = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Size.Width, (int)this.Size.Height);
            if (this.ParentAlignment == AlignModes.None)
            {
                return;
            }

            float x = this.Position.X;
            float y = this.Position.Y;
           
            if ((this.ParentAlignment & AlignModes.Horizontaly) == AlignModes.Horizontaly)
            {
                x = (this.Parent.SizeScaled.Width / 2) - (this.SizeScaled.Width / 2);
            }
            if ((this.ParentAlignment & AlignModes.Vertically) == AlignModes.Vertically)
            {
                y = (this.Parent.SizeScaled.Height / 2) - (this.SizeScaled.Height / 2);
            }

            if ((this.ParentAlignment & AlignModes.Right) == AlignModes.Right)
            {
                x = this.Parent.SizeScaled.Width - this.SizeScaled.Width;
                x -= (int) this.Margins.Right;
            }

            if ((this.ParentAlignment & AlignModes.Bottom) == AlignModes.Bottom)
            {
                y = this.Parent.SizeScaled.Height - this.SizeScaled.Height;
                y -= (int)this.Margins.Bottom;
            }

            if ((this.ParentAlignment & AlignModes.Left) == AlignModes.Left)
            {
                x = BrainGame.ScreenRectangle.X;
                x += (int)this.Margins.Left;
            }

            if ((this.ParentAlignment & AlignModes.Top) == AlignModes.Top)
            {
				y = BrainGame.ScreenRectangle.Y;
                y += (int)this.Margins.Top;
            }

            x += +this.ParentAlignmentOffset.X;
            y += +this.ParentAlignmentOffset.Y;
            this.Position = new Vector2(x, y);
            this.ClientRect = new Rectangle((int)x, (int)y, (int)this.Size.Width, (int)this.Size.Height);
            this.NeedUpdateLayout = false;
            this.InvalidateChildsLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessController()
        {

            if (
                (this.CursorInside && this.WithFocus &&
                    (this.ScreenOwner.InputController.ActionAccept ||
                    (!this.ScreenOwner.InputController.IsMotionPointerDown &&
                    this.ScreenOwner.InputController.WasMotionPointerDown &&
                    this.InvokeOnAcceptOnMotionUp))) ||
                ((this.ControllerActionCode & (int)this.ScreenOwner.InputController.Actions) != 0)
                )
            {
                this.InvokeOnAcceptBegin();

                if (this.OnAcceptEffect != null)
                {
                    this.EventsEffectsBlender.DeleteEffects(UIControl.ONACCEPT_EFFECT_IDX);
                    this.OnAcceptEffect.Reset();
                    this.OnAcceptEffect.OnEnd = this.OnAcceptEffect_Ended;
                    this.EventsEffectsBlender.Add(this.OnAcceptEffect, UIControl.ONACCEPT_EFFECT_IDX);
                }
                else
                {
                    this.InvokeOnAccept();
                }
            }
            // Mouse / motion cursor action down on cursor
            if (this.CursorInside && this.WithFocus && this.ScreenOwner.InputController.ActionAcceptDown)
            {
                this._cursorDownOnControl = true;
                if (this.OnMotionPointerDown != null)
                {
                    this._cursorDownOnControl = true;
                    this.OnMotionPointerDown(this);
                }
            }

            // On mouse/ motion cursor up
            if (this._cursorDownOnControl && this.ScreenOwner.InputController.ActionAcceptUp)
            {
                if (this.OnMotionPointerUp != null)
                {
                    this._cursorDownOnControl = false;
                    this.OnMotionPointerUp(this);
                }
            }

            if (this.CursorInside && this.ScreenOwner.InputController.ActionBack && this.OnBack != null)
            {
                this.OnBack(this);
            }

            if (this.CursorInside && this.ScreenOwner.InputController.ActionCancel && this.OnCancel != null)
            {
                this.OnCancel(this);
            }


            if (this.ControllerActionCode != 0)
            {
              
                if ((this.ControllerActionCode & (int)this.ScreenOwner.InputController.Actions) != 0 &&
                     this.OnControllerAction != null)
                {
                    this.OnControllerAction(this);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void HideEffectEnded()
        {
            this.CurrentEffect = null;
            this.Busy = false;
            this.CurrentEffectContext = EffectContext.None;
            this._visible = false;
            if (this.OnHide != null)
            {
                this.OnHide(this);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void ShowEffectEnded()
        {
            this.CurrentEffect = null;
            this.Busy = false;
            this.CurrentEffectContext = EffectContext.None;
            if (this.OnShow != null)
            {
                this.OnShow(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnAcceptEffect_Ended(object param)
        {
            if (this.OnAccept != null)
            {
                this.OnAccept(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool CheckCursorInside()
        {
            if (this.ScreenOwner.FocusControl == this &&
                this.BoundingBox.Contains(BrainGame.GameCursor.Position) == false)
            {
                return this.BoundingBox.Contains(BrainGame.GameCursor.Position);
            }
            return this.BoundingBox.Contains(BrainGame.GameCursor.Position);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void UpdateCurrentEffect(BrainGameTime gameTime)
        {
            if (this.CurrentEffect != null)
            {
                this.CurrentEffect.Update(gameTime);
                this.ApplySingleEffect(this.CurrentEffect);

                // Effect ended. Because CurrentEffect is a generic effect, we have to check the context
                // where the effect is being used
                if (this.CurrentEffect.Ended)
                {
                    switch (this.CurrentEffectContext)
                    {
                        case EffectContext.Hidding:
                            this.HideEffectEnded();
                            break;
                        case EffectContext.Showing:
                            this.ShowEffectEnded();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void UpdateEffects(BrainGameTime gameTime)
        {
            if (this.CurrentEffect != null)
            {
                this.UpdateCurrentEffect(gameTime);
            }
            else
            {
                if (this.EffectsBlender.WithActiveEffects)
                {
                    this.EffectsBlender.Update(gameTime);
                    this.BlendColor = this.EffectsBlender.Color;
                    this.PositionInPixels += this.EffectsBlender.PositionV2;
                    this.Scale = this._scale + this.EffectsBlender._scale;
                }

                // Event effects
                if (this.EventsEffectsBlender.WithActiveEffects)
                {
                    this.EventsEffectsBlender.Update(gameTime);
                    this.BlendColor = this.EventsEffectsBlender.Color;
                    this.PositionInPixels += this.EventsEffectsBlender.PositionV2;
                    this.Scale = this._scale + this.EventsEffectsBlender._scale;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void UnFocus()
        {
            this.EventsEffectsBlender.DeleteEffects(UIControl.ONFOCUS_EFFECT_IDX);

            this.InvokeOnLostFocus();
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void ParentChanged()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Focus()
        {
            if (this.CanFocus == false)
            {
                return;
            }

            this.ScreenOwner.FocusControl = this;
            if (this.ScreenOwner != null && this.ScreenOwner.CursorMode == CursorModes.SnapToControl)
            {
                BrainGame.GameCursor.Position = this.CenterInPixels;
                //this.CursorInside = true; // WARNING: this prevent in XBOX to run the event OnEnter/OnLeave
                //      Yes it did!! And I knew that!! Don't know how this was here...
            }

            if (this.OnFocusEffect != null)
            {
                this.OnFocusEffect.Reset();
                this.EventsEffectsBlender.DeleteEffects(UIControl.ONFOCUS_EFFECT_IDX);
                this.EventsEffectsBlender.Add(this.OnFocusEffect, UIControl.ONFOCUS_EFFECT_IDX);
            }

            if (this.OnFocus != null)
            {
                this.OnFocus(this);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void ApplySingleEffect(TransformEffectBase effect)
        {
            if (effect != null)
            {
                this.BlendColor = effect.Color;
                this.Scale = effect.Scale;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void ApplyEffects(TransformBlender effectsBlender)
        {
            this.BlendColor = effectsBlender.Color;
        }

        /// <summary>
        /// Places the control on top of the parent Controls list
        /// </summary>
        public void SendToBack()
        {
            if (this.Parent != null)
            {
                if (this.Parent.Controls.Contains(this))
                {
                    IUIControl parent = this.Parent;
                    this.Parent.Controls.Remove(this);
                    parent.Controls.InsertAt(0, this);
                }
            }
        }

        /// <summary>
        /// Places the control on top of the parent Controls list
        /// </summary>
        public void BringToFront()
        {
            if (this.Parent != null)
            {
                if (this.Parent.Controls.Contains(this))
                {
                    IUIControl parent = this.Parent;
                    this.Parent.Controls.Remove(this);
                    parent.Controls.InsertAt(parent.Controls.Count, this);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ScaleChilds(Vector2 scaleFactor)
        {
            foreach (UIControl control in this.Controls)
            {
                if (control.CanBeScaled)
                {
                    control.Scale *= scaleFactor;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Show()
        {
            if (this.ShowSoundEffect != null)
            {
                this.ShowSoundEffect.Play();
            }

            this.Visible = true;
            if (this.OnShowBegin != null)
            {
                this.OnShowBegin(this);
            }         

            if (this.ShowEffect != null)
            {
               
                this.ShowEffect.Reset();
                this.CurrentEffect = this.ShowEffect;
                this.Busy = true;
                this.CurrentEffectContext = EffectContext.Showing;
            }
            foreach (UIControl ctl in this.Controls)
            {
                if (ctl.Visible == true)
                {
                    ctl.Show();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Hide()
        {
            if (this.Visible == false)
            {
                return;
            }

            if (this.OnHideBegin != null)
            {
                this.OnHideBegin(this);
            }

            if (this.HideEffect != null)
            {
                this.HideEffect.Reset();
                this.CurrentEffect = this.HideEffect;
                this.Busy = true;
                this.CurrentEffectContext = EffectContext.Hidding;

            }
            else // If there's an effect, visible is only set to false when the effect ends
            {
                this.Visible = true; // ????? what the fk?!
                foreach (UIControl ctl in this.Controls)
                {
                    if (ctl.Visible == true)
                    {
                        ctl.Hide();
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return (this.Name != null? this.Name : base.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InternalOnScreenStart()
        {
            this.InvokeOnScreenStart();

            // Invoke OnScreenStart on child controls
            foreach (UIControl childControl in this.Controls)
            {
                childControl.InternalOnScreenStart();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void MarginChanged()
        {
            this.NeedUpdateLayout = true;
        }


        #region Event lauching
        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnScreenStart()
        {
            if (this.OnScreenStart != null)
            {
                this.OnScreenStart(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnAcceptBegin()
        {
            if (this.OnAcceptBegin != null)
            {
                this.OnAcceptBegin(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnAccept()
        {
            if (this.OnAccept != null)
            {
                this.OnAccept(this);
            }
        }

         /// <summary>
        /// 
        /// </summary>
        internal bool InvokeOnBeforeControlAdded(UIControl control)
        {
            if (this.OnBeforeControlAdded != null)
            {
                return this.OnBeforeControlAdded(this, control);
            }
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnDoubleTapAcceptBegin()
        {
            if (this.OnDoubleTapAcceptBegin != null)
            {
                this.OnDoubleTapAcceptBegin(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnDoubleTapAccept()
        {
            if (this.OnDoubleTapAccept != null)
            {
                this.OnDoubleTapAccept(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnShow()
        {
            if (this.OnShow != null)
            {
                this.OnShow(this);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void InvokeOnFocus()
        {
            if (this.OnFocus != null)
            {
                this.OnFocus(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InvokeOnLostFocus()
        {
            if (this.OnLostFocus != null)
            {
                this.OnLostFocus(this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnHide()
        {
            if (this.OnHide != null)
            {
                this.OnHide(this);
            }
        }
        #endregion

        #region Resolution convertions
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
        protected Size NativeResolution(Size size)
        {
            return new Size(size.Width * (float)BrainGame.NativeScreenWidth / (float)BrainGame.ScreenWidth,
                            size.Height * (float)BrainGame.NativeScreenHeight / (float)BrainGame.ScreenHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        protected BoundingSquare NativeResolution(BoundingSquare bs)
        {
            return new BoundingSquare(this.NativeResolution(bs.UpperLeft), 
                                      this.NativeResolution(new Vector2(bs.Width, bs.Height)));
        }

        /// <summary>
        /// 
        /// </summary>
        protected float NativeResolutionY(float val)
        {
            return (val * (float)BrainGame.NativeScreenHeight / (float)BrainGame.ScreenHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        protected float NativeResolutionX(float val)
        {
            return (val * (float)BrainGame.NativeScreenWidth / (float)BrainGame.ScreenWidth);
        }

        #endregion

        #region Unit conversions


        /// <summary>
        /// Converts Point to screen units
        /// </summary>
        public float ScreenUnitToPixelsX(float val)
        {
            return (val * (float)BrainGame.ScreenWidth / (float)UIScreen.MAX_SCREEN_WITDH_IN_POINTS);
        }

        /// <summary>
        /// Converts Point to screen units
        /// </summary>
        public float ScreenUnitToPixelsY(float val)
        {
            return (val * (float)BrainGame.ScreenHeight / (float)UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS);
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
            return (val * (float)UIScreen.MAX_SCREEN_WITDH_IN_POINTS / (float)BrainGame.ScreenWidth);
        }

        /// <summary>
        /// Converts float pixel to screen units Y axis
        /// </summary>
        public float PixelsToScreenUnitsY(float val)
        {
            return (val * (float)UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS / (float)BrainGame.ScreenHeight);
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
    }
}
