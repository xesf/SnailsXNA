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

			// Utilizar screen units aqui
			this.Position = new Vector2(0, 3500);
			if (SnailsGame.GameSettings.PresentationMode != Configuration.GameSettings.PresentationType.HD)
			{
				this.Position = new Vector2(0, 2700);
			}
			this.Size = new Size(UIScreen.MAX_SCREEN_WITDH_IN_POINTS, this.PixelsToScreenUnitsY(400));
		}
	}
}
