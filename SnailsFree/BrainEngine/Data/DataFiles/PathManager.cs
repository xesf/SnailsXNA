using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    class PathManager
    {
        string[] _PathFields;

        public bool HasSingleField
        {
            get { return this._PathFields.Length == 1; }
        }

        public string LastField
        {
            get
            {
                if (this._PathFields.Length <= 0)
                    return null;

                return this._PathFields[this._PathFields.Length - 1];
            }
        }

        public string FirstField
        {
            get
            {
                if (this._PathFields.Length == 0)
                    return null;

                return this._PathFields[0];
            }
        }
        public string Path
        {
            get
            {
                string toReturn = "";
                for (int i = 0; i < this._PathFields.Length; i++)
                {
                    toReturn += this._PathFields[i];
                    if (i < this._PathFields.Length - 1)
                        toReturn += '\\';
                }

                return toReturn;
            }
        }

        public PathManager()
        {
        }

        public static PathManager Parse(string path)
        {
            PathManager manager = new PathManager();

            manager._PathFields = path.Split('\\');
            return manager;
        }


        public PathManager RemoveFirst()
        {
            string toReturn = "";
            for (int i = 1; i < this._PathFields.Length; i++)
            {
                toReturn += this._PathFields[i];
                if (i < this._PathFields.Length - 1)
                    toReturn += '\\';
            }

            return PathManager.Parse(toReturn);
        }

    }
}
