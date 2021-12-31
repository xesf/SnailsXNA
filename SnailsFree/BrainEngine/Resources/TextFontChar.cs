using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.BrainEngine.Resources
{
    public class TextFontChar : IDataFileSerializable
    {
        #region Member vars
        Char _charMap;
        Rectangle _rect;
        int _spacing;
        int _spacingAfter;
        #endregion

        #region Properties
        public Char Character
        {
            get { return this._charMap; }
        }
        public int Spacing
        {
            get { return this._spacing; }
        }
        public int SpacingAfter
        {
            get { return this._spacingAfter; }
        }
        public Rectangle Rectangle
        {
            get { return this._rect; }
        }
        #endregion

        #region Constructs
        /// <summary>
        /// 
        /// </summary>
        private TextFontChar()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public TextFontChar(char ch, Rectangle rect)
        {
            this._charMap = ch;
            this._rect = rect;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public static TextFontChar FromDataFile(DataFileRecord record)
        {
            TextFontChar newChar = new TextFontChar();
            newChar.InitFromDataFileRecord(record);
            return newChar;
        }


        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._charMap = record.GetFieldValue<char>("CharMap");
            int left = record.GetFieldValue<int>("Left");
            int right = record.GetFieldValue<int>("Top");
            int width = record.GetFieldValue<int>("Width");
            int height = record.GetFieldValue<int>("Height");
            this._rect = new Rectangle(left, right, width, height);
            this._spacing = record.GetFieldValue<int>("Spacing", 0);
            this._spacingAfter = record.GetFieldValue<int>("SpacingAfter", 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("Character");

            record.AddField("CharMap", this._charMap);
            record.AddField("Left", this._rect.Left);
            record.AddField("Top", this._rect.Top);
            record.AddField("Width", this._rect.Width);
            record.AddField("Height", this._rect.Height);
            record.AddField("Spacing", this._spacing);
            record.AddField("SpacingAfter", this._spacingAfter);


            return record;
        }

        #endregion
    }
}
