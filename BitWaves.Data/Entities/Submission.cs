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
        /// <param name="languageId">提交所使用的语言的全局唯一 ID。</param>
        /// <param name="code">提交的源代码。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="author"/> 为 null
        ///     或
        ///     <paramref name="code"/> 为 null。
        /// </exception>
        public Submission(string author, ObjectId problemId, ObjectId languageId, string code)
        {
            Contract.NotNull(author, nameof(author));
            Contract.NotNull(code, nameof(code));

            Id = ObjectId.GenerateNewId();
            Author = author;
            ProblemId = problemId;
            CreationTime = DateTime.UtcNow;
            LanguageId = languageId;
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
        /// 获取或设置提交的语言的全局唯一 ID。
        /// </summary>
        public ObjectId LanguageId { get; private set; }

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
