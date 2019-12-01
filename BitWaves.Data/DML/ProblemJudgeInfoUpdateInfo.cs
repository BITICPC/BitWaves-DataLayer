using BitWaves.Data.Entities;
using BitWaves.Data.Utils;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为 <see cref="ProblemJudgeInfo"/> 提供数据更新模型。
    /// </summary>
    public sealed class ProblemJudgeInfoUpdateInfo : UpdateInfo<Problem>
    {
        /// <summary>
        /// 获取或设置更新的题目评测模式。
        /// </summary>
        [Set]
        public Maybe<ProblemJudgeMode> JudgeMode { get; set; }

        /// <summary>
        /// 获取或设置更新的内建答案检查器选项。
        /// </summary>
        [Set]
        public Maybe<BuiltinCheckerOptions?> BuiltinCheckerOptions { get; set; }

        /// <summary>
        /// 获取或设置更新的题目时间限制，单位为毫秒。
        /// </summary>
        [Set]
        public Maybe<int> TimeLimit { get; set; }

        /// <summary>
        /// 获取或设置更新的题目内存限制，单位为毫秒。
        /// </summary>
        [Set]
        public Maybe<int> MemoryLimit { get; set; }
    }
}
