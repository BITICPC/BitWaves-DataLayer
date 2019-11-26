namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示提交的评测状态。
    /// </summary>
    public enum JudgeStatus : int
    {
        /// <summary>
        /// 用户提交正在队列中等待评测。
        /// </summary>
        Pending,

        /// <summary>
        /// 用户提交正在被评测。
        /// </summary>
        Judging,

        /// <summary>
        /// 用户提交已经评测完毕。
        /// </summary>
        Finished
    }
}
