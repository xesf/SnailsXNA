using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Stages
{
    public class LightSource : IDataFileSerializable
    {
        public enum LightSourceType
        {
            Baselight,
            Spotlight
        }

        public enum LightState
        {
            On,
            Off
        }

        private LightSourceType Type { get; set; }
        public const float FullPower = 1f;
        private Color _lightMapColor = Color.White;

        public string Id { get; private set; }
        private string MaskSpriteResourceName { get; set; }
        private string TintSpriteResourceName { get; set; }
        public Sprite MaskSprite { get; set; }
        private Sprite TintSprite { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }
        public Color ColorWithPower { get { return new Color(this.Color.ToVector4() * this._power); } }
        public Vector2 Position { get; set; }
        public LightState State { get; private set; }
        public bool IsOn { get { return this.State == LightState.On; } }
        public bool IsOff { get { return this.State == LightState.Off; } }
        public bool WithTint { get { return this.TintSprite != null; } }
        public TransformBlender EffectsBlender { get; private set;}
        private float _power;
        public float Power 
        {
            get { return this._power; }
            set 
            {
                this._power = value;
            }
        } // Controls the power of the light (this is the alpha channel for the light map)
        
        /// <summary>
        /// 
        /// </summary>
        public LightSource()
        {
            this.Scale = new Vector2(1f, 1f);
            this.Rotation = 0f;
            this.Color = Color.White;
            this.State = LightState.On;
            this.Power = LightSource.FullPower;
            this.EffectsBlender = new TransformBlender();
        }

        /// <summary>
        /// 
        /// </summary>
        public static LightSource Create(LightSourceType type)
        {
            switch (type)
            {
                case LightSourceType.Baselight: 
                    return new LightSource();
            }
            throw new BrainException("Invalid light source type [" + type.ToString() + "]");
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual LightSource Clone()
        {
            LightSource clone = new LightSource();
            clone.Copy(this);
            return clone;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Copy(LightSource from)
        {
            this.MaskSprite = from.MaskSprite;
            this.MaskSpriteResourceName = from.MaskSpriteResourceName;
            this.TintSpriteResourceName = from.TintSpriteResourceName;
            this.Color = from.Color;
            this.Id = from.Id;
            this.Position = from.Position;
            this.Rotation = from.Rotation;
            this.Scale = from.Scale;
            this.State = from.State;
            this.Power = from.Power;
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadContent()
        {
            this.MaskSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.MaskSpriteResourceName);
            if (!string.IsNullOrEmpty(this.TintSpriteResourceName))
            {
                this.TintSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.TintSpriteResourceName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update(BrainGameTime gameTime)
        {
            if (this.EffectsBlender.Count > 0)
            {
                this.UpdateEffects(gameTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void UpdateEffects(BrainGameTime gameTime)
        {
            this.EffectsBlender.Update(gameTime);
            this.Position += this.EffectsBlender.PositionV2;
            this.Rotation += this.EffectsBlender.Rotation;
            this.Scale += this.EffectsBlender._scale;
            this.Color = this.EffectsBlender.Color;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.MaskSprite.Draw(this.Position , 0, this.Rotation, Vector2.Zero, this.Scale.X, this.Scale.Y, this._lightMapColor, spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void DrawTint(SpriteBatch spriteBatch)
        {
            if (this.TintSprite != null)
            {
                this.TintSprite.Draw(this.Position, 0, this.Rotation, Vector2.Zero, this.Scale.X, this.Scale.Y, this.ColorWithPower, spriteBatch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOn()
        {
            this.State = LightState.On;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOff()
        {
            this.State = LightState.Off;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetState(LightState state)
        {
            if (state == LightState.On)
            {
                this.SwitchOn();
            }
            else
            {
                this.SwitchOff();
            }

        }


        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this.Type = (LightSource.LightSourceType)Enum.Parse(typeof(LightSource.LightSourceType), record.GetFieldValue<string>("type", LightSource.LightSourceType.Baselight.ToString()), false);
            this.Id = record.GetFieldValue<string>("id");
            this.MaskSpriteResourceName = record.GetFieldValue<string>("maskSprite");
            this.TintSpriteResourceName = record.GetFieldValue<string>("tintSprite");
            this.Color = record.GetFieldValue<Color>("color");
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("LightSource");
            record.AddField("type", this.Type.ToString());
            record.AddField("id", this.Id);
            record.AddField("maskSprite", this.MaskSpriteResourceName);
            record.AddField("tintSprite", this.TintSpriteResourceName);
            record.AddField("color", this.Color);
            return record;
        }
        #endregion
    }
}
