using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.BrainEngine.Graphics
{
    public class Camera2D : ICamera2D
    {
        #region Members
        //protected float _viewportHeight;
        //protected float _viewportWidth;
        private TransformBlender _EffectsBlender;
        #endregion

        #region Properties
        public Vector2 Position { get; set; }
        Vector2 PositionOffset { get; set; }
        Vector2 CenterPinchOffset { get; set; }
        public float Rotation { get; set; }
        public float RotationOffset { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public Matrix Transform { get; set; }
        public float MoveSpeed { get; set; }

        public Vector2 TransformedPosition { get; set; }
    
        public TransformBlender EffectsBlender
        {
            get { return this._EffectsBlender; }
        }
        #endregion
        
        public Camera2D()
        { }      

        /// <summary>
        /// Called when the GameComponent needs to be initialized. 
        /// </summary>
        public virtual void Initialize()
        {
            this.Origin = new Vector2(BrainGame.ScreenWidth / 2, BrainGame.ScreenHeight / 2);
            this.Scale = Vector2.One;
            this._EffectsBlender = new TransformBlender();
            UpdateTransform();
        }


        public void UpdateTransform()
        {
            // Remove half pixels
            Vector2 pos = new Vector2((int)(-this.Position.X - this.PositionOffset.X), (int)(-this.Position.Y -this.PositionOffset.Y));
            // Create the Transform used by any
            // spritebatch process
            Transform = Matrix.CreateTranslation(pos.X, pos.Y, 0) *
                        Matrix.CreateRotationZ(Rotation + this.RotationOffset) *
                        Matrix.CreateScale(new Vector3(Scale.X, Scale.Y, 0f)) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0);

            this.TransformedPosition = pos + Origin;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update(BrainGameTime gameTime)
        {
            this._EffectsBlender.Update(gameTime);
            this.Position += this._EffectsBlender.PositionV2;
            this.Rotation += this._EffectsBlender.Rotation;
            this.PositionOffset = this._EffectsBlender.VirtualPositionV2;
            this.RotationOffset = this._EffectsBlender.VirtualRotation;
            this.Scale += this._EffectsBlender._scale;

            UpdateTransform();

            //this.UpdateEffect(gameTime);
        }


        /// <summary>   
        /// 
        /// </summary>
        public virtual void Zoom(float scale)
        {
            this.Scale = new Vector2(this.Scale.X * scale, this.Scale.Y * scale);
        }
    }
}
