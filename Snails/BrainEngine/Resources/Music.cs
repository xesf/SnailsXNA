using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using TwoBrainsGames.BrainEngine.Audio;

namespace TwoBrainsGames.BrainEngine.Resources
{
    public class Music : Resource
    {
        #region Member
        private Song _song;
        private TwoBrainsGames.BrainEngine.Audio.MusicManager.MusicState _state = MusicManager.MusicState.Stopped;
        #endregion

        #region Properties
        public Song Song
        {
            get { return _song; }
            set { _song = value; }
        }

        public bool IsPlaying
        {
            get { return _state == MusicManager.MusicState.Playing; }
        }
        public bool IsPaused
        {
            get { return _state == MusicManager.MusicState.Paused; }
        }
        public bool IsStopped
        {
            get { return _state == MusicManager.MusicState.Stopped; }
        }
        #endregion

        public Music()
        { }

        public Music(Song song)
        {
            _song = song;
        }

        public override bool Load(ContentManager contentManager)
        {
            IsLoaded = false;

            _song = contentManager.Load<Song>(Path);
            if (_song != null)
            {
                IsLoaded = true;
            }

            return IsLoaded;
        }

        public override bool Release(ContentManager contentManager)
        {
            if (_song != null)
                _song.Dispose();
            IsLoaded = false;
            return true;
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        public void Play()
        {
            Play(false);
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">True if song should loop, false otherwise</param>
        public void Play(bool loop)
        {
            _state = MusicManager.MusicState.Playing;
            BrainGame.MusicManager.PlayMusic(_song, loop);
        }

        /// <summary>
        /// Pauses the currently playing song. This is a no-op if the song is already paused,
        /// or if no song is currently playing.
        /// </summary>
        public void Pause()
        {
            _state = MusicManager.MusicState.Paused;
            BrainGame.MusicManager.PauseMusic();
        }

        /// <summary>
        /// Resumes the currently paused song. This is a no-op if the song is not paused,
        /// or if no song is currently playing.
        /// </summary>
        public void Resume()
        {
            _state = MusicManager.MusicState.Playing;
            BrainGame.MusicManager.ResumeMusic();
        }

        /// <summary>
        /// Stops the currently playing song. This is a no-op if no song is currently playing.
        /// </summary>
        public void Stop()
        {
            _state = MusicManager.MusicState.Stopped;
            BrainGame.MusicManager.StopMusic();
        }

        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public void Fade(float targetVolume, TimeSpan duration)
        {
            BrainGame.MusicManager.FadeMusic(targetVolume, duration);
            if (targetVolume == 0)
                _state = MusicManager.MusicState.Stopped;
            else
                _state = MusicManager.MusicState.Playing;
        }

        /// <summary>
        /// Stop the current fade.
        /// </summary>
        /// <param name="option">Options for setting the music volume</param>
        public void CancelFade()
        {
            BrainGame.MusicManager.CancelFade();
            if (BrainGame.MusicManager.MasterVolume == 0)
                _state = MusicManager.MusicState.Stopped;
            else
                _state = MusicManager.MusicState.Playing;
        }
    }
}
