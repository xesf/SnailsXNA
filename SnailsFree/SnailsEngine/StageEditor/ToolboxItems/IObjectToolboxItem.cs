using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;

namespace TwoBrainsGames.Snails.StageEditor.ToolboxItems
{
    public interface IObjectToolboxItem : IToolboxItem
    {
        StageObject StageObject { get; set; }
        ThemeType ValidTheme { get; }
    }
}
