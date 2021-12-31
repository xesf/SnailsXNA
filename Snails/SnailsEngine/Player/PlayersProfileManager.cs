using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
#if SAVE_XML
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
#else
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
#endif

namespace TwoBrainsGames.Snails.Player
{
    class PlayersProfileManagerFactory
    {
        public static PlayersProfileManager Create()
        {
#if XBOX
            return new PlayersProfileManagerXBOX();
#elif WP7
            return new PlayersProfileManagerWP7();
#elif WIN8
            return new PlayersProfileManagerWIN8();
#else
            return new PlayersProfileManagerCROSS();
#endif
        }
    }

    class PlayersProfileManager
    {
        #region Constants
#if SAVE_XML
        protected string SAVE_FILENAME = "PlayerProfile.xdf";
#else
        protected string SAVE_FILENAME = "PlayerProfile.bdf";
#endif
        protected string DEFAULT_PLAYER_NAME = "Player 1";
        #endregion

        protected string _seletedProfileName;
        protected List<PlayerProfile> _profiles;
        protected PlayerProfile _currentProfile;
        public bool IsCompleted { get; protected set; }
     
        public PlayerProfile CurrentProfile 
        {
            get 
            {
                return _currentProfile;
            }
            set { _currentProfile = value; } 
        }

        public PlayersProfileManager()
        {
            _seletedProfileName = string.Empty;
            _profiles = new List<PlayerProfile>(1); // 1 profiles as default size
            _currentProfile = null;
            this.IsCompleted = true;
        }

        protected virtual string GetUserFilename()
        {
            return SAVE_FILENAME;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void AddProfile(string name)
        {
            PlayerProfile profile = new PlayerProfile(name);
            _profiles.Add(profile);

            // select the default profile if only one profile exists at this time
            if (_profiles.Count == 1)
            {
                _currentProfile = profile;
                _seletedProfileName = name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveProfile(string name)
        {
            _profiles.RemoveAll(delegate(PlayerProfile match) { return match.Name == name; });
            if (name == _seletedProfileName)
            {
                _seletedProfileName = string.Empty;

                if (_profiles != null && _profiles.Count == 1)
                {
                    SelectProfile("");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistsProfile(string name)
        {
            return _profiles.Exists(delegate(PlayerProfile match) { return match.Name == name; });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>false - if already exists; true - if created;</returns>
        public bool NewProfile(string name)
        {
            if (!ExistsProfile(name))
            {
                AddProfile(name);
                SelectProfile(name);
                return true;
            }

            SelectProfile(name);
            
            return false; // already exists
        }

        public void SelectProfile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (_profiles != null && _profiles.Count == 1)
                {
                    _currentProfile = _profiles[0];
                    _seletedProfileName = _profiles[0].Name;
                }
            }

            _currentProfile = GetProfile(name);
            if (_currentProfile != null)
            {
                _seletedProfileName = name;

                // Por causa de um bug, é possível haver profiles que tenham como último nível jogado, níveis bloqueados no trial
                if (BrainGame.IsTrial)
                {
                    LevelStage lastPlayedStageInfo = Levels.CurrentLevel.FindStageInfo(_currentProfile.LastPlayedTheme, _currentProfile.LastPlayedStageNr);
                    if (lastPlayedStageInfo != null && !lastPlayedStageInfo.AvailableInDemo)
                    {
                        // Se estiver bloqueado, forçamos o 1º nível
                        _currentProfile.LastPlayedTheme = ThemeType.ThemeA;
                        _currentProfile.LastPlayedStageNr = 1;
                    }
                }

                Levels.CurrentTheme = _currentProfile.LastPlayedTheme;
                Levels.CurrentStageNr = _currentProfile.LastPlayedStageNr;
                BrainGame.SampleManager.MasterVolume = (SnailsGame.ProfilesManager.CurrentProfile.SoundVolume / 100f);
                BrainGame.MusicManager.MasterVolume = (SnailsGame.ProfilesManager.CurrentProfile.MusicVolume / 100f);
            }

            Achievements.Register();
        }

        /// <summary>
        /// Get default profile
        /// </summary>
        /// <returns></returns>
        public PlayerProfile GetSelectedProfile()
        {
            return GetProfile(_seletedProfileName);
        }

        /// <summary>
        /// Get profile
        /// </summary>
        /// <returns></returns>
        public PlayerProfile GetProfile(string name)
        {
            return _profiles.Find(delegate(PlayerProfile match) { return match.Name == name; });
        }

        /// <summary>
        /// Load profile file. Note, in Xbox this save is asychronous
        /// </summary>
        public void DeleteProfile()
        {
#if !WIN8
            string filename = GetUserFilename();
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
#endif
        }

        public virtual int LoadProfile()
        {
            if (ExistsProfile(_seletedProfileName))
            {
                SelectProfile(_seletedProfileName);
                return 1;
            }
            else
            {
                if (_currentProfile == null)
                {
                    CreateProfile(null);
                }
            }
            return 0;
        }

        public void CreateAnonymousProfile()
        {
            if (_currentProfile == null)
            {
                NewProfile(DEFAULT_PLAYER_NAME);
                this._currentProfile.IsAnonymous = true;
            }
        }

        public virtual void CreateProfile(string name)
        {
            if (_currentProfile == null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = DEFAULT_PLAYER_NAME;
                }
                NewProfile(name);
            }

            _currentProfile.Viewport = SnailsGame.Viewport; // in windows will be used the default viewport

            Achievements.Register();

            Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected DataFile GetDataFileToSave()
        {
            DataFile dataFile = new DataFile();

            DataFileRecord record = new DataFileRecord("Players");
            record.AddField("defaultProfile", _seletedProfileName);
            foreach (PlayerProfile profile in _profiles)
            {
                record.AddRecord(profile.ToDataFileRecord());
            }
            dataFile.RootRecord = record;
            
            return dataFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFile"></param>
        protected void LoadDataFile(DataFile dataFile)
        {
            if (dataFile != null)
            {
                _seletedProfileName = dataFile.RootRecord.GetFieldValue<string>("defaultProfile");

                DataFileRecordList records = dataFile.RootRecord.SelectRecords("Profile");
                if (records != null && records.Count > 0)
                {
                    this._profiles = new List<PlayerProfile>(records.Count); // create instance with the right size
                    foreach (DataFileRecord recordStage in records)
                    {
                        PlayerProfile profile = new PlayerProfile(string.Empty);
                        profile.InitFromDataFileRecord(recordStage);
                        this._profiles.Add(profile);
                    }
                }
            }
        }

        /// <summary>
        /// Save profile file. Note, in Xbox this save is asychronous
        /// </summary>
        public virtual void Save()
        { }

        public virtual void BeginLoad()
        { }

        /// <summary>
        /// Load profile file. Note, in Xbox this save is asychronous
        /// </summary>
        public virtual void Load()
        { }

        /// <summary>
        /// Only used for XBOX to deal with Storage requests
        /// </summary>
        public virtual void Update()
        { }

        /// <summary>
        /// Only used for XBOX
        /// </summary>
        public virtual void SignedInPlayer()
        { }

#if DEBUG
        public void LoadPlayerProfileForDEBUG()
        {
            if (SnailsGame.ProfilesManager.CurrentProfile == null)
            {
                int errorCode = SnailsGame.ProfilesManager.LoadProfile();
                if (errorCode == 0) // if not exist a profile, than create a new one
                {
                    if (SnailsGame.ProfilesManager.CurrentProfile == null)
                    {
                        SnailsGame.ProfilesManager.CreateProfile(null);
                    }
                }
            }
        }
#endif
    }
}
