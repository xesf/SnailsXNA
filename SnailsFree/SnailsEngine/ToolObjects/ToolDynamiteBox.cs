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
    /// Dynamite box tool - Box that explodes after a few seconds
    /// With the ToolCrate base class, boxed tools are now an empty class
    /// It seems that boxed tools may use all the same class
    public class ToolDynamiteBox : ToolCrate
    {
        public const string ID = "TOOL_DYNAMITE_BOX";
  
        public ToolDynamiteBox()
            : base(ToolObjectType.DynamiteBox)
        {

        }

    }
}
