using System;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为题目查询提供查询管道。
    /// </summary>
    public sealed class ProblemFindPipeline : FindPipeline<Problem>
    {
        /// <summary>
        /// 初始化 <see cref="ProblemFindPipeline"/> 类的新实例。
        /// </summary>
        /// <param name="filterBuilder">题目查询过滤器建造器。</param>
        /// <exception cref="ArgumentNullException"><paramref name="filterBuilder"/> 为 null。</exception>
        public ProblemFindPipeline(ProblemFilterBuilder filterBuilder) : base(filterBuilder)
        {
            SortKey = ProblemSortKey.LastUpdateTime;
        }

        /// <summary>
        /// 获取或设置排序键。
        /// </summary>
        public ProblemSortKey SortKey { get; set; }

        /// <summary>
        /// 获取或设置是否按照指定排序键的降序排列。
        /// </summary>
        public bool SortByDescending { get; set; }

        /// <inheritdoc />
        protected override IFindFluent<Problem, Problem> Sort(IFindFluent<Problem, Problem> find)
        {
            if (SortByDescending)
            {
                find = find.Sort(Builders<Problem>.Sort.Descending(SortKey.GetField()));
            }
            else
            {
                find = find.Sort(Builders<Problem>.Sort.Ascending(SortKey.GetField()));
            }

            return find;
        }

        /// <summary>
        /// 获取 <see cref="ProblemFindPipeline"/> 的默认实例。
        /// </summary>
        public static ProblemFindPipeline Default => new ProblemFindPipeline(ProblemFilterBuilder.Empty);
    }
}
