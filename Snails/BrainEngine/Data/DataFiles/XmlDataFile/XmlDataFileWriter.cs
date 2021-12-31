using System;
using System.Xml;
using System.IO;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile
{
    public class XmlDataFileWriter : IDataFileWriter
    {
        #region Member vars
        XmlDocument _XmlDoc;
        #endregion

        #region Constructors

        public XmlDataFileWriter()
        {

        }
        #endregion

        #region IDataFileWriter Members

        public Stream ToStream(DataFile file)
        {
            this._XmlDoc = new XmlDocument();

            if (file.RootRecord != null)
            {
                this._XmlDoc.AppendChild(this.CreateElement(file.RootRecord));
            }

            MemoryStream ms = new MemoryStream();
            this._XmlDoc.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
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

        public void Write(string filename, DataFile file)
        {
            this.Write(filename, file, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Write(string filename, DataFile file, string encryptionKey)
        {
            if (!string.IsNullOrEmpty(encryptionKey))
            {
                throw new BrainException("XmlDataFileWriter does not support encryption");
            }

            Stream stream = null;
            StreamReader sr = null;
            try
            {
                stream = this.ToStream(file);
                sr = new StreamReader(stream);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sr.ReadToEnd());
                xmlDoc.Save(filename);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (sr != null)
                    sr.Close();
            }
        }
        #endregion

        #region Private methods

        private XmlElement CreateElement(DataFileRecord record)
        {
            XmlElement elem = this._XmlDoc.CreateElement(record.Name);
            foreach (DataFileField field in record.Fields)
            {
                elem.SetAttribute(field.Name, (field.Value == null ? "" : field.Value.ToString()));
            }

            foreach (DataFileRecord childRecord in record.ChildRecords)
            {
                elem.AppendChild(this.CreateElement(childRecord));
            }

            return elem;
        }

        #endregion
    }
}
