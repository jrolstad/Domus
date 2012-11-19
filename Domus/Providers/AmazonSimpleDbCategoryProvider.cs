using System;
using System.Collections.Generic;
using System.Linq;
using Directus.SimpleDb.Providers;
using Domus.Entities;

namespace Domus.Providers
{
    public class AmazonSimpleDbCategoryProvider : IDataProvider<Category, string>
    {
        private readonly SimpleDBProvider<Category, string> _provider;
        private readonly ICacheProvider _cache;
        public static readonly TimeSpan CacheDuration = new TimeSpan(0, 4, 0, 0);

        private const string categoryCachKey = "DomusCategories";

        /// <summary>
        /// Constructor that consumes the underlying simpledb provider
        /// </summary>
        /// <param name="simpleDbProvider"></param>
        /// <param name="cache"> </param>
        internal AmazonSimpleDbCategoryProvider(SimpleDBProvider<Category, string> simpleDbProvider,ICacheProvider cache)
        {
            _provider = simpleDbProvider;
            _cache = cache;
        }

        /// <summary>
        /// Constructor security credentials
        /// </summary>
        /// <param name="accessKey">Access key</param>
        /// <param name="secretKey">Secret Key</param>
        /// <param name="cache"> </param>
        public AmazonSimpleDbCategoryProvider(string accessKey, string secretKey,ICacheProvider cache)
            : this(new SimpleDBProvider<Category, string>(accessKey, secretKey),cache)
        {
           
        }

        /// <summary>
        /// Obtains a single item
        /// </summary>
        /// <param name="identifier">Identifier to get the item for</param>
        /// <returns></returns>
        public Category Get(string identifier)
        {
            var cateogoriesInCached = _cache.Get<IEnumerable<Category>>(categoryCachKey);
            return cateogoriesInCached != null ?
                cateogoriesInCached.FirstOrDefault(r => r.CategoryId == identifier) :
                _provider.Get(identifier);
        }

        /// <summary>
        /// Obtains all of the items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> Get()
        {

            var cateogoriesInCached = _cache.Get<IEnumerable<Category>>(categoryCachKey);
            var categories = (cateogoriesInCached ?? _provider.Get()).ToArray();

            if (cateogoriesInCached == null)
            {
                _cache.Put(categories, categoryCachKey, CacheDuration);
            }

            return categories;
        }

        /// <summary>
        /// Search for a particular item
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public IEnumerable<Category> Search(Func<Category, bool> filterCriteria)
        {
            return Get().Where(filterCriteria);
        }

        /// <summary>
        /// Saves a particular item
        /// </summary>
        /// <param name="item"></param>
        public void Save(Category item)
        {
            _provider.Save(new[] { item });
            _cache.Remove(categoryCachKey);
        }

        /// <summary>
        /// Deletes a given item
        /// </summary>
        /// <param name="identifier"></param>
        public void Delete(string identifier)
        {
            _provider.Delete(new[] { identifier });
            _cache.Remove(categoryCachKey);
        }

        /// <summary>
        /// Removes items from the cache
        /// </summary>
        public void Refresh()
        {
            _cache.Remove(categoryCachKey);
        }
    }
}