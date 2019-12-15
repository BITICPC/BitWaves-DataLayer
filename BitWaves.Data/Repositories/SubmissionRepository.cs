using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using BitWaves.Data.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 提交数据集。
    /// </summary>
    public sealed class SubmissionRepository
        : ImmutableEntityRepository<Submission, ObjectId, SubmissionFilterBuilder, SubmissionFindPipeline>
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
                    // LanguageTriple.Identifier 上的哈希索引
                    new CreateIndexModel<Submission>(
                        Builders<Submission>.IndexKeys.Hashed(sub => sub.LanguageTriple.Identifier)),
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
        /// 更新指定用户的统计数据。
        /// </summary>
        /// <param name="username">用户名。</param>
        /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        private async Task SyncUserStatisticsAsync(string username)
        {
            Contract.NotNull(username, nameof(username));

            // FIXME: Possible performance bottleneck here.
            var totalSubmissions = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDocumentsAsync(
                    Builders<Submission>.Filter.Eq(s => s.Author, username)));
            var totalAcceptedSubmissions = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDocumentsAsync(
                    Builders<Submission>.Filter.And(
                        Builders<Submission>.Filter.Eq(s => s.Author, username),
                        Builders<Submission>.Filter.Eq(s => s.Result.Verdict, Verdict.Accepted))));
            var attemptedProblems = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDistinctByAsync(
                    Builders<Submission>.Filter.Eq(s => s.Author, username),
                    s => s.ProblemId));
            var solvedProblems = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDistinctByAsync(
                    Builders<Submission>.Filter.And(
                        Builders<Submission>.Filter.Eq(s => s.Author, username),
                        Builders<Submission>.Filter.Eq(s => s.Result.Verdict, Verdict.Accepted)),
                    s => s.ProblemId));

            await ThrowRepositoryExceptionOnErrorAsync(
                async (_, context) => await context.Users.UpdateOneAsync(
                    Builders<User>.Filter.Eq(u => u.Username, username),
                    Builders<User>.Update.Combine(
                        Builders<User>.Update.Set(u => u.TotalSubmissions, totalSubmissions),
                        Builders<User>.Update.Set(u => u.TotalAcceptedSubmissions, totalAcceptedSubmissions),
                        Builders<User>.Update.Set(u => u.TotalProblemsAttempted, attemptedProblems),
                        Builders<User>.Update.Set(u => u.TotalProblemsAccepted, solvedProblems))));
        }

        /// <summary>
        /// 更新指定题目的统计数据。
        /// </summary>
        /// <param name="problemId">题目 ID。</param>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        private async Task SyncProblemStatisticsAsync(ObjectId problemId)
        {
            // FIXME: Possible performance bottleneck here.
            var totalSubmissions = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDocumentsAsync(
                    Builders<Submission>.Filter.Eq(s => s.ProblemId, problemId)));
            var acceptedSubmissions = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDocumentsAsync(
                    Builders<Submission>.Filter.And(
                        Builders<Submission>.Filter.Eq(s => s.ProblemId, problemId),
                        Builders<Submission>.Filter.Eq(s => s.Result.Verdict, Verdict.Accepted))));
            var attemptedUsers = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDistinctByAsync(
                    Builders<Submission>.Filter.Eq(s => s.ProblemId, problemId),
                    s => s.Author));
            var solvedUsers = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDistinctByAsync(
                    Builders<Submission>.Filter.And(
                        Builders<Submission>.Filter.Eq(s => s.ProblemId, problemId),
                        Builders<Submission>.Filter.Eq(s => s.Result.Verdict, Verdict.Accepted)),
                    s => s.Author));

            await ThrowRepositoryExceptionOnErrorAsync(
                async (_, context) => await context.Problems.UpdateOneAsync(
                    Builders<Problem>.Filter.Eq(p => p.Id, problemId),
                    Builders<Problem>.Update.Combine(
                        Builders<Problem>.Update.Set(p => p.TotalSubmissions, totalSubmissions),
                        Builders<Problem>.Update.Set(p => p.AcceptedSubmissions, acceptedSubmissions),
                        Builders<Problem>.Update.Set(p => p.TotalAttemptedUsers, attemptedUsers),
                        Builders<Problem>.Update.Set(p => p.TotalSolvedUsers, solvedUsers))));
        }

        /// <summary>
        /// 更新所有数据集中与提交相关的统计信息。
        /// </summary>
        /// <param name="username">要更新的用户统计信息所属的用户名。</param>
        /// <param name="problemId">要更新的题目统计信息所属的题目 ID。</param>
        /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        private async Task SyncStatisticsAsync(string username, ObjectId problemId)
        {
            Contract.NotNull(username, nameof(username));

            await SyncUserStatisticsAsync(username);
            await SyncProblemStatisticsAsync(problemId);
        }

        /// <inheritdoc />
        public override async Task InsertOneAsync(Submission entity)
        {
            await base.InsertOneAsync(entity);

            await SyncStatisticsAsync(entity.Author, entity.ProblemId);
        }

        /// <inheritdoc />
        public override async Task InsertManyAsync(IEnumerable<Submission> entities)
        {
            var entitiesList = entities.ToList();
            await base.InsertManyAsync(entitiesList);

            var problems = entitiesList.Select(e => e.ProblemId)
                                       .ToHashSet();
            var usernames = entitiesList.Select(e => e.Author)
                                        .ToHashSet();

            // FIXME: Possible performance bottleneck here.
            foreach (var problemId in problems)
            {
                await SyncProblemStatisticsAsync(problemId);
            }

            foreach (var username in usernames)
            {
                await SyncUserStatisticsAsync(username);
            }
        }

        /// <inheritdoc />
        public override async Task<bool> DeleteOneAsync(ObjectId key)
        {
            var entity = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    var document = await collection.FindOneAndDeleteAsync(
                        Builders<Submission>.Filter.Eq(s => s.Id, key),
                        new FindOneAndDeleteOptions<Submission, BsonDocument>
                        {
                            Projection = Builders<Submission>.Projection.Combine(
                                Builders<Submission>.Projection.Include(s => s.Author),
                                Builders<Submission>.Projection.Include(s => s.ProblemId))
                        });
                    if (document == null)
                    {
                        return null;
                    }

                    return BsonSerializer.Deserialize<Submission>(document);
                });
            if (entity == null)
            {
                return false;
            }

            await SyncStatisticsAsync(entity.Author, entity.ProblemId);
            return true;
        }

        /// <summary>
        /// 在数据集中查找被给定的提交所引用的题目的实体对象，并将其与相应的 <see cref="Submission"/> 对象关联起来。
        /// </summary>
        /// <param name="submissions">提交实体对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="submissions"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        private async Task LookupRelatedProblems(IEnumerable<Submission> submissions)
        {
            Contract.NotNull(submissions, nameof(submissions));

            var submissionsList = submissions.ToList();
            var problemIds = submissionsList.Select(s => s.ProblemId);
            var problems = await ThrowRepositoryExceptionOnErrorAsync(
                async (_, context) => await context.Problems.Find(Builders<Problem>.Filter.In(p => p.Id, problemIds))
                                                   .Project(Builders<Problem>.Projection.Exclude(p => p.Description))
                                                   .ToEntityListAsync());
            var problemLookup = problems.ToDictionary(p => p.Id);

            foreach (var s in submissionsList)
            {
                if (problemLookup.TryGetValue(s.ProblemId, out var p))
                {
                    s.Problem = p;
                }
            }
        }

        /// <inheritdoc />
        public override async Task<Submission> FindOneAsync(ObjectId key)
        {
            var submission = await FindOneWithoutProblemAsync(key);
            if (submission == null)
            {
                return null;
            }

            await LookupRelatedProblems(new[] { submission });
            return submission;
        }

        /// <summary>
        /// 根据给定的提交 ID 查找提交实体对象。返回的提交实体对象中不包含有效的 <see cref="Submission.Problem"/> 属性值。
        /// </summary>
        /// <param name="key">要查找的提交的 ID。</param>
        /// <returns>根据给定的提交 ID 查找到的提交实体对象。若没有这样的提交，返回 null。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        public async Task<Submission> FindOneWithoutProblemAsync(ObjectId key)
        {
            return await base.FindOneAsync(key);
        }

        /// <inheritdoc />
        public override async Task<FindResult<Submission>> FindManyAsync(SubmissionFindPipeline pipeline)
        {
            Contract.NotNull(pipeline, nameof(pipeline));

            var submissions = await base.FindManyAsync(pipeline);

            if (pipeline.IncludeProblems)
            {
                await LookupRelatedProblems(submissions.ResultSet);
            }

            return submissions;
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

            var entity = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    var document = await collection.FindOneAndUpdateAsync(
                        GetKeyFilter(key),
                        Builders<Submission>.Update.Combine(
                            Builders<Submission>.Update.Set(s => s.Status, JudgeStatus.Finished),
                            Builders<Submission>.Update.Set(s => s.Result, result)),
                        new FindOneAndUpdateOptions<Submission, BsonDocument>
                        {
                            Projection = Builders<Submission>.Projection.Combine(
                                Builders<Submission>.Projection.Include(s => s.ProblemId),
                                Builders<Submission>.Projection.Include(s => s.Author))
                        });
                    if (document == null)
                    {
                        return null;
                    }

                    return BsonSerializer.Deserialize<Submission>(document);
                });
            if (entity == null)
            {
                return false;
            }

            await SyncStatisticsAsync(entity.Author, entity.ProblemId);
            return true;
        }

        /// <summary>
        /// 将指定的提交的评测状态重置为评测开始前的状态。
        /// </summary>
        /// <param name="key">需要重置评测状态的评测的 ID。</param>
        /// <returns>是否成功地重置了将指定的提交的评测状态。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        public async Task<bool> ResetJudgeStatusAsync(ObjectId key)
        {
            var entity = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    var document = await collection.FindOneAndUpdateAsync(
                        GetKeyFilter(key),
                        Builders<Submission>.Update.Combine(
                            Builders<Submission>.Update.Set(s => s.JudgeTime, null),
                            Builders<Submission>.Update.Set(s => s.Status, JudgeStatus.Pending),
                            Builders<Submission>.Update.Set(s => s.Result, null)),
                        new FindOneAndUpdateOptions<Submission, BsonDocument>
                        {
                            Projection = Builders<Submission>.Projection.Combine(
                                Builders<Submission>.Projection.Include(s => s.ProblemId),
                                Builders<Submission>.Projection.Include(s => s.Author))
                        });
                    if (document == null)
                    {
                        return null;
                    }

                    return BsonSerializer.Deserialize<Submission>(document);
                });
            if (entity == null)
            {
                return false;
            }

            await SyncStatisticsAsync(entity.Author, entity.ProblemId);
            return true;
        }

        /// <summary>
        /// 重置指定的题目的所有提交的评测状态为评测之前的状态。
        /// </summary>
        /// <param name="problemId">题目 ID。</param>
        /// <returns>成功地重置了多少个提交的评测状态。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时发生错误。</exception>
        public async Task<long> ResetJudgeStatusOfProblemAsync(ObjectId problemId)
        {
            var usernames = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    return await collection.Find(Builders<Submission>.Filter.Eq(s => s.ProblemId, problemId))
                                           .Project(s => s.Author)
                                           .ToHashSetAsync();
                });

            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.UpdateManyAsync(
                    Builders<Submission>.Filter.Eq(s => s.ProblemId, problemId),
                    Builders<Submission>.Update.Combine(
                        Builders<Submission>.Update.Set(s => s.JudgeTime, null),
                        Builders<Submission>.Update.Set(s => s.Status, JudgeStatus.Pending),
                        Builders<Submission>.Update.Set(s => s.Result, null))));

            // TODO: Possible performance bottleneck here.
            await SyncProblemStatisticsAsync(problemId);
            foreach (var username in usernames)
            {
                await SyncUserStatisticsAsync(username);
            }

            return updateResult.MatchedCount;
        }
    }
}
