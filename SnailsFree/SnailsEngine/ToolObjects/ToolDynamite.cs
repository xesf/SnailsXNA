using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.ToolObjects
{
    public class ToolDynamite : ToolObject
    {
        #region Constants
        const string SPRITE_PLAYER_CURSOR_BOMB_SELECTED = "BombToolSelected";
        public const string ID = "TOOL_DYNAMITE";
        #endregion

        #region Vars
        Sprite _spriteHotSpot;
        float _hotSpotRotation;
        Dynamite _dynamite;
        #endregion

        public ToolDynamite()
            : base(ToolObjectType.Dynamite)
        {
        }

        public ToolDynamite(ToolObjectType type)
            : base(type)
        {
        }
         /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._spriteHotSpot = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite/DynamiteRadius");
            this._hotSpotRotation = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Action(Vector2 position)
        {
            if (this.Quantity > 0)
            {
                base.Action(position);

                Dynamite obj = (Dynamite)Stage.CurrentStage.StageData.GetObject(Dynamite.ID);
                obj.Position = position;
                obj.UpdateBoundingBox();

                Stage.CurrentStage.AddObjectInRuntime(obj);
            }
        }
        /// <summary>
        /// Useful to show the tutorial when a tool is selected
        /// </summary>
        public override void OnSelect()
        {
            base.OnSelect();
            if (this._dynamite == null)
            {
                this._dynamite = (Dynamite)Stage.CurrentStage.StageData.GetObject(Dynamite.ID);
                this._dynamite.Extinguish();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this._hotSpotRotation += 0.01f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DrawCursor(Vector2 position, bool enabled)
        {
            //base.DrawCursor(position, enabled);
            this._dynamite.Position = position;
            this._dynamite.Draw(false);
            if (enabled)
            {
                this._spriteHotSpot.Draw(position, 0, this._hotSpotRotation, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, Levels.CurrentLevel.SpriteBatch);
            }
        }

    }
}
