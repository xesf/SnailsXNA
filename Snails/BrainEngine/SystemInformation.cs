using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Info;

namespace TwoBrainsGames.BrainEngine
{
    public class SystemInformation
    {
        static string _deviceUniqueID;

        public static string DeviceUniqueID
        {
            get
            {
                if (_deviceUniqueID == null)
                {
                    _deviceUniqueID = DeviceExtendedProperties.GetValue(;
                }
            }
        }
    }
}
