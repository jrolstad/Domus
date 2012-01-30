using System;
using System.Collections.Generic;
using System.Linq;
using Directus.SimpleDb.Providers;
using Domus.Entities;

namespace Domus.Providers
{
    public class AmazonSimpleDbUserProvider:IDataProvider<User,string>
    {
        private readonly SimpleDBProvider<User, string> _provider;

        /// <summary>
        /// Constructor that consumes the underlying simpledb provider
        /// </summary>
        /// <param name="simpleDbProvider"></param>
        internal AmazonSimpleDbUserProvider(SimpleDBProvider<User, string> simpleDbProvider)
        {
            _provider = simpleDbProvider;
        }

        /// <summary>
        /// Constructor security credentials
        /// </summary>
        /// <param name="accessKey">Access key</param>
        /// <param name="secretKey">Secret Key</param>
        public AmazonSimpleDbUserProvider(string accessKey, string secretKey)
            : this(new SimpleDBProvider<User, string>(accessKey, secretKey))
        {
           
        }

        /// <summary>
        /// Obtains a single item
        /// </summary>
        /// <param name="identifier">Identifier to get the item for</param>
        /// <returns></returns>
        public User Get( string identifier )
        {
            return _provider.Get(identifier);
        }

        /// <summary>
        /// Obtains all of the items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> Get()
        {
            return _provider.Get();
        }

        /// <summary>
        /// Search for a particular item
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public IEnumerable<User> Search( Func<User, bool> filterCriteria )
        {
            return Get().Where(filterCriteria);
        }

        /// <summary>
        /// Saves a particular item
        /// </summary>
        /// <param name="item"></param>
        public void Save( User item )
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

        /// <summary>
        /// Removes items from the cache
        /// </summary>
        public void Refresh()
        {
           
        }
    }
}