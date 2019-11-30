using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 表示 BitWaves 的数据仓库。
    /// </summary>
    public sealed class Repository
    {
        private readonly Lazy<ContentRepository> _contents;
        private readonly Lazy<UserRepository> _users;
        private readonly Lazy<AnnouncementRepository> _announcements;
        private readonly Lazy<ProblemRepository> _problems;
        private readonly Lazy<SubmissionRepository> _submissions;
        private readonly Lazy<LanguageRepository> _languages;

        /// <summary>
        /// 初始化 <see cref="Repository"/> 类的新实例。
        /// </summary>
        /// <param name="connectionString">到 MongoDB 实例的连接字符串。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="connectionString"/> 为 null。
        /// </exception>
        public Repository(string connectionString)
        {
            Contract.NotNull(connectionString, nameof(connectionString));

            var database = new MongoClient(connectionString).GetDatabase(RepositoryNames.Repository);
            DataContext = new RepositoryDataContext(database);

            _contents = new Lazy<ContentRepository>(() => new ContentRepository(this, DataContext.Contents));
            _users = new Lazy<UserRepository>(() => new UserRepository(this, DataContext.Users));
            _announcements =
                new Lazy<AnnouncementRepository>(() => new AnnouncementRepository(this, DataContext.Announcements));
            _problems = new Lazy<ProblemRepository>(() => new ProblemRepository(this, DataContext.Problems));
            _submissions =
                new Lazy<SubmissionRepository>(() => new SubmissionRepository(this, DataContext.Submissions));
            _languages = new Lazy<LanguageRepository>(() => new LanguageRepository(this, DataContext.Languages));
        }

        /// <summary>
        /// 获取 BitWaves 数据集的 MongoDB 数据上下文。
        /// </summary>
        internal RepositoryDataContext DataContext { get; }

        /// <summary>
        /// 获取静态内容数据集。
        /// </summary>
        public ContentRepository Contents => _contents.Value;

        /// <summary>
        /// 获取用户数据集。
        /// </summary>
        public UserRepository Users => _users.Value;

        /// <summary>
        /// 获取全站公告数据集。
        /// </summary>
        public AnnouncementRepository Announcements => _announcements.Value;

        /// <summary>
        /// 获取题目数据集。
        /// </summary>
        public ProblemRepository Problems => _problems.Value;

        /// <summary>
        /// 获取提交数据集。
        /// </summary>
        public SubmissionRepository Submissions => _submissions.Value;

        /// <summary>
        /// 获取语言数据集。
        /// </summary>
        public LanguageRepository Languages => _languages.Value;
    }

    namespace DependencyInjection
    {
        /// <summary>
        /// 为 <see cref="Repository"/> 提供依赖注入逻辑。
        /// </summary>
        public static class RepositoryExtensions
        {
            /// <summary>
            /// 将 <see cref="Repository"/> 依赖项添加到指定的服务集合中。
            /// </summary>
            /// <param name="services">服务集合。</param>
            /// <param name="connectionString">连接字符串。</param>
            /// <returns>服务集合。</returns>
            /// <exception cref="ArgumentNullException">
            ///     <paramref name="services"/> 为 null
            ///     或
            ///     <paramref name="connectionString"/> 为 null。
            /// </exception>
            public static IServiceCollection AddBitWavesRepository(this IServiceCollection services,
                                                                   string connectionString)
            {
                Contract.NotNull(services, nameof(services));
                Contract.NotNullOrEmpty(connectionString, "连接字符串不能为空。", nameof(connectionString));

                return services.AddScoped(serviceProvider => new Repository(connectionString));
            }
        }
    }
}
