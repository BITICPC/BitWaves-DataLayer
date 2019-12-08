using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        : EntityRepository<Submission, ObjectId, SubmissionUpdateInfo, SubmissionFilterBuilder, SubmissionFindPipeline>
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
                    // Language.Identifier 上的哈希索引
                    new CreateIndexModel<Submission>(
                        Builders<Submission>.IndexKeys.Hashed(sub => sub.Language.Identifier)),
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

        /// <summary>
        /// 在数据集中查找一个尚未被评测的提交，并将其的评测状态修改为 <see cref="JudgeStatus.Judging"/>。
        /// </summary>
        /// <returns>找到的尚未被评测的提交。如果不存在这样的提交，返回 null。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        public async Task<Submission> FindOneUnjudgedSubmissionAsync()
        {
            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, context) =>
                    await collection.FindOneAndUpdateAsync(
                        Builders<Submission>.Filter.Eq(s => s.Status, JudgeStatus.Pending),
                        Builders<Submission>.Update.Set(s => s.Status, JudgeStatus.Judging)));
        }

        /// <summary>
        /// 更新指定的提交的评测结果。该方法会将指定的提交的评测状态修改为 <see cref="JudgeStatus.Finished"/>。
        /// </summary>
        /// <param name="key">需要更新评测结果的提交的 ID。</param>
        /// <param name="result">更新的评测结果。</param>
        /// <returns>是否成功地更新了指定的提交的评测结果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="result"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        public async Task<bool> SetJudgeResultAsync(ObjectId key, JudgeResult result)
        {
            Contract.NotNull(result, nameof(result));

            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.UpdateOneAsync(
                    GetKeyFilter(key),
                    Builders<Submission>.Update.Combine(
                        Builders<Submission>.Update.Set(s => s.Status, JudgeStatus.Finished),
                        Builders<Submission>.Update.Set(s => s.Result, result))));
            return updateResult.MatchedCount == 1;
        }
    }
}
