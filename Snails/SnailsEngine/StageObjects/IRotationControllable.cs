using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// Objects that are controlled by a rotation controller should implement this interface
    /// </summary>
    interface IRotationControllable
    {
        StageObject ControlledObject { get; }
        bool ControllerValueChanged(float value);
    }
}
