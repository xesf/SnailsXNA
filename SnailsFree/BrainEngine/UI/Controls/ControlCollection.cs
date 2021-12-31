using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.UI.Screens;
using System.Collections;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class ControlCollection : IEnumerable
    {

        #region vars
        private List<UIControl> _items;
        ControlCollectionEnumerator _enumerator;
        IUIControl _ownerControl;
        #endregion

        #region Properties
        public int Count
        {
            get { return this._items.Count; }
        }

        #endregion

        public UIControl this[int i]
        {
            get { return this._items[i]; }
        }

        /// <summary>
        /// 
        /// </summary>
        internal ControlCollection(IUIControl owner)
        {
            this._items = new List<UIControl>();
            this._enumerator = new ControlCollectionEnumerator(this._items);
            this._ownerControl = owner;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(UIControl control)
        {
            if (control != null)
            {
                if (this._ownerControl is UIControl)
                {
                    if (((UIControl)this._ownerControl).InvokeOnBeforeControlAdded(control) == false)
                    {
                        return;
                    }
                }
                this._items.Add(control);
                control.Parent = this._ownerControl;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove(UIControl control)
        {
            if (control != null)
            {
                this._items.Remove(control);
                control.Parent = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAt(int idx)
        {
            UIControl control = this._items[idx];
            this._items.RemoveAt(idx);
            control.Parent = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Insert(int idx, UIControl control)
        {
            if (control != null)
            {
                this._items.Insert(idx, control);
                control.Parent = this._ownerControl;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (UIControl control in this._items)
            {
                control.Parent = null;
            }
            this._items.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(UIControl control)
        {
            if (control != null)
            {
                foreach (UIControl ctl in this._items)
                {
                    if (control == ctl)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InsertAt(int index, UIControl control)
        {
            if (control != null)
            {
                this._items.Insert(index, control);
                control.Parent = this._ownerControl;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("Count: {0}", this._items.Count);
        }

        #region IEnumerable
        public IEnumerator GetEnumerator()
        {
            return new ControlCollectionEnumerator(this._items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._enumerator;
        }
        #endregion


    }

    #region Enumerator
    /// <summary>
    /// 
    /// </summary>
    public class ControlCollectionEnumerator : IEnumerator
    {
        public List<UIControl> _controls;
        int _position = -1;

        public ControlCollectionEnumerator( List<UIControl> controls)
        {
            this._controls = controls;
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _controls.Count);
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public UIControl Current
        {
            get
            {
                try
                {
                    return _controls[_position];
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
