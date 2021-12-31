using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.ToolObjects
{
    public class ToolTrampoline : ToolObject
    {
        #region Consts
        public const string ID = "TOOL_TRAMPOLINE";
        #endregion

        //private Sprite _powerSprite;
        //private Sprite _baseSprite;

        Trampoline _trampoline;

        public ToolTrampoline()
            : base(ToolObjectType.Trampoline)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            
            //this._powerSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/trampoline", Trampoline.SPRITE_POWER);
            //this._baseSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/trampoline", Trampoline.SPRITE_BASE);
        }

        /// <summary>
        /// Useful to show the tutorial when a tool is selected
        /// </summary>
        public override void OnSelect()
        {
            base.OnSelect();
            this._trampoline = (Trampoline)Stage.CurrentStage.StageData.GetObject(Trampoline.ID);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Action(Vector2 position)
        {
            if (this.Quantity > 0)
            {
                base.Action(position);
                StageObject obj = Stage.CurrentStage.StageData.GetObject(Trampoline.ID);
                obj.Position = position;
                obj.UpdateBoundingBox();
                obj.Initialize();
                Stage.CurrentStage.AddObjectInRuntime(obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DrawCursor(Vector2 position, bool enabled)
        {
            this._trampoline.Position = position;
            this._trampoline.Draw(false);
        }
    }
}
