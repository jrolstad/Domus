using System;
using System.Collections.Generic;
using System.Linq;
using Directus.SimpleDb.Providers;
using Domus.Entities;

namespace Domus.Providers.Repositories
{
    /// <summary>
    /// Recipe data provider that uses the Amazon SimpleDb
    /// </summary>
    public class AmazonSimpleDbRecipeProvider:IRepository<Recipe,string>
    {
        internal SimpleDBProvider<Recipe, string> _provider;
        private readonly ICache _cache;
        public static readonly TimeSpan CacheDuration = new TimeSpan(0, 4, 0, 0);

        private const string recipeCachKey = "DomuRecipes";

        /// <summary>
        /// Constructor that consumes the underlying simpledb provider
        /// </summary>
        /// <param name="simpleDbProvider"></param>
        /// <param name="cache">Cache provider</param>
        internal AmazonSimpleDbRecipeProvider(SimpleDBProvider<Recipe, string> simpleDbProvider,ICache cache)
        {
            _provider = simpleDbProvider;
            _cache = cache;
        }

        /// <summary>
        /// Constructor security credentials
        /// </summary>
        /// <param name="accessKey">Access key</param>
        /// <param name="secretKey">Secret Key</param>
        /// <param name="cache">Cache provider</param>
        public AmazonSimpleDbRecipeProvider(string accessKey, string secretKey,ICache cache)
            : this(new SimpleDBProvider<Recipe, string>(accessKey, secretKey),cache)
        {
           
        }

        /// <summary>
        /// Obtains a single item
        /// </summary>
        /// <param name="identifier">Identifier to get the item for</param>
        /// <returns></returns>
        public Recipe Get( string identifier )
        {
            var recipesFromCache = _cache.Get<IEnumerable<Recipe>>(recipeCachKey);
            return recipesFromCache != null ? 
                recipesFromCache.FirstOrDefault(r => r.RecipeId == identifier) : 
                _provider.Get(identifier);
        }

        /// <summary>
        /// Obtains all of the items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Recipe> Get()
        {

            var recipesFromCache = _cache.Get<IEnumerable<Recipe>>(recipeCachKey);
            var recipes = (recipesFromCache ?? _provider.Get()).ToArray();

            if(recipesFromCache == null)
            {
                _cache.Put(recipes,recipeCachKey,CacheDuration);
            }

            return recipes;
        }

        /// <summary>
        /// Search for a particular item
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public IEnumerable<Recipe> Find( Func<Recipe, bool> filterCriteria )
        {
            return Get().Where(filterCriteria);
        }

        /// <summary>
        /// Saves a particular item
        /// </summary>
        /// <param name="item"></param>
        public void Save( Recipe item )
        {
            _provider.Save(new[]{item});
            _cache.Remove(recipeCachKey);
        }

        /// <summary>
        /// Deletes a given item
        /// </summary>
        /// <param name="identifier"></param>
        public void Delete( string identifier )
        {
            _provider.Delete(new[]{identifier});
            _cache.Remove(recipeCachKey);
        }

        /// <summary>
        /// Removes items from the cache
        /// </summary>
        public void Refresh()
        {
            _cache.Remove(recipeCachKey);
        }
    }
}