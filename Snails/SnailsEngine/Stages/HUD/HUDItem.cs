using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    /// <summary>
    ///  This is the base class for hud info items 
    ///  Items can be
    ///    -Goal icon
    ///    -Timer
    ///    -Snails delivered
    ///    -Total snails to be released + in game (to use in the snail killer mode)
    ///    -Mission status
    /// </summary>
    class HUDItem
    {
        public Vector2 _size;
        public Vector2 _position; // Position in screen coordinates of the item
        protected TextFont _font;
        public float _width;
        public bool _autoPosition;
        public bool _visible;

        public HUDItem()
        {
            this._autoPosition = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize(Vector2 position)
        {
            this._position = position;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadContent()
        {
            this._font = BrainGame.ResourceManager.Load<TextFont>(FontResources.MAIN_FONT_MEDIUM, ResourceManager.ResourceManagerCacheType.Static);
        }

        public virtual void Update(BrainGameTime gameTime)
        { }

        public virtual void Draw(SpriteBatch spriteBatch)
        { }

        public virtual void UnloadContent()
        { }

        public virtual void SnailsStageStatsChanged()
        { }

        public virtual void HandleInput(BrainGameTime gameTime)
        { }


        public virtual void MissionStateChanged()
        { }

        public virtual void TimeWarpChanged()
        { }

        public virtual void StageAreaChanged(BoundingSquare newStageArea)
        { }
    }
}
