using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIGoldMedalInfoPanel : UIControl
    {
        public override Vector2 Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                base.Scale = value;
            }
        }
        Color _labelColor = new Color(60, 240, 240);
        UIPanel _pnlContainer;
        UIImage _imgInfo;
        UIValuedCaption _capLevel;
        UIValuedCaption _capGold;

        public UIGoldMedalInfoPanel(UIScreen screenOwner) :
            base(screenOwner)
        {
            this._pnlContainer = new UIPanel(screenOwner);
            this._pnlContainer.Size = this.NativeResolution(new BrainEngine.UI.Size(3800f, 600f));
            this.Controls.Add(this._pnlContainer);

            // Information sign
            this._imgInfo = new UIImage(screenOwner, "spriteset/common-elements-1/InformationSign", ResourceManager.RES_MANAGER_ID_STATIC);
            this._imgInfo.Position = this.NativeResolution(new Vector2(0f, 300f));
            this._pnlContainer.Controls.Add(this._imgInfo);

            // Points caption
            this._capLevel = new UIValuedCaption(this.ScreenOwner, "LBL_LEVEL", 0, this._labelColor, Color.White, UICaption.CaptionStyle.GoldMedalInfo, 900, false);
            this._capLevel.Position = this.NativeResolution(new Vector2(300f, 100f));
            this._capLevel.CaptionAlignment = UIValuedCaption.ValueAlignmentMode.Left;
            this._capLevel.CaptionSpacing = 50f;
            this._capLevel.AnimateValue = false;
            this._pnlContainer.Controls.Add(this._capLevel);

            // Time caption
            this._capGold = new UIValuedCaption(this.ScreenOwner, "LBL_GOLD_MEDAL_SCORE", 0, this._labelColor, Color.White, UICaption.CaptionStyle.GoldMedalInfo, 2300, false);
            this._capGold.Position = this.NativeResolution(new Vector2(1100f, 100f));
            this._capGold.CaptionAlignment = UIValuedCaption.ValueAlignmentMode.Left;
            this._capGold.CaptionSpacing = 50f;
            this._capGold.AnimateValue = false;
            this._pnlContainer.Controls.Add(this._capGold);

            this.OnScreenStart += new UIEvent(UIGoldMedalInfoPanel_OnScreenStart);
            this.Size = this._pnlContainer.Size;
            this.BackgroundColor = new Color(0, 0, 0, 160);         
            this.ShowEffect = new ColorEffect(Color.Transparent, Color.White, 0.1f, false);
            this.HideEffect = new ColorEffect(Color.White, Color.Transparent, 0.1f, false);
        }

        /// <summary>
        /// 
        /// </summary>
        void UIGoldMedalInfoPanel_OnScreenStart(IUIControl sender)
        {
            this._capLevel.Value = Stage.CurrentStage.LevelStage.StageNr;
            this._capGold.Value = string.Format("{0}, {1}",
                    Formater.FormatLevelTime(Stage.CurrentStage.LevelStage._goldMedalTime),
                    Formater.FormatLevelScore(Stage.CurrentStage.LevelStage._goldMedalScore));
        }


    }
}
