using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;

namespace TwoBrainsGames.Snails.Screens
{
    class XBoxHelpScreen : SnailsScreen
    {
        UIXBoxControls _xboxController;

        public XBoxHelpScreen(ScreenNavigator owner) :
            base(owner, ScreenType.XBoxControllerHelp)
        {
            this.BackgroundColor = new Color(0, 0, 0, 200);

            this._xboxController = new UIXBoxControls(this);
            this._xboxController.Position = new Vector2(0f, 3500f);
            this._xboxController.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._xboxController.OnDismiss += new BrainEngine.UI.Controls.UIControl.UIEvent(_xboxController_OnDismiss);
            this._xboxController.OnShow += new BrainEngine.UI.Controls.UIControl.UIEvent(_xboxController_OnShow);
            this.Controls.Add(this._xboxController);
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
            base.OnStart();
            this.DisableInput();
            this._xboxController.Visible = false;
            this._xboxController.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        void _xboxController_OnShow(IUIControl sender)
        {
            this.EnableInput();
            this._xboxController.Focus();
        }
      

        /// <summary>
        /// 
        /// </summary>
        void _xboxController_OnDismiss(IUIControl sender)
        {
            this.Close();
        }
    }
}
