using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    class HUDItemSnailsDelivered : HUDItem
    {
        Sprite _sprite;
        Vector2 _spritePosition;
        Vector2 _stringPosition;
        string _deliveredString;
        int _frameNr;

        public HUDItemSnailsDelivered()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            this._spritePosition = position + new Vector2(0f, 5f); // Pull a little bit down
            this._stringPosition = position + new Vector2(50f, 10f);
            this.UpdateString();
            this._width = 120f;
            this._frameNr = (int)Stage.CurrentStage.LevelStage.ThemeId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "DeliveredIcon");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this._sprite.Draw(this._spritePosition, this._frameNr, spriteBatch);
            this._font.DrawString(spriteBatch, this._deliveredString, this._stringPosition, new Vector2(1.0f, 1.0f), Colors.StageHUDInfoColor);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateString()
        {
            this._deliveredString = string.Format("{0}/{1}", Stage.CurrentStage.Stats.NumSnailsSafe, Stage.CurrentStage.Stats.NumSnailsToSave);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SnailsStageStatsChanged()
        {
            this.UpdateString();
        }
    }
}
