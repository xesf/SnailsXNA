using System;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects
{
    public class TransformEffectBase : ITransformEffect
    {
     
        public delegate void OnEndEvent(object param);

        public OnEndEvent OnEnd { get; set; }

        #region Vars
        private bool _ended;
        public double _expirationTime; // Expiration time in msecs. 0 if not set
        public double _ellapsedExpirationTime;
        public object _onEndEventParam;
        #endregion

        #region Properties
        public bool Active { get; set; }
        public bool UseRealTime { get; set; }

        Matrix Transform { get; set; }
        public virtual bool Ended  
        { 
            get { return this._ended; }
            set
            {
                if (this._ended != value)
                {
                    this._ended = value;
                    if (this._ended && this.OnEnd != null)
                    {
                        this.OnEnd(this._onEndEventParam);
                    }
                }
            }
        }
     
  
        public Vector3 Position { get; set; }
        public Vector2 PositionV2
        {
            get { return new Vector2(this.Position.X, this.Position.Y); }
            set { this.Position = new Vector3(value.X, value.Y, 0.0f); }
        }
        public Vector3 VirtualPosition { get; set; }
        public Vector2 VirtualPositionV2
        {
            get { return new Vector2(this.VirtualPosition.X, this.VirtualPosition.Y); }
            set { this.VirtualPosition = new Vector3(value.X, value.Y, 0.0f); }
        }

        public float Rotation { get; set;}
        public float VirtualRotation { get; set; }
        Vector4 _ColorVector;
        public Vector4 ColorVector 
        { 
            get { return this._ColorVector; }
            set { this._ColorVector = value; } 
        }
        public Color Color { get { return new Color(this.ColorVector); } }

        public Vector2 Scale { get; set; }
        public Vector2 LastScale { get; set; }

        public bool AutoDeleteOnEnd { get; set; }
        public bool SetScaleOnEnd { get; set; }

        #endregion

        #region Contructs and overrides
        /// <summary>
        /// 
        /// </summary>
        public TransformEffectBase():
          this(true)
        {
            this.Reset();
            this.Active = true;
            this.AutoDeleteOnEnd = true;
            this.SetScaleOnEnd = true;
        } 
      
        /// <summary>
        /// 
        /// </summary>
        public TransformEffectBase(bool persistentEffect)
        {
          this.Transform = Matrix.Identity;
        }
        #endregion

        #region ITransformEffect Members

        public virtual void Update(BrainGameTime gameTime)
        {
            
        }

        public virtual void InternalUpdate(BrainGameTime gameTime)
        {
            if (this._expirationTime > 0)
            {
                this._ellapsedExpirationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this._ellapsedExpirationTime >= this._expirationTime)
                {
                    this.Ended = true;
                    return;
                }
            }
            this.Update(gameTime);
        }

        public virtual void Reset()
        {
          this.Transform = Matrix.Identity;
          this.Ended = false;
          this.Position = new Vector3();
          this.Rotation = 0.0f;
          this.VirtualPosition = new Vector3();
          this.VirtualRotation = 0.0f;
          this.ColorVector = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
          this._ellapsedExpirationTime = 0;
          this.Scale = new Vector2(1.0f, 1.0f);
          this.LastScale = this.Scale;
        }

        #endregion
    }
}
