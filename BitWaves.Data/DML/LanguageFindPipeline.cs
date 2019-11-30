using System;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为查询语言实体对象提供查询管道。
    /// </summary>
    public sealed class LanguageFindPipeline : FindPipeline<Language>
    {
        /// <summary>
        /// 初始化 <see cref="LanguageFindPipeline"/> 类的新实例。
        /// </summary>
        /// <param name="filterBuilder">语言实体对象的筛选器建造器。</param>
        /// <exception cref="ArgumentNullException"><paramref name="filterBuilder"/> 为 null。</exception>
        public LanguageFindPipeline(LanguageFilterBuilder filterBuilder) : base(filterBuilder)
        {
        }

        /// <inheritdoc />
        protected override IFindFluent<Language, Language> Sort(IFindFluent<Language, Language> find)
        {
            return find.SortBy(lang => lang.Identifier);
        }

        /// <summary>
        /// 获取默认的 <see cref="LanguageFindPipeline"/> 实例对象。
        /// </summary>
        public static LanguageFindPipeline Default => new LanguageFindPipeline(LanguageFilterBuilder.Empty);
    }
}
