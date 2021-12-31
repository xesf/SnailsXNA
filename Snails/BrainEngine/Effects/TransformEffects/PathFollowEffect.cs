using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
    public class PathFollowEffect : TransformEffectBase, ITransformEffect
    {
        List<Vector3> _pathPoints;
        float _speed;
        //bool _connectEndPoints; // If true, the start/end point of the path will be connected making closed path
        int _currentPointIdx;
        Vector3 _currentPosition;
        Vector3 _previousPosition;

        /// <summary>
        /// 
        /// </summary>
        public PathFollowEffect(List<Vector3> pathPoints, float speed, bool connectEndPoints)
        {
            this._pathPoints = pathPoints;
            this._speed = speed;
            //this._connectEndPoints = connectEndPoints;
            this._currentPointIdx = 0;
            if (this._pathPoints.Count > 0)
            {
                this._currentPosition = this._previousPosition = this._pathPoints[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PathFollowEffect(Vector3 startPoint, Vector3 endPoint, float speed, bool connectEndPoints) :
            this(new List<Vector3>(), speed, connectEndPoints)
        {
            this._pathPoints.Add(startPoint);
            this._pathPoints.Add(endPoint);
            this._currentPosition = this._previousPosition = this._pathPoints[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public PathFollowEffect(Vector2 startPoint, Vector2 endPoint, float speed, bool connectEndPoints) :
            this(new Vector3(startPoint.X, startPoint.Y, 0.0f),
                 new Vector3(endPoint.X, endPoint.Y, 0.0f), speed, connectEndPoints)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this._previousPosition = this._currentPosition;
            if (this._pathPoints[this._currentPointIdx].Equals(this._pathPoints[this._currentPointIdx + 1]))
            {
                this.Ended = true;
                return;
            }

            float speed = this._speed * gameTime.ElapsedGameTime.Milliseconds / 10;
            Vector3 currentPath = this._pathPoints[this._currentPointIdx + 1] - this._pathPoints[this._currentPointIdx];

            Vector3 dirVector = currentPath;
            dirVector.Normalize();
            Vector3 deltaMove = new Vector3(dirVector.X * speed, dirVector.Y * speed, 0.0f);
            this._currentPosition += deltaMove;

            // Check if position is out of the path
            Vector3 travelled = this._pathPoints[this._currentPointIdx] - this._currentPosition;
            float diffTravelled = travelled.Length() - currentPath.Length();

            if (diffTravelled > 0)
            {
                this.Ended = true;
                this._currentPosition = this._pathPoints[this._currentPointIdx + 1];
            }

            this.Position = this._currentPosition - this._previousPosition;
        }
    }
}
