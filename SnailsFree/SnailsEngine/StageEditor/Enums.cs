using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.StageEditor
{
        public enum ToolType
        {
            None,
            Tile,
            GameObject,
            Select,
            Prop
        }

        public enum SelectionType
        {
            Tiles,
            Objects,
            TilesAndObjects
        }
}
