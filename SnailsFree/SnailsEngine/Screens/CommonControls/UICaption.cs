using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    public class UICaption : UITextFontLabel
    {

        public enum CaptionStyle
        {
            AutoSaveMessage,
            OverscanMessage,
            NormalText,
            NormalTextSmall,
            Heading1,
            CreditsCategory,
            CreditsName,
            StageSelectionStageNr,
            NormalTextMedium,
            ThemeUnlockInfo,
            ThemeUnlockInfoTitle,
            Notebook,
            StageStartBoardCaptions,
            StageCompletedTitle,
            StageCompletedCaptions,
            MissionFailedCaptions,
            ThemeStats,
            StageInfoHeader,
            StageInfoDetail,
            IntroPressAnyKey,
            ControllerHelp,
            GoldMedalInfo,
            StageId, 
            AwardCaption,
            PlayerStats,
            NormalTextBig,
            LoadingTip
        }

        private TextFont NotebookFont;
        private TextFont NotebookFontMedium;
        //private TextFont NotebookFontBig;
        private TextFont MainFontMedium;
        private TextFont MainFontBig;
        private TextFont MainFontSmall;
        private TextFont MainFontMediumSmall;
        
        private CaptionStyle _style;

        private CaptionStyle Style 
        {
            get { return this._style; }
            set
            {
                this.Scale = new Vector2(1f, 1f);
                this._style = value;
                this.DropShadow = false;
                switch (this._style)
                {
                    case CaptionStyle.StageCompletedTitle:
                        this.Font = this.MainFontMedium;
                        break;

                    case CaptionStyle.AutoSaveMessage:
                    case CaptionStyle.OverscanMessage:
                        this.Font = this.MainFontMedium;
                        break;
                        
                    case CaptionStyle.NormalText:
                    case CaptionStyle.NormalTextMedium:
                    case CaptionStyle.ThemeUnlockInfoTitle:
                        this.Font = this.MainFontMedium;
                        break;
                    case CaptionStyle.AwardCaption:
                    case CaptionStyle.PlayerStats:
                       if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LD)
                        {
                            this.Font = this.MainFontSmall;
                        }
                        else
                        {
                            this.Font = this.MainFontMedium;
                        }
                        break;
                    case CaptionStyle.LoadingTip:
                        this.Font = this.MainFontMedium;
                        break;
                    case CaptionStyle.ControllerHelp:
                    case CaptionStyle.NormalTextSmall:
                    case CaptionStyle.GoldMedalInfo:
                    case CaptionStyle.CreditsCategory:
                    case CaptionStyle.CreditsName:
                    case CaptionStyle.StageId:
                        this.Font = this.MainFontSmall;
                        break;
                    case CaptionStyle.IntroPressAnyKey:
                    case CaptionStyle.Heading1:
                    case CaptionStyle.StageSelectionStageNr:
                    case CaptionStyle.NormalTextBig:
                        this.Font = this.MainFontBig;
                        break;
                    case CaptionStyle.ThemeUnlockInfo:
                        this.Font = this.MainFontMediumSmall;
                        break;
                    case CaptionStyle.Notebook:
                        this.Font = this.NotebookFont;
                        break;
                    case CaptionStyle.StageInfoDetail:
                        this.Font = this.NotebookFont;
                        break;
                    case CaptionStyle.StageInfoHeader:
                        this.Font = this.NotebookFontMedium;
                        break;
                        
                    case CaptionStyle.StageStartBoardCaptions:
                    case CaptionStyle.StageCompletedCaptions:
                    case CaptionStyle.MissionFailedCaptions:
                    case CaptionStyle.ThemeStats:
                        this.Font = this.MainFontMedium;
                        break;
                    default:
                        this.Font = this.MainFontMedium;
                        break;
                }
                this.CalculateSize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UICaption(UIScreen screenOwner, string text, Color color, CaptionStyle style) :
            base(screenOwner)
        {
            this.Text = text;
            // What the heck is this?! Why load all fonts?! The Caption only needs 1!! Improve this later
            this.NotebookFont = BrainGame.ResourceManager.Load<TextFont>("fonts/notebook", ResourceManager.ResourceManagerCacheType.Static);
            this.NotebookFontMedium = BrainGame.ResourceManager.Load<TextFont>("fonts/notebook-medium", ResourceManager.ResourceManagerCacheType.Static);
            this.MainFontBig = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-big", ResourceManager.ResourceManagerCacheType.Static);
            this.MainFontMedium = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
            this.MainFontSmall = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-small", ResourceManager.ResourceManagerCacheType.Static);
            this.MainFontMediumSmall = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium-2", ResourceManager.ResourceManagerCacheType.Static);
            this.BlendColor = color;
            this.Style = style;
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 MeasureString()
        {
            return new Vector2(this.Font.MeasureString(this.Text, this.Scale),
                               this.Font.MeasureStringHeight(this.Text, this.Scale));
        }
    }
}
