using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Screens.Transitions;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens
{
    class PurchaseScreen : SnailsScreen
    {
        private Color GetGameColor = new Color(255, 136, 25);
        private Color FeaturesColor = new Color(240, 255, 25);

        protected UISnailsButton _btnPurchase;
        protected UISnailsButton _btnMainMenu;

        public PurchaseScreen(ScreenNavigator owner) :
            base(owner, ScreenType.Purchase)
        {
        }

        UIImage _imgScreen1;
        UIImage _imgScreen2;
        UIImage _imgScreen3;
        UIImage _imgTitle;

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.BackgroundType = ScreenBackgroundType.Leafs;

            // Screen capture 1
            this._imgScreen1 = new UIImage(this, "spriteset/purchase/Photo1", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgScreen1.Rotation = -8f;
            this._imgScreen1.Position = new Vector2(2300f, 2500f);
            this.Controls.Add(this._imgScreen1);

            // Screen capture 2
            this._imgScreen2 = new UIImage(this, "spriteset/purchase/Photo2", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgScreen2.Rotation = 10f;
            this._imgScreen2.Position = new Vector2(2900f, 6600f);
            this.Controls.Add(this._imgScreen2);

            // Screen capture 3
            this._imgScreen3 = new UIImage(this, "spriteset/purchase/Photo3", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgScreen3.Rotation = 3f;
            this._imgScreen3.Position = new Vector2(4400f, 4200f);
            this.Controls.Add(this._imgScreen3);

            UIPanel panel = new UIPanel(this);
            panel.Position = new Vector2(4700f, 2000f);
            panel.Size = new Size(4800f, 7000f);
            panel.BackgroundColor = new Color(0, 0, 0, 90);
            this.Controls.Add(panel);

            // Snails Title
            this._imgTitle = new UIImage(this, "spriteset/common-elements-1/SnailsLogTitle", ResourceManager.RES_MANAGER_ID_STATIC);
            this._imgTitle.Position = new Vector2(0f, -1500f);
            this._imgTitle.ParentAlignment = AlignModes.Horizontaly;
            this._imgTitle.Scale = new Vector2(0.7f, 0.7f);
            panel.Controls.Add(this._imgTitle);

            // Get the full version
            UICaption caption = new UICaption(this, "", GetGameColor, UICaption.CaptionStyle.Heading1);
            caption.TextResourceId = "LBL_GET_FULLVERSION";
            caption.Position = new Vector2(150f, 950f);
            caption.Scale = new Vector2(0.7f, 0.7f);
            panel.Controls.Add(caption);

            // 4 themes
            caption = new UICaption(this, "", FeaturesColor, UICaption.CaptionStyle.NormalText);
            caption.TextResourceId = "LBL_4_THEMES";
            caption.Position = new Vector2(600f, 2200f);
            panel.Controls.Add(caption);

            // 84 levels
            caption = new UICaption(this, "", FeaturesColor, UICaption.CaptionStyle.NormalText);
            caption.TextResourceId = "LBL_84_STAGES";
            caption.Position = new Vector2(600f, 2700f);
            panel.Controls.Add(caption);

            // 4 game modes
            caption = new UICaption(this, "", FeaturesColor, UICaption.CaptionStyle.NormalText);
            caption.TextResourceId = "LBL_4_MODES";
            caption.Position = new Vector2(600f, 3200f);
            panel.Controls.Add(caption);

            // New tools
            caption = new UICaption(this, "", FeaturesColor, UICaption.CaptionStyle.NormalText);
            caption.TextResourceId = "LBL_NEW_TOOLS";
            caption.Position = new Vector2(600f, 3700f);
            panel.Controls.Add(caption);

            // Lot of fun
            caption = new UICaption(this, "", FeaturesColor, UICaption.CaptionStyle.NormalText);
            caption.TextResourceId = "LBL_LOT_OF_FUN";
            caption.Position = new Vector2(600f, 4200f);
            panel.Controls.Add(caption);

            // Purchase button
            this._btnPurchase = new UISnailsButton(this, "BTN_PURCHASE", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, this.btnPurchase_OnClick, true);
            this._btnPurchase.Name = "_btnPurchase";
            this._btnPurchase.Position = new Vector2(300f, 5000f);
            this._btnPurchase.Visible = false;
            this._btnPurchase.OnShow += new UIControl.UIEvent(_btnPurchase_OnShow);
            panel.Controls.Add(this._btnPurchase);

            // Button Main menu
            this._btnMainMenu = new UISnailsButton(this, "BTN_MAIN_MENU", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, this.btnMainMenu_OnClick, false);
            this._btnMainMenu.Name = "_btnMainMenu";
            this._btnMainMenu.Position = new Vector2(2700f, 5000f);
            this._btnMainMenu.Visible = false;
            this._btnMainMenu.OnAcceptBegin += new UIControl.UIEvent(_btnMainMenu_OnAcceptBegin);
            panel.Controls.Add(this._btnMainMenu);


            // Support us
            caption = new UICaption(this, "", new Color(0, 250, 55), UICaption.CaptionStyle.NormalText);
            caption.TextResourceId = "LBL_SUPPORT_US";
            caption.ParentAlignment = AlignModes.Bottom | AlignModes.Horizontaly;
            caption.Margins.Bottom = 300f;
            this.Controls.Add(caption);

        }

        void _btnPurchase_OnShow(IUIControl sender)
        {
            this.EnableInput();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this.DisableInput();
            this._btnPurchase.Show();
            this._btnMainMenu.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainEngine.BrainGameTime gameTime)
        {
 	         base.OnUpdate(gameTime);

             if (!SnailsGame.IsTrial) // This may change. Because the market is opening is async
             {
                 this.NavigateToMain();
             }
        }

        /// <summary>
        /// 
        /// </summary>
        void btnMainMenu_OnClick(IUIControl sender)
        {
            this.NavigateToMain();
        }
        
        /// <summary>
        /// 
        /// </summary>
        void btnPurchase_OnClick(IUIControl sender)
        {
            SnailsGame.Instance.PurchaseGame();
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnMainMenu_OnAcceptBegin(IUIControl sender)
        {
            this.DisableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        void NavigateToMain()
        {
            this.DisableInput();
            this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleAndMenuVisible);
            this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), ScreenTransitions.LeafsClosed, ScreenTransitions.LeafsOpening);
        }
    }
}
