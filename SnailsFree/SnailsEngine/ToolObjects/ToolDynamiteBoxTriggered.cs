using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.ToolObjects
{
    /// Triggered dynamite box tool - Box that explodes when a snail passes over it (after a few seconds)
    /// With the ToolCrate base class, boxed tools are now an empty class
    /// It seems that boxed tools may use all the same class
    public class ToolDynamiteBoxTriggered : ToolCrate
    {
        public const string ID = "TOOL_DYNAMITE_BOX_TRIGGERED";

        public ToolDynamiteBoxTriggered()
            : base(ToolObjectType.DynamiteBoxTriggered)
        {
        }
    }
}
