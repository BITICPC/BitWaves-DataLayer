using System;
using System.Linq.Expressions;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为用户实体对象提供排序键。
    /// </summary>
    public enum UserSortKey
    {
        /// <summary>
        /// 按照 <see cref="User.TotalSubmissions"/> 进行排序。
        /// </summary>
        TotalSubmissions,

        /// <summary>
        /// 按照 <see cref="User.TotalAcceptedSubmissions"/> 进行排序。
        /// </summary>
        TotalAcceptedSubmissions,

        /// <summary>
        /// 按照 <see cref="User.TotalProblemsAttempted"/> 进行排序。
        /// </summary>
        TotalProblemsAttempted,

        /// <summary>
        /// 按照 <see cref="User.TotalProblemsAccepted"/> 进行排序。
        /// </summary>
        TotalProblemsAccepted,

        /// <summary>
        /// 按照 <see cref="User.JoinTime"/> 进行排序。
        /// </summary>
        JoinTime
    }

    /// <summary>
    /// 为 <see cref="UserSortKey"/> 提供扩展方法。
    /// </summary>
    internal static class UserSortKeyExtensions
    {
        /// <summary>
        /// 获取与给定的 <see cref="UserSortKey"/> 值对应的实体对象成员定义。
        /// </summary>
        /// <param name="key"><see cref="UserSortKey"/> 值。</param>
        /// <returns>与给定的 <see cref="UserSortKey"/> 值对应的实体对象成员定义。</returns>
        /// <exception cref="ArgumentException">给定的 <see cref="UserSortKey"/> 值无效。</exception>
        public static FieldDefinition<User> GetField(this UserSortKey key)
        {
            LambdaExpression fieldSelector;
            switch (key)
            {
                case UserSortKey.TotalSubmissions:
                    fieldSelector = (Expression<Func<User, int>>) (u => u.TotalSubmissions);
                    break;

                case UserSortKey.TotalAcceptedSubmissions:
                    fieldSelector = (Expression<Func<User, int>>) (u => u.TotalAcceptedSubmissions);
                    break;

                case UserSortKey.TotalProblemsAttempted:
                    fieldSelector = (Expression<Func<User, int>>) (u => u.TotalProblemsAttempted);
                    break;

                case UserSortKey.TotalProblemsAccepted:
                    fieldSelector = (Expression<Func<User, int>>) (u => u.TotalProblemsAccepted);
                    break;

                case UserSortKey.JoinTime:
                    fieldSelector = (Expression<Func<User, DateTime>>) (u => u.JoinTime);
                    break;

                default:
                    throw new ArgumentException($"Invalid UserSortKey value: {key}");
            }

            return new ExpressionFieldDefinition<User>(fieldSelector);
        }
    }
}
