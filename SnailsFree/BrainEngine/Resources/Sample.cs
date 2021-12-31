using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Audio;

namespace TwoBrainsGames.BrainEngine.Resources
{
    public class Sample : Resource
    {
        public enum SampleState
        {
            Invalid = -1,
            Playing = 0,
            Paused = 1,
            Stopped = 2,
        }

        public delegate void FadeOutSampleHandler();
        public event FadeOutSampleHandler OnFadeOut;

        #region Member
        private SoundEffect _sample;
        private SoundEffectInstance _sampleInstance;
        // fade effect members
        private bool _isFading = false;
        //private float _saveMasterVolume;
        public float _sourceVolume;
        public float _targetVolume;
        private TimeSpan _elapsedTime;
        private TimeSpan _fadeDuration;
        private bool _canApply3D = false;
        private Object2D _objEmitter;
        private AudioEmitter _emitter;
        #endregion

        #region Properties
        public double PlaySameEffectMinTime { get; set; }
        public SoundEffect Sound
        {
            get { return _sample; }
            set { _sample = value; }
        }

        public AudioEmitter Emitter
        {
            get 
            {
                if (_canApply3D)
                {
                    _emitter.Position = SampleManager.Sample3DPosition(_objEmitter.Position);
                    return _emitter;
                }
                return null;
            }
        }

        public Object2D ObjEmitter
        {
            get { return this._objEmitter; }
        }

        public bool IsFading
        {
            get { return _isFading; }
        }

        public bool IsDisposed
        {
            get
            {
                if (_sampleInstance != null)
                    return _sampleInstance.IsDisposed;

                return false;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (_sampleInstance != null && !_sampleInstance.IsDisposed)
                    return _sampleInstance.State == SoundState.Playing;

                return false;
            }
        }

        public bool IsStopped
        {
            get
            {
                if (_sampleInstance != null && !_sampleInstance.IsDisposed)
                    return _sampleInstance.State == SoundState.Stopped;

                return false;
            }
        }

        public bool IsPaused
        {
            get
            {
                if (_sampleInstance != null && !_sampleInstance.IsDisposed)
                    return _sampleInstance.State == SoundState.Paused;

                return false;
            }
        }

        public bool CanApply3D
        {
            get { return _canApply3D; }
            set { _canApply3D = value; }
        }
        #endregion

        public Sample() :
            this(null, null)
        { }

        public Sample(SoundEffect sound)
            : this(sound, null)
        {
            
        }

        public Sample(SoundEffect sound, Object2D objEmitter)
        {
            _sample = sound;
            _objEmitter = objEmitter;
            if (_objEmitter != null)
            {
                _canApply3D = true;
                _emitter = new AudioEmitter();
                _emitter.Position = new Vector3(_objEmitter.Position.X, _objEmitter.Position.Y, 0);
            }
            this.PlaySameEffectMinTime = 20;
        }

        public override bool Load(ContentManager contentManager)
        {
            IsLoaded = false;

            _sample = contentManager.Load<SoundEffect>(Path);
            if (_sample != null)
            {
                IsLoaded = true;
            }

            return IsLoaded;
        }

        public override bool Release(ContentManager contentManager)
        {
            if (_sample != null)
                _sample.Dispose();
            IsLoaded = false;
            return true;
        }

        /// <summary>
        /// Play sample with default settings
        /// </summary>
        public void Play()
        {
            if (!this.IsPlaying)
            {
                this._isFading = false;
                Play(1.0f, 0.0f, 0.0f, false);
            }
        }

        /// <summary>
        /// Play sample and loop playing
        /// </summary>
        /// <param name="loop">True to enable sample looping</param>
        public void Play(bool loop)
        {
            if (!this.IsPlaying)
            {
                Play(1.0f, 0.0f, 0.0f, loop);
            }
        }

        /// <summary>
        /// Play sample and set volume
        /// </summary>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        public void Play(float volume)
        {
            if (!this.IsPlaying)
            {
                Play(volume, 0.0f, 0.0f, false);
            }
        }

        /// <summary>
        /// Play sample and set volume
        /// </summary>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// /// <param name="loop">True to enable sample looping</param>
        public void Play(float volume, bool loop)
        {
            if (!this.IsPlaying)
            {
                Play(volume, 0.0f, 0.0f, loop);
            }
        }

        /// <summary>
        /// Plays sample with special settings
        /// </summary>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        /// <param name="loop">True to enable sample looping</param>
        private void Play(float volume, float pitch, float pan, bool loop)
        {
            _sampleInstance = BrainGame.SampleManager.Play(this, volume, pitch, pan, loop);
        }

        /// <summary>
        /// Pause sample
        /// </summary>
        public void Pause()
        {
            if (_sampleInstance != null && !_sampleInstance.IsDisposed)
                _sampleInstance.Pause();
        }

        /// <summary>
        /// Resume sample
        /// </summary>
        public void Resume()
        {
            if (_sampleInstance != null && !_sampleInstance.IsDisposed)
                _sampleInstance.Resume();
        }

        /// <summary>
        /// Stop sample and free from memory
        /// </summary>
        public void Stop()
        {
            if (_sampleInstance != null && !_sampleInstance.IsDisposed)
            {
                _sampleInstance.Stop();
            }
        }

        /// <summary>
        /// Get sample states
        /// </summary>
        /// <returns></returns>
        public SampleState State()
        {
            if (_sampleInstance != null)
                return (SampleState)_sampleInstance.State;
            
            return SampleState.Invalid;
        }

        public float GetFadeVolume()
        {
            return MathHelper.Lerp(_sourceVolume, _targetVolume, (float)_elapsedTime.Ticks / _fadeDuration.Ticks);
        }

        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public void Fade(float sourceVolume, float targetVolume, TimeSpan fadeDuration)
        {
            //_saveMasterVolume = BrainGame.SampleManager.MasterVolume;
            _sourceVolume = sourceVolume;
            _targetVolume = targetVolume;
            _fadeDuration = fadeDuration;
            _elapsedTime = TimeSpan.Zero;

            _isFading = true;
        }

        public void FadeIn(TimeSpan fadeDuration)
        {
            if (!_isFading)  // to unsure we don't call more than once
            {
                Fade(0f, 1f, fadeDuration);
            }
        }

        public void FadeOut(TimeSpan fadeDuration)
        {
            if (!_isFading) // to unsure we don't call more than once
            {
                Fade(1f, 0f, fadeDuration);
            }
        }

        public void FadeCancel()
        {
            _isFading = false; // retain the same volume when stop
        }

        /// <summary>
        /// Used it only to allow fade in this sample
        /// </summary>
        public void FadeUpdate(BrainGameTime gameTime)
        {
            if (_isFading && 
                _sampleInstance != null && 
                 !_sampleInstance.IsDisposed && 
                _sampleInstance.State == SoundState.Playing)
            {
                _elapsedTime += gameTime.ElapsedGameTime;

                if (_elapsedTime >= _fadeDuration)
                {
                    _elapsedTime = _fadeDuration;
                    _isFading = false;
                }

                _sampleInstance.Volume = GetFadeVolume();

                if (!_isFading) // end fading, stop sample
                {
                    if (_sampleInstance.Volume == 0)
                    {
                        this.Stop();
                        if (this.OnFadeOut != null) // fade out
                        {
                            this.OnFadeOut();
                        }
                    }
                }
            }
        }


    }
}
