using System;
using System.Collections.Generic;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 静态对象实体对象仓库。
    /// </summary>
    public sealed class ContentRepository : ImmutableEntityRepository<Content, ObjectId, ContentFindPipeline>
    {
        /// <summary>
        /// 初始化 <see cref="ContentRepository"/> 的新实例。
        /// </summary>
        /// <param name="repository">BitWaves 数据仓库。</param>
        /// <param name="mongoCollection">MongoDB 接口。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="repository"/> 为 null
        ///     或
        ///     <paramref name="mongoCollection"/> 为 null。
        /// </exception>
        internal ContentRepository(Repository repository, IMongoCollection<Content> mongoCollection)
            : base(repository, mongoCollection)
        {
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            ThrowRepositoryExceptionOnErrorAsync(async (collection, _) =>
            {
                var indexesList = new List<CreateIndexModel<Content>>
                {
                    // CreationTime 上的递减索引
                    new CreateIndexModel<Content>(
                        Builders<Content>.IndexKeys.Descending(content => content.CreationTime)),
                    // Name 上的哈希索引
                    new CreateIndexModel<Content>(Builders<Content>.IndexKeys.Hashed(content => content.Name)),
                    // MimeType 上的哈希索引
                    new CreateIndexModel<Content>(Builders<Content>.IndexKeys.Hashed(content => content.MimeType))
                };

                await collection.Indexes.CreateManyAsync(indexesList);
            }).Wait();
        }

        /// <inheritdoc />
        protected override FilterDefinition<Content> GetKeyFilter(ObjectId key)
        {
            return Builders<Content>.Filter.Eq(c => c.Id, key);
        }
    }
}
