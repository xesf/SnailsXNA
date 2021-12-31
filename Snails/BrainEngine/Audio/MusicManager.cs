using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace TwoBrainsGames.BrainEngine.Audio
{
    public class MusicManager : GameComponent
    {
        public enum MusicState
        {
            Stopped = 0,
            Playing = 1,
            Paused = 2,
        }

        #region Constants
        private const float DEFAULT_VOLUME = 0.5f;
        #endregion

        #region Members
        public delegate void FadeOutMusicHandler(); // WARNING: this can only be used once
        public event FadeOutMusicHandler OnFadeOut; // WARNING: this can only be used once

        private bool _isMusicPaused = false;
        private bool _isFading = false;
        private Song _currentMusic = null;
        private float _saveMasterVolume;
        private MusicState _state = MusicState.Stopped;

        // fade effect members
        public float _sourceVolume;
        public float _targetVolume;
        private TimeSpan _elapsedTime;
        private TimeSpan _fadeDuration;
        #endregion

        #region Properties
        public bool EnabledMusic = true;

        /// <summary>
        /// Current playing music
        /// </summary>
        public Song CurrentSong 
        { 
            get { return _currentMusic; } 
        }

        /// <summary>
        /// Music volume: 1.0f is max volume
        /// </summary>
        public float MasterVolume
        {
            get { return this._saveMasterVolume; }
            set 
            {
                if (MediaPlayer.GameHasControl)
                {
                    MediaPlayer.Volume = value;
                }
                this._saveMasterVolume = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMusicActive 
        {
            get { return this._currentMusic != null && this._state != MusicState.Stopped; } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMusicPaused 
        {
            get { return this._currentMusic != null && this._isMusicPaused; } 
        }
        #endregion

        /// <summary>
        /// Creates a new Sample Manager component
        /// </summary>
        public MusicManager(BrainGame game)
            :base(game)
        {
            MasterVolume = DEFAULT_VOLUME;
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        public void PlayMusic(Song song)
        {
            PlayMusic(song, false);
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">True if song should loop, false otherwise</param>
        public void PlayMusic(Song song, bool loop)
        {
            if (!MediaPlayer.GameHasControl)
            {
                return;
            }

            if (song != this._currentMusic)
            {
                if (this._currentMusic != null && this._state == MusicState.Playing)
                {
                    this._state = MusicState.Stopped;
                    MediaPlayer.Stop();
                }

                this._currentMusic = song;

                MediaPlayer.Volume = this._saveMasterVolume;

                this._isMusicPaused = false;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(this._currentMusic);
                this._state = MusicState.Playing;

                if (!this.EnabledMusic)
                {
                    this._state = MusicState.Paused;
                    MediaPlayer.Pause();
                }
            }
        }

        /// <summary>
        /// Pauses the currently playing song. This is a no-op if the song is already paused,
        /// or if no song is currently playing.
        /// </summary>
        public void PauseMusic()
        {
            if (!MediaPlayer.GameHasControl)
            {
                return;
            }

            if (this._currentMusic != null && !this._isMusicPaused)
            {
                if (this.EnabledMusic) 
                    MediaPlayer.Pause();
                this._isMusicPaused = true;
                this._state = MusicState.Paused;
            }
        }

        /// <summary>
        /// Resumes the currently paused song. This is a no-op if the song is not paused,
        /// or if no song is currently playing.
        /// </summary>
        public void ResumeMusic()
        {
            if (!MediaPlayer.GameHasControl)
            {
                return;
            }

            if (this._currentMusic != null && this._isMusicPaused)
            {
                if (this.EnabledMusic)
                {
                    MediaPlayer.Volume = this._saveMasterVolume;
                    MediaPlayer.Resume();
                }
                this._isMusicPaused = false;
                this._state = MusicState.Playing;
            }
        }

        /// <summary>
        /// Stops the currently playing song. This is a no-op if no song is currently playing.
        /// </summary>
        public void StopMusic()
        {
            if (!MediaPlayer.GameHasControl)
            {
                return;
            }

            if (this._currentMusic != null && this._state != MusicState.Stopped)
            {
                this._state = MusicState.Stopped;
                this._isMusicPaused = false;
                MediaPlayer.Stop();
                MediaPlayer.Volume = this._saveMasterVolume;
            }
            this._currentMusic = null;
        }

        public float GetFadeVolume()
        {
            return MathHelper.Lerp(this._sourceVolume, this._targetVolume, (float)this._elapsedTime.Ticks / this._fadeDuration.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FadeMusic(float targetVolume, int msecs)
        {
            this.FadeMusic(targetVolume, new TimeSpan(0, 0, 0, 0, msecs));
        }

        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public void FadeMusic(float targetVolume, TimeSpan fadeDuration)
        {
            this._sourceVolume = MasterVolume;
            this._targetVolume = targetVolume;
            this._fadeDuration = fadeDuration;
            this._elapsedTime = TimeSpan.Zero;

            this._isFading = true;
        }

        /// <summary>
        /// Stop the current fade.
        /// </summary>
        /// <param name="option">Options for setting the music volume</param>
        public void CancelFade()
        {
            if (this._isFading)
            {
                this._isFading = false;
                this.StopMusic();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Do nothing if the game is not active. Don't remove this, accessing MediaPlayer.State when
            // the game is inactive has a strage side effect
            if (BrainGame.IsGameActive == false)
            {
                return;
            }

            if (this._currentMusic != null && this._state == MusicState.Stopped)
            {
                this._currentMusic = null;
                this._isMusicPaused = false;
            }

            if (MediaPlayer.GameHasControl && 
                this._isFading &&
                !this._isMusicPaused &&
                this._state == MusicState.Playing)
            {
                this._elapsedTime += gameTime.ElapsedGameTime;

                if (this._elapsedTime >= this._fadeDuration)
                {
                    this._elapsedTime = this._fadeDuration;
                    this._isFading = false;
                }

                MediaPlayer.Volume = GetFadeVolume();

                if (!_isFading) // end fading, stop music
                {
                    this.StopMusic();
                    if (this.OnFadeOut != null) // fade out
                    {
                        this.OnFadeOut();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisableMusic()
        {
            this.EnabledMusic = false;
            this.PauseMusic();
        }

        /// <summary>
        /// 
        /// </summary>
        public void EnableMusic()
        {
            this.EnabledMusic = true;
            this.ResumeMusic();
        }
    }
}
