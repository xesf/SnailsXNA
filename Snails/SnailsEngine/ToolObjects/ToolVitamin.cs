using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.ToolObjects
{
    public class ToolVitamin : ToolObject
    {
        #region Constants
        public const string ID = "TOOL_VITAMIN";
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ToolVitamin()
            : base(ToolObjectType.Vitamin)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Action(Vector2 position)
        {
            if (this.Quantity > 0)
            {
                base.Action(position);

                StageObject obj = Stage.CurrentStage.StageData.GetObject(Vitamin.ID);
                obj.Position = position;
                obj.UpdateBoundingBox();
                Stage.CurrentStage.AddObjectInRuntime(obj);
            }
        }

    }
}
