using System;
using System.Collections.Generic;
using System.Collections;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UIMenuItemCollection : IEnumerable
    {
        #region vars
        #endregion


        #region Properties
        public int Count
        {
            get { return this.Items.Count; }
        }

        private List<UIMenuItem> Items { get; set; }
        private UIMenuItemCollectionEnumerator Enumerator { get; set; }
        private UIMenu OwnerMenu;
        #endregion

        public UIMenuItem this[int i]
        {
            get { return this.Items[i]; }
        }

         /// <summary>
        /// 
        /// </summary>
        internal UIMenuItemCollection(UIMenu owner)
        {
            this.Items = new List<UIMenuItem>();
            this.Enumerator = new UIMenuItemCollectionEnumerator(this.Items);
            this.OwnerMenu = owner;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void Add(UIMenuItem menuItem)
        {
            if (menuItem != null)
            {
                this.Items.Add(menuItem);
                menuItem.Parent = this.OwnerMenu;
                menuItem.Parent.Controls.Add(menuItem);
                this.OwnerMenu.RepositionItems();
                menuItem.OnFocus += new UIControl.UIEvent(this.OwnerMenu.OnItemEnter);
                menuItem.OnLostFocus += new UIControl.UIEvent(this.OwnerMenu.OnItemLeave);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void Remove(UIMenuItem menuItem)
        {
            if (menuItem != null)
            {
                this.Items.Remove(menuItem);
                menuItem.Parent.Controls.Remove(menuItem);
                this.OwnerMenu.RepositionItems();
                menuItem.Parent = null;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (UIMenuItem menuItem in this.Items)
            {
                menuItem.Parent.Controls.Remove(menuItem);
                menuItem.Parent = null;
            }
            this.Items.Clear();
            this.OwnerMenu.RepositionItems();
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("Count: {0}", this.Items.Count);
        }

        
        #region IEnumerable
        public IEnumerator GetEnumerator()
        {
            return new UIMenuItemCollectionEnumerator(this.Items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Enumerator;
        }
        #endregion
    }

    
    #region Enumerator
    /// <summary>
    /// 
    /// </summary>
    public class UIMenuItemCollectionEnumerator : IEnumerator
    {
        public List<UIMenuItem> _items;
        int _position = -1;

        public UIMenuItemCollectionEnumerator( List<UIMenuItem> items)
        {
            this._items = items;
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _items.Count);
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
                    return _items[_position];
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
