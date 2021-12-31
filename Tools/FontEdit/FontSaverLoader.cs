using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using System.Drawing;

namespace FontEdit
{
    class FontSaverLoader
    {
        public static void ExportGameFont(string filename, CFont font, string absPath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement elemRoot = xmlDoc.CreateElement("Font");
            elemRoot.SetAttribute("ImageId", font.ImageId);
            xmlDoc.AppendChild(elemRoot);

            XmlElement elemCharacters = xmlDoc.CreateElement("Characters");
            elemCharacters.SetAttribute("SpaceWidth", font.SpaceWidth.ToString());
            elemCharacters.SetAttribute("BetweenCharsWidth", font.BetweenCharsWidth.ToString());
			elemCharacters.SetAttribute("LineHeight", font.LineHeight.ToString());
            elemRoot.AppendChild(elemCharacters);

            foreach (FontEdit.CFont.FontData ch in font.Characters)
            {
                if (ch.Character != null)
                {
                    XmlElement elemChar = xmlDoc.CreateElement("Character");
                    elemChar.SetAttribute("CharMap", ch.Character.ToString());
                    elemChar.SetAttribute("Left", ch.Rect.X.ToString());
                    elemChar.SetAttribute("Top", ch.Rect.Y.ToString());
                    elemChar.SetAttribute("Width", ch.Rect.Width.ToString());
                    elemChar.SetAttribute("Height", ch.Rect.Height.ToString());
                    elemChar.SetAttribute("Spacing", ch.Spacing.ToString());
                    elemChar.SetAttribute("SpacingAfter", ch.SpacingAfter.ToString());

                    elemCharacters.AppendChild(elemChar);
                }
            }

            xmlDoc.Save(filename);
        }

        public static void Save(string filename, CFont font)
        {
            XmlDocument xmlDoc = new XmlDocument();
            
            XmlElement elemRoot = xmlDoc.CreateElement("Font");
            elemRoot.SetAttribute("Image", Helper.RelativePath(Path.GetDirectoryName(filename), font.FontEditImageFilename));
            elemRoot.SetAttribute("ImageId", font.ImageId);
            elemRoot.SetAttribute("IngameExportFile", font.IngameExportFile);
            elemRoot.SetAttribute("ImageWidth", font.ImageWidth.ToString());
            elemRoot.SetAttribute("ImageHeight", font.ImageHeight.ToString());
            elemRoot.SetAttribute("NumCharsWidth", font.NumCharsWidth.ToString());
            elemRoot.SetAttribute("NumCharsHeight", font.NumCharsHeight.ToString());
            xmlDoc.AppendChild(elemRoot);

            XmlElement elemCharacters = xmlDoc.CreateElement("Characters");
            elemCharacters.SetAttribute("SpaceWidth", font.SpaceWidth.ToString());
            elemCharacters.SetAttribute("BetweenCharsWidth", font.BetweenCharsWidth.ToString());
			elemCharacters.SetAttribute("LineHeight", font.LineHeight.ToString());
            elemRoot.AppendChild(elemCharacters);

            foreach (FontEdit.CFont.FontData ch in font.Characters)
            {
                if (ch.Character != null)
                {
                    XmlElement elemChar = xmlDoc.CreateElement("Character");
                    elemChar.SetAttribute("CharMap", ch.Character.ToString());
                    elemChar.SetAttribute("Left", ch.Rect.X.ToString());
                    elemChar.SetAttribute("Top", ch.Rect.Y.ToString());
                    elemChar.SetAttribute("Width", ch.Rect.Width.ToString());
                    elemChar.SetAttribute("Height", ch.Rect.Height.ToString());
                    elemChar.SetAttribute("Spacing", ch.Spacing.ToString());
                    elemChar.SetAttribute("SpacingAfter", ch.SpacingAfter.ToString());

                    elemCharacters.AppendChild(elemChar);
                }
            }

            xmlDoc.Save(filename);
        }

        public static CFont Load(string filename)
        {
            CFont font = new CFont();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            XmlElement elemRoot = (XmlElement)xmlDoc.SelectSingleNode("Font");
            font.ImageFilename = XmlHelper.GetAttribute(elemRoot, "Image", "");
            font.FontEditImageFilename = font.ImageFilename;
            font.ImageId = XmlHelper.GetAttribute(elemRoot, "ImageId", "");
            font.IngameExportFile = XmlHelper.GetAttribute(elemRoot, "IngameExportFile", "");

            font.ImageWidth = XmlHelper.GetAttribute(elemRoot, "ImageWidth", 0); 
            font.ImageHeight = XmlHelper.GetAttribute(elemRoot, "ImageHeight", 0); 
            font.NumCharsWidth = XmlHelper.GetAttribute(elemRoot, "NumCharsWidth", 0); 
            font.NumCharsHeight = XmlHelper.GetAttribute(elemRoot, "NumCharsHeight", 0); 

            XmlElement elemChars = (XmlElement)elemRoot.SelectSingleNode("Characters");
            font.SpaceWidth = XmlHelper.GetAttribute(elemChars, "SpaceWidth", 0);
            font.BetweenCharsWidth = XmlHelper.GetAttribute(elemChars, "BetweenCharsWidth", 0);
            font.LineHeight = XmlHelper.GetAttribute(elemChars, "LineHeight", 0); 

            XmlNodeList elemCharacters = elemRoot.SelectNodes("Characters/Character");

            font.Characters = new CFont.FontData[font.NumCharsWidth * font.NumCharsHeight];

            int c = 0;
            foreach (XmlElement elemChar in elemCharacters)
            {
                if (c >= font.Characters.Length)
                {
                    break;
                }
                CFont.FontData data = new CFont.FontData();
                string ch = elemChar.GetAttribute("CharMap");
                if (!string.IsNullOrEmpty(ch))
                {
                    data.Character = Convert.ToChar(ch); 
                }

                int left = XmlHelper.GetAttribute(elemChar, "Left", 0);
                int top = XmlHelper.GetAttribute(elemChar, "Top", 0);
                int width = XmlHelper.GetAttribute(elemChar, "Width", 0);
                int height = XmlHelper.GetAttribute(elemChar, "Height", 0); 

                data.Rect = new Rectangle(left, top, width, height);
                data.Spacing = XmlHelper.GetAttribute(elemChar, "Spacing", 0);
                data.SpacingAfter = XmlHelper.GetAttribute(elemChar, "SpacingAfter", 0);
                font.Characters[c] = data;
                c++;
            }

            return font;
        }

       
    }
}
