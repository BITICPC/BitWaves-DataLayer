using MongoDB.Bson;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 为题目提供评测信息。
    /// </summary>
    public sealed class ProblemJudgeInfo
    {
        /// <summary>
        /// 获取或设置题目的评测模式。
        /// </summary>
        public ProblemJudgeMode JudgeMode { get; set; }

        /// <summary>
        /// 当评测模式为 Standard 时，获取或设置内建答案检查器的评测选项。
        /// </summary>
        public BuiltinCheckerOptions? CheckerOptions { get; set; }

        /// <summary>
        /// 获取或设置题目在单个测试点上的时间限制，单位为毫秒。
        /// </summary>
        public int TimeLimit { get; set; }

        /// <summary>
        /// 获取或设置题目在单个测试点上的峰值内存使用限制，单位为 MB。
        /// </summary>
        public int MemoryLimit { get; set; }

        /// <summary>
        /// 获取或设置题目的测试数据包在 GridFS 中的 ID。
        /// </summary>
        public ObjectId? TestDataArchiveFileId { get; set; }
    }
}
