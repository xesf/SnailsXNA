using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Localization;
using System.IO;
using TwoBrainsGames.BrainEngine.Player;
#if DEBUG
using TwoBrainsGames.BrainEngine.Debugging;
#endif
#if WIN8
using Windows.Storage;
#endif
#if BETA_TESTING
using TwoBrainsGames.BrainEngine.Beta;
using TwoBrainsGames.BrainEngine.Windows.Forms;
#endif

namespace TwoBrainsGames.BrainEngine
{

    public class BrainGame : Microsoft.Xna.Framework.Game, IDisposable
    {
        public const string BrainTeamName = "2BrainsGames";
        internal static string Ek = "changeme";

        #region Events
        public event EventHandler OnLanguageChanged;
        public event EventHandler OnGameActivated;
        #endregion

        #region Vars
        internal static BrainGame Instance;
        private ScreenNavigator _screenNavigator;
        protected string _contentRootDir;
        protected SampleManager _sampleManager;
        protected MusicManager _musicManager;
        private ResourceManager _resourceManager;
        protected AchievementsManager _achievementsManager;
        //protected RemoteServicesManager _remoteServicesManager;
        protected Camera2D _defaultCamera;
        protected BrainGameTime _gameTime = new BrainGameTime();
        protected BrainSettings _settings;
        protected Random _rand;
        protected Texture2D _rectTexture;
        protected LineBatch _lineBatch;
        protected Camera2D _activeCamera;
        protected GraphicsDeviceManager _graphicsManager;
        protected Viewport _viewport;
        protected float _viewportRatioX;
        protected float _viewportRatioY;
        protected Rectangle _screenRectangle;
        protected BasicEffect _renderEffect; // Needed because if the viewport does not match the screen resolution
        // (it might be the case if we are on fullscreen) we have to set a custom
        // projection matrix
        SpriteBatch _spriteBatch;
        private IHddIndicator _hddAccessIcon;

        private SamplerState _sampler2D;
        private SamplerState _currentSampler;
        private PlayerIndex _currentControllerIndex;
        private Color _clearColor;
        private bool _screenModeChanged; // Screen mode changed (fullscreen/windowed)
        private bool _displayHDDAccessIcon;
        private Rectangle _viewportRectangle;
        private string _betaName;
        private string _gameName;
        private string _gameFolderName;
		private static string _mainGameFolder;
        private LanguageCode _currentLanguage;
#if DEBUG || TROUBLESHOOT
        SpriteFont _debugFont;
#endif
#if DEBUG
        protected DebugInfo _debugInfo;
#endif
        //private SpriteFont _gameVersionFont;
        //private string _gameVersionString;
        protected bool _accessingStorage;
        private int _nativeScreenWidth;
        private int _nativeScreenHeight;
		private int _presentationNativeScreenWidth;
		private int _presentationNativeScreenHeight;
        public static bool IsLoading;

#if TROUBLESHOOT
        System.Exception _lastExceptionThrown;
#endif
        #endregion

        #region Properties
#if DEBUG || TROUBLESHOOT
        public static SpriteFont DebugFont { get { return Instance._debugFont; } }
#endif
        public static int ScreenWidth { get { return Instance._graphicsManager.PreferredBackBufferWidth; } }
        public static int ScreenHeight { get { return Instance._graphicsManager.PreferredBackBufferHeight; } }
        public static int NativeScreenWidth { get { return Instance._nativeScreenWidth; } set { Instance._nativeScreenWidth = value; } }
        public static int NativeScreenHeight { get { return Instance._nativeScreenHeight; } set { Instance._nativeScreenHeight = value; } }
		public static int PresentationNativeScreenWidth { get { return Instance._presentationNativeScreenWidth; } set { Instance._presentationNativeScreenWidth = value; } }
		public static int PresentationNativeScreenHeight { get { return Instance._presentationNativeScreenHeight; } set { Instance._presentationNativeScreenHeight = value; } }
        public static float ViewportRatioX { get { return Instance._viewportRatioX; } }
        public static float ViewportRatioY { get { return Instance._viewportRatioY; } }
        public static GraphicsDevice Graphics { get { return Instance._graphicsManager.GraphicsDevice; } }
        public static GraphicsDeviceManager GraphicsManager { get { return Instance._graphicsManager; } }
        public static SpriteBatch SpriteBatch { get { return Instance._spriteBatch; }  }
        public static Camera2D DefaultCamera { get { return Instance._defaultCamera; } }
        public static Viewport Viewport { get { return Instance._viewport; } }
        public static BasicEffect RenderEffect { get { return Instance._renderEffect; } set { Instance._renderEffect = value; } }
        public static SampleManager SampleManager { get { return Instance._sampleManager; } }
        public static MusicManager MusicManager { get { return Instance._musicManager; } }
        public static ResourceManager ResourceManager { get { return Instance._resourceManager; } }
        public static AchievementsManager AchievementsManager { get { return Instance._achievementsManager; } }
        //public static RemoteServicesManager RemoteServicesManager { get { return Instance._remoteServicesManager; } }
        public static bool IsGameActive { get { return Instance.IsActive; } }
        public static bool IsTrial { get { return (Instance._settings.GameplayMode == BrainSettings.GameplayModeType.Demo); } }

        public static ScreenNavigator ScreenNavigator { get { return Instance._screenNavigator; } }
        public static BrainGameTime GameTime { get { return Instance._gameTime; } }
        
        public static bool IsFullscreen { get { return Instance._graphicsManager.IsFullScreen; } }
        public static bool PreferMultiSampling { get { return Instance._graphicsManager.PreferMultiSampling; } set { Instance._graphicsManager.PreferMultiSampling = value; } }

        
		#if DEBUG_INFO
        public static DebugInfo DebugInfo { get { return Instance._debugInfo; } }
        #endif
        public static IHddIndicator HddAccessIcon { get { return Instance._hddAccessIcon; } set { Instance._hddAccessIcon = value; } }
        public static Cursor GameCursor { get { return Instance._gameCursor; } set { Instance._gameCursor = value; } }
        public static IntPtr WindowHandle { get { return Instance.Window.Handle; } }
        public static SamplerState Sampler2D { get { return Instance._sampler2D; } }
        public static SamplerState CurrentSampler { get { return Instance._currentSampler; } }
        public static PlayerIndex CurrentControllerIndex { get { return Instance._currentControllerIndex; } set { Instance._currentControllerIndex = value; } }
        public static Color ClearColor { get { return Instance._clearColor; } set { Instance._clearColor = value; } }
        public static bool DisplayHDDAccessIcon { get { return Instance._displayHDDAccessIcon; } set { Instance._displayHDDAccessIcon = value; } }
        public static Rectangle ViewportRectangle { get { return Instance._viewportRectangle; } }
        public static Rectangle ScreenRectangle { get { return Instance._screenRectangle; } }
        public static string BetaName { get { return Instance._betaName; } protected set { Instance._betaName = value; } }
        public static string GameName { get { return Instance._gameName; } protected set { Instance._gameName = value; } }
        public static string GameFolderName { get { return Instance._gameFolderName; } protected set { Instance._gameFolderName = value; } }
         public static string GameUserFolderName 
        { 
            get 
            {
				if (!string.IsNullOrEmpty(_mainGameFolder))
				{
					return _mainGameFolder;
				}
#if !WIN8 && !PSMobile
				// TODO: add this in configuration file
				Environment.SpecialFolder folderType = Environment.SpecialFolder.ApplicationData;
#if MONOMAC || IOS
				folderType = Environment.SpecialFolder.Personal;
#endif

                if (Environment.GetFolderPath(folderType) == null)
                {
                    throw new BrainException("Environment.SpecialFolder." + folderType.ToString() + " not defined!! Note that this property might not be available in all platforms.");
                }
                
				string mainFolder = Environment.GetFolderPath(folderType);

				// ugly but don't seam to exist an elangant way to do this in MacOSX
#if MONOMAC || IOS
				if (mainFolder.Contains("/Documents"))
				{
					mainFolder = mainFolder.Replace("/Documents","");
				}
				mainFolder += "/Library/Application Support/";
#endif
				string teamFolder = Path.Combine(mainFolder, BrainGame.BrainTeamName);
				_mainGameFolder = Path.Combine(teamFolder, BrainGame.GameFolderName);

				// create directory if not exist
				if(!Directory.Exists(_mainGameFolder)) 
				{
					Directory.CreateDirectory(_mainGameFolder);
				}
#else 
				_mainGameFolder = "";
#if WIN8
                _mainGameFolder = ApplicationData.Current.LocalFolder.Path;
#endif
#endif
				return _mainGameFolder;
            } 
        }

        public Camera2D ActiveCamera
        {
            get { return _activeCamera; }
            set { _activeCamera = value; }
        }

        private Cursor _gameCursor;
        protected bool IsExiting { get; private set; }
        public static LanguageCode CurrentLanguage
        {
            get { return Instance._currentLanguage; }
            set
            {
                Instance._currentLanguage = value;
                LanguageManager.LanguageChanged();
                if (Instance._screenNavigator != null)
                {
                    if (BrainGame.Instance.OnLanguageChanged != null)
                    {
                        BrainGame.Instance.OnLanguageChanged(BrainGame.Instance, new EventArgs());
                    }
                    Instance._screenNavigator.LanguageChanged();
                }
            }
        }
#if STORE
        private static TwoBrainsGames.BrainEngine.Store.Rate _rating;
		public static TwoBrainsGames.BrainEngine.Store.Rate Rating 
		{ 
			get  
			{ 	

				if (_rating == null) {
					_rating = TwoBrainsGames.BrainEngine.Store.Rate.Create ();
				}
				return _rating; 
			} 
		}
#endif

#endregion

#region Static Properties and Methods
        public static Random Rand
        {
            get { return Instance._rand; }
        }

        public static Texture2D RectTexture
        {
            get { return Instance._rectTexture; }
        }

        public static LineBatch LineBatch
        {
            get { return Instance._lineBatch; }
        }

        public static void DrawRectangleFilled(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(Instance._rectTexture, rectangle, color);
        }

        public static void DrawLine(Vector2 pt1, Vector2 pt2, Color color)
        {
            DrawLine(pt1, pt2, color, 1.0f);
        }

        public static void DrawLine(Vector2 pt1, Vector2 pt2, Color color, float layerDepth)
        {
            Instance._lineBatch.Batch(pt1, pt2, color, layerDepth);
        }

        public static void DrawRectangleFrame(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (lineWidth == 0)
                lineWidth = 1;

            spriteBatch.Draw(Instance._rectTexture, new Rectangle(rectangle.Left, rectangle.Top, lineWidth, rectangle.Height), color);   // Left
            spriteBatch.Draw(Instance._rectTexture, new Rectangle(rectangle.Right, rectangle.Top, lineWidth, rectangle.Height), color);  // Right
            spriteBatch.Draw(Instance._rectTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, lineWidth), color);    // Top
            spriteBatch.Draw(Instance._rectTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, lineWidth), color); // Bottom
        }

        public static BrainSettings Settings
        {
            get { return Instance._settings; }
            set { Instance._settings = value; }
        }


#endregion

        /// <summary>
        /// 
        /// </summary>
        public BrainGame()
        {
            BrainGame.Instance = this;
            this._graphicsManager = new GraphicsDeviceManager(this);
#if DEBUG && APP_STORE_SUPPORT
            Guide.SimulateTrialMode = true;
#endif

//#if MONOGAME
//			this.Activated += OnActivated;
//			this.Deactivated += OnDeactivated;
//#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Screen CreateScreen(ScreenNavigator owner, string screenId)
        {
            throw new BrainException("Override BrainGame.CreateScreen() in yout Game class to create Screen instances.");
        }

        /// <summary>
        /// Called by BrainGame.Initialize()
        /// </summary>
        protected virtual void LoadSettings()
        {
        }

#region Overrides
        /// <summary>
        /// 
        /// </summary>
        protected override void Initialize()
        {
            this._contentRootDir = "Content";
            this._resourceManager = new ResourceManager(this.Services, this._contentRootDir);
            this.LoadSettings();

/*#if TRACE
            BETrace.SetTraceToFile(Path.Combine(BrainGame.GameUserFolderName, "braingame.log"), 1024);
#endif*/

#if BETA
            if (this.InitializeBeta() == false)
            {
                return;
            }
#endif

#if GAMER_SERVICES
            this.Components.Add(new GamerServicesComponent(this));
#endif
            this._sampleManager = new SampleManager(this);
            this.Components.Add(this._sampleManager);
            this._musicManager = new MusicManager(this);
            this.Components.Add(this._musicManager);

            if (this._settings.UseAchievements)
            {
                this._achievementsManager = new AchievementsManager();
                this._achievementsManager.Load("achievements");
            }

            //this._remoteServicesManager = new RemoteServicesManager();
            //this._remoteServicesManager.ApiKey = BrainGame.Settings.RemoteServicesAPIKey;
            //this._remoteServicesManager.GameId = BrainGame.Settings.RemoteServicesGameId;

            this._activeCamera = new Camera2D();
            this._rand = new Random(DateTime.Now.Millisecond);
            this._rectTexture = new Texture2D(this._graphicsManager.GraphicsDevice, 1, 1);
            this._rectTexture.SetData(new Color[] { Color.White });
            this._lineBatch = new LineBatch(this._graphicsManager.GraphicsDevice, 1.0f);

#if DEBUG_INFO
            this._debugInfo = new DebugInfo(this, new Vector2(0, 0));
            this.Components.Add(this._debugInfo);
            this._debugInfo.Visible = this._settings.ShowDebugInfo;
#endif
#if DEBUG || TROUBLESHOOT
            this._debugFont = BrainGame.ResourceManager.Load<SpriteFont>("fonts/debug", Resources.ResourceManager.ResourceManagerCacheType.Static);
#endif

            this._screenNavigator = new ScreenNavigator(this);
            this._screenNavigator.LoadContent();
            this._spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.Setup2DTextureSampling();
            this._currentSampler = BrainGame.Sampler2D;
            this._currentControllerIndex = PlayerIndex.One;

            this._clearColor = Color.Black;

            this._currentLanguage = LanguageManager.GetDefaultSystemLanguage();

            //TwoBrainsGames.BrainEngine.RemoteServices.Network.TestInternetAsync();

            this.OnInitialize();

            // This should be reviewed. BrainGame will crash if this content was not added by the child game
            base.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
#if TROUBLESHOOT
            try
            {
#endif
                base.Update(gameTime);
#if TROUBLESHOOT
                if (this._lastExceptionThrown != null)
                {
                    return;
                }
#endif

#if MONOGAME
                // Há algo estranho com o monogame. Ele está a alterar o viewport depois de arrancar
                // Forçamos sempre ao nosso viewport
                BrainGame.GraphicsManager.GraphicsDevice.Viewport = BrainGame.Instance._viewport;
#endif
                if (this._accessingStorage)
                {
                    return;
                }

                if (this.IsExiting)
                {
                    return;
                }

                // Screen mode changed? Rebuild viewport and projection matrix (because they are reset when view mode changes)
                if (this._screenModeChanged)
                {
                    this.SetupRenderViewport();
                    this.SetupProjectionMatrix();
                    this._screenModeChanged = false;
                    this._screenNavigator.SendScreenModeChangedToScreens();
                }
#if DEBUG_INFO
                this._debugInfo.Visible = this._settings.ShowDebugInfo; // Don't remove this because this._settings.ShowFPS could be changed after Initialize
                this._debugInfo.UpdateStarted();
#endif
                this._gameTime.Update(gameTime);

                //this._remoteServicesManager.Update(this._gameTime);
                this._screenNavigator.Update(this._gameTime);

#if DEBUG_INFO
                this._debugInfo.UpdateEnded();
#endif

#if TROUBLESHOOT
            }
            catch(System.Exception ex)
            {
                if (this._lastExceptionThrown == null)
                {
                    this._lastExceptionThrown = ex;
                }
                else
                {
                    throw;
                }
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
#if TROUBLESHOOT
            try
            {
#endif
            if (this._screenModeChanged) // Screen mode changed (fullscreen/windowed) don't draw nothing until the projection matrix and viewport have been reset
            {
                return;
            }
#if DEBUG_INFO
            this._debugInfo.DrawStarted();
#endif
#if TROUBLESHOOT
            if (this._lastExceptionThrown != null)
            {
                base.Draw(gameTime);
                this.GraphicsDevice.Clear(Color.Black);
                SpriteBatch.Begin();
                SpriteBatch.DrawString(this._debugFont, this._lastExceptionThrown.ToString(), Vector2.Zero, Color.White);
                SpriteBatch.End();
                return;
            }
#endif
            //this.GraphicsDevice.Clear(Color.Black);
            this._screenNavigator.Draw();
            base.Draw(gameTime);

#if DEBUG
            if (this._settings.ShowResourceManagerData && 
                ! BrainGame.IsLoading)
            {
                this._spriteBatch.Begin();
                this._resourceManager.TraceLoadedResources(this._spriteBatch, this._debugFont);
                this._spriteBatch.End();
             //   BrainGame.ResourceManager.Unlock();
            }
#if DEBUG_INFO
            this._debugInfo.DrawEnded();
#endif
#endif
#if TROUBLESHOOT
            }
            catch (System.Exception ex)
            {
                if (this._lastExceptionThrown == null)
                {
                    this._lastExceptionThrown = ex;
                }
                else
                {
                    throw;
                }
            }
#endif
        }

        protected override void OnActivated(object sender, EventArgs args)
		{
			base.OnActivated(sender, args);
            BrainActivated(sender, args);
        }
		
        protected override void OnDeactivated(object sender, EventArgs args)
		{
			base.OnDeactivated(sender, args);
            BrainDeactivated(sender, args);
        }

		private void BrainActivated(object sender, EventArgs args)
		{
            if (this._settings != null &&
				this._settings.WindowAlwaysActive)
                return;

            if (this._screenNavigator != null)
            {
                this._screenNavigator.GameWindowActivated();
            }

            if (this.OnGameActivated != null)
            {
                this.OnGameActivated(this, new EventArgs());
            }
        }
            
		private void BrainDeactivated(object sender, EventArgs args)
        {
            if (this._settings.WindowAlwaysActive)
                return;

            if (this._screenNavigator != null)
            {
                this._screenNavigator.GameWindowDeactivated();
            }
        }

        public void Quit()
        {
            this.IsExiting = true;
            this.Exit();
        }

        /// <summary>
        /// I have no idea if this should be done like this but it realy doesn't matter. I just want to exit from the main menu
        /// we'll see later
        /// </summary>
        public static void QuitGame()
        {
            BrainGame.Instance.Quit();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            // It's not garranteed this objects are created
            if (this._musicManager != null)
            {
                this._musicManager.StopMusic();
            }
            if (this._sampleManager != null)
            {
                this._sampleManager.StopAll();
            }
            if (this._resourceManager != null)
            {
                this._resourceManager.Unload();
            }
        }

#endregion
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnInitialize()
        { }

        /// <summary>
        /// 
        /// </summary>
        private void CreateAppFolders()
        {
#if !WIN8
            if (!Directory.Exists(BrainGame.GameUserFolderName))
            {
                Directory.CreateDirectory(BrainGame.GameUserFolderName);
            }
#endif
        }

        /// <summary>
        /// Set the texture sampling to avoid half pixels problems that cause texture blur
        /// </summary>
        private void Setup2DTextureSampling()
        {
            this._sampler2D = new SamplerState();
            this._sampler2D.AddressU = TextureAddressMode.Clamp;
            this._sampler2D.AddressV = TextureAddressMode.Clamp;
            this._sampler2D.AddressW = TextureAddressMode.Clamp;
            this._sampler2D.Filter = TextureFilter.Anisotropic;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetupProjectionMatrix()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void SetupRenderViewport()
        {
        }

        public static void AddComponent(GameComponent component)
        {
            BrainGame.Instance.Components.Add(component);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetViewport(Viewport viewport)
        {
            BrainGame.Instance._viewport = new Viewport(viewport.Bounds);
            BrainGame.GraphicsManager.GraphicsDevice.Viewport = BrainGame.Instance._viewport; // viewport;
            BrainGame.GraphicsManager.ApplyChanges();
            BrainGame.Instance._viewportRectangle = new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ResetGameTime()
        {
            BrainGame.Instance._gameTime.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ToggleFullScreen()
        {
            BrainGame.GraphicsManager.ToggleFullScreen();
            BrainGame.Instance._screenModeChanged = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static ResourceManager CreateResourceManager()
        {
            return new ResourceManager(BrainGame.Instance.Services, BrainGame.Instance._contentRootDir);
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void ProcessException(System.Exception ex)
        {

            // Don't let this method launch exceptions because it is used inside Catch statements
            try
            {
#if BETA_TESTING
                ClosedBeta.LogException(ex);
#endif
                // Restore windowed mode if needed, this is because MessageBox will not be visible in fullscreen
                if (this._graphicsManager != null &&
                    this._graphicsManager.IsFullScreen)
                {
                    this._graphicsManager.IsFullScreen = false;
                    this._graphicsManager.ApplyChanges();
                }
            }
            catch (System.Exception)
            {
            }
#if FORMS_SUPPORT
            System.Windows.Forms.MessageBox.Show(ex.ToString());
#else
            throw ex; // No forms support? Just launch exception, this will be catched by the running system
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private bool InitializeBeta()
        {
#if BETA_TESTING

            if (ClosedBeta.Initialize() == false)
            {
                this.Quit();
                return false;
            }

            /* 	SnailsStageStats stats = new SnailsStageStats();
              stats.Status = (int)0;
              stats.Theme = "teste";
              stats.StageNumber = 1;
              stats.Goldcoins = 0;
              stats.Silvercoins = 0;
              stats.Bronzecoins = 0;
              stats.Score = 0;
              stats.Unusedtools = 0;
              stats.TimeStarted = DateTime.Now;
              stats.TimeTaken = DateTime.Now;
              ClosedBeta.LogInfo("Teste datas outra vez");
              ClosedBeta.RegisterStageStats(stats);
           ClosedBeta.LogInfo("Teste2");
              ClosedBeta.LogInfo("Teste3");
              ClosedBeta.LogInfo("Teste4");
              ClosedBeta.LogInfo("Teste5");

              for (int i = 0; i < 1000; i++)
              {
                  Thread.Sleep(150);
              }
               Thread.Sleep(10000);*/
#endif
            return true;
        }

        /// <summary>
        ///  Randomizes a float number between minValue and maxValue with n decimals
        /// </summary>
        public static float RandomizeFloat(float minValue, float maxValue, int decimals)
        {
            int randNumber = (int)((maxValue - minValue) * 100 * decimals);
            return minValue + (float)(BrainGame.Rand.Next(randNumber) / (float)decimals / 100f);
        }
    }
}
