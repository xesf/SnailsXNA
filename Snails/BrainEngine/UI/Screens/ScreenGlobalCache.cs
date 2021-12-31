using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.UI.Screens
{
    public class ScreenGlobalCache
    {
        private Dictionary<string, object> Items
        {
            get;
            set;
        }

        public object this[string key]
        {
            get 
            {
                if (this.Items.ContainsKey(key))
                {
                    return this.Items[key];
                }
                return null; 
            }
        }

        internal ScreenGlobalCache()
        {
            this.Items = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Set(string key, object data)
        {
            if (this.Items.ContainsKey(key))
            {
                this.Items[key] = data;
            }
            else
            {
                this.Items.Add(key, data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove(string key)
        {
            if (this.Items.ContainsKey(key))
            {
                this.Items.Remove(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            this.Items.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public T Get<T>(string key, T defaultVal)
        {
            if (this.Items.ContainsKey(key))
            {
                return (T)this.Items[key];
            }

            return defaultVal;
            
        }

        /// <summary>
        /// 
        /// </summary>
        public T Get<T>(string key)
        {
            if (this.Items.ContainsKey(key))
            {
                return (T)this.Items[key];
            }

            //throw new BrainException("Key '" + key + "' not found in Global Cache");

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(string key)
        {
            return this.Items.ContainsKey(key);
        }
    }
}
