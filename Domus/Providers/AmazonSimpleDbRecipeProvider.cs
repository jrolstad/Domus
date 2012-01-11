using System;
using System.Collections.Generic;
using Domus.Entities;

namespace Domus.Providers
{
    public class AmazonSimpleDbRecipeProvider:IDataProvider<Recipe,string>
    {
        public AmazonSimpleDbRecipeProvider(string accessKey, string secretKey)
        {
            
        }
        /// <summary>
        /// Obtains a single item
        /// </summary>
        /// <param name="identifier">Identifier to get the item for</param>
        /// <returns></returns>
        public Recipe Get( string identifier )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains all of the items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Recipe> Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search for a particular item
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public IEnumerable<Recipe> Search( Func<Recipe, bool> filterCriteria )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a particular item
        /// </summary>
        /// <param name="item"></param>
        public void Save( Recipe item )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a given item
        /// </summary>
        /// <param name="identifier"></param>
        public void Delete( string identifier )
        {
            throw new NotImplementedException();
        }
    }
}