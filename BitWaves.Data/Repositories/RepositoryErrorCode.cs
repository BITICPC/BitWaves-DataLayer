namespace BitWaves.Data.Repositories
{
    /// <summary>
    /// 为 <see cref="RepositoryException"/> 异常提供错误码。
    /// </summary>
    public enum RepositoryErrorCode
    {
        /// <summary>
        /// 未知错误码。
        /// </summary>
        Unknown,

        /// <summary>
        /// 超时错误。
        /// </summary>
        Timeout,

        /// <summary>
        /// 键冲突错误。
        /// </summary>
        DuplicateKey,
    }
}
