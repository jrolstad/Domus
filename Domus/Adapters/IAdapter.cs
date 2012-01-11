using System.Collections.Generic;

namespace Domus.Adapters
{
    /// <summary>
    /// Adapter interface
    /// </summary>
    /// <typeparam name="F">Item adapting from</typeparam>
    /// <typeparam name="T">Item adapting to</typeparam>
    public interface IAdapter<F,T>
    {
        /// <summary>
        /// Convert a collection from one type to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        IEnumerable<T> Convert(IEnumerable<F> from);

        /// <summary>
        /// Convert a single item from one thing to another
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        T Convert(F from);

    }
}