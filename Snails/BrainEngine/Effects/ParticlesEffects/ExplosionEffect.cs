using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Effects.ParticlesEffects
{
    public class ExplosionEffectEntity
    {
        // particles display
        public Vector2 Position;
        public Sprite Sprite;
        public int SpriteFrame;
        public TransformBlender _EffectsBlender;
        public float Rotation;
    }

    public class ExplosionEffect : ParticlesEffect
    {
        #region Constants
        public const int DEFAULT_EXPLOSION_PARTICLES = 6;
        public const float DEFAULT_MIN_SPEED = 15;
        public const float DEFAULT_MAX_SPEED = 20;
        private const int EXPLOSION_TIME = 10000;
        #endregion

        #region Members
        private float _x0;
        private float _y0;
        private float _minSpeed;
        private float _maxSpeed;
        private float t;
        private List<ExplosionEffectEntity> _particles;
        private int _explosionTime;
        private Sprite _fragSprite;
        private float _layerDepth;
        private Color _color;
        private int _minAngle;
        private int _maxAngle;
        #endregion

        #region Properties
        Sprite FragmentsSprite { get; set; }
        public float MinSpeed
        {
            get { return this._minSpeed; }
            set { this._minSpeed = value; }
        }
        public float MaxSpeed
        {
            get { return this._maxSpeed; }
            set { this._maxSpeed = value; }
        }
        public int MaxAngle
        {
            get { return this._maxAngle; }
            set { this._maxAngle = value; }
        }
        public int MinAngle
        {
            get { return this._minAngle; }
            set { this._minAngle = value; }
        }
        public Color Color
        {
            get { return this._color; }
            set { this._color = value; }
        }
         
        #endregion

        public ExplosionEffect(float x, float y, Sprite fragSprite, float layerDepth, float gravityForce)
        {
            _color = Color.White;
            _x0 = x;
            _y0 = y;
            _fragSprite = fragSprite;
            _maxSpeed = BrainGame.Settings.ExplosionMaxVelocity;
            _minSpeed = BrainGame.Settings.ExplosionMinVelocity;
            t = 0; // initial time
            _numParticles = BrainGame.Settings.ExplosionParticles;
            _explosionTime = EXPLOSION_TIME;
            _ended = false;
            _minAngle = 0;
            _maxAngle = 356;

            _layerDepth = layerDepth;
            this.FragmentsSprite = fragSprite;
            ComputeParticles(gravityForce);
        }

        public void ComputeParticles(float gravityForce)
        {
            _particles = new List<ExplosionEffectEntity>(_numParticles);

            for (int p = 0; p < _numParticles; p++)
            {
                ExplosionEffectEntity exp = new ExplosionEffectEntity();
                exp.Sprite = _fragSprite;
                //FIXME: we should have a proper solution to set the right fragments
                exp.SpriteFrame = BrainGame.Rand.Next(0, this.FragmentsSprite.FrameCount - 1); // tiles fragments frames
                float speed = BrainGame.Rand.Next((int)_minSpeed * 100, (int)_maxSpeed * 100) / 100f;
                exp._EffectsBlender = new TransformBlender();

                float angle = BrainGame.Rand.Next(_minAngle, _maxAngle);
                Vector2 direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(angle)), 
                                                -(float)Math.Sin(MathHelper.ToRadians(angle)));
                exp._EffectsBlender.Add(new MotionEffect(gravityForce, direction * speed));
                exp._EffectsBlender.Add(new RotationEffect(BrainGame.Rand.Next(-15, 15)));
                exp.Position = new Vector2(this._x0, this._y0);
                _particles.Add(exp);
            }
        }

        public override void Update(BrainGameTime gameTime)
        {
            if (_explosionTime <= 0)
            {
                _ended = true;
                return;
            }

            foreach(ExplosionEffectEntity exp in this._particles)
            {
                exp._EffectsBlender.Update(gameTime);
                exp.Position += exp._EffectsBlender.PositionV2;
                exp.Rotation += exp._EffectsBlender.Rotation;
            }

            t += (gameTime.ElapsedGameTime.Milliseconds / 1000.0f); // time in seconds due to equation

            _explosionTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_ended)
                return;

            foreach(ExplosionEffectEntity exp in this._particles)
            {
                exp.Sprite.Draw(exp.Position, exp.SpriteFrame, exp.Rotation, SpriteEffects.None, _layerDepth, this._color, 1f, spriteBatch);
            }
        }
    }
}
