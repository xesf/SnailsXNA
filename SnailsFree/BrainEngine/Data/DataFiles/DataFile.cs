using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
#if REFLECTION_SUPPORT
using System.Runtime.Remoting;
#else
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
#endif

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
        XNAColor,
        Vector2,
        Size
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
                    throw new BrainException("Root node cannot be null.");
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
#if REFLECTION_SUPPORT
            ObjectHandle objHandle = Activator.CreateInstance(assemblyName, typeName);
            object reader = objHandle.Unwrap();

            if (reader as IDataFileReader == null)
            {
              throw new BrainException("Type '" + typeName + "' in assembly '" + assemblyName + "' does not implement IDataFileReader interface.");
            }
            return (IDataFileReader)reader;
#else
            return (IDataFileReader)(new BinaryDataFileReader());
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static IDataFileWriter CreateWriter(string assemblyName, string typeName)
        {
#if REFLECTION_SUPPORT
            ObjectHandle objHandle = Activator.CreateInstance(assemblyName, typeName);
            object writer = objHandle.Unwrap();

            if (writer as IDataFileWriter == null)
            {
              throw new BrainException("Type '" + typeName + "' in assembly '" + assemblyName + "' does not implement IDataFileWriter interface.");
            }

            return (IDataFileWriter)writer;
#else
            return (IDataFileWriter)(new BinaryDataFileWriter());
#endif
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
