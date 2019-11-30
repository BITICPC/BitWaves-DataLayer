using System;
using Microsoft.Extensions.Logging;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 提供 BitWaves 数据库的初始化逻辑。
    /// </summary>
    public sealed class RepositoryInitializer
    {
        private readonly ILogger<RepositoryInitializer> _logger;
        private readonly Repository _repo;

        /// <summary>
        /// 初始化 <see cref="RepositoryInitializer"/> 的新实例。
        /// </summary>
        /// <param name="repo">要初始化的数据库。</param>
        /// <param name="logger">日志组件。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="repo"/> 为 null。
        /// </exception>
        public RepositoryInitializer(Repository repo, ILogger<RepositoryInitializer> logger = null)
        {
            Contract.NotNull(repo, nameof(repo));
            Contract.NotNull(logger, nameof(logger));

            _logger = logger;
            _repo = repo;
        }

        /// <summary>
        /// 初始化静态内容数据集。
        /// </summary>
        private void InitializeContentCollection()
        {
            _logger?.LogTrace("初始化静态内容数据集...");
            _repo.Contents.Initialize();
        }

        /// <summary>
        /// 初始化用户数据集。
        /// </summary>
        private void InitializeUserCollection()
        {
            _logger?.LogTrace("初始化用户数据集...");
            _repo.Users.Initialize();
        }

        /// <summary>
        /// 初始化全站公告数据集。
        /// </summary>
        private void InitializeAnnouncementCollection()
        {
            _logger?.LogTrace("初始化全站公告数据集...");
            _repo.Announcements.Initialize();
        }

        /// <summary>
        /// 初始化题目数据集。
        /// </summary>
        private void InitializeProblemCollection()
        {
            _logger?.LogTrace("初始化题目数据集...");
            _repo.Problems.Initialize();
        }

        /// <summary>
        /// 初始化用户提交数据集。
        /// </summary>
        private void InitializeSubmissionCollection()
        {
            _logger?.LogTrace("初始化用户提交数据集...");
            _repo.Submissions.Initialize();
        }

        /// <summary>
        /// 初始化语言数据集。
        /// </summary>
        private void InitializeLanguageCollection()
        {
            _logger?.LogTrace("初始化语言数据集...");
            _repo.Languages.Initialize();
        }

        /// <summary>
        /// 初始化数据库。
        /// </summary>
        public void Initialize()
        {
            _logger?.LogTrace("初始化 BitWaves 数据库...");

            InitializeContentCollection();
            InitializeUserCollection();
            InitializeAnnouncementCollection();
            InitializeProblemCollection();
            InitializeSubmissionCollection();
            InitializeLanguageCollection();
        }

        /// <summary>
        /// 向用户数据集添加种子数据。
        /// </summary>
        private void SeedUserCollection()
        {
            _logger?.LogTrace("向用户数据集添加种子数据...");
            _repo.Users.Seed();
        }

        /// <summary>
        /// 向数据库添加种子数据。
        /// </summary>
        public void Seed()
        {
            _logger?.LogTrace("向 BitWaves 数据集中添加种子数据...");

            SeedUserCollection();
        }
    }
}
