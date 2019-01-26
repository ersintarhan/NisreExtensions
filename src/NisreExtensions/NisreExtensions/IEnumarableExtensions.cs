using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class IEnumarableExtensions
    {
        /// <summary>
        ///     Checks whether collection equels to NULL or contains no items
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="collection">collection</param>
        /// <returns>true, if empty, otherwise false</returns>
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }


        /// <summary>
        ///     iterates over collection and aplies action on every item
        /// </summary>
        /// <typeparam name="T">Type of collection items</typeparam>
        /// <param name="list">iterative collection</param>
        /// <param name="modifier">action</param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> modifier)
        {
            if (list == null)
                return;

            foreach (var item in list)
                modifier(item);
        }


        /// <summary>
        ///     Converts one collection to another one by applying conversion function
        /// </summary>
        /// <typeparam name="T1">Type of input collection items</typeparam>
        /// <typeparam name="TResult">Type of output item collection</typeparam>
        /// <param name="list">iterative collection</param>
        /// <param name="modifier">converting function</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Convert<T1, TResult>(this IEnumerable<T1> list, Func<T1, TResult> modifier)
        {
            return list.Select(x => modifier(x));
        }

        /// <summary>
        ///     Merges two collections by applying selector function to elements with same index
        /// </summary>
        /// <typeparam name="TLeft">Type of input collection items</typeparam>
        /// <typeparam name="TRight">Type of output item collection</typeparam>
        /// <typeparam name="TResult">Type of result collection</typeparam>
        /// <param name="left">merging iterative collection</param>
        /// <param name="right">second iterative collection to merge</param>
        /// <param name="selector">function takes two appropriate items and produce one resulting items</param>
        /// <returns></returns>
        public static IEnumerable<TResult> JoinByIndex<TLeft, TRight, TResult>(this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, TResult> selector)
        {
            if (left == null || right == null)
                yield break;

            var ltor = left.GetEnumerator();
            var rtor = right.GetEnumerator();

            try
            {
                while (ltor.MoveNext() & rtor.MoveNext())
                    yield return selector(ltor.Current, rtor.Current);
            }
            finally
            {
                ltor.Dispose();
                rtor.Dispose();
            }
        }

        /// <summary>
        ///     Checks whether two itarative collections have identical items (include items count and places)
        /// </summary>
        /// <typeparam name="T">items type</typeparam>
        /// <param name="left">left collection</param>
        /// <param name="right">right collection</param>
        /// <returns>true - if collections are equel</returns>
        public static bool EquelsByIndex<T>(this IEnumerable<T> left, IEnumerable<T> right) where T : IEquatable<T>
        {
            if (left == null || right == null)
                return true;

            var ltor = left.GetEnumerator();
            var rtor = right.GetEnumerator();

            try
            {
                bool lp = false, rp = false;
                while ((lp = ltor.MoveNext()) & (rp = rtor.MoveNext()))
                    if (!ltor.Current.Equals(rtor.Current))
                        return false;

                return lp == rp;
            }
            finally
            {
                ltor.Dispose();
                rtor.Dispose();
            }
        }

        /// <summary>
        ///     Iterates over collection to element with given index.
        ///     Returns element or given default value if the index is out of bounds
        /// </summary>
        /// <typeparam name="T">Elemnts type</typeparam>
        /// <param name="list">list of elements</param>
        /// <param name="index">index</param>
        /// <param name="defaulValue">default value</param>
        /// <returns>element</returns>
        public static T ElementAtOrDefault<T>(this IEnumerable<T> list, int index, T defaulValue)
        {
            var currIdx = 0;
            foreach (var item in list)
                if (currIdx++ == index)
                    return item;

            return defaulValue;
        }

        /// <summary>
        ///     Returns the first index of element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="list">iterative collection</param>
        /// <param name="predicate"> A function to test each element for a condition.</param>
        /// <returns>The first index of element in the sequence that passes the test in the specified predicate function.</returns>
        public static int FirstIndex<T>(this IEnumerable<T> list, Func<T, bool> predicate) where T : IEquatable<T>
        {
            var i = 0;
            foreach (var x in list)
            {
                if (predicate(x))
                    return i;

                i++;
            }

            return -1;
        }


        public static IEnumerable<T> RemoveAllButFirstXItems<T>(this IEnumerable<T> collection, int itemsToKeep = 0)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var list = collection.ToList();
            if (itemsToKeep == 0)
                list.Clear();
            else if (itemsToKeep > list.Count)
                throw new InvalidOperationException(
                    $"Collection only contains <{list.Count}> items and we cannot prune it down to <{itemsToKeep}>");
            else
                list.RemoveRange(itemsToKeep, list.Count - itemsToKeep);
            return list;
        }

        public static IEnumerable<int> IndexesOfRepeats<T>(this IEnumerable<T> collection, int repeatCount = 2,
            Func<T, T, bool> equalityComparer = null)
        {
            var collectionAsList = collection as IList<T> ?? collection.ToList();
            if (!collectionAsList.Any())
                return new int[0];

            // if no equality comparer is given, use the default Equals operator
            if (equalityComparer == null)
                equalityComparer = (lhs, rhs) => lhs.Equals(rhs);

            var indexesOfRepeats = new List<int>();
            var currentRepeats = new List<T>();
            var itemSeen = default(T);

            var index = 0;
            var firstIndexOfRepeat = -1;
            foreach (var item in collectionAsList)
            {
                if (itemSeen != null && equalityComparer(itemSeen, item))
                {
                    currentRepeats.Add(itemSeen);
                    itemSeen = item;
                }
                else
                {
                    // lets check if repeats count is greater than the supplied parameter
                    if (currentRepeats.Count >= repeatCount)
                        indexesOfRepeats.Add(firstIndexOfRepeat);

                    itemSeen = item;
                    firstIndexOfRepeat = index;
                    currentRepeats.Clear();
                    currentRepeats.Add(itemSeen);
                }

                index++;
            }

            // lets check if we have any left over repeats
            if (currentRepeats.Count >= repeatCount)
                indexesOfRepeats.Add(firstIndexOfRepeat);

            return indexesOfRepeats;
        }
    }
}