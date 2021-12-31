using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails
{
    /// <summary>
    /// Implement this interface in any class that needs do perform initialization, loading etc in a separate thread
    /// IAsyncOperation objects can be added to the AsyncProcessor
    /// </summary>
    interface IAsyncOperation
    {
        void BeginLoad();
        object AsyncLoadingParams { set; }
    }
}
