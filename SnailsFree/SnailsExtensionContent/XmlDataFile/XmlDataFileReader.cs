using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.Xml;
using System.IO;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile
{
    public class XmlDataFileReader : IDataFileReader
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public XmlDataFileReader()
        {

        }
        #endregion

        #region IDataFileReader Members
        /// <summary>
        /// 
        /// </summary>
        public DataFile Read(Stream stream)
        {
          DataFile dataFile = new DataFile();
          StreamReader sr = new StreamReader(stream);
          
          XmlDocument xmlDoc = new XmlDocument();
          try
          {
            xmlDoc.LoadXml(sr.ReadToEnd());
          }
          catch (System.Exception ex)
          {
            throw new DataFileFormatException("XML file is not in a valid format. " + ex.ToString());
          }

          foreach (XmlNode node in xmlDoc.ChildNodes)
          {
            if (node as XmlElement == null)
              continue;
            dataFile.RootRecord = this.CreateRecord((XmlElement)node);
            break;
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
            if (!string.IsNullOrEmpty(encryptionKey))
            {
                throw new BrainException("XmlDataFileReader does not support encryption");
            }

            DataFile dataFile = new DataFile();

            StreamReader sr = new StreamReader(filename);
            return this.Read(sr.BaseStream);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private DataFileRecord CreateRecord(XmlElement elem)
        {
            DataFileRecord record = new DataFileRecord(elem.Name);
            foreach (XmlAttribute attrib in elem.Attributes)
            {
                DataFileField field = new DataFileField(attrib.Name, attrib.Value);
                record.AddField(field);
            }

            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node as XmlElement == null)
                    continue;
                record.AddRecord(this.CreateRecord((XmlElement)node));
            }

            return record;
        }
    }
}
