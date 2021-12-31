using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    class HUDItemGoal : HUDItem
    {
        Sprite _sprite;
        Vector2 _spritePosition;

        public HUDItemGoal()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize(Microsoft.Xna.Framework.Vector2 position)
        {
            base.Initialize(position);
            this._spritePosition = position + new Vector2(0, 5f);
            this._width = 50f;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "GoalIcons");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(this._spritePosition, (int)Stage.CurrentStage.LevelStage._goal, spriteBatch);
        }
    }
}
