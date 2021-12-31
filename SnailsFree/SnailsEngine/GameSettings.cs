using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects.ParticlesEffects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;

#if XMLDATAFILE_SUPPORT
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using Microsoft.Xna.Framework;
#endif

namespace TwoBrainsGames.Snails.Configuration
{
    public class GameSettings : BrainSettings, IDataFileSerializable
    {
        public enum GameEntryPoint
        {
            Beginning,
            MainMenu,
            StageBriefing,
            StageEditor,
            Awards
        }

        public enum PresentationType
        {
            HD,
			LD,
			LDW
        }

        #region Consts
        public const string CustomStagesExtension = "customStage";
        public const string GAME_SETTINGS_FILE = "snails_debug_settings.xml";
        public const string StagesOutputFolder = @"..\..\..\..\SnailsResourcesCustom";
        public const string StageDataOutputFolder = @"..\..\..\..\SnailsResourcesCustom\Stages";
        #endregion


        public bool ShowBoardFrames;
        public bool ShowBoardCoordinates;
        public bool ShowPaths;
        public bool ShowObjectIds;
        public bool ShowQuadtree;
        public bool ShowBoardGrid;
        public bool ShowTiles;
        public bool ShowTheInstructionBar;
        public bool ShowStageStats;
        public bool ShowMapScrollIndicators;
        public bool ShowConfirmationMenus; // if true, yes/no confirmation menu is displayed, when quiting game, stages, etc
        public bool PauseGameWhenFocusLost;
        public bool PauseInTutorial;
        public bool AllowToggleFullScreen;
        public int StartupStageNr;
        public ThemeType StartupTheme;
        public bool UseButtonIcons;
        public bool ShowContinueOption;
        public bool AllowContinueFromMainMenu;
        public bool ShowOptionsInMainMenu;

        public GameEntryPoint EntryPoint;
        public bool DeletePlayerProfile; // If true, the player profile is deleted on started 
        public bool MinimapVisible;
        public string CustomStagesFolder;
        public bool AllowCustomStages;
        public bool AllowDebugWindow;
        public bool AllowCustomUserDataFolder;
        public bool AllowOverscanAdjustment;
        public bool WithAppStore;
        public bool BackQuitsGameOnIntroPicture;
        public bool WithToolSelectionShortcutKeys;
        public int MaxTools;
        public float MaxZoomOut;
        public float LeafTransitionScale;
        public bool ShowEULA;
        public bool ShowGameSettingsWindow;
        public bool ShowCloseTutorialMessage;
        public bool ShowCloseTutorialShortcut;
        public bool ShowBackButtonInThemeSelection;
        public bool ShowCursor;
        public bool ShowAutoSaveScreen;
        public bool ShowGameTitleInPause;
        public bool ShowQuitOptions;
        public bool ShowFooterMessage;
        public bool AsyncProfileLoading;
        public bool UseWaterEffect;
        public string GameplayRecordingPath;
        public bool AllStagesUnlocked;
        public LanguageType Languages;
        public float DefaultSoundVolume;
        public float DefaultMusicVolume;
        public bool SupportsRating;
        public bool ShowToolWithCursor;
        public bool AllowYouTubeVideos;
        public bool PlayThemeMusic;

        public bool AllowRate
        {
            get { return this.WithAppStore; }
        }

        public PresentationType PresentationMode {
			get;
			private set;
       
        }

        public GameSettings()
        {

        }

        public bool SingleLanguage
        {
            get
            {
                return (this.Languages == LanguageType.English ||
                        this.Languages == LanguageType.French ||
                        this.Languages == LanguageType.German ||
                        this.Languages == LanguageType.Italian ||
                        this.Languages == LanguageType.Portuguese ||
                        this.Languages == LanguageType.Spanish);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.PauseInTutorial = this.GetValue<bool>("PauseInTutorial", ConfigSectionType.Game);
            this.UseButtonIcons = this.GetValue<bool>("UseButtonIcons", ConfigSectionType.Game);
            this.ShowContinueOption = this.GetValue<bool>("ShowContinueOption", ConfigSectionType.Game);
            this.AllowContinueFromMainMenu = this.GetValue<bool>("AllowContinueFromMainMenu", ConfigSectionType.Game);
            this.ShowOptionsInMainMenu = this.GetValue<bool>("ShowOptionsInMainMenu", ConfigSectionType.Game);
            this.ShowConfirmationMenus = this.GetValue<bool>("ShowConfirmationMenus", ConfigSectionType.Game);
            this.MinimapVisible = this.GetValue<bool>("MinimapVisible", ConfigSectionType.Game);
            this.ShowTheInstructionBar = this.GetValue<bool>("ShowTheInstructionBar", ConfigSectionType.Game);
            this.UseWaterEffect = this.GetValue<bool>("UseWaterEffect", ConfigSectionType.Game);
            this.AllowToggleFullScreen = this.GetValue<bool>("AllowToggleFullScreen", ConfigSectionType.Game);
            this.ShowBackButtonInThemeSelection = this.GetValue<bool>("ShowBackButtonInThemeSelection", ConfigSectionType.Game);
            this.ShowEULA = this.GetValue<bool>("ShowEULA", ConfigSectionType.Game);
            this.AllowCustomUserDataFolder = this.GetValue<bool>("AllowCustomUserDataFolder", ConfigSectionType.Game);
            this.AllowOverscanAdjustment = this.GetValue<bool>("AllowOverscanAdjustment", ConfigSectionType.Game);
            this.WithAppStore = this.GetValue<bool>("WithAppStore", ConfigSectionType.Game);
            this.BackQuitsGameOnIntroPicture = this.GetValue<bool>("BackQuitsGameOnIntroPicture", ConfigSectionType.Game);
            this.WithToolSelectionShortcutKeys = this.GetValue<bool>("WithToolSelectionShortcutKeys", ConfigSectionType.Game);
            this.MaxTools = this.GetValue<int>("MaxTools", ConfigSectionType.Game);
            this.MaxZoomOut = this.GetValue<float>("MaxZoomOut", ConfigSectionType.Game);
            this.ShowCloseTutorialMessage = this.GetValue<bool>("ShowCloseTutorialMessage", ConfigSectionType.Game);
            this.ShowCloseTutorialShortcut = this.GetValue<bool>("ShowCloseTutorialShortcut", ConfigSectionType.Game);
            this.ShowCursor = this.GetValue<bool>("ShowCursor", ConfigSectionType.Game);
            this.ShowAutoSaveScreen = this.GetValue<bool>("ShowAutoSaveScreen", ConfigSectionType.Game);
            this.ShowGameTitleInPause = this.GetValue<bool>("ShowGameTitleInPause", ConfigSectionType.Game);
            this.ShowQuitOptions = this.GetValue<bool>("ShowQuitOptions", ConfigSectionType.Game);
            this.ShowFooterMessage = this.GetValue<bool>("ShowFooterMessage", ConfigSectionType.Game);
            this.AsyncProfileLoading = this.GetValue<bool>("AsyncProfileLoading", ConfigSectionType.Game);
            this.ShowGameSettingsWindow = this.GetValue<bool>("ShowGameSettingsWindow", ConfigSectionType.Game);
            this.Languages = (LanguageType)Enum.Parse(typeof(LanguageType), this.GetValue<string>("Languages", ConfigSectionType.Game), true);
            this.DefaultSoundVolume = this.GetValue<float>("DefaultSoundVolume", ConfigSectionType.Game);
            this.DefaultMusicVolume = this.GetValue<float>("DefaultMusicVolume", ConfigSectionType.Game);
            this.SupportsRating = this.GetValue<bool>("SupportsRating", ConfigSectionType.Game);
            this.ShowToolWithCursor = this.GetValue<bool>("ShowToolWithCursor", ConfigSectionType.Game);
            this.AllowYouTubeVideos = this.GetValue<bool>("AllowYouTubeVideos", ConfigSectionType.Game);
            this.PlayThemeMusic = this.GetValue<bool>("PlayThemeMusic", ConfigSectionType.Game);

            // Debug 
			this.AllStagesUnlocked = this.GetValue<bool>("AllStagesUnlocked", ConfigSectionType.Debug);
			this.AllowDebugWindow = this.GetValue<bool>("AllowDebugWindow", ConfigSectionType.Debug);
            this.ShowBoardFrames = this.GetValue<bool>("ShowBoardFrames", false, ConfigSectionType.Debug);
            this.ShowBoardCoordinates = this.GetValue<bool>("ShowBoardCoordinates", false, ConfigSectionType.Debug);
            this.ShowPaths = this.GetValue<bool>("ShowPaths", false, ConfigSectionType.Debug);
            this.ShowQuadtree = this.GetValue<bool>("ShowQuadtree", false, ConfigSectionType.Debug);
            this.ShowBoardGrid = this.GetValue<bool>("ShowBoardGrid", false, ConfigSectionType.Debug);
            this.ShowTiles = this.GetValue<bool>("ShowTiles", false, ConfigSectionType.Debug);
            this.ShowDebugInfo = this.GetValue<bool>("ShowDebugInfo", false, ConfigSectionType.Debug);
            this.ShowResourceManagerData = this.GetValue<bool>("ShowResourceManagerData", false, ConfigSectionType.Debug);
            this.ShowStageStats = this.GetValue<bool>("ShowStageStats", false, ConfigSectionType.Debug);
            this.StartupStageNr = this.GetValue<int>("StartupStageNr", 1, ConfigSectionType.Debug);
            this.StartupTheme = (ThemeType)Enum.Parse(typeof(ThemeType), this.GetValue<string>("StartupTheme", ThemeType.ThemeA.ToString(), ConfigSectionType.Debug), true);
            this.DeletePlayerProfile = this.GetValue<bool>("DeletePlayerProfile", false, ConfigSectionType.Debug);
            this.AllowCustomStages = this.GetValue<bool>("AllowCustomStages", false, ConfigSectionType.Debug);
            this.CustomStagesFolder = (this.AllowCustomStages? Path.Combine(BrainGame.GameUserFolderName, "CustomStages") : null);

            this.InitializeSupportedLanguages();
#if DEBUG
            this.ShowDebugInfo = true;
            this.PauseGameWhenFocusLost = this.GetValue<bool>("PauseGameWhenFocusLost", ConfigSectionType.Game);
#else
            this.ShowDebugInfo = false;
            this.PauseGameWhenFocusLost = true;
#endif

#if TRIAL
            this.GameplayMode = GameplayModeType.Demo;
#endif
			if (this.PresentationModeString == PresentationType.HD.ToString()) {
				PresentationMode = PresentationType.HD;
			}
			else if (this.PresentationModeString == PresentationType.LD.ToString()) {
				PresentationMode = PresentationType.LD;
			}
			else if (this.PresentationModeString == PresentationType.LDW.ToString()) {
				PresentationMode = PresentationType.LDW;
			}
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeSupportedLanguages()
        {
           BrainGame.SupportedLanguages.Clear();

           if ((this.Languages & LanguageType.English) == LanguageType.English)
           {
               BrainGame.SupportedLanguages.Add(BrainEngine.Localization.LanguageCode.en);
           }

           if ((this.Languages & LanguageType.French) == LanguageType.French)
           {
               BrainGame.SupportedLanguages.Add(BrainEngine.Localization.LanguageCode.fr);
           }

           if ((this.Languages & LanguageType.German) == LanguageType.German)
           {
               BrainGame.SupportedLanguages.Add(BrainEngine.Localization.LanguageCode.de);
           }

           if ((this.Languages & LanguageType.Italian) == LanguageType.Italian)
           {
               BrainGame.SupportedLanguages.Add(BrainEngine.Localization.LanguageCode.it);
           }

           if ((this.Languages & LanguageType.Portuguese) == LanguageType.Portuguese)
           {
               BrainGame.SupportedLanguages.Add(BrainEngine.Localization.LanguageCode.pt);
           }

           if ((this.Languages & LanguageType.Spanish) == LanguageType.Spanish)
           {
               BrainGame.SupportedLanguages.Add(BrainEngine.Localization.LanguageCode.es);
           }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveToFile()
        {
#if XMLDATAFILE_SUPPORT
            string filename = Path.Combine(BrainGame.GameUserFolderName, GameSettings.GAME_SETTINGS_FILE);
            DataFile file = new DataFile();
            file.RootRecord = this.ToDataFileRecord();
            XmlDataFileWriter writer = new XmlDataFileWriter();
            writer.Write(filename, file);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadFromFile()
        {
#if XMLDATAFILE_SUPPORT 
            string filename = Path.Combine(BrainGame.GameUserFolderName, GameSettings.GAME_SETTINGS_FILE);
            if (File.Exists(filename))
            {
                XmlDataFileReader reader = new XmlDataFileReader();
                DataFile file = reader.Read(filename);
                this.InitFromDataFileRecord(file.RootRecord);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this.IsFullScreen = record.GetFieldValue<bool>("IsFullscreen");
            this.ShowBoardFrames = record.GetFieldValue<bool>("ShowBoardFrames");
            this.ShowBoardCoordinates = record.GetFieldValue<bool>("ShowBoardCoordinates");
            this.ShowPaths = record.GetFieldValue<bool>("ShowPaths");
            this.ShowObjectIds = record.GetFieldValue<bool>("ShowObjectIds");
            this.FPSLocked = record.GetFieldValue<bool>("FPSLocked");
            this.ShowQuadtree = record.GetFieldValue<bool>("ShowQuadtree", false);
            this.ShowTiles = record.GetFieldValue<bool>("ShowTiles", true);
            this.ShowTheInstructionBar = record.GetFieldValue<bool>("ShowTheInstructionBar");
            this.ShowStageStats = record.GetFieldValue<bool>("ShowStageStats");
            this.ShowMapScrollIndicators = record.GetFieldValue<bool>("ShowMapScrollIndicators");
            this.PauseGameWhenFocusLost = record.GetFieldValue<bool>("PauseGameWhenFocusLost");
            this.StartupStageNr = record.GetFieldValue<int>("StartupStageNr", 1);
            this.StartupTheme = (ThemeType)Enum.Parse(typeof(ThemeType), record.GetFieldValue<string>("StartupTheme", ThemeType.ThemeA.ToString()), true);
            this.DeletePlayerProfile = record.GetFieldValue<bool>("DeletePlayerProfile", false);
            this.ShowSprites = record.GetFieldValue<bool>("ShowSprites", true);
            this.ShowDebugInfo = record.GetFieldValue<bool>("ShowDebugInfo", false);
            this.WindowAlwaysActive = record.GetFieldValue<bool>("WindowAlwaysActive", false);
            this.AllStagesUnlocked = record.GetFieldValue<bool>("AllStagesUnlocked", false);
            string ep = record.GetFieldValue<string>("EntryPoint", null);
            if (ep != null)
            {
                this.EntryPoint = (GameEntryPoint)Enum.Parse(typeof(GameEntryPoint), ep, true);
            }
            this.ShowBoundingBoxes = record.GetFieldValue<bool>("ShowBB", false);
            this.ShowSpriteFrame = record.GetFieldValue<bool>("ShowSpriteFrame", false);
            this.GameplayMode = (GameplayModeType)Enum.Parse(typeof(GameplayModeType), record.GetFieldValue<string>("GameplayMode"), true);
            this.GameplayRecordingPath = record.GetFieldValue<string>("GameplayRecordingPath", string.Empty);
            this.DefaultSoundVolume = record.GetFieldValue<float>("DefaultSoundVolume", 1.0f);

        }

        /// <summary>
        /// This could be on the BrainSettings base class, but I don't feel like doing that now
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord root = new DataFileRecord("GameSettings");

            root.AddField("IsFullscreen", this.IsFullScreen);
            root.AddField("ShowBoardFrames", this.ShowBoardFrames);
            root.AddField("ShowBoardCoordinates", this.ShowBoardCoordinates);
            root.AddField("ShowPaths", this.ShowPaths);
            root.AddField("ShowObjectIds", this.ShowObjectIds);
            root.AddField("FPSLocked", this.FPSLocked);
            root.AddField("ShowQuadtree", this.ShowQuadtree);
            root.AddField("ShowTiles", this.ShowTiles);
            root.AddField("ShowSprites", this.ShowSprites);
            root.AddField("ShowTheInstructionBar", this.ShowTheInstructionBar);
            root.AddField("ShowStageStats", this.ShowStageStats);
            root.AddField("ShowMapScrollIndicators", this.ShowMapScrollIndicators);
            root.AddField("ShowDebugInfo", this.ShowDebugInfo);
            root.AddField("WindowAlwaysActive", this.WindowAlwaysActive);
            root.AddField("PauseGameWhenFocusLost", this.PauseGameWhenFocusLost);
            root.AddField("StartupStageNr", this.StartupStageNr);
            root.AddField("StartupTheme", this.StartupTheme.ToString());
            root.AddField("EntryPoint", this.EntryPoint.ToString());
            root.AddField("ShowBB", this.ShowBoundingBoxes.ToString());
            root.AddField("ShowSpriteFrame", this.ShowSpriteFrame.ToString());
            root.AddField("DeletePlayerProfile", this.DeletePlayerProfile);
            root.AddField("GameplayMode", this.GameplayMode.ToString());
            root.AddField("GameplayRecordingPath", this.GameplayRecordingPath);
            root.AddField("AllStagesUnlocked", this.AllStagesUnlocked);
            root.AddField("Languages", this.Languages.ToString());
            root.AddField("DefaultSoundVolume", this.DefaultSoundVolume);

            return root;
        }
    }
}
