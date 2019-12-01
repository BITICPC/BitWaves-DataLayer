using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为数据筛选器构建器提供公共抽象基类。
    /// </summary>
    /// <typeparam name="TEntity">实体对象类型。</typeparam>
    public abstract class FilterBuilder<TEntity>
    {
        private readonly List<FilterDefinition<TEntity>> _filters;

        /// <summary>
        /// 初始化 <see cref="FilterBuilder{TEntity}"/> 类的新实例。
        /// </summary>
        protected FilterBuilder()
        {
            _filters = new List<FilterDefinition<TEntity>>();
        }

        /// <summary>
        /// 向当前的 <see cref="FilterBuilder{TEntity}"/> 对象添加一个新的筛选器定义。
        /// </summary>
        /// <param name="filter">要添加的筛选器定义。</param>
        /// <exception cref="ArgumentNullException"><paramref name="filter"/> 为 null。</exception>
        protected void AddFilter(FilterDefinition<TEntity> filter)
        {
            Contract.NotNull(filter, nameof(filter));

            _filters.Add(filter);
        }

        /// <summary>
        /// 从当前的 <see cref="FilterBuilder{TEntity}"/> 对象创建 <see cref="FilterDefinition{TEntity}"/> 定义。
        /// </summary>
        /// <returns>
        ///     从当前的 <see cref="FilterBuilder{TEntity}"/> 对象创建的 <see cref="FilterDefinition{TEntity}"/> 定义。
        /// </returns>
        internal FilterDefinition<TEntity> CreateFilterDefinition()
        {
            if (_filters.Count == 0)
            {
                return FilterDefinition<TEntity>.Empty;
            }

            if (_filters.Count == 1)
            {
                return _filters[0];
            }

            return Builders<TEntity>.Filter.And(_filters);
        }

        /// <summary>
        /// 获取空的 <see cref="FilterBuilder{TEntity}"/> 实现。
        /// </summary>
        public static FilterBuilder<TEntity> Empty { get; } = new EmptyFilterBuilder<TEntity>();
    }
}
