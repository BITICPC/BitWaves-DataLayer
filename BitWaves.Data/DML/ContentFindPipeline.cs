using System;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为查找静态对象操作提供查询管道。
    /// </summary>
    public sealed class ContentFindPipeline : FindPipeline<Content>
    {
        /// <summary>
        /// 初始化 <see cref="ContentFindPipeline"/> 类的新实例。
        /// </summary>
        /// <param name="filterBuilder">静态对象筛选器建造器。</param>
        /// <exception cref="ArgumentNullException"><paramref name="filterBuilder"/> 为 null。</exception>
        public ContentFindPipeline(ContentFilterBuilder filterBuilder) : base(filterBuilder)
        {
        }

        /// <inheritdoc />
        protected override IFindFluent<Content, Content> Sort(IFindFluent<Content, Content> find)
        {
            return find.SortByDescending(c => c.CreationTime);
        }

        /// <summary>
        /// 获取 <see cref="ContentFindPipeline"/> 类的默认实例。
        /// </summary>
        public static ContentFindPipeline Default => new ContentFindPipeline(ContentFilterBuilder.Empty);
    }
}
