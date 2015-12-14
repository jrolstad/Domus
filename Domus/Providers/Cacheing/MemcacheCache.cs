using System;
using System.Collections.Generic;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace Domus.Providers.Cacheing
{
    /// <summary>
    /// Cache provider for Membase
    /// </summary>
    public class MemcacheCache:ICache
    {
        private readonly MemcachedClient _cache;

        /// <summary>
        /// Constructor with arguments
        /// </summary>
        /// <param name="cache">Membase Cacher</param>
        public MemcacheCache(MemcachedClient cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Gets a given item
        /// </summary>
        /// <typeparam name="T">Type to get</typeparam>
        /// <param name="key">Unique identifer</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        /// <summary>
        /// Puts a value into the cache
        /// </summary>
        /// <typeparam name="T">Type to put</typeparam>
        /// <param name="value">Value to put</param>
        /// <param name="key">Unique identifier of the item</param>
        /// <param name="expiration">How long until it expires</param>
        public bool Put<T>(T value, string key, TimeSpan expiration)
        {
            return _cache.Store(StoreMode.Set, key, value, expiration);

        }

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">Unique identifier</param>
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

    }

    public class InMemoryCache:ICache
    {
        private Dictionary<string,object> _items = new Dictionary<string, object>();
         
        public T Get<T>(string key)
        {
            if (!_items.ContainsKey(key))
                return default(T);

            return (T) _items[key];
        }

        public bool Put<T>(T value, string key, TimeSpan expiration)
        {
            if(!_items.ContainsKey(key))
                _items.Add(key,value);

            _items[key] = value;

            return true;
        }

        public void Remove(string key)
        {
            if (_items.ContainsKey(key))
                _items.Remove(key);
        }
    }
}