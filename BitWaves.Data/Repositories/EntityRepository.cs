using System;
using System.Threading.Tasks;
using BitWaves.Data.DML;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 为实体对象数据集提供公共抽象基类。
    /// </summary>
    /// <typeparam name="TEntity">实体对象类型。</typeparam>
    /// <typeparam name="TKey">实体对象的 ID 的类型。</typeparam>
    /// <typeparam name="TUpdateInfo">实体对象的更新类型。</typeparam>
    /// <typeparam name="TFindPipeline">实体对象的查询管道类型。</typeparam>
    public abstract class EntityRepository<TEntity, TKey, TUpdateInfo, TFindPipeline>
        : ImmutableEntityRepository<TEntity, TKey, TFindPipeline>,
          IEntityRepository<TEntity, TKey, TUpdateInfo, TFindPipeline>
        where TEntity: class
        where TUpdateInfo: UpdateInfo<TEntity>
        where TFindPipeline: FindPipeline<TEntity>
    {
        /// <summary>
        /// 初始化 <see cref="EntityRepository{TEntity, TKey, TUpdateInfo, TFindPipeline}"/> 类的新实例。
        /// </summary>
        /// <param name="repository">BitWaves 数据仓库。</param>
        /// <param name="collection">MongoDB 数据集接口。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="repository"/> 为 null
        ///     或
        ///     <paramref name="collection"/> 为 null。
        /// </exception>
        protected EntityRepository(Repository repository, IMongoCollection<TEntity> collection)
            : base(repository, collection)
        {
        }

        /// <inheritdoc />
        public async Task<bool> UpdateOneAsync(TKey key, TUpdateInfo updateInfo)
        {
            Contract.NotNull(updateInfo, nameof(updateInfo));

            var filter = GetKeyFilter(key);
            var updateDefinition = updateInfo.CreateUpdateDefinition();
            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    var updateResult = await collection.UpdateOneAsync(filter, updateDefinition);
                    return updateResult.MatchedCount == 1;
                });
        }
    }
}
