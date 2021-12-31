using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.StageEditor
{
    class StageValidationException : ApplicationException
    {
        public StageValidationException(string message)
            : base(message)
        {
        }
    }
}
