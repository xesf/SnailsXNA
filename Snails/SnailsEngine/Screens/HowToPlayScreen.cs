using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI.Controls;
using System.Collections.Generic;
using TwoBrainsGames.Snails.Tutorials;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens
{
    class HowToPlayScreen : SnailsScreen
    {
        UIHowToPlayWindow _howToPlayWindow;

        public HowToPlayScreen(ScreenNavigator owner) :
            base(owner, ScreenType.HowToPlay)
        {
            this.Name = "HowToPlay";
            this.BackgroundColor = new Color(0, 0, 0, 200);
            // How to play (tutorial topics)
            this._howToPlayWindow = new UIHowToPlayWindow(this);
            this._howToPlayWindow.Name = "_howToPlayWindow";
            this._howToPlayWindow.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._howToPlayWindow.OnDismiss += new UIControl.UIEvent(_howToPlayWindow_OnDismiss);
            this._howToPlayWindow.OnShow += new UIControl.UIEvent(_howToPlayWindow_OnShow);
            this._howToPlayWindow.OnDismissPressed += new UIControl.UIEvent(_howToPlayWindow_OnDismissPressed);
            this._howToPlayWindow.UseCloseHotKey = true;
            this.Controls.Add(this._howToPlayWindow);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
            BrainGame.ResourceManager.Unload(ResourceManagerIds.TUTORIAL_RESOURCES);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.BackgroundImage = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            this.WithBlurEffect = this.Navigator.GlobalCache.Get<bool>("SHOW_BLUR", false);
            base.OnStart();
            this._howToPlayWindow.Topics = SnailsGame.ScreenNavigator.GlobalCache.Get<List<TutorialTopic>>(GlobalCacheKeys.TUTORIAL_TOPIC_LIST, new List<TutorialTopic>());

            this.DisableInput();
            this._howToPlayWindow.Visible = false;
            this._howToPlayWindow.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _howToPlayWindow_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this._howToPlayWindow.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        void _howToPlayWindow_OnDismiss(IUIControl sender)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        void _howToPlayWindow_OnDismissPressed(IUIControl sender)
        {
            this._howToPlayWindow.Close();
        }

        public static void PopUp(List<TutorialTopic> topics)
        {
            PopUp(topics, false);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PopUp(List<TutorialTopic> topics, bool showBlur)
        {
            SnailsGame.ScreenNavigator.GlobalCache.Set("SHOW_BLUR", showBlur);
            SnailsGame.ScreenNavigator.GlobalCache.Set(GlobalCacheKeys.TUTORIAL_TOPIC_LIST, topics);
            SnailsGame.ScreenNavigator.PopUp(ScreenType.HowToPlay.ToString());
        }
    }
}
