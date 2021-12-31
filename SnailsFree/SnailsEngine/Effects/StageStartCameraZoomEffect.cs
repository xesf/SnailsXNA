using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.Effects
{
    class StageStartCameraZoomEffect : TransformEffectBase
    {
        public const float MAX_ZOOM_IN = 0.85f;
        const double EFFECT_DURATION = 500;

        Vector2 _pointOfInterest;
        //Vector2 _initialScale;
        double _ellapsedTime;
        InGameCamera _camera;
        float _distance;
        float _speed;
        float _zoomSpeed;
        Vector2 _currentScale;

        /// <summary>
        /// 
        /// </summary>
        public StageStartCameraZoomEffect(InGameCamera camera)
        {
            this._camera = camera;
        }

        /// <summary>
        /// 
        /// </summary>
        public StageStartCameraZoomEffect(Vector2 initialScale, Vector2 poi, InGameCamera camera)
        {
            this._camera = camera;
            this.Reset(initialScale, poi);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this._ellapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
            // Movement
            float frameSpeed = this._speed * (float) gameTime.ElapsedRealTime.TotalMilliseconds;
            Vector2 dir = (this._pointOfInterest - this._camera.Position);
            dir.Normalize();
            this.PositionV2 = dir * frameSpeed;
            
            // Scale
            float increment = this._zoomSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            this._currentScale += new Vector2(increment, increment);
            if (this._currentScale.X > MAX_ZOOM_IN ||
                this._currentScale.Y > MAX_ZOOM_IN)
            {
                this.Scale = new Vector2(MAX_ZOOM_IN, MAX_ZOOM_IN);
                this.Active = false;
                return;
            }
            this.Scale = this._currentScale;
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset(Vector2 initialScale, Vector2 poi)
        {
            base.Reset();
            this._ellapsedTime = 0;
            this._pointOfInterest = poi;
            //this._initialScale = initialScale;
            this._distance = (this._camera.Position - poi).Length();
            this._speed = (this._distance / (float)EFFECT_DURATION);
            this._currentScale = initialScale;
            this.Scale = this.LastScale = initialScale;
            this._zoomSpeed = (MAX_ZOOM_IN - initialScale.X) / (float)EFFECT_DURATION;
         }
    }
}
