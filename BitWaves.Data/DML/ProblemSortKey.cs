using System;
using System.Linq.Expressions;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为题目排序提供排序关键字。
    /// </summary>
    public enum ProblemSortKey
    {
        /// <summary>
        /// 按照 <see cref="Problem.ArchiveId"/> 进行排序。
        /// </summary>
        ArchiveId,

        /// <summary>
        /// 按照 <see cref="Problem.CreationTime"/> 进行排序。
        /// </summary>
        CreationTime,

        /// <summary>
        /// 按照 <see cref="Problem.LastUpdateTime"/> 进行排序。
        /// </summary>
        LastUpdateTime,

        /// <summary>
        /// 按照 <see cref="Problem.Difficulty"/> 进行排序。
        /// </summary>
        Difficulty,

        /// <summary>
        /// 按照 <see cref="Problem.TotalSubmissions"/> 进行排序。
        /// </summary>
        TotalSubmissions,

        /// <summary>
        /// 按照 <see cref="Problem.AcceptedSubmissions"/> 进行排序。
        /// </summary>
        AcceptedSubmissions,

        /// <summary>
        /// 按照 <see cref="Problem.TotalAttemptedUsers"/> 进行排序。
        /// </summary>
        TotalAttemptedUsers,

        /// <summary>
        /// 按照 <see cref="Problem.TotalSolvedUsers"/> 进行排序。
        /// </summary>
        TotalSolvedUsers,

        /// <summary>
        /// 按照 <see cref="Problem.LastSubmissionTime"/> 进行排序。
        /// </summary>
        LastSubmissionTime
    }

    /// <summary>
    /// 为 <see cref="ProblemSortKey"/> 提供扩展方法。
    /// </summary>
    internal static class ProblemSortKeyExtensions
    {
        /// <summary>
        /// 获取与给定的 <see cref="ProblemSortKey"/> 值相对应的排序成员定义。
        /// </summary>
        /// <param name="key">排序键。</param>
        /// <returns>排序成员定义。</returns>
        /// <exception cref="ArgumentException">给定的 <see cref="ProblemSortKey"/> 值无效。</exception>
        public static FieldDefinition<Problem> GetField(this ProblemSortKey key)
        {
            LambdaExpression selector;
            switch (key)
            {
                case ProblemSortKey.ArchiveId:
                    selector = (Expression<Func<Problem, int?>>) (p => p.ArchiveId);
                    break;

                case ProblemSortKey.CreationTime:
                    selector = (Expression<Func<Problem, DateTime>>) (p => p.CreationTime);
                    break;

                case ProblemSortKey.LastUpdateTime:
                    selector = (Expression<Func<Problem, DateTime>>) (p => p.LastUpdateTime);
                    break;

                case ProblemSortKey.Difficulty:
                    selector = (Expression<Func<Problem, int>>) (p => p.Difficulty);
                    break;

                case ProblemSortKey.TotalSubmissions:
                    selector = (Expression<Func<Problem, int>>) (p => p.TotalSubmissions);
                    break;

                case ProblemSortKey.AcceptedSubmissions:
                    selector = (Expression<Func<Problem, int>>) (p => p.AcceptedSubmissions);
                    break;

                case ProblemSortKey.TotalAttemptedUsers:
                    selector = (Expression<Func<Problem, int>>) (p => p.TotalAttemptedUsers);
                    break;

                case ProblemSortKey.TotalSolvedUsers:
                    selector = (Expression<Func<Problem, int>>) (p => p.TotalSolvedUsers);
                    break;

                case ProblemSortKey.LastSubmissionTime:
                    selector = (Expression<Func<Problem, DateTime>>) (p => p.LastSubmissionTime);
                    break;

                default:
                    throw new ArgumentException("Invalid sort key for problems: " + key, nameof(key));
            }

            return new ExpressionFieldDefinition<Problem>(selector);
        }
    }
}
