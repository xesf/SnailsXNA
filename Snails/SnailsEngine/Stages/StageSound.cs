using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Stages
{
    public class StageSound : IBrainComponent, ISnailsDataFileSerializable
    {
        #region Members
        private const int DEFAULT_SURROUND_TIME = 3000; // 3s
        private const int DEFAULT_SURROUND_TIME_RND = 15000; // 15s

        private int _maxRandomDelay;
        private int _minDelay;
        private double _elapsedTime = 0;
        private double _nextSurroundTime = 0;
        private int _surroundPlayed = 0;
        private int _surroundPlayedMaxBit = 0;
        private bool _musicIsPlaying = false;

        private Music _music;
        private Sample _ambienceSample;
        private List<Sample> _surroundingsSamples = new List<Sample>();

        public StageSoundSource Music = new StageSoundSource();
        public StageSoundSource AmbienceSetting = new StageSoundSource();
        public List<StageSoundSource> AmbienceSurroundings = new List<StageSoundSource>();
        #endregion

        #region Properties
      

        public SpriteBatch SpriteBatch
        {
            get { return Levels.CurrentLevel.SpriteBatch; }
        }
        #endregion

        public StageSound()
        { }

        public void Initialize()
        {
            _musicIsPlaying = false;
            _surroundPlayed = 0;
            _elapsedTime = 0;
            _nextSurroundTime = _minDelay + SnailsGame.Rand.Next(_maxRandomDelay);
        }

        public void LoadContent()
        {
            // load music resource
            if (!string.IsNullOrEmpty(Music.Res))
                _music = BrainGame.ResourceManager.GetMusic(Music.Res, ResourceManagerIds.STAGE_THEME_RESOURCES);

            // load ambience setting resource
            if (!string.IsNullOrEmpty(AmbienceSetting.Res))
                _ambienceSample = BrainGame.ResourceManager.GetSample(AmbienceSetting.Res, ResourceManagerIds.STAGE_THEME_RESOURCES);

            // load surroundings resources
            _surroundingsSamples.Clear();
            for (int i = 0; i < AmbienceSurroundings.Count; i++)
            {
                if (!string.IsNullOrEmpty(AmbienceSurroundings[i].Res))
                    _surroundingsSamples.Add(BrainGame.ResourceManager.GetSample(AmbienceSurroundings[i].Res, ResourceManagerIds.STAGE_THEME_RESOURCES));
            }
        }

        public void Update(BrainGameTime gameTime)
        {
            if (AmbienceSurroundings.Count == 0)
                return;

            _elapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds; // use real time here or else it becomes anoying
            if (_elapsedTime >= _nextSurroundTime) // todo add random time here
            {
                int idx = SnailsGame.Rand.Next(AmbienceSurroundings.Count);

                if ((_surroundPlayed & (1 << idx)) == 0) // not played yet
                {
                    _surroundPlayed |= (1 << idx); // set sample as played
                    
                    if (_surroundPlayed == _surroundPlayedMaxBit) // reset played samples
                        _surroundPlayed = 0;

                    
                    StageSoundSource snd = AmbienceSurroundings[idx];
                    _surroundingsSamples[idx].Play(snd.Volume, snd.Loop);

                    _elapsedTime = 0;
                    _nextSurroundTime = _minDelay + SnailsGame.Rand.Next(_maxRandomDelay);
                }
            }
        }

        public void Draw()
        { }

        public void UnloadContent()
        { }

        public void PlayMusic()
        {
            _music.CancelFade();
            if (_music != null)
                _music.Play(true);
        }

        public void PlayMusicIfNotPlaying()
        {
            if (_music != null && !_musicIsPlaying)
            {
                _music.Play(true);
                _musicIsPlaying = true;
            }
        }

        public void StopMusic()
        {
            _musicIsPlaying = false;
            if (_music != null)
            {
                _music.Fade(0, new TimeSpan(0, 0, 0, 0, AudioTags.MUSIC_FADE_MSECONDS));
            }
        }

        public void PlayAmbience()
        {
            if (_ambienceSample != null)
                _ambienceSample.Play(AmbienceSetting.Volume, true);
        }

        public void PauseAmbience()
        {
            if (_ambienceSample != null)
                _ambienceSample.Pause();
        }

        public void ResumeAmbience()
        {
            if (_ambienceSample != null)
                _ambienceSample.Resume();
        }

        public void InitFromDataFileRecord(DataFileRecord record)
        {
            Music.InitFromDataFileRecord(record.SelectRecord("Music"));
            AmbienceSetting.InitFromDataFileRecord(record.SelectRecord("Ambience\\Setting")); 

            DataFileRecord surroundingsRecord = record.SelectRecord("Ambience\\Surroundings");
            if (surroundingsRecord != null)
            {
                _minDelay = surroundingsRecord.GetFieldValue<int>("minDelay", DEFAULT_SURROUND_TIME);
                _maxRandomDelay = surroundingsRecord.GetFieldValue<int>("maxRandomDelay", DEFAULT_SURROUND_TIME_RND);

                DataFileRecordList surroundList = surroundingsRecord.SelectRecords("Surround");
                if (surroundList != null)
                {
                    _surroundPlayedMaxBit = (int)Math.Pow(2, surroundList.Count) - 1;
                    AmbienceSurroundings = new List<StageSoundSource>(surroundList.Count);

                    foreach (DataFileRecord surround in surroundList)
                    {
                        StageSoundSource sss = new StageSoundSource();
                        sss.InitFromDataFileRecord(surround);
                        AmbienceSurroundings.Add(sss);
                    }
                }
            }
        }

        public DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("Sound");           
            record.AddRecord(Music.ToDataFileRecord("Music"));

            DataFileRecord ambienceRecord = new DataFileRecord("Ambience");
            ambienceRecord.AddRecord(AmbienceSetting.ToDataFileRecord("Setting"));

            DataFileRecord surroundingsRecord = new DataFileRecord("Surroundings");
            surroundingsRecord.AddField("minDelay", _minDelay);
            surroundingsRecord.AddField("maxRandomDelay", _maxRandomDelay); // to randonly play this surroundings in stage
            foreach (StageSoundSource sss in AmbienceSurroundings)
            {
                surroundingsRecord.AddRecord(sss.ToDataFileRecord("Surround"));
            }
            ambienceRecord.AddRecord(surroundingsRecord);
            record.AddRecord(ambienceRecord);

            return record;
        }
    }
}
