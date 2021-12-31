using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    public class DataFileRecordList : IEnumerable
    {
        List<DataFileRecord> _Items;

        public DataFileRecord this[int i]
        {
            get { return _Items[i]; }
            set { _Items[i] = value; }
        }

        private List<DataFileRecord> Items
        {
            get { return this._Items; }
            set { this._Items = value; }
        }

        public int Count
        {
            get { return this.Items.Count; }
        }

        public DataFileRecordList()
        {
            this.Items = new List<DataFileRecord>();
        }

        public void Add(DataFileRecord record)
        {
            this.Items.Add(record);
        }

        public void Clear()
        {
            this.Items.Clear();
        }

        public virtual DataFileRecord SelectRecord(string path)
        {
            PathManager pathManager = PathManager.Parse(path);
            foreach (DataFileRecord record in this.Items)
            {
                if (pathManager.HasSingleField && record.Name == pathManager.LastField)
                {
                    return record;
                }
                if (record.ChildRecords.Count > 0)
                {
                    DataFileRecord found = record.ChildRecords.SelectRecord(pathManager.RemoveFirst().Path);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }

        public virtual DataFileRecordList SelectRecords(string path)
        {
            DataFileRecordList records = new DataFileRecordList();
            PathManager pathManager = PathManager.Parse(path);

            if (pathManager.HasSingleField)
            {
                foreach (DataFileRecord record in this.Items)
                {
                    if (record.Name == pathManager.LastField)
                        records.Add(record);
                }
            }
            else
            {
                foreach (DataFileRecord record in this.Items)
                {
                    if (record.Name == pathManager.FirstField)
                    {
                        return record.ChildRecords.SelectRecords(pathManager.RemoveFirst().Path);
                    }
                }
            }

            return records;
        }
        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return new DataFileRecordListEnumerator(this._Items);
        }

        #endregion
    }

    #region DataFileRecordListEnumerator
    /// <summary>
    /// 
    /// </summary>
    public class DataFileRecordListEnumerator : IEnumerator
    {
        List<DataFileRecord> _Messages;
        int position = -1;

        public DataFileRecordListEnumerator(List<DataFileRecord> list)
        {
            _Messages = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _Messages.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public DataFileRecord Current
        {
            get
            {
                try
                {
                    return _Messages[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
    #endregion
}
