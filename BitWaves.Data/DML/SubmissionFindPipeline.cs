using System;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为提交查询提供查询管道。
    /// </summary>
    public sealed class SubmissionFindPipeline : FindPipeline<Submission>
    {
        /// <summary>
        /// 初始化 <see cref="SubmissionFindPipeline"/> 的新实例。
        /// </summary>
        /// <param name="filterBuilder">用户提交筛选器的建造器。</param>
        /// <exception cref="ArgumentNullException"><paramref name="filterBuilder"/> 为 null。</exception>
        public SubmissionFindPipeline(SubmissionFilterBuilder filterBuilder) : base(filterBuilder)
        {
            IncludeProblems = true;
            SortByDescending = true;
        }

        /// <summary>
        /// 获取或设置排序键。
        /// </summary>
        public SubmissionSortKey SortKey { get; set; }

        /// <summary>
        /// 获取或设置是否按照降序进行排序。
        /// </summary>
        public bool SortByDescending { get; set; }

        /// <summary>
        /// 获取或设置是否在查询结果中包含有效的 <see cref="Submission.Problem"/> 属性值。
        /// </summary>
        public bool IncludeProblems { get; set; }

        /// <inheritdoc />
        protected override IFindFluent<Submission, Submission> Sort(IFindFluent<Submission, Submission> find)
        {
            if (SortByDescending)
            {
                find = find.Sort(Builders<Submission>.Sort.Descending(SortKey.GetField()));
            }
            else
            {
                find = find.Sort(Builders<Submission>.Sort.Ascending(SortKey.GetField()));
            }

            return find;
        }

        /// <summary>
        /// 获取 <see cref="SubmissionFindPipeline"/> 的默认实现。
        /// </summary>
        public static SubmissionFindPipeline Default => new SubmissionFindPipeline(SubmissionFilterBuilder.Empty);
    }
}
