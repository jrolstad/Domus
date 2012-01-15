using System;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace Domus.Providers
{
    /// <summary>
    /// Cache provider for Membase
    /// </summary>
    public class MemcacheCache:ICacheProvider
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
}