using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;
using System.Diagnostics;

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
                retStream.Write(writeStream.GetBuffer(), 0, (int)writeStream.Length);
                retStream.Position = 0;
                return retStream;
            }
            finally
            {
                if (this._Writer != null)
                    this._Writer.Close();
                if (writeStream != null)
                    writeStream.Close();
            }
        }

        public void Write(BinaryWriter sr, DataFile file)
        {

            Stream stream = this.ToStream(file);

            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                sr.Write(buffer, 0, len);
            }
        }

        public void Write(string filename, DataFile dataFile)
        {
            FileStream writeStream = null;
            Stream stream = null;
            try
            {
                writeStream = new FileStream(filename, FileMode.OpenOrCreate);

                stream = this.ToStream(dataFile);
                byte[] buffer = new byte[8 * 1024];
                int len;
                while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writeStream.Write(buffer, 0, len);
                }
            }
            finally
            {
                if (writeStream != null)
                    writeStream.Close();
                if (stream != null)
                    stream.Close();
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

                default:
                    throw new DataFileFormatException("Unexpected FieldType '" + field.Type.ToString() + "'.");
            }
            this._Writer.Write((Int32)BlockId.FieldEnd);
        }
    }
}
