using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    /// <summary>
    /// This is a panel used in the main menu screen (and options screen) to hold the menus
    /// I decided to create a control because it will be reused in main menu and options
    /// This will contain the menus, main objective is to have the menus centered on this control
    /// This way it will be platform independent
    /// </summary>
    class UIMainMenuBodyPanel : UIPanel
    {
        /// <summary>
        /// 
        /// </summary>
        public UIMainMenuBodyPanel(UIScreen screenOwner) :
            base(screenOwner)
        {
            this.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this.Size = new Size(UIScreen.MAX_SCREEN_WITDH_IN_POINTS, new Vector2(0f, 7000f).Y);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitializeFromContent()
        {
            base.InitializeFromContent();
            this.InitializeFromContent("UIMainMenuBodyPanel");
        }
    }
}
