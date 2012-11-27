using System.Collections.Generic;

namespace Domus.Mappers
{
    /// <summary>
    /// Adapter interface
    /// </summary>
    /// <typeparam name="TIn">Item adapting from</typeparam>
    /// <typeparam name="TOut">Item adapting to</typeparam>
    public interface IMapper<in TIn, out TOut>
    {
        /// <summary>
        /// Convert a collection from one type to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        IEnumerable<TOut> Map(IEnumerable<TIn> from);

        /// <summary>
        /// Convert a single item from one thing to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        TOut Map(TIn from);

    }
}