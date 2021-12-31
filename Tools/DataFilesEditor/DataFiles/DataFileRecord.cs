using System;
using System.Collections.Generic;
using System.Globalization;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    public class DataFileRecord
    {
        #region Member vars
        List<DataFileField> _Fields;
        DataFileRecordList _ChildRecords;
        DataFileRecord _Parent;
        string _Name;
        #endregion

        #region Properties
        DataFileRecord Parent
        {
            get { return this._Parent; }
            set { this._Parent = value; }
        }

        public List<DataFileField> Fields
        {
            get { return this._Fields; }
            private set { this._Fields = value; }
        }

        public DataFileRecordList ChildRecords
        {
            get { return this._ChildRecords; }
            private set { this._ChildRecords = value; }
        }

        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        private int Level
        {
            get
            {
                int Level = 1;
                DataFileRecord record = this.Parent;
                while (record != null)
                {
                    Level++;
                    record = record.Parent;
                }

                return Level;
            }

        }
        private int Ident
        {
            get
            {
                return Level * 5;
            }
        }
        #endregion

        #region Operator overloading
        public DataFileField this[string fieldName]
        {
            set
            {
                for (int i = 0; i < this.Fields.Count; i++)
                {
                    if (this.Fields[i].Name == fieldName)
                    {
                        this.Fields[i] = value;
                    }
                }
            }
            get
            {
                return this.GetFieldByName(fieldName);
            }
        }
        #endregion

        #region Constructors
        public DataFileRecord()
        {
			this.Fields = new List<DataFileField>();
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord(string name)
        {
            this.CheckName(name);
            this._ChildRecords = new DataFileRecordList();
            this._Fields = new List<DataFileField>();
            this._Name = name;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// 
        /// </summary>
        public void AddRecord(DataFileRecord record)
        {
            record.Parent = this;
            this.ChildRecords.Add(record);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord AddRecord(string name)
        {
            DataFileRecord record = new DataFileRecord(name);
            this.AddRecord(record);
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRecord(string name, string fieldName, object fieldValue)
        {
            DataFileRecord record = new DataFileRecord(name);
            this.AddRecord(record);
            record.AddField(fieldName, fieldValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddField(DataFileField field)
        {
            this.Fields.Add(field);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddField(string name, object value)
        {
            this.AddField(new DataFileField(name, value));
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveField(string name)
        {
            this._Fields.Remove(this[name]);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord SelectRecord(string path)
        {
            return this.ChildRecords.SelectRecord(path);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecordList SelectRecords(string path)
        {
            return this.ChildRecords.SelectRecords(path);
        }

        /// <summary>
        /// Selects a record with by field name/value
        /// </summary>
        public DataFileRecord SelectRecordByField(string path, string fieldName, object fieldValue)
        {
            DataFileRecordList records = this.ChildRecords.SelectRecords(path);

            foreach (DataFileRecord record in records)
            {
                DataFileField field = record.GetFieldByName(fieldName);
                if (field != null)
                {
                    // Only supports strings for now...
                    if (fieldValue is String && field.Value is String)
                    {
                        if ((string)field.Value == (string)fieldValue)
                        {
                            return record;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public T GetChildRecordField<T>(string recordName, string fieldName, T defaultVal)
        {
            DataFileRecord record = this.SelectRecord(recordName);
            if (record == null)
            {
                return defaultVal;
            }

            return record.GetFieldValue<T>(fieldName, defaultVal);
        }

        /// <summary>
        /// 
        /// </summary>
        public T GetFieldValue<T>(string fieldName, T defaultVal)
        {
            DataFileField field = this.GetFieldByName(fieldName);
            if (field == null)
                return defaultVal;
            return this.GetFieldValue<T>(fieldName);
        }

        /// <summary>
        /// 
        /// </summary>
        public T GetFieldValue<T>(string fieldName)
        {
            DataFileField field = this.GetFieldByName(fieldName);
            if (field == null)
                return default(T);

            if (field.Value == null)
            {
                return default(T);
            }
            if (typeof(T) == field.Value.GetType())
            {
                return (T)field.Value;
            }

            object ret = default(T);
            // String
            if (typeof(T) == typeof(string))
            {
                return (T)(object)(field.Value.ToString());
            }
            // Int
            if (typeof(T) == typeof(int))
            {
                return (T)(object)(this.GetIntValue(field));
            }
                
            // long
            if (typeof(T) == typeof(long))
            {
                return (T)(object)(this.GetLongValue(field));
            }

            // float
            if (typeof(T) == typeof(float))
            {
                return (T)(object)(this.GetFloatValue(field));
            }

            // bool
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)(this.GetBoolValue(field));
            }

            // char
            if (typeof(T) == typeof(char))
            {
                return (T)(object)(this.GetCharValue(field));
            }

            // TimeSpan
            if (typeof(T) == typeof(TimeSpan))
            {
                return (T)(object)(this.GetTimeSpanValue(field));
            }

            // double
            if (typeof(T) == typeof(double))
            {
                return (T)(object)(this.GetDoubleValue(field));
            }

            // FlagsType
            if (typeof(T) == typeof(FlagsType))
            {
                return (T)(object)(this.GetFlagsTypeValue(field));
            }

            // Color
            if (typeof(T) == typeof(Microsoft.Xna.Framework.Color))
            {
                return (T)(object)(this.GetColorValue(field));
            }
            return (T)ret;
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Improve this later, fields should check this too
        /// </summary>
        private void CheckName(string name)
        {
            if (name.Contains("\\") || name.Contains("//") || name.Contains(">") || name.Contains("<"))
            {
                throw new DataFileFormatException(@"Record name cannot contain characters '\', '/', '>' or '<'.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private DataFileField GetFieldByName(string name)
        {
            foreach (DataFileField field in this.Fields)
            {
                if (field.Name == name)
                    return field;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        private int GetIntValue(DataFileField field)
        {
            if (field.Value.GetType() == typeof(int))
            {
                return (int)field.Value;
            }

            if (field.Value.GetType() == typeof(string) ||
                field.Value.GetType() == typeof(float)  ||
                field.Value.GetType() == typeof(double) ||
                field.Value.GetType() == typeof(bool))
            {
                return Convert.ToInt32(field.Value);
            }

            throw new DataFileFormatException("Invalid DataType while converting Field value to int.");
        }

        /// <summary>
        /// 
        /// </summary>
        private long GetLongValue(DataFileField field)
        {
            if (field.Value.GetType() == typeof(long))
            {
                return (long)field.Value;
            }

            if (field.Value.GetType() == typeof(string) ||
                field.Value.GetType() == typeof(int) ||
                field.Value.GetType() == typeof(float) ||
                field.Value.GetType() == typeof(double) ||
                field.Value.GetType() == typeof(bool))
            {
                return Convert.ToInt64(field.Value);
            }

            throw new DataFileFormatException("Invalid DataType while converting Field value to long.");
        }

        /// <summary>
        /// 
        /// </summary>
        private float GetFloatValue(DataFileField field)
        {
            if (field.Value.GetType() == typeof(float))
            {
                return (float)(field.Value);
            }
            if (field.Value.GetType() == typeof(double))
            {
                return (float)Convert.ToDecimal(field.Value);
            }
            if (field.Value.GetType() == typeof(string))
            {
                string str = field.Value.ToString().Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                str = str.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                return (float)Convert.ToDouble(str, CultureInfo.CurrentCulture);
            }
            if (field.Value.GetType() == typeof(int) ||
                field.Value.GetType() == typeof(bool))
            {
                return (float)Convert.ToDouble(field.Value);
            }

            throw new DataFileFormatException("Invalid DataType while converting Field value to float.");
        }

        /// <summary>
        /// 
        /// </summary>
        private double GetDoubleValue(DataFileField field)
        {
            if (field.Value.GetType() == typeof(double))
            {
                return (double)(field.Value);
            }
            if (field.Value.GetType() == typeof(float))
            {
                return (double)Convert.ToDecimal(field.Value);
            }
            if (field.Value.GetType() == typeof(string))
            {
                string str = field.Value.ToString().Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                str = str.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                return (double)Convert.ToDouble(str, CultureInfo.CurrentCulture);
            }
            if (field.Value.GetType() == typeof(int) ||
                field.Value.GetType() == typeof(bool))
            {
                return (double)Convert.ToDouble(field.Value);
            }

            throw new DataFileFormatException("Invalid DataType while converting Field value to double.");
        }

        /// <summary>
        /// FlagsType acepts strings or ints
        /// A string representation may be "1|2|4|8"
        /// </summary>
        private FlagsType GetFlagsTypeValue(DataFileField field)
        {
            if (field.Value == null)
            {
                return FlagsType.Zero;
            }

            string [] flagsStr = field.Value.ToString().Split('|');
            FlagsType flag = new FlagsType();
            foreach(string s in flagsStr)
            {
                int i;
                if (int.TryParse(s, out i) == false)
                {
                    throw new DataFileFormatException("Invalid DataType while converting Field value to FlagsType. Expecting ints separated by '|', received " + flagsStr);
                }
                flag.Value += i;
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool GetBoolValue(DataFileField field)
        {

            if (field.Value.GetType() == typeof(bool))
            {
                return (bool)field.Value;
            }

            if (field.Value.GetType() == typeof(float) ||
                field.Value.GetType() == typeof(int))
            {
                return Convert.ToBoolean(field.Value);
            }

            if (field.Value.GetType() == typeof(string))
            {
                bool bVal;
                bool.TryParse(field.Value.ToString(), out bVal);
                return bVal;
            }

            throw new DataFileFormatException("Invalid DataType while converting Field value to bool.");
        }

        /// <summary>
        /// 
        /// </summary>
        private char GetCharValue(DataFileField field)
        {
            if (field.Value.GetType() == typeof(char))
            {
                return (char)field.Value;
            }

            if (field.Value.GetType() == typeof(float) ||
                field.Value.GetType() == typeof(double) ||
                field.Value.GetType() == typeof(int) ||
                field.Value.GetType() == typeof(string))
            {
                return Convert.ToChar(field.Value);
            }

            //throw new ApplicationException("Invalid DataType while converting Field value to bool.");
            throw new DataFileFormatException("Invalid DataType while converting Field value to bool.");
        }

        /// <summary>
        /// 
        /// </summary>
        private Microsoft.Xna.Framework.Color GetColorValue(DataFileField field)
        {
            if (field.Value.GetType() == typeof(Microsoft.Xna.Framework.Color))
            {
                return (Microsoft.Xna.Framework.Color)field.Value;
            }
            if (field.Value.GetType() == typeof(string))
            {
                return this.ParseColor(field.Value.ToString());
            }

            throw new DataFileFormatException("Invalid DataType while converting Field value to TimeSpan.");
        }

        /// <summary>
        /// "{R:0 G:0 B:0 A:128}"
        /// </summary>
        private Microsoft.Xna.Framework.Color ParseColor(string colorString)
        {
            colorString = colorString.Replace("{", "");
            colorString = colorString.Replace("}", "");

            int [] rgba = new int[4];
            string[] clrs = colorString.Split(' ');
            for (int i = 0; i < 4; i++)
            {
                string [] clr = clrs[i].Split(':');
                rgba[i] = Convert.ToInt32(clr[1]);
            }

            return new Microsoft.Xna.Framework.Color(rgba[0], rgba[1], rgba[2], rgba[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        private TimeSpan GetTimeSpanValue(DataFileField field)
        {
            if (field.Value.GetType() == typeof(TimeSpan))
            {
                return (TimeSpan)field.Value;
            }
            if (field.Value.GetType() == typeof(string))
            {
                return TimeSpan.Parse((string)field.Value);
            }

            //throw new ApplicationException("Invalid DataType while converting Field value to bool.");
            throw new Exception("Invalid DataType while converting Field value to TimeSpan.");
        }

        #endregion

        #region Class overrides
        public override string ToString()
        {
            string str = "";

            str += "RECORD\n";
            str += "  Name  : " + this.Name + "\n";
            str += "  Parent: " + (this.Parent == null ? "ROOT" : this.Parent.Name) + "\n";
            str += "  FIELDS\n";
            foreach (DataFileField field in this.Fields)
            {
                str += field.ToString();
            }

            foreach (DataFileRecord record in this.ChildRecords)
            {
                str += record.ToString();
            }

            return str;
        }
        #endregion
    }
}
