using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UIMask : UIControl
    {
        public enum MaskAction
        {
            Apply,
            Remove
        }

        public MaskAction Action { get; set; }
        RenderMask _mask;

        public UIMask(UIScreen screenOwner) :
            this(screenOwner, null)
        {
        }

         /// <summary>
        /// 
        /// </summary>
        public UIMask(UIScreen screenOwner, string spriteName) :
            base(screenOwner)
        {
            this.Action = MaskAction.Apply;
            if (!string.IsNullOrEmpty(spriteName))
            {
                this._mask = RenderMask.CreateFromSprite(spriteName);
            }
        }

        /// <summary>
        /// DOn't call initialize when loading, because loading is assync and this may cause problems
        /// with SpriteBatch
        /// </summary>
        public void Inicialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this._mask != null && this.Action == MaskAction.Apply)
            {
                this._mask.Position = this.AbsolutePositionInPixels;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        { 

            if (this.Action == MaskAction.Apply)
            {
                this.ScreenOwner.EndDraw();
                this._mask.Render();
                this.ScreenOwner.Mask = this._mask;
                this.ScreenOwner.BeginDraw(BlendState.AlphaBlend);
            }
            else
            {
                this.ScreenOwner.Mask = null;
                this.ScreenOwner.EndDraw();
                this.ScreenOwner.BeginDraw(BlendState.AlphaBlend);
            }
        }
    }
}
