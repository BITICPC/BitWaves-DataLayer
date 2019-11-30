using System;
using BitWaves.Data.Entities;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 为数据抽象层数据集提供 MongoDB 数据上下文。
    /// </summary>
    public sealed class RepositoryDataContext
    {
        private readonly Lazy<IMongoCollection<Content>> _contents;
        private readonly Lazy<IMongoCollection<User>> _users;
        private readonly Lazy<IMongoCollection<Announcement>> _announcements;
        private readonly Lazy<IMongoCollection<Problem>> _problems;
        private readonly Lazy<IGridFSBucket> _problemTestDataArchives;
        private readonly Lazy<IMongoCollection<Submission>> _submissions;
        private readonly Lazy<IMongoCollection<Language>> _languages;

        /// <summary>
        /// 初始化 <see cref="RepositoryDataContext"/> 类的新实例。
        /// </summary>
        /// <param name="database">MongoDB 数据库接口。</param>
        /// <exception cref="ArgumentNullException"><paramref name="database"/> 为 null。</exception>
        public RepositoryDataContext(IMongoDatabase database)
        {
            Contract.NotNull(database, nameof(database));

            Database = database;

            _contents = new Lazy<IMongoCollection<Content>>(
                () => Database.GetCollection<Content>(RepositoryNames.Contents));
            _users = new Lazy<IMongoCollection<User>>(
                () => Database.GetCollection<User>(RepositoryNames.Users));
            _announcements = new Lazy<IMongoCollection<Announcement>>(
                () => Database.GetCollection<Announcement>(RepositoryNames.Announcements));
            _problems = new Lazy<IMongoCollection<Problem>>(
                () => Database.GetCollection<Problem>(RepositoryNames.Problems));
            _problemTestDataArchives = new Lazy<IGridFSBucket>(
                () => new GridFSBucket(
                    Database, new GridFSBucketOptions { BucketName = RepositoryNames.TestDataArchiveBucket }));
            _submissions = new Lazy<IMongoCollection<Submission>>(
                () => Database.GetCollection<Submission>(RepositoryNames.Submissions));
            _languages = new Lazy<IMongoCollection<Language>>(
                () => Database.GetCollection<Language>(RepositoryNames.Languages));
        }

        /// <summary>
        /// 获取 MongoDB 数据库接口。
        /// </summary>
        public IMongoDatabase Database { get; }

        /// <summary>
        /// 获取静态对象数据集的 MongoDB 接口。
        /// </summary>
        public IMongoCollection<Content> Contents => _contents.Value;

        /// <summary>
        /// 获取用户数据集的 MongoDB 接口。
        /// </summary>
        public IMongoCollection<User> Users => _users.Value;

        /// <summary>
        /// 获取全站公告数据集的 MongoDB 接口。
        /// </summary>
        public IMongoCollection<Announcement> Announcements => _announcements.Value;

        /// <summary>
        /// 获取题目数据集的 MongoDB 接口。
        /// </summary>
        public IMongoCollection<Problem> Problems => _problems.Value;

        /// <summary>
        /// 获取包含题目评测数据包的 GridFS bucket。
        /// </summary>
        public IGridFSBucket ProblemTestDataArchives => _problemTestDataArchives.Value;

        /// <summary>
        /// 获取用户提交数据集的 MongoDB 接口。
        /// </summary>
        public IMongoCollection<Submission> Submissions => _submissions.Value;

        /// <summary>
        /// 获取语言数据字典的 MongoDB 接口。
        /// </summary>
        public IMongoCollection<Language> Languages => _languages.Value;
    }
}
