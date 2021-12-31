using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UICreditsButton : /*UIButton*/ UISnailsButton // temporary UISnailsButton
    {
        public UICreditsButton(UIScreen screenOwner, UIEvent pressCallback) :
            base(screenOwner, "MNU_ITEM_CREDITS", ButtonSizeType.Small, BrainEngine.Input.InputBase.InputActions.None, pressCallback, false)
        {
        }
    }
}
