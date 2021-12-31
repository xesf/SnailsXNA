using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.Stages.Lighting
{
    class SpotlightLightSource : LightSource
    {
        private Sprite _godRaysSprite;

        public SpotlightLightSource()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override LightSource Clone()
        {
            SpotlightLightSource clone = new SpotlightLightSource();
            clone.Copy(this);
            return clone;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._godRaysSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/light-tints/GodRays");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DrawTint(SpriteBatch spriteBatch)
        {
            base.DrawTint(spriteBatch);
            this._godRaysSprite.Draw(this.Position + Stage.CurrentStage.Camera.TransformedPosition, 0, 45f + this.Rotation, SpriteEffects.None, spriteBatch);
            this._godRaysSprite.Draw(this.Position + Stage.CurrentStage.Camera.TransformedPosition, 0, -45f + this.Rotation, SpriteEffects.FlipHorizontally, spriteBatch);
        }

    }
}
