using System;
using System.Threading.Tasks;
using BitWaves.Data.DML;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 为实体对象数据集提供接口。
    /// </summary>
    /// <typeparam name="TEntity">实体对象类型。</typeparam>
    /// <typeparam name="TKey">实体对象的 ID 类型。</typeparam>
    /// <typeparam name="TUpdateInfo">更新给定实体对象类型的更新定义类型。</typeparam>
    /// <typeparam name="TFindPipeline">查询管道类型。</typeparam>
    public interface IEntityRepository<TEntity, in TKey, TUpdateInfo, TFindPipeline>
        : IImmutableEntityRepository<TEntity, TKey, TFindPipeline>
        where TEntity: class
        where TUpdateInfo: UpdateInfo<TEntity>
        where TFindPipeline: FindPipeline<TEntity>
    {
        /// <summary>
        /// 更新数据集中的一个实体对象。
        /// </summary>
        /// <param name="key">要更新的实体对象的 ID。</param>
        /// <param name="updateInfo">实体对象的更新信息。</param>
        /// <returns>异步操作的 <see cref="Task{T}"/> 包装，异步操作的结果表示是否成功地更新了指定的实体对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="updateInfo"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        Task<bool> UpdateOneAsync(TKey key, TUpdateInfo updateInfo);
    }
}
