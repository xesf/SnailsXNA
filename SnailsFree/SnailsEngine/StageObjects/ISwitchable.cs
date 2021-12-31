using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// Objects that may be activated by a switch must implement this interface
    /// </summary>
    public interface ISwitchable
    {
        void SwitchOn();
        void SwitchOff();
        bool IsOn { get; }
    }
}
