using System;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    public sealed class LanguageRepository : ImmutableEntityRepository<Language, ObjectId, LanguageFindPipeline>
    {
        /// <summary>
        /// 初始化 <see cref="LanguageRepository"/> 类的新实例。
        /// </summary>
        /// <param name="repository">BitWaves 数据仓库。</param>
        /// <param name="collection">语言数据集的 MongoDB 接口。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="repository"/> 为 null
        ///     或
        ///     <paramref name="collection"/> 为 null。
        /// </exception>
        internal LanguageRepository(Repository repository, IMongoCollection<Language> collection)
            : base(repository, collection)
        {
        }

        /// <inheritdoc />
        protected override FilterDefinition<Language> GetKeyFilter(ObjectId key)
        {
            return Builders<Language>.Filter.Eq(lang => lang.Id, key);
        }
    }
}
