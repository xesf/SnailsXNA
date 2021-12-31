using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.RemoteServices
{
    public class RemoteAPICallResult
    {
        public bool WithError { get; internal set; }
        public string Message { get; internal set; }
        public RemoteAPIStatusCode StatusCode { get; private set; }

        public RemoteAPICallResult(string nativeAPImessage)
        {
            this.StatusCode = this.GetStatusCode(nativeAPImessage);
        }

        /// <summary>
        /// 
        /// </summary>
        RemoteAPIStatusCode GetStatusCode(string nativeAPImessage)
        {
            nativeAPImessage = nativeAPImessage.ToUpper();
            if (nativeAPImessage == "PLAYER NOT FOUND")
            {
                return RemoteAPIStatusCode.UserDoesNotExist;
            }

            if (nativeAPImessage == "INVALID PLAYER PASSWORD")
            {
                return RemoteAPIStatusCode.InvalidPassword;
            }

            return RemoteAPIStatusCode.Unknown;
        }
    }
}
