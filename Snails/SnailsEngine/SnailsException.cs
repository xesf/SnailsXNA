using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails
{
    class SnailsException : BrainEngine.BrainException
    {
        public SnailsException()
        {
        }

        public SnailsException(string message) :
            base(message)
        {
        }


        public SnailsException(string message, System.Exception innerEx) :
            base(message, innerEx)
        {
        }
    }
}
