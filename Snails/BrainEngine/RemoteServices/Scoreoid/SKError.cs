using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scoreoid.Kit
{
    public class SKError
    {
        public SKError(int errorCode)
        {
            
        }

        public SKError(string description)
        {
            this.LocalizedDescription = description;
        }

        public string LocalizedDescription
        {
            get;
            private set;
        }       
    }
}
