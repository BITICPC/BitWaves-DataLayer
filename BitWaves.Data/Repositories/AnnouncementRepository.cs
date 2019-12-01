using System.Collections.Generic;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 表示全站公告仓库。
    /// </summary>
    public sealed class AnnouncementRepository
        : EntityRepository<Announcement, ObjectId, AnnouncementUpdateInfo, AnnouncementFilterBuilder,
            AnnouncementFindPipeline>
    {
        /// <summary>
        /// 初始化 <see cref="AnnouncementRepository"/> 类的新实例。
        /// </summary>
        /// <param name="repository">BitWaves 数据仓库。</param>
        /// <param name="mongoCollection">MongoDB 数据集接口。s</param>
        internal AnnouncementRepository(Repository repository, IMongoCollection<Announcement> mongoCollection)
            : base(repository, mongoCollection)
        {
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            ThrowRepositoryExceptionOnErrorAsync(async (collection, _) =>
            {
                var indexesList = new List<CreateIndexModel<Announcement>>
                {
                    new CreateIndexModel<Announcement>(
                        Builders<Announcement>.IndexKeys.Descending(announcement => announcement.LastUpdateTime))
                };

                await collection.Indexes.CreateManyAsync(indexesList);
            }).Wait();
        }

        /// <inheritdoc />
        protected override FilterDefinition<Announcement> GetKeyFilter(ObjectId key)
        {
            return Builders<Announcement>.Filter.Eq(a => a.Id, key);
        }
    }
}
