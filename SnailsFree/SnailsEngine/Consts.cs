    using Microsoft.Xna.Framework.Input;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;
 
namespace TwoBrainsGames.Snails
{
    public class ResourceManagerIds
    {
        public const string STAGE_THUMBNAILS = "STAGE_THUMBNAILS";
        public const string STAGE_THEME_RESOURCES = "STAGE_THEME_RESOURCES";
        public const string TUTORIAL_RESOURCES = "TUTORIAL";
        public const string PAID_SNAILS_AD = "PAID_SNAILS_AD";
    }

    public class GameCursors
    {
        public const int Default = 0;
        public const int Busy = 1;
        public const int Forbidden = 2;
        public const int Salt = 3;
        public const int SaltForbidden = 4;
        public const int Select = 0;
        public const int OutOfStock = 5;
        public const int MapPanCursor = 6;
    }

    public class GenericConsts
    {
        public const string SettingsResourceName = "game-settings";
        public const int VersionHi = 0;
        public const int VersionLo = 1;

        public static Vector2 TilesShadowDepth = new Vector2(12f, 8f);
        public static Vector2 BackgroundObjectShadowDepth = new Vector2(3f, 2f);
        public static Vector2 ForegroundObjectShadowDepth = new Vector2(10f, 7f);
        public static Vector2 SnailsShadowDepth = new Vector2(8f, 5f);
        public static Vector2 ShadowDepth = new Vector2(12f, 8f);
    }

    public class Colors   
    {
        // Mission status messages
        public static Color HudItem_MsgStatusCompleted =new Color(50, 240, 30);
        public static Color HudItem_MsgStatusFailed = new Color(255, 80, 0);
        public static Color HudItem_GoalDescription = new Color(50, 240, 30);

        public static Color ButtonsText = new Color(255, 190, 70);
        public static Color ButtonsFocusText = new Color(255, 255, 0);

        // Theme selection
        public static Color ThemeSelectionNeeded = new Color(103, 153, 255); // Theme Selection-> Theme Icon -> "Stages needed to unlock"
        public static Color ThemeSelectionNeededFromThemes = new Color(52, 245, 32);  // Theme Selection-> Theme Icon -> "X needed from y"
        public static Color ThemeStageStats = Color.White;

        // Stage selection
        public static Color StageSelectionNumber = new Color(255, 162, 0); // Stage Selection -> Stage Icon -> Stage Nr
        public static Color StageSelectionNotebookText = new Color(255, 80, 0); 
        public static Color StageSelectionNotebookHighScoreText = new Color(0, 80, 255);
        public static Color StageSelectionNotebookStageMode = new Color(0, 0, 0);

        // Colors used for theme specific captions
        public static Color[] ThemeCaptions = new Color[] { new Color (160, 235, 80),
                                                                  new Color (235, 222, 80),
                                                                  new Color (240, 40, 50),
                                                                  new Color (160, 160, 160)};
        public static Color [] ThemeValues = new Color[] { new Color(234, 234, 234),
                                                                new Color(234, 234, 234),
                                                                new Color(234, 234, 234),
                                                                new Color(234, 234, 234) };

        public static Color MenuItem = new Color(220, 175, 255); // Item text color in the menus
        public static Color MenuItemSelected = new Color(78, 255, 85); // Menu item text color when selected

        public static Color AutoSaveWarningText = new Color(255, 155, 30);
        public static Color OverscanMessageText = new Color(255, 155, 30);

        public static Color StageCompletedText = Color.Yellow;
        public static Color StageCompletedCaptions = new Color(150, 180, 250);
        public static Color StageCompletedBonusCaptions = new Color(240, 160, 255);

        public static Color MissionFailedCaptions = new Color(255, 150, 0);

        public static Color InstructionBarBackground = new Color(0, 0, 0, 150);
        
        // Tutorial
        public static Color TutorialTopicBackground = new Color(0, 50, 0, 255);
        public static Color TutorialTextColor = Color.Black;
        public static Color TutorialEnphasizeTextColor = new Color(255, 0, 0);

        // Incoming messages
        public static Color IncomingMessageInfo = new Color(50, 240, 30);
        public static Color IncomingMessageError = new Color(255, 80, 0);
        public static Color IncomingMessageHints = new Color(124, 200, 255);

        // Stage Hud
        public static Color StageHUDInfoColor = new Color(230, 250, 30);
        public static Color StageHUDTimerLowColor = new Color(255, 0, 0);

        // Ingame shadow colors
        public static Color IngameShadows = new Color(0, 0, 0, 180);

        // Screens background colors
        public static Color MainMenuScrBkColor = new Color(60, 185, 230); // Blueish
        public static Color OptionsScrBkColor = new Color(60, 185, 230); // Blueish
        public static Color AutoSaveScrBkColor = new Color(110, 230, 42); // Greenish
        public static Color OverscanScrBkColor1 = Colors.MainMenuScrBkColor; // Used when overscan is called in startup
        public static Color OverscanScrBkColor2 = new Color(110, 230, 42); // Used when overscan is called from the options
        public static Color ThemeSelectionScrBkColor = new Color(152, 244, 100); //Greenish
        public static Color CreditsScrBkColor = new Color(240, 160, 120); // Redish
        public static Color AwardsScrBkColor = new Color(60, 185, 230); // Redish

        // Backbuffer clear color
        public static Color BBBrainsLogoScreen = new Color(255, 255, 255);
        public static Color BBOverscanScreen = new Color(200, 0, 0);
        public static Color BBDefaultColor = new Color(0, 0, 0);

        public static Color ControllerHelp = new Color(200, 255, 112); 

        // Wp scrollable panels background
        public static Color ScrollablePanel = new Color(0, 0, 0, 50); 

    }

    class GlobalCacheKeys
    {
        public const string OVERSCAN_CALLER_SCREEN = "OVERSCAN_CALLER_SCREEN"; // Set this to ScreenType from where the last navigation ocorred

        public const string MAIN_SCREEN_STARTUP_MODE = "MAIN_SCREEN_STARTUP_MODE";
        public const string SELECTED_STAGE_INFO = "SELECTED_STAGE_INFO";
        public const string AUTO_SELECT_STAGE = "AUTO_SELECT_STAGE";
        public const string STAGE_START_SHOW_STAGE_INFO = "STAGE_START_SHOW_STAGE_INFO";
        public const string STAGE_START_SHOW_XBOX_HELP = "STAGE_START_SHOW_XBOX_HELP";

        public const string MISSION_FAILED_REASON = "MISSION_FAILED_REASON";
        public const string STAGE_CHANGED_BY_THE_EDITOR = "STAGE_CHANGED_BY_THE_EDITOR";
        public const string NEW_GAME_BACK_SCREEN = "NEW_GAME_BACK_SCREEN";
        public const string THEME_UNLOCK_NEXT_SCREEN = "THEME_UNLOCK_NEXT_SCREEN";
        public const string THEME_UNLOCK_NEXT_SCREEN_GROUP = "THEME_UNLOCK_NEXT_SCREEN_GROUP";
        public const string THEME_UNLOCK_OPEN_TRANSITION = "THEME_UNLOCK_OPEN_TRANSITION";
        public const string THEME_UNLOCK_UNLOCKED_THEME = "THEME_UNLOCK_UNLOCKED_THEME";
        public const string NEXT_STAGE_AVAILABLE = "NEXT_STAGE_AVAILABLE";
        public const string FOOTER_MESSAGE_POSITION = "FOOTER_MESSAGE_POSITION";
        public const string OPTIONS_STARTUP_MODE = "OPTIONS_STARTUP_MODE";
        public const string OPTIONS_LAST_SELECTED_ITEM = "OPTIONS_LAST_SELECTED_ITEM";
        public const string CREDITS_SCREEN_CALLER = "CREDITS_SCREEN_CALLER";
        public const string GAMEPLAY_RECORDER_LOAD_SOLUTION = "GAMEPLAY_RECORDER_LOAD_SOLUTION";
        public const string GAMEPLAY_RECORDER_SAVE_SOLUTION = "GAMEPLAY_RECORDER_SAVE_SOLUTION";
        public const string INTRO_PICTURE_STATE = "INTRO_PICTURE_STATE";
        public const string TUTORIAL_TOPIC = "TUTORIAL_TOPIC";
        public const string TUTORIAL_TOPIC_LIST = "TUTORIAL_TOPIC_LIST";
        public const string CURRENT_SCREEN = "CURRENT_SCREEN";
        public const string SHOW_TIP_ON_LOADING = "SHOW_TIP_ON_LOADING";
        public const string SNAILS_PAID_AD_SEEN_ON_LOADING = "SNAILS_PAID_AD_SEEN_ON_LOADING";
        public const string STAGE_START_SCREEN_CALLER = "STAGE_START_SCREEN_CALLER";
    }

    public class AudioTags
    {
        public const string THEME_UNLOCKED = "sfx/theme-unlocked";

        public const int MUSIC_FADE_MSECONDS = 500;
        // sfx
        public const string MENU_CLOSED = "sfx/menu-closed";
        public const string MENU_SHOWN = "sfx/menu-shown";

        public const string MENU_ITEM_SELECTED = "sfx/menu-item-selected";
        public const string MENU_ITEM_FOCUS = "sfx/menu-item-focus";

        public const string BUTTON_SELECTED = "sfx/menu-item-selected";
        public const string BUTTON_FOCUS = "sfx/menu-item-focus";
        public const string BUTTON_SHOWN = "sfx/menu-item-shown";

        public const string CREDIT_YEAH = "sfx/gold-medal";

        public const string SLIDER_VALUE_CHANGED = "sfx/slider-move";

        public const string STAGE_PANEL_HIDDEN = "sfx/menu-closed";

        public const string BOARD_HIDDEN = "sfx/menu-closed";
        public const string BOARD_SHOWN = "sfx/menu-item-shown";

        public const string BOX_DROP = "sfx/objects/box-drop";
        public const string SALT_DROP = "sfx/objects/salt";
        public const string SALT_DISSOLVING = "sfx/objects/salt-dissolving";

        public const string POINT_INCREMENT = "sfx/point-increment";
        public const string CAPTION_SHOW = "sfx/caption-show";
        public const string TIME_UP = "sfx/time-up";
        public const string TIMER_TICK = "sfx/timer-tick";
        public const string CRATE_TIMER_TICK = "sfx/objects/crate-timer-tick";
        public const string FUSE = "sfx/objects/fuse";
        public const string DYNAMITE_BEEP = "sfx/objects/dynamite-beep";
        public const string TEXT_BLINK = "sfx/text-blink";
        public const string TOGGLE_ALL_STAGES_UNLOCKED = "sfx/dynamite_beep";
        public const string TOOL_INVALID = "sfx/tool_invalid";

        public const string LEAFS_OPEN_TRANSITION = "sfx/leafs-open";
        public const string LEAFS_CLOSE_TRANSITION = "sfx/leafs-close";

        public const string FIRE = "sfx/objects/fire";
        public const string MEDAL = "sfx/medal-hitting-board";
        public const string FAIL_STAMP = "sfx/fail-stamp";

        public const string OBJECT_KILLED_FIRE = "sfx/objects/object_burn";

        // Snail 
        public const string SNAIL_KILLED_FIRE = "sfx/objects/snail_burn";
        public const string SNAIL_DEATH_1 = "sfx/objects/snail_death_1";
        public const string SNAIL_DEATH_2 = "sfx/objects/snail_death_2";
        public const string SNAIL_DEATH_3 = "sfx/objects/snail_death_3";
        public const string SNAIL_DEATH_4 = "sfx/objects/snail_death_4";
        public const string SNAIL_DRAG = "sfx/objects/snail-drag";
        public static string SNAIL_ENTRANCE
        { get { return string.Format("sfx/{0}/entrance", Stage.CurrentStage.LevelStage.ThemeId); } }
        public const string SNAIL_ENTERING_EXIT = "sfx/objects/snail_entering";
        public const string EYE_BLINK_SAMPLE = "sfx/objects/snails_eye_blink_echo";
        public const string SNAIL_BREATH_UNDERWATER = "sfx/objects/snail-breath-underwater";
        public const string SNAIL_ENTERING_STAGE = "sfx/objects/snail-entering-stage";

        // Tile impacts
        public static string TILE_IMPACT
        { get { return string.Format("sfx/{0}/tile_impact", Stage.CurrentStage.LevelStage.ThemeId); } }
        public static string BREAKBLE_TILE_IMPACT
        { get { return string.Format("sfx/{0}/breakable_tile_impact", Stage.CurrentStage.LevelStage.ThemeId); } }
        public static string TILE_IMPACT_UNDERWATER = "sfx/objects/underwater-inpact";

        public const string TRAMPOLINE_JUMP = "sfx/objects/trampoline";

        public const string SPIKES = "sfx/objects/spikes";
        public const string SPIKES_HIT_SNAIL = "sfx/objects/spikes-hit-snail";
        public const string SPIKES_CLOSING = "sfx/objects/spikes-closing";

        public const string ROCKET = "sfx/objects/rocket";
        public const string ROCKET_UNDERWATER = "sfx/objects/rocket-underwater";
        public const string APPLE_BITE = "sfx/objects/apple_bite";
        public const string C4_BEEP = "sfx/objects/C4-beep";
        public const string SWITCH_LEVER_MOVING = "sfx/objects/switch-lever-moving";
        public const string SWITCH_LEVER_ACTIVATING = "sfx/objects/switch-lever-activating";
        public const string TOOL_FOUND = "sfx/objects/tool_found";
        public const string COIN_FOUND = "sfx/objects/coin_found";
        public const string TOOL_SELECTION = "sfx/tool_selection";
        public const string GOLD_MEDAL = "sfx/gold-medal";
        public const string STAGE_COMPLETE_TICTIC = "sfx/stage-completed-tic-tic";
        public const string INCOMMING_START = "sfx/incomming_start";
        public const string INCOMMING_END = "sfx/incomming_end";
        public const string MISSION_FAILED = "sfx/mission_failed";
        public const string MISSION_FAILED_TIME_ATTACK = "sfx/mission_failed";

        //  Musics
        public const string MAIN_MENU_THEME = "musics/snail-menu-theme";

        // Fade in out box
        public const string FADE_BOX_IN = "sfx/objects/fade-box-in";
        public const string FADE_BOX_OUT = "sfx/objects/fade-box-out";
        // Lasers
        public const string LASER = "sfx/objects/laser-beam";
        public const string LASER_POWER_UP = "sfx/objects/laser-beam-power-up";
        public const string ROTAION_CONTROLLER_TICK = "sfx/objects/rotation-controller-tick";
        public const string ELECTRICITY = "sfx/objects/laser-switch-electricity";

        public const string SWITCH = "sfx/objects/switch";
        public const string SNAILS_SWITCH_SOUP = "sfx/objects/snail-switch-soup";
        public const string SNAILS_SWITCH_PROCESSING = "sfx/objects/snail-switch-processing";
        public const string SNAILS_SWITCH_SUCK = "sfx/objects/snail-switch-suck";
        public const string SNAIL_THROW = "sfx/objects/snail-throw";

        public const string BUBBLE_POP = "sfx/objects/bubble-pop";
        public const string BUBBLE_POP_2 = "sfx/objects/bubble-pop-2";
        public const string BUBBLE_POP_3 = "sfx/objects/bubble-pop-3";
        public const string WATER_SPLASH = "sfx/objects/water-splash";
        public const string UNDERWATER_BUBBLES = "sfx/objects/underwater-bubbles";

        public const string EXPLOSION_1 = "sfx/objects/explosion-1";
        public const string EXPLOSION_2 = "sfx/objects/explosion-2";
        public const string EXPLOSION_3 = "sfx/objects/explosion-3";
        public const string EXPLOSION_UNDERWATER = "sfx/objects/explosion_underwater";

        public const string WATER_PUMP_ENGINE = "sfx/objects/water-pump";
        public const string WATER_TAP = "sfx/objects/water-tap";
        public const string WATER = "sfx/objects/water";


        public const string STAGE_COMPLETED = "sfx/stage-completed";
    }

    public class TutorialTags
    {
        public const int SNAIL_DELIVERY_MODE = 101;
        public const int EXTERMINATOR_MODE = 107;
        public const int TIME_ATACK_MODE = 106;
        public const int SNAIL_KING_MODE = 108;
        public const int THE_CRATE_OBJECT = 102;
        public const int CRATE_INVALID = 116;
        public const int MISSION_COMPLETED_MORE_POINTS = 103;
        public const int THE_COPPER_OBJECT = 109;
        public const int ABOUT_COINS = 110;
        public const int SALT_TOOL = 111;
        public const int TIME_WARP = 112;
    }

    // Fonts
    public class FontResources
    {
        public const string MAIN_FONT_SMALL = "fonts/main-font-small";
        public const string MAIN_FONT_MEDIUM = "fonts/main-font-medium";
        public const string MAIN_FONT_BIG = "fonts/main-font-big";
        public const string TUTORIAL = "fonts/notebook";
    }

    public class SpriteResources
    {

        // StageHud
        public const string END_MISSION_BUTTON = "spriteset/StageHUD/ToolIcon";
        public const string END_MISSION_SELECTED_BUTTON = "spriteset/StageHUD/ToolIcon";
        public const string RESTART_BUTTON = "spriteset/StageHUD/ToolIcon";
        public const string RESTART_SELECTED_BUTTON = "spriteset/StageHUD/ToolIcon";
        public const string TOOLS_SHORTCUT_KEYS = "spriteset/StageHUD/ShortcutKeys";
        public const string TOOL_INVALID_FEEDBACK = "spriteset/StageHUD/ToolInvalid";

        // Cursors
        public const string PLAYER_CURSOR_DEFAULT = "spriteset/player-cursor/DefaultCursor";
        public const string PLAYER_CURSOR_BUSY = "spriteset/player-cursor/BusyCursor";
        public const string PLAYER_CURSOR_FORBIDDEN = "spriteset/player-cursor/ForbiddenCursor";
        public const string PLAYER_CURSOR_SALT = "spriteset/player-cursor/SaltCursor";
        public const string PLAYER_CURSOR_SALT_FORBIDDEN = "spriteset/player-cursor/SaltCursorForbidden";
        public const string PLAYER_CURSOR_OUTOFSTOCK = "spriteset/player-cursor/OutOfStockCursor";
        public const string PLAYER_CURSOR_CAM_PAN = "spriteset/player-cursor/PanCursor";
        public const string PLAYER_CURSOR_DROP = "spriteset/player-cursor/DropCursor";
        public const string PLAYER_CURSOR_END_MISSION = "spriteset/player-cursor/EndMissionCursor";
        public const string PLAYER_CURSOR_RESTART_MISSION = "spriteset/player-cursor/RestartCursor";


        // Stage objects
        public const string FIRE_DEATH_ANIMATION = "spriteset/stage-objects/FireDeath";
    }
}

