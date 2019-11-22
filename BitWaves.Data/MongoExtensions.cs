using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BitWaves.Data
{
    /// <summary>
    /// 为 MongoDB 组件提供扩展方法。
    /// </summary>
    public static class MongoExtensions
    {
        /// <summary>
        /// 收集给定 <see cref="IAsyncCursor{T}"/> 中的数据到 <see cref="HashSet{T}"/> 中。
        /// </summary>
        /// <param name="cursor">指向目标数据的 <see cref="IAsyncCursor{T}"/> 对象。</param>
        /// <typeparam name="T">目标数据类型。</typeparam>
        /// <returns>收集到的 <see cref="HashSet{T}"/> 对象。</returns>
        internal static HashSet<T> ToHashSet<T>(this IAsyncCursor<T> cursor)
        {
            var set = new HashSet<T>();
            while (cursor.MoveNext())
            {
                foreach (var value in cursor.Current)
                {
                    set.Add(value);
                }
            }

            return set;
        }

        /// <summary>
        /// 测试给定的 MongoDB 数据集是否存在。
        /// </summary>
        /// <param name="collection">MongoDB 数据集。</param>
        /// <typeparam name="T">数据集中的数据类型。</typeparam>
        /// <returns>给定的数据集是否存在。</returns>
        internal static bool Exists<T>(this IMongoCollection<T> collection)
        {
            return collection.Database.ListCollectionNames().ToHashSet().Contains(
                collection.CollectionNamespace.CollectionName);
        }

        /// <summary>
        /// 获取给定的异步游标的所有数据，并将其从 BsonDocument 转换为相应的实体对象。
        /// </summary>
        /// <param name="source">包含数据的异步游标。</param>
        /// <typeparam name="TEntity">实体对象类型。</typeparam>
        /// <returns>包含数据的实体对象列表。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> 为 null。
        /// </exception>
        public static async Task<List<TEntity>> ToEntityListAsync<TEntity>(this IAsyncCursorSource<BsonDocument> source)
        {
            Contract.NotNull(source, nameof(source));

            var result = new List<TEntity>();

            var cursor = await source.ToCursorAsync();
            while (await cursor.MoveNextAsync())
            {
                result.AddRange(cursor.Current.Select(doc => BsonSerializer.Deserialize<TEntity>(doc)));
            }

            return result;
        }
    }
}
