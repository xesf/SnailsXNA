using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.StageObjects;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.Snails.ToolObjects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Screens;
using TwoBrainsGames.BrainEngine.Debugging;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.Stages.HUD;
using TwoBrainsGames.Snails.Tutorials;
using TwoBrainsGames.BrainEngine.Effects.Shades;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages.Hints;
#if FORMS_SUPPORT
using TwoBrainsGames.Snails.Winforms;

#endif
#if BETA_TESTING
using TwoBrainsGames.BrainEngine.Beta;
#endif

namespace TwoBrainsGames.Snails.Stages
{
    public partial class Stage : IBrainComponent, ISnailsDataFileSerializable
    {
        // The context where the stage loading is being made
        public enum StageLoadingContext
        {
            Gameplay,
            StageEditor
        }

        public enum MissionStateType
        {
            None,
            Starting,
            Running,
            Completed,
            Failed
        }

        public enum MissionFailedReasonType
        {
            Incomplete,
            NotEnoughSnails,
            TimeExpired,
            KingIsDead
            
        }

        public enum StageState
        {
            Startup,
            Playing,
            Paused,
            Ended,
            LoadingGameplay,
            InspectingHints
        }
       

        #region Constants
        public const int TIME_WARP_MULTIPLIER = 4;
        public const int NORMAL_TIME_MULTIPLIER = 1;

        public const double STAGE_COMPLETION_DELAY = 3000;
        public const double STAGE_COMPLETION_DELAY_MORE_SNAILS = 1000;
        public const double STAGE_FAILED_DELAY = 3000;
        
        public const double STAGE_MISSION_CHECK_TIMER = 500; // Mission status is checked twice per second
        public const string STAGES_ID = "stages/{0}/{1}";
        public const string STAGES_THEME_ID = "stages/{0}/{0}";

        public const string SPRITESET_ID = "spriteset/{0}/{0}";
        public const string STAGES_ASSET_TILES = "tiles";
        public const string STAGES_ASSET_OBJECTS = SPRITESET_ID + "-objects";
        public const string STAGES_ASSET_TILESFRAGMENTS = SPRITESET_ID + "-tiles-fragments";
        public const string STAGES_ASSET_TEXTFONT_ID = "fonts/{0}/{0}-{1}";

        // Quadtree related
        public const int QUADTREE_MAX_OBJECTS_PER_NODE = 10;
        public const int QUADTREE_MIN_NODE_SIZE = 200;
        public const int QUADTREE_OBJ_LIST_COUNT = 3; // Number of object lists to have in the nodes
        public const int QUADTREE_SNAIL_LIST_IDX = 0; // List index for snails
        public const int QUADTREE_STAGEOBJ_LIST_IDX = 1; // List index for StageObjects
        public const int QUADTREE_PATH_LIST_IDX = 2; // List index for path nodes

        // Transform effects IDs
        public const int TRANSF_EFFECT_CAMERA_SHAKE = 1;

        public const int TILE_WIDTH = 60;
        public const int TILE_HEIGHT = 60;

        public const int BONUS_SNAILS_DELIVERED = 5; // Bonus points for each snail delivered
        public const int BONUS_TIME_TAKEN = 1; // Bonus points for second less then the targe time
        public const int BONUS_UNUSED_TOOL = 5; // Bonus points for each unused tool
        public const int BONUS_BRONZE_COINS = 20;
        public const int BONUS_SILVER_COINS = 50;
        public const int BONUS_GOLD_COINS = 100;

        #endregion

        #region Events
        public delegate void StageDrawEventHandler(bool shadow, SpriteBatch spriteBatch);
        public event StageDrawEventHandler OnBeforeObjectsDraw;
        #endregion

        #region Static Members
        public static Stage CurrentStage;
        #endregion

        #region Private Members
        //private Vector2 _viewportScale; // Viewport scale relative to the screen size
		//private DateTime _startTime;
		//private DateTime _endTime;
        private MissionFailedReasonType _missionFailedReason;
        private double _missionStateCheckTimer; // Timer used to check the mission state. Don't need to check the state in all frames
        private double _stageEndedTimer; // When the stage is completed this is used to wait a bit of a time before
        // proceding to the stage completed screen
        private double _stageCompletedDelay;
        //private Sprite _spriteBackground;
        private bool fireIsPlaying = false; //  this will prevent more than one fire sound effect beeing played in the stage
        private WaterShadeEffect _waterShadeEffect;
        public bool HasWater = false;
        public Texture2D WaterTexture;
        private RenderTarget2D _waterRenderTarget;
        private GameplayRecorder _gameplayRecorder;
        public bool InTimeWarp { get; private set; }
        #endregion

        #region Public Members
        public string Id;
        public string Key { get { return this.LevelStage.StageKey; } } // used to stop users from copying stages over other stages. If the key does not match Levels, then the load will fail
        public string Description;
        public bool _withShadows;
        public GameplayInput Input { get { return GameplayScreen.Instance.Input; } }
        public PlayerProfile Player { get { return SnailsGame.ProfilesManager.CurrentProfile; } }
        public StageHUD StageHUD;
        public StageStats Stats;
        public StageData StageData
        {
            get { return Levels._instance.StageData; }
        }
        public LevelStage LevelStage;
        public Board Board;
        public StageCursor Cursor = new StageCursor();
        public InGameCamera Camera;
        public List<Snail> Snails;
        public List<StageObject> Objects = new List<StageObject>();
        public List<StageObject> ObjectsRemove = new List<StageObject>();
        public List<StageObject> ObjectsAdd = new List<StageObject>();
        public List<StageObject> LinksToRemove = new List<StageObject>();
        public List<Liquid> LiquidObjects = new List<Liquid>();
        public List<SnailsBackgroundLayer> Layers = new List<SnailsBackgroundLayer>();
        public List<StageObject> BackgroundObjectsDrawList = new List<StageObject>();
        public List<StageObject> ForegroundObjectsDrawList = new List<StageObject>();
        public List<StageObject> ForegroundWaterDrawList = new List<StageObject>();
        public List<ParticlesEffect> Particles = new List<ParticlesEffect>();
        RenderTarget2D _renderTarget;
        public RenderTarget2D RenderTarget 
        {
            get { return this._renderTarget; }
            private set
            {
                this._renderTarget = value;
            }
        } 
        // will keep the last frame buffer from stage to be used outside
        public bool IsPaused = false;
        public Vector2 _backgroundLayersOffset;
        private MissionStateType _missionState;
        public MissionStateType MissionState
        {
            get { return this._missionState; }
            set
            {
                if (this._missionState != value)
                {
                    this._missionState = value;
                    if (this.StageHUD != null)
                    {
                        this.StageHUD.MissionStateChanged();
                    }
                }
            }
        }
        public ControllerRumble Rumble;
        public GameplayRecorder GameplayRecorder { get { return this._gameplayRecorder; } }
#if DEBUG
        public int SnailCounter;
        SpriteFont _debugFont;
#endif
        public StageState State;
//        protected int _saveTimeMultiplier;
        #endregion

        #region Properties
        
        public SpriteBatch SpriteBatch
        {
            get { return Levels.CurrentLevel.SpriteBatch; }
        }

        public int[] StartupTutorialTopics { get; private set; }
        public string StartupTopicsString { get; set; } // Comma separated string with the ids do show
       
        // Where the camera should be placed - in board row/cols
        public Vector2 StartupCameraPOI
        {
            get
            {
                return new Vector2(((this.StartupCenter.X - 1)* Stage.TILE_WIDTH + (Stage.TILE_WIDTH / 2)),
                                   ((this.StartupCenter.Y - 1) * Stage.TILE_HEIGHT) + (Stage.TILE_HEIGHT / 2));
            }
        }
        public Vector2 StartupCenter
        {
            get;
            set;
        }

		public int BuildNr { get; private set; }
        public string YouTubeUrl { get; private set; }

        public MedalScoreCriteria GoldMedalScoreCriteria { get; set; }
        public MedalScoreCriteria SilverMedalScoreCriteria { get; set; }
        public MedalScoreCriteria BronzeMedalScoreCriteria { get; set; }

        public bool FireIsPlaying { get { return fireIsPlaying; } set { fireIsPlaying = value; } }

        public bool IsCustomStage { get { return this.LevelStage.IsCustomStage; } }
        public static StageLoadingContext LoadingContext
        {
            get;
            set;
        }
        public LightManager LightManager { get; private set; }

        public Color ShadowColor
        {
            get { return Levels.CurrentThemeSettings._shadowColor; }
        }

        public bool _withLiquids;
        Sample _stageCompletedSound;
        public HintManager HintManager { get; private set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Stage():
            this(null)
        {
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    public Stage(LevelStage levelStage)
        {
            CurrentStage = this;
            LevelStage = levelStage;
            this.Snails = new List<Snail>();

            Stats = new StageStats();
            StageHUD = new StageHUD();
            this.Board = new Board();
            this._waterRenderTarget = null;
            this.GoldMedalScoreCriteria = new MedalScoreCriteria(this);
            this.SilverMedalScoreCriteria = new MedalScoreCriteria(this);
            this.BronzeMedalScoreCriteria = new MedalScoreCriteria(this);
            this.MissionState = MissionStateType.None;
            this.Camera = new InGameCamera(this);
            this.LightManager = new LightManager();
            this._gameplayRecorder = new GameplayRecorder(this);
            this.HintManager = new HintManager(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            GameplayScreen.Instance.TimeMultiplier = Stage.NORMAL_TIME_MULTIPLIER;
            Stats = new StageStats();
            Stats.NumSnailsToSave = this.LevelStage._snailsToSave;
            if (this.LevelStage._goal == GoalType.TimeAttack)
            {
                Stats.Timer = this.LevelStage._targetTime;
            }
            
            StageHUD.Initialize();
            Cursor.Initialize();
            Camera.Initialize();
            Board.Initialize();
            SnailsGame.Instance.ActiveCamera = Camera;
            foreach (SnailsBackgroundLayer layer in this.Layers)
            {
                layer.Initialize();
            }

            this._stageEndedTimer = 0;
            this._missionStateCheckTimer = 0;
            SnailsGame.GameTime.Reset(); // This is to reset the multiplier
            SnailsGame.Tutorial.Initialize();

            // Precompute this
            //this._viewportScale = new Vector2((float)BrainGame.Viewport.Width / (float)BrainGame.ScreenWidth, (float)BrainGame.Viewport.Height / (float)BrainGame.ScreenHeight);


          //  this._backgroundColorEffect = new ColorEffect();
          //  this._backgroundColorEffect.Active = false;
          //  this._backgroundBlendColor = Color.White;

#if DEBUG
            // Update the settings. This will store the last stage palyed
            if (Levels.CurrentStageNr != Levels.CUSTOM_STAGE_NR)
            {
                SnailsGame.GameSettings.StartupStageNr = Levels.CurrentStageNr;
                SnailsGame.GameSettings.StartupTheme = Levels.CurrentTheme;
                SnailsGame.GameSettings.SaveToFile();
            }
#endif
           
            if (SnailsGame.GameSettings.WithRumbble)
            {
                Rumble = new ControllerRumble(Stage.CurrentStage.Input.GamePad);
            }

            if (this.HasWater && SnailsGame.GameSettings.UseWaterEffect)
            {
                _waterShadeEffect = new WaterShadeEffect(GameplayScreen.Instance);
                _waterShadeEffect.Initialize();
            }

            this._gameplayRecorder.Initialize();

            // TutorialTopics
            this.StartupTutorialTopics = null;
            if (!string.IsNullOrEmpty(this.StartupTopicsString))
            {
                string[] topicsList = this.StartupTopicsString.Split(',');
                this.StartupTutorialTopics = new int[topicsList.Length];
                for (int i = 0; i < this.StartupTutorialTopics.Length; i++)
                {
                    this.StartupTutorialTopics[i] = Convert.ToInt32(topicsList[i]);
                }
            }

			this.LightManager.Initialize();
            this.HintManager.Initialize();

            PresentationParameters pp = SpriteBatch.GraphicsDevice.PresentationParameters;
            this.RenderTarget = new RenderTarget2D(SpriteBatch.GraphicsDevice, BrainGame.Viewport.Width, BrainGame.Viewport.Height, false, pp.BackBufferFormat, pp.DepthStencilFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {           
            StageHUD.LoadContent();
            Cursor.LoadContent();
            Board.LoadContent();
            
            Stats.NumSnailsToRelease = this.GetTotalSnailsToRelease();

            this._waterRenderTarget = null;

#if DEBUG
            this._debugFont = BrainGame.ResourceManager.Load<SpriteFont>("fonts/dbgWindow", ResourceManager.ResourceManagerCacheType.Static);
#endif

            foreach (SnailsBackgroundLayer layer in this.Layers)
            {
                layer.LoadContent();
            }
            this.MissionState = MissionStateType.Starting;
            this._missionFailedReason = MissionFailedReasonType.Incomplete;
            this.Camera.SetOriginToHudCenter(this.StageHUD);

            if (this.HasWater && SnailsGame.GameSettings.UseWaterEffect)
            {
                _waterShadeEffect.LoadContent();
            }
            this._gameplayRecorder.LoadContent();
            this._stageCompletedSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.STAGE_COMPLETED);
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetTotalSnailsToRelease()
        {
            int toRelease = 0;
            foreach (StageObject obj in this.Objects)
            {
                if (obj is StageEntrance)
                {
                    StageEntrance objEntrance = (StageEntrance)obj;
                    if (!objEntrance.ReleasesEvilSnails)
                    {
                        toRelease += objEntrance.TotalSnailsToRelease;
                    }
                }
            }
            return toRelease;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnOpenTransitionEnded()
        {
            if (this.StartupTutorialTopics != null &&
                this.StartupTutorialTopics.Length > 0)
            {
                this.ShowTutorialTopics(this.StartupTutorialTopics, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Levels.CurrentLevel.StageSound.PlayAmbience();

          
            // Place the camera 
            if (this.StartupCameraPOI != Vector2.Zero)
            {
                this.Camera.MoveTo(this.StartupCameraPOI);
            }
            else // Not defined? Place the camera in the center of the board
            {
                this.Camera.MoveToOrigin();
            }

            if (SnailsGame.GameSettings.MaxZoomOut != 1f)
            {
                this.Camera.FullZoomOut(); // On wp, this will let the player see a bigger part of the stage
                                           // On windows it will do nothing because max zoom out is 1
                this.Camera.CenterInStage();
            }
			//this._startTime = DateTime.Now;
            this.State = StageState.Startup;
            this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.ClickToStart);

            this.QueryGameplayLoadingRecording();
            this.UpdateAudibleBoundinBox();

            this.StageHUD._toolsMenu.HideHintButton();
        }

        /// <summary>
        /// Startup is the phase when the player may inspect the stage before the stage starts
        /// This will start the stage
        /// </summary>
        public void StartupEnded()
        {
            Levels.CurrentLevel.StageSound.PlayMusic();
            this.State = StageState.Playing;
            this.MissionState = MissionStateType.Running;
            this.StageHUD.OnStageStarted();
            switch (this.LevelStage._goal)
            {
                case GoalType.SnailDelivery:
                    this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.DeliveryStart);
                    break;
                case GoalType.SnailKiller:
                    this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.SnailKillerStart);
                    break;
                case GoalType.SnailKing:
                    this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.SnailKingStart);
                    break;
                case GoalType.TimeAttack:
                    this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.TimeAttackStart);
                    break;
            }
            this.Camera.StageStartupZoomIn(this.StartupCameraPOI); // Zooms in on the startup stage POI

            // Notificar todos os objectos
            foreach (StageObject obj in this.Objects)
            {
                obj.StageStartupPhaseEnded();
            }
            this.StageHUD._toolsMenu.ShowHintButton();
        }

        /// <summary>
        /// 
        /// </summary>
        public void QueryGameplayLoadingRecording()
        {
            // Gameplay recording support
            this._gameplayRecorder.Enabled = false;
#if FORMS_SUPPORT
            // Save solution 
            if (BrainGame.ScreenNavigator.GlobalCache.Get<bool>(GlobalCacheKeys.GAMEPLAY_RECORDER_SAVE_SOLUTION, false))
            {
                this._gameplayRecorder.StartRecording();
                this._gameplayRecorder.Enabled = true;
            }

            // Load solution form
            if (BrainGame.ScreenNavigator.GlobalCache.Get<bool>(GlobalCacheKeys.GAMEPLAY_RECORDER_LOAD_SOLUTION, false))
            {
                this.State = StageState.LoadingGameplay;
                BrainGame.ScreenNavigator.Disable();
                LoadGameplayForm form = new LoadGameplayForm();
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    this.GameplayRecorder.Load(form.Filename);
                    this.GameplayRecorder.Play();
                }
                BrainGame.ScreenNavigator.Enable();
                this.State = StageState.Startup;
                this._gameplayRecorder.Enabled = true;
            }
#endif   

            // Some stuff must be disabled or else gameplay recording may fail
            this.Cursor.EndPanEaseOutEnabled = !this._gameplayRecorder.Enabled;
        }
      

        /// <summary>
        /// 
        /// </summary>
        public void HandleEvents(BrainGameTime gameTime)
        {
			
            Vector2 cameraOffset = Vector2.Zero;

            if (SnailsGame.GameSettings.UseKeyboard)
            {
                if (this.Input.IsCameraUpPressed)
                    cameraOffset.Y -= 5;
                if (this.Input.IsCameraDownPressed)
                    cameraOffset.Y += 5;
                if (this.Input.IsCameraLeftPressed)
                    cameraOffset.X -= 5;
                if (this.Input.IsCameraRightPressed)
                    cameraOffset.X += 5;
            }
            else
            if (SnailsGame.GameSettings.UseGamepad)
            {
                if (Stage.CurrentStage.Input.CameraMotion)
                {
                    cameraOffset.X += Stage.CurrentStage.Input.CameraMotionPosition.X * 7;
                    cameraOffset.Y += Stage.CurrentStage.Input.CameraMotionPosition.Y * -7; // invert Y axis
                }   
            }

            if (cameraOffset != Vector2.Zero)
            {
                this.Camera.MoveByOffset(cameraOffset);
            }

            if (this.Input.ActionPause && this.StageHUD.ControlButtonsVisible == false)
            {
                this.PauseGame();
            }

#if DEBUG
            if (this.Input.ActionOpenDebugOptions)
            {
                GameplayScreen.Instance.PopUp(ScreenType.DebugOptions.ToString(), true);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateMissionState(BrainGameTime gameTime)
        {
            if (this.MissionState == MissionStateType.Running)
            {
                // Don't need to check the state of the mission in all frames so a timer is used
                this._missionStateCheckTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this._missionStateCheckTimer > Stage.STAGE_MISSION_CHECK_TIMER)
                {
                    this._missionStateCheckTimer = 0;
                    MissionStateType previousState = this.MissionState;
                    this.MissionState = this.CheckMissionState(out  this._missionFailedReason);

                    if (previousState != this.MissionState)
                    {
                        switch (this.MissionState)
                        {
                            case MissionStateType.Completed:
                                this._stageCompletedSound.Play();
                                this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.MissionCompleted);
                                // Show "Mission completed" on top of the screen only if not all snails have been delivered
                                // This is because in this situation the stage does not automaticaly ends
                                // We give the change to the player to deliver more snails
                                if (this.Stats.NumSnailsActive > 0)
                                {
                                    this._stageCompletedDelay = Stage.STAGE_COMPLETION_DELAY_MORE_SNAILS;
                                //    Stage.CurrentStage.ShowTutorialTopic(TutorialTags.MISSION_COMPLETED_MORE_POINTS, false);
                                }
                                else
                                {   // Make the delay before stage ends bigger when there aren't more snails
                                    // This gives it time for the "Mission completed" message goes by
                                    this._stageCompletedDelay = Stage.STAGE_COMPLETION_DELAY;
                                }
                                this.StageHUD.StopTimer();
                                this.Stats.TimeTaken = this.Stats.Timer;
                                // If this is the time attack mode, subtract the timer to get the time taken,
                                // this is because the timer is inverted
                                if (this.LevelStage._goal == GoalType.TimeAttack)
                                {
                                    this.Stats.TimeTaken = this.LevelStage._targetTime - this.Stats.Timer;
                                }
                                this.StageHUD._toolsMenu.HideHintButton();
                                break;

                            case MissionStateType.Failed:
                                switch (this._missionFailedReason)
                                {
                                    case MissionFailedReasonType.TimeExpired:
                                        this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.TimeIsUp);
                                        break;
                                    default:
                                        this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.MissionFailed);
                                        break;
                                }
                                this._stageCompletedDelay = Stage.STAGE_FAILED_DELAY;
                                this.StageHUD.StopTimer();
                                this.State = StageState.Ended;
                                this.StageHUD._toolsMenu.HideHintButton();
                                SnailsGame.ProfilesManager.CurrentProfile.StagesFailedCount++;
   
                                break;
                        }
                    }
                }
            }
            else
            {
                // Only automatically end the stage if all snails have been delivered and there's no tutorial topic active
                if (((this.Stats.NumSnailsActive == 0 && this.Stats.NumSnailsToRelease == 0) || this.MissionState == MissionStateType.Failed))
               //     && SnailsGame.Tutorial.TopicVisible == false)
                {
                    this._stageEndedTimer += gameTime.ElapsedRealTime.TotalMilliseconds;
                    if (this._stageEndedTimer > this._stageCompletedDelay)
                    {
                        this.EndMission();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            if (this._gameplayRecorder.Enabled)
            {
                this._gameplayRecorder.Update(gameTime);
            }

            if (this.HintManager.HintsVisible)
            {
                this.HintManager.Update(gameTime);
            }

            // On startup we give the chance to the player to analyse the stage
            // The game is paused but the player may pan the screen
            // Just perform a limited set of updates
            if (this.State == StageState.Startup ||
                this.State == StageState.InspectingHints)
            {
                this.StageHUD.UpdateIncomingMessage(gameTime);
                this.Cursor.ControllerEvents(gameTime);
                this.Cursor.Update(gameTime);
                this.Camera.Update(gameTime);
                this.Board.Update(gameTime);
                this.LightManager.Update(gameTime);
                this.UpdateAudibleBoundinBox();
                SnailsGame.Tutorial.Update(gameTime);
                // Update the background layers
                foreach (SnailsBackgroundLayer layer in this.Layers)
                {
                    layer.Update(gameTime, false);
                } 
                return;
            }

            SnailsGame.Tutorial.Update(gameTime);
            if (SnailsGame.Tutorial.TopicVisible)
            {
                if (SnailsGame.GameSettings.PauseInTutorial)
                {
                    this.Cursor.ControllerEvents(gameTime); // prevents cursor to stuck on screen
                    return;
                }
            }

            this.UpdateMissionState(gameTime);
            StageHUD.Update(gameTime);
			this.Camera.Update(gameTime);
            Cursor.ControllerEvents(gameTime);
            Cursor.Update(gameTime);

            if (this.HasWater && SnailsGame.GameSettings.UseWaterEffect)
            {
                _waterShadeEffect.Update(gameTime);
            }

            // Update the background layers
            // Disable this for now...
         /*   if (this._backgroundColorEffect.Active) // Color effect when time warp is active
            {
                this._backgroundColorEffect.Update(gameTime);
                this._backgroundBlendColor = this._backgroundColorEffect.Color;
            }*/
            foreach (SnailsBackgroundLayer layer in this.Layers)
            {
                layer.Update(gameTime, true);
            }

            // Update the board
            Board.Update(gameTime);

            if (Objects.Count > 0)
            {
                foreach (StageObject obj in Objects)
                {
                    obj.Update(gameTime);
                }
            }

            if (this.Snails.Count > 0)
            {
                foreach (Snail snail in this.Snails)
                {
                    snail.Update(gameTime);
 //                   snail.AfterUpdate(gameTime);
                }
            }

            // This loops were inside the Udpate loops
            // They were moved outside because of the LaserSwitch
            // This method makes possible to know if 2 objects didn't collided in the current frame
            foreach (StageObject obj in Objects)
            {
                obj.AfterUpdate(gameTime);
            }
            foreach (Snail snail in this.Snails)
            {
                snail.AfterUpdate(gameTime);
            }

            if (Particles.Count > 0)
            {
                foreach (ParticlesEffect particle in Particles)
                {
                    if (!particle.Ended)
                    {
                        particle.Update(gameTime);
                    }
                }
            }

            this.LightManager.Update(gameTime);


            // rumble xbox controller
            if (Rumble != null)
            {
                Rumble.Update(gameTime);
            }
            // I think this is not necessary anymore
            // Colision detection now is object->snail, and not snail->object
            //ProcessCollisionDetection();
            
            ProcessObjectsToRemove();
            ProcessParticlesToRemove();
            this.ProcessObjectsToAdd();
#if DEBUG
            SnailsGame.DebugInfo.SnailCounter.Set(this.Snails.Count);
            SnailsGame.DebugInfo.ObjectsCounter.Set(this.Objects.Count);
#if (DEBUG && DEBUG_ASSERTIONS)
           this.Board.Quadtree.AssertTreeIntegrity();
#endif
#endif

            Levels.CurrentLevel.StageSound.Update(gameTime);

            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalPlayingTime += gameTime.ElapsedRealTime;
            this.UpdateAudibleBoundinBox();


        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateAudibleBoundinBox()
        {
            BrainGame.SampleManager.AudibleBoundingSquare = this.StageHUD._stageArea.Transform(this.Camera.Position - this.Camera.Origin);
        }

        /// <summary>
        /// Draw the current framebuffer into RenderTarget
        /// </summary>
        public void DrawToRenderTarget()
        {
    //        if (RenderTarget != null) // this will block all the next interations if we alredy got the frame buffer
        //        return;

            bool cursorVisible = BrainGame.GameCursor.Visible;
            BrainGame.GameCursor.Visible = false; // take cursor out of blur render

            // initialize the render target
            // apply render targer - NOTE: all drawing code will be saved in thie "_renderTarget" and won't be blit to screen during this phase
            BrainGame.Graphics.SetRenderTarget(RenderTarget);
            this.Draw();
            BrainGame.Graphics.SetRenderTarget(null);
            BrainGame.Graphics.Viewport = BrainGame.Viewport;
            BrainGame.GameCursor.Visible = cursorVisible; // Restore cursor
        }

        /// <summary>
        /// Draw the current framebuffer into RenderTarget to allow the generation of stage thumbnails
        /// </summary>
        public void DrawToRenderTargetThumbs()
        {
      //      if (RenderTarget != null) // this will block all the next interations if we alredy got the frame buffer
      //          return;

            bool cursorVisible = BrainGame.GameCursor.Visible;
            BrainGame.GameCursor.Visible = false; // take cursor out of blur render
            // initialize the render target
            PresentationParameters pp = SpriteBatch.GraphicsDevice.PresentationParameters;
            RenderTarget = new RenderTarget2D(SpriteBatch.GraphicsDevice, BrainGame.Viewport.Width, BrainGame.Viewport.Height, false, pp.BackBufferFormat, pp.DepthStencilFormat);
            // apply render targer - NOTE: all drawing code will be saved in thie "_renderTarget" and won't be blit to screen during this phase
            BrainGame.Graphics.SetRenderTarget(RenderTarget);
            this.DrawForThumbs();
            BrainGame.Graphics.SetRenderTarget(null);
            BrainGame.Graphics.Viewport = BrainGame.Viewport;
            BrainGame.GameCursor.Visible = cursorVisible; // Restore cursor
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawUnderWaterToRenderTarget()
        {
            //if (_waterRenderTarget != null) // this will block all the next interations if we alredy got the frame buffer
            //    return;

            if (_waterRenderTarget == null)
            {
                // initialize the render target
                PresentationParameters pp = SpriteBatch.GraphicsDevice.PresentationParameters;
                _waterRenderTarget = new RenderTarget2D(SpriteBatch.GraphicsDevice, BrainGame.Viewport.Width, BrainGame.Viewport.Height, false, pp.BackBufferFormat, pp.DepthStencilFormat, 1, RenderTargetUsage.DiscardContents);
            }

            //_stageRenderTarget = (Texture2D)BrainGame.Graphics.GetRenderTargets()[0].RenderTarget;

            bool cursorVisible = BrainGame.GameCursor.Visible;
            BrainGame.GameCursor.Visible = false; // take cursor out of blur render

            
            // apply render targer - NOTE: all drawing code will be saved in thie "_renderTarget" and won't be blit to screen during this phase
            BrainGame.Graphics.SetRenderTarget(_waterRenderTarget);
            this.DrawBackgroundLayers();
            BeginDraw();
            this.Draw(false, true);
            EndDraw();
            this.DrawForegroundLayers();
            BrainGame.Graphics.SetRenderTarget(null);
            BrainGame.Graphics.Viewport = BrainGame.Viewport;
            BrainGame.GameCursor.Visible = cursorVisible; // Restore cursor
        }

        /// <summary>
        /// 
        /// </summary>
        private void Draw(bool shadow)
        {
            this.Draw(shadow, false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Draw(bool shadow, bool drawForWaterShade)
        {
            if (this.OnBeforeObjectsDraw != null)
            {
                this.OnBeforeObjectsDraw(shadow, this.SpriteBatch); // Change event handler later
            }

            // Draw background objects
            foreach (StageObject obj in this.BackgroundObjectsDrawList)
            {
                obj.Draw(shadow);
            }

            // Draw Snails
            foreach (Snail snail in this.Snails)
            {
                snail.Draw(shadow);
            }

            Board.DrawBackground(shadow);

            // FIXME improve this code
            if (drawForWaterShade)
            {
                // Draw foreground water object
                foreach (StageObject obj in this.ForegroundWaterDrawList)
                {
                    if (!(obj is Liquid))
                        obj.Draw(shadow);
                }
            }
            else
            {
                // Draw foreground water object
                foreach (StageObject obj in this.ForegroundWaterDrawList)
                {
                    obj.Draw(shadow);
                }
            }


            // Draw tiles
            Board.Draw(shadow);

            // if we are using this method to draw the stage for the water effect
            // we will need to avoid the last part of the drawing. It will only
            // draw the above objects
            if (drawForWaterShade)
                return;

            // Draw foreground objects
            foreach (StageObject obj in this.ForegroundObjectsDrawList)
            {
                obj.Draw(shadow);
            }

            // Draw background objects foreground part
            foreach (StageObject obj in this.BackgroundObjectsDrawList)
            {
               obj.ForegroundDraw();
            }

            // Draw particles
            if (Particles.Count > 0)
            {
                foreach (ParticlesEffect particle in Particles)
                {
                    if (!particle.Ended)
                    {
                        particle.Draw(SpriteBatch);
                    }
                }
            }

            // Hints
            if (this.HintManager.HintsVisible)
            {
                this.HintManager.Draw(this.SpriteBatch);
            }
        }

        public void BeginDraw()
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, this.Camera.StageRenderEffect);
        }

        public void EndDraw()
        {
            SpriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawWaterTexture()
        {
            // get render texture when 
            if (this.HasWater && SnailsGame.GameSettings.UseWaterEffect)
            {
                DrawUnderWaterToRenderTarget();
                WaterTexture = (Texture2D)this._waterRenderTarget;
                WaterTexture = _waterShadeEffect.Draw(WaterTexture);
            }
        }

        private void DrawBackgroundLayers()
        {
            // Draw background layers
            foreach (SnailsBackgroundLayer layer in Layers)
            {
                // layer.BlendColor = this._backgroundBlendColor;
                if (layer._layerType == LayerType.Background)
                {
                    SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, layer.BackgroundEffect);
                    layer.Draw(this.SpriteBatch);
                    SpriteBatch.End();
                }
            }
        }

        private void DrawForegroundLayers()
        {
            // Draw background layers
            foreach (SnailsBackgroundLayer layer in Layers)
            {
                // layer.BlendColor = this._backgroundBlendColor;
                if (layer._layerType == LayerType.Foreground)
                {
                    SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, layer.BackgroundEffect);
                    layer.Draw(this.SpriteBatch);
                    SpriteBatch.End();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {

#if DEBUG
            //   BrainGame.LineBatch.WorldTransform =Stage.CurrentStage.Camera.Transform; // This doesn't work don't know why, Camera.Transform returns identity matrix
            BrainGame.LineBatch.WorldTransform = Matrix.CreateTranslation(Stage.CurrentStage.Camera.Origin.X, Stage.CurrentStage.Camera.Origin.Y, 0);
            BrainGame.LineBatch.Begin();
#endif
            this.DrawBackgroundLayers();

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, this.Camera.StageRenderEffect);

            if (this._withShadows)
            {
                this.Draw(true);
            }
            this.Draw(false);
            SpriteBatch.End();

            this.DrawForegroundLayers();
#if DEBUG
            BrainGame.LineBatch.End();
#endif

#if DEBUG
            //   BrainGame.LineBatch.WorldTransform =Stage.CurrentStage.Camera.Transform; // This doesn't work don't know why, Camera.Transform returns identity matrix
            BrainGame.LineBatch.WorldTransform = Matrix.Identity;
            BrainGame.LineBatch.Begin();
#endif            
           this.LightManager.Draw();

#if MONOGAME1 // commented for now...
			// Fix for monogame!!
			// Issue reported here: https://github.com/mono/MonoGame/issues/780
			BrainGame.RenderEffect.World = Matrix.Identity; // WORK AROUND!
#endif
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);

            this.StageHUD.Draw(SpriteBatch);
#if DEBUG
            if (SnailsGame.GameSettings.ShowStageStats)
            {
                this.DrawStageStats();
            }
#endif

            this.Cursor.Draw();

            SpriteBatch.End();
#if DEBUG
            BrainGame.LineBatch.End();
#endif     

            
            if (this._gameplayRecorder.Enabled)
            {
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
                this._gameplayRecorder.Draw(Stage.CurrentStage.SpriteBatch);
                SpriteBatch.End();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawForThumbs()
        {
            // update layers to display in the correct position
            this.Camera.MoveTo(this.StartupCameraPOI);
            this.Camera.Update(null);
            foreach (SnailsBackgroundLayer layer in this.Layers)
            {
                layer.Update(null, false);
            } 

            // draw stage area
            this.DrawBackgroundLayers();
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, this.Camera.StageRenderEffect);
            this.Draw(false);
            SpriteBatch.End();
            this.DrawForegroundLayers();
            this.LightManager.Draw();
        }
    
#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        private void DrawStageStats()
        {
            Vector2 pos = new Vector2(10f, 50f);
            this.SpriteBatch.DrawString(this._debugFont, string.Format("Snails to release : {0} ", Stats.NumSnailsToRelease), pos, Color.Yellow);
            pos += new Vector2(0f, 15f);
            this.SpriteBatch.DrawString(this._debugFont, string.Format("Snails delivered  :  {0} ", Stats.NumSnailsSafe), pos, Color.Yellow);
            pos += new Vector2(0f, 15f);
            this.SpriteBatch.DrawString(this._debugFont, string.Format("Snails active     : {0} ", Stats.NumSnailsActive), pos, Color.Yellow);
            pos += new Vector2(0f, 15f);
            this.SpriteBatch.DrawString(this._debugFont, string.Format("Snails released   : {0} ", Stats.NumSnailsReleased), pos, Color.Yellow);
            pos += new Vector2(0f, 15f);

        }
#endif
        /// <summary>     
        /// 
        /// </summary>
        public void UnloadContent()
        {
            if (RenderTarget != null)
            {
                this.RenderTarget.Dispose();
                this.RenderTarget = null;
            }
            if (this.LightManager != null)
            {
                this.LightManager.Unload();
            }
            if (_waterRenderTarget != null)
            {
                _waterRenderTarget.Dispose();
                _waterRenderTarget = null;
            }
        }

        /// <summary>
        /// Checks the current state of the mission
        /// -Completed, failed or running
        /// </summary>
        public MissionStateType CheckMissionState(out MissionFailedReasonType failedReason)
        {
            failedReason = MissionFailedReasonType.Incomplete;

            if (this.LevelStage._goal == GoalType.SnailDelivery ||
                this.LevelStage._goal == GoalType.SnailKiller ||
                this.LevelStage._goal == GoalType.SnailKing)
            {
                if (this.Stats.Timer.TotalMinutes >= 60)
                {
                    failedReason = MissionFailedReasonType.TimeExpired;
                    return MissionStateType.Failed;
                }
            }

            switch (this.LevelStage._goal)
            {
                // Snail delivery
                case GoalType.SnailDelivery:
                case GoalType.TimeAttack:
                    // Stage completed if at least n snails were saved
                    if (this.Stats.NumSnailsSafe >= this.Stats.NumSnailsToSave)
                    {
                        return MissionStateType.Completed;
                    }
                    else // Snails on the board+ snails to release + snails safe must be grater then the total snails to save
                    if (this.Stats.NumSnailsToRelease + this.Stats.NumSnailsSafe + this.Stats.NumSnailsActive < this.Stats.NumSnailsToSave)
                    {
                        failedReason = MissionFailedReasonType.NotEnoughSnails;
                        return MissionStateType.Failed;
                    }

                    if (this.LevelStage._goal == GoalType.TimeAttack)
                    {
                        if (this.Stats.Timer.TotalSeconds == 0)
                        {
                            failedReason = MissionFailedReasonType.TimeExpired;
                            return MissionStateType.Failed;
                        }
                    }
                    break;

                case GoalType.SnailKiller:
                    if (this.Stats.NumSnailsDead >= this.LevelStage._snailsToRelease)
                    {
                        return MissionStateType.Completed;
                    }
                    break;

                case GoalType.SnailKing:
                    if (this.Stats.SnailKingDelivered)
                    {
                        return MissionStateType.Completed;
                    }
                    if (this.Stats.SnailKingDead)
                    {
                        failedReason = MissionFailedReasonType.KingIsDead;
                        return MissionStateType.Failed;
                    }
                    break;
            }
            return MissionStateType.Running;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void ProcessObjectsToRemove()
        {
            foreach (StageObject obj in ObjectsRemove)
            {
                if (obj.IsSnail)
                {
                    this.RemoveSnail((Snail)obj);
                }
                else
                {
                    Objects.Remove(obj);
                }
                
                this.Board.RemoveObjectFromQuadtree(obj);
                // Remove all links to this object
                this.RemoveObjectLinks(obj);
            }
            ObjectsRemove.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessCollisionDetection()
        {
            foreach (Snail snail in this.Snails)
            {
                snail.DoQuadtreeCollisions(QUADTREE_STAGEOBJ_LIST_IDX);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessParticlesToRemove()
        {
            if (Particles.Count <= 0)
                return;

            // only remove particles after finishing to process all of them
            List<ParticlesEffect> particlesRemove = new List<ParticlesEffect>();

            // set the particles to remove
            foreach (ParticlesEffect particle in Particles)
            {
                if (particle.Ended)
                {
                    particlesRemove.Add(particle);
                }
            }

            // remove particles from main list
            foreach (ParticlesEffect particle in particlesRemove)
            {
                Particles.Remove(particle);
            }

            particlesRemove.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddSnail(Snail snail)
        {
            this.Snails.Add(snail);
            this.Board.AddObjectToQuadtree(snail, QUADTREE_SNAIL_LIST_IDX);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReleaseSnailKing(Vector2 pos, MovingObject.WalkDirection dir)
        {
            SnailKing snailKing = (SnailKing)this.StageData.GetObject(Snail.KING_ID);
            this.ReleaseSnail(snailKing, pos, dir);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReleaseSnail(Vector2 pos, MovingObject.WalkDirection dir, string snailId)
        {
            Snail snail = (Snail)this.StageData.GetObject(snailId);
            this.ReleaseSnail(snail, pos, dir);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReleaseSnail(Snail snail, Vector2 pos, MovingObject.WalkDirection dir)
        {
            snail.X = pos.X + (Stage.CurrentStage.Board.TileWidth / 2); /*+ (snail.Sprite.BoundingBox.Width / 2)*/
            snail.Y = pos.Y + Stage.CurrentStage.Board.TileHeight;
            // Right its the default direction so we don't need to set it
            snail.SetDirection(dir);
            snail.UpdateBoundingBox();

            if (snail.AllowStageStatistics)
            {
                Stats.NumSnailsReleased++;
                Stats.NumSnailsToRelease--;
                Stats.NumSnailsActive++;
            }
            Stage.CurrentStage.AddSnail(snail);
            snail.OnEnterStage();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessObjectsToAdd()
        {
            foreach (StageObject obj in this.ObjectsAdd)
            {
                obj.UpdateBoundingBox(); // Force a BB update for new objects
                this.AddObject(obj, true);
            }
            this.ObjectsAdd.Clear();
        }

        /// <summary>
        /// This method should be called to add objects after the game has started
        /// The objects are added to the add cache list
        /// </summary>
        public void AddObjectInRuntime(StageObject obj)
        {
#if DEBUG
            if (this.ObjectsAdd.Contains(obj))
            {
                throw new SnailsException("Object was added twice to the ObjectsAdd list!!");
            }
#endif
            this.ObjectsAdd.Add(obj);
        }

        /// <summary>
        /// Specific to stage editor
        /// This is equal to AddObject() with the diference that it is public
        /// AddObject was made private because it shouldn't be added in runtime
        /// Runtime objects should use AddObjectInRuntime()
        /// </summary>
        public void AddObjectFromStageEditor(StageObject obj)
        {
            this.AddObject(obj, false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddObject(StageObject obj)
        {
            this.AddObject(obj, false);
        }

        /// <summary>
        /// This adds an object
        /// It should be called only in loadtime because this method does not raise OnAddedToStage() event in the object
        /// </summary>
        private void AddObject(StageObject obj, bool lauchOnAddEvent)
        {
#if DEBUG
            if (this.Objects.Contains(obj))
            {
                throw new SnailsException("Object was added twice to the Objects list!!");
            }
#endif
            Objects.Add(obj);
            if (obj.CanCollide)
            {
                Board.AddObjectToQuadtree(obj, QUADTREE_STAGEOBJ_LIST_IDX);
            }

            if (obj is Liquid)
            {
                HasWater = true;
                this.ForegroundWaterDrawList.Add(obj);
                this.LiquidObjects.Add(obj as Liquid);
            }
            else if (obj is TileObject)
            {
                // TRICKY: we use Insert() instead of Add() to sort correctly tile objects against water objects.
                // Dynamite Box objects are different from Box objects, they remain on the stage until explode, while Box only
                // appears while deploying. Thats why boxes don't appear above water (what appear behind is a Tile)
                this.ForegroundWaterDrawList.Insert(0, obj);
            }
            else
            {
                if (obj.DrawInForeground)
                {
                    this.ForegroundObjectsDrawList.Add(obj);
                }
                else
                {
                    this.BackgroundObjectsDrawList.Add(obj);
                }
            }

            if (lauchOnAddEvent)
            {
                obj.OnAddedToStage();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveObject(StageObject obj)
        {
            obj.StopSamples();
            obj.DynamicFlags = StageObjectDynamicFlags.IsDisposed;
            this.ObjectsRemove.Add(obj);

            if (obj is Liquid)
            {
                this.LiquidObjects.Remove(obj as Liquid);
                this.ForegroundWaterDrawList.Remove(obj);
            }
            else if (obj is TileObject)
            {
                this.ForegroundWaterDrawList.Remove(obj);               
            }
            else
            {
                if (obj.DrawInForeground)
                {
                    this.ForegroundObjectsDrawList.Remove(obj);
                }
                else
                {
                    this.BackgroundObjectsDrawList.Remove(obj);
                }
            }

            if (obj.IsSnail)
            {
                Snail snail = (Snail)obj;
                if (snail.AllowStageStatistics && !snail._inactiveAccounted)
                {
                    Stage.CurrentStage.Stats.NumSnailsActive--;
                    snail._inactiveAccounted = true;
                }
            }
            obj.OnStageRemoved();
		}

        /// <summary>
        /// 
        /// </summary>
        private void RemoveSnail(Snail snail)
        {
            this.Snails.Remove(snail);
            if (snail.AllowStageStatistics && !snail._deathAccounted)
            {
                this.Stats.NumSnailsDisposed++;
                snail._deathAccounted = true;
            }
            if (snail.IsSnailKing)
            {
                this.Stats.SnailKingDead = true;
            }
        }

        /// <summary>
        /// Removes all links to the "obj" from all other objects
        /// </summary>
        public void RemoveObjectLinks(StageObject obj)
        {
            foreach (StageObject stageObj in this.Objects)
            {
                if (stageObj.WithLinks)
                {
                    if (stageObj.LinkedObjects.Contains(obj))
                    {
                        stageObj.LinkedObjects.Remove(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetObjectLinks()
        {
            // Object linking
            foreach (StageObject obj in Objects)
            {
                if (string.IsNullOrEmpty(obj.LinkString))
                    continue;
                string[] links = obj.LinkString.Split(';');

                foreach (string link in links)
                {
                    StageObject linkedObj = this.GetObjectByUid(link);
#if DEBUG
                    if (linkedObj == null)
                    {
                        throw new SnailsException("Linked object '" + link + "' not found in object list.");
                    }
#endif
                    if (linkedObj != null)
                    {
                        obj.AddLinkedObject(linkedObj);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private StageObject GetObjectByUid(string uid)
        {
            foreach (StageObject obj in this.Objects)
            {
                if (obj.UniqueId == uid)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnObjectPicked(PickableObject.PickableType type, int quantity)
        {
            if (PickableObject.IsTool(type))
            {
                ToolObjectType toolType = (ToolObjectType)Enum.Parse(typeof(ToolObjectType), type.ToString(), true);
                this.StageHUD._toolsMenu.AddToolQuantity(toolType, quantity); // add quantity to tool (if tool doesn't exist create a new one)
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisposeObject(StageObject obj)
        {
            this.RemoveObject(obj);
            obj.StaticFlags = StageObjectStaticFlags.None;
            obj.DynamicFlags = StageObjectDynamicFlags.IsDisposed | StageObjectDynamicFlags.IsDead;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendExplosionNotification(Explosion explosion)
        {
            foreach (StageObject obj in this.Objects)
            {
                if (!obj.IsDead && !obj.IsDisposed)
                {
                    obj.OnExplosion(explosion);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ToggleTimeWarp()
        {
            if (Stage.CurrentStage.InTimeWarp)
            {
                this.InTimeWarp = false;
                BrainGame.SampleManager.ChangePlayingPitch(0f);
                GameplayScreen.Instance.TimeMultiplier = Stage.NORMAL_TIME_MULTIPLIER;
            }
            else
            {
               // BrainGame.GameTime.Multiplier = Stage.TIME_WARP_MULTIPLIER;
                GameplayScreen.Instance.TimeMultiplier = Stage.TIME_WARP_MULTIPLIER;
                this.InTimeWarp = true;
                BrainGame.SampleManager.ChangePlayingPitch(0.7f);
            }

            if (Stage.CurrentStage.State == Stage.StageState.Startup)
            {
                this.StartupEnded();
            }

            this.StageHUD.TimeWarpChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public void PauseGame()
        {
#if STAGE_EDITOR
            if (StageEditor.StageEditor.IsActive)
            {
                return;
            }
#endif
            if (!this.IsPaused)
            {
                this.IsPaused = true;
                BrainGame.SampleManager.PauseAll();
                BrainGame.MusicManager.PauseMusic();
                GameplayScreen.Instance.PopUp(ScreenType.InGameOptions.ToString(), false);
                
                if (this._gameplayRecorder.Enabled)
                {
                    this._gameplayRecorder.Pause();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResumeGame()
        {
            this.IsPaused = false;

            // restore the multiplier
//            BrainGame.GameTime.Multiplier = this._saveTimeMultiplier;

            BrainGame.SampleManager.ResumeAll();
            BrainGame.MusicManager.ResumeMusic();
            BrainGame.GameCursor.Visible = SnailsGame.GameSettings.ShowCursor;
            BrainGame.SampleManager.UseAudibleBoundingSquare = true;
            if (this._gameplayRecorder.Enabled)
            {
                this._gameplayRecorder.Resume();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndInspectingHints()
        {
            this.StageHUD.HideIncomingMessage();
            this.State = StageState.Playing;
            this.HintManager.HideHint();
            this.HintManager.IncrementHint();
            this.StageHUD.ControlButtonsVisible = true;
            this.StageHUD._toolsMenu.ShowHintButton();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartIspectingHints()
        {

            Player.PlayerStageStats playerStats = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(Stage.CurrentStage.LevelStage.StageId);
#if DEBUG
            // Isto só acontece quando se começa um nível directamente sem o ter desbloqueado (em debug)
            if (playerStats == null)
            {
                playerStats = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.UnlockStage(Stage.CurrentStage.LevelStage.StageId);
            }
#endif
            if (playerStats != null)
            {
                if (!playerStats.Completed) // If level was already completed, don't count the hint...
                {
                    playerStats.HintsTaken = Math.Max(playerStats.HintsTaken, Stage.CurrentStage.HintManager.CurrentHintIndex + 1);
                    SnailsGame.ProfilesManager.Save();
                }
            }

            this.State = StageState.InspectingHints;
            this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.InspectHints);
            this.HintManager.ShowHint();
            this.StageHUD.ControlButtonsVisible = false;
            this.StageHUD._toolsMenu.HideHintButton();
            this.StageHUD._toolsMenu.DeselectTool();
        }

        /// <summary>
        /// 
        /// </summary>
        public void QuitStage()
        {
            BrainGame.MusicManager.StopMusic();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowTutorialTopic(int topicId, bool showIfAlreadyViewed)
        {
            this.ShowTutorialTopics(new int[1] { topicId }, showIfAlreadyViewed);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowTutorialTopics(int [] topicsIds, bool showIfAlreadyViewed)
        {
            HowToPlayScreen.PopUp(SnailsGame.Tutorial.GetTopics(topicsIds), true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndMission()
        {
            BrainGame.SampleManager.StopAll();
            if (SnailsGame.GameSettings.UseGamepad)
            {
                BrainGame.GameCursor.Visible = false;
            }

            this.ComputeStageScore();

#if BETA_TESTING
            this.SendStageStatsToServer();
#endif      
            if (this._gameplayRecorder.IsRecording)
            {
                this._gameplayRecorder.Stop();
#if FORMS_SUPPORT
                BrainGame.ScreenNavigator.Disable();
                SaveGameplayForm form = new SaveGameplayForm();
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    this.GameplayRecorder.Save(form.Filename, form.Description);
                }
                BrainGame.ScreenNavigator.Enable();
#endif
            }

            this._gameplayRecorder.Enabled = false;
            
            SnailsGame.ProfilesManager.Save();
            if (this.MissionState == MissionStateType.Completed)
            {
                GameplayScreen.Instance.NavigateTo(ScreenType.StageCompleted.ToString());
            }
            else
            {
                GameplayScreen.Instance.Navigator.GlobalCache.Set(GlobalCacheKeys.MISSION_FAILED_REASON, this._missionFailedReason);
                GameplayScreen.Instance.NavigateTo(ScreenType.MissionFailed.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RestartMission()
        {
            BrainGame.SampleManager.StopAll();
            BrainGame.MusicManager.FadeMusic(0, 500);
            if (this._gameplayRecorder.Enabled)
            {
                this._gameplayRecorder.Stop();
            }

            GameplayScreen.Instance.Navigator.GlobalCache.Set(GlobalCacheKeys.SHOW_TIP_ON_LOADING, false);
            GameplayScreen.Instance.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_STAGE_INFO, false);
            GameplayScreen.Instance.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SHOW_XBOX_HELP, false);
            GameplayScreen.Instance.Navigator.GlobalCache.Set(GlobalCacheKeys.STAGE_START_SCREEN_CALLER, ScreenType.Gameplay);

            GameplayScreen.Instance.NavigateTo(ScreenType.StageStart.ToString(), ScreenTransitions.LeafsClosing, null);

            SnailsGame.ProfilesManager.CurrentProfile.StagesFailedCount++;

        }

        #region Board methods
        // This methods look strange in here, but the truth is that they have to be here
        // because things are needed from the board and the stage

        /// <summary>
        /// In case of a board resize and ther's a crop, checks if the cropped are as tiles or objects
        /// </summary>
        public bool IsCropAreaEmpty(int newColCount, int newRowCount)
        {
            if (newColCount >= this.Board.Columns &&
                newRowCount >= this.Board.Rows)
            {
                return true; // No crop, just return
            }

            for (int i = 0; i < Math.Max(this.Board.Columns, newColCount); i++)
            {
                if (i < newColCount)
                    continue;
                for (int j = newRowCount; j < this.Board.Rows; j++)
                {
                    if (this.Board.Tiles[j, i] != null)
                    {
                        return false;
                    }
                }
            }

            // Check if there are any objects outside the new crop area
            BoundingSquare newBs = new BoundingSquare(new Vector2(0, 0), new Vector2(newColCount * this.Board.TileWidth, newRowCount * this.Board.TileHeight));
            foreach (StageObject obj in this.Objects)
            {
                if (newBs.Contains(obj.AABoundingBox) == false)
                {
                    return false;
                }
            }

            // Snails
            foreach (Snail snail in this.Snails)
            {
                if (newBs.Contains(snail.AABoundingBox) == false)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResizeBoard(int cols, int rows)
        {
            this.Board.Resize(cols, rows);
            // Remove any objects out of the board
            for (int i = 0; i < this.Objects.Count; i++)
            {
                if (this.Board.BoundingBox.Contains(this.Objects[i].AABoundingBox) == false)
                {
                    this.Objects.Remove(this.Objects[i]);
                    i--;
                }
            }

            // Remove any snails out of the board
            for (int i = 0; i < this.Snails.Count; i++)
            {
                if (this.Board.BoundingBox.Contains(this.Snails[i].AABoundingBox) == false)
                {
                    this.RemoveSnail(this.Snails[i]);
                    i--;
                }
            }
        }
        #endregion

        #region Score
        // I think this all should move to stage stats, but I don't feel like doing it...
        /// <summary>
		/// 
		/// </summary>
		public int GetUnusedToolsCount()
		{
			return this.StageHUD._toolsMenu.GetTotalTools();
		}

        /// <summary>
        /// 
        /// </summary>
        public int GetUnusedToolsPoints()
        {
            return this.StageHUD._toolsMenu.GetTotalTools() * Stage.BONUS_UNUSED_TOOL;
        }
		/// <summary>
		/// 
		/// </summary>
        public int GetCoinPoints()
        {
            return ((this.Stats.NumBronzeCoins * Stage.BONUS_BRONZE_COINS) +
                    (this.Stats.NumSilverCoins * Stage.BONUS_SILVER_COINS) +
                    (this.Stats.NumGoldCoins * Stage.BONUS_GOLD_COINS));
        }

        /// <summary>
        /// 
        /// </summary>
        private int GetTotalPointsForSeconds(int totalSeconds)
        {
            return (totalSeconds * Stage.BONUS_TIME_TAKEN);
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetTimePoints()
        {
            if (this.LevelStage._goal == GoalType.TimeAttack)
            {
                TimeSpan ts = this.Stats.Timer;
                if (ts.Milliseconds > 0)
                {
                    if (ts.Seconds < 59)
                    {
                        ts = new TimeSpan(ts.Hours, ts.Minutes, ts.Seconds + 1);
                    }
                    else
                    {
                        ts = new TimeSpan(ts.Hours, ts.Minutes + 1, 0);
                    }
                }
                return ((int)ts.TotalSeconds) * Stage.BONUS_TIME_TAKEN;
            }

            return this.GetTotalPointsForSeconds(this.GetTotalSecondsBellowTargeTime());
        }

        /// <summary>
        /// 
        /// </summary>
        public int SnailsDeliveredPoints(out int totalSnails)
        {
            totalSnails = 0;
            if (this.LevelStage._goal == GoalType.SnailKiller)
            {
                totalSnails = this.Stats.NumSnailsDead;
                return this.Stats.NumSnailsDead * Stage.BONUS_SNAILS_DELIVERED;
            }

            totalSnails = this.Stats.NumSnailsSafe;
            return this.Stats.NumSnailsSafe * Stage.BONUS_SNAILS_DELIVERED;
        }


        /// <summary>
        /// 
        /// </summary>
        public int GetTotalSecondsBellowTargeTime()
        {
            // Bonus for seconds less then the reference time
            if (Stats.Timer< this.LevelStage._targetTime)
            {
                return (int)Math.Ceiling((double)(this.LevelStage._targetTime - this.Stats.Timer).TotalSeconds);
            }
            return 0;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void SnailsStageStatsChanged()
        {
           this.StageHUD.SnailsStageStatsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SnailKingDelivered()
        {
            this.Stats.SnailKingDelivered = true;

        }

		public void IncrementBuildNr()
		{
			this.BuildNr++;
		}

        /// <summary>
        /// 
        /// </summary>
        public StageObject GetObjectById(string id)
        {
            foreach (StageObject obj in this.Objects)
            {
                if (obj.UniqueId == id)
                {
                    return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public ToolObject GetToolByType(ToolObjectType type)
        {
            foreach (ToolObject obj in this.StageHUD._toolsMenu.Tools)
            {
                if (obj.Type == type)
                {
                    return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GameLostFocus()
        {
            if (SnailsGame.GameSettings.PauseGameWhenFocusLost)
            {
                this.Cursor.GameLostFocus();
                this.PauseGame();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TutorialTopicOpened()
        {
            this.Cursor.TutorialTopicOpened();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TutorialTopicClosed()
        {
            this.Cursor.TutorialTopicClosed();
        }


#if BETA_TESTING
		/// <summary>
		/// 
		/// </summary>
		private void SendStageStatsToServer()
		{
			SnailsStageStats stats = new SnailsStageStats();
			stats.Status = (int)this._missionState;
			stats.Theme = this.LevelStage.Theme.ToString();
			stats.StageNumber = this.LevelStage.StageNr;
			stats.Goldcoins = this.Stats.NumGoldCoins;
			stats.Silvercoins = this.Stats.NumSilverCoins;
			stats.Bronzecoins = this.Stats.NumBronzeCoins;
			MedalType medal = MedalType.None;
			stats.Score = this.ComputeTotalScore(out medal);
			stats.Unusedtools = this.GetUnusedToolsCount();
			stats.TimeStarted = this._startTime;
			stats.TimeTaken = this._endTime.Subtract(this._startTime.TimeOfDay);
			stats.BuildNr = this.BuildNr;
            stats.Medal = (int)medal;
			ClosedBeta.RegisterStageStats(stats);
		}
#endif
        /// <summary>
        /// 
        /// </summary>
        public static LevelStage GetLevelStageFromDataFileRecord(DataFileRecord stageRecord)
        {
            LevelStage levelStage = new LevelStage();
            levelStage._snailsToSave = stageRecord.GetFieldValue<int>("snailsToSave");
            levelStage._snailsToRelease = stageRecord.GetFieldValue<int>("snailsToRelease");
            levelStage._targetTime = stageRecord.GetFieldValue<TimeSpan>("targetTime");
            levelStage._goal = (GoalType)stageRecord.GetFieldValue<int>("goal");
            levelStage.AvailableInDemo = stageRecord.GetFieldValue<bool>("availableInDemo");
            levelStage.ThemeId = (ThemeType)stageRecord.GetFieldValue<int>("theme");
            levelStage.StageId = stageRecord.GetFieldValue<string>("id");
            levelStage.StageKey = stageRecord.GetFieldValue<string>("key", null);
            levelStage.IsCustomStage = stageRecord.GetFieldValue<bool>("isCustomStage", false);
            levelStage.YouTubeUrl = stageRecord.GetFieldValue<string>("youTubeUrl", null);

            DataFileRecord goldMedalRecord = stageRecord.SelectRecord("goldMedalCriteria");
            levelStage._goldMedalTime = goldMedalRecord.GetFieldValue<TimeSpan>("timeNeeded");
            levelStage._goldMedalScore = goldMedalRecord.GetFieldValue<int>("totalScore");

            return levelStage;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshTiles()
        {
            this.Board.RefreshTiles();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetLayers(List<SnailsBackgroundLayer> layers)
        {
            this.Layers.Clear();
            foreach (SnailsBackgroundLayer layer in layers)
            {
                this.Layers.Add(layer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CountPickableObjects(PickableObject.PickableType objType)
        {
            int count = 0;
            foreach (StageObject obj in this.Objects)
            {
                if (obj is PickableObject)
                {
                    PickableObject pickObj = (PickableObject)obj;
                    if (pickObj._pickableType == objType)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeStageScore()
        {
            int totalSnails = 0;

            this.Stats.TimePointsWon = this.GetTimePoints();
            this.Stats.SnailsDeliveredPointsWon = this.SnailsDeliveredPoints(out totalSnails);
            this.Stats.CoinPointsWon = this.GetCoinPoints();
            this.Stats.TotalScore = this.Stats.SnailsDeliveredPointsWon + this.Stats.TimePointsWon + this.Stats.CoinPointsWon;

            // if we complete the stage, than give always a bronze medal no matter how good it was
            this.Stats.MedalWon = MedalType.Bronze;
            if (this.Stats.TotalScore >= this.GoldMedalScoreCriteria.Score &&
                totalSnails >= this.GoldMedalScoreCriteria.SnailsNeeded &&
                Math.Floor(this.Stats.TimeTaken.TotalSeconds) <= Math.Floor(this.GoldMedalScoreCriteria.TimeNeeded.TotalSeconds) &&
                this.Stats.NumBronzeCoins >= this.GoldMedalScoreCriteria.BronzeCoinsNeeded &&
                this.Stats.NumSilverCoins >= this.GoldMedalScoreCriteria.SilverCoinsNeeded &&
                this.Stats.NumGoldCoins >= this.GoldMedalScoreCriteria.GoldCoinsNeeded)
            {
                this.Stats.MedalWon = MedalType.Gold;
            }
            else
                if (this.Stats.TotalScore >= this.SilverMedalScoreCriteria.Score &&
                totalSnails >= this.SilverMedalScoreCriteria.SnailsNeeded &&
                Math.Floor(this.Stats.TimeTaken.TotalSeconds) < Math.Floor(this.SilverMedalScoreCriteria.TimeNeeded.TotalSeconds) &&
                this.Stats.NumBronzeCoins >= this.SilverMedalScoreCriteria.BronzeCoinsNeeded &&
                this.Stats.NumSilverCoins >= this.SilverMedalScoreCriteria.SilverCoinsNeeded &&
                this.Stats.NumGoldCoins >= this.SilverMedalScoreCriteria.GoldCoinsNeeded)
            {
                this.Stats.MedalWon = MedalType.Silver;
            }
        }


        #region IDataFileSerializable Members

        public virtual void InitFromDataFileRecord(DataFileRecord stageRecord)
        {
            this.LightManager = new LightManager();

            this.Description = stageRecord.GetFieldValue<string>("desc");
            this.Id = stageRecord.GetFieldValue<string>("id");
            this.BuildNr = stageRecord.GetFieldValue<int>("buildNr", 0);
            this.YouTubeUrl = stageRecord.GetFieldValue<string>("youTubeUrl", null);
            this.LevelStage = Stage.GetLevelStageFromDataFileRecord(stageRecord);

            this.LightManager.LightEnabled = stageRecord.GetFieldValue<bool>("lightEnabled", false);
            this.LightManager.LightColor = stageRecord.GetFieldValue<Color>("lightColor", Color.Black);
            this._withShadows = stageRecord.GetFieldValue<bool>("withShadows", false);
            this.StartupTopicsString = stageRecord.GetFieldValue<string>("startupTutorialTopics", null);
            int centerX, centerY;
            centerX = stageRecord.GetFieldValue<int>("startupCenterX", 0);
            centerY = stageRecord.GetFieldValue<int>("startupCenterY", 0);
            this.StartupCenter = new Vector2(centerX, centerY);
            centerX = stageRecord.GetFieldValue<int>("backLayersOffsetX", 0);
            centerY = stageRecord.GetFieldValue<int>("backLayersOffsetY", 0);
            this._backgroundLayersOffset = new Vector2(centerX, centerY);

            DataFileRecordList toolRecords = stageRecord.SelectRecords("Tools\\Tool");
            foreach (DataFileRecord record in toolRecords)
            {
                string id = record.GetFieldValue<string>("id");

                ToolObject tool = (ToolObject)this.StageData.GetTool(id).Clone();
                tool.InitFromDataFileRecord(record);
                this.StageHUD._toolsMenu.AddTool(tool);
            }
            this.Board = Board.FromDataFileRecord(stageRecord.SelectRecord("Board"));
            this._withLiquids = false;

            DataFileRecordList objectRecords = stageRecord.SelectRecords("Objects\\Object");
            foreach (DataFileRecord objRecord in objectRecords)
            {
                string id = objRecord.GetFieldValue<string>("id");

                StageObject obj = StageData.GetObjectNoInitialize(id);
                obj.InitFromDataFileRecord(objRecord);
                obj.PreviousPosition = obj.Position;
                obj.LoadContent();
                obj.Initialize();
                obj.StageInitialize();
                obj.UpdateBoundingBox();
                obj.UpdateCrateCollisionBoundingBox();
                if (obj is Liquid)
                {
                    this._withLiquids = true;
                }
                this.AddObject(obj);
            }

            // Do this after adding all objects. This is because crates might have been added
            // Initialize the board
         /*   this.Board = Board.FromDataFileRecord(stageRecord.SelectRecord("Board"));*/
            Board.ComputePaths();

            foreach (StageObject obj in this.Objects)
            {
                obj.AfterBoardInitialize();
            }

            // Layers
            this.Layers = new List<SnailsBackgroundLayer>();
            DataFileRecordList backLayerRecords = stageRecord.SelectRecords("Layers\\Layer");
            foreach (DataFileRecord layerRecord in backLayerRecords)
            {
                string id = layerRecord.GetFieldValue<string>("id");
                SnailsBackgroundLayer layer = StageData.GetLayer(id);
                layer.InitFromDataFileRecord(layerRecord);
                this.Layers.Add(layer);
            }
            this.SetObjectLinks();
 
            // Medal criteria
            this.GoldMedalScoreCriteria = MedalScoreCriteria.CreateFromDataFileRecord(stageRecord.SelectRecord("goldMedalCriteria"), this);
            this.SilverMedalScoreCriteria = MedalScoreCriteria.CreateFromDataFileRecord(stageRecord.SelectRecord("silverMedalCriteria"), this);
            this.BronzeMedalScoreCriteria = MedalScoreCriteria.CreateFromDataFileRecord(stageRecord.SelectRecord("bronzeMedalCriteria"), this);

            // Unlock all tutorial topics that are used in this stage
            bool topicsMarked = false;
            foreach (StageObject obj in this.Objects)
            {
                if (obj is TutorialSign)
                {
                    if (SnailsGame.ProfilesManager.CurrentProfile != null)
                    {
                        TutorialSign sign = (TutorialSign)obj;
                        if (sign.TutorialTopics != null)
                        {
                            foreach (int topic in sign.TutorialTopics)
                            {
                                if (!SnailsGame.ProfilesManager.CurrentProfile.IsTutorialTopicRead(topic))
                                {
                                    SnailsGame.ProfilesManager.CurrentProfile.MarkTutorialTopicAsRead(topic);
                                    topicsMarked = true;
                                }
                            }
                        }
                    }
                }
            }
            if (topicsMarked)
            {
                SnailsGame.ProfilesManager.Save();
            }

            // Hints
            DataFileRecord hintRecord = stageRecord.SelectRecord("Hints");
            this.HintManager = new HintManager(this);
            if (hintRecord != null)
            {
                this.HintManager.InitFromDataFileRecord(hintRecord);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        /// <summary>
        /// When the context is StageSave, not all properties are exported and some are removed
        /// This is because this properties are global for all objects of the same type and come from StageData
        /// Only the object specific properties will matter in this case
        /// </summary>
        public virtual DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("Stage");
            record.AddField("id", this.Id);
            record.AddField("key", this.Key);
            record.AddField("buildNr", this.BuildNr);
            record.AddField("youTubeUrl", this.YouTubeUrl);
            record.AddField("desc", this.Description);
            record.AddField("snailsToSave", this.LevelStage._snailsToSave);
            record.AddField("snailsToRelease", this.LevelStage._snailsToRelease);
            record.AddField("targetTime", this.LevelStage._targetTime);
            record.AddField("goal", (int)this.LevelStage._goal);
            record.AddField("theme", (int)this.LevelStage.ThemeId);
            record.AddField("isCustomStage", this.LevelStage.IsCustomStage);
            record.AddField("availableInDemo", this.LevelStage.AvailableInDemo);
            record.AddField("lightEnabled", this.LightManager.LightEnabled);
            record.AddField("lightColor", this.LightManager.LightColor);
            record.AddField("withShadows", this._withShadows);
            record.AddField("startupTutorialTopics", this.StartupTopicsString);
            record.AddField("startupCenterX", this.StartupCenter.X);
            record.AddField("startupCenterY", this.StartupCenter.Y);
            record.AddField("backLayersOffsetX", this._backgroundLayersOffset.X);
            record.AddField("backLayersOffsetY", this._backgroundLayersOffset.Y);

            // Medal criteria
            record.AddRecord(this.GoldMedalScoreCriteria.ToDataFileRecord("goldMedalCriteria"));
            record.AddRecord(this.SilverMedalScoreCriteria.ToDataFileRecord("silverMedalCriteria"));
            record.AddRecord(this.BronzeMedalScoreCriteria.ToDataFileRecord("bronzeMedalCriteria"));
            
            // Background Layers
            DataFileRecord layerRecords = record.AddRecord("Layers");
            foreach (SnailsBackgroundLayer layer in this.Layers)
            {
                layerRecords.AddRecord(layer.ToDataFileRecord(context));
            }

            DataFileRecord toolRecords = record.AddRecord("Tools");
            foreach (ToolObject tool in this.StageHUD._toolsMenu.Tools)
            {
                // Ignore the EndMission tool
                if (tool is ToolEndMission)
                    continue;

                toolRecords.AddRecord(tool.ToDataFileRecord(context));
            }

            DataFileRecord objectRecords = record.AddRecord("Objects");
            foreach (StageObject obj in this.Objects)
            {
                objectRecords.AddRecord(obj.ToDataFileRecord(context));
            }

            record.AddRecord(this.Board.ToDataFileRecord(context));

            record.AddRecord(this.HintManager.ToDataFileRecord(context));

            return record;
        }

        #endregion

    }
}
