using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.Xml;
using System.IO;
using System.Windows.Forms;

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
            StreamReader sr = null;
            try
            {
                DataFile dataFile = new DataFile();
                sr = new StreamReader(filename);
                return this.Read(sr.BaseStream);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close(); 
                }
            }
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
