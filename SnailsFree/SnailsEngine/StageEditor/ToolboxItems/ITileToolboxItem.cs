using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.StageEditor.ToolboxItems
{
    public interface ITileToolboxItem : IToolboxItem
    {
        int StageDataGroupId { get; set; }
        Tile Tile { get; set; }
        bool Visible { get; set; }
    }
}
