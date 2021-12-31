using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    public struct FlagsType
    {
        public int Value;

        public static FlagsType Zero { get { return new FlagsType(); } }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
