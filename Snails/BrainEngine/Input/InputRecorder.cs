using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.BrainEngine.Input
{
    public class InputRecorder
    {
        enum RecorderState
        {
            Stopped,
            Recording,
            Playing
        }

        private InputRecorderStream _currentStream;
        private double _ellapsedTime;
        //private TimeSpan _startTime;
        private RecorderState _state;
        private int _currentPosition;

        public InputRecorderStream InputStream { get { return this._currentStream; } }
        public InputRecorderStream.StreamItem CurrentItem
        {
            get
            {
                if (this._currentStream.ItemsCount <= this._currentPosition)
                {
                    return null;
                }
                return (this._currentStream[this._currentPosition]);
            }
        }

        public bool IsActive
        {
            get { return (this._state != RecorderState.Stopped); }
        }

        public bool IsRecording
        {
            get { return (this._state == RecorderState.Recording); }
        }

        public bool IsPlaying
        {
            get { return (this._state == RecorderState.Playing); }
        }

        public InputRecorder()
        {
            this._state = RecorderState.Stopped;
            this._currentStream = new InputRecorderStream();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartRecording()
        {
            this._state = RecorderState.Recording;
            //this._startTime = DateTime.Now.TimeOfDay;
            this._currentStream.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            this._state = RecorderState.Stopped;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayBack()
        {
            this._state = RecorderState.Playing;
            this.Rewind();

        }


        /// <summary>
        /// 
        /// </summary>
        public void Rewind()
        {
            this._currentPosition = 0;
            this._ellapsedTime = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime, ulong action, Vector2 position)
        {
            if (this.IsActive == false)
            {
                return;
            }

            switch(this._state)
            {
                case RecorderState.Recording:
                    this._ellapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    this._currentStream.Append(this._ellapsedTime, action, position);
                    break;

                case RecorderState.Playing:

                    if (this._currentPosition < this._currentStream.ItemsCount)
                    {
                        this._ellapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        while (this._currentStream[this._currentPosition]._milliseconds < this._ellapsedTime)
                        {
                            this._currentPosition++;
                            if (this._currentPosition >= this._currentStream.ItemsCount)
                            {
                                break;
                            }
                        }
                    }

                    if (this._currentPosition >= this._currentStream.ItemsCount)
                    {
                        this.Stop();
                        break;
                    }
                    break;
            }
        }
    }
}
