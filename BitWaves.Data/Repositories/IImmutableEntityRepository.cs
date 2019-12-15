using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BitWaves.Data.DML;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 为不支持更新操作的实体对象集提供抽象。
    /// </summary>
    /// <typeparam name="TEntity">实体对象类型。</typeparam>
    /// <typeparam name="TKey">实体对象的键类型。</typeparam>
    /// <typeparam name="TFilterBuilder">实体对象筛选器建造器的类型。</typeparam>
    /// <typeparam name="TFindPipeline">实体对象的查找管道类型。</typeparam>
    public interface IImmutableEntityRepository<TEntity, in TKey, TFilterBuilder, TFindPipeline>
        where TEntity: class
        where TFilterBuilder: FilterBuilder<TEntity>
        where TFindPipeline: FindPipeline<TEntity>
    {
        /// <summary>
        /// 初始化数据集。
        /// </summary>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        void Initialize();

        /// <summary>
        /// 向数据集中添加种子数据。
        /// </summary>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        void Seed();

        /// <summary>
        /// 向数据集中插入一个实体对象。
        /// </summary>
        /// <param name="entity">要插入的实体对象。</param>
        /// <returns>异步操作的 <see cref="Task"/> 包装。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        Task InsertOneAsync(TEntity entity);

        /// <summary>
        /// 向数据集中插入多个实体对象。
        /// </summary>
        /// <param name="entities">要插入的实体对象。</param>
        /// <returns>异步操作的 <see cref="Task"/> 包装。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entities"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        Task InsertManyAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 从数据集中删除一个实体对象。
        /// </summary>
        /// <param name="key">要删除的实体对象的 ID。</param>
        /// <returns>异步操作的 <see cref="Task{T}"/> 包装，异步操作的结果表示是否成功地删除了指定的实体对象。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        Task<bool> DeleteOneAsync(TKey key);

        /// <summary>
        /// 在数据集中查找指定的实体对象。
        /// </summary>
        /// <param name="key">要查找的实体对象的 ID。</param>
        /// <returns>
        /// 异步操作的 <see cref="Task{T}"/> 包装，异步操作的结果表示查找到的实体对象。若不存在这样的实体对象，返回 null。
        /// </returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        Task<TEntity> FindOneAsync(TKey key);

        /// <summary>
        /// 在数据集中查找满足筛选条件的实体对象集合。
        /// </summary>
        /// <param name="pipeline">查询管道。</param>
        /// <returns>异步操作的 <see cref="Task{T}"/> 包装，异步操作的结果表示查找结果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pipeline"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        Task<FindResult<TEntity>> FindManyAsync(TFindPipeline pipeline);

        /// <summary>
        /// 统计在数据集中满足给定的筛选条件的实体对象的数量。
        /// </summary>
        /// <param name="filterBuilder">筛选条件建造器。</param>
        /// <returns>在数据集中满足给定的筛选条件的实体对象的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filterBuilder"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        Task<long> CountAsync(TFilterBuilder filterBuilder);

        /// <summary>
        /// 检查指定的实体对象 ID 是否存在于当前的数据集中。
        /// </summary>
        /// <param name="key">要检查的实体对象 ID。</param>
        /// <returns>指定的实体对象 ID 是否存在于当前的数据集中。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        Task<bool> ExistAsync(TKey key);
    }
}
