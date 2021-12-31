using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
namespace TwoBrainsGames.Snails.Stages.HUD
{
    class HUDItemSnailsCounter : HUDItem
    {
        Sprite _sprite;
        Vector2 _spritePosition;
        Vector2 _stringPosition;
        string _text;

        public HUDItemSnailsCounter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            this._spritePosition = position + new Vector2(0f, 10f); // Pull a little bit down
            this._stringPosition = position + new Vector2(45f, 10f);
            this.UpdateText();
            this._width = 85f;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "SnailIcon");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this._sprite.Draw(this._spritePosition, spriteBatch);
            this._font.DrawString(spriteBatch, this._text, this._stringPosition, new Vector2(1.0f, 1.0f), Colors.StageHUDInfoColor);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateText()
        {
            this._text = Stage.CurrentStage.Stats.TotalSnails.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SnailsStageStatsChanged()
        {
            this.UpdateText();
        }
    }
}
