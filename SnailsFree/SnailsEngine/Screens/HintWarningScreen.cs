using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI.Controls;
using System.Collections.Generic;
using TwoBrainsGames.Snails.Tutorials;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Localization;
using System;

namespace TwoBrainsGames.Snails.Screens
{
    class HintWarningScreen : SnailsScreen
    {
        UISnailsWindow _window;
        UICaption _capWarning;
        UICaption _capWarning2;
        UIImage _imgSnailIcon;

        public HintWarningScreen(ScreenNavigator owner) :
            base(owner, ScreenType.HintWarning)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.BackgroundImage = null;
            this.WithBlurEffect = true;


            // Window
            this._window = new UISnailsWindow(this);
            this._window.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._window.ButtonCaptionType = UISnailsWindow.ButtonType.YesNo;
            this._window.CloseButtonVisible = false;
            this._window.OnNo += new UIControl.UIEvent(_window_OnNo);
            this._window.OnYes += new UIControl.UIEvent(_window_OnYes);
            this._window.OnDismiss += new UIControl.UIEvent(_window_OnDismiss);
            this._window.WithTitle = true;
            this.Controls.Add(this._window);

            // Warning text
            this._capWarning = new UICaption(this, "", Color.Orange, UICaption.CaptionStyle.NormalText);
            this._capWarning.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._capWarning.Position = this.NativeResolution(new Vector2(0f, 500f));
            this._capWarning.HorizontalAligment = BrainEngine.UI.HorizontalTextAligment.Center;
            this._capWarning.LineSpacing = this.NativeResolutionY(150f);
            this._window.Board.Controls.Add(this._capWarning);

            // Icon
            this._imgSnailIcon = new UIImage(this, "spriteset/common-elements-1/HintIcons");
            this._imgSnailIcon.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._imgSnailIcon.Animate = false;
            this._imgSnailIcon.Position = this.NativeResolution(new Vector2(0f, 1500f));
            this._window.Board.Controls.Add(this._imgSnailIcon);

            // Warning text
            this._capWarning2 = new UICaption(this, "", Color.Orange, UICaption.CaptionStyle.NormalText);
            this._capWarning2.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._capWarning2.Position = this.NativeResolution(new Vector2(0f, 2600f));
            this._capWarning2.TextResourceId = "LBL_HINT_WARNING2";
            this._capWarning2.HorizontalAligment = BrainEngine.UI.HorizontalTextAligment.Center;
            this._window.Board.Controls.Add(this._capWarning2);

        }



        /// <summary>
        /// 
        /// </summary>
        void _window_OnYes(IUIControl sender)
        {
            Stage.CurrentStage.StartIspectingHints();
            
            this._window.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        void _window_OnNo(IUIControl sender)
        {
            this._window.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        void _window_OnDismiss(IUIControl sender)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this._window.TitleText = string.Format(LanguageManager.GetString("LBL_HINT_NUMBER"), (Stage.CurrentStage.HintManager.CurrentHintIndex + 1)); ;
            this._window.Show();
            this.AcceptControllerInput = true;
            this._imgSnailIcon.CurrentFrame = Stage.CurrentStage.HintManager.CurrentHintIndex;
            switch (Stage.CurrentStage.HintManager.CurrentHintIndex)
            {
                case 0: 
                    this._capWarning.Text = LanguageManager.GetString("LBL_HINT_WARNING_HINT1");
                    break;
                case 1:
                    this._capWarning.Text = LanguageManager.GetString("LBL_HINT_WARNING_HINT2");
                    break;
                default:
                    this._capWarning.Text = LanguageManager.GetString("LBL_HINT_WARNING_HINT3");
                    break;
            }
        }

        public override void OnUpdate(BrainGameTime gameTime)
        {
            base.OnUpdate(gameTime);
        }
    }
}
