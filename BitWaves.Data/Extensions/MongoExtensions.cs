using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BitWaves.Data.DML;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BitWaves.Data.Extensions
{
    /// <summary>
    /// 为 MongoDB 组件提供扩展方法。
    /// </summary>
    internal static class MongoExtensions
    {
        /// <summary>
        /// 收集给定 <see cref="IAsyncCursor{T}"/> 中的数据到 <see cref="HashSet{T}"/> 中。
        /// </summary>
        /// <param name="source">指向目标数据的 <see cref="IAsyncCursor{T}"/> 对象。</param>
        /// <typeparam name="T">目标数据类型。</typeparam>
        /// <returns>收集到的 <see cref="HashSet{T}"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 null。</exception>
        public static async Task<HashSet<T>> ToHashSetAsync<T>(this IAsyncCursorSource<T> source)
        {
            Contract.NotNull(source, nameof(source));

            var set = new HashSet<T>();
            var cursor = await source.ToCursorAsync();
            while (await cursor.MoveNextAsync())
            {
                foreach (var value in cursor.Current)
                {
                    set.Add(value);
                }
            }

            return set;
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

        /// <summary>
        /// 获取给定的 <see cref="IAsyncCursorSource{BsonDocument"/> 异步游标所迭代的到的第一个 <see cref="BsonDocument"/>
        /// 对象，并将该对象反序列化为给定的实体对象类型。
        /// </summary>
        /// <param name="source">包含数据的异步游标。</param>
        /// <typeparam name="TEntity">要转换到的实体对象类型。</typeparam>
        /// <returns>
        /// 给定的异步游标所迭代到的第一个实体对象。如果给定的异步游标无法迭代出任何对象，返回
        /// <code>default(<typeparamref name="TEntity"/>)</code>。
        /// </returns>
        public static async Task<TEntity> FirstEntityOrDefaultAsync<TEntity>(
            this IAsyncCursorSource<BsonDocument> source)
        {
            Contract.NotNull(source, nameof(source));

            var bsonDoc = await source.FirstOrDefaultAsync();
            if (Equals(bsonDoc, default(TEntity)))
            {
                return default;
            }

            return BsonSerializer.Deserialize<TEntity>(bsonDoc);
        }

        /// <summary>
        /// 将给定的查找会话中找到的实体对象收集到 <see cref="HashSet{T}"/> 中。
        /// </summary>
        /// <param name="find">查找会话。</param>
        /// <typeparam name="TEntity">实体对象类型。</typeparam>
        /// <typeparam name="TDocument">查找会话的结果对象类型。</typeparam>
        /// <returns>包含给定的查找会话中的实体对象的 <see cref="HashSet{T}"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="find"/> 为 null。</exception>
        public static async Task<HashSet<TDocument>> ToHashSetAsync<TEntity, TDocument>(
            this IFindFluent<TEntity, TDocument> find)
        {
            Contract.NotNull(find, nameof(find));

            return await ToHashSetAsync((IAsyncCursorSource<TDocument>) find);
        }

        /// <summary>
        /// 从给定的查询会话中读取所有的 <see cref="BsonDocument"/> 对象，将其转换为给定的实体对象后作为列表返回。
        /// </summary>
        /// <param name="find">查询会话。</param>
        /// <typeparam name="TEntity">实体对象类型。</typeparam>
        /// <returns>包含查询结果的实体对象列表。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="find"/> 为 null。</exception>
        public static async Task<List<TEntity>> ToEntityListAsync<TEntity>(
            this IFindFluent<TEntity, BsonDocument> find)
        {
            Contract.NotNull(find, nameof(find));

            return await ToEntityListAsync<TEntity>((IAsyncCursorSource<BsonDocument>) find);
        }

        /// <summary>
        /// 在给定的查询会话上执行分页操作。
        /// </summary>
        /// <param name="find">要执行分页操作的查询会话。</param>
        /// <param name="pagination">分页操作参数。</param>
        /// <typeparam name="TEntity">实体对象类型。</typeparam>
        /// <typeparam name="TProjection">映射目标对象类型。</typeparam>
        /// <returns>应用了分页操作的查询会话。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="find"/> 为 null
        ///     或
        ///     <paramref name="pagination"/> 为 null。
        /// </exception>
        public static IFindFluent<TEntity, TProjection> Paginate<TEntity, TProjection>(
            this IFindFluent<TEntity, TProjection> find,
            Pagination pagination)
        {
            Contract.NotNull(find, nameof(find));
            Contract.NotNull(pagination, nameof(pagination));

            return find.Skip(pagination.Skip)
                       .Limit(pagination.ItemsPerPage);
        }

        /// <summary>
        /// 在给定的 <see cref="IMongoCollection{TEntity}"/> 上创建一个空的查询会话。
        /// </summary>
        /// <param name="collection">MongoDB 数据集。</param>
        /// <typeparam name="TEntity">要查询的实体对象类型。</typeparam>
        /// <returns>在给定的 <see cref="IMongoCollection{TEntity}"/> 上创建的空的查询会话。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> 为 null。</exception>
        public static IFindFluent<TEntity, TEntity> Find<TEntity>(this IMongoCollection<TEntity> collection)
        {
            Contract.NotNull(collection, nameof(collection));

            return collection.Find(Builders<TEntity>.Filter.Empty);
        }

        /// <summary>
        /// 在给定的 <see cref="IMongoCollection{TEntity}"/> 上创建一个查询会话，使用给定的
        /// <see cref="FilterBuilder{TEntity}"/> 对象创建的筛选器。
        /// </summary>
        /// <param name="collection">MongoDB 数据集。</param>
        /// <param name="filterBuilder">包含筛选器定义的 <see cref="FilterBuilder{TEntity}"/> 对象。</param>
        /// <typeparam name="TEntity">要查询的实体对象类型。</typeparam>
        /// <returns>在给定的 <see cref="IMongoCollection{TEntity}"/> 上创建的查询会话。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="collection"/> 为 null
        ///     或
        ///     <paramref name="filterBuilder"/> 为 null。
        /// </exception>
        public static IFindFluent<TEntity, TEntity> Find<TEntity>(this IMongoCollection<TEntity> collection,
                                                                  FilterBuilder<TEntity> filterBuilder)
        {
            Contract.NotNull(collection, nameof(collection));
            Contract.NotNull(filterBuilder, nameof(filterBuilder));

            return collection.Find(filterBuilder.CreateFilterDefinition());
        }

        /// <summary>
        /// 在给定的 MongoDB 数据集上执行给定的查询管道。
        /// </summary>
        /// <param name="collection">需要执行查询的 MongoDB 数据集。</param>
        /// <param name="pipeline">查询管道。</param>
        /// <typeparam name="TEntity">实体对象类型。</typeparam>
        /// <returns>查询结果。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="collection"/> 为 null
        ///     或
        ///     <paramref name="pipeline"/> 为 null。
        /// </exception>
        public static async Task<FindResult<TEntity>> FindAsync<TEntity>(this IMongoCollection<TEntity> collection,
                                                                         FindPipeline<TEntity> pipeline)
        {
            Contract.NotNull(collection, nameof(collection));
            Contract.NotNull(pipeline, nameof(pipeline));

            return await pipeline.ExecuteAsync(collection);
        }

        public static async Task<long> CountDistinctByAsync<TEntity, TField>(
            this IMongoCollection<TEntity> collection,
            FilterDefinition<TEntity> filter,
            Expression<Func<TEntity, TField>> field)
        {
            Contract.NotNull(collection, nameof(collection));
            Contract.NotNull(filter, nameof(filter));
            Contract.NotNull(field, nameof(field));

            var countResult = await collection.Aggregate()
                                              .Match(filter)
                                              .Group(field, g => new { Id = g.Key })
                                              .Count()
                                              .FirstOrDefaultAsync();
            return countResult?.Count ?? 0;
        }
    }
}
