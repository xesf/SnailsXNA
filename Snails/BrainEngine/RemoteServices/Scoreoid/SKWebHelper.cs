namespace Scoreoid.Kit
{
    using System;
    using System.Collections.Generic;
    using System.IO; 
    using System.Reflection;
    using System.Text;
    using System.Net;    
    using System.Xml.Linq;

#if WINDOWS

    using System.Web;

#endif

    class SKWebRequestParameter
    {
        public string Name 
        { 
            get; 
            set; 
        }

        public object Value 
        { 
            get; 
            set; 
        }
    }

    static class SKWebHelper
    {
        #region Public Methods

        public static void SubmitRequest(string method, string apiKey, string gameID, object parameters, Action<XDocument> success, Action<string> failed)
        {
            // Create a request

            string uri = String.Format("{0}/{1}", SKSettings.ApiUrl, method);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Method = "POST"; 
            			 			           
            // What we are sending
            string postData = String.Format("api_key={0}&game_id={1}&response={2}",
                HtmlEncode(apiKey),
                HtmlEncode(gameID),
                HtmlEncode("XML"));
                        
            if (parameters != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (SKWebRequestParameter p in GetRequestParameters(parameters))
                    sb.AppendFormat("&{0}={1}", p.Name, HtmlEncode(p.Value.ToString()));

                postData = String.Concat(postData, sb.ToString());
            }

            // Turn our request string into a byte stream
            byte[] postBuffer = Encoding.UTF8.GetBytes(postData);

            // This is important - make sure you specify type this way
            webRequest.ContentType = "application/x-www-form-urlencoded";

#if !WP7 && !WIN8

            webRequest.ContentLength = postBuffer.Length;
            webRequest.KeepAlive = false;
            webRequest.ProtocolVersion = HttpVersion.Version10;

#endif

            int timeoutInterval = 30000;            

#if WP7

            DateTime requestDate = DateTime.Now;


            Timer timer = new Timer(
                (state) =>
                {
                    if ((DateTime.Now - requestDate).TotalMilliseconds >= timeoutInterval)
                        webRequest.Abort();

                }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
            

#elif IOS || ANDROID

			webRequest.Timeout = timeoutInterval;            
            webRequest.Proxy = new WebProxy(SKSettings.ProxyUrl);
			ServicePointManager.ServerCertificateValidationCallback = (p1, p2, p3, p4) => true;

#endif

            try
            {
                webRequest.BeginGetRequestStream(
                    requestAsyncResult =>
                    {                        
						try
                        {

#if WP7

                            timer.Change(Timeout.Infinite, Timeout.Infinite); 
#endif

	                        HttpWebRequest request =
	                            ((HttpWebRequest)((object[])requestAsyncResult.AsyncState)[0]);

	                        byte[] buffer =
	                            ((byte[])((object[])requestAsyncResult.AsyncState)[1]);

	                        Stream requestStream =
	                            request.EndGetRequestStream(requestAsyncResult);

	                        requestStream.Write(buffer, 0, buffer.Length);
#if WIN8
                            requestStream.Flush();
#else
	                        requestStream.Close();
#endif
#if WP7

                            requestDate = DateTime.Now;
                            timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));

#endif

                            request.BeginGetResponse((state) =>
                            {

#if WP7

                                timer.Change(Timeout.Infinite, Timeout.Infinite); 

#endif

                                HttpWebResponse response = null;

                                try
                                {
                                    response =
                                        (HttpWebResponse)((HttpWebRequest)state.AsyncState).EndGetResponse(state);

                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        // If the request success, then call the success callback
                                        // or the failed callback by reading the response data      
                                        using (Stream stream = response.GetResponseStream())
                                        {
                                            try
                                            {
                                                XDocument xdoc = XDocument.Load(stream);

                                                // Data contains error notification.
                                                if (xdoc.Root.Name == "error")
                                                    throw new InvalidOperationException(xdoc.Root.Value);

                                                success(xdoc);
                                            }
                                            catch (Exception ex)
                                            {
                                                failed(ex.Message);
                                            }

#if WIN8
                                            stream.Flush();
#else
	                                        stream.Close();
#endif
                                        }
                                    }
                                    else
                                    {
                                        // If the request fails, then call the failed callback
                                        // to notfiy the failing status description of the request
                                        failed(response.StatusDescription);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // If the request fails, then call the failed callback
                                    // to notfiy the failing status description of the request
                                    failed("Unknown HTTP error.");
                                }
                                finally
                                {
                                    request.Abort();

#if !WIN8
                                    if(response != null)
	                                    response.Close();
#endif
                                }

	                        }, request);
						}
                        catch (Exception ex)
						{
							// Raise an error in case of exception
							// when submitting a request
							failed("Unknown HTTP error.");
						}

                    }, new object[] { webRequest, postBuffer });
            }
            catch (Exception ex)
            {
                // Raise an error in case of exception
                // when submitting a request
                failed("Unknown HTTP error.");
            }
        }

        #endregion

        #region Methods

        private static IEnumerable<SKWebRequestParameter> GetRequestParameters(object parameters)
        {
#if !WIN8
            if (parameters != null)
            {

#if WINDOWS

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(parameters);
                foreach (PropertyDescriptor prop in properties)
                {
                    object val = prop.GetValue(parameters);
                    if (val != null)
                        yield return new SKWebRequestParameter { Name = prop.Name, Value = val };                    
                }

#else

                Type type = parameters.GetType();
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (PropertyInfo prop in properties)
                {
                    object val = prop.GetValue(parameters, null);
                    if (val != null)
                        yield return new SKWebRequestParameter { Name = prop.Name, Value = val };
                }

#endif

            }
#else 
            return null;
#endif
        }

        private static string HtmlEncode(string value)
        {

#if WINDOWS && !MONOGAME

            return HttpUtility.HtmlEncode(value);

#else

            return Uri.EscapeUriString(value);

#endif

        }

        #endregion
    }
}
