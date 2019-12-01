using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 题目数据集。
    /// </summary>
    public sealed class ProblemRepository
        : EntityRepository<Problem, ObjectId, ProblemUpdateInfo, ProblemFilterBuilder, ProblemFindPipeline>
    {
        /// <summary>
        /// 初始化 <see cref="ProblemRepository"/> 类的新实例。
        /// </summary>
        /// <param name="repository">BitWaves 数据仓库。</param>
        /// <param name="mongoCollection">题目数据集的 MongoDB 接口。</param>
        internal ProblemRepository(Repository repository, IMongoCollection<Problem> mongoCollection)
            : base(repository, mongoCollection)
        {
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            ThrowRepositoryExceptionOnErrorAsync(async (collection, _) =>
            {
                var indexesList = new List<CreateIndexModel<Problem>>
                {
                    // ArchiveId 上的稀疏递增唯一索引
                    new CreateIndexModel<Problem>(Builders<Problem>.IndexKeys.Ascending(problem => problem.ArchiveId),
                                                  new CreateIndexOptions { Sparse = true, Unique = true }),
                    // LastUpdateTime 上的递减索引
                    new CreateIndexModel<Problem>(
                        Builders<Problem>.IndexKeys.Descending(problem => problem.LastUpdateTime)),
                    // Difficulty 上的递增索引
                    new CreateIndexModel<Problem>(Builders<Problem>.IndexKeys.Ascending(problem => problem.Difficulty)),
                    // Tags 上的多键索引
                    new CreateIndexModel<Problem>(Builders<Problem>.IndexKeys.Ascending(problem => problem.Tags)),
                    // TotalSubmissions 上的递减索引
                    new CreateIndexModel<Problem>(
                        Builders<Problem>.IndexKeys.Descending(problem => problem.TotalSubmissions)),
                    // AcceptedSubmissions 上的递减索引
                    new CreateIndexModel<Problem>(
                        Builders<Problem>.IndexKeys.Descending(problem => problem.AcceptedSubmissions)),
                    // TotalAttemptedUsers 上的递减索引
                    new CreateIndexModel<Problem>(
                        Builders<Problem>.IndexKeys.Descending(problem => problem.TotalAttemptedUsers)),
                    // TotalSolvedUsers 上的递减索引
                    new CreateIndexModel<Problem>(
                        Builders<Problem>.IndexKeys.Descending(problem => problem.TotalSolvedUsers)),
                    // LastSubmissionTime 上的递减索引
                    new CreateIndexModel<Problem>(
                        Builders<Problem>.IndexKeys.Descending(problem => problem.LastSubmissionTime))
                };

                await collection.Indexes.CreateManyAsync(indexesList);
            }).Wait();
        }

        /// <inheritdoc />
        protected override FilterDefinition<Problem> GetKeyFilter(ObjectId key)
        {
            return Builders<Problem>.Filter.Eq(p => p.Id, key);
        }

        /// <summary>
        /// 将给定的标签加入到指定的题目中。
        /// </summary>
        /// <param name="key">要添加标签的题目的 ID。</param>
        /// <param name="tagsToAdd"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="tagsToAdd"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> AddTagsToProblemAsync(ObjectId key, IEnumerable<string> tagsToAdd)
        {
            Contract.NotNull(tagsToAdd, nameof(tagsToAdd));

            var updateDefinition = Builders<Problem>.Update.AddToSetEach(p => p.Tags, tagsToAdd);
            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.UpdateOneAsync(GetKeyFilter(key), updateDefinition));

            return updateResult.MatchedCount == 1;
        }

        /// <summary>
        /// 从指定的题目删除标签。
        /// </summary>
        /// <param name="key">要删除标签的题目的 ID。</param>
        /// <param name="tagsToDelete">要删除的标签。</param>
        /// <returns>是否成功地从指定的题目删除了标签。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tagsToDelete"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> DeleteTagsFromProblemAsync(ObjectId key, IEnumerable<string> tagsToDelete)
        {
            Contract.NotNull(tagsToDelete, nameof(tagsToDelete));

            var updateDefinition = Builders<Problem>.Update.PullAll(p => p.Tags, tagsToDelete);
            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.UpdateOneAsync(GetKeyFilter(key), updateDefinition));

            return updateResult.MatchedCount == 1;
        }

        /// <summary>
        /// 清空指定题目的所有标签。
        /// </summary>
        /// <param name="key">要清空标签的题目的 ID。</param>
        /// <returns>是否成功地清空了指定题目的标签。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> ClearProblemTagsAsync(ObjectId key)
        {
            var updateDefinition = Builders<Problem>.Update.Set(p => p.Tags, new List<string>());
            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.UpdateOneAsync(GetKeyFilter(key), updateDefinition));

            return updateResult.MatchedCount == 1;
        }

        /// <summary>
        /// 查找所有至少存在于一个满足给定筛选条件的题目中的标签。
        /// </summary>
        /// <param name="filterBuilder">题目筛选条件。</param>
        /// <returns>所有至少存在于一个题目中的标签。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filterBuilder"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<List<ProblemTag>> FindAllTagsAsync(ProblemFilterBuilder filterBuilder)
        {
            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    var data = await collection.Aggregate()
                                               .Match(filterBuilder.CreateFilterDefinition())
                                               .Project(p => new { p.Id, p.Tags })
                                               .Unwind(p => p.Tags)
                                               .Group(p => p["Tags"], g => new { Name = g.Key, Count = g.Count() })
                                               .ToListAsync();
                    return data.Select(e => new ProblemTag(e.Name.AsString, e.Count))
                               .ToList();
                });
        }

        /// <summary>
        /// 向指定的题目添加样例输入。
        /// </summary>
        /// <param name="key">要添加样例输入的题目 ID。</param>
        /// <param name="sampleTests">要添加的样例信息。</param>
        /// <returns>是否成功地添加了样例输入。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sampleTests"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> AddSampleTestCasesToProblemAsync(ObjectId key,
                                                                 IEnumerable<ProblemSampleTest> sampleTests)
        {
            Contract.NotNull(sampleTests, nameof(sampleTests));

            var updateDefinition = Builders<Problem>.Update.PushEach(p => p.Description.SampleTests, sampleTests);
            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.UpdateOneAsync(GetKeyFilter(key), updateDefinition));

            return updateResult.MatchedCount == 1;
        }

        /// <summary>
        /// 清空指定题目的所有样例。
        /// </summary>
        /// <param name="key">要清空样例的题目 ID。</param>
        /// <returns>是否成功地清空了指定题目的所有样例。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> ClearProblemSampleTestsAsync(ObjectId key)
        {
            var updateDefinition =
                Builders<Problem>.Update.Set(p => p.Description.SampleTests, new List<ProblemSampleTest>());
            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.UpdateOneAsync(GetKeyFilter(key), updateDefinition));

            return updateResult.MatchedCount == 1;
        }

        /// <summary>
        /// 创建筛选给定的公开题目集 ID 的 <see cref="FilterDefinition{Problem}"/> 对象。
        /// </summary>
        /// <param name="archiveId">要筛选的公开题目集 ID。</param>
        /// <returns>筛选给定的公开题目集 ID 的 <see cref="FilterDefinition{Problem}"/> 对象。</returns>
        private FilterDefinition<Problem> GetArchiveIdFilter(int archiveId)
        {
            return Builders<Problem>.Filter.Eq(p => p.ArchiveId, archiveId);
        }

        /// <summary>
        /// 获取具有给定的公开题目集 ID 的题目的详细信息。
        /// </summary>
        /// <param name="archiveId">公开题目集 ID。</param>
        /// <returns>具有给定的公开题目集 ID 的题目的详细信息。若没有找到这样的题目，返回 null。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<Problem> FindOneArchiveProblemAsync(int archiveId)
        {
            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.Find(GetArchiveIdFilter(archiveId)).FirstOrDefaultAsync());
        }

        /// <summary>
        /// 将给定的题目添加至公开题目集中。
        /// </summary>
        /// <remarks>
        /// 如果给定的公开题目集 ID 与已有的题目 ID 发生冲突，该方法抛出 <see cref="RepositoryException"/> 异常，且异常的
        /// <see cref="RepositoryException.ErrorCode"/> 属性为 <see cref="RepositoryErrorCode.DuplicateKey"/>。
        /// </remarks>
        /// <param name="key">要添加到公开题目集的题目的全局唯一 ID。</param>
        /// <param name="archiveId">公开题目集编号。</param>
        /// <returns>是否成功地更新了给定的题目的公开题目集 ID。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> AddProblemToArchiveAsync(ObjectId key, int archiveId)
        {
            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    return await collection.UpdateOneAsync(
                        GetKeyFilter(key),
                        Builders<Problem>.Update.Set(p => p.ArchiveId, archiveId));
                });
            return updateResult.MatchedCount == 1;
        }

        /// <summary>
        /// 从公开题目集中删除给定的题目。
        /// </summary>
        /// <param name="archiveIds">要删除的题目在公开题目集中的 ID。</param>
        /// <exception cref="ArgumentNullException"><paramref name="archiveIds"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task DeleteProblemsFromArchiveAsync(IEnumerable<int> archiveIds)
        {
            Contract.NotNull(archiveIds, nameof(archiveIds));

            await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    return await collection.UpdateManyAsync(
                        Builders<Problem>.Filter.In(p => p.ArchiveId, archiveIds.Cast<int?>()),
                        Builders<Problem>.Update.Unset(p => p.ArchiveId));
                });
        }

        /// <summary>
        /// 创建包含给定的表达式作为映射表达式的 <see cref="FindOneAndUpdateOptions{Problem, TProjection}"/> 对象。该方法主要
        /// 用于引导编译器推导 <typeparamref name="TProjection"/> 类型参数。
        /// </summary>
        /// <param name="projection">表达式。</param>
        /// <typeparam name="TProjection">映射目标类型。</typeparam>
        /// <returns>
        /// 包含给定的表达式作为映射表达式的 <see cref="FindOneAndUpdateOptions{Problem, TProjection}"/> 对象。
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="projection"/> 为 null。</exception>
        private FindOneAndUpdateOptions<Problem, TProjection> CreateFindOneAndUpdateOptionsWithProjection<TProjection>(
            Expression<Func<Problem, TProjection>> projection)
        {
            Contract.NotNull(projection, nameof(projection));

            return new FindOneAndUpdateOptions<Problem, TProjection>
            {
                Projection = Builders<Problem>.Projection.Expression(projection)
            };
        }

        /// <summary>
        /// 从给定的数据流中上传测试数据包给指定的题目。
        /// </summary>
        /// <param name="problemId">题目的全局唯一 ID。</param>
        /// <param name="stream">包含测试数据包数据的流。</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> UploadProblemTestDataArchive(ObjectId problemId, Stream stream)
        {
            Contract.NotNull(stream, nameof(stream));

            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, context) =>
                {
                    // Upload the data onto GridFS.
                    var archiveId = ObjectId.GenerateNewId();
                    var archiveFileName = archiveId + ".zip";
                    await context.ProblemTestDataArchives.UploadFromStreamAsync(archiveId, archiveFileName, stream);

                    // Update the corresponding problem entity.
                    var oldArchiveIdWrapper = await collection.FindOneAndUpdateAsync(
                        GetKeyFilter(problemId),
                        Builders<Problem>.Update.Set(p => p.JudgeInfo.TestDataArchiveFileId, archiveId),
                        CreateFindOneAndUpdateOptionsWithProjection(p => new { p.JudgeInfo.TestDataArchiveFileId }));
                    if (oldArchiveIdWrapper == null)
                    {
                        // The problem does not exist.
                        await context.ProblemTestDataArchives.DeleteAsync(archiveId);
                        return false;
                    }

                    // Delete the old archive if it exists.
                    var oldArchiveId = oldArchiveIdWrapper.TestDataArchiveFileId;
                    if (oldArchiveId != null)
                    {
                        await context.ProblemTestDataArchives.DeleteAsync(oldArchiveId.Value);
                    }

                    return true;
                });
        }

        /// <summary>
        /// 将给定的题目的测试数据包下载到给定的流中。
        /// </summary>
        /// <param name="problemId">题目 ID。</param>
        /// <param name="output">存放下载的测试数据包的流。</param>
        /// <returns>是否成功地下载了指定的题目的测试数据包。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="output"/> 为 null。</exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> DownloadProblemTestDataArchive(ObjectId problemId, Stream output)
        {
            Contract.NotNull(output, nameof(output));

            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, context) =>
                {
                    var archiveId = await collection.Find(GetKeyFilter(problemId))
                                                    .Project(p => p.JudgeInfo.TestDataArchiveFileId)
                                                    .FirstOrDefaultAsync();
                    if (archiveId == null)
                    {
                        return false;
                    }

                    await context.ProblemTestDataArchives.DownloadToStreamAsync(archiveId.Value, output);
                    return true;
                });
        }

        /// <summary>
        /// 删除给定题目的测试数据包。
        /// </summary>
        /// <param name="problemId">要删除测试数据包的题目的 ID。</param>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task DeleteProblemTestDataArchive(ObjectId problemId)
        {
            await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, context) =>
                {
                    var archiveId = await collection.FindOneAndUpdateAsync(
                        GetKeyFilter(problemId),
                        Builders<Problem>.Update.Set(p => p.JudgeInfo.TestDataArchiveFileId, null),
                        CreateFindOneAndUpdateOptionsWithProjection(p => p.JudgeInfo.TestDataArchiveFileId));

                    if (archiveId != null)
                    {
                        await context.ProblemTestDataArchives.DeleteAsync(archiveId.Value);
                    }
                });
        }
    }
}
