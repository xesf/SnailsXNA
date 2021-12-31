using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// Objects that the cursor can interact with, should implement this interface
    /// Trampoline is an example of such an object, where the player can use the cursor
    /// to adjust power and direction
    /// </summary>
    public interface ICursorInteractable
    {
        StageCursor.CursorType QueryCursor(); // Object should return the cursor type to set
        bool QueryInterating(); // Object should return false to indicate that it is no longer interating with the cursor
        void CursorActionPressed(Vector2 cursorPos); // Cursor uses this method to inform the object that the user pressed the action key down
        void CursorActionReleased(); // Cursor uses this method to inform the object that the user pressed the action key up
        void CursorActionSelected(); // Cursor uses this method to inform a action click on the object
        bool CanAcceptCursorInteraction { get; }
    }
}
