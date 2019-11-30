using System;
using System.Collections.Generic;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 封装查询操作的结果。
    /// </summary>
    /// <typeparam name="TEntity">实体对象类型。</typeparam>
    public sealed class FindResult<TEntity>
    {
        /// <summary>
        /// 初始化 <see cref="FindResult{TEntity}"/> 类的新实例。
        /// </summary>
        /// <param name="totalCount">分页前满足筛选条件的元素总数量。</param>
        /// <param name="resultSet">最终查询结果集。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="totalCount"/> 为负数。</exception>
        public FindResult(long totalCount, List<TEntity> resultSet)
        {
            Contract.NonNegative(totalCount, $"{nameof(totalCount)} cannot be negative.", nameof(totalCount));
            Contract.NotNull(resultSet, nameof(resultSet));

            TotalCount = totalCount;
            ResultSet = resultSet;
        }

        /// <summary>
        /// 获取在分页前满足筛选条件的实体对象总数量。
        /// </summary>
        public long TotalCount { get; }

        /// <summary>
        /// 获取最终的结果集。
        /// </summary>
        public List<TEntity> ResultSet { get; }
    }
}
