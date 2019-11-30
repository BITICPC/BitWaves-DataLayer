using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BitWaves.Data.Extensions;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为查询管道提供公共抽象基类。
    /// </summary>
    /// <remarks>
    /// 所有的查询操作均分为四个阶段：筛选阶段、排序阶段、分页阶段和收集阶段。筛选阶段的筛选定义在构造时由给定的
    /// <see cref="FilterBuilder{TEntity}"/> 对象产生；排序阶段和收集阶段的逻辑可以在子类中被重写。该基类实现了分页阶段的逻辑。
    /// </remarks>
    /// <typeparam name="TEntity">实体对象类型。</typeparam>
    public abstract class FindPipeline<TEntity>
    {
        private readonly FilterBuilder<TEntity> _filterBuilder;
        private Pagination _pagination;

        /// <summary>
        /// 初始化 <see cref="FindPipeline{TEntity}"/> 类的新实例。
        /// </summary>
        /// <param name="filterBuilder">实体对象筛选器构造器。</param>
        /// <exception cref="ArgumentNullException"><paramref name="filterBuilder"/> 为 null。</exception>
        protected FindPipeline(FilterBuilder<TEntity> filterBuilder)
        {
            Contract.NotNull(filterBuilder, nameof(filterBuilder));

            _filterBuilder = filterBuilder;
            _pagination = Pagination.AllElements;
        }

        /// <summary>
        /// 获取或设置分页参数。
        /// </summary>
        /// <exception cref="ArgumentNullException">尝试设置该属性为 null。</exception>
        public Pagination Pagination
        {
            get => _pagination;
            set
            {
                Contract.NotNull(value, nameof(value));
                _pagination = value;
            }
        }

        /// <summary>
        /// 当在子类中重写时，在给定的查询会话上执行排序操作。
        /// </summary>
        /// <param name="find">需要执行排序操作的查询会话。</param>
        /// <returns>执行了排序操作的查询会话。</returns>
        protected abstract IFindFluent<TEntity, TEntity> Sort(IFindFluent<TEntity, TEntity> find);

        /// <summary>
        /// 当在子类中重写时，在给定的查询会话上执行收集操作。执行收集操作前子类可以按需要执行映射操作。
        /// </summary>
        /// <param name="find">需要执行读取操作的查询会话。</param>
        /// <returns>异步操作的 <see cref="Task{T}"/> 封装，异步操作的结果为从给定的查询会话上读取的结果集。</returns>
        protected virtual async Task<List<TEntity>> CollectAsync(IFindFluent<TEntity, TEntity> find)
        {
            return await find.ToListAsync();
        }

        /// <summary>
        /// 在给定的 <see cref="IMongoCollection{TEntity}"/> 对象上执行当前的查询管道逻辑。
        /// </summary>
        /// <param name="collection">需要执行查询的 <see cref="IMongoCollection{TEntity}"/> 对象。</param>
        /// <returns>查询结果。</returns>
        internal async Task<FindResult<TEntity>> ExecuteAsync(IMongoCollection<TEntity> collection)
        {
            Contract.NotNull(collection, nameof(collection));

            // Phase 1: filter phase
            var find = collection.Find(_filterBuilder);
            var totalCount = await find.CountDocumentsAsync();

            // Phase 2: sort phase
            find = Sort(find);

            // Phase 3: pagination phase
            find = find.Paginate(Pagination);

            // Phase 4: collect phase
            var resultSet = await CollectAsync(find);

            return new FindResult<TEntity>(totalCount, resultSet);
        }
    }
}
