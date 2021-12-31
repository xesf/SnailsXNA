using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Debugging;

namespace TwoBrainsGames.BrainEngine.Audio
{
    public class SampleManager : GameComponent
    {
        private const float OFF_AUDIBLE_ZONE_DECAY = 0.5f;

        class PlayingSample {
            public bool CanApply3D;
            public SoundEffectInstance SoundInstance;
            public AudioEmitter Emitter;
            public Sample _sample;
            public bool _insideOfAudibleBB;
            
            public bool CheckInsideAudibleBB()
            {
                if (_sample.ObjEmitter == null)
                {
                    return true;
                }
                return (BrainGame.SampleManager.AudibleBoundingSquare.Collides(_sample.ObjEmitter.SoundEmmiterBoundingBox));
            }
        }

        class PlayingSoundEffectData
        {
            public SoundEffect _soundEffect;
            public DateTime _startTime;

            public PlayingSoundEffectData(SoundEffect soundEffect)
            {
                _soundEffect = soundEffect;
            }
        }
        #region Constants
        // Maximum number of simultaneous sounds that can be playing
        private const int MAX_SAMPLES = 32;
        public const float FRACTION_3D = 250;
        public const int MAX_SOUND_EFFECTS_AT_SAME_TIME = 2; // Maximum number of sound effects of the same type playing
        #endregion

        #region Members
        // All current active sounds
        private PlayingSample[] _playingSamples = new PlayingSample[MAX_SAMPLES];
        private AudioListener _audioListener;
        #endregion

        #region Properties
        public bool EnabledSamples = true;

        /// <summary>
        /// Samples master volume: 1.0f is max volume
        /// </summary>
        public float MasterVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }

        private List<PlayingSoundEffectData> CurrentlyPlayingSoundEffects { get; set; }
        public BoundingSquare AudibleBoundingSquare { get; set; }
        public bool UseAudibleBoundingSquare { get; set; }
        #endregion

        /// <summary>
        /// Creates a new Sample Manager component
        /// </summary>
        public SampleManager(BrainGame game)
            :base(game)
        {
            _audioListener = new AudioListener();
            this.CurrentlyPlayingSoundEffects = new List<PlayingSoundEffectData>();
        }

        public void SetAudioListenerPosition(Vector2 camPos)
        {
            _audioListener.Position = Sample3DPosition(camPos);
        }

        public static Vector3 Sample3DPosition(Vector2 pos)
        {
            return new Vector3(pos.X / FRACTION_3D, 0, pos.Y / FRACTION_3D);
        }

        /// <summary>
        /// Get available slot
        /// </summary>
        /// <returns></returns>
        private int GetAvailableSlot()
        {
            for (int i = 0; i < _playingSamples.Length; ++i)
            {
                if (_playingSamples[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Plays the sound of the given name.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        public SoundEffectInstance Play(Sample sample)
        {
            return Play(sample, 1.0f, 0.0f, 0.0f, false);
        }

        /// Plays the sound of the given name.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        public SoundEffectInstance Play(Sample sample, bool loop)
        {
            return Play(sample, 1.0f, 0.0f, 0.0f, loop);
        }

        /// <summary>
        /// Plays the sound of the given name at the given volume.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        public SoundEffectInstance Play(Sample sample, float volume)
        {
            return Play(sample, volume, 0.0f, 0.0f, false);
        }

		private float XnaPitchToAlPitch(float pitch)
        {
            // pitch is different in XNA and OpenAL. XNA has a pitch between -1 and 1 for one octave down/up.
            // openAL uses 0.5 to 2 for one octave down/up, while 1 is the default. The default value of 0 would make it completely silent.
            float alPitch = 1;
            if (pitch < 0)
                alPitch = 1 + 0.5f * pitch;
            else if (pitch > 0)
                alPitch = 1 + pitch;
            return alPitch;
        }
        
        /// <summary>
        /// Plays the sound of the given name with the given parameters.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        public SoundEffectInstance Play(Sample sample, float volume, float pitch, float pan, bool loop)
        {
            if (sample == null)
            {
                throw new BrainException("Invalid sample to play");
            }

            PlayingSoundEffectData soundEffectData = this.GetPlayingSoundEffect(sample.Sound);
            if (soundEffectData == null)
            {
                soundEffectData = new PlayingSoundEffectData(sample.Sound);
                this.CurrentlyPlayingSoundEffects.Add(soundEffectData);
            }
            else
            {
                // Don't play the same sample if hasn't been more then x time before last one started
                if (DateTime.Now.Subtract(soundEffectData._startTime).TotalMilliseconds < sample.PlaySameEffectMinTime)
                {
                    return null;
                }
            }
            soundEffectData._startTime = DateTime.Now;

            int index = GetAvailableSlot();

            if (index != -1)
            {
                _playingSamples[index] = new PlayingSample();
                _playingSamples[index].CanApply3D = sample.CanApply3D;
                _playingSamples[index].Emitter = sample.Emitter;
                _playingSamples[index].SoundInstance = sample.Sound.CreateInstance();
                _playingSamples[index].SoundInstance.Volume = volume;
                _playingSamples[index]._sample = sample;
                if (BrainGame.GameTime.Multiplier > 1)
				{
                   // pitch -= 0.3f; // 0.7
                    pitch *= 0.7f; // 0.7
				}

                _playingSamples[index].SoundInstance.Pitch = pitch;
                _playingSamples[index].SoundInstance.Pan = pan;
                _playingSamples[index].SoundInstance.IsLooped = loop;
                
                if (_playingSamples[index].CanApply3D)
                {
                    _playingSamples[index].SoundInstance.Apply3D(_audioListener, _playingSamples[index].Emitter);
                }

                _playingSamples[index].SoundInstance.Play();
                if (!EnabledSamples)
                {
                    _playingSamples[index].SoundInstance.Pause();
                }
                _playingSamples[index]._insideOfAudibleBB = true;
             /*   if (this.UseAudibleBoundingSquare && sample.ObjEmitter != null)
                {
                    _playingSamples[index]._insideOfAudibleBB = _playingSamples[index].CheckInsideAudibleBB();
                    if (!_playingSamples[index]._insideOfAudibleBB)
                    {
                        _playingSamples[index].SoundInstance.Volume *= OFF_AUDIBLE_ZONE_DECAY;
                    }
                }*/
                return _playingSamples[index].SoundInstance;
            }

            return null;
        }

        /// <summary>
        /// Higher Pitch for all samples
        /// </summary>
        public void ChangePlayingPitch(float pitch)
        {
            for (int i = 0; i < _playingSamples.Length; ++i)
            {
                if (_playingSamples[i] != null && _playingSamples[i].SoundInstance.State == SoundState.Playing)
                {
                    _playingSamples[i].SoundInstance.Pitch = pitch;
                }
            }
        }

        /// <summary>
        /// Stops all currently playing sounds
        /// </summary>
        public void StopAll()
        {
            for (int i = 0; i < _playingSamples.Length; ++i)
            {
                if (_playingSamples[i] != null)
                {
                    _playingSamples[i].SoundInstance.Stop();
                    _playingSamples[i].SoundInstance.Dispose();
                    _playingSamples[i] = null;
                }
            }
            this.CurrentlyPlayingSoundEffects.Clear();
        }

        /// <summary>
        /// Resume all currently playing sounds
        /// </summary>
        public void ResumeAll()
        {
            PauseResumeSounds(true);
        }

        /// <summary>
        /// Pause all currently playing sounds
        /// </summary>
        public void PauseAll()
        {
            PauseResumeSounds(false);
        }

        /// <summary>
        /// Pause and Resume all playing sounds
        /// </summary>
        /// <param name="resume"></param>
        private void PauseResumeSounds(bool resume)
        {
            if (!this.EnabledSamples)
                return;

            if (resume)
            {
                for (int i = 0; i < _playingSamples.Length; ++i)
                {
                    if (_playingSamples[i] != null && _playingSamples[i].SoundInstance.State == SoundState.Paused)
                    {
                        _playingSamples[i].SoundInstance.Resume();
                    }
                }
            }
            else
            {
                for (int i = 0; i < _playingSamples.Length; ++i)
                {
                    if (_playingSamples[i] != null && _playingSamples[i].SoundInstance.State == SoundState.Playing)
                    {
                        _playingSamples[i].SoundInstance.Pause();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisableSamples()
        {
            this.PauseAll();
            this.EnabledSamples = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void EnableSamples()
        {
            this.EnabledSamples = true;
            this.ResumeAll();
        }

        /// <summary>
        /// Manager update to track sound states
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!this.EnabledSamples)
                return;

            // Free sound instance
            for (int i = 0; i < _playingSamples.Length; ++i)
            {
                if (_playingSamples[i] != null && 
                    _playingSamples[i].SoundInstance != null)
                {
                    switch (_playingSamples[i].SoundInstance.State)
                    {
                        case SoundState.Playing:
                        case SoundState.Paused:
                            if (_playingSamples[i].CanApply3D)
                            {
                                _playingSamples[i].SoundInstance.Apply3D(_audioListener, _playingSamples[i].Emitter);
                            }
                       /*     if (this.UseAudibleBoundingSquare)
                            {
                                if (!_playingSamples[i]._insideOfAudibleBB)
                                {
                                    if (_playingSamples[i].CheckInsideAudibleBB() == true)
                                    {
                                        _playingSamples[i].SoundInstance.Volume /= OFF_AUDIBLE_ZONE_DECAY;
                                        _playingSamples[i]._insideOfAudibleBB = true;
                                    }
                                }
                                else
                                {
                                    if (_playingSamples[i].CheckInsideAudibleBB() == false)
                                    {
                                        _playingSamples[i].SoundInstance.Volume *= OFF_AUDIBLE_ZONE_DECAY;
                                        _playingSamples[i]._insideOfAudibleBB = false;
                                    }
                                }
                            }*/
                            break;

                        case SoundState.Stopped:
                            _playingSamples[i].SoundInstance.Dispose();
                            _playingSamples[i].SoundInstance = null;
                            _playingSamples[i] = null;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private PlayingSoundEffectData GetPlayingSoundEffect(SoundEffect effect)
        {
            foreach (PlayingSoundEffectData soundData in this.CurrentlyPlayingSoundEffects)
            {
                if (soundData._soundEffect == effect)
                {
                    return soundData;
                }
            }
            return null;
        }
    }
}
