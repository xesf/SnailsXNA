using System;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  public class DataFileFormatException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public DataFileFormatException()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileFormatException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileFormatException(string message, System.Exception ex) :
            base(message, ex)
        {

        }
    }
}
