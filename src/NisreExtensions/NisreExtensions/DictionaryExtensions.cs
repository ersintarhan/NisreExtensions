using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class DictionaryExtensions
    {
        /// <summary>
        ///     Enumerable To Dictionary conversion
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="coll">enumerable collection</param>
        /// <returns>dictionary</returns>
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> coll)
        {
            return coll.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}