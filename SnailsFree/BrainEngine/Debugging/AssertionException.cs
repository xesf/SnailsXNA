using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Debugging
{
    public class AssertionException : BrainException
    {
        /// <summary>
        /// 
        /// </summary>
        public AssertionException()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public AssertionException(string message) :
                base ("Debug Assertion failed! " + message)
        {

        }
    }
}
