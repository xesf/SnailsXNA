using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIPlayerStat : UIPanel
    {
        public enum PlayerStatType
        {
            PlayingTime,
            RunningTime,
            TotalSnailsSafe,
            TotalSnailsKingSafe,
            TotalSnailsDeadByFire,
            TotalSnailsDeadBySpikes,
            TotalSnailsDeadByDynamite,
            TotalSnailsDeadByCrate,
            TotalSnailsDeadByWater,
            TotalSnailsDeadByLaser,
            TotalSnailsDeadBySacrifice,
            TotalGoldMedals,
            TotalSilverMedals,
            TotalBronzeMedals,
            TotalBoots,
            TotalSnailsDeadByCrateExplosion,
            TotalSnailsDeadByAcid,
            TotalSnailsDeadByOutOfStage,
            TotalSnailsDeadByEvilSnail,
            SnailsDeadInDifferentWays
        }

        public delegate void UIPlayerStatEvent(IUIControl sender, PlayerStatType stat);
        public event UIPlayerStatEvent OnReset;
     
        UIValuedCaption _caption;
        UISnailsButton _btnReset;
        PlayerStatType _statType;

        public object Value
        {
            get
            {
                return this._caption.Value;
            }
            set
            {
                this._caption.Value = value;
            }
        }

        public UIValuedCaption.CaptionMode CaptionFormat
        {
            get { return this._caption.Mode; }
            set { this._caption.Mode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public UIPlayerStat(UIScreen screenOwner, string textResource, Vector2 pos, PlayerStatType statType) :
            base(screenOwner)
        {
            // Caption
            this._caption = new UIValuedCaption(screenOwner, textResource, 0, Color.LightBlue, Color.White, UICaption.CaptionStyle.PlayerStats, 5000f, false);
            this._caption.ParentAlignment = AlignModes.Left | AlignModes.Vertically;
            this._caption.Margins.Left = 200f;
            this._caption.AnimateValue = false;
           
            this.Controls.Add(this._caption);

            // Button
            this._btnReset = new UISnailsButton(screenOwner, "BTN_RESET", UISnailsButton.ButtonSizeType.Small, InputBase.InputActions.None, this.btnReset_OnPress, false);
            this._btnReset.ParentAlignment = AlignModes.Right | AlignModes.Vertically;
            this._btnReset.Scale = this.FromNativeResolution(this._btnReset.Scale);
            this.Controls.Add(this._btnReset);

            this._statType = statType;
            this.OnScreenStart += new UIEvent(UIPlayerStat_OnScreenStart);
            this.Position = pos;
            this.BackgroundColor = new Color(0, 0, 0, 50);
        }

        /// <summary>
        /// 
        /// </summary>
        void UIPlayerStat_OnScreenStart(IUIControl sender)
        {
            this.Size = new Size(this.Parent.Size.Width - 600f, 900f);
        }


         /// <summary>
        /// 
        /// </summary>
        void btnReset_OnPress(IUIControl sender)
        {
            if (this.OnReset != null)
            {
                this.OnReset(this, this._statType);
            }
        }
    }
}
