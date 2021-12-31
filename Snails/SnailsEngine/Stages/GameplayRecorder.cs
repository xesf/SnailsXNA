using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TwoBrainsGames.BrainEngine.Input;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Effects;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Input;

namespace TwoBrainsGames.Snails.Stages
{
    public class GameplayRecorder : Object2D
    {
        public const string DEFAULT_EXTENSION = "SGD"; // Snails gameplay data
        public const string FILE_DESCRIPTION = "Snails Gameplay"; 

        public enum RecorderState
        {
            Stopped,
            Recording,
            Playing,
            Paused
        }

        public class SnailsGameplayRecord
        {
            public string Version { get; set; }
            public string Filename { get; set; }
            public string Description { get; set; }
            public int BuildNr { get; set; }
            public string StageKey { get; set; }
            public TimeSpan TimeTaken { get; set; }
            public int TotalScore { get; set; }
            public Snails.MedalType MedalWon { get; set; }
            public DateTime FileDate { get; set; }
            public Vector2 StageStartupPosition { get; set; }

            public SnailsGameplayRecord()
            {
            }

            public override string ToString()
            {
                if (this.Description != null)
                {
                    return string.Format("{0} [Build:{1}]", this.Description, this.BuildNr);
                }
                return "NO DESCRIPTION";
            }
        }

        const string STAGE_GAMEPLAY_FILE_STAMP = "SNAILS_GAMEPLAY";
        const string STAGE_GAMEPLAY_FILE_STAMP2 = "SNAILS_GAMEPLV2"; // Created because I didn't had file version !! This will be used to determine if we have file version or not
        const string STAGE_GAMEPLAY_FILE_VERSION_1_1 = "V1.1"; // Added file version and stage buildNr
        const string STAGE_GAMEPLAY_FILE_VERSION_1_2 = "V1.2"; // Added Date/time of file, 
                                                               // Added stage startup position
                                                               // Added medal won
                                                               // Add total score

        private Stage _stage;
        private BlinkEffect _blink;
        private RecorderState StateBeforePause { get; set; }
        private InputRecorder Recorder { get; set; }
        private GameplayInput.GamePlayButtons _actionsToIgnore;
        private GameplayInput.GamePlayButtons _acceptedActionsWhilePlaying;

        public RecorderState State { get; private set; }
        public bool Enabled { get; set; }

        public bool IsRecording
        {
            get
            {
                return (this.State == RecorderState.Recording);
            }
        }


        public bool IsPlaying
        {
            get
            {
                return (this.State == RecorderState.Playing);
            }
        }


        public bool IsPaused
        {
            get
            {
                return (this.State == RecorderState.Paused);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GameplayRecorder(Stage stage)
        {
            this._stage = stage;
            this._stage.Input.OnAfterUpdate += new GameplayInput.InputEvent(Input_OnAfterUpdate);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this.SpriteAnimationActive = false;
            this._position = new Vector2(50f, 50f);
            this._blink = new BlinkEffect(500, 500);
            this._blink.UseRealTime = true;
            this.Recorder = new InputRecorder();
            this._actionsToIgnore = GameplayInput.GamePlayButtons.Pause;
            this._acceptedActionsWhilePlaying = GameplayInput.GamePlayButtons.Pause | GameplayInput.GamePlayButtons.TimeWarp;
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "GameplayRecorder");
        }


        /// <summary>
        /// 
        /// </summary>
        void Input_OnAfterUpdate(GameplayInput gameplayInput, BrainGameTime gameTime, out Vector2 newMotionPosition)
        {
            newMotionPosition = gameplayInput.MotionPosition;
            if (this.Enabled)
            {
                this.Recorder.Update(gameTime, (ulong)(gameplayInput.GameButtons & ~this._actionsToIgnore), gameplayInput.MotionPosition);
                if (this.State == RecorderState.Playing)
                {
                    if (this.Recorder.CurrentItem != null)
                    {
                        GameplayInput.GamePlayButtons acceptedActions = (gameplayInput.GameButtons & this._acceptedActionsWhilePlaying);
                        gameplayInput.GameButtons = (GameplayInput.GamePlayButtons)this.Recorder.CurrentItem._inputAction | acceptedActions;
                        newMotionPosition = this.Recorder.CurrentItem._position;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this.Enabled)
            {
                base.Update(gameTime);
                this._blink.Update(gameTime);
                this.CurrentFrame = (int)this.State;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Enabled && this._blink.Visible)
            {  
                base.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartRecording()
        {
            this.Recorder.StartRecording();
            this.State = RecorderState.Recording;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Play()
        {
            this.Recorder.PlayBack();
            this.State = RecorderState.Playing;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            this.Recorder.Stop();
            this.State = RecorderState.Stopped;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Pause()
        {
            this.StateBeforePause = this.State;
            this.State = RecorderState.Paused;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Resume()
        {
            this.State = this.StateBeforePause;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save(string filename, string description)
        {
#if !WIN8
            FileStream writeStream = new FileStream(filename, FileMode.Create);

            BinaryWriter writeBinary = new BinaryWriter(writeStream);
            try
            {
                // Header
                writeBinary.Write(STAGE_GAMEPLAY_FILE_STAMP2); // Stamp
                writeBinary.Write(STAGE_GAMEPLAY_FILE_VERSION_1_2); // File version
                writeBinary.Write(this._stage.LevelStage.StageKey); // Stage key
                writeBinary.Write(this._stage.BuildNr); // Stage build nr
                writeBinary.Write(description); // A description
                GameplayRecorder.Write(writeBinary, DateTime.Now); // Current date/time
                GameplayRecorder.Write(writeBinary, this._stage.StartupCameraPOI); // Stage startup position

                GameplayRecorder.Write(writeBinary, this._stage.Stats.TimeTaken); // Time taken
                writeBinary.Write((int)this._stage.Stats.MedalWon); // Medal
                writeBinary.Write(this._stage.Stats.TotalScore); // Score


                // Controller input stream
                writeBinary.Write(this.Recorder.InputStream.ItemsCount);
                for (int i = 0; i < this.Recorder.InputStream.ItemsCount; i++)
                {
                    writeBinary.Write(this.Recorder.InputStream[i]._inputAction);
                    writeBinary.Write(this.Recorder.InputStream[i]._milliseconds);
                    writeBinary.Write((double)this.Recorder.InputStream[i]._position.X);
                    writeBinary.Write((double)this.Recorder.InputStream[i]._position.Y);
                }
            }
            finally
            {
                if (writeBinary != null)
                {
                    writeBinary.Close();
                }
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load(string filename)
        {
#if !WIN8
            FileStream readStream = new FileStream(filename, FileMode.Open);

            BinaryReader binaryReader = new BinaryReader(readStream);
            try
            {
                SnailsGameplayRecord record = GameplayRecorder.ReadHeaderFromStream(binaryReader);
                if (record.StageKey != this._stage.LevelStage.StageKey)
                {
                    throw new SnailsException("Gameplay storage file is not from the current stage.");
                }


                int itemCount = binaryReader.ReadInt32();

                for (int i = 0; i < itemCount; i++)
                {

                    ulong action = binaryReader.ReadUInt64();
                    double milliseconds = binaryReader.ReadDouble();
                    float x = (float)binaryReader.ReadDouble();
                    float y = (float)binaryReader.ReadDouble();
                    Vector2 pos = new Vector2(x, y);
                    this.Recorder.InputStream.Append(milliseconds, action, pos);
                }

                if (record.StageStartupPosition != Vector2.Zero)
                {
                    this._stage.Camera.MoveTo(record.StageStartupPosition);
                }
            }
            finally
            {
                if (readStream != null)
                {
                    readStream.Close();
                }
                if (binaryReader != null)
                {
                    binaryReader.Close();
                }
            }
#endif
        }
        
        /// <summary>
        /// 
        /// </summary>
        private static SnailsGameplayRecord ReadHeaderFromStream_V1_1(BinaryReader binaryReader)
        {
            SnailsGameplayRecord record = new SnailsGameplayRecord();
            record.Version = STAGE_GAMEPLAY_FILE_VERSION_1_1;
            record.StageKey = binaryReader.ReadString();
            record.BuildNr = binaryReader.ReadInt32();
            record.Description = binaryReader.ReadString();
            record.TimeTaken = GameplayRecorder.ReadTimeSpan(binaryReader);
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        private static SnailsGameplayRecord ReadHeaderFromStream_V1_2(BinaryReader binaryReader)
        {
            SnailsGameplayRecord record = new SnailsGameplayRecord();
            record.Version = STAGE_GAMEPLAY_FILE_VERSION_1_2;
            record.StageKey = binaryReader.ReadString();
            record.BuildNr = binaryReader.ReadInt32();
            record.Description = binaryReader.ReadString();
            record.FileDate = GameplayRecorder.ReadDateTime(binaryReader);
            record.StageStartupPosition = GameplayRecorder.ReadVector2(binaryReader);
            record.TimeTaken = GameplayRecorder.ReadTimeSpan(binaryReader);
            record.MedalWon = (Snails.MedalType)binaryReader.ReadInt32();
            record.TotalScore = binaryReader.ReadInt32();

            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        private static SnailsGameplayRecord ReadHeaderFromStream(BinaryReader binaryReader)
        {
            SnailsGameplayRecord record = new SnailsGameplayRecord();
            // Header
            string fileType = binaryReader.ReadString();
            if (fileType != STAGE_GAMEPLAY_FILE_STAMP &&
                fileType != STAGE_GAMEPLAY_FILE_STAMP2)
            {

                throw new SnailsException("Gameplay storage file is invalid.");
            }
            // New file type with version support
            if (fileType == STAGE_GAMEPLAY_FILE_STAMP2)
            {
                string version = binaryReader.ReadString();
                if (version == STAGE_GAMEPLAY_FILE_VERSION_1_1)
                {
                    return GameplayRecorder.ReadHeaderFromStream_V1_1(binaryReader);
                }
                if (version == STAGE_GAMEPLAY_FILE_VERSION_1_2)
                {
                    return GameplayRecorder.ReadHeaderFromStream_V1_2(binaryReader);
                }

            }

            // Old format...
            record.StageKey = binaryReader.ReadString();
            record.Description = binaryReader.ReadString(); 

            // Time taken
            binaryReader.ReadInt32();
            binaryReader.ReadInt32();
            binaryReader.ReadInt32();
            binaryReader.ReadInt32();

            return record;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private static SnailsGameplayRecord ReadHeader(string filename)
        {
#if !WIN8
           FileStream readStream = new FileStream(filename, FileMode.Open);

            BinaryReader binaryReader = new BinaryReader(readStream);
            try
            {
                SnailsGameplayRecord record = GameplayRecorder.ReadHeaderFromStream(binaryReader);
                record.Filename = filename;
                return record;
            }
            finally
            {
                if (readStream != null)
                {
                    readStream.Close();
                }
                if (binaryReader != null)
                {
                    binaryReader.Close();
                }
            }
#else
            return null;
#endif
        }

        /// <summary>
        /// Enumerates all gameplay data files with the specified stage key
        /// </summary>
        public static List<SnailsGameplayRecord> EnumerateFiles(string path, string stageKey)
        {
            List<SnailsGameplayRecord> files = new List<SnailsGameplayRecord>();
#if !WIN8
            if (Directory.Exists(path))
            {
                string[] allFiles = Directory.GetFiles(path, "*." + GameplayRecorder.DEFAULT_EXTENSION);
                foreach (string fileName in allFiles)
                {
                    SnailsGameplayRecord record = GameplayRecorder.ReadHeader(fileName);
                    if (record.StageKey == stageKey)
                    {
                        files.Add(record);
                    }
                }
            }
#endif
            return files;
        }

        #region Read/Write methods
        // We could just create a new class, derive from BinaryWriter and implement this methods...
        /// <summary>
        /// 
        /// </summary>
        private static void Write(BinaryWriter writeBinary, DateTime dateTime)
        {
            writeBinary.Write(dateTime.Year);
            writeBinary.Write(dateTime.Month);
            writeBinary.Write(dateTime.Day);
            writeBinary.Write(dateTime.Hour);
            writeBinary.Write(dateTime.Minute);
            writeBinary.Write(dateTime.Second);
            writeBinary.Write(dateTime.Millisecond);
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Write(BinaryWriter writeBinary, TimeSpan time)
        {
            writeBinary.Write(time.Hours);
            writeBinary.Write(time.Minutes);
            writeBinary.Write(time.Seconds);
            writeBinary.Write(time.Milliseconds);
        }


        /// <summary>
        /// 
        /// </summary>
        private static void Write(BinaryWriter writeBinary, Vector2 pos)
        {
            writeBinary.Write((double)pos.X);
            writeBinary.Write((double)pos.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        private static TimeSpan ReadTimeSpan(BinaryReader reader)
        {
            int hour = reader.ReadInt32();
            int minute = reader.ReadInt32();
            int second = reader.ReadInt32();
            int millisecond = reader.ReadInt32();
            return new TimeSpan(0, hour, minute, second, millisecond);
        }


        /// <summary>
        /// 
        /// </summary>
        private static DateTime ReadDateTime(BinaryReader reader)
        {
            int year = reader.ReadInt32();
            int month = reader.ReadInt32();
            int day = reader.ReadInt32();
            int hour = reader.ReadInt32();
            int minute = reader.ReadInt32();
            int second = reader.ReadInt32();
            int millisecond = reader.ReadInt32();
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// 
        /// </summary>
        private static Vector2 ReadVector2(BinaryReader reader)
        {
            float x = (float)reader.ReadDouble();
            float y = (float)reader.ReadDouble();
           
            return new Vector2(x, y);
        }

        #endregion
    }
}
