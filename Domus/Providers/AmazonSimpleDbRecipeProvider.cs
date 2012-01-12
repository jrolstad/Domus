using System;
using System.Collections.Generic;
using System.Linq;
using Directus.SimpleDb.Providers;
using Domus.Entities;

namespace Domus.Providers
{
    /// <summary>
    /// Recipe data provider that uses the Amazon SimpleDb
    /// </summary>
    public class AmazonSimpleDbRecipeProvider:IDataProvider<Recipe,string>
    {
        private SimpleDBProvider<Recipe, string> _provider;

        /// <summary>
        /// Constructor security credentials
        /// </summary>
        /// <param name="accessKey">Access key</param>
        /// <param name="secretKey">Secret Key</param>
        public AmazonSimpleDbRecipeProvider(string accessKey, string secretKey)
        {
            _provider = new SimpleDBProvider<Recipe, string>(accessKey, secretKey);
        }

        /// <summary>
        /// Obtains a single item
        /// </summary>
        /// <param name="identifier">Identifier to get the item for</param>
        /// <returns></returns>
        public Recipe Get( string identifier )
        {
            return _provider.Get(identifier);
        }

        /// <summary>
        /// Obtains all of the items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Recipe> Get()
        {
            return _provider.Get();
        }

        /// <summary>
        /// Search for a particular item
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public IEnumerable<Recipe> Search( Func<Recipe, bool> filterCriteria )
        {
            return _provider.Get().Where(filterCriteria);
        }

        /// <summary>
        /// Saves a particular item
        /// </summary>
        /// <param name="item"></param>
        public void Save( Recipe item )
        {
            _provider.Save(new[]{item});
        }

        /// <summary>
        /// Deletes a given item
        /// </summary>
        /// <param name="identifier"></param>
        public void Delete( string identifier )
        {
            _provider.Delete(new[]{identifier});
        }
    }
}