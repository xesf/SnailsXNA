using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UIPanel : UIControl
    {
        public UIPanel(UIScreen screenOwner) :
            base(screenOwner)
        {
           
        }

        public UIPanel(UIScreen screenOwner, Vector2 position) :
            base(screenOwner)
        {
            this.Position = position;
        }

    }
}
