using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace FontEdit
{
    class CSettings
    {
        static string _LastEditedFile;
        public static string LastEditedFile
        {
            get { return CSettings._LastEditedFile; }
            set { CSettings._LastEditedFile = value; }
        }

        static string _LastOpenFileFolder;
        public static string LastOpenFileFolder
        {
            get { return CSettings._LastOpenFileFolder; }
            set { CSettings._LastOpenFileFolder = value; }
        }

        static string _LastOpenImageFolder;
        public static string LastOpenImageFolder
        {
            get { return CSettings._LastOpenImageFolder; }
            set { CSettings._LastOpenImageFolder = value; }
        }

        static string _LastExportFolder;
        public static string LastExportFolder
        {
            get { return CSettings._LastExportFolder; }
            set { CSettings._LastExportFolder = value; }
        }
        private static bool _UseBlackBox = true;
        public static bool UseBlackBox
        {
            get
            {
                return CSettings._UseBlackBox;
            }
            set
            {
                CSettings._UseBlackBox = value;
            }
        }

        private static int _DefaultCharWidth = 25;
        public static int DefaultCharWidth
        {
            get
            {
                return CSettings._DefaultCharWidth;
            }
            set
            {
                CSettings._DefaultCharWidth = value;
            }
        }

        private static int _DefaultCharHeight = 25;
        public static int DefaultCharHeight
        {
            get
            {
                return CSettings._DefaultCharHeight;
            }
            set
            {
                CSettings._DefaultCharHeight = value;
            }
        }

        private static int _Snap = 10;
        public static int Snap
        {
            get
            {
                return CSettings._Snap;
            }
            set
            {
                CSettings._Snap = value;
            }
        }

        public static void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement elemRoot = xmlDoc.CreateElement("FontEdit.Settings");
            elemRoot.SetAttribute("LastEditedFile", CSettings.LastEditedFile);
            elemRoot.SetAttribute("LastOpenFileFolder", CSettings.LastOpenFileFolder);
            elemRoot.SetAttribute("LastOpenImageFolder", CSettings.LastOpenImageFolder);
            elemRoot.SetAttribute("LastExportFolder", CSettings.LastExportFolder);
            xmlDoc.AppendChild(elemRoot);
            xmlDoc.Save("FontEdit.Settings.xml");
        }

        public static void Load()
        {
            if (File.Exists("FontEdit.Settings.xml") == false)
                return;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("FontEdit.Settings.xml");
            XmlElement elemRoot = (XmlElement)xmlDoc.FirstChild;
            if (elemRoot.Attributes["LastEditedFile"] != null)
                CSettings.LastEditedFile = elemRoot.Attributes["LastEditedFile"].Value;
            if (elemRoot.Attributes["LastOpenFileFolder"] != null)
                CSettings.LastOpenFileFolder = elemRoot.Attributes["LastOpenFileFolder"].Value;
            if (elemRoot.Attributes["LastOpenImageFolder"] != null)
                CSettings.LastOpenImageFolder = elemRoot.Attributes["LastOpenImageFolder"].Value;
            if (elemRoot.Attributes["LastExportFolder"] != null)
                CSettings.LastExportFolder = elemRoot.Attributes["LastExportFolder"].Value;

        }

    }
}
