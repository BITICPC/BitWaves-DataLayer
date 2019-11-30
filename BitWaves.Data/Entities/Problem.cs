using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示一道题目。
    /// </summary>
    public sealed class Problem
    {
        /// <summary>
        /// 初始化 <see cref="Problem"/> 类的新实例。
        /// </summary>
        private Problem()
        {
        }

        /// <summary>
        /// 初始化 <see cref="Problem"/> 类的新实例。
        /// </summary>
        /// <param name="title">题目标题。</param>
        /// <param name="author">题目作者。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="title"/> 为 null
        ///     或
        ///     <paramref name="author"/> 为 null。
        /// </exception>
        public Problem(string title, string author)
        {
            Contract.NotNull(title, nameof(title));
            Contract.NotNull(author, nameof(author));

            Id = ObjectId.GenerateNewId();
            Title = title;
            Author = author;
            CreationTime = DateTime.UtcNow;
            LastUpdateTime = DateTime.UtcNow;
            Tags = new List<string>();

            Description = new ProblemDescription();
            JudgeInfo = new ProblemJudgeInfo();
        }

        /// <summary>
        /// 获取题目实体对象 ID。
        /// </summary>
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取或设置题目在 Problem Archive 中的 ID。若题目不在 Problem Archive 中，该属性值应为 null。
        /// </summary>
        [BsonIgnoreIfDefault]
        public int? ArchiveId { get; set; }

        /// <summary>
        /// 获取或设置题目的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取题目的作者。
        /// </summary>
        public string Author { get; private set; }

        /// <summary>
        /// 获取或设置题目的来源。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 获取题目的创建时间。
        /// </summary>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 获取题目的上次更改时间。
        /// </summary>
        public DateTime LastUpdateTime { get; private set; }

        /// <summary>
        /// 获取或设置题目的标签列表。
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 获取或设置题目的难度。
        /// </summary>
        public int Difficulty { get; set; }

        /// <summary>
        /// 获取题目的总提交数量。
        /// </summary>
        public int TotalSubmissions { get; private set; }

        /// <summary>
        /// 获取题目的总 AC 提交数量。
        /// </summary>
        public int AcceptedSubmissions { get; private set; }

        /// <summary>
        /// 获取总的尝试解答该题目的用户数量。
        /// </summary>
        public int TotalAttemptedUsers { get; private set; }

        /// <summary>
        /// 获取总的成功解答该题目的用户数量。
        /// </summary>
        public int TotalSolvedUsers { get; private set; }

        /// <summary>
        /// 获取题目的最后一次提交的时间。
        /// </summary>
        public DateTime LastSubmissionTime { get; private set; }

        /// <summary>
        /// 获取或设置题目的描述信息。
        /// </summary>
        public ProblemDescription Description { get; private set; }

        /// <summary>
        /// 获取或设置题目的评测相关信息。
        /// </summary>
        public ProblemJudgeInfo JudgeInfo { get; private set; }
    }
}
