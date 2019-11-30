using BitWaves.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为用户提交的查询提供筛选器建造器。
    /// </summary>
    public sealed class SubmissionFilterBuilder : FilterBuilder<Submission>
    {
        /// <summary>
        /// 筛选给定的作者。
        /// </summary>
        /// <param name="author">提交的作者的用户名。</param>
        public SubmissionFilterBuilder Author(string author)
        {
            Contract.NotNull(author, nameof(author));

            AddFilter(Builders<Submission>.Filter.Eq(s => s.Author, author));
            return this;
        }

        /// <summary>
        /// 筛选给定的题目。
        /// </summary>
        /// <param name="problemId">要筛选的题目的 ID。</param>
        public SubmissionFilterBuilder Problem(ObjectId problemId)
        {
            AddFilter(Builders<Submission>.Filter.Eq(s => s.ProblemId, problemId));
            return this;
        }

        /// <summary>
        /// 筛选给定的语言。
        /// </summary>
        /// <param name="languageId">要筛选的语言的 ID。</param>
        /// <returns></returns>
        public SubmissionFilterBuilder Language(ObjectId languageId)
        {
            AddFilter(Builders<Submission>.Filter.Eq(s => s.LanguageId, languageId));
            return this;
        }

        /// <summary>
        /// 筛选给定的评测状态。
        /// </summary>
        /// <param name="status">要筛选的评测状态。</param>
        public SubmissionFilterBuilder Status(JudgeStatus status)
        {
            AddFilter(Builders<Submission>.Filter.Eq(s => s.Status, status));
            return this;
        }

        /// <summary>
        /// 筛选给定的评测结果。
        /// </summary>
        /// <param name="verdict">要筛选的评测结果。</param>
        public SubmissionFilterBuilder Verdict(Verdict verdict)
        {
            AddFilter(Builders<Submission>.Filter.Eq(s => s.Result.Verdict, verdict));
            return this;
        }

        /// <summary>
        /// 获取空的 <see cref="SubmissionFilterBuilder"/> 对象。
        /// </summary>
        public new static SubmissionFilterBuilder Empty => new SubmissionFilterBuilder();
    }
}
