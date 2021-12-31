using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.GamerServices;

namespace TwoBrainsGames.Snails.Player
{
    class PlayersProfileManagerXBOX : PlayersProfileManager
    {
        private string STORAGE_NAME = "Snails HD Players Profile";

        // storage related objects
        private StorageDevice _storage;
        private IAsyncResult _result;
        private bool _gameSaveRequested = false;
        private bool _beginSaveRequested = false;
        private bool _beginLoadRequested = false;
        private string _gamerTag;

        public override int LoadProfile()
        {
            _seletedProfileName = _gamerTag;

            if (ExistsProfile(_seletedProfileName))
            {
                SelectProfile(_seletedProfileName);

                // Set Viewport of the loaded profile
                if (_currentProfile.OverscanSet)
                {
                    BrainGame.SetViewport(_currentProfile.Viewport);
                }

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

        public override void CreateProfile(string name)
        {
            name = _gamerTag;
            if (string.IsNullOrEmpty(_gamerTag))
            {
                name = DEFAULT_PLAYER_NAME;
                SignedInGamer gamer = Gamer.SignedInGamers[PlayerIndex.One];
                if (gamer != null)
                {
                    _gamerTag = gamer.Gamertag;
                    name = _gamerTag;
                }
            }

            if (_currentProfile == null)
            {
                NewProfile(name);
            }

            if (_currentProfile != null)
            {
                _currentProfile.Viewport = SnailsGame.Viewport;
                Save();
            }
            else 
            {
                // TODO: this should show a popup saying the Gamertag is invalid
                //Guide.BeginShowMessageBox
            }
        }

        public override void Save()
        {
            // only save if isn't an anonymous profile
            if (_currentProfile != null && !_currentProfile.IsAnonymous)
            {
                this.IsCompleted = false;
                _beginSaveRequested = true;
            }
        }

        public override void Load()
        {
            _storage = null;
            this.IsCompleted = false;
            _beginLoadRequested = true;
        }

        /// <summary>
        /// Save method only for Xbox
        /// </summary>
        /// <param name="device"></param>
        private void SaveXbox(StorageDevice device)
        {
            StorageContainer container = null;
            IAsyncResult result = null;
            DataFile dataFile = GetDataFileToSave();

            // Open a storage container.
            if (device.IsConnected)
            {
                result = device.BeginOpenContainer(STORAGE_NAME, null, null);
            }
            // Wait for the WaitHandle to become signaled.
            if (result != null)
            {
                result.AsyncWaitHandle.WaitOne();
            }
            if (device.IsConnected)
            {
                container = device.EndOpenContainer(result);
            }
            // Close the wait handle.
            if (result != null)
            {
                result.AsyncWaitHandle.Close();
            }

            // if an exception happens with the container device, than
            if (container == null)
            {
                return;
            }

            // Create the file.
            Stream stream = container.CreateFile(SAVE_FILENAME);

            BinaryDataFileWriter writer = new BinaryDataFileWriter();
            BinaryWriter wr = new BinaryWriter(stream);
            writer.Write(wr, dataFile);

            // Close the file.
            stream.Close();
            // Dispose the container, to commit changes.
            container.Dispose();
        }

        /// <summary>
        /// Load method only for Xbox
        /// </summary>
        private void LoadXbox(StorageDevice device)
        {
            DataFile dataFile = null;
            StorageContainer container = null;
            IAsyncResult result = null;

            // Open a storage container.
            if (device.IsConnected)
            {
                result = device.BeginOpenContainer(STORAGE_NAME, null, null);
            }
            // Wait for the WaitHandle to become signaled.
            if (result != null)
            {
                result.AsyncWaitHandle.WaitOne();
            }
            if (device.IsConnected)
            {
                container = device.EndOpenContainer(result);
            }
            // Close the wait handle.
            if (result != null)
            {
                result.AsyncWaitHandle.Close();
            }

            // if an exception happens with the container device, than
            if (container == null)
            {
                return;
            }

            // Check to see whether the save exists.
            if (!container.FileExists(SAVE_FILENAME))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                LoadProfile(); // LoadProfile(true); // if it doesnt exist, it will create a default profile
                return;
            }

            // Open the file.
            Stream stream = container.OpenFile(SAVE_FILENAME, FileMode.Open);
            BinaryDataFileReader reader = new BinaryDataFileReader();
            dataFile = reader.Read(stream);

            LoadDataFile(dataFile);

            // Close the file.
            stream.Close();
            // Dispose the container, to commit changes.
            container.Dispose();

            LoadProfile(); // LoadProfile(false);
        }

        /// <summary>
        /// This is only used for Xbox version after the player controller is SET
        /// </summary>
        public override void SignedInPlayer()
        {
            SignedInGamer gamer = Gamer.SignedInGamers[BrainGame.CurrentControllerIndex];

            if (gamer != null)
            {
                _gamerTag = gamer.Gamertag;
            }
            else
            {
                Guide.ShowSignIn(1, false);
            }
        }

        /// <summary>
        /// Only used for XBOX to deal with Storage requests
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update()
        {
            if ((_beginSaveRequested || _beginLoadRequested) && !_gameSaveRequested) // should only run once per save/load
            {
                // Set the request flag
                if ((!Guide.IsVisible) && (_gameSaveRequested == false))
                {
                    _gameSaveRequested = true;
                    
                    // only start selecting storage if we don't have already one
                    if (_storage == null)
                    {
                        _result = StorageDevice.BeginShowSelector(BrainGame.CurrentControllerIndex, null, null);
                    }
                }
            }

            // If a save/load is pending, save/load as soon as the storage device is chosen
            if ((_gameSaveRequested || _beginLoadRequested) && (_result != null && _result.IsCompleted))
            {
                // only start selecting storage if we don't have already one
                if (_storage == null)
                {
                    _storage = StorageDevice.EndShowSelector(_result);
                }

                if (_storage != null && _storage.IsConnected)
                {
                    if (_beginSaveRequested)
                    {
                        _beginSaveRequested = false;
                        SaveXbox(_storage);
                    }
                    else if (_beginLoadRequested)
                    {
                        _beginLoadRequested = false;
                        LoadXbox(_storage);
                    }
                }
                else
                {
                    // if we have a storage and lost its connection, than try to select a new one, otherwise, we will use a anonymous profile
                    /*if (_storage != null && !_storage.IsConnected)
                    {
                        // don't do anything, just wait till we get a valid storage
                        // it will keep using the same profile.
                    }
                    else*/ if (_storage == null)
                    {
                        // create an anonymous profile if Storage selection was cancelled
                        CreateAnonymousProfile();
                    }
                }
                // Reset the request flag
                _gameSaveRequested = false;
                this.IsCompleted = true;
            }
        }
    }
}
