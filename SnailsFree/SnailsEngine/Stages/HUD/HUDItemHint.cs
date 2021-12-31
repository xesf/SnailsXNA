using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Screens;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    class HUDItemHint : HUDItem
    {
		const float BOTTOM_MARGIN = 10f;
        public const int MAX_FAIL_COUNT_CALL_ATTENTION = 4;

        Sprite _sprite;
        string _text;
        Vector2 _textPosition;
        ColorEffect _colorEffect;

        public bool CallPlayerAttention { get; set; }

        public bool Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                this._visible = value;
                if (this._visible)
                {
                    this.UpdateCaption();
                }
            }
        }
        int ButtonWitdh
        {
            get
            {
                return this._sprite.Frames[0].Width;
            }
        }

        int ButtonHeight
        {
            get
            {
                return this._sprite.Frames[0].Height;
            }
        }

        public HUDItemHint()
        {
            this.Selectable = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize(BoundingSquare toolsArea)
        {

			this.Position = new Vector2(toolsArea.Left + ((toolsArea.Width / 2) - (this.ButtonWitdh / 2)), toolsArea.Bottom - this.Size.Y - BOTTOM_MARGIN);
            this._colorEffect = new ColorEffect(Color.Yellow, Color.Gray, 0.05f, true);
            this._colorEffect.UseRealTime = true;
            this.CallPlayerAttention = (SnailsGame.ProfilesManager.CurrentProfile.StagesFailedCount >= MAX_FAIL_COUNT_CALL_ATTENTION);
            this.UpdateCaption();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "HintButton");
            this.Size = new Vector2(this._sprite.Frames[0].Rect.Width, this._sprite.Frames[0].Rect.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime brainTime)
        {
            
            base.Update(brainTime);

            if (this.CallPlayerAttention)
            {
                this._colorEffect.Update(brainTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(this.Position, spriteBatch);
            this._font.DrawString(spriteBatch, this._text, this._textPosition, Vector2.One, this._colorEffect.Color);

#if DEBUG
            if (SnailsGame.Settings.ShowBoundingBoxes)
            {
                this.SelectableArea.Draw(Color.White, Vector2.Zero);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnSelect()
        {
            if (Stage.CurrentStage.State == Stage.StageState.Playing)
            {
                Player.PlayerStageStats playerStats = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(Stage.CurrentStage.LevelStage.StageId);
                if (playerStats == null)
                {
                    return;
                }

                // Mostra quadro a avisar da penalização se nunca terminou o nível 
                // e se a hint que vai ver é inferior à última que já viu
                if (Stage.CurrentStage.HintManager.CurrentHintIndex + 1 > playerStats.HintsTaken &&
                    playerStats.Completed == false)
                {
                    SnailsGame.ScreenNavigator.PopUp(ScreenType.HintWarning.ToString());
                }
                else
                {
                    Stage.CurrentStage.StartIspectingHints();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCaption()
        {
            this._text = string.Format(LanguageManager.GetString("LBL_HINT"), Stage.CurrentStage.HintManager.CurrentHintIndex + 1);
            this._textPosition = this.Position + new Vector2((this.ButtonWitdh / 2) - (this._font.MeasureString(this._text) / 2),
                                                             (this.ButtonHeight / 2) - (this._font.MeasureStringHeight(this._text, Vector2.One) / 2));
        }

    }
}
