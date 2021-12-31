using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine
{
    public interface IHddIndicator : IBrainComponent
    {
        bool Visible { get; set; }
        void Draw(SpriteBatch spriteBatch);
        void Draw(SpriteBatch spriteBatch, Vector2 position);
        
    }
}
