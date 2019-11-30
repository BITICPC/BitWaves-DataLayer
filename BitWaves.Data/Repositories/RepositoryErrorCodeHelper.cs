using MongoDB.Driver;

namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 为 <see cref="RepositoryErrorCode"/> 提供帮助类。
    /// </summary>
    internal static class RepositoryErrorCodeHelper
    {
        /// <summary>
        /// 从给定的 <see cref="ServerErrorCategory"/> 枚举值创建相应的 <see cref="RepositoryErrorCode"/> 枚举值。
        /// </summary>
        /// <param name="category"><see cref="ServerErrorCategory"/> 枚举值</param>
        /// <returns>
        /// 从给定的 <see cref="ServerErrorCategory"/> 枚举值创建的 <see cref="RepositoryErrorCode"/> 枚举值。
        /// </returns>
        public static RepositoryErrorCode FromMongoServerErrorCategory(ServerErrorCategory category)
        {
            switch (category)
            {
                case ServerErrorCategory.DuplicateKey:
                    return RepositoryErrorCode.DuplicateKey;

                case ServerErrorCategory.ExecutionTimeout:
                    return RepositoryErrorCode.Timeout;

                default:
                    return RepositoryErrorCode.Unknown;
            }
        }
    }
}
