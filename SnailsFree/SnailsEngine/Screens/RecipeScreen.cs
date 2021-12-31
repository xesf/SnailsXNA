using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.Snails.Screens
{
    class RecipeScreen  : Screen
    {
        Sprite _Sprite;

        public RecipeScreen(ScreenNavigator owner) :
            base(owner)
        { }


        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            this._Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/recipe", "Recipe");
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
            if (this._inputController.ActionAccept)
            {
                this.NavigateTo(ScreenType.MainMenu.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnDraw()
        {
          this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, null, SnailsGame.DefaultCamera.Transform);
            this._Sprite.Draw(Vector2.Zero, this._spriteBatch);
            this._spriteBatch.End();
        }
    }
}
