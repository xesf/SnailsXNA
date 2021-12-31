using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    public class DataFileField
    {
        #region Member vars
        string _Name;
        object _Value;
        #endregion

        #region Properties
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        public object Value
        {
            get { return this._Value; }
            set
            {
                if (value != null &&
                    value.GetType() != typeof(string) &&
                    value.GetType() != typeof(int) &&
                    value.GetType() != typeof(float) &&
                    value.GetType() != typeof(bool) &&
                    value.GetType() != typeof(double) &&
                    value.GetType() != typeof(long) &&
                    value.GetType() != typeof(char) &&
                    value.GetType() != typeof(TimeSpan) &&
                    value.GetType() != typeof(Microsoft.Xna.Framework.Color))
                {
                    throw new Exception("Type '" + value.GetType().Name + "' not supported by DataFiles.");
                }

                this._Value = value;
            }
        }

        public FieldType Type
        {
            get
            {
                if (this.Value == null)
                    return FieldType.Null;

                if (this.Value.GetType() == typeof(string))
                    return FieldType.Str;

                if (this.Value.GetType() == typeof(int))
                    return FieldType.Int;

                if (this.Value.GetType() == typeof(float))
                    return FieldType.Float;

                if (this.Value.GetType() == typeof(bool))
                    return FieldType.Bool;

                if (this.Value.GetType() == typeof(double))
                    return FieldType.Double;

                if (this.Value.GetType() == typeof(long))
                    return FieldType.Long;

                if (this.Value.GetType() == typeof(char))
                    return FieldType.Char;

                if (this.Value.GetType() == typeof(TimeSpan))
                    return FieldType.TimeSpan;

                if (this.Value.GetType() == typeof(Microsoft.Xna.Framework.Color))
                    return FieldType.XNAColor;

                return FieldType.NotSupported;
            }
        }
        #endregion

        #region Constructors
        public DataFileField(string name)
        {
            this.Name = name;
        }

        public DataFileField(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        #endregion

        #region Class overrides
        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return "    Name: " + this.Name + ", Value: " + this.Value.ToString() + "\n";
        }
        #endregion
    }
}
