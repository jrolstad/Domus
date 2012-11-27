using System.Collections.Generic;
using System.Linq;

namespace Domus.Mappers
{
    /// <summary>
    /// Converts items using the automapper
    /// </summary>
    /// <typeparam name="F">What to convert from</typeparam>
    /// <typeparam name="T">What to convert to</typeparam>
    public class AutoMapperMapper<F,T>:IMapper<F,T>
    {
        private static readonly object lockObject = new object();
        private static readonly bool _isConfigured;

        /// <summary>
        /// Static constructor; creates a map
        /// </summary>
        static AutoMapperMapper()
        {
            lock (lockObject)
            {
                if (!_isConfigured)
                {
                    AutoMapper.Mapper.CreateMap<F, T>();
                    _isConfigured = true;
                }
            }
        }

        /// <summary>
        /// Converts an enumerable from one type to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Map(IEnumerable<F> from)
        {
            return from
                .AsParallel()
                .Select(Map);
        }

        /// <summary>
        /// Converts a single type from one to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public virtual T Map(F from)
        {
            return AutoMapper.Mapper.Map<F, T>(from);
        }


    }
}