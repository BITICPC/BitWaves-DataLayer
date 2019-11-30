using System.Collections.Generic;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 提交数据集。
    /// </summary>
    public sealed class SubmissionRepository
        : EntityRepository<Submission, ObjectId, SubmissionUpdateInfo, SubmissionFindPipeline>
    {
        /// <summary>
        /// 初始化 <see cref="SubmissionRepository"/> 类的新实例。
        /// </summary>
        /// <param name="repository">BitWaves 数据仓库。</param>
        /// <param name="mongoCollection">MongoDB 数据库接口。</param>
        internal SubmissionRepository(Repository repository, IMongoCollection<Submission> mongoCollection)
            : base(repository, mongoCollection)
        {
        }

        /// <inheritdoc />
        protected override FilterDefinition<Submission> GetKeyFilter(ObjectId key)
        {
            return Builders<Submission>.Filter.Eq(s => s.Id, key);
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            ThrowRepositoryExceptionOnErrorAsync(async (collection, _) =>
            {
                var indexesList = new List<CreateIndexModel<Submission>>
                {
                    // Author 上的哈希索引
                    new CreateIndexModel<Submission>(Builders<Submission>.IndexKeys.Hashed(sub => sub.Author)),
                    // ProblemId 上的递增索引
                    new CreateIndexModel<Submission>(Builders<Submission>.IndexKeys.Ascending(sub => sub.ProblemId)),
                    // LanguageId 上的哈希索引
                    new CreateIndexModel<Submission>(Builders<Submission>.IndexKeys.Hashed(sub => sub.LanguageId)),
                    // CreationTime 上的递减索引
                    new CreateIndexModel<Submission>(
                        Builders<Submission>.IndexKeys.Descending(sub => sub.CreationTime)),
                    // Status 上的递增索引
                    new CreateIndexModel<Submission>(Builders<Submission>.IndexKeys.Ascending(sub => sub.Status)),
                    // Result.Verdict 上的递增索引
                    new CreateIndexModel<Submission>(
                        Builders<Submission>.IndexKeys.Ascending(sub => sub.Result.Verdict))
                };

                await collection.Indexes.CreateManyAsync(indexesList);
            }).Wait();
        }
    }
}
