using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.Debuging
{
    public class AssertionException : Exception
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
