using System.Collections.Generic;
using System.Linq;

namespace Domus.Adapters
{
    /// <summary>
    /// Converts items using the automapper
    /// </summary>
    /// <typeparam name="TFrom">What to convert from</typeparam>
    /// <typeparam name="TTo">What to convert to</typeparam>
    public class AutoMapperAdapter<TFrom,TTo>:IAdapter<TFrom,TTo>
    {
        private static object lockObject = new object();
        private static bool _isConfigured;

        /// <summary>
        /// Static constructor; creates a map
        /// </summary>
        static AutoMapperAdapter()
        {
            lock (lockObject)
            {
                if (!_isConfigured)
                {
                    AutoMapper.Mapper.CreateMap<TFrom, TTo>();
                    AutoMapper.Mapper.CreateMap<TTo, TFrom>();

                    _isConfigured = true;
                }
            }
        }

        /// <summary>
        /// Converts an enumerable from one type to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public virtual IEnumerable<TTo> Convert(IEnumerable<TFrom> from)
        {
            return from.Select(Convert);
        }

        /// <summary>
        /// Converts a single type from one to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public virtual TTo Convert(TFrom from)
        {
            return AutoMapper.Mapper.Map<TFrom, TTo>(from);
        }


    }
}