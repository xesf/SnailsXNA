using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Remoting;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    public enum FieldType
    {
        NotSupported,
        Null,
        Str,
        Int,
        Float,
        Bool,
        Double,
        Char,
        Long,
        TimeSpan,
        XNAColor
    }

    public class DataFile
    {
        DataFileRecord _RootRecord;

        #region Properties
        public DataFileRecord RootRecord
        {
            get { return this._RootRecord; }
            set
            {
                if (value == null)
                {
                    throw new Exception("Root node cannot be null.");
                }
                this._RootRecord = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public DataFile()
        {
            this.Clear();
        }
        #endregion

        #region Reader/Writer creators
        /// <summary>
        /// 
        /// </summary>
        public static IDataFileReader CreateReader(string assemblyName, string typeName)
        {
            ObjectHandle objHandle = Activator.CreateInstance(assemblyName, typeName);
            object reader = objHandle.Unwrap();

            if (reader as IDataFileReader == null)
            {
              throw new Exception("Type '" + typeName + "' in assembly '" + assemblyName + "' does not implement IDataFileReader interface.");
            }
            return (IDataFileReader)reader;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IDataFileWriter CreateWriter(string assemblyName, string typeName)
        {
            ObjectHandle objHandle = Activator.CreateInstance(assemblyName, typeName);
            object writer = objHandle.Unwrap();

            if (writer as IDataFileWriter == null)
            {
              throw new Exception("Type '" + typeName + "' in assembly '" + assemblyName + "' does not implement IDataFileWriter interface.");
            }

            return (IDataFileWriter)writer;
        }
        #endregion

        #region IDataFile Members

        /// <summary>
        /// 
        /// </summary>
        public virtual DataFileRecord SelectRecord(string path)
        {
            if (this.RootRecord == null)
                return null;
            PathManager pathManager = PathManager.Parse(path);
            if (this.RootRecord.Name != pathManager.FirstField)
                return null;

            return this.RootRecord.ChildRecords.SelectRecord(pathManager.RemoveFirst().Path);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DataFileRecordList SelectRecords(string path)
        {
            if (this.RootRecord == null)
                return null;
            PathManager pathManager = PathManager.Parse(path);
            if (this.RootRecord.Name != pathManager.FirstField)
                return null;

            return this.RootRecord.ChildRecords.SelectRecords(pathManager.RemoveFirst().Path);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            this.RootRecord = new DataFileRecord("Root");
        }

        #endregion

        #region Class overrides
        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            string str = "";

            if (this.RootRecord != null)
            {
                str += this.RootRecord.ToString();

            }
            return str;
        }
        #endregion
    }
}
