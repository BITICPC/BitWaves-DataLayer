using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BitWaves.Data.DML;
using BitWaves.Data.Extensions;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 为不可更新的实体对象集提供抽象基类。
    /// </summary>
    /// <typeparam name="TEntity">实体对象类型。</typeparam>
    /// <typeparam name="TKey">实体对象的键类型。</typeparam>
    /// <typeparam name="TFilterBuilder">实体对象的筛选器的建造器类型。</typeparam>
    /// <typeparam name="TFindPipeline">实体对象的查询管道类型。</typeparam>
    public abstract class ImmutableEntityRepository<TEntity, TKey, TFilterBuilder, TFindPipeline>
        : IImmutableEntityRepository<TEntity, TKey, TFilterBuilder, TFindPipeline>
        where TEntity: class
        where TFilterBuilder: FilterBuilder<TEntity>
        where TFindPipeline: FindPipeline<TEntity>
    {
        private readonly IMongoCollection<TEntity> _collection;

        /// <summary>
        /// 初始化 <see cref="ImmutableEntityRepository{TEntity, TKey, TFilterBuilder, TFindPipeline}"/> 类的新实例。
        /// </summary>
        /// <param name="repository">BitWaves 数据集。</param>
        /// <param name="collection">MongoDB 数据集接口。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="repository"/> 为 null
        ///     或
        ///     <paramref name="collection"/> 为 null。
        /// </exception>
        protected ImmutableEntityRepository(Repository repository, IMongoCollection<TEntity> collection)
        {
            Contract.NotNull(repository, nameof(repository));
            Contract.NotNull(collection, nameof(collection));

            Repository = repository;
            _collection = collection;
        }

        /// <summary>
        /// 获取当前的实体对象数据集所属的 BitWaves 数据集。
        /// </summary>
        public Repository Repository { get; }

        /// <summary>
        /// 执行给定的异步委托，并将抛出的 <see cref="MongoException"/> 异常使用 <see cref="RepositoryException"/> 异常进行
        /// 包装。
        /// </summary>
        /// <param name="action">要执行的委托。</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> 为 null。</exception>
        /// <exception cref="RepositoryException"><paramref name="action"/> 的执行抛出了异常。</exception>
        protected async Task ThrowRepositoryExceptionOnErrorAsync(
            Func<IMongoCollection<TEntity>, RepositoryDataContext, Task> action)
        {
            Contract.NotNull(action, nameof(action));

            try
            {
                await action(_collection, Repository.DataContext);
            }
            catch (MongoException e)
            {
                throw new RepositoryException(e);
            }
        }

        /// <summary>
        /// 执行给定的异步委托，并将抛出的 <see cref="MongoException"/> 异常使用 <see cref="RepositoryException"/> 异常进行
        /// 包装。
        /// </summary>
        /// <param name="action">要执行的委托。</param>
        /// <returns><paramref name="action"/> 的返回值。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> 为 null。</exception>
        /// <exception cref="RepositoryException"><paramref name="action"/> 的执行抛出了异常。</exception>
        protected async Task<T> ThrowRepositoryExceptionOnErrorAsync<T>(
            Func<IMongoCollection<TEntity>, RepositoryDataContext, Task<T>> action)
        {
            Contract.NotNull(action, nameof(action));

            try
            {
                return await action(_collection, Repository.DataContext);
            }
            catch (MongoException e)
            {
                throw new RepositoryException(e);
            }
        }

        /// <summary>
        /// 当在子类中被重写时，获取筛选给定的实体对象 ID 的筛选器定义。
        /// </summary>
        /// <param name="key">要筛选的实体对象定义。</param>
        /// <returns>筛选给定的实体对象 ID 的筛选器定义。</returns>
        protected abstract FilterDefinition<TEntity> GetKeyFilter(TKey key);

        /// <inheritdoc />
        public virtual void Initialize()
        {
        }

        /// <inheritdoc />
        public virtual void Seed()
        {
        }

        /// <inheritdoc />
        public async Task InsertOneAsync(TEntity entity)
        {
            Contract.NotNull(entity, nameof(entity));

            await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.InsertOneAsync(entity));
        }

        /// <inheritdoc />
        public async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            Contract.NotNull(entities, nameof(entities));

            await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.InsertManyAsync(entities));
        }

        /// <inheritdoc />
        public async Task<bool> DeleteOneAsync(TKey key)
        {
            var filter = GetKeyFilter(key);
            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    var deleteResult = await collection.DeleteOneAsync(filter);
                    return deleteResult.DeletedCount == 1;
                });
        }

        /// <inheritdoc />
        public async Task<TEntity> FindOneAsync(TKey key)
        {
            var filter = GetKeyFilter(key);
            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.Find(filter).FirstOrDefaultAsync());
        }

        /// <inheritdoc />
        public async Task<FindResult<TEntity>> FindManyAsync(TFindPipeline pipeline)
        {
            Contract.NotNull(pipeline, nameof(pipeline));

            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.FindAsync(pipeline));
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(TFilterBuilder filterBuilder)
        {
            Contract.NotNull(filterBuilder, nameof(filterBuilder));

            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.Find(filterBuilder).CountDocumentsAsync());
        }
    }
}
