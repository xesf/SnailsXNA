using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Effects
{
    public class TransformBlender
    {
        #region Structs
        class EffectData
        {
            public int _Id;
            public ITransformEffect _Effect;
            public bool _Deleted;

            public EffectData(int id, ITransformEffect effect)
            {
                this._Id = id;
                this._Effect = effect;
                this._Deleted = false;
            }
        }
        #endregion

        #region Member vars
        List<EffectData> _EffectList;
        Vector3 _Position;
        Vector4 _color;
        #endregion

        #region Properties
        public Vector2 _scale;
        public float Rotation { get; set; }
        public Color Color 
        { 
            get { return new Color(this._color); }
            set { this._color = value.ToVector4(); }
        }

        public int Count
        {
            get { return this._EffectList.Count; }
        }

        public Vector3 Position
        {
            get { return this._Position; }
        }

        public Vector2 PositionV2
        {
            get { return new Vector2(this._Position.X, this._Position.Y); }
        }

        public Vector3 VirtualPosition { get; set;}
        public Vector2 VirtualPositionV2
        {
            get { return new Vector2(this.VirtualPosition.X, this.VirtualPosition.Y); }
        }

        public float VirtualRotation { get; set; }
        public bool Active { get; set; }

        public ITransformEffect this[int i]
        {
            get 
            { 
                return this._EffectList[i]._Effect; 
            }
        }
        #endregion

        #region Contructs and overrides
        /// <summary>
        /// 
        /// </summary>
        public TransformBlender()
        {
            this._EffectList = new List<EffectData>();
            this.Active = true;
            this._color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// 
        /// </summary>
        public void DisableAll()
        {
            foreach (EffectData effectData in this._EffectList)
            {
                effectData._Effect.Active = false;
            }
            this._Position = Vector3.Zero;
            this.Rotation = 0.0f;
            this._color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (EffectData effectData in this._EffectList)
            {
                effectData._Deleted = true;
            }
            this._Position = Vector3.Zero;
            this.Rotation = 0.0f;
            this._color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Add(ITransformEffect effect)
        {
            this.Add(effect, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(ITransformEffect effect, int id)
        {
            EffectData effectData = new EffectData(id, effect);
            this._EffectList.Add(effectData);
        }


        /// <summary>
        /// 
        /// </summary>
        private void DeleteEffect(EffectData effect)
        {
          effect._Deleted = true;
          effect._Effect.Ended = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteEffects(Type type)
        {
            for (int i = 0; i < this._EffectList.Count; i++)
            {
                if (this._EffectList[i]._Effect.GetType() == type)
                {
                  this.DeleteEffect(this._EffectList[i]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteEffects(int id)
        {
            for (int i = 0; i < this._EffectList.Count; i++)
            {
                if (this._EffectList[i]._Id == id)
                {
                  this.DeleteEffect(this._EffectList[i]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteEffectsExcept(int id)
        {
            for (int i = 0; i < this._EffectList.Count; i++)
            {
                if (this._EffectList[i]._Id != id)
                {
                    this.DeleteEffect(this._EffectList[i]);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void DeleteAllEffects()
        {
          for (int i = 0; i < this._EffectList.Count; i++)
          {
            this.DeleteEffect(this._EffectList[i]);
          }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {          
            this._Position = Vector3.Zero;
            this.Rotation = 0.0f;
            this._color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            this._scale = Vector2.Zero;

            this.VirtualPosition = Vector3.Zero;
            this.VirtualRotation = 0.0f;
            if (this._EffectList.Count == 0)
                return;

            if (this.Active == false)
                return;

            for (int i= 0 ; i < this._EffectList.Count; i++)
            {
                EffectData effectData = this._EffectList[i];
                if (!effectData._Deleted && effectData._Effect.Active)
                {
                    effectData._Effect.LastScale = effectData._Effect.Scale;
                    effectData._Effect.InternalUpdate(gameTime);

                    this._Position += effectData._Effect.Position;
                    this.Rotation += effectData._Effect.Rotation;
                    this._color *= effectData._Effect.ColorVector;
                    this._scale += effectData._Effect.Scale - effectData._Effect.LastScale;

                    this.VirtualPosition += effectData._Effect.VirtualPosition;
                    this.VirtualRotation += effectData._Effect.VirtualRotation;
                    if (effectData._Effect.Ended == true)
                    {
                        if (effectData._Effect.AutoDeleteOnEnd)
                        {
                            effectData._Deleted = true;
                        }
                       
                    }
                }
            }

            this.RemoveDeletedEffects();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WithActiveEffects
        {
            get
            {
                for (int i = 0; i < this._EffectList.Count; i++)
                {
                    if (this._EffectList[i]._Deleted == false &&
                        this._EffectList[i]._Effect.Active &&
                        this._EffectList[i]._Effect.Ended == false)
                    {
                        return true;
                    }

                }
                return false;
            }
        }
        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        private void RemoveDeletedEffects()
        {
            for (int i = 0; i < this._EffectList.Count; i++)
            {
                if (this._EffectList[i]._Deleted == true)
                {
                    this._EffectList.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ITransformEffect FindEffect(int id)
        {
            foreach (EffectData effectData in this._EffectList)
            {
                if (effectData._Id == id && effectData._Deleted == false)
                    return effectData._Effect;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(int id)
        {
            return (this.FindEffect(id) != null);
        }
        #endregion
    }
}
