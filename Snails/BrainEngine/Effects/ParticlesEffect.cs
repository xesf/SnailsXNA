using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Effects
{
    public class ParticlesEffect
    {
        #region Members
        protected int _numParticles;
        protected bool _ended;
        #endregion

        #region Properties
        public int NumParticles
        {
            get { return _numParticles; }
            set { _numParticles = value; }
        }

        public bool Ended
        {
            get { return _ended; }
            set { _ended = value; }
        }
        #endregion

        public ParticlesEffect()
        {
            _numParticles = 0;
        }

        public virtual void Update(BrainGameTime gameTime)
        { }

        public virtual void Draw(SpriteBatch spriteBatch)
        { }
    }
}
