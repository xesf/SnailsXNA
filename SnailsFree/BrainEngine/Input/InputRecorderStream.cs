using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Input
{
    public class InputRecorderStream
    {
        public class StreamItem
        {
            public double _milliseconds;
            public ulong _inputAction;
            public Vector2 _position;

            public StreamItem()
            {
            }

            public StreamItem(double milliseconds, ulong inputAction, Vector2 position)
            {
                this._milliseconds = milliseconds;
                this._inputAction = inputAction;
                this._position = position;
            }

        }

        private List<StreamItem> _items;
        public int ItemsCount { get { return this._items.Count; } }

        public StreamItem this[int key]
        {
            get
            {
                return this._items[key];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public InputRecorderStream()
        {
            this._items = new List<StreamItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Append(double milliseconds, ulong action, Vector2 position)
        {
            if (this._items.Count > 0)
            {
                // Ignore if it is equal to the previous item
                if (this._items[this._items.Count - 1]._inputAction == action &&
                    this._items[this._items.Count - 1]._position == position)
                {
                    return;
                }
            }
          
            this._items.Add(new StreamItem(milliseconds, action, position));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            this._items.Clear();
        }

    }
}
