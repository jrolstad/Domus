using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase;
using Enyim.Caching.Memcached;

namespace Domus.Providers
{
    public class CouchDataProvider<T,I>:IDataProvider<T,I> where T : class
    {
        private readonly CouchbaseClient _client;

        public CouchDataProvider(string name, string password):this(new CouchbaseClient(name,password))
        {
  
        }

        public CouchDataProvider(CouchbaseClient client)
        {
            _client = client;
        }

        public T Get(I identifier)
        {
            return _client.Get<T>(identifier.ToString());
        }

        public IEnumerable<T> GetAll()
        {
            return _client.GetView("all", "all")
                .Select(r => r.GetItem() as T);
        }

        public IEnumerable<T> Search(Func<T, bool> filterCriteria)
        {
            return GetAll()
                .Where(filterCriteria);
        }

        public void Save(T item, I identifier)
        {
            _client.Store(StoreMode.Set, identifier.ToString(), item);
        }

        public void Delete(I identifier)
        {
            _client.Remove(identifier.ToString());
        }
    }
}