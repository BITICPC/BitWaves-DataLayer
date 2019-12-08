using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using BitWaves.Data.Utils;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 表示用户实体对象集。
    /// </summary>
    public sealed class UserRepository
        : EntityRepository<User, string, UserUpdateInfo, UserFilterBuilder, UserFindPipeline>
    {
        /// <summary>
        /// 初始化 <see cref="UserRepository"/> 类的新实例。
        /// </summary>
        /// <param name="repository">BitWaves 数据仓库。</param>
        /// <param name="mongoCollection">MongoDB 数据集接口。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="repository"/> 为 null
        ///     或
        ///     <paramref name="mongoCollection"/> 为 null。
        /// </exception>
        internal UserRepository(Repository repository, IMongoCollection<User> mongoCollection)
            : base(repository, mongoCollection)
        {
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            ThrowRepositoryExceptionOnErrorAsync(async (collection, _) =>
            {
                var indexesList = new List<CreateIndexModel<User>>
                {
                    // Username 上的唯一升序索引
                    // 注意：MongoDB 哈希索引不能保证唯一性，因此使用普通索引
                    new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(user => user.Username),
                                               new CreateIndexOptions { Unique = true }),
                    // TotalSubmissions 上的递减索引
                    new CreateIndexModel<User>(Builders<User>.IndexKeys.Descending(user => user.TotalSubmissions)),
                    // TotalAcceptedSubmissions 上的递减索引
                    new CreateIndexModel<User>(
                        Builders<User>.IndexKeys.Descending(user => user.TotalAcceptedSubmissions)),
                    // TotalProblemsAttempted 上的递减索引
                    new CreateIndexModel<User>(
                        Builders<User>.IndexKeys.Descending(user => user.TotalProblemsAttempted)),
                    // TotalProblemsAccepted 上的递减索引
                    new CreateIndexModel<User>(Builders<User>.IndexKeys.Descending(user => user.TotalProblemsAccepted))
                };

                await collection.Indexes.CreateManyAsync(indexesList);
            }).Wait();
        }

        /// <inheritdoc />
        public override void Seed()
        {
            ThrowRepositoryExceptionOnErrorAsync(async (collection, _) =>
            {
                var adminUser = new User("admin", "bitwaves2019") { IsAdmin = true };

                try
                {
                    await collection.InsertOneAsync(adminUser);
                }
                catch (MongoWriteException ex)
                {
                    if (ex.WriteError.Category != ServerErrorCategory.DuplicateKey)
                    {
                        throw;
                    }

                    // 管理员用户已经存在。不执行任何操作。
                }
            }).Wait();
        }

        /// <inheritdoc />
        protected override FilterDefinition<User> GetKeyFilter(string key)
        {
            return Builders<User>.Filter.Eq(u => u.Username, key);
        }

        /// <summary>
        /// 更新给定用户的密码。
        /// </summary>
        /// <param name="username">要更新的用户的用户名。</param>
        /// <param name="password">要更新的用户的新密码。</param>
        /// <returns>是否成功地更新了用户的密码。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="username"/> 为 null
        ///     或
        ///     <paramref name="password"/> 为 null。
        /// </exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> UpdatePasswordAsync(string username, string password)
        {
            Contract.NotNull(username, nameof(username));
            Contract.NotNull(password, nameof(password));

            var passwordHash = PasswordUtils.GetPasswordHash(password);
            var updateResult = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.UpdateOneAsync(
                    GetKeyFilter(username),
                    Builders<User>.Update.Set(u => u.PasswordHash, passwordHash)));

            return updateResult.MatchedCount == 1;
        }

        /// <summary>
        /// 检查给定的用户名和密码是否正确。
        /// </summary>
        /// <param name="username">用户名。</param>
        /// <param name="password">密码。</param>
        /// <returns>给定的用户名和密码是否正确。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="username"/> 为 null
        ///     或
        ///     <paramref name="password"/> 为 null。
        /// </exception>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<bool> ChallengeAsync(string username, string password)
        {
            Contract.NotNull(username, nameof(username));
            Contract.NotNull(password, nameof(password));

            var passwordHash = await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) =>
                {
                    return await collection.Find(GetKeyFilter(username))
                                           .Project(u => u.PasswordHash)
                                           .FirstOrDefaultAsync();
                });

            if (passwordHash == null)
            {
                return false;
            }

            return PasswordUtils.Challenge(passwordHash, password);
        }

        /// <summary>
        /// 在数据集中查询有多少个用户的 AC 题目数量大于指定的阈值。
        /// </summary>
        /// <param name="threshold">AC 题目数量阈值。</param>
        /// <returns>有多少个用户的 AC 题目数量大于指定的阈值。</returns>
        /// <exception cref="RepositoryException">访问底层数据源时出现错误。</exception>
        public async Task<long> CountUsersWithMoreAcceptedProblemsAsync(int threshold)
        {
            return await ThrowRepositoryExceptionOnErrorAsync(
                async (collection, _) => await collection.CountDocumentsAsync(
                    Builders<User>.Filter.Gt(u => u.TotalProblemsAccepted, threshold)));
        }
    }
}
