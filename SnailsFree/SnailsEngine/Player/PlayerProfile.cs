using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
#if XMLDATAFILE_SUPPORT
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
#endif
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Player
{
    public class PlayerProfile : IDataFileSerializable
    {
        #region Constants
        const int DEFAULT_MUSIC_VOLUME = 80;
        const int DEFAULT_SFX_VOLUME = 100;
        #endregion 

        #region Members
        protected string _name;
        protected bool _overscanSet;
        protected bool _viewportSet;
        protected Viewport _viewport;
        protected bool _useGamePadVibration;
        protected bool _showTutorial;
        protected PlayerStats _playerStats;
        protected int _soundVolume; // Percent
        protected int _musicVolume; // Percent
        protected List<int> _tutorialTopicsRead;
        protected int _lastPlayedStageNr;
        protected ThemeType _lastPlayedTheme;
        protected bool _isAnonymous;
        protected List<int> _achievementsEarned;
        #endregion

        #region Properties
        public string Name 
        { 
            get { return _name; } 
            set { _name = value; } 
        }

        /// <summary>
        /// For now its only used for Xbox to know if the profile storage was cancelled by the user. 
        /// This will prevent the game to save any game data during this playtime.
        /// </summary>
        public bool IsAnonymous
        {
            get { return _isAnonymous; }
            set { _isAnonymous = value; }
        }

        public bool OverscanSet
        {
            get { return _overscanSet; }
            set { _overscanSet = value; }
        }

        public bool ViewportSet
        {
            get { return _viewportSet; }
            set { _viewportSet = value; }
        }

        public Viewport Viewport
        {
            get { return _viewport; }
            set 
            {
                _viewportSet = true;
                _viewport = value;
            }
        }

        public bool UseGamePadVibration
        {
            get { return _useGamePadVibration; }
            set { _useGamePadVibration = value; }
        }

        public PlayerStats PlayerStats
        {
            get { return _playerStats; }
            set { _playerStats = value; }
        }

        public int SoundVolume 
        {
            get
            {
                return this._soundVolume;
            }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new BrainEngine.BrainException("Invalid value. Sound volume must be a number between 0 and 100");
                }
                this._soundVolume = value;
            }
        } 

        public int MusicVolume
        {
            get
            {
                return this._musicVolume;
            }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new BrainEngine.BrainException("Invalid value. Music volume must be a number between 0 and 100");
                }
                this._musicVolume = value;
            }
        }

        public bool AnyStagedPlayed
        {
            get { return (this.PlayerStats.TotalPlayingTime.TotalMilliseconds != 0); }
        }

        public int LastPlayedStageNr
        {
            get { return _lastPlayedStageNr; }
            set { _lastPlayedStageNr = value; }
        }

        public ThemeType LastPlayedTheme
        {
            get { return _lastPlayedTheme; }
            set { _lastPlayedTheme = value; }
        }

        public bool HasStartedNewGame
        {
            get { return this.AnyStagedPlayed; }
        }

        public bool ScreenModeSelected { get; set; }
        public bool Fullscreen { get; set; }
        public string EULAAcceptedInVersion { get; set; } // Version where the eula was accepted
                                                          // Eula will be displayed if this changes

        public List<int> AchievementsEarned { get { return this._achievementsEarned; } }

        public int StagesFailedCount { get; set; }
        public int LastHintMessageSeen { get; set; }
        public bool GameWasRated { get; set; }
        #endregion

        public PlayerProfile(string name) 
        {
            this._name = name;
            this._overscanSet = false;
            this._viewportSet = false;
            this._isAnonymous = false;
            this._playerStats = new PlayerStats();
            this._useGamePadVibration = true;
            this._showTutorial = true;
            this._soundVolume = (int)(SnailsGame.GameSettings.DefaultSoundVolume * 100f);
            this._musicVolume = (int)(SnailsGame.GameSettings.DefaultMusicVolume * 100f);
            this._tutorialTopicsRead = new List<int>();
            this._lastPlayedStageNr = 1;
            this._lastPlayedTheme = ThemeType.ThemeA;
            this._playerStats.UnlockFirstStage(ThemeType.ThemeA);
            this._achievementsEarned = new List<int>();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAchievementEarned(int type)
        {
            foreach (int i in this._achievementsEarned)
            {
                if (i == type)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void MarkAchievementEarned(int type)
        {
            // We don't want to add the same topic twice
            foreach (int i in this._achievementsEarned)
            {
                if (i == type)
                    return;
            }

            this._achievementsEarned.Add(type);
        }

        /// <summary>
        /// 
        /// </summary>
        public void MarkAchievementNotEarned(int type)
        {
            // We don't want to add the same topic twice
            foreach (int i in this._achievementsEarned)
            {
                if (i == type)
                {
                    this._achievementsEarned.Remove(type);
                    return;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTutorialTopicRead(int id)
        {
            foreach (int i in this._tutorialTopicsRead)
            {
                if (i == id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void MarkTutorialTopicAsRead(int id)
        {
            // We don't want to add the same topic twice
            foreach (int i in this._tutorialTopicsRead)
            {
                if (i == id)
                    return;
            }

            this._tutorialTopicsRead.Add(id);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearAchievements()
        {
            this.AchievementsEarned.Clear();
        }


#if XMLDATAFILE_SUPPORT
        /// <summary>
        /// 
        /// </summary>
        public void SaveToFile(string filename)
        {
          DataFile dataFile = new DataFile();
          dataFile.RootRecord = this.ToDataFileRecord();
          XmlDataFileWriter writer = new XmlDataFileWriter();
          writer.Write(filename, dataFile);
        }
#endif
        #region IDataFileSerializable Members

        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._name = record.GetFieldValue<string>("name");

            // player settings node
            DataFileRecord recordSettings = record.SelectRecord("Settings");
            SnailsGame.GameSettings.CustomStagesFolder = recordSettings.GetFieldValue<string>("customStagesFolder", SnailsGame.GameSettings.CustomStagesFolder);
            
            this._overscanSet = recordSettings.GetFieldValue<bool>("overscanSet", this._overscanSet);
            this._viewportSet = recordSettings.GetFieldValue<bool>("viewportSet", this._viewportSet);
            if (this._viewportSet)
            {
                this._viewport.X = recordSettings.GetFieldValue<int>("viewportX", this._viewport.X);
                this._viewport.Y = recordSettings.GetFieldValue<int>("viewportY", this._viewport.Y);
                this._viewport.Width = recordSettings.GetFieldValue<int>("viewportWidth", this._viewport.Width);
                this._viewport.Height = recordSettings.GetFieldValue<int>("viewportHeight", this._viewport.Height);
            }
            this._useGamePadVibration = recordSettings.GetFieldValue<bool>("useGamePadVibration", this._useGamePadVibration);
            this._showTutorial = recordSettings.GetFieldValue<bool>("showTutorial", this._showTutorial);
            this.LastHintMessageSeen = recordSettings.GetFieldValue<int>("lastHintMessageSeen", this.LastHintMessageSeen);
            this.GameWasRated = recordSettings.GetFieldValue<bool>("gameWasRated", this.GameWasRated);

            this._soundVolume = recordSettings.GetFieldValue<int>("soundVolume", this._soundVolume);
            this._musicVolume = recordSettings.GetFieldValue<int>("musicVolume", this._musicVolume);

            this._lastPlayedStageNr = recordSettings.GetFieldValue<int>("lastPlayedStageNr", this._lastPlayedStageNr);
            this.ScreenModeSelected = recordSettings.GetFieldValue<bool>("screenModeSelected", this.ScreenModeSelected);
            this.Fullscreen = recordSettings.GetFieldValue<bool>("fullscreen", this.Fullscreen);
            this.EULAAcceptedInVersion = recordSettings.GetFieldValue<string>("eulaAcceptedInVersion", this.EULAAcceptedInVersion);
            this._lastPlayedTheme = (ThemeType)Enum.Parse(typeof(ThemeType), recordSettings.GetFieldValue<string>("lastPlayedTheme", this._lastPlayedTheme.ToString()), true);
            BrainGame.CurrentLanguage = (LanguageCode)Enum.Parse(typeof(LanguageCode), recordSettings.GetFieldValue<string>("language", BrainGame.CurrentLanguage.ToString()), true);
            // Tutorials read
            string tuts = recordSettings.GetFieldValue<string>("tutorialTopicsRead", "");
            this._tutorialTopicsRead = new List<int>();
            string [] tutIds = tuts.Split(';');
            foreach(string tutId in tutIds)
            {
                if (string.IsNullOrEmpty(tutId) == false)
                {
                    this._tutorialTopicsRead.Add(Convert.ToInt32(tutId));
                }
            }
            // Achievements earned
            string achievs = recordSettings.GetFieldValue<string>("achievementsEarned", "");
            this._achievementsEarned = new List<int>();
            string[] achievsIds = achievs.Split(';');
            foreach (string achievId in achievsIds)
            {
                if (string.IsNullOrEmpty(achievId) == false)
                {
                    this._achievementsEarned.Add(Convert.ToInt32(achievId));
                }
            }

            // player stats node
            this._playerStats.InitFromDataFileRecord(record.SelectRecord("Statistics"));
        }

        public virtual DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public virtual DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("Profile");
            record.AddField("name", this._name);

            // player settings node
            DataFileRecord recordSettings = new DataFileRecord("Settings");
            recordSettings.AddField("customStagesFolder", SnailsGame.GameSettings.CustomStagesFolder);

            recordSettings.AddField("overscanSet", this._overscanSet);
            recordSettings.AddField("viewportSet", this._viewportSet);
            if (this._viewportSet)
            {
                recordSettings.AddField("viewportX", this._viewport.X);
                recordSettings.AddField("viewportY", this._viewport.Y);
                recordSettings.AddField("viewportWidth", this._viewport.Width);
                recordSettings.AddField("viewportHeight", this._viewport.Height);
            }
            recordSettings.AddField("useGamePadVibration", this._useGamePadVibration);
            recordSettings.AddField("showTutorial", this._showTutorial);
            recordSettings.AddField("lastHintMessageSeen", this.LastHintMessageSeen);
            recordSettings.AddField("gameWasRated", this.GameWasRated);

            recordSettings.AddField("soundVolume", this._soundVolume);
            recordSettings.AddField("musicVolume", this._musicVolume);

            recordSettings.AddField("lastPlayedStageNr", this._lastPlayedStageNr);
            recordSettings.AddField("lastPlayedTheme", this._lastPlayedTheme.ToString());
            recordSettings.AddField("language", BrainGame.CurrentLanguage.ToString());
            recordSettings.AddField("screenModeSelected", this.ScreenModeSelected);
            recordSettings.AddField("fullscreen", this.Fullscreen);
            recordSettings.AddField("eulaAcceptedInVersion", this.EULAAcceptedInVersion);

            string tuts = "";
            foreach (int topicId in this._tutorialTopicsRead)
            {
                tuts += topicId.ToString() + ";";
            }
            recordSettings.AddField("tutorialTopicsRead", tuts);

            string achievs = "";
            foreach (int achievId in this._achievementsEarned)
            {
                achievs += achievId.ToString() + ";";
            }
            recordSettings.AddField("achievementsEarned", achievs);

            record.AddRecord(recordSettings);

            // player stats node
            record.AddRecord(_playerStats.ToDataFileRecord());

            return record;
        }

        #endregion
    }
}
