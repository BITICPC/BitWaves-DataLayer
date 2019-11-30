using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为语言实体对象提供筛选器建造器。
    /// </summary>
    public sealed class LanguageFilterBuilder : FilterBuilder<Language>
    {
        /// <summary>
        /// 筛选给定的语言标识符。
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public LanguageFilterBuilder Identifier(string identifier)
        {
            Contract.NotNull(identifier, nameof(identifier));

            AddFilter(Builders<Language>.Filter.Eq(lang => lang.Identifier, identifier));
            return this;
        }

        /// <summary>
        /// 获取一个空的 <see cref="LanguageFilterBuilder"/> 实例。
        /// </summary>
        public new static LanguageFilterBuilder Empty => new LanguageFilterBuilder();
    }
}
