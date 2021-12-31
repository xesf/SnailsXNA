using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.ToolObjects
{
    public class ToolApple : ToolObject
    {
        public const string ID = "TOOL_APPLE";

        public ToolApple()
            : base(ToolObjectType.Apple)
        {

        }

        public override void Action(Vector2 position)
        {
            if (this.Quantity > 0)
            {
                base.Action(position);

                StageObject obj = Stage.CurrentStage.StageData.GetObject(Apple.ID);
                obj.Position = position;
                obj.UpdateBoundingBox();
                Stage.CurrentStage.AddObjectInRuntime(obj);
            }
        }


    }
}

