using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIMiniSnailsTitle : UIControl
    {
        UIImage _imgTitle;

        public UIMiniSnailsTitle(UIScreen ScreenOwner) :
            base(ScreenOwner)
        {
            this.Position = new Vector2(0.0f, 400.0f);
            this.ShowEffect = new SquashEffect(0.7f, 4.0f, 0.04f);

            this._imgTitle = new UIImage(ScreenOwner, "spriteset/common-elements-1/SnailsLeafSmallTitle", ResourceManager.RES_MANAGER_ID_STATIC);
            this._imgTitle.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this.Controls.Add(this._imgTitle);

            this.Size = this._imgTitle.Size;
            this.AcceptControllerInput = false;
        }
    }
}
