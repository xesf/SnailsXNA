using System.Diagnostics;
using System.IO;
using System;

namespace TwoBrainsGames.BrainEngine.Debugging
{
    public class BETrace
    {
#if TRACE && !WIN8
        static TextWriterTraceListener Listener { get; set; }
        private static string _filename;
        private static long _maxLogSize;
        private static bool _traceLoaded = false;
#endif

        /// <summary>
        /// 
        /// </summary>
        public static void SetTraceToFile(string filename, long maxLogSize)
        {
#if TRACE && !WIN8
            _filename = filename;
            _maxLogSize = maxLogSize;

            try
            {
                if (Trace.Listeners.Contains(BETrace.Listener))
                {
                    Trace.Listeners.Remove(BETrace.Listener);
                    BETrace.Listener = null;
                }

                if (File.Exists(_filename))
                {
                    FileInfo fi = new FileInfo(_filename);
                    if (fi.Length > _maxLogSize)
                    {
                        File.Delete(_filename);
                    }
                }
                BETrace.Listener = new TextWriterTraceListener(_filename);
                Trace.Listeners.Add(BETrace.Listener);
                Trace.AutoFlush = true; // Beware with this, for now I don't care
                
                _traceLoaded = true;
            }
            catch (Exception)
            {
                _traceLoaded = false;
                //SetTraceToFile(_filename, _maxLogSize); // try again
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static void WriteLine(string message, params object[] args)
        {
#if TRACE && !WIN8
            if (_traceLoaded)
            {
                Trace.WriteLine(string.Format("{0}|{1}||", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()) + string.Format(message, args));
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static void WriteLine(string message)
        {
#if TRACE && !WIN8
            if (_traceLoaded)
            {
                Trace.WriteLine(message);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Write(System.Exception ex)
        {
#if TRACE && !WIN8
            if (_traceLoaded)
            {
                BETrace.WriteLine("Exception!! {0}\n{1}", ex.Message, ex.ToString());
            }
#endif
        }
    }
}
