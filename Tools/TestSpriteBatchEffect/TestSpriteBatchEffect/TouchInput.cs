using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using TestSpriteBatchEffect;

namespace TwoBrainsGames.BrainEngine.Input
{
    public class TouchInput : GameComponent
    {
        private GestureType _gestureType;
        private Vector2 _flickDelta;
        private float _pinchDistance;
        private float _pinchPrevDistance;
        private Vector2 _pinchCenterPosition;

        private TouchLocation _lastLocation;
        private TouchLocation _previousLocation;

        private TouchLocationState _lastLocationState;

        public TouchInput(Game game) :
            base(game)
        { }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            this.Game.IsMouseVisible = false;

            TouchPanel.EnabledGestures = 
                        GestureType.Tap |
                        GestureType.DoubleTap |
                        GestureType.Hold |
                        GestureType.FreeDrag |
                        GestureType.HorizontalDrag |
                        GestureType.VerticalDrag |
                        //GestureType.DragComplete |
                        GestureType.Pinch |
                        GestureType.PinchComplete |
                        GestureType.Flick;

            base.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            this._lastLocationState = TouchLocationState.Invalid;
            this._gestureType = GestureType.None;
            GestureSample firstPinchGesture = new GestureSample();
            GestureSample lastPinchGesture = new GestureSample();
            int pinchCount = 0;

			TouchCollection touches = TouchPanel.GetState();
			if (touches.Count >= 1)
			{
				this._previousLocation = this._lastLocation;
				this._lastLocation = touches[0];
				this._lastLocationState = this._lastLocation.State;
			}

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gestureSample = TouchPanel.ReadGesture();
                this._gestureType |= gestureSample.GestureType;

                if (gestureSample.GestureType == GestureType.Flick)
                {
                    this._flickDelta = gestureSample.Delta;
                }
                else if (gestureSample.GestureType == GestureType.Pinch)
                {
                    pinchCount++;
                    if (pinchCount == 1)
                    {
                        firstPinchGesture = lastPinchGesture = gestureSample;
                    }
                    else
                    {
                        lastPinchGesture = gestureSample;
                    }
                }
            }

            // Use the first and last pinch gestures from the queue to calculate the delta
            if (pinchCount > 0)
            {
                Vector2 p = lastPinchGesture.Position;
                Vector2 pOld = firstPinchGesture.Position;
                Vector2 p2 = lastPinchGesture.Position2;
                Vector2 p2Old = firstPinchGesture.Position2;

                // distance between the current and previous locations
                float d = Vector2.Distance(p, p2);
                float dOld = Vector2.Distance(pOld, p2Old);

                // difference between the two locations
                _pinchDistance = (d - dOld); // will be used to factor the scale

                // pinch center point
                this._pinchCenterPosition = (p + p2) / 2;

                this._pinchPrevDistance = Vector2.Distance(pOld, p2Old);
                this._pinchDistance = Vector2.Distance(p, p2);
            }

            
        }

        public bool CanGetPosition { get { return this._lastLocationState != TouchLocationState.Invalid; } }
        public bool WasPressed { get { return (this._lastLocationState == TouchLocationState.Pressed && this._previousLocation.State == TouchLocationState.Released);} }
        public bool IsDown { get { return (this._lastLocationState == TouchLocationState.Pressed || this._lastLocationState == TouchLocationState.Moved); } }
        public bool WasReleased { get { return this._lastLocationState == TouchLocationState.Released; } }
        public bool HasMoved { get { return this._lastLocationState == TouchLocationState.Moved; } }
        public bool IsTapping { get { return (this._gestureType & GestureType.Tap) == GestureType.Tap; } }
        public bool IsDoubleTapping { get { return ((this._gestureType & GestureType.DoubleTap) == GestureType.DoubleTap); } }
        public bool StartDragging { get { return ((this._gestureType & GestureType.FreeDrag) == GestureType.FreeDrag || (this._gestureType & GestureType.HorizontalDrag) == GestureType.HorizontalDrag || (this._gestureType & GestureType.VerticalDrag) == GestureType.VerticalDrag); } }
        public bool EndDragging { get { return ((this._gestureType & GestureType.DragComplete) == GestureType.DragComplete); } }
        public bool IsHorizontalDrag { get { return ((this._gestureType & GestureType.HorizontalDrag) == GestureType.HorizontalDrag); } }
        public bool IsVerticalDrag { get { return ((this._gestureType & GestureType.VerticalDrag) == GestureType.VerticalDrag); } }
        public bool OnHold { get { return ((this._gestureType & GestureType.Hold) == GestureType.Hold); } }
        public bool IsNone { get { return (this._gestureType == GestureType.None); } }
        public bool HasFlick { get { return ((this._gestureType & GestureType.Flick) == GestureType.Flick); } }
        public bool StartPinching { get { return ((this._gestureType & GestureType.Pinch) == GestureType.Pinch); } }
        public bool EndPinching { get { return ((this._gestureType & GestureType.PinchComplete) == GestureType.PinchComplete); } }

        public Vector2 FlickDelta
        {
            get { return this._flickDelta; }
        }

        public float PinchDistance
        {
            get { return this._pinchDistance; }
        }

        public float PinchPrevDistance
        {
            get { return this._pinchPrevDistance; }
        }

        public Vector2 PinchCenterPosition
        {
            get { return this._pinchCenterPosition; }
        }

        public Vector2 GetTouchPosition()
        {
            Vector2 pos = Vector2.Zero;

            if (this.CanGetPosition)
            {
                pos = this._lastLocation.Position;
            }

            return pos;
        }

        public bool CheckActionStartPressed()
        {
            return this.IsTapping || this.IsDoubleTapping;
        }
    }
}
