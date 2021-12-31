using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile
{
    public class BinaryDataFileReader : IDataFileReader
    {
        #region Member vars
        BinaryReader _Reader;
        bool _WithRootRecord;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public BinaryDataFileReader()
        {

        }
        #endregion

        #region IDataFileReader Members
        /// <summary>
        /// 
        /// </summary>
        public DataFile Read(Stream stream)
        {
          this._WithRootRecord = false;

          DataFile dataFile = new DataFile();
          this._Reader = new BinaryReader(stream);

          this.ReadHeader();
          if (this._WithRootRecord)
          {
            dataFile.RootRecord = this.ReadRecord();
          }
          return dataFile;
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFile Read(string filename)
        {
          this._WithRootRecord = false;

            DataFile dataFile = new DataFile();
          //  MemoryStream memStream = null;
            FileStream readStream = new FileStream(filename, FileMode.Open);
            try
            {
           /*     memStream = new MemoryStream();
                memStream.SetLength(readStream.Length);
                readStream.Read(memStream.GetBuffer(), 0, (int)readStream.Length);
                readStream.Close();
                */
                this._Reader = new BinaryReader(readStream);

                this.ReadHeader();
                if (this._WithRootRecord)
                {
                  dataFile.RootRecord = this.ReadRecord();
                }
                return dataFile;
            }
            finally
            {
                if (this._Reader != null)
                    this._Reader.Close();

                if (readStream != null)
                    readStream.Close();
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// 
        /// </summary>
        private DataFileRecord ReadRecord()
        {
            BlockId blockId = (BlockId)this._Reader.ReadInt32();
            if (blockId != BlockId.RecordStart)
            {
              throw new DataFileFormatException();
            }
            string recName = this._Reader.ReadString();
            int childRecordCount = this._Reader.ReadInt32();
            int fieldsCount = this._Reader.ReadInt32();
            DataFileRecord record = new DataFileRecord(recName);
            for (int i = 0; i < fieldsCount; i++)
            {
                record.AddField(this.ReadField());
            }
            for (int i = 0; i < childRecordCount; i++)
            {
                record.AddRecord(this.ReadRecord());
            }

            blockId = (BlockId)this._Reader.ReadInt32();
            if (blockId != BlockId.RecordEnd)
            {
              throw new DataFileFormatException();
            }
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        private DataFileField ReadField()
        {
            BlockId blockId = (BlockId)this._Reader.ReadInt32();
            if (blockId != BlockId.FieldStart)
            {
                throw new DataFileFormatException();
            }

            string fieldName = this._Reader.ReadString();
            FieldType type = (FieldType)this._Reader.ReadInt32();
            object data = null;
            switch (type)
            {
                case FieldType.Str:
                    data = this._Reader.ReadString();
                    break;
                case FieldType.Int:
                    data = this._Reader.ReadInt32();
                    break;
                case FieldType.Long:
                    data = this._Reader.ReadInt64();
                    break;
                case FieldType.Float:
                case FieldType.Double:
                    data = this._Reader.ReadDouble();
                    break;
                case FieldType.Bool:
                    data = this._Reader.ReadBoolean();
                    break;
                case FieldType.Char:
                    data = this._Reader.ReadChar();
                    break;
               
            }

            blockId = (BlockId)this._Reader.ReadInt32();
            if (blockId != BlockId.FieldEnd)
            {
              throw new DataFileFormatException();
            }

            return new DataFileField(fieldName, data);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReadHeader()
        {
            BlockId blockId = (BlockId)this._Reader.ReadInt32();
            if (blockId != BlockId.HeaderStart)
            {
              throw new DataFileFormatException();
            }
            int version = this._Reader.ReadInt32();
            string fileId = this._Reader.ReadString();
            if (fileId != Consts.FILE_STAMP)
            {
              throw new DataFileFormatException();
            }
            int rootRecordFlag = this._Reader.ReadInt32();
            this._WithRootRecord = (rootRecordFlag == 1);
            blockId = (BlockId)this._Reader.ReadInt32();
            if (blockId != BlockId.HeaderEnd)
            {
              throw new DataFileFormatException();
            }

        }
        #endregion
    }
}
