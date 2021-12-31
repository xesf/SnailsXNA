using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Secutiry;
#if WP7
using System.IO.IsolatedStorage;
#endif
#if WIN8
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Foundation;
using System.Threading.Tasks;
#endif

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
            return this.Read(stream, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFile Read(Stream stream, string encryptionKey)
        {
          this._WithRootRecord = false;

          DataFile dataFile = new DataFile();
          if (string.IsNullOrEmpty(encryptionKey))
          {
              this._Reader = new BinaryReader(stream);
          }
          else
          {
              byte[] decryptedBytes = Encryption.Decrypt(stream, encryptionKey);
              MemoryStream memStream = new MemoryStream(decryptedBytes);
              this._Reader = new BinaryReader(memStream);
          }

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
            return this.Read(filename, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFile Read(string filename, string encryptionKey)
        {
          this._WithRootRecord = false;

            DataFile dataFile = new DataFile();
          //  MemoryStream memStream = null;
#if XBOX
            Stream readStream = TitleContainer.OpenStream(filename);
#elif WP7
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            FileStream readStream = new IsolatedStorageFileStream(filename, FileMode.Open, storage);
#elif WIN8
            Stream readStream = StorageHelper.ReadStream(filename);
#else
            FileStream readStream = new FileStream(filename, System.IO.FileMode.Open);
#endif
            try
            {
                if (readStream != null)
                {
                    if (string.IsNullOrEmpty(encryptionKey))
                    {
                        this._Reader = new BinaryReader(readStream);
                    }
                    else
                    {
                        byte[] decryptedBytes = Encryption.Decrypt(readStream, encryptionKey);
                        MemoryStream memStream = new MemoryStream(decryptedBytes);
                        this._Reader = new BinaryReader(memStream);
                    }

                    this.ReadHeader();
                    if (this._WithRootRecord)
                    {
                        dataFile.RootRecord = this.ReadRecord();
                    }
                }
                return dataFile;
            }
            finally
            {
#if !WIN8
                if (this._Reader != null)
                    this._Reader.Close();

                if (readStream != null)
                    readStream.Close();
#else
                if (readStream != null)
                {
                    readStream.Dispose();
                }
#endif
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
                case FieldType.XNAColor:
                    byte r = this._Reader.ReadByte();
                    byte g = this._Reader.ReadByte();
                    byte b = this._Reader.ReadByte();
                    byte a = this._Reader.ReadByte();
                    data = new Color(r, g, b, a);
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
            this._Reader.ReadInt32();
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
