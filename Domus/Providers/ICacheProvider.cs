using System;

namespace Domus.Providers
{
    /// <summary>
    /// Provider for cacheing
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets a given value from the cache
        /// </summary>
        /// <typeparam name="T">Type to get</typeparam>
        /// <param name="key">Unique identifer</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Puts a value into the cache
        /// </summary>
        /// <typeparam name="T">Type to put</typeparam>
        /// <param name="value">Value to put</param>
        /// <param name="key">Unique identifier of the item</param>
        /// <param name="expiration">How long until it expires</param>
        bool Put<T>(T value, string key, TimeSpan expiration);

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">Unique identifier</param>
        void Remove(string key);
    }
}