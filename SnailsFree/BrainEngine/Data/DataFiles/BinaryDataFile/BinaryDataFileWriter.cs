using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using TwoBrainsGames.BrainEngine.Secutiry;
#if WP7
using System.IO.IsolatedStorage;
#endif

namespace TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile
{
    public class BinaryDataFileWriter : IDataFileWriter
    {
        #region Member vars
        BinaryWriter _Writer;
        #endregion

        #region Constructors

        public BinaryDataFileWriter()
        {

        }
        #endregion

        #region IDataFileWriter Members

        public Stream ToStream(DataFile dataFile)
        {
            MemoryStream writeStream = new MemoryStream();
            try
            {
                this._Writer = new BinaryWriter(writeStream);

                this.WriteHeader(dataFile);
                this.WriteRecord(dataFile.RootRecord);
                writeStream.Position = 0;

                MemoryStream retStream = new MemoryStream();
                writeStream.CopyTo(retStream);
                retStream.Position = 0;
                return retStream;
            }
            finally
            {
#if !WIN8
                if (this._Writer != null)
                    this._Writer.Close();
                if (writeStream != null)
                    writeStream.Close();
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Write(BinaryWriter sr, DataFile file)
        {
            this.Write(sr, file, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Write(BinaryWriter sr, DataFile file, string encryptionKey)
        {

            Stream stream = this.ToStream(file);
            if (string.IsNullOrEmpty(encryptionKey))
            {
                byte[] buffer = new byte[8 * 1024];
                int len;
                while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sr.Write(buffer, 0, len);
                }
            }
            else
            {
                byte[] buffer = Encryption.Encrypt(stream, encryptionKey);
                sr.Write(buffer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Write(string filename, DataFile dataFile)
        {
            this.Write(filename, dataFile, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Write(string filename, DataFile dataFile, string encryptionKey)
        {
#if !WIN8
            FileStream writeStream = null;
#else 
            Stream writeStream = null;
#endif
            Stream stream = null;
            try
            {
#if WP7
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                writeStream = new IsolatedStorageFileStream(filename, FileMode.OpenOrCreate, storage);
#elif WIN8
                writeStream = StorageHelper.WriteStream(filename);
                writeStream.Position = 0;
#else
                
                writeStream = new FileStream(filename, FileMode.OpenOrCreate);
#endif

#if WIN8
                stream = this.ToStream(dataFile);

                stream.CopyTo(writeStream);
                writeStream.SetLength(stream.Length);
#else
                stream = this.ToStream(dataFile);
                if (string.IsNullOrEmpty(encryptionKey))
                {
                    byte[] buffer = new byte[8 * 1024];
                    int len;
                    while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writeStream.Write(buffer, 0, len);
                    }
                }
                else
                {
                    byte[] buffer = Encryption.Encrypt(stream, encryptionKey);
                    writeStream.Write(buffer, 0, buffer.Length);
                }
#endif
            }
            finally
            {
#if !WIN8 
                if (writeStream != null)
                    writeStream.Close();               
                if (stream != null)
                    stream.Close();
#else
                if (writeStream != null)
                {
                    writeStream.Flush();
                    writeStream.Dispose();
                }
#endif
            }
        }
        #endregion

        private void WriteHeader(DataFile dataFile)
        {
            this._Writer.Write((Int32)BlockId.HeaderStart);
            this._Writer.Write((Int32)1); // Version
            this._Writer.Write(Consts.FILE_STAMP);
            this._Writer.Write((Int32)(dataFile.RootRecord == null ? 0 : 1));
            this._Writer.Write((Int32)BlockId.HeaderEnd);
        }

        private void WriteRecord(DataFileRecord record)
        {
            if (record == null)
                return;

            this._Writer.Write((Int32)BlockId.RecordStart);
            this._Writer.Write(record.Name);
            this._Writer.Write((Int32)record.ChildRecords.Count);
            this._Writer.Write((Int32)record.Fields.Count);
            foreach (DataFileField field in record.Fields)
            {
                this.WriteField(this._Writer, field);
            }
            foreach (DataFileRecord childRecord in record.ChildRecords)
            {
                this.WriteRecord(childRecord);
            }
            this._Writer.Write((Int32)BlockId.RecordEnd);
        }

        private void WriteField(BinaryWriter writer, DataFileField field)
        {
            this._Writer.Write((Int32)BlockId.FieldStart);
            this._Writer.Write(field.Name);
            this._Writer.Write((Int32)field.Type);
            switch (field.Type)
            {
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Str:
                    this._Writer.Write(field.Value.ToString());
                    break;
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Int:
                    this._Writer.Write((Int32)field.Value);
                    break;
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Float:
                    this._Writer.Write(Convert.ToDouble(field.Value));
                    break;
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Bool:
                    this._Writer.Write((bool)field.Value);
                    break;
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Double:
                    this._Writer.Write((double)field.Value);
                    break;
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Long:
                    this._Writer.Write((long)field.Value);
                    break;
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Char:
                    this._Writer.Write((char)field.Value);
                    break;
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Null:
                    break;
                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.XNAColor:
                    Color color = (Color)field.Value;
                    this._Writer.Write((byte)color.R);
                    this._Writer.Write((byte)color.G);
                    this._Writer.Write((byte)color.B);
                    this._Writer.Write((byte)color.A);
                    break;

                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Vector2:
                    Vector2 vector = (Vector2)field.Value;
                    this._Writer.Write(Convert.ToDouble(vector.X));
                    this._Writer.Write(Convert.ToDouble(vector.Y));
                    break;

                case TwoBrainsGames.BrainEngine.Data.DataFiles.FieldType.Size:
                    TwoBrainsGames.BrainEngine.UI.Size size = (TwoBrainsGames.BrainEngine.UI.Size)field.Value;
                    this._Writer.Write(Convert.ToDouble(size.Width));
                    this._Writer.Write(Convert.ToDouble(size.Height));
                    break;

                default:
                    throw new DataFileFormatException("Unexpected FieldType '" + field.Type.ToString() + "'.");
            }
            this._Writer.Write((Int32)BlockId.FieldEnd);
        }
    }
}
