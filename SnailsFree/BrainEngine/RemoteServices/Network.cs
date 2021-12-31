using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwoBrainsGames.BrainEngine.RemoteServices
{
    public class Network
    {
        public static bool IsInternetAvailable { get; set; }
        private static bool _testing;

        /// <summary>
        /// 
        /// </summary>
        public static void TestInternetAsync()
        {
            if (_testing)
            {
                return;
            }

            // retrieve an avatar image from the Web
            string uri = "http://www.google.com";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.BeginGetResponse(GooglePingCallback, request);
        }

        /// <summary>
        /// 
        /// </summary>
        static void GooglePingCallback(IAsyncResult result)
        {
            try
            {
                HttpWebRequest request = result.AsyncState as HttpWebRequest;
                if (request != null)
                {
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)result.AsyncState).EndGetResponse(result);
                        IsInternetAvailable = (response.StatusCode == HttpStatusCode.OK);
                    }
                    catch (WebException )
                    {
                        IsInternetAvailable = false;
                        return;
                    }
                }
            }
            finally
            {
                _testing = false;
            }
        }

    }
}
