using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.ToolObjects
{
    class ToolDirectionalBox : ToolCrate
    {
        public const string ID_CW = "TOOL_DIRECTIONAL_BOX_CW";
        public const string ID_CCW = "TOOL_DIRECTIONAL_BOX_CCW";

        public ToolDirectionalBox(ToolObjectType type)
            : base(type)
        {

        }


    }
}
