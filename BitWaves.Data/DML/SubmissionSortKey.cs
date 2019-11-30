using System;
using System.Linq.Expressions;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为用户提交定义排序键。
    /// </summary>
    public enum SubmissionSortKey
    {
        /// <summary>
        /// 按照 <see cref="Submission.CreationTime"/> 进行排序。
        /// </summary>
        CreationTime,

        /// <summary>
        /// 按照 <see cref="Submission.Result.Time"/> 进行排序。
        /// </summary>
        Time,

        /// <summary>
        /// 按照 <see cref="Submission.Result.Memory"/> 进行排序。
        /// </summary>
        Memory
    }

    /// <summary>
    /// 为 <see cref="SubmissionSortKey"/> 提供扩展方法。
    /// </summary>
    internal static class SubmissionSortKeyExtensions
    {
        /// <summary>
        /// 获取与给定的 <see cref="SubmissionSortKey"/> 相对应的实体对象成员定义。
        /// </summary>
        /// <param name="key">排序键。</param>
        /// <returns>与给定的 <see cref="SubmissionSortKey"/> 相对应的实体对象成员定义。</returns>
        /// <exception cref="ArgumentException"><paramref name="key"/> 不是一个有效的排序键。</exception>
        public static FieldDefinition<Submission> GetField(this SubmissionSortKey key)
        {
            LambdaExpression selector;
            switch (key)
            {
                case SubmissionSortKey.CreationTime:
                    selector = (Expression<Func<Submission, DateTime>>) (s => s.CreationTime);
                    break;

                case SubmissionSortKey.Time:
                    selector = (Expression<Func<Submission, int>>) (s => s.Result.Time);
                    break;

                case SubmissionSortKey.Memory:
                    selector = (Expression<Func<Submission, int>>) (s => s.Result.Memory);
                    break;

                default:
                    throw new ArgumentException("Invalid submission sort key: " + key, nameof(key));
            }

            return new ExpressionFieldDefinition<Submission>(selector);
        }
    }
}
