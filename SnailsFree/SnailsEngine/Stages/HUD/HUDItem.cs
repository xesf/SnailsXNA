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
        Vector2 _size;
        Vector2 _position; // Position in screen coordinates of the item
        protected TextFont _font;
        public float _width;

        public Vector2 Size
        {
            get { return this._size; }
            set
            {
                this._size = value;
                this.UpdateBoundingSquare();
            }
        }

        public Vector2 Position
        {
            get { return this._position; }
            set
            {
                this._position = value;
                this.UpdateBoundingSquare();
            }
        }

        public bool _autoPosition;
        public bool _visible;
        public bool Selectable { get; set; }
        public BoundingSquare SelectableArea { get; set; }

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
        public virtual void Update(BrainGameTime brainTime)
        {
            if (this.Selectable == false)
            {
                return;
            }
            if (Stage.CurrentStage.Input.IsActionClicked)
            {
                if (this.SelectableArea.Contains(Stage.CurrentStage.Input.MotionPosition))
                {
                    this.OnSelect();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnSelect()
        {
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadContent()
        {
            this._font = BrainGame.ResourceManager.Load<TextFont>(FontResources.MAIN_FONT_MEDIUM, ResourceManager.ResourceManagerCacheType.Static);
        }

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

        /// <summary>
        /// 
        /// </summary>
        private void UpdateBoundingSquare()
        {
            this.SelectableArea = new BoundingSquare(this.Position, this.Size.X, this.Size.Y);
        }

    }
}
