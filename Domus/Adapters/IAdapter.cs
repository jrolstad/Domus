using System.Collections.Generic;

namespace Domus.Adapters
{
    /// <summary>
    /// Adapter interface
    /// </summary>
    /// <typeparam name="From">Item adapting from</typeparam>
    /// <typeparam name="To">Item adapting to</typeparam>
    public interface IAdapter<From,To>
    {
        /// <summary>
        /// Convert a collection from one type to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        IEnumerable<To> Convert(IEnumerable<From> from);

        /// <summary>
        /// Convert a single item from one thing to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        To Convert(From from);

    }
}