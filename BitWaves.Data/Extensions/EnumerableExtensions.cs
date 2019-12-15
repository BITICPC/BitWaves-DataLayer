using System;
using System.Collections.Generic;

namespace BitWaves.Data.Extensions
{
    /// <summary>
    /// 为 <see cref="IEnumerable{T}"/> 提供扩展方法。
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// 将 <see cref="IEnumerable{T}"/> 中的元素收集到 <see cref="HashSet{T}"/> 中。
        /// </summary>
        /// <param name="enumerable">包含迭代元素的 <see cref="IEnumerable{T}"/> 迭代器。</param>
        /// <typeparam name="T">元素对象类型。</typeparam>
        /// <returns>包含给定的 <see cref="IEnumerable{T}"/> 中全部不同元素的 <see cref="HashSet{T}"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> 为 null。</exception>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
        {
            Contract.NotNull(enumerable, nameof(enumerable));

            var set = new HashSet<T>();
            foreach (var el in enumerable)
            {
                set.Add(el);
            }

            return set;
        }
    }
}
