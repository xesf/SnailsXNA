using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.ToolObjects
{
    /// <summary>
    /// Wooden box tool
    /// With the ToolCrate base class, boxed tools are now an empty class
    /// It seems that boxed tools may use all the same class
    /// </summary>
    public class ToolBox : ToolCrate
    {
        public const string ID = "TOOL_BOX";
        /// <summary>
        /// 
        /// </summary>
        public ToolBox()
            : base(ToolObjectType.Box)
        {

        }

    }
}
