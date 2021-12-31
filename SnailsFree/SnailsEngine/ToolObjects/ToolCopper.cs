using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.ToolObjects
{
    /// Cooper box tool
    /// With the ToolCrate base class, boxed tools are now an empty class
    /// It seems that boxed tools may use all the same class
    public class ToolCopper : ToolCrate
    {
        public const string ID = "TOOL_COPPER";

        public ToolCopper()
            : base(ToolObjectType.Copper)
        {

        }

    }
}
