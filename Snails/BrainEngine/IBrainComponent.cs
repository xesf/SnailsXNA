using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine
{
    public interface IBrainComponent
    {
        SpriteBatch SpriteBatch { get; }

        void Initialize();
        void LoadContent();
        void Update(BrainGameTime gameTime);
        void Draw();
        void UnloadContent();
    }
}
