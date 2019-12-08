using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示一个提交。
    /// </summary>
    public sealed class Submission
    {
        /// <summary>
        /// 初始化 <see cref="Submission"/> 类的新实例。
        /// </summary>
        private Submission()
        {
        }

        /// <summary>
        /// 初始化 <see cref="Submission"/> 类的新实例。
        /// </summary>
        /// <param name="author">创建提交的作者。</param>
        /// <param name="problemId">提交的题目 ID。</param>
        /// <param name="language">提交所使用的语言。</param>
        /// <param name="judgeMode">评测模式。</param>
        /// <param name="timeLimit">时间限制。</param>
        /// <param name="memoryLimit">空间限制。</param>
        /// <param name="code">提交的源代码。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="author"/> 为 null
        ///     或
        ///     <paramref name="language"/> 为 null
        ///     或
        ///     <paramref name="code"/> 为 null。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="timeLimit"/> 小于或等于零
        ///     或
        ///     <paramref name="memoryLimit"/> 小于或等于零。
        /// </exception>
        public Submission(string author, ObjectId problemId, LanguageTriple language,
                          ProblemJudgeMode judgeMode, int timeLimit, int memoryLimit, string code)
        {
            Contract.NotNull(author, nameof(author));
            Contract.NotNull(language, nameof(language));
            Contract.Positive(timeLimit, "Time limit cannot be negative or zero.", nameof(timeLimit));
            Contract.Positive(memoryLimit, "Memory limit cannot be negative or zero.", nameof(memoryLimit));
            Contract.NotNull(code, nameof(code));

            Id = ObjectId.GenerateNewId();
            Author = author;
            ProblemId = problemId;
            CreationTime = DateTime.UtcNow;
            Language = language;
            JudgeMode = judgeMode;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
            Code = code;
        }

        /// <summary>
        /// 获取提交的全局唯一 ID。
        /// </summary>
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取创建提交的用户名称。
        /// </summary>
        public string Author { get; private set; }

        /// <summary>
        /// 获取提交的题目的全局唯一 ID。
        /// </summary>
        public ObjectId ProblemId { get; private set; }

        /// <summary>
        /// 获取提交的创建时间。
        /// </summary>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 获取或设置提交的评测时间。
        /// </summary>
        public DateTime? JudgeTime { get; set; }

        /// <summary>
        /// 获取或设置提交的语言。
        /// </summary>
        public LanguageTriple Language { get; private set; }

        /// <summary>
        /// 获取或设置提交的题目的评测模式。
        /// </summary>
        public ProblemJudgeMode JudgeMode { get; private set; }

        /// <summary>
        /// 获取或设置提交的题目的时间限制，单位为毫秒。
        /// </summary>
        public int TimeLimit { get; private set; }

        /// <summary>
        /// 获取或设置提交的题目的内存限制，单位为 MB。
        /// </summary>
        public int MemoryLimit { get; private set; }

        /// <summary>
        /// 获取提交的源代码。
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 获取或设置提交的评测状态。
        /// </summary>
        public JudgeStatus Status { get; set; }

        /// <summary>
        /// 获取或设置提交的评测结果。
        /// </summary>
        public JudgeResult Result { get; set; }
    }
}
