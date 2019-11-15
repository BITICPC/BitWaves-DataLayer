namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示题目的评测模式。
    /// </summary>
    public enum ProblemJudgeMode : byte
    {
        /// <summary>
        /// 标准评测模式。
        /// </summary>
        Standard,

        /// <summary>
        /// 用户提供 Special Judge 的评测模式。
        /// </summary>
        SpecialJudge,

        /// <summary>
        /// 用户提供交互器的评测模式。
        /// </summary>
        Interactive,
    }
}
