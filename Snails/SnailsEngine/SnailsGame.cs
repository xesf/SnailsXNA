using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Screens;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.ThemeSelection;
using TwoBrainsGames.Snails.Tutorials;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.UI;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.Snails.StageObjects;
#if APP_STORE_SUPPORT
using Microsoft.Phone.Tasks;
#endif
#if FORMS_SUPPORT
using TwoBrainsGames.Snails.Winforms;
using TwoBrainsGames.BrainEngine.Windows.Forms;

#endif
#if DEBUG && WIN8
using TwoBrainsGames.Snails.Debuging;
#endif

namespace TwoBrainsGames.Snails
{
   
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SnailsGame : BrainGame, IDisposable
    {
        internal static string Ek = "changeme";

        #region Members

		public static SnailsGame _instance;
 
#if DEBUG
        SpriteFont _DebugFont { get; set; }
#endif
        internal MainMenuScreen MenuScreen { get; set; }
        

        PlayersProfileManager _profilesManager;
        Tutorial _tutorial;
        Music _themeMusic;
        List<string> _footerMessages;
#if DEBUG
        SpriteFont ObjectIdsFont;
#endif
#if DEBUG && WIN8
        DebugWindow _DebugWidow;
#endif
        #endregion

        #region Static properties
        public static bool AchievementRetryEnabled { get; set; }
        public static SnailsGame Instance { get { return _instance; } }
        public static Rectangle SafeArea { get { return _instance.GraphicsDevice.Viewport.TitleSafeArea; } }

        internal static GameSettings GameSettings { get { return (GameSettings)_instance._settings; } set { _instance._settings = value; } }
        internal static PlayersProfileManager ProfilesManager { get { return _instance._profilesManager; } }
        internal static Tutorial Tutorial { get { return _instance._tutorial; } }
        internal static Music ThemeMusic { get { return _instance._themeMusic; } set { _instance._themeMusic = value; } }
        internal static List<string> FooterMessages { get { return _instance._footerMessages; } }
#if DEBUG
        internal static SpriteFont ObjectsIdFont { get { return _instance.ObjectIdsFont; } }
#endif
#if DEBUG && WIN8
        internal static DebugWindow DebugWidow { get { return _instance._DebugWidow; } }
#endif
#endregion


        /// <summary>
        /// 
        /// </summary>
        public SnailsGame()
        {
            _instance = this;
            BrainGame.GameFolderName = "Snails";
        }

        /// <summary>
        /// Called by BrainGame.Initialize()
        /// </summary>
        protected override void LoadSettings()
        {
            this._settings = new GameSettings();
            this._settings.Load(GenericConsts.SettingsResourceName);

#if DEBUG && MONOMAC
			this._settings.ShowResourceManagerData = false;
			this._settings.UseAsyncLoading = false;
#endif

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
		protected override void OnInitialize()
        {


#if APP_STORE_SUPPORT
#if DEBUG
            Guide.SimulateTrialMode = true;
#endif
            this.UpdateGamePlayMode();
#endif

#if DEBUG
            // QUery game settings 
            if (SnailsGame.GameSettings.ShowGameSettingsWindow)
            {
                this.SelectGameSettings();
            }
#endif
            this.InitializeProfile();

            // EULA Query
            if (SnailsGame.GameSettings.ShowEULA)
            {
                if (this.EULAAgreement() == false)
                {
                    this.Quit();
                    return;
                }
            }

            // Screen mode : fullscreen/normal
            if (SnailsGame.GameSettings.AllowToggleFullScreen)
            {
                this.QueryScreenMode();
            }


            this.IsFixedTimeStep = false; // Settings.FPSLocked;
			_graphicsManager.PreferredBackBufferWidth = BrainGame.PresentationNativeScreenWidth; // SnailsGame.GameSettings.ScreenWidth;
			_graphicsManager.PreferredBackBufferHeight = BrainGame.PresentationNativeScreenHeight; // SnailsGame.GameSettings.ScreenHeight;
            _graphicsManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            _graphicsManager.SupportedOrientations = DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft;
            // Screen mode : fullscreen / normal
			_graphicsManager.IsFullScreen = SnailsGame.GameSettings.IsFullScreen;
            if (SnailsGame.ProfilesManager.CurrentProfile != null &&
                SnailsGame.GameSettings.AllowToggleFullScreen)
            {
                _graphicsManager.IsFullScreen = SnailsGame.ProfilesManager.CurrentProfile.Fullscreen;
            }

            _graphicsManager.SynchronizeWithVerticalRetrace = SnailsGame.GameSettings.UseVSync;
            _graphicsManager.ApplyChanges();

			this.SetupRenderViewport();
			this.SetupProjectionMatrix();

#if DEBUG
            this._DebugFont = BrainGame.ResourceManager.Load<SpriteFont>("fonts/FPS", ResourceManager.ResourceManagerCacheType.Static);
            this.ObjectIdsFont = BrainGame.ResourceManager.Load<SpriteFont>("fonts/objectsId", ResourceManager.ResourceManagerCacheType.Static);
#endif
#if DEBUG && WIN8
            if (SnailsGame.GameSettings.AllowDebugWindow)
            {
                this._DebugWidow = new DebugWindow(this);
                this.Components.Add(this._DebugWidow);
                this._DebugWidow.Visible = false;
            }     
#endif

            this.IsMouseVisible = false;
            _defaultCamera = new Camera2D();
            _defaultCamera.Initialize();


            // Add sprite batch to Services list so the Engine could use it if needed.
            Services.AddService(typeof(SpriteBatch), BrainGame.SpriteBatch);

#if DEBUG && !RETAIL
            this._DebugFont = BrainGame.ResourceManager.Load<SpriteFont>("fonts/Debug", ResourceManager.ResourceManagerCacheType.Static);
#endif

            this._lineBatch = new LineBatch(this._graphicsManager.GraphicsDevice, 1.0f); // init line batch with the right viewport
            this._tutorial = BrainGame.ResourceManager.Load<Tutorial>("tutorials/tutorial", BrainEngine.Resources.ResourceManager.ResourceManagerCacheType.Static);


            BrainGame.HddAccessIcon = new Snails.HddAccessIcon();
            BrainGame.HddAccessIcon.LoadContent();
            BrainGame.DisplayHDDAccessIcon = false;
            BrainGame.ScreenNavigator.UseAssyncGroupLoading = SnailsGame.GameSettings.UseAsyncLoading;
            BrainGame.ClearColor = Color.Black;

            // Load mouse cursors - in windows this should be preloaded because they are 
            // bitmaps converted to windows cursors

            // Cursors are becoming a big mess... This should be in the BrainGame base
            BrainGame.GameCursor = new SoftwareCursor();


            if (BrainGame.GameCursor is SoftwareCursor)
            {
                BrainGame.GameCursor.LoadCursor(SpriteResources.PLAYER_CURSOR_DEFAULT, GameCursors.Default);
                BrainGame.GameCursor.LoadCursor(SpriteResources.PLAYER_CURSOR_BUSY, GameCursors.Busy);
                BrainGame.GameCursor.LoadCursor(SpriteResources.PLAYER_CURSOR_FORBIDDEN, GameCursors.Forbidden);
                BrainGame.GameCursor.LoadCursor(SpriteResources.PLAYER_CURSOR_SALT, GameCursors.Salt);
                BrainGame.GameCursor.LoadCursor(SpriteResources.PLAYER_CURSOR_SALT_FORBIDDEN, GameCursors.SaltForbidden);
                BrainGame.GameCursor.LoadCursor(SpriteResources.PLAYER_CURSOR_OUTOFSTOCK, GameCursors.OutOfStock);
                BrainGame.GameCursor.LoadCursor(SpriteResources.PLAYER_CURSOR_CAM_PAN, GameCursors.MapPanCursor);
                BrainGame.GameCursor.Visible = false;
                BrainGame.GameCursor.SetCursor(GameCursors.Default);
            }
            
            BrainGame.ScreenNavigator.NavigateTo(GameSettings.StartupScreenGroup, GameSettings.StartupScreen);
            ScreenTransitions.Initialize();
            StageObjectFactory.Initialize();
            
            // Create custom content manager to hold specific groups of contents
            // The unload/load of this type of contents must be managed by the game, not the engine
            BrainGame.ResourceManager.CreateUserDefinedResourceManager(ResourceManagerIds.STAGE_THUMBNAILS);
            BrainGame.ResourceManager.CreateUserDefinedResourceManager(ResourceManagerIds.STAGE_THEME_RESOURCES);
            BrainGame.ResourceManager.CreateUserDefinedResourceManager(ResourceManagerIds.TUTORIAL_RESOURCES);
            BrainGame.ResourceManager.CreateUserDefinedResourceManager(ResourceManagerIds.PAID_SNAILS_AD);

            this.InitializeFooterMessages();
            this.OnLanguageChanged += new EventHandler(SnailsGame_OnLanguageChanged);
            this.OnGameActivated += new EventHandler(SnailsGame_OnGameActivated);
			//TwoBrainsGames.BrainEngine.RemoteServices.Network.TestInternetAsync ();

			// Rate system initialization
			//BrainGame.Rating.OnUserAskedToRateLater += SnailsGame_OnUserAskedToRateLater;
			//BrainGame.Rating.OnUserDeclinedGameRate += SnailsGame_OnUserDeclinedGameRate;
			//BrainGame.Rating.OnUserRatedGame += SnailsGame_OnUserRatedGame;
			BrainGame.ClearColor = Color.White;
			AchievementRetryEnabled = true;
			/*	testes
			SnailsGame.AchievementsManager.Notify((int)AchievementsType.SafelyEscort100Snails);
			SnailsGame.AchievementsManager.Notify((int)AchievementsType.SafelyEscort300Snails);
			SnailsGame.AchievementsManager.Notify((int)AchievementsType.SafelyEscort600Snails);
			*/
        }
		const double ACHIEVEMENT_RETRY_TIME = 5000;
		double _achievementRetryTime = ACHIEVEMENT_RETRY_TIME; // deixamos no maximo assim o 1? salta logo no arranque do jogo
		/// <summary>
		/// 
		/// </summary>
		protected override void Update(GameTime gameTime)
		{
			base.Update (gameTime);
            // Retry do envio de achievements
#if GAMESERVICE
            if (AchievementRetryEnabled && 
				SnailsGame.ProfilesManager.CurrentProfile.AchievementsNotSent.Count > 0 &&
				Gamer.SignedInGamers.Count > 0 &&
				Gamer.SignedInGamers [0].IsSignedInToLive)
			{
				_achievementRetryTime += gameTime.ElapsedGameTime.TotalMilliseconds;
				if (_achievementRetryTime >= ACHIEVEMENT_RETRY_TIME) 
				{
					int id = SnailsGame.ProfilesManager.CurrentProfile.AchievementsNotSent[0];
					SnailsGame.ProfilesManager.CurrentProfile.AchievementsNotSent.RemoveAt(0);
					Achievements.AchievementEarned(id);
					_achievementRetryTime = 0;
				}
			}
#endif
		}

        /// <summary>
        /// 
        /// </summary>
        private void UpdateGamePlayMode()
        {
#if APP_STORE_SUPPORT
            SnailsGame.GameSettings.GameplayMode = (Guide.IsTrialMode ? BrainSettings.GameplayModeType.Demo : BrainSettings.GameplayModeType.Retail);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        void SnailsGame_OnGameActivated(object sender, EventArgs e)
        {
            this.UpdateGamePlayMode();
            if (SnailsGame.ScreenNavigator != null)
            {
                ScreenType scrType = SnailsGame.ScreenNavigator.GlobalCache.Get<ScreenType>(GlobalCacheKeys.CURRENT_SCREEN, ScreenType.None);
                if (scrType == ScreenType.Purchase)
                {
                    SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleAndMenuVisible);
                    SnailsGame.ScreenNavigator.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), null, null);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void SnailsGame_OnLanguageChanged(object sender, EventArgs e)
        {
            this.InitializeFooterMessages();    
        }

        /// <summary>
        /// To be called outside so we can update according with language
        /// </summary>
        public void InitializeFooterMessages()
        {
            if (this._footerMessages == null)
                this._footerMessages = new List<string>();
            else
                this._footerMessages.Clear();

          //  this._footerMessages.Add(LanguageManager.GetString("MSG_FOOTER_COPYRIGHT"));
          //  this._footerMessages.Add(LanguageManager.GetString("MSG_FOOTER_DEVELOPED_BY"));
            this._footerMessages.Add(LanguageManager.GetString("MSG_FOOTER_GET_WP_VERSION"));
        }

        /// <summary>
        /// 
        /// </summary>
        private void QueryScreenMode()
        {
#if FORMS_SUPPORT
            if (SnailsGame.ProfilesManager.CurrentProfile.ScreenModeSelected)
            {
                return;
            }

            ScreenModeForm form = new ScreenModeForm();
            System.Windows.Forms.DialogResult diagResult = form.ShowDialog();
            if (diagResult == System.Windows.Forms.DialogResult.OK)
            {
                SnailsGame.ProfilesManager.CurrentProfile.ScreenModeSelected = true;
                SnailsGame.ProfilesManager.CurrentProfile.Fullscreen = SnailsGame.GameSettings.IsFullScreen;
                SnailsGame.ProfilesManager.Save();
            }
            else 
            {
                this.Quit(); // if window is closed, just quit the game
            }
#else
            //throw new SnailsException("QueryScreenMode is not valid in non Forms application.");
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeProfile()
        {
            this._profilesManager = PlayersProfileManagerFactory.Create();
#if DEBUG
            if (SnailsGame.GameSettings.DeletePlayerProfile)
            {
                this._profilesManager.DeleteProfile();
            }
#endif
            this._profilesManager.BeginLoad();
        }

#if XBOX
        /// <summary>
        /// Only used because the asyncronous Xbox storage read
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void  Update(GameTime gameTime)
        {
            if (!this._profilesManager.IsCompleted)
            {
                this._accessingStorage = true;
                this._profilesManager.Update();
                base.Update(gameTime);
                return;
            }
            this._accessingStorage = false;
            base.Update(gameTime);
        }
#endif

#region Private methods

        /// <summary>
        /// 
        /// </summary>
        public override void SetupRenderViewport()
        {

            // Calcular AR do ?cran (janela se n?o for fullscreen, ou do ?cran todo em fullscreen)
            float aspectRatioScreen = ((float)this._graphicsManager.GraphicsDevice.Viewport.Width /
                                       (float)this._graphicsManager.GraphicsDevice.Viewport.Height);
            // Calcular o AR do jogo
            float aspectRatioGame = ((float)BrainGame.PresentationNativeScreenWidth /
                                     (float)BrainGame.PresentationNativeScreenHeight);

          //  this._graphicsManager.IsFullScreen = false;
           // this._graphicsManager.ApplyChanges();
            // Agora, uma de duas:
            //   1) Ou ajustamos a altura do jogo ? janela/ecran e ficamos com barras laterais pretas
            //   2) Ou ajustamos a largura do jogo ? janela/ecran e ficamos com barras em cima e baixo
            // Isto pode-se ver pela compara??o dos aspect ratios
            

            // Caso 1
            if (aspectRatioGame < aspectRatioScreen)
            {
                // A largura calcula-se com uma simples regra de 3 simples
                float w = (float)this._graphicsManager.GraphicsDevice.Viewport.Height *
                          (float)BrainGame.PresentationNativeScreenWidth / (float)BrainGame.PresentationNativeScreenHeight;
                float x = (float)(this._graphicsManager.GraphicsDevice.Viewport.Width - w) / 2f;
                BrainGame.SetViewport(new Viewport((int)x, 0, (int)w, this._graphicsManager.GraphicsDevice.DisplayMode.Height));
            }
            else // Caso 2
            if (aspectRatioGame > aspectRatioScreen)
            {
                // A altura calcula-se com uma simples regra de 3 simples
                float h = (float)this._graphicsManager.GraphicsDevice.Viewport.Width *
                            (float)BrainGame.PresentationNativeScreenHeight / (float)BrainGame.PresentationNativeScreenWidth;
                // Centramos no ecran no y (no x n?o precisa porque enche o ecran todo
                float y = (float)(this._graphicsManager.GraphicsDevice.Viewport.Height - h) / 2f;

                BrainGame.SetViewport(new Viewport(0, (int)y, this._graphicsManager.GraphicsDevice.Viewport.Width, (int)h));
            }
            else // Igual? Facil, ? s? meter o viewport igual a janela/ecran
            {
                BrainGame.SetViewport(this._graphicsManager.GraphicsDevice.Viewport);
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        protected override void SetupProjectionMatrix()
        {
            this._renderEffect = new BasicEffect(BrainGame.Graphics);
            this._renderEffect.World = Matrix.Identity;
            this._renderEffect.View = Matrix.Identity;

            int dx = 0;
            int dy = 0;
            int wN = BrainGame.PresentationNativeScreenWidth;
            int hN = BrainGame.PresentationNativeScreenHeight;
            int wC = this._graphicsManager.GraphicsDevice.Viewport.Width;  // current device width
            int hC = this._graphicsManager.GraphicsDevice.Viewport.Height; // current device height
            int nW = SnailsGame.GameSettings.ScreenWidth;
            int nH = SnailsGame.GameSettings.ScreenHeight;

            float fN = (float)wN / (float)hN;
            float fC = (float)wC / (float)hC;

            if (fN > fC)
            {
                nW = wC * hN / hC; // new width
                nH = hN;
                dx = Math.Abs((wN - nW) / 2);
            }
            else if (fN <= fC)
            {
                nH = wN * hC / wC; // new height
                nW = wN;
                dy = Math.Abs((hN - nH) / 2);
            }

            // signed a different translation for Windows 8 game. On fullscreen it will appear correctly.
            int signal = 1;
            if (SnailsGame.GameSettings.Platform == BrainSettings.PlaformType.Windows8 ||
                SnailsGame.GameSettings.Platform == BrainSettings.PlaformType.Windows8RT)
            {
                signal = -1;
            }

			nW = BrainGame.PresentationNativeScreenWidth;
			nH = BrainGame.PresentationNativeScreenHeight;
			dx =0;
			dy = 0;
            this._screenRectangle = new Rectangle(dx, dy, SnailsGame.GameSettings.ScreenWidth, SnailsGame.GameSettings.ScreenHeight);
            this._renderEffect.Projection = Matrix.CreateTranslation(-dx - 0.5f * signal, dy - 0.5f * signal, 0) * Matrix.CreateOrthographicOffCenter(0, nW, nH, 0, -1, 1);
            this._renderEffect.TextureEnabled = true;
            this._renderEffect.VertexColorEnabled = true;

            // Correct the aspect ratio used with the mouse input
            _viewportRatioX = _viewportRatioY = 1f;
            /* Isto estava a dar probs no PC sem ser em fullscreen - rever no Mac
            if (_graphicsManager.GraphicsDevice.DisplayMode.Width > BrainGame.PresentationNativeScreenWidth)
                _viewportRatioX = (float)_graphicsManager.GraphicsDevice.DisplayMode.Width / (float)BrainGame.PresentationNativeScreenWidth;
            if (_graphicsManager.GraphicsDevice.DisplayMode.Height > BrainGame.PresentationNativeScreenHeight)
                _viewportRatioY = (float)_graphicsManager.GraphicsDevice.DisplayMode.Height / (float)BrainGame.PresentationNativeScreenHeight;
        
             */
        }
  
#endregion

        /// <summary>
        /// 
        /// </summary>
        public override TwoBrainsGames.BrainEngine.UI.Screens.Screen CreateScreen(ScreenNavigator navigator, string screenId)
        {
            ScreenType type = (ScreenType)Enum.Parse(typeof(ScreenType), screenId, true);
            Screen screen;
            switch (type)
            {
                case ScreenType.Startup:
                    screen = (Screen)new StartupScreen(navigator);
                    break;
		        case ScreenType.BrainsLogo:
                    screen = (Screen)new BrainsLogoScreen(navigator);
                    break;
                case ScreenType.Overscan:
                    screen = (Screen)new OverscanScreen(navigator);
                    break;
                case ScreenType.Options:
                    screen = (Screen)new OptionsScreen(navigator);
                    break;
                case ScreenType.Gameplay:
                    screen = (Screen)new GameplayScreen(navigator);
                    break;
                case ScreenType.Credits:
                    screen = (Screen)new CreditsScreen(navigator);
                    break;
                case ScreenType.DebugOptions:
                    screen = (Screen)new DebugOptionsScreen(navigator);
                    break;
                case ScreenType.InGameOptions:
                    screen = (Screen)new InGameOptionsScreen(navigator);
                    break;
                case ScreenType.AutoSave:
                    screen = (Screen)new AutoSaveScreen(navigator);
                    break;
                case ScreenType.Quit:
                    screen = (Screen)new QuitGameScreen(navigator);
                    break;
                case ScreenType.NewGame:
                    screen = (Screen)new NewGameScreen(navigator);
                    break;
                case ScreenType.MissionFailed:
                    screen = (Screen)new MissionFailedScreen(navigator);
                    break;
                case ScreenType.XBoxControllerHelp:
                    screen = (Screen)new XBoxHelpScreen(navigator);
                    break;
                case ScreenType.MainMenu:
                    screen = (Screen)new MainMenuScreen(navigator);
                    break;
			case ScreenType.ThemeSelection:
				if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LD ||
					SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LDW) 
					{
						screen = (Screen)new ThemeSelectionLDScreen (navigator);
					} 
					else 
					{
						screen = (Screen)new ThemeSelectionScreen(navigator);
					}
                    break;
                case ScreenType.StageCompleted:
                    screen = (Screen)new StageCompletedScreen(navigator);
                    break;
                case ScreenType.StageStart:
                    screen = (Screen)new StageStartScreen(navigator);
                    break;
                case ScreenType.ThemeUnlocked:
                    screen = (Screen)new ThemeUnlockedScreen(navigator);
                    break;
                case ScreenType.HowToPlay:
                    screen = (Screen)new HowToPlayScreen(navigator);
                    break;
                case ScreenType.Purchase:
                    screen = (Screen)new PurchaseScreen(navigator);
                    break;
                case ScreenType.Awards:
                    screen = (Screen)new AwardsScreen(navigator);
                    break;
                case ScreenType.PlayerStats:
                    screen = (Screen)new PlayerStatsScreen(navigator);
                    break;
                case ScreenType.Leaderboards:
                    screen = (Screen)new LeaderboardsScreen(navigator);
                    break;
                case ScreenType.Login:
                    screen = (Screen)new LoginScreen(navigator);
                    break;
                case ScreenType.MessageBox:
                    screen = (Screen)new MessageBoxScreen(navigator);
                    break;
               /* case ScreenType.RemoteAPICall:
                    screen = (Screen)new RemoteAPICallScreen(navigator);
                    break;*/
                case ScreenType.Rate:
                    screen = (Screen)new RateScreen(navigator);
                    break;
                default:
                    throw new BrainException("Invalid ScreenType [" + type.ToString() + "]");
            }

            return screen;
        }

        private void SelectGameSettings()
        {
#if FORMS_SUPPORT
            GameSettingsForm form = new GameSettingsForm();
            form.ShowDialog();
#else
            throw new SnailsException("SelectGameSettings is not valid in non Forms application.");
#endif
        }


        /// <summary>
        /// Show the eula
        /// The GameVersion where the eula was read is stored in the user profile
        /// The Eula is displayed if the version changes
        /// </summary>
        private bool EULAAgreement()
        {
#if FORMS_SUPPORT
            if (SnailsGame.ProfilesManager.CurrentProfile.EULAAcceptedInVersion == 
                SnailsGame.Settings.GameVersion)
            {
                return true;
            }

            EULAForm form = new EULAForm();
            if (form.ShowDialog(null) == System.Windows.Forms.DialogResult.Cancel)
            {
                return false;
            }
            SnailsGame.ProfilesManager.CurrentProfile.EULAAcceptedInVersion = SnailsGame.Settings.GameVersion;
            return true;
#else
            throw new SnailsException("EULAAgreement is not valid in non Forms application.");
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void PurchaseGame()
        {
#if APP_STORE_SUPPORT
            if (SnailsGame.IsTrial)
            {
                Guide.ShowMarketplace(PlayerIndex.One);
#if DEBUG
                Guide.SimulateTrialMode = false;
#endif
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static void OpenStoreReviewPanel()
        {
#if APP_STORE_SUPPORT
            // Not sure if this launches exceptions...
            try
            {
                MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                marketplaceReviewTask.Show();
                SnailsGame.ProfilesManager.CurrentProfile.GameWasRated = true;
            }
            catch (System.Exception )
            {
            }
#endif
        }


        /// <summary>
        /// 
        /// </summary>
        public static void RedirectToFreeSnailsOnStore()
        {
#if APP_STORE_SUPPORT
            // Not sure if this launches exceptions...
            try
            {

                MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

                marketplaceDetailTask.ContentIdentifier = "c8987ea5-0c7d-42c4-beaa-6dffe289921b";
                marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;
                marketplaceDetailTask.Show();
            }
            catch (System.Exception)
            {
            }
#endif
        }


		// A falta de melhor sitio...
		// E perciso ter o load dos thumbnails centralizado
		public static ThemeType LastThumbnailThemeLoaded;
	public static Sprite LoadThumbnailSprite1(ThemeType theme, string stageKey)
		{
			if (LastThumbnailThemeLoaded != theme) {
			SnailsGame.UnloadThumbnails ();
			}
			LastThumbnailThemeLoaded = theme;
			return BrainGame.ResourceManager.GetSprite ("spriteset/" + 	theme.ToString () + "/thumbnails/" + stageKey, ResourceManagerIds.STAGE_THUMBNAILS);
	    }

		public static void LoadThumbnails()
		{
			// Carrega todos os thumbnails
		    LevelStage stage = Levels._instance.GetFirstLevelStageFromTheme (ThemeType.ThemeA);
			BrainGame.ResourceManager.GetSprite ("spriteset/" + ThemeType.ThemeA.ToString () + "/thumbnails/" + stage.StageKey, ResourceManagerIds.STAGE_THUMBNAILS);
			stage = Levels._instance.GetFirstLevelStageFromTheme (ThemeType.ThemeB);
			BrainGame.ResourceManager.GetSprite ("spriteset/" + ThemeType.ThemeB.ToString () + "/thumbnails/" + stage.StageKey, ResourceManagerIds.STAGE_THUMBNAILS);
			stage = Levels._instance.GetFirstLevelStageFromTheme (ThemeType.ThemeC);
			BrainGame.ResourceManager.GetSprite ("spriteset/" + ThemeType.ThemeC.ToString () + "/thumbnails/" + stage.StageKey, ResourceManagerIds.STAGE_THUMBNAILS);
			stage = Levels._instance.GetFirstLevelStageFromTheme (ThemeType.ThemeD);
			BrainGame.ResourceManager.GetSprite ("spriteset/" + ThemeType.ThemeD.ToString () + "/thumbnails/" + stage.StageKey, ResourceManagerIds.STAGE_THUMBNAILS);
		}	

		public static void UnloadThumbnails()
		{
		
			BrainGame.ResourceManager.Unload(ResourceManagerIds.STAGE_THUMBNAILS);
			LastThumbnailThemeLoaded = ThemeType.None;
		}

		/// <summary>
		/// 
		/// </summary>
		private void SnailsGame_OnUserAskedToRateLater(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		private void SnailsGame_OnUserDeclinedGameRate(object sender, EventArgs e)
		{
			SnailsGame.ProfilesManager.CurrentProfile.GameWasRated = true;
			SnailsGame.ProfilesManager.Save ();
		}

		/// <summary>
		/// 
		/// </summary>
		private void SnailsGame_OnUserRatedGame(object sender, EventArgs e)
		{
			SnailsGame.ProfilesManager.CurrentProfile.GameWasRated = true;
			SnailsGame.ProfilesManager.Save ();
		}


	}
}
