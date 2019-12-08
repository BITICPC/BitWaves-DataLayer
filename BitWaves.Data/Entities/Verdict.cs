namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示评测结果。
    /// </summary>
    public enum Verdict : int
    {
        /// <summary>
        /// 用户的提交通过了所有的测试点。
        /// </summary>
        Accepted,

        /// <summary>
        /// 用户的提交无法通过编译。
        /// </summary>
        CompilationFailed,

        /// <summary>
        /// 用户的提交在某些测试点上给出了错误的结果。
        /// </summary>
        WrongAnswer,

        /// <summary>
        /// 用户的提交发生了运行时错误。
        /// </summary>
        RuntimeError,

        /// <summary>
        /// 用户的提交超过了时间限制。
        /// </summary>
        TimeLimitExceeded,

        /// <summary>
        /// 用户的提交超过了内存限制。
        /// </summary>
        MemoryLimitExceeded,

        /// <summary>
        /// 用户的提交超过了实际运行时间限制。
        /// </summary>
        IdlenessLimitExceeded,

        /// <summary>
        /// 用户的提交执行了不被允许的系统调用。
        /// </summary>
        BadSystemCall,

        /// <summary>
        /// 提交的题目不包含有效的测试数据。
        /// </summary>
        NoTestData,

        /// <summary>
        /// 自定义答案检查器出错。
        /// </summary>
        CheckerFailed,

        /// <summary>
        /// 自定义交互器出错。
        /// </summary>
        InteractorFailed,

        /// <summary>
        /// 评测系统出错。
        /// </summary>
        JudgeFailed
    }
}
