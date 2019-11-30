using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class ProblemRepository : EntityRepository<Problem, ObjectId, ProblemUpdateInfo, ProblemFindPipeline>
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
    }
}
